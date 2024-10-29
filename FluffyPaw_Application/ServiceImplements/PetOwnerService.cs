using AutoMapper;
using FluffyPaw_Application.DTO.Request.BookingRequest;
using FluffyPaw_Application.DTO.Request.NotificationRequest;
using FluffyPaw_Application.DTO.Request.PetOwnerRequest;
using FluffyPaw_Application.DTO.Response.BookingResponse;
using FluffyPaw_Application.DTO.Response.FilesResponse;
using FluffyPaw_Application.DTO.Response.PetOwnerResponse;
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

namespace FluffyPaw_Application.ServiceImplements
{
    public class PetOwnerService : IPetOwnerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthentication _authentication;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJobScheduler _jobScheduler;
        private readonly IFirebaseConfiguration _firebaseConfiguration;
        private readonly INotificationService _notificationService;

        public PetOwnerService(IUnitOfWork unitOfWork, IMapper mapper, IAuthentication authentication,
                    IHttpContextAccessor httpContextAccessor, IHashing hashing, 
                    IFirebaseConfiguration firebaseConfiguration, IJobScheduler jobScheduler,
                    INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authentication = authentication;
            _httpContextAccessor = httpContextAccessor;
            _jobScheduler = jobScheduler;
            _firebaseConfiguration = firebaseConfiguration;
            _notificationService = notificationService;
        }

        public async Task<PetOwnerResponse> GetPetOwnerDetail()
        {
            var accountId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var po = _unitOfWork.PetOwnerRepository.Get(u => u.AccountId == accountId && u.Reputation != AccountReputation.Ban.ToString(), includeProperties: "Account").FirstOrDefault();
            if (po == null)
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
            var exitstingPo = _unitOfWork.PetOwnerRepository.Get(u => u.AccountId == accountId, includeProperties: "Account").FirstOrDefault();
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
            if (petOwnerRequest.Avatar != null) exitstingPo.Account.Avatar = await _firebaseConfiguration.UploadImage(petOwnerRequest.Avatar);
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

        public async Task<List<StoreSerResponse>> GetAllStoreServiceByServiceId(long id)
        {
            var existingService = _unitOfWork.ServiceRepository.GetByID(id);
            if (existingService == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy dịch vụ.");
            }

            var storeServices = _unitOfWork.StoreServiceRepository.Get(ss => ss.ServiceId == existingService.Id
                                                    && ss.Status == StoreServiceStatus.Available.ToString());
            if (!storeServices.Any())
            {
                throw new CustomException.DataNotFoundException($"Không tìm thấy lịch trình của dịch vụ {existingService.Name}.");
            }

            var storeSerResponses = _mapper.Map<List<StoreSerResponse>>(storeServices);
            return storeSerResponses;
        }

        public async Task<List<BookingResponse>> GetAllBookingByPetId(long id, string? bookingStatus)
        {
            var userId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(userId);
            var po = _unitOfWork.PetOwnerRepository.Get(po => po.AccountId == account.Id).FirstOrDefault();
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

        public async Task<List<BookingResponse>> CreateBooking(CreateBookingRequest createBookingRequest)
        {
            var userId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(userId);
            var po = _unitOfWork.PetOwnerRepository.Get(po => po.AccountId == account.Id).FirstOrDefault();
            var pets = _unitOfWork.PetRepository.Get(p => p.PetOwnerId == po.Id).ToList();
            if (!pets.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thú cưng hoặc thú cưng không thuộc quyền sở hữu của bạn.");
            }

            var bookings = new List<Booking>();

            foreach (var petId in createBookingRequest.PetId)
            {
                if (!pets.Any(p => p.Id == petId))
                {
                    throw new CustomException.InvalidDataException("Thú cưng được đặt lịch không thuộc quyền quản lý của bạn.");
                }

                var existingStoreService = _unitOfWork.StoreServiceRepository.Get(
                                ess => ess.Id == createBookingRequest.StoreServiceId
                                && ess.Status == StoreServiceStatus.Available.ToString(),
                                includeProperties: "Service,Store,Store.Account").FirstOrDefault();
                if (existingStoreService == null)
                {
                    throw new CustomException.DataNotFoundException("Lịch trình không tồn tại.");
                }

                if (existingStoreService.CurrentPetOwner == existingStoreService.LimitPetOwner)
                {
                    throw new CustomException.InvalidDataException("Lịch trình đã đạt số người đặt tối đa.");
                }

                if (createBookingRequest.PaymentMethod != BookingPaymentMethod.COD.ToString()
                    && createBookingRequest.PaymentMethod != BookingPaymentMethod.PayOS.ToString())
                {
                    throw new CustomException.InvalidDataException("Phương thức thanh toán không hợp lệ.");
                }

                var pet = _unitOfWork.PetRepository.GetByID(petId);

                var newStartTime = existingStoreService.StartTime;
                var newEndTime = existingStoreService.StartTime + existingStoreService.Service.Duration;

                var timeDifference = newStartTime - CoreHelper.SystemTimeNow;
                if (timeDifference < TimeSpan.FromMinutes(30))
                {
                    throw new CustomException.InvalidDataException("Không được đặt lịch trong vòng 30 phút trước khi bắt đầu dịch vụ.");
                }

                if (newStartTime <= CoreHelper.SystemTimeNow)
                {
                    throw new CustomException.InvalidDataException("Thời gian bắt đầu phải là tương lai.");
                }

                var overlappingBooking = _unitOfWork.BookingRepository.Get(b =>
                                                    b.PetId == petId &&
                                                    b.StoreServiceId == createBookingRequest.StoreServiceId &&
                                                    (b.Status == BookingStatus.Pending.ToString()
                                                    || b.Status == BookingStatus.Accepted.ToString()) &&
                                                    b.StartTime < newEndTime &&
                                                    b.EndTime > newStartTime
                                                    );
                if (overlappingBooking.Any())
                {
                    throw new CustomException.InvalidDataException($"Thú cưng {pet.Name} đã có lịch trong khung thời gian này.");
                }

                var newBooking = new Booking
                {
                    PetId = petId,
                    StoreServiceId = createBookingRequest.StoreServiceId,
                    PaymentMethod = createBookingRequest.PaymentMethod,
                    Cost = existingStoreService.Service.Cost,
                    Description = createBookingRequest.Description,
                    CreateDate = CoreHelper.SystemTimeNow,
                    StartTime = existingStoreService.StartTime,
                    EndTime = existingStoreService.StartTime + existingStoreService.Service.Duration,
                    Checkin = false,
                    CheckinTime = existingStoreService.StartTime,
                    CheckOut = false,
                    CheckOutTime = existingStoreService.StartTime + existingStoreService.Service.Duration,
                    Status = BookingStatus.Pending.ToString()
                };

                _unitOfWork.BookingRepository.Insert(newBooking);
                bookings.Add(newBooking);
                existingStoreService.CurrentPetOwner++;
                _unitOfWork.Save();

                _jobScheduler.ScheduleBookingNotificationJob(newBooking);

                var storeAccountId = existingStoreService.Store.Account.Id;
                var notificationRequest = new NotificationRequest
                {
                    ReceiverId = storeAccountId,
                    Name = "Đặt lịch mới",
                    Type = "Booking",
                    Description = $"Đặt lịch mới cho dịch vụ {existingStoreService.Service.Name} cho thú cưng {pet.Name}."
                };
                await _notificationService.CreateNotification(notificationRequest);
            }

            //Handle xử lý thanh toán

            //

            var bookingResponses = _mapper.Map<List<BookingResponse>>(bookings);
            return bookingResponses;
        }

        public async Task<BookingResponse> CreateBookingTimeSelection(TimeSelectionRequest timeSelectionRequest)
        {
            var userId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(userId);
            var po = _unitOfWork.PetOwnerRepository.Get(po => po.AccountId == account.Id).FirstOrDefault();
            var pets = _unitOfWork.PetRepository.Get(p => p.PetOwnerId == po.Id).ToList();
            if (!pets.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thú cưng hoặc thú cưng không thuộc quyền sở hữu của bạn.");
            }

            if (!pets.Any(p => p.Id == timeSelectionRequest.PetId))
            {
                throw new CustomException.InvalidDataException("Thú cưng được đặt lịch không thuộc quyền quản lý của bạn.");
            }

            if (timeSelectionRequest.PaymentMethod != BookingPaymentMethod.COD.ToString()
                    && timeSelectionRequest.PaymentMethod != BookingPaymentMethod.PayOS.ToString())
            {
                throw new CustomException.InvalidDataException("Phương thức thanh toán không hợp lệ.");
            }

            var pet = _unitOfWork.PetRepository.GetByID(timeSelectionRequest.PetId);

            var firstStoreServiceId = timeSelectionRequest.StoreServiceIds.First();
            var lastStoreServiceId = timeSelectionRequest.StoreServiceIds.Last();

            var firstStoreService = _unitOfWork.StoreServiceRepository.Get(
                                    ess => ess.Id == firstStoreServiceId && ess.Status == StoreServiceStatus.Available.ToString(),
                                    includeProperties: "Service,Store,Store.Account").FirstOrDefault();
            var lastStoreService = _unitOfWork.StoreServiceRepository.Get(
                                    ess => ess.Id == lastStoreServiceId && ess.Status == StoreServiceStatus.Available.ToString(),
                                    includeProperties: "Service,Store,Store.Account").FirstOrDefault();

            if (firstStoreService == null || lastStoreService == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy lịch trình hoặc lịch trình không khả dụng.");
            }

            var startDate = firstStoreService.StartTime;
            var newEndTime = firstStoreService.StartTime + firstStoreService.Service.Duration;
            var endDate = lastStoreService.StartTime + lastStoreService.Service.Duration;

            foreach (var storeServiceId in timeSelectionRequest.StoreServiceIds)
            {
                var existingStoreService = _unitOfWork.StoreServiceRepository.Get(
                                ess => ess.Id == storeServiceId
                                && ess.Status == StoreServiceStatus.Available.ToString()
                                && ess.Service.ServiceType.Name == "Hotel",
                                includeProperties: "Service,Store,Store.Account").FirstOrDefault();
                if (existingStoreService == null)
                {
                    throw new CustomException.DataNotFoundException("Lịch trình không tồn tại.");
                }

                if (existingStoreService.CurrentPetOwner == existingStoreService.LimitPetOwner)
                {
                    throw new CustomException.InvalidDataException($"Lịch trình {existingStoreService.StartTime} đã đạt số người đặt tối đa.");
                }

                if (existingStoreService.StartTime <= CoreHelper.SystemTimeNow)
                {
                    throw new CustomException.InvalidDataException("Thời gian bắt đầu phải là tương lai.");
                }

                var overlappingBooking = _unitOfWork.BookingRepository.Get(b =>
                                            b.PetId == timeSelectionRequest.PetId &&
                                            b.StoreServiceId == storeServiceId &&
                                            (b.Status == BookingStatus.Pending.ToString()
                                            || b.Status == BookingStatus.Accepted.ToString()) &&
                                            b.StartTime < newEndTime &&
                                            b.EndTime > existingStoreService.StartTime
                                            );
                if (overlappingBooking.Any())
                {
                    throw new CustomException.InvalidDataException($"Thú cưng {pet.Name} đã có lịch trong khung thời gian này.");
                }

                existingStoreService.CurrentPetOwner += 1;
                _unitOfWork.StoreServiceRepository.Update(existingStoreService);
                await _unitOfWork.SaveAsync();
            }


            var newBooking = new Booking
            {
                PetId = timeSelectionRequest.PetId,
                StoreServiceId = firstStoreServiceId,
                PaymentMethod = timeSelectionRequest.PaymentMethod,
                Cost = firstStoreService.Service.Cost * timeSelectionRequest.StoreServiceIds.Count(),
                Description = timeSelectionRequest.Description,
                CreateDate = CoreHelper.SystemTimeNow,
                StartTime = startDate,
                EndTime = endDate,
                Checkin = false,
                CheckinTime = startDate,
                CheckOut = false,
                CheckOutTime = endDate,
                Status = BookingStatus.Pending.ToString()
            };
            _unitOfWork.BookingRepository.Insert(newBooking);
            await _unitOfWork.SaveAsync();

            await _jobScheduler.ScheduleBookingNotification(newBooking);

            var bookingResponse = _mapper.Map<BookingResponse>(newBooking);
            return bookingResponse;
        }

        public async Task<bool> CancelBooking(long id)
        {
            var userId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(userId);
            var po = _unitOfWork.PetOwnerRepository.Get(po => po.AccountId == account.Id).FirstOrDefault();
            var pets = _unitOfWork.PetRepository.Get(p => p.PetOwnerId == po.Id).ToList();
            if (!pets.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thú cưng.");
            }

            var pendingBooking = _unitOfWork.BookingRepository.Get(pb => pb.Id == id
                                            && pb.Status == BookingStatus.Pending.ToString(),
                                            includeProperties: "Pet").FirstOrDefault();
            if (pendingBooking == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy đặt lịch này.");
            }

            if (!pets.Any(p => p.Id == pendingBooking.PetId))
            {
                throw new CustomException.InvalidDataException("Thú cưng trong đặt lịch không thuộc quyền quản lý của bạn.");
            }

            pendingBooking.Status = BookingStatus.Canceled.ToString();

            var storeService = _unitOfWork.StoreServiceRepository.Get(ss => ss.Id == pendingBooking.StoreServiceId).FirstOrDefault();
            storeService.CurrentPetOwner -= 1;

            _unitOfWork.Save();

            //Handle xử lý thanh toán

            //

            var storeAccountId = storeService.Store.Account.Id;
            var notificationRequest = new NotificationRequest
            {
                ReceiverId = storeAccountId,
                Name = "Huỷ đặt lịch",
                Type = "Cancel Booking",
                Description = $"Hủy đặt lịch cho dịch vụ {storeService.Service.Name} cho thú cưng {pendingBooking.Pet.Name}."
            };
            await _notificationService.CreateNotification(notificationRequest);

            return true;
        }
    }
}
