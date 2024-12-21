using AutoMapper;
using FluffyPaw_Application.DTO.Request.BookingRequest;
using FluffyPaw_Application.DTO.Request.NotificationRequest;
using FluffyPaw_Application.DTO.Response.BookingResponse;
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
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FluffyPaw_Application.ServiceImplements
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthentication _authentication;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJobScheduler _jobScheduler;
        private readonly INotificationService _notificationService;
        private readonly IFirebaseConfiguration _firebaseConfiguration;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper, IAuthentication authentication,
                            IHttpContextAccessor httpContextAccessor, IJobScheduler jobScheduler,
                            INotificationService notificationService, IFirebaseConfiguration firebaseConfiguration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authentication = authentication;
            _httpContextAccessor = httpContextAccessor;
            _jobScheduler = jobScheduler;
            _notificationService = notificationService;
            _firebaseConfiguration = firebaseConfiguration;
        }

        public async Task<BookingResponse> GetBookingById(long id)
        {
            var existingBooking = _unitOfWork.BookingRepository.Get(b => b.Id == id,
                                        includeProperties: "Pet,StoreService,StoreService.Service,StoreService.Store").FirstOrDefault();
            if (existingBooking == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy đặt lịch này.");
            }

            var bookingResponse = _mapper.Map<BookingResponse>(existingBooking);
            return bookingResponse;
        }

        public async Task<BookingResponse> Checkin(CheckinRequest checkinRequest)
        {
            var userId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(userId);
            if (account == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy tài khoản.");
            }

            var store = _unitOfWork.StoreRepository.Get(s => s.AccountId == account.Id && s.Status == true).FirstOrDefault();
            if (store == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy cửa hàng.");
            }

            var booking = _unitOfWork.BookingRepository.Get(b => b.Id == checkinRequest.Id && b.Status == BookingStatus.Accepted.ToString(),
                                            includeProperties: "StoreService,StoreService.Store,StoreService.Service,Pet,Pet.PetOwner").FirstOrDefault();
            if (booking == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy đặt lịch này.");
            }

            var storeService = _unitOfWork.StoreServiceRepository.GetByID(booking.StoreServiceId);
            if (storeService == null || storeService.StoreId != store.Id)
            {
                throw new CustomException.InvalidDataException("Đặt lịch này không thuộc lịch trình của cửa hàng.");
            }

            var allowedCheckInTime = booking.StartTime.AddHours(-7.5);
            if (CoreHelper.SystemTimeNow <= allowedCheckInTime)
            {
                throw new CustomException.InvalidDataException("Chỉ được phép check-in trước 30 phút.");
            }

            booking.Checkin = true;
            booking.CheckinTime = CoreHelper.SystemTimeNow.AddHours(7);
            booking.CheckinImage = await _firebaseConfiguration.UploadImage(checkinRequest.CheckinImagge);
            _unitOfWork.BookingRepository.Update(booking);
            await _unitOfWork.SaveAsync();

            var notificationRequest = new NotificationRequest
            {
                ReceiverId = booking.Pet.PetOwner.AccountId,
                Name = "Check in booking",
                Type = NotificationType.Checkin.ToString(),
                Description = $"Đã xác nhận dịch vụ {booking.StoreService.Service.Name} của {booking.Pet.Name} check in thành công.",
                ReferenceId = booking.Id
            };

            await _notificationService.CreateNotification(notificationRequest);

            var bookingResponse = _mapper.Map<BookingResponse>(booking);
            bookingResponse.CheckinTime = CoreHelper.SystemTimeNow;

            return bookingResponse;
        }

        public async Task<List<BookingResponse>> Checkout(CheckOutRequest checkOutRequest)
        {
            var userId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(userId);
            if (account == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy tài khoản.");
            }

            var store = _unitOfWork.StoreRepository.Get(s => s.AccountId == account.Id && s.Status == true,
                                            includeProperties: "Brand,Brand.Account").FirstOrDefault();
            if (store == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy cửa hàng.");
            }

            var bookingResponses = new List<BookingResponse>();
            double totalAmountToAdd = 0;

            var booking = _unitOfWork.BookingRepository.Get(b => b.Id == checkOutRequest.Id,
                                        includeProperties: "StoreService,StoreService.Service,Pet,Pet.PetOwner").FirstOrDefault();
            if (booking == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy đặt lịch này.");
            }

            var storeService = _unitOfWork.StoreServiceRepository.Get(ss => ss.Id == booking.StoreServiceId,
                                                    includeProperties: "Service,Service.ServiceType").FirstOrDefault();
            if (storeService == null || storeService.StoreId != store.Id)
            {
                throw new CustomException.InvalidDataException("Đặt lịch này không thuộc lịch trình của cửa hàng.");
            }

            var serviceTypeName = storeService.Service.ServiceType.Name;
            if (serviceTypeName == "Khách sạn")
            {
                var currentDate = CoreHelper.SystemTimeNow.Date;
                var noonToday = currentDate.AddHours(12);

                if (CoreHelper.SystemTimeNow < noonToday)
                {
                    // Nếu hủy trước 12 giờ trưa, giảm CurrentPetOwner cho các ngày từ ngày hiện tại đến ngày kết thúc
                    var endDate = booking.EndTime.Date;

                    // Lấy tất cả StoreService tương ứng với các ngày cần giảm
                    var affectedStoreServices = _unitOfWork.StoreServiceRepository.Get(ss =>
                        ss.ServiceId == storeService.ServiceId &&
                        ss.StoreId == storeService.StoreId &&
                        ss.StartTime.Date >= currentDate &&
                        ss.StartTime.Date <= endDate
                    );

                    foreach (var affectedStoreService in affectedStoreServices)
                    {
                        if (affectedStoreService.CurrentPetOwner > 0)
                        {
                            affectedStoreService.CurrentPetOwner--;
                            _unitOfWork.StoreServiceRepository.Update(affectedStoreService);
                        }
                    }
                    await _unitOfWork.SaveAsync();
                }
            }

            var service = _unitOfWork.ServiceRepository.GetByID(storeService.ServiceId);
            service.BookingCount++;
            _unitOfWork.ServiceRepository.Update(service);

            booking.CheckOut = true;
            booking.CheckOutTime = CoreHelper.SystemTimeNow.AddHours(7);
            if (CoreHelper.SystemTimeNow < booking.EndTime)
            {
                booking.EndTime = CoreHelper.SystemTimeNow.AddHours(7);
            }
            booking.CheckoutImage = await _firebaseConfiguration.UploadImage(checkOutRequest.CheckoutImage);
            booking.Status = BookingStatus.Ended.ToString();
            _unitOfWork.BookingRepository.Update(booking);
            await _unitOfWork.SaveAsync();

            if (serviceTypeName == "Tiêm chủng")
            {
                var pet = _unitOfWork.PetRepository.Get(p => p.Id == booking.PetId).FirstOrDefault();
                var imageUrl = await _firebaseConfiguration.UploadImage(checkOutRequest.Image);

                float weight = pet.Weight;
                if (checkOutRequest.PetCurrentWeight != null)
                {
                    weight = checkOutRequest.PetCurrentWeight.Value;
                }
                // Tạo lịch sử tiêm chủng mới
                var vaccineHistory = new VaccineHistory
                {
                    PetId = pet.Id,
                    Image = imageUrl,
                    Name = checkOutRequest.Name,
                    PetCurrentWeight = weight,
                    VaccineDate = (DateTimeOffset)booking.CheckOutTime,
                    NextVaccineDate = checkOutRequest.NextVaccineDate,
                    Description = checkOutRequest.Description,
                    Status = checkOutRequest.NextVaccineDate.HasValue ? VaccineStatus.Incomplete.ToString() : VaccineStatus.Complete.ToString()
                };

                _unitOfWork.VaccineHistoryRepository.Insert(vaccineHistory);
                pet.Weight = weight;
                _unitOfWork.PetRepository.Update(pet);
                await _unitOfWork.SaveAsync();
            }

            var notificationRequest = new NotificationRequest
            {
                ReceiverId = booking.Pet.PetOwner.AccountId,
                Name = "Check out booking",
                Type = NotificationType.Checkout.ToString(),
                Description = $"Đã xác nhận dịch vụ {booking.StoreService.Service.Name} của {booking.Pet.Name} check out thành công.",
                ReferenceId = booking.Id
            };

            await _notificationService.CreateNotification(notificationRequest);

            totalAmountToAdd += booking.Cost;

            var bookingResponse = _mapper.Map<BookingResponse>(booking);
            bookingResponse.CheckOutTime = CoreHelper.SystemTimeNow;
            bookingResponses.Add(bookingResponse);

            var brandWallet = _unitOfWork.WalletRepository.Get(bw => bw.AccountId == store.Brand.AccountId).FirstOrDefault();
            if (booking.PaymentMethod == BookingPaymentMethod.FluffyPay.ToString())
            {
                brandWallet.Balance += totalAmountToAdd;
                _unitOfWork.WalletRepository.Update(brandWallet);

                var newBillingRecord = new BillingRecord
                {
                    WalletId = brandWallet.Id,
                    BookingId = booking.Id,
                    Amount = booking.Cost,
                    Type = BillingType.Add.ToString(),
                    Description = $"Doanh thu của dịch vụ {storeService.Service.Name} từ cửa hàng {store.Name} đã được cộng vào ví FluffyPay.",
                    CreateDate = CoreHelper.SystemTimeNow.AddHours(7)
                };

                _unitOfWork.BillingRecordRepository.Insert(newBillingRecord);
                await _unitOfWork.SaveAsync();

                var brandNotificationRequest = new NotificationRequest
                {
                    ReceiverId = store.Brand.AccountId,
                    Name = "Check out booking",
                    Type = NotificationType.Checkout.ToString(),
                    Description = $"Đã xác nhận dịch vụ {booking.StoreService.Service.Name} của {booking.Pet.Name} " +
                                                        $"từ cửa hàng {store.Name} check out thành công.",
                    ReferenceId = booking.Id
                };

                await _notificationService.CreateNotification(brandNotificationRequest);
            }
            else if (booking.PaymentMethod == BookingPaymentMethod.COD.ToString())
            {
                var newBillingRecord = new BillingRecord
                {
                    WalletId = brandWallet.Id,
                    BookingId = booking.Id, 
                    Amount = booking.Cost,
                    Type = BillingType.COD.ToString(),
                    Description = $"Khách đã thanh toán trực tiếp tại cửa hàng cho dịch vụ {storeService.Service.Name} từ cửa hàng {store.Name}.",
                    CreateDate = CoreHelper.SystemTimeNow.AddHours(7)
                };

                _unitOfWork.BillingRecordRepository.Insert(newBillingRecord);
                await _unitOfWork.SaveAsync();

                var brandNotificationRequest = new NotificationRequest
                {
                    ReceiverId = store.Brand.AccountId,
                    Name = "Check out booking",
                    Type = NotificationType.Checkout.ToString(),
                    Description = $"Đã xác nhận dịch vụ {booking.StoreService.Service.Name} của {booking.Pet.Name} " +
                                                        $"từ cửa hàng {store.Name} check out thành công.",
                    ReferenceId = booking.Id
                };

                await _notificationService.CreateNotification(brandNotificationRequest);
            }

            await _unitOfWork.SaveAsync();

            return bookingResponses;
        }

        public async Task<List<BookingRatingResponse>> GetAllBookingRatingByServiceId(long id)
        {
            var service = _unitOfWork.ServiceRepository.GetByID(id);
            if (service == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy dịch vụ này.");
            }

            var storeServiceIds = _unitOfWork.StoreServiceRepository
                .Get(ss => ss.ServiceId == service.Id)
                .Select(ss => ss.Id)
                .ToList();


            var bookingIds = _unitOfWork.BookingRepository
                .Get(b => storeServiceIds.Contains(b.StoreServiceId))
                .Select(b => b.Id)
                .ToList();

            var bookingRatings = _unitOfWork.BookingRatingRepository
                .Get(br => bookingIds.Contains(br.BookingId))
                .ToList();

            if (!bookingRatings.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy đánh giá nào cho dịch vụ này.");
            }

            var petOwnerIds = bookingRatings.Select(br => br.PetOwnerId).Distinct().ToList();
            var petOwners = _unitOfWork.PetOwnerRepository.Get(po => petOwnerIds.Contains(po.Id), includeProperties: "Account").ToList();

            var bookingRatingResponses = _mapper.Map<List<BookingRatingResponse>>(bookingRatings);

            foreach (var bookingRatingResponse in bookingRatingResponses)
            {
                var petOwner = petOwners.FirstOrDefault(po => po.Id == bookingRatingResponse.PetOwnerId);
                var account = petOwner?.Account;
                bookingRatingResponse.FullName = petOwner?.FullName;
                bookingRatingResponse.Avatar = account?.Avatar;
            }

            return bookingRatingResponses;
        }

        public async Task<List<BookingRatingResponse>> GetAllBookingRatingByStoreId(long id)
        {
            var store = _unitOfWork.StoreRepository.GetByID(id);
            if (store == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy cửa hàng này.");
            }

            var storeServices = _unitOfWork.StoreServiceRepository.Get(ss => ss.StoreId == id).ToList();

            var storeServiceIds = storeServices.Select(ss => ss.Id).ToList();

            var bookingIds = _unitOfWork.BookingRepository
                .Get(b => storeServiceIds.Contains(b.StoreServiceId))
                .Select(b => b.Id)
                .ToList();

            var bookingRatings = _unitOfWork.BookingRatingRepository.Get(br => bookingIds.Contains(br.BookingId)).ToList();

            if (!bookingRatings.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy đánh giá nào cho cửa hàng này.");
            }

            var petOwnerIds = bookingRatings.Select(br => br.PetOwnerId).Distinct().ToList();
            var petOwners = _unitOfWork.PetOwnerRepository.Get(po => petOwnerIds.Contains(po.Id), includeProperties: "Account").ToList();

            var bookingRatingResponses = _mapper.Map<List<BookingRatingResponse>>(bookingRatings);

            foreach (var bookingRatingResponse in bookingRatingResponses)
            {
                var petOwner = petOwners.FirstOrDefault(po => po.Id == bookingRatingResponse.PetOwnerId);
                var account = petOwner?.Account;
                bookingRatingResponse.FullName = petOwner?.FullName;
                bookingRatingResponse.Avatar = account?.Avatar;
            }

            return bookingRatingResponses;
        }

        public async Task<BookingRatingResponse> GetBookingRatingByBookingId(long id)
        {
            var booking = _unitOfWork.BookingRepository.GetByID(id);
            if (booking == null)
            {
                throw new CustomException.DataNotFoundException("Đơn đặt bạn chọn không tồn tại.");
            }

            var bookingRating = _unitOfWork.BookingRatingRepository.Get(b => b.BookingId == booking.Id,
                                             includeProperties: "PetOwner,PetOwner.Account").FirstOrDefault();
            if (bookingRating == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy đánh giá của đơn đặt này.");
            }

            var bookingRatingResponse = _mapper.Map<BookingRatingResponse>(bookingRating);
            bookingRatingResponse.FullName = bookingRating.PetOwner.FullName;
            bookingRatingResponse.Avatar = bookingRating.PetOwner.Account.Avatar;
            return bookingRatingResponse;
        }
        public async Task<BookingRatingResponse> GetBookingRatingById(long id)
        {
            var bookingRating = _unitOfWork.BookingRatingRepository.Get(br => br.Id == id,
                                            includeProperties: "PetOwner,PetOwner.Account").FirstOrDefault();
            if (bookingRating == null)
            {
                throw new CustomException.DataNotFoundException("Khoong tìm thấy đánh giá này.");
            }

            var bookingRatingResponse = _mapper.Map<BookingRatingResponse>(bookingRating);
            bookingRatingResponse.FullName = bookingRating.PetOwner.FullName;
            bookingRatingResponse.Avatar = bookingRating.PetOwner.Account.Avatar;
            return bookingRatingResponse;
        }

        public async Task<BookingRatingResponse> CreateBookingRatingByBookingId(long bookingId, BookingRatingRequest createBookingRatingRequest)
        {
            var userId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(userId);
            if (account == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy tài khoản.");
            }

            var po = _unitOfWork.PetOwnerRepository.Get(po => po.AccountId == account.Id, includeProperties: "Account").FirstOrDefault();
            if (po == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy chủ thú cưng liên kết với tài khoản này.");
            }

            var existingBooking = _unitOfWork.BookingRepository.Get(eb => eb.Id == bookingId && eb.CheckOut == true).FirstOrDefault();
            if (existingBooking == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy dịch vụ hoặc dịch vụ này chưa được checkout.");
            }

            var existingBookingRating = _unitOfWork.BookingRatingRepository.Get(ebr => ebr.BookingId == bookingId && ebr.PetOwnerId == po.Id).FirstOrDefault();
            if (existingBookingRating != null)
            {
                throw new CustomException.DataExistException("Bạn đã đánh giá dịch vụ này rồi, vui lòng chọn nút chỉnh sửa để thay đổi đánh giá của bạn.");
            }

            string uploadedImageUrl = null;

            if (createBookingRatingRequest.Image != null)
            {
                uploadedImageUrl = await _firebaseConfiguration.UploadImage(createBookingRatingRequest.Image);
            }

            var newBookingRating = new BookingRating
            {
                BookingId = bookingId,
                PetOwnerId = po.Id,
                ServiceVote = createBookingRatingRequest.ServiceVote,
                StoreVote = createBookingRatingRequest.StoreVote,
                Description = createBookingRatingRequest.Description,
                Image = uploadedImageUrl,
                CreateDate = CoreHelper.SystemTimeNow.AddHours(7)
            };

            _unitOfWork.BookingRatingRepository.Insert(newBookingRating);
            await _unitOfWork.SaveAsync();

            UpdateServiceTotalRatingByBookingRatingId(newBookingRating.Id);
            UpdateStoreTotalRatingByBookingRatingId(newBookingRating.Id);

            var bookingRatingResponse = _mapper.Map<BookingRatingResponse>(newBookingRating);
            bookingRatingResponse.FullName = po.FullName;
            bookingRatingResponse.Avatar = po.Account.Avatar;
            bookingRatingResponse.CreateDate = CoreHelper.SystemTimeNow;
            return bookingRatingResponse;
        }


        public async Task<BookingRatingResponse> UpdateBookingRatingById(long id, BookingRatingRequest bookingRatingRequest)
        {
            var userId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(userId);
            if (account == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy tài khoản.");
            }

            var po = _unitOfWork.PetOwnerRepository.Get(po => po.AccountId == account.Id,
                                            includeProperties: "Account").FirstOrDefault();
            if (po == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy chủ thú cưng liên kết với tài khoản.");
            }

            var existingBookingRating = _unitOfWork.BookingRatingRepository.Get(ebr => ebr.Id == id && ebr.PetOwnerId == po.Id).FirstOrDefault();
            if (existingBookingRating == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy đánh giá.");
            }

            _mapper.Map(bookingRatingRequest, existingBookingRating);

            if (bookingRatingRequest.Image != null)
            {
                var imgUrl = await _firebaseConfiguration.UploadImage(bookingRatingRequest.Image);
                existingBookingRating.Image = imgUrl;
            }

            existingBookingRating.CreateDate = CoreHelper.SystemTimeNow.AddHours(7);

            await _unitOfWork.SaveAsync();

            UpdateServiceTotalRatingByBookingRatingId(existingBookingRating.Id);
            UpdateStoreTotalRatingByBookingRatingId(existingBookingRating.Id);

            var bookingRatingResponse = _mapper.Map<BookingRatingResponse>(existingBookingRating);
            bookingRatingResponse.FullName = po.FullName;
            bookingRatingResponse.Avatar = po.Account.Avatar;
            bookingRatingResponse.CreateDate = CoreHelper.SystemTimeNow;
            return bookingRatingResponse;
        }


        public async Task<bool> DeleteBookingRating(long id)
        {
            var bookingRating = _unitOfWork.BookingRatingRepository.GetByID(id);
            if (bookingRating == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy đánh giá này.");
            }

            var booking = _unitOfWork.BookingRepository.GetByID(bookingRating.BookingId);
            if (booking == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy đặt lịch liên kết với đánh giá này.");
            }

            var storeService = _unitOfWork.StoreServiceRepository.GetByID(booking.StoreServiceId);
            if (storeService == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy dịch vụ liên kết với đặt lịch này.");
            }

            var service = _unitOfWork.ServiceRepository.GetByID(storeService.ServiceId);
            if (service == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy dịch vụ liên kết với đánh giá này.");
            }

            _unitOfWork.BookingRatingRepository.Delete(bookingRating);
            await _unitOfWork.SaveAsync();

            var ratings = _unitOfWork.BookingRatingRepository.Get(br => br.BookingId != booking.Id)
                .Select(br => br.ServiceVote)
                .ToList();

            service.TotalRating = ratings.Any() ? (float)ratings.Average() : 0;
            _unitOfWork.ServiceRepository.Update(service);
            await _unitOfWork.SaveAsync();

            return true;
        }


        public void UpdateServiceTotalRatingByBookingRatingId(long bookingRatingId)
        {
            var bookingRating = _unitOfWork.BookingRatingRepository.GetByID(bookingRatingId);
            var booking = _unitOfWork.BookingRepository.GetByID(bookingRating.BookingId);
            var storeService = _unitOfWork.StoreServiceRepository.GetByID(booking.StoreServiceId);
            var service = _unitOfWork.ServiceRepository.GetByID(storeService.ServiceId);

            var allStoreServices = _unitOfWork.StoreServiceRepository.Get(ss => ss.ServiceId == service.Id).ToList();
            var allStoreServiceIds = allStoreServices.Select(ss => ss.Id).ToList();

            var allBookings = _unitOfWork.BookingRepository.Get(b => allStoreServiceIds.Contains(b.StoreServiceId)).ToList();
            var allBookingIds = allBookings.Select(b => b.Id).ToList();

            var allBookingRatings = _unitOfWork.BookingRatingRepository.Get(br => allBookingIds.Contains(br.BookingId)).ToList();

            var totalVotes = allBookingRatings.Sum(br => br.ServiceVote);
            var totalCount = allBookingRatings.Count;

            service.TotalRating = totalCount > 0 ? (float)totalVotes / totalCount : 0f;

            _unitOfWork.ServiceRepository.Update(service);
            _unitOfWork.Save();
        }

        public void UpdateStoreTotalRatingByBookingRatingId(long bookingRatingId)
        {
            var bookingRating = _unitOfWork.BookingRatingRepository.GetByID(bookingRatingId);
            var booking = _unitOfWork.BookingRepository.GetByID(bookingRating.BookingId);
            var storeService = _unitOfWork.StoreServiceRepository.GetByID(booking.StoreServiceId);
            var store = _unitOfWork.StoreRepository.GetByID(storeService.StoreId);

            var allStoreServices = _unitOfWork.StoreServiceRepository.Get(ss => ss.StoreId == store.Id).ToList();
            var allStoreServiceIds = allStoreServices.Select(ss => ss.Id).ToList();

            var allBookings = _unitOfWork.BookingRepository.Get(b => allStoreServiceIds.Contains(b.StoreServiceId)).ToList();
            var allBookingIds = allBookings.Select(b => b.Id).ToList();

            var allBookingRatings = _unitOfWork.BookingRatingRepository.Get(br => allBookingIds.Contains(br.BookingId)).ToList();

            var totalVotes = allBookingRatings.Sum(br => br.StoreVote);
            var totalCount = allBookingRatings.Count;

            store.TotalRating = totalCount > 0 ? (float)totalVotes / totalCount : 0f;

            _unitOfWork.StoreRepository.Update(store);
            _unitOfWork.Save();
        }

    }
}
