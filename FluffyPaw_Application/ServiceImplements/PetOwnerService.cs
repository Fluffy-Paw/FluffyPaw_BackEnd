using AutoMapper;
using FluffyPaw_Application.DTO.Request.BookingRequest;
using FluffyPaw_Application.DTO.Request.PetOwnerRequest;
using FluffyPaw_Application.DTO.Response.BookingResponse;
using FluffyPaw_Application.DTO.Response.FilesResponse;
using FluffyPaw_Application.DTO.Response.PetOwnerResponse;
using FluffyPaw_Application.DTO.Response.StoreManagerResponse;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.CustomException;
using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Enums;
using FluffyPaw_Domain.Interfaces;
using FluffyPaw_Domain.Utils;
using FluffyPaw_Repository.Enum;
using Microsoft.AspNetCore.Http;

namespace FluffyPaw_Application.ServiceImplements
{
    public class PetOwnerService : IPetOwnerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthentication _authentication;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHashing _hashing;
        private readonly IFirebaseConfiguration _firebaseConfiguration;

        public PetOwnerService(IUnitOfWork unitOfWork, IMapper mapper, IAuthentication authentication, IHttpContextAccessor httpContextAccessor, IHashing hashing, IFirebaseConfiguration firebaseConfiguration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authentication = authentication;
            _httpContextAccessor = httpContextAccessor;
            _hashing = hashing;
            _firebaseConfiguration = firebaseConfiguration;
        }

        public async Task<PetOwnerResponse> GetPetOwnerDetail()
        {
            var accountId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var po = _unitOfWork.PetOwnerRepository.Get(u => u.AccountId == accountId && u.Reputation != AccountReputation.Ban.ToString(), includeProperties: "Account").FirstOrDefault();
            if(po == null)
            {
                throw new CustomException.ForbbidenException("Bạn đã bị cấm, liên hệ admin để biết thêm thông tin.");
            }

            var result = _mapper.Map<PetOwnerResponse>(po);
            result.Email = po.Account.Email;
            result.Avatar = po.Account.Avatar;
            result.Username = po.Account.Username;
            return result;
        }

        public async Task<PetOwnerResponse> UpdatePetOwnerAccount(PetOwnerRequest petOwnerRequest)
        {
            var accountId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var exitstingPo = _unitOfWork.PetOwnerRepository.Get(u => u.AccountId == accountId, includeProperties : "Account").FirstOrDefault();
            if (exitstingPo == null)
            {
                throw new CustomException.DataNotFoundException("Bạn không phải Pet Owner.");
            }

            if (petOwnerRequest.Dob > DateTimeOffset.UtcNow) throw new CustomException.InvalidDataException("Ngày sinh không hợp lệ.");

            if (petOwnerRequest.Phone != exitstingPo.Phone)
            {
                if (_unitOfWork.PetOwnerRepository.Get(po => po.Phone == petOwnerRequest.Phone).Any())
                {
                    throw new CustomException.DataExistException("Số điện thoại này đã tồn tại trong hệ thống");
                }
            }

            exitstingPo.Account.Email = petOwnerRequest.Email;
            if(petOwnerRequest.Avatar != null) exitstingPo.Account.Avatar = await _firebaseConfiguration.UploadImage(petOwnerRequest.Avatar);
            var po = _mapper.Map(petOwnerRequest, exitstingPo);
            _unitOfWork.Save();

            var result = _mapper.Map<PetOwnerResponse>(po);
            result.Avatar = po.Account.Avatar;
            result.Email = po.Account.Email;

            return result;
        }

        public async Task<List<StoreResponse>> GetAllStore()
        {
            var stores = _unitOfWork.StoreRepository.Get(s => s.Status == true, includeProperties: "Brand");
            if (!stores.Any())
            {
                throw new CustomException.DataNotFoundException("Thương hiệu chưa đăng kí các chi nhánh cửa hàng.");
            }

            var storeResponses = _mapper.Map<List<StoreResponse>>(stores);
            return storeResponses;
        }

        public async Task<List<StoreResponse>> GetStoreById(long id)
        {
            var stores = _unitOfWork.StoreRepository.Get(s => s.Id == id, includeProperties: "Brand").ToList();
            if (!stores.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy cửa hàng này.");
            }

            var storeResponses = new List<StoreResponse>();

            foreach (var store in stores)
            {
                var storeResponse = _mapper.Map<StoreResponse>(store);

                var storeFiles = _unitOfWork.StoreFileRepository.Get(sf => sf.StoreId == store.Id, includeProperties: "Files")
                                .Select(sf => sf.Files)
                                .ToList();

                storeResponse.Files = _mapper.Map<List<FileResponse>>(storeFiles);

                storeResponses.Add(storeResponse);
            }

            return storeResponses;
        }

        public async Task<List<BookingResponse>> GetAllBookingByPetId(long id, string? bookingStatus)
        {
            var userId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(userId);
            var po = _unitOfWork.PetOwnerRepository.Get(po => po.AccountId == account.Id).First();
            var pet = _unitOfWork.PetRepository.GetByID(id);
            if (pet == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thú cưng.");
            }

            if (pet.PetOwnerId != po.Id)
            {
                throw new CustomException.InvalidDataException("Thú cưng không thuộc quyền quản lý của bạn.");
            }

            var bookings = _unitOfWork.BookingRepository.Get(b => b.PetId == pet.Id
                                            && (string.IsNullOrEmpty(bookingStatus) || b.Status == bookingStatus), 
                                            includeProperties: "StoreService,StoreService.Store,StoreService.Service");
            if (!bookings.Any())
            {
                throw new CustomException.DataNotFoundException("Thú cưng này hiện chưa có lịch nào");
            }

            var bookingResponses = _mapper.Map<List<BookingResponse>>(bookings);
            return bookingResponses;
        }

        public async Task<BookingResponse> CreateBooking(CreateBookingRequest createBookingRequest)
        {
            var userId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(userId);
            var po = _unitOfWork.PetOwnerRepository.Get(po => po.AccountId == account.Id).First();
            var pets = _unitOfWork.PetRepository.Get(p => p.PetOwnerId == po.Id).ToList();
            if (!pets.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thú cưng.");
            }

            if (!pets.Any(p => p.Id == createBookingRequest.PetId))
            {
                throw new CustomException.InvalidDataException("Thú cưng được đặt lịch không thuộc quyền quản lý của bạn.");
            }

            var existingStoreService = _unitOfWork.StoreServiceRepository.Get(
                                ess => ess.Id == createBookingRequest.StoreServiceId,
                                includeProperties: "Service").FirstOrDefault();
            if (existingStoreService == null)
            {
                throw new CustomException.DataNotFoundException("Lịch trình không tồn tại.");
            }

            if (createBookingRequest.PaymentMethod != BookingPaymentMethod.COD.ToString()
                && createBookingRequest.PaymentMethod != BookingPaymentMethod.PayOS.ToString())
            {
                throw new CustomException.InvalidDataException("Phương thức thanh toán không hợp lệ.");
            }

            var newBooking = _mapper.Map<Booking>(createBookingRequest);
            newBooking.Cost = existingStoreService.Service.Cost;
            newBooking.CreateDate = CoreHelper.SystemTimeNow;
            newBooking.StartTime = existingStoreService.StartTime;
            newBooking.EndTime = existingStoreService.StartTime + existingStoreService.Service.Duration;
            newBooking.Checkin = false;
            newBooking.CheckinTime = CoreHelper.SystemTimeNow;
            newBooking.Status = BookingStatus.Pending.ToString();

            _unitOfWork.BookingRepository.Insert(newBooking);
            _unitOfWork.Save();

            /*var newBooking = new Booking
            {
                PetId = createBookingRequest.PetId,
                StoreServiceId = createBookingRequest.StoreServiceId,
                PaymentMethod = createBookingRequest.PaymentMethod,
                Cost = createBookingRequest.Cost,
            };*/

            //Handle xử lý thanh toán

            //

            var bookingResponse = _mapper.Map<BookingResponse>(newBooking);
            return bookingResponse;
        }

        public async Task<bool> CancelBooking(long id)
        {
            var userId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(userId);
            var po = _unitOfWork.PetOwnerRepository.Get(po => po.AccountId == account.Id).First();
            var pets = _unitOfWork.PetRepository.Get(p => p.PetOwnerId == po.Id).ToList();
            if (!pets.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thú cưng.");
            }

            var pendingBooking = _unitOfWork.BookingRepository.Get(pb => pb.Id == id
                                            && pb.Status == BookingStatus.Pending.ToString()).First();
            if (pendingBooking == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy đặt lịch này.");
            }

            if (!pets.Any(p => p.Id == pendingBooking.PetId))
            {
                throw new CustomException.InvalidDataException("Thú cưng trong đặt lịch không thuộc quyền quản lý của bạn.");
            }

            pendingBooking.Status = BookingStatus.Canceled.ToString();
            _unitOfWork.Save();

            //Handle xử lý thanh toán

            //

            return true;
        }
    }
}
