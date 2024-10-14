using AutoMapper;
using FluffyPaw_Application.DTO.Request.StoreServiceRequest;
using FluffyPaw_Application.DTO.Response.BookingResponse;
using FluffyPaw_Application.DTO.Response.FilesResponse;
using FluffyPaw_Application.DTO.Response.ServiceResponse;
using FluffyPaw_Application.DTO.Response.StoreManagerResponse;
using FluffyPaw_Application.DTO.Response.StoreServiceResponse;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.CustomException;
using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Enums;
using FluffyPaw_Domain.Interfaces;
using FluffyPaw_Domain.Utils;
using FluffyPaw_Repository.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace FluffyPaw_Application.ServiceImplements
{
    public class StaffService : IStaffService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthentication _authentication;
        private readonly IHttpContextAccessor _contextAccessor;

        public StaffService(IUnitOfWork unitOfWork, IMapper mapper, IAuthentication authentication, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authentication = authentication;
            _contextAccessor = contextAccessor;
        }
        public async Task<List<SerResponse>> GetAllServiceByBrandId(long id)
        {
            var staff = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(staff);
            var store = _unitOfWork.StoreRepository.Get(s => s.AccountId == account.Id && s.Status == true).First();
            if (store == null)
            {
                throw new CustomException.DataNotFoundException("Cửa hàng đang bị hạn chế. Hãy thử lại sau.");
            }

            var brand = _unitOfWork.BrandRepository.GetByID(id);
            if (brand == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thương hiệu.");
            }

            else if (store.BrandId != brand.Id)
            {
                throw new CustomException.InvalidDataException("Bạn không có truy cập vào thương hiệu này.");
            }

            var services = _unitOfWork.ServiceRepository.Get(ss => ss.BrandId == id, includeProperties: "ServiceType").ToList();
            if (services == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy dịch vụ của doanh nghiệp");
            }

            var serviceType = _unitOfWork.ServiceTypeRepository.GetByID(services.First().ServiceTypeId);

            var serviceResponses = _mapper.Map<List<SerResponse>>(services);
            foreach (SerResponse serviceResponse in serviceResponses)
            {
                serviceResponse.ServiceTypeName = serviceType.Name;
            }

            return serviceResponses;
        }

        public async Task<StoreResponse> GetStoreByStaff()
        {
            var staff = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(staff);
            if (account == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thông tin của Staff.");
            }

            var store = _unitOfWork.StoreRepository.Get(s => s.AccountId == account.Id && s.Status == true).First();
            if (store == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy cửa hàng.");
            }

            var storeResponse = _mapper.Map<StoreResponse>(store);

            var storeFiles = _unitOfWork.StoreFileRepository.Get(sf => sf.StoreId == store.Id, includeProperties: "Files")
                                .Select(sf => sf.Files)
                                .ToList();

            storeResponse.Files = _mapper.Map<List<FileResponse>>(storeFiles);

            return storeResponse;
        }

        public async Task<List<StoreSerResponse>> CreateStoreService(CreateStoreServiceRequest createStoreServiceRequest)
        {
            var staff = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(staff);
            var store = _unitOfWork.StoreRepository.Get(s => s.AccountId == account.Id && s.Status == true).First();
            if (store == null)
            {
                throw new CustomException.DataNotFoundException("Cửa hàng đang bị hạn chế. Hãy thử lại sau.");
            }

            var existingServices = _unitOfWork.ServiceRepository.Get(ex => ex.BrandId == store.BrandId
                                                        && ex.Id == createStoreServiceRequest.ServiceId
                                                        && ex.Status == true).FirstOrDefault();
            if (existingServices == null)
            {
                throw new CustomException.DataNotFoundException("Dịch vụ này chưa được xác thực hoặc chưa thuộc thương hiệu này.");
            }

            var existingStoreServiceTimes = _unitOfWork.StoreServiceRepository.Get(
                            ss => ss.ServiceId == createStoreServiceRequest.ServiceId)
                            .Select(ss => ss.StartTime)
                            .ToList();

            var storeServices = new List<StoreService>();

            if (createStoreServiceRequest.CreateScheduleRequests == null || !createStoreServiceRequest.CreateScheduleRequests.Any())
            {
                throw new CustomException.InvalidDataException("Danh sách yêu cầu lịch trình trống.");
            }

            foreach (var createScheduleRequest in createStoreServiceRequest.CreateScheduleRequests)
            {
                if (existingStoreServiceTimes.Contains(createScheduleRequest.StartTime))
                {
                    throw new CustomException.InvalidDataException($"Thời gian bắt đầu {createScheduleRequest.StartTime} đã tồn tại.");
                }

                if (createScheduleRequest.StartTime <= CoreHelper.SystemTimeNow)
                {
                    throw new CustomException.InvalidDataException($"Thời gian {createScheduleRequest.StartTime} không phù hợp.");
                }

                if (createScheduleRequest.LimitPetOwner < 1)
                {
                    throw new CustomException.DataNotFoundException("Số lượng tối thiểu là 1");
                } 

                var newStoreService = new StoreService
                {
                    StoreId = store.Id,
                    ServiceId = createStoreServiceRequest.ServiceId,
                    StartTime = createScheduleRequest.StartTime,
                    LimitPetOwner = createScheduleRequest.LimitPetOwner,
                    CurrentPetOwner = 0,
                    Status = StoreServiceStatus.Available.ToString()
                };
                _unitOfWork.StoreServiceRepository.Insert(newStoreService);
                storeServices.Add(newStoreService);
                _unitOfWork.Save();
            }

            await _unitOfWork.SaveAsync();

            var storeServiceResponses = _mapper.Map<List<StoreSerResponse>>(storeServices);
            return storeServiceResponses;
        }

        public async Task<StoreSerResponse> UpdateStoreService(long id, UpdateStoreServiceRequest updateStoreServiceRequest)
        {
            var existingstoreService = _unitOfWork.StoreServiceRepository.GetByID(id);
            if (existingstoreService == null)
            {
                throw new CustomException.DataNotFoundException("Lịch trình không tồn tại.");
            }

            if (updateStoreServiceRequest.StartTime <= System.DateTimeOffset.Now)
            {
                throw new CustomException.InvalidDataException($"Thời gian {updateStoreServiceRequest.StartTime} không phù hợp.");
            }

            var existingBooking = _unitOfWork.BookingRepository.Get(eb => eb.StoreServiceId == id);
            if (existingBooking.Any())
            {
                throw new CustomException.DataExistException("Khung giờ này đã có người đặt.");
            }

            _mapper.Map(existingstoreService, updateStoreServiceRequest);
            _unitOfWork.Save();

            var storeServiceResponses = _mapper.Map<StoreSerResponse>(existingstoreService);
            return storeServiceResponses;
        }

        public async Task<bool> DeleteStoreService(long id)
        {
            var existingstoreService = _unitOfWork.StoreServiceRepository.Get(ess => ess.Id == id, includeProperties: "Store").First();
            if (existingstoreService == null)
            {
                throw new CustomException.DataNotFoundException("Lịch trình không tồn tại.");
            }

            var bookings = _unitOfWork.BookingRepository.Get(b => b.StoreServiceId == existingstoreService.Id
                                        && b.Status == BookingStatus.Pending.ToString()
                                        || b.Status == BookingStatus.Accepted.ToString()
                                        || b.Status == BookingStatus.CheckedIn.ToString()).ToList();
            if (bookings.Any())
            {
                throw new CustomException.DataExistException($"Chi nhánh {existingstoreService.Store.Name} vẫn còn dịch vụ đang được book.");
            }

            _unitOfWork.StoreServiceRepository.Delete(existingstoreService);
            _unitOfWork.Save();

            return true;
        }

        public async Task<List<StoreBookingResponse>> GetAllBookingByStore()
        {
            var staff = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(staff);

            var store = _unitOfWork.StoreRepository.Get(s => s.AccountId == account.Id && s.Status == true).FirstOrDefault();
            if (store == null)
            {
                throw new CustomException.DataNotFoundException("Cửa hàng đang bị hạn chế hoặc không tồn tại.");
            }

            var bookings = _unitOfWork.BookingRepository.Get(
                                    b => b.StoreService.StoreId == store.Id
                                    && b.Status == BookingStatus.Pending.ToString(),
                                    includeProperties: "Pet,Pet.PetOwner,StoreService.Service").ToList();
            if (!bookings.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy đặt lịch nào cho cửa hàng này.");
            }

            var storeBookingResponses = _mapper.Map<List<StoreBookingResponse>>(bookings);
            return storeBookingResponses;
        }

        public async Task<List<BookingResponse>> GetAllBookingByStoreServiceId(long id)
        {
            var staff = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(staff);

            var store = _unitOfWork.StoreRepository.Get(s => s.AccountId == account.Id && s.Status == true).FirstOrDefault();
            if (store == null)
            {
                throw new CustomException.DataNotFoundException("Cửa hàng đang bị hạn chế hoặc không tồn tại.");
            }

            var existingstoreService = _unitOfWork.StoreServiceRepository.Get(
                                                    ss => ss.Id == id
                                                    && ss.StoreId == store.Id
                                                    && ss.Status == StoreServiceStatus.Available.ToString(),
                                                    includeProperties: "Store")
                                                    .FirstOrDefault();
            if (existingstoreService == null)
            {
                throw new CustomException.DataNotFoundException("Lịch trình không tồn tại hoặc không thuộc quyền quản lý của cửa hàng.");
            }

            var existingBooking = _unitOfWork.BookingRepository.Get(b => b.StoreServiceId == id).ToList();
            if (!existingBooking.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy đặt lịch nào cho lịch trình này.");
            }

            var bookingResponses = _mapper.Map<List<BookingResponse>>(existingBooking);
            return bookingResponses;
        }

        public async Task<bool> AcceptBooking(long id)
        {
            var staff = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(staff);
            var store = _unitOfWork.StoreRepository.Get(s => s.AccountId == account.Id && s.Status == true).First();
            if (store == null)
            {
                throw new CustomException.DataNotFoundException("Cửa hàng đang bị hạn chế. Hãy thử lại sau.");
            }

            var pendingBooking = _unitOfWork.BookingRepository.Get(pb => pb.Id == id
                                            && pb.Status == BookingStatus.Pending.ToString()).FirstOrDefault();
            if (pendingBooking == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy đặt lịch này.");
            }

            var storeService = _unitOfWork.StoreServiceRepository.Get(ss => ss.Id == pendingBooking.StoreServiceId
                                                && ss.StoreId == store.Id
                                                && ss.Status == StoreServiceStatus.Available.ToString())
                                                .FirstOrDefault();
            if (storeService == null)
            {
                throw new CustomException.DataNotFoundException("Lịch trình không tồn tại hoặc không thuộc quyền quản lý của cửa hàng.");
            }

            pendingBooking.Status = BookingStatus.Accepted.ToString();
            _unitOfWork.Save();

            //Handle xử lý thanh toán

            //

            return true;
        }

        public async Task<bool> DeniedBooking(long id)
        {
            var staff = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(staff);
            var store = _unitOfWork.StoreRepository.Get(s => s.AccountId == account.Id && s.Status == true).First();
            if (store == null)
            {
                throw new CustomException.DataNotFoundException("Cửa hàng đang bị hạn chế. Hãy thử lại sau.");
            }

            var pendingBooking = _unitOfWork.BookingRepository.Get(pb => pb.Id == id
                                            && pb.Status == BookingStatus.Pending.ToString()).FirstOrDefault();
            if (pendingBooking == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy đặt lịch này.");
            }

            var storeService = _unitOfWork.StoreServiceRepository.Get(ss => ss.Id == pendingBooking.StoreServiceId
                                                && ss.StoreId == store.Id
                                                && ss.Status == StoreServiceStatus.Available.ToString())
                                                .FirstOrDefault();
            if (storeService == null)
            {
                throw new CustomException.DataNotFoundException("Lịch trình không tồn tại hoặc không thuộc quyền quản lý của cửa hàng.");
            }

            pendingBooking.Status = BookingStatus.Denied.ToString();
            _unitOfWork.Save();

            //Handle xử lý thanh toán

            //

            return true;
        }
    }
}
