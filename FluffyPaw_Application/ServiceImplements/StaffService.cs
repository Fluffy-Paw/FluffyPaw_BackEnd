using AutoMapper;
using FluffyPaw_Application.DTO.Request.BookingRequest;
using FluffyPaw_Application.DTO.Request.NotificationRequest;
using FluffyPaw_Application.DTO.Request.StoreServiceRequest;
using FluffyPaw_Application.DTO.Request.TrackingRequest;
using FluffyPaw_Application.DTO.Response.BookingResponse;
using FluffyPaw_Application.DTO.Response.FilesResponse;
using FluffyPaw_Application.DTO.Response.ServiceResponse;
using FluffyPaw_Application.DTO.Response.StaffResponse;
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
        private readonly IJobScheduler _jobScheduler;
        private readonly IFirebaseConfiguration _firebaseConfiguration;
        private readonly INotificationService _notificationService;

        public StaffService(IUnitOfWork unitOfWork, IMapper mapper, IAuthentication authentication,
                            IHttpContextAccessor contextAccessor, IJobScheduler jobScheduler,
                            IFirebaseConfiguration firebaseConfiguration, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authentication = authentication;
            _contextAccessor = contextAccessor;
            _jobScheduler = jobScheduler;
            _firebaseConfiguration = firebaseConfiguration;
            _notificationService = notificationService;
        }
        public async Task<List<SerResponse>> GetAllServiceByBrandId(long id)
        {
            var staff = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(staff);
            var store = _unitOfWork.StoreRepository.Get(s => s.AccountId == account.Id && s.Status == true)
                                            .FirstOrDefault();
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

            var store = _unitOfWork.StoreRepository.Get(s => s.AccountId == account.Id && s.Status == true,
                                            includeProperties: "Brand")
                                                .FirstOrDefault();
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

        public async Task<List<StoreSerResponse>> CreateScheduleStoreService(ScheduleStoreServiceRequest scheduleStoreServiceRequest)
        {
            var staff = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(staff);
            var store = _unitOfWork.StoreRepository.Get(s => s.AccountId == account.Id && s.Status == true)
                                                .FirstOrDefault();
            if (store == null)
            {
                throw new CustomException.DataNotFoundException("Cửa hàng đang bị hạn chế. Hãy thử lại sau.");
            }


            var storeServices = new List<StoreService>();

            var existingStoreServices = _unitOfWork.StoreServiceRepository.Get(s => scheduleStoreServiceRequest.Id.Contains(s.Id)
                                                            && s.StoreId == store.Id
                                                            && s.Service.ServiceType.Name == "Hotel",
                                                            includeProperties: "Service").ToList();
            if (existingStoreServices.Count != scheduleStoreServiceRequest.Id.Count)
            {
                throw new CustomException.DataNotFoundException("Một hoặc nhiều lịch trình cho dịch vụ Hotel không tồn tại" +
                                                                " hoặc không thuộc quyền sở hữu của cửa hàng.");
            }

            for (int i = 0; i < existingStoreServices.Count * scheduleStoreServiceRequest.DuplicateNumber; i++)
            {
                var serviceIndex = i / scheduleStoreServiceRequest.DuplicateNumber; // Xác định StoreService nào
                var duplicateIndex = i % scheduleStoreServiceRequest.DuplicateNumber; // Xác định duplicate nào
                var daysToAdd = (duplicateIndex + 1) * 7;

                var existingStoreService = existingStoreServices[serviceIndex];

                var newStartTime = existingStoreService.StartTime.AddDays(daysToAdd);
                var newEndTime = existingStoreService.StartTime + existingStoreService.Service.Duration;

                var overlappingService = _unitOfWork.StoreServiceRepository.Get(s =>
                                            s.StoreId == existingStoreService.StoreId &&
                                            ((s.StartTime < newEndTime && (s.StartTime + s.Service.Duration) > newStartTime) || 
                                            s.StartTime == newStartTime)).FirstOrDefault(); // Kiểm tra chồng chéo thời gian
                if (overlappingService != null)
                {
                    throw new CustomException.InvalidDataException($"Lịch trình mới trùng với lịch trình hiện có vào ngày {newStartTime}.");
                }

                var newStoreService = new StoreService
                {
                    StoreId = existingStoreService.StoreId,
                    ServiceId = existingStoreService.ServiceId,
                    StartTime = existingStoreService.StartTime.AddDays(daysToAdd),
                    Status = StoreServiceStatus.Available.ToString(),
                    LimitPetOwner = existingStoreService.LimitPetOwner,
                    CurrentPetOwner = 0
                };

                storeServices.Add(newStoreService);
                _unitOfWork.StoreServiceRepository.Insert(newStoreService);

                await _jobScheduler.ScheduleStoreServiceClose(newStoreService);
            }

            await _unitOfWork.SaveAsync();

            var scheduleStoreServiceResponses = _mapper.Map<List<StoreSerResponse>>(storeServices);
            return scheduleStoreServiceResponses;
        }

        public async Task<List<StoreSerResponse>> CreateStoreService(CreateStoreServiceRequest createStoreServiceRequest)
        {
            var staff = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(staff);
            var store = _unitOfWork.StoreRepository.Get(s => s.AccountId == account.Id && s.Status == true)
                                                .FirstOrDefault();
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

            /*var service = _unitOfWork.ServiceRepository.Get(s => s.Id == createStoreServiceRequest.ServiceId,
                                            includeProperties: "ServiceType").FirstOrDefault();
            if (service.ServiceType.Name == "Hotel")
            {
                throw new CustomException.InvalidDataException($"Dịch vụ {service.ServiceType.Name} không phù hợp để tạo lịch trình này.");
            }*/

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
                    StartTime = createScheduleRequest.StartTime.AddHours(7),
                    LimitPetOwner = createScheduleRequest.LimitPetOwner,
                    CurrentPetOwner = 0,
                    Status = StoreServiceStatus.Available.ToString()
                };
                _unitOfWork.StoreServiceRepository.Insert(newStoreService);
                storeServices.Add(newStoreService);
                _unitOfWork.Save();

                await _jobScheduler.ScheduleStoreServiceClose(newStoreService);
            }

            await _unitOfWork.SaveAsync();

            var storeServiceResponses = _mapper.Map<List<StoreSerResponse>>(storeServices);
            return storeServiceResponses;
        }

        /*public async Task<List<StoreSerResponse>> CreateStoreServiceHotel()
        {


            var storeServiceResponses = _mapper.Map<List<StoreSerResponse>>(storeServices);
            return storeServiceResponses;
        }*/

        public async Task<bool> UpdateStoreService(long id, UpdateStoreServiceRequest updateStoreServiceRequest)
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

            _mapper.Map(updateStoreServiceRequest, existingstoreService);
            existingstoreService.StartTime = updateStoreServiceRequest.StartTime.AddHours(7);
            await _unitOfWork.SaveAsync();

            await _jobScheduler.ScheduleStoreServiceClose(existingstoreService);

            return true;
        }

        public async Task<bool> DeleteStoreService(long id)
        {
            var existingstoreService = _unitOfWork.StoreServiceRepository.Get(ess => ess.Id == id, includeProperties: "Store")
                                                            .FirstOrDefault();
            if (existingstoreService == null)
            {
                throw new CustomException.DataNotFoundException("Lịch trình không tồn tại.");
            }

            var bookings = _unitOfWork.BookingRepository.Get(b => b.StoreServiceId == existingstoreService.Id
                                        && b.Status == BookingStatus.Pending.ToString()
                                        || b.Status == BookingStatus.Accepted.ToString());
            if (bookings.Any())
            {
                throw new CustomException.DataExistException($"Chi nhánh {existingstoreService.Store.Name} vẫn còn dịch vụ đang được book.");
            }

            _unitOfWork.StoreServiceRepository.Delete(existingstoreService);
            _unitOfWork.Save();

            return true;
        }

        public async Task<List<StoreBookingResponse>> GetAllBookingByStore(FilterBookingRequest filterBookingRequest)
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
                                    && (string.IsNullOrEmpty(filterBookingRequest.Status) || b.Status == filterBookingRequest.Status),
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
                                                    includeProperties: "Store,Service")
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
            var store = _unitOfWork.StoreRepository.Get(s => s.AccountId == account.Id && s.Status == true)
                                            .FirstOrDefault();
            if (store == null)
            {
                throw new CustomException.DataNotFoundException("Cửa hàng đang bị hạn chế. Hãy thử lại sau.");
            }

            var pendingBooking = _unitOfWork.BookingRepository.Get(pb => pb.Id == id
                                            && pb.Status == BookingStatus.Pending.ToString(),
                                            includeProperties: "Pet,Pet.PetOwner,Pet.PetOwner.Account").FirstOrDefault();
            if (pendingBooking == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy đặt lịch này.");
            }

            var storeService = _unitOfWork.StoreServiceRepository.Get(ss => ss.Id == pendingBooking.StoreServiceId
                                                && ss.StoreId == store.Id
                                                && ss.Status == StoreServiceStatus.Available.ToString(),
                                                includeProperties: "Service")
                                                .FirstOrDefault();
            if (storeService == null)
            {
                throw new CustomException.DataNotFoundException("Lịch trình không tồn tại hoặc không thuộc quyền quản lý của cửa hàng.");
            }

            pendingBooking.Status = BookingStatus.Accepted.ToString();
            _unitOfWork.Save();

            //Handle xử lý thanh toán

            //
            var pet = _unitOfWork.PetRepository.GetByID(pendingBooking.PetId);

            var poAccountId = pet.PetOwner.AccountId;
            var notificationRequest = new NotificationRequest
            {
                ReceiverId = poAccountId,
                Name = "Xác thực yêu cầu đặt lịch",
                Type = "Booking",
                Description = $"Đặt lịch mới cho dịch vụ {storeService.Service.Name} cho thú cưng {pet.Name} thành công."
            };
            await _notificationService.CreateNotification(notificationRequest);

            return true;
        }

        public async Task<bool> DeniedBooking(long id)
        {
            var staff = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(staff);
            var store = _unitOfWork.StoreRepository.Get(s => s.AccountId == account.Id && s.Status == true)
                                            .FirstOrDefault();
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
            storeService.CurrentPetOwner -= 1;

            _unitOfWork.Save();

            //Handle xử lý thanh toán

            //

            var poAccountId = pendingBooking.Pet.PetOwner.Account.Id;
            var notificationRequest = new NotificationRequest
            {
                ReceiverId = poAccountId,
                Name = "Từ chối yêu cầu đặt lịch",
                Type = "Denied Booking",
                Description = $"Đặt lịch mới cho dịch vụ {storeService.Service.Name} cho thú cưng {pendingBooking.Pet.Name} không thành công."
            };
            await _notificationService.CreateNotification(notificationRequest);

            return true;
        }

        public async Task<List<TrackingResponse>> GetAllTrackingByBookingId(long id)
        {
            var staff = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(staff);
            var store = _unitOfWork.StoreRepository.Get(s => s.AccountId == account.Id && s.Status == true)
                                            .FirstOrDefault();
            if (store == null)
            {
                throw new CustomException.DataNotFoundException("Cửa hàng đang bị hạn chế. Hãy thử lại sau.");
            }

            var duringBooking = _unitOfWork.BookingRepository.Get(db => db.Id == id
                                            //&& db.StoreService.Store.Id == store.Id
                                            && db.Status == BookingStatus.Accepted.ToString(),
                                            includeProperties: "StoreService,StoreService.Store").FirstOrDefault();
            if (duringBooking == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy đặt lịch này.");
            }

            var trackings = _unitOfWork.TrackingRepository.Get(t => t.BookingId == duringBooking.Id);
            if (!trackings.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy theo dõi nào.");
            }

            var trackingResponses = new List<TrackingResponse>();

            foreach (var tracking in trackings)
            {
                var trackingResponse = _mapper.Map<TrackingResponse>(tracking);

                var trackingFile = _unitOfWork.TrackingFileRepository.Get(tf => tf.TrackingId == tracking.Id,
                                                    includeProperties: "Tracking,Files")
                                                    .Select(tf => tf.Files)
                                                    .ToList();

                trackingResponse.Files = _mapper.Map<List<FileResponse>>(trackingFile);

                trackingResponses.Add(trackingResponse);
            }

            return trackingResponses;
        }

        public async Task<TrackingResponse> GetTrackingById(long id)
        {
            var tracking = _unitOfWork.TrackingRepository.GetByID(id);
            if (tracking == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy theo dõi.");
            }

            var trackingResponse = _mapper.Map<TrackingResponse>(tracking);

            var trackingFiles = _unitOfWork.TrackingFileRepository.Get(tf => tf.TrackingId == tracking.Id,
                                                    includeProperties: "Files")
                                                    .Select(s => s.Files)
                                                    .ToList();


            trackingResponse.Files = _mapper.Map<List<FileResponse>>(trackingFiles);

            return trackingResponse;
        }

        public async Task<TrackingResponse> CreateTracking(TrackingRequest trackingRequest)
        {
            var existingBooking = _unitOfWork.BookingRepository.Get(eb => eb.Id == trackingRequest.BookingId
                                                    && eb.Status == BookingStatus.Accepted.ToString());
            if (!existingBooking.Any())
            {
                throw new CustomException.DataNotFoundException("Đặt lịch không tìm thấy hoặc đã hết hạn.");
            }

            var newTracking = new Tracking
            {
                BookingId = trackingRequest.BookingId,
                Description = trackingRequest.Description,
                UploadDate = CoreHelper.SystemTimeNow,
                Status = true
            };
            _unitOfWork.TrackingRepository.Insert(newTracking);
            await _unitOfWork.SaveAsync();


            var fileResponses = new List<FileResponse>();

            foreach (var file in trackingRequest.Files)
            {
                var newFile = new Files
                {
                    File = await _firebaseConfiguration.UploadImage(file),
                    CreateDate = CoreHelper.SystemTimeNow,
                    Status = true
                };
                _unitOfWork.FilesRepository.Insert(newFile);
                await _unitOfWork.SaveAsync();

                var newTrackingFile = new TrackingFile
                {
                    TrackingId = newTracking.Id,
                    FileId = newFile.Id
                };
                _unitOfWork.TrackingFileRepository.Insert(newTrackingFile);
                _unitOfWork.Save();

                var fileResponse = _mapper.Map<FileResponse>(newFile);
                fileResponses.Add(fileResponse);
            }

            var trackingResponse = _mapper.Map<TrackingResponse>(newTracking);
            trackingResponse.Files = fileResponses;

            return trackingResponse;
        }

        public async Task<TrackingResponse> UpdateTracking(long id, UpdateTrackingRequest updateTrackingRequest)
        {
            var existingTracking = _unitOfWork.TrackingRepository.GetByID(id);
            if (existingTracking == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy theo dõi này.");
            }

            var fileResponses = new List<FileResponse>();

            foreach (var file in updateTrackingRequest.Files)
            {
                var newFile = new Files
                {
                    File = await _firebaseConfiguration.UploadImage(file),
                    Status = true
                };
                _unitOfWork.FilesRepository.Insert(newFile);
                await _unitOfWork.SaveAsync();

                var newTrackingFile = new TrackingFile
                {
                    TrackingId = existingTracking.Id,
                    FileId = newFile.Id
                };
                _unitOfWork.TrackingFileRepository.Insert(newTrackingFile);
                _unitOfWork.Save();

                var fileResponse = _mapper.Map<FileResponse>(newFile);
                fileResponses.Add(fileResponse);
            }

            var trackingResponse = _mapper.Map<TrackingResponse>(existingTracking);
            trackingResponse.Files = fileResponses;

            return trackingResponse;
        }

        public async Task<bool> DeleteTracking(long id)
        {
            var existingTracking = _unitOfWork.TrackingRepository.GetByID(id);
            if (existingTracking == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy theo dõi này.");
            }

            var trackingFiles = _unitOfWork.TrackingFileRepository.Get(lf => lf.TrackingId == existingTracking.Id,
                                            includeProperties: "Files");
            foreach (var trackingFile in trackingFiles)
            {
                var file = _unitOfWork.FilesRepository.GetByID(trackingFile.FileId);
                _unitOfWork.FilesRepository.Delete(file);
                await _unitOfWork.SaveAsync();
            }

            _unitOfWork.TrackingRepository.Delete(existingTracking);
            _unitOfWork.Save();

            return true;
        }
    }
}
