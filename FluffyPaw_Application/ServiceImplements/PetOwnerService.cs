using AutoMapper;
using FluffyPaw_Application.DTO.Request.BookingRequest;
using FluffyPaw_Application.DTO.Request.NotificationRequest;
using FluffyPaw_Application.DTO.Request.PetOwnerRequest;
using FluffyPaw_Application.DTO.Response;
using FluffyPaw_Application.DTO.Response.BookingResponse;
using FluffyPaw_Application.DTO.Response.BrandResponse;
using FluffyPaw_Application.DTO.Response.CertificateResponse;
using FluffyPaw_Application.DTO.Response.DasboardResponse;
using FluffyPaw_Application.DTO.Response.FilesResponse;
using FluffyPaw_Application.DTO.Response.PetOwnerResponse;
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
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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
        private readonly IHashing _hashing;

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
            _hashing = hashing;
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

            exitstingPo.Account.Email = petOwnerRequest.Email;
            if (petOwnerRequest.Avatar != null) exitstingPo.Account.Avatar = await _firebaseConfiguration.UploadImage(petOwnerRequest.Avatar);
            var po = _mapper.Map(petOwnerRequest, exitstingPo);
            _unitOfWork.Save();

            var result = _mapper.Map<PetOwnerResponse>(po);
            result.Avatar = po.Account.Avatar;
            result.Email = po.Account.Email;

            return result;
        }

        public async Task<BrResponse> GetBrandById(long id)
        {
            var brand = _unitOfWork.BrandRepository.Get(b => b.Id == id && b.Status == true).FirstOrDefault();
            if (brand == null)
            {
                throw new CustomException.DataNotFoundException("Thương hiệu này không tồn tại.");
            }

            var brandResponse = _mapper.Map<BrResponse>(brand);
            return brandResponse;
        }

        public async Task<List<StoreResponse>> GetAllStore()
        {
            var stores = _unitOfWork.StoreRepository.Get(s => s.Status == true, includeProperties: "Brand").ToList();

            if (!stores.Any())
            {
                throw new CustomException.DataNotFoundException("Thương hiệu chưa đăng kí các chi nhánh cửa hàng.");
            }

            var storeServiceIds = _unitOfWork.StoreServiceRepository.Get(ss => stores.Select(s => s.Id).Contains(ss.StoreId))
                                       .Select(ss => new { ss.Id, ss.StoreId }).ToList();

            var bookingIds = _unitOfWork.BookingRepository.Get(b => storeServiceIds.Select(ss => ss.Id).Contains(b.StoreServiceId))
                                    .Select(b => new { b.Id, b.StoreServiceId }).ToList();

            var bookingRatings = _unitOfWork.BookingRatingRepository.Get(br => bookingIds.Select(b => b.Id)
                                .Contains(br.BookingId) && br.StoreVote >= 4)
                                 .GroupBy(br => br.Booking.StoreServiceId)
                                 .ToDictionary(g => g.Key, g => g.Count());

            var storeResponses = new List<StoreResponse>();

            foreach (var store in stores)
            {
                int validVotesCount = 0;

                var storeServiceIdsForStore = storeServiceIds.Where(ss => ss.StoreId == store.Id).Select(ss => ss.Id);

                foreach (var storeServiceId in storeServiceIdsForStore)
                {
                    if (bookingRatings.ContainsKey(storeServiceId))
                    {
                        validVotesCount += bookingRatings[storeServiceId];
                    }
                }

                var storeResponse = _mapper.Map<StoreResponse>(store);

                storeResponses.Add(storeResponse);
            }

            return storeResponses;
        }

        public async Task<List<StoreResponse>> GetAllStoreByBrandId(long id)
        {
            var brand = _unitOfWork.BrandRepository.GetByID(id);
            if (brand == null || brand.Status == false)
            {
                throw new CustomException.DataNotFoundException("Thương hiệu này không tồn tại trong hệ thống.");
            }

            var stores = _unitOfWork.StoreRepository.Get(s => s.BrandId == brand.Id && s.Status == true, includeProperties: "Brand");
            if (!stores.Any())
            {
                throw new CustomException.DataNotFoundException("Thương hiệu chưa đăng kí các chi nhánh cửa hàng.");
            }

            var storeResponses = _mapper.Map<List<StoreResponse>>(stores);
            return storeResponses;
        }

        public async Task<List<StoreResponse>> GetAllStoreByServiceTypeId(long id)
        {
            var serviceType = _unitOfWork.ServiceTypeRepository.GetByID(id);
            if (serviceType == null)
            {
                throw new CustomException.DataNotFoundException("Không tồn tại loại hình dịch vụ này.");
            }
            var serviceIds = _unitOfWork.ServiceRepository.Get(s => s.ServiceTypeId == id
                                                && s.Status == true,
                                                includeProperties: "ServiceType")
                                                .Select(s => s.Id)
                                                .ToList();
            if (!serviceIds.Any())
            {
                throw new CustomException.DataNotFoundException($"Loại hình dịch vụ {serviceType.Name} hiện không có bất kì dịch vụ nào khả dụng.");
            }

            var storeResponses = new List<StoreResponse>();

            var storeServices = _unitOfWork.StoreServiceRepository.Get(ss => serviceIds.Contains(ss.ServiceId),
                                                includeProperties: "Store,Store.Account,Store.Brand")
                                                .ToList();
            if (!storeServices.Any())
            {
                throw new CustomException.DataNotFoundException($"Không tìm thấy cửa hàng nào có loại hình dịch vụ {serviceType.Name}");
            }

            var stores = storeServices
                            .Select(ss => ss.Store)
                            .OrderByDescending(ss => ss.TotalRating)
                            .Distinct();

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

        public async Task<StoreSerResponse> GetStoreServiceById(long id)
        {
            var storeService = _unitOfWork.StoreServiceRepository.GetByID(id);
            if (storeService == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy khung giờ này");
            }

            var storeServiceResponse = _mapper.Map<StoreSerResponse>(storeService);
            return storeServiceResponse;
        }

        public async Task<List<StoreSerResponse>> SuggestionSameTimeAndBrand(long id)
        {
            var storeServiceFull = _unitOfWork.StoreServiceRepository.Get(bst => bst.Id == id,
                                                        includeProperties: "Store,Service,Service.ServiceType").FirstOrDefault();

            var storeServices = _unitOfWork.StoreServiceRepository.Get(ss => ss.StartTime == storeServiceFull.StartTime
                                                    && ss.Service.ServiceType.Name == storeServiceFull.Service.ServiceType.Name
                                                    && ss.Store.BrandId == storeServiceFull.Store.BrandId
                                                    && ss.Status == StoreServiceStatus.Available.ToString()).ToList();
            storeServices.Remove(storeServiceFull);

            var storeSerResponses = _mapper.Map<List<StoreSerResponse>>(storeServices);
            return storeSerResponses;
        }

        public async Task<List<StoreSerResponse>> SuggestionSameTime(long id)
        {
            var storeServiceFull = _unitOfWork.StoreServiceRepository.Get(bst => bst.Id == id,
                                                        includeProperties: "Store,Service,Service.ServiceType").FirstOrDefault();

            var storeServices = _unitOfWork.StoreServiceRepository.Get(ss => ss.StartTime == storeServiceFull.StartTime
                                                    && ss.Service.ServiceType.Name == storeServiceFull.Service.ServiceType.Name
                                                    && ss.Store.BrandId != storeServiceFull.Store.BrandId
                                                    && ss.Status == StoreServiceStatus.Available.ToString()).ToList();
            storeServices.Remove(storeServiceFull);

            var storeSerResponses = _mapper.Map<List<StoreSerResponse>>(storeServices);
            return storeSerResponses;
        }

        public async Task<StResponse> GetStoreById(long id)
        {
            var store = _unitOfWork.StoreRepository.Get(s => s.Id == id, includeProperties: "Brand").FirstOrDefault();
            if (store == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy cửa hàng này.");
            }

            var storeResponse = _mapper.Map<StResponse>(store);

            var storeFiles = _unitOfWork.StoreFileRepository.Get(sf => sf.StoreId == store.Id).ToList();

            var files = new List<FileResponse>();

            foreach (var storeFile in storeFiles)
            {
                var file = _unitOfWork.FilesRepository.GetByID(storeFile.FileId);
                if (file != null)
                {
                    var fileResponse = _mapper.Map<FileResponse>(file);
                    files.Add(fileResponse);
                }
            }

            storeResponse.Files = files;
            storeResponse.BrandName = store.Brand.Name;

            return storeResponse;
        }


        public async Task<List<StoreSerResponse>> GetAllStoreServiceByServiceIdStoreId(long serviceId, long storeId)
        {
            var service = _unitOfWork.ServiceRepository.GetByID(serviceId);

            var storeServices = _unitOfWork.StoreServiceRepository.Get(ss => ss.ServiceId == serviceId
                                                    && ss.StoreId == storeId
                                                    && ss.Status == StoreServiceStatus.Available.ToString(),
                                                    includeProperties: "Service,Service.Brand");
            if (!storeServices.Any())
            {
                throw new CustomException.DataNotFoundException($"Không tìm thấy lịch trình nào.");
            }

            var storeSerResponses = _mapper.Map<List<StoreSerResponse>>(storeServices);
            return storeSerResponses;
        }

        public async Task<List<SerResponse>> GetAllServiceByServiceTypeIdDateTime(long serviceTypeId, DateTimeOffset? startTime, DateTimeOffset? endTime)
        {
            var services = _unitOfWork.ServiceRepository.Get(s => s.Status == true, includeProperties: "ServiceType,Brand,StoreServices")
                                            .Where(ss => ss.ServiceTypeId == serviceTypeId);

            if (startTime.HasValue && endTime.HasValue)
            {
                DateTimeOffset localStartTime = startTime.Value.ToLocalTime();
                DateTimeOffset localEndTime = endTime.Value.ToLocalTime();

                services = services.Where(s =>
                    s.StoreServices != null && s.StoreServices.Any(ss =>
                        ss.StartTime >= localStartTime && ss.StartTime <= localEndTime
                    ));

                if (!services.Any())
                {
                    throw new CustomException.DataNotFoundException($"Không tìm thấy bất kì dịch vụ nào " +
                               $"trong khung giờ từ {startTime.Value.ToString("HH:mm:ss")} đến {endTime.Value.ToString("HH:mm:ss")} này.");
                }

            }

            var serviceList = services
                .OrderByDescending(s => s.TotalRating)
                .ThenByDescending(s => s.BookingCount)
                .ThenBy(s => s.Cost)
                .ToList();

            var serviceResponses = new List<SerResponse>();

            foreach (var service in serviceList)
            {
                var serResponse = _mapper.Map<SerResponse>(service);

                var certificates = _unitOfWork.CertificateRepository.Get(c => c.ServiceId == service.Id).ToList();

                serResponse.Certificate = certificates?
                    .Select(certificate => _mapper.Map<CertificatesResponse>(certificate))
                    .ToList();

                serviceResponses.Add(serResponse);
            }

            return serviceResponses;
        }


        public async Task<List<StResponse>> GetAllStoreByServiceIdDateTime(long serviceId, DateTimeOffset? startTime, DateTimeOffset? endTime)
        {
            var storeServices = _unitOfWork.StoreServiceRepository.Get(ss => ss.ServiceId == serviceId,
                                                    includeProperties: "Store");

            if (startTime.HasValue && endTime.HasValue)
            {
                DateTimeOffset localStartTime = startTime.Value.ToLocalTime();
                DateTimeOffset localEndTime = endTime.Value.ToLocalTime();

                // Lọc storeServices dựa trên khoảng thời gian từ startTime đến endTime
                storeServices = storeServices.Where(ss =>
                    ss.StartTime >= localStartTime && ss.StartTime <= localEndTime
                );

                if (!storeServices.Any())
                {
                    throw new CustomException.DataNotFoundException($"Không tìm thấy cửa hàng nào có lịch trình" +
                        $" trong khoảng thời gian từ {startTime.Value.ToString("HH:mm:ss")} đến {endTime.Value.ToString("HH:mm:ss")}.");
                }
            }

            var stores = storeServices.Select(ss => ss.Store)
                                    .Distinct()
                                    .OrderByDescending(s => s.TotalRating)
                                    .ToList();

            foreach (var store in stores)
            {
                var storeFiles = _unitOfWork.StoreFileRepository.Get(sf => sf.StoreId == store.Id)
                    .Select(sf => sf.Files)
                    .ToList();

                store.Files = storeFiles.ToList();
            }

            var stResponses = _mapper.Map<List<StResponse>>(stores);

            return stResponses;
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

        public async Task<List<BookingResponse>> GetAllBooking()
        {
            var userId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(userId);
            var po = _unitOfWork.PetOwnerRepository.Get(po => po.AccountId == account.Id).FirstOrDefault();
            var pets = _unitOfWork.PetRepository.Get(p => p.PetOwnerId == po.Id && p.Status == PetStatus.Available.ToString());

            var bookingResponses = new List<BookingResponse>();
            foreach (var pet in pets)
            {
                var bookings = _unitOfWork.BookingRepository.Get(b => b.PetId == pet.Id,
                                                orderBy: q => q.OrderByDescending(b => b.CreateDate),
                                                includeProperties: "StoreService,StoreService.Store,StoreService.Service,Pet").ToList();
                if (!bookings.Any())
                {
                    throw new CustomException.DataNotFoundException("Không tìm thấy đặt lịch nào.");
                }

                var mappedBookings = _mapper.Map<List<BookingResponse>>(bookings);
                bookingResponses.AddRange(mappedBookings);
            }

            return bookingResponses;
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
                                            orderBy: q => q.OrderByDescending(b => b.CreateDate),
                                            includeProperties: "StoreService,StoreService.Store,StoreService.Service,Pet");
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
            var pets = _unitOfWork.PetRepository.Get(p => p.PetOwnerId == po.Id && p.Status == PetStatus.Available.ToString()).ToList();
            if (!pets.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thú cưng hoặc thú cưng không thuộc quyền sở hữu của bạn.");
            }

            var bookingResponses = new List<BookingResponse>();

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
                    && createBookingRequest.PaymentMethod != BookingPaymentMethod.FluffyPay.ToString())
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
                                        (b.Status == BookingStatus.Pending.ToString() || b.Status == BookingStatus.Accepted.ToString()) &&
                                        b.StartTime < newEndTime &&
                                        b.EndTime > newStartTime);
                if (overlappingBooking.Any())
                {
                    throw new CustomException.InvalidDataException($"Thú cưng {pet.Name} đã có lịch trong khung thời gian này.");
                }

                var existingBookingCodes = _unitOfWork.BookingRepository.Get(b => true).Select(b => b.Code);

                string code = "";
                bool status = true;

                while (status)
                {
                    code = _hashing.GenerateCode();

                    if (!existingBookingCodes.Contains(code))
                    {
                        status = false;
                    }
                }

                var newBooking = new Booking
                {
                    Code = code,
                    PetId = petId,
                    StoreServiceId = createBookingRequest.StoreServiceId,
                    PaymentMethod = createBookingRequest.PaymentMethod,
                    Cost = existingStoreService.Service.Cost,
                    Description = createBookingRequest.Description,
                    CreateDate = CoreHelper.SystemTimeNow.AddHours(7),
                    StartTime = existingStoreService.StartTime,
                    EndTime = existingStoreService.StartTime + existingStoreService.Service.Duration,
                    Checkin = false,
                    CheckinTime = null,
                    CheckOut = false,
                    CheckOutTime = null,
                    Status = BookingStatus.Pending.ToString()
                };


                _unitOfWork.BookingRepository.Insert(newBooking);
                await _unitOfWork.SaveAsync();


                if (createBookingRequest.PaymentMethod == BookingPaymentMethod.FluffyPay.ToString())
                {
                    var wallet = _unitOfWork.WalletRepository.Get(w => w.AccountId == account.Id).FirstOrDefault();
                    if (wallet == null || wallet.Balance < newBooking.Cost)
                    {
                        throw new CustomException.InvalidDataException($"Số dư ví không đủ để thực hiện đặt lịch cho thú cưng {pet.Name}.");
                    }
                    wallet.Balance -= newBooking.Cost;

                    var billingRecord = new BillingRecord
                    {
                        WalletId = wallet.Id,
                        BookingId = newBooking.Id,
                        Amount = newBooking.Cost,
                        Description = $"Đặt lịch dịch vụ {existingStoreService.Service.Name}.",
                        CreateDate = CoreHelper.SystemTimeNow.AddHours(7)
                    };

                    _unitOfWork.BillingRecordRepository.Insert(billingRecord);
                }

                existingStoreService.CurrentPetOwner++;
                _unitOfWork.StoreServiceRepository.Update(existingStoreService);
                _unitOfWork.Save();


                var bookingResponse = _mapper.Map<BookingResponse>(newBooking);
                bookingResponse.CreateDate = newBooking.CreateDate.AddHours(-7);
                bookingResponses.Add(bookingResponse);


                await _jobScheduler.ScheduleBookingNotification(newBooking);
                await _jobScheduler.ScheduleOverTimeRefund(newBooking);

                var storeAccountId = existingStoreService.Store.Account.Id;
                var notificationRequest = new NotificationRequest
                {
                    ReceiverId = storeAccountId,
                    Name = "Đặt lịch mới",
                    Type = NotificationType.Booking.ToString(),
                    Description = $"Đặt lịch mới cho dịch vụ {existingStoreService.Service.Name} cho thú cưng {pet.Name}.",
                    ReferenceId = newBooking.Id
                };
                await _notificationService.CreateNotification(notificationRequest);
            }

            return bookingResponses;
        }

        public async Task<BookingResponse> CreateBookingTimeSelection(TimeSelectionRequest timeSelectionRequest)
        {
            var userId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(userId);
            var po = _unitOfWork.PetOwnerRepository.Get(po => po.AccountId == account.Id).FirstOrDefault();
            var pets = _unitOfWork.PetRepository.Get(p => p.PetOwnerId == po.Id && p.Status == PetStatus.Available.ToString()).ToList();
            if (!pets.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thú cưng hoặc thú cưng không thuộc quyền sở hữu của bạn.");
            }

            if (!pets.Any(p => p.Id == timeSelectionRequest.PetId))
            {
                throw new CustomException.InvalidDataException("Thú cưng được đặt lịch không thuộc quyền quản lý của bạn.");
            }

            if (timeSelectionRequest.PaymentMethod != BookingPaymentMethod.COD.ToString()
                    && timeSelectionRequest.PaymentMethod != BookingPaymentMethod.FluffyPay.ToString())
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

            var store = _unitOfWork.StoreRepository.Get(s => s.Id == firstStoreService.StoreId, includeProperties: "Account").FirstOrDefault();
            var service = _unitOfWork.ServiceRepository.Get(s => s.Id == firstStoreService.ServiceId, includeProperties: "ServiceType").FirstOrDefault();

            var startDate = firstStoreService.StartTime;
            var newEndTime = firstStoreService.StartTime + firstStoreService.Service.Duration;
            var endDate = lastStoreService.StartTime + lastStoreService.Service.Duration;

            foreach (var storeServiceId in timeSelectionRequest.StoreServiceIds)
            {
                var existingStoreService = _unitOfWork.StoreServiceRepository.Get(
                                ess => ess.Id == storeServiceId
                                && ess.Status == StoreServiceStatus.Available.ToString()
                                && ess.Service.ServiceType.Name == "Khách sạn",
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

            var existingBookingCodes = _unitOfWork.BookingRepository.Get(b => true).Select(b => b.Code);

            string code = "";
            bool status = true;

            while (status)
            {
                code = _hashing.GenerateCode();

                if (!existingBookingCodes.Contains(code))
                {
                    status = false;
                }
            }

            var newBooking = new Booking
            {
                Code = code,
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
                CheckOutTime = endDate.AddHours(-1),
                Status = BookingStatus.Pending.ToString()
            };

            _unitOfWork.BookingRepository.Insert(newBooking);
            await _unitOfWork.SaveAsync();

            if (timeSelectionRequest.PaymentMethod == BookingPaymentMethod.FluffyPay.ToString())
            {
                var wallet = _unitOfWork.WalletRepository.Get(w => w.AccountId == account.Id).FirstOrDefault();
                if (wallet == null || wallet.Balance < newBooking.Cost)
                {
                    throw new CustomException.InvalidDataException($"Số dư ví không đủ để thực hiện đặt lịch cho thú cưng {pet.Name}.");
                }
                wallet.Balance -= newBooking.Cost;

                var billingRecord = new BillingRecord
                {
                    WalletId = wallet.Id,
                    BookingId = newBooking.Id,
                    Amount = newBooking.Cost,
                    Description = $"Đặt lịch dịch vụ {service.Name} từ {firstStoreService.StartTime.ToString("HH:mm:ss dd/MM/yyyy")} đến" +
                                    $"{lastStoreService.StartTime.ToString("HH:mm:ss dd/MM/yyyy")}.",
                    CreateDate = CoreHelper.SystemTimeNow.AddHours(7)
                };

                _unitOfWork.BillingRecordRepository.Insert(billingRecord);
            }

            await _unitOfWork.SaveAsync();

            await _jobScheduler.ScheduleOverTimeRefund(newBooking);
            await _jobScheduler.ScheduleBookingNotification(newBooking);

            var notificationRequest = new NotificationRequest
            {
                ReceiverId = store.AccountId,
                Name = "Đặt lịch mới",
                Type = NotificationType.Booking.ToString(),
                Description = $"Đặt lịch mới cho dịch vụ {service.Name} cho thú cưng {pet.Name}.",
                ReferenceId = newBooking.Id
            };
            await _notificationService.CreateNotification(notificationRequest);

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

            var storeService = _unitOfWork.StoreServiceRepository.Get(ss => ss.Id == pendingBooking.StoreServiceId,
                                                    includeProperties: "Store,Service").FirstOrDefault();
            storeService.CurrentPetOwner -= 1;


            //Đợi thêm bussiness rule cho vde Cancel
            var currentTime = CoreHelper.SystemTimeNow;
            var timeDifference = pendingBooking.StartTime - currentTime;

            if (timeDifference > TimeSpan.FromDays(1))
            {
                var wallet = _unitOfWork.WalletRepository.Get(w => w.AccountId == userId).FirstOrDefault();
                wallet.Balance += pendingBooking.Cost;
                _unitOfWork.WalletRepository.Update(wallet);

                var billingRecord = new BillingRecord
                {
                    WalletId = wallet.Id,
                    BookingId = pendingBooking.Id,
                    Amount = pendingBooking.Cost,
                    Description = $"Huỷ Đặt lịch dịch vụ {storeService.Service.Name}.",
                    CreateDate = CoreHelper.SystemTimeNow.AddHours(7)
                };

                _unitOfWork.BillingRecordRepository.Insert(billingRecord);
            }

            await _unitOfWork.SaveAsync();

            var storeAccountId = storeService.Store.AccountId;
            var notificationRequest = new NotificationRequest
            {
                ReceiverId = storeAccountId,
                Name = "Huỷ đặt lịch",
                Type = NotificationType.Booking.ToString(),
                Description = $"Hủy đặt lịch cho dịch vụ {storeService.Service.Name} cho thú cưng {pendingBooking.Pet.Name}.",
                ReferenceId = pendingBooking.Id
            };
            await _notificationService.CreateNotification(notificationRequest);

            return true;
        }

        public async Task<List<BillingRecordResponse>> GetAllBillingRecord()
        {
            var user = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(user);
            var wallet = _unitOfWork.WalletRepository.Get(w => w.AccountId == account.Id).FirstOrDefault();

            var billingRecords = _unitOfWork.BillingRecordRepository.Get(brs => brs.WalletId == wallet.Id,
                                                    orderBy: q => q.OrderByDescending(br => br.CreateDate),
                                                    includeProperties: "Booking").ToList();
            if (!billingRecords.Any())
            {
                throw new CustomException.DataNotFoundException("Bạn không có đơn nào.");
            }
            var billingRecordResponses = _mapper.Map<List<BillingRecordResponse>>(billingRecords);
            return billingRecordResponses;
        }

        public async Task<List<TrackingResponse>> GetAllTrackingByBookingId(long id)
        {
            var user = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(user);
            var po = _unitOfWork.PetOwnerRepository.Get(s => s.AccountId == account.Id)
                                            .FirstOrDefault();

            var duringBooking = _unitOfWork.BookingRepository.Get(db => db.Id == id
                                            //&& db.StoreService.Store.Id == store.Id
                                            && db.Status != BookingStatus.Denied.ToString(),
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

        public async Task<List<StoreServicePOResponse>> RecommendService()
        {
            var result = new List<StoreServicePOResponse>();

            var listStoreServices = _unitOfWork.StoreServiceRepository.Get(ss => ss.StartTime > DateTimeOffset.UtcNow && ss.Status == StoreServiceStatus.Available.ToString() && ss.CurrentPetOwner < ss.LimitPetOwner, includeProperties: "Store,Service,Service.ServiceType,Service.Brand").OrderByDescending(ob => ob.Service.BookingCount).ToList();


            foreach (var item in listStoreServices)
            {
                if (!result.FindAll(r => r.ServiceId == item.ServiceId && r.StoreId == item.StoreId).Any())
                {
                    var service = _mapper.Map<StoreServicePOResponse>(item);
                    service.StoreName = item.Store.Name;
                    service.ServiceName = item.Service.Name;
                    service.ServiceType = item.Service.ServiceType.Name;
                    service.Image = item.Service.Image;
                    service.Cost = item.Service.Cost;
                    service.Duration = item.Service.Duration;
                    service.ServiceTypeId = item.Service.ServiceTypeId;
                    service.BrandName = item.Service.Brand.Name;
                    service.BrandId = item.Service.Brand.Id;
                    service.Description = item.Service.Description;
                    service.BookingCount = item.Service.BookingCount;
                    service.TotalRating = item.Service.TotalRating;

                    result.Add(service);
                }
            }

            return result;
        }

        public async Task<List<StoreServicePOResponse>> RecommendServicePO()
        {
            var result = new List<StoreServicePOResponse>();

            var listStoreServices = _unitOfWork.StoreServiceRepository.Get(ss => ss.StartTime > DateTimeOffset.UtcNow && ss.Status == StoreServiceStatus.Available.ToString() && ss.CurrentPetOwner < ss.LimitPetOwner, includeProperties: "Store,Service,Service.ServiceType,Service.Brand").ToList();

            //var accountId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            //var po = _unitOfWork.PetOwnerRepository.Get(u => u.AccountId == accountId).FirstOrDefault();
            //var pets = _unitOfWork.PetRepository.Get(p => p.PetOwnerId == po.Id && p.Status == PetStatus.Available.ToString());
            //var petIds = pets.Select(p => p.Id).ToList();

            //var storeServiceDict = listStoreServices.ToDictionary(ss => ss.Id);
            //var servicePointList = new List<ServicePoint>();
            //var vaccines = _unitOfWork.VaccineHistoryRepository.Get(v => petIds.Contains(v.PetId)).ToList();

            //foreach (var service in listStoreServices)
            //{
            //    var point = await CalculatePoint(service);
            //    if (vaccines.Any() && service.Service.ServiceTypeId == 2) point += 10;
            //    servicePointList.Add(new ServicePoint { ServiceId = service.Id, Point =  point});
            //}
            //var list = servicePointList.OrderByDescending(ob => ob.Point).ToList();
            //foreach (var item in list)
            //{
            //    if (storeServiceDict.TryGetValue(item.ServiceId, out var storeService))
            //    {
            //        result.Add(_mapper.Map<StoreSerResponse>(storeService));
            //    }
            //}

            var list = listStoreServices.OrderByDescending(s => s.Service.TotalRating).ThenByDescending(s => s.Store.TotalRating).ThenByDescending(s => s.Service.BookingCount).ToList();
            foreach (var item in list)
            {
                var service = _mapper.Map<StoreServicePOResponse>(item);
                service.StoreName = item.Store.Name;
                service.ServiceName = item.Service.Name;
                service.ServiceType = item.Service.ServiceType.Name;
                service.Image = item.Service.Image;
                service.Cost = item.Service.Cost;
                service.Duration = item.Service.Duration;
                service.ServiceTypeId = item.Service.ServiceTypeId;
                service.BrandName = item.Service.Brand.Name;
                service.BrandId = item.Service.Brand.Id;
                service.Description = item.Service.Description;
                service.BookingCount = item.Service.BookingCount;
                service.TotalRating = item.Service.TotalRating;

                result.Add(service);
            }

            return result;
        }

        public async Task<double> CalculatePoint(StoreService storeService)
        {
            var point = 0;

            if (storeService.Service.TotalRating > 4) point += 20;
            else if (storeService.Service.TotalRating > 3) point += 15;
            else if (storeService.Service.TotalRating > 1) point += 10;
            else point += 5;

            if (storeService.Store.TotalRating > 4) point += 20;
            else if (storeService.Store.TotalRating > 3) point += 15;
            else if (storeService.Store.TotalRating > 1) point += 10;
            else point += 5;

            if (storeService.Service.Cost < 150000) point += 15;
            else if (storeService.Service.Cost < 1000000) point += 12;
            else if (storeService.Service.Cost < 10000000) point += 10;
            else if (storeService.Service.Cost < 70000000) point += 5;

            if (storeService.Service.BookingCount < 100) point += 0;
            else if (storeService.Service.BookingCount < 1000) point += 10;
            else point += 20;

            var report = _unitOfWork.ReportRepository.Get(rp => rp.TargetId.Equals(storeService.Store.AccountId)).Count();
            point -= report;

            return point;
        }

        public async Task<List<Store>> SearchStore(string character)
        {
            return _unitOfWork.StoreRepository.Get(s => s.Status == true && s.Name.Contains(character)).ToList();
        }

        public async Task<List<StoreServicePOResponse>> SearchStoreService(string character)
        {
            var result = new List<StoreServicePOResponse>();
            var list = _unitOfWork.StoreServiceRepository.Get(ss => ss.Status.Equals(StoreServiceStatus.Available.ToString()) && ss.Service.Name.Contains(character), includeProperties: "Store,Service,Service.ServiceType,Service.Brand").ToList();
            foreach (var item in list)
            {
                var service = _mapper.Map<StoreServicePOResponse>(item);
                service.StoreName = item.Store.Name;
                service.ServiceName = item.Service.Name;
                service.ServiceType = item.Service.ServiceType.Name;
                service.Image = item.Service.Image;
                service.Cost = item.Service.Cost;
                service.Duration = item.Service.Duration;
                service.ServiceTypeId = item.Service.ServiceTypeId;
                service.BrandName = item.Service.Brand.Name;
                service.BrandId = item.Service.Brand.Id;
                service.Description = item.Service.Description;
                service.BookingCount = item.Service.BookingCount;
                service.TotalRating = item.Service.TotalRating;

                result.Add(service);
            }

            return result;
        }

        public async Task<List<SearchBrandResponse>> SearchBrand(string character)
        {
            var brands = _unitOfWork.BrandRepository.Get(b => b.Status == true && b.Name.Contains(character)).ToList();
            var result = _mapper.Map<List<SearchBrandResponse>>(brands);
            foreach (var brand in result)
            {
                var services = _unitOfWork.ServiceRepository.Get(s => s.BrandId == brand.Id).ToList();
                var stores = _unitOfWork.StoreRepository.Get(s => s.BrandId == brand.Id).ToList();

                var serviceList = _mapper.Map<List<BrandServiceResponse>>(services);
                brand.ServiceNames = serviceList;
                brand.TotalStore = stores.Count;
                brand.TotalServices = services.Count;
                brand.Rating = stores.Average(s => s.TotalRating);
            }

            return result;
        }

        public async Task<List<StoreServicePOResponse>> Top6StoreServices()
        {
            var result = new List<StoreServicePOResponse>();

            var listStoreServices = _unitOfWork.StoreServiceRepository.Get(ss => ss.StartTime > DateTimeOffset.UtcNow && ss.Status == StoreServiceStatus.Available.ToString() && ss.CurrentPetOwner < ss.LimitPetOwner, includeProperties: "Store,Service,Service.ServiceType,Service.Brand").OrderByDescending(ob => ob.Service.BookingCount).ToList();


            foreach (var item in listStoreServices)
            {
                if (!result.FindAll(r => r.ServiceId == item.ServiceId && r.StoreId == item.StoreId).Any())
                {
                    var service = _mapper.Map<StoreServicePOResponse>(item);
                    service.StoreName = item.Store.Name;
                    service.ServiceName = item.Service.Name;
                    service.ServiceType = item.Service.ServiceType.Name;
                    service.Image = item.Service.Image;
                    service.Cost = item.Service.Cost;
                    service.Duration = item.Service.Duration;
                    service.ServiceTypeId = item.Service.ServiceTypeId;
                    service.BrandName = item.Service.Brand.Name;
                    service.BrandId = item.Service.Brand.Id;
                    service.Description = item.Service.Description;
                    service.BookingCount = item.Service.BookingCount;
                    service.TotalRating = item.Service.TotalRating;

                    result.Add(service);
                }
            }

            return result.Take(6).ToList();
        }

    }
}
