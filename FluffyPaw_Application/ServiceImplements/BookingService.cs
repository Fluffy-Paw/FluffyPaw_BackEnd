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
            var existingBooking = _unitOfWork.BookingRepository.GetByID(id);
            if (existingBooking == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy đặt lịch này.");
            }

            var bookingResponse = _mapper.Map<BookingResponse>(existingBooking);
            return bookingResponse;
        }

        public async Task<List<BookingResponse>> Checkin(CheckinRequest checkinRequest)
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

            var bookingResponses = new List<BookingResponse>();

            var booking = _unitOfWork.BookingRepository.Get(b => b.Id == checkinRequest.Id && b.Status == BookingStatus.Accepted.ToString(),
                                            includeProperties: "StoreService,StoreService.Service,Pet,Pet.PetOwner").FirstOrDefault();
            if (booking == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy đặt lịch này.");
            }

            var storeService = _unitOfWork.StoreServiceRepository.GetByID(booking.StoreServiceId);
            if (storeService == null || storeService.StoreId != store.Id)
            {
                throw new CustomException.InvalidDataException("Đặt lịch này không thuộc lịch trình của cửa hàng.");
            }

            booking.Checkin = true;
            booking.CheckinTime = CoreHelper.SystemTimeNow.AddHours(7);
            _unitOfWork.BookingRepository.Update(booking);

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
            bookingResponses.Add(bookingResponse);

            await _unitOfWork.SaveAsync();

            return bookingResponses;
        }

        public async Task<List<BookingResponse>> Checkout(CheckOutRequest checkRequest)
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

            var booking = _unitOfWork.BookingRepository.Get(b => b.Id == checkRequest.Id,
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
            if (serviceTypeName == "Hotel")
            {
                booking.CheckOut = true;
                booking.CheckOutTime = CoreHelper.SystemTimeNow.AddHours(7);
            }

            else
            {
                if (booking.Status != BookingStatus.Ended.ToString())
                {
                    throw new CustomException.InvalidDataException("Đặt lịch này cần phải kết thúc trước khi check out.");
                }

                booking.CheckOut = true;
                booking.CheckOutTime = CoreHelper.SystemTimeNow.AddHours(7);
            }

            _unitOfWork.BookingRepository.Update(booking);
            await _unitOfWork.SaveAsync();

            if (serviceTypeName == "Vaccine")
            {
                var pet = _unitOfWork.PetRepository.Get(p => p.Id == booking.Id).FirstOrDefault();
                var imageUrl = await _firebaseConfiguration.UploadImage(checkRequest.Image);
                var vaccineHistory = new VaccineHistory
                {
                    Image = imageUrl,
                    Name = checkRequest.Name,
                    PetCurrentWeight = checkRequest.PetCurrentWeight ?? pet.Weight,
                    VaccineDate = (DateTimeOffset)booking.CheckOutTime,
                    NextVaccineDate = checkRequest.NextVaccineDate,
                    Description = checkRequest.Description,
                    Status = VaccineStatus.Complete.ToString()
                };

                _unitOfWork.VaccineHistoryRepository.Insert(vaccineHistory);
                await _unitOfWork.SaveAsync();
            }

            var notificationRequest = new NotificationRequest
            {
                ReceiverId = booking.Pet.PetOwner.AccountId,
                Name = "Check in booking",
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
            brandWallet.Balance += totalAmountToAdd;
            _unitOfWork.WalletRepository.Update(brandWallet);

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

            if (!storeServiceIds.Any())
            {
                throw new CustomException.DataNotFoundException("Dịch vụ này không có lịch trình nào.");
            }

            var bookingIds = _unitOfWork.BookingRepository
                .Get(b => storeServiceIds.Contains(b.StoreServiceId))
                .Select(b => b.Id)
                .ToList();

            if (!bookingIds.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy đặt lịch nào liên quan đến dịch vụ này.");
            }

            var bookingRatings = _unitOfWork.BookingRatingRepository
                .Get(br => bookingIds.Contains(br.BookingId))
                .ToList();

            if (!bookingRatings.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy đánh giá nào cho dịch vụ này.");
            }

            var bookingRatingResponses = bookingRatings
                .Select(br => _mapper.Map<BookingRatingResponse>(br))
                .ToList();

            return bookingRatingResponses;
        }

        public async Task<BookingRatingResponse> GetBookingRatingByBookingId(long id)
        {
            var booking = _unitOfWork.BookingRepository.GetByID(id);
            var bookingRating = _unitOfWork.BookingRatingRepository.Get(b => b.BookingId == booking.Id).FirstOrDefault();
            if (bookingRating == null)
            {
                throw new CustomException.DataNotFoundException("Khoong tìm thấy đánh giá này.");
            }

            var bookingRatingResponse = _mapper.Map<BookingRatingResponse>(bookingRating);
            return bookingRatingResponse;
        }
        public async Task<BookingRatingResponse> GetBookingRatingById(long id)
        {
            var bookingRating = _unitOfWork.BookingRatingRepository.GetByID(id);
            if (bookingRating == null)
            {
                throw new CustomException.DataNotFoundException("Khoong tìm thấy đánh giá này.");
            }

            var bookingRatingResponse = _mapper.Map<BookingRatingResponse>(bookingRating);
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

            var po = _unitOfWork.PetOwnerRepository.Get(po => po.AccountId == account.Id).FirstOrDefault();
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
                Vote = createBookingRatingRequest.Vote,
                Description = createBookingRatingRequest.Description,
                Image = uploadedImageUrl
            };

            _unitOfWork.BookingRatingRepository.Insert(newBookingRating);
            await _unitOfWork.SaveAsync();

            UpdateTotalRatingByBookingRatingId(newBookingRating.Id);

            var bookingRatingResponse = _mapper.Map<BookingRatingResponse>(newBookingRating);
            return bookingRatingResponse;
        }


        public async Task<BookingRatingResponse> UpdateBookingRatingById(long id, BookingRatingRequest bookingRatingRequest)
        {
            // Validate the user
            var userId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(userId);
            if (account == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy tài khoản.");
            }

            // Validate the pet owner
            var po = _unitOfWork.PetOwnerRepository.Get(po => po.AccountId == account.Id).FirstOrDefault();
            if (po == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy chủ thú cưng liên kết với tài khoản.");
            }

            // Validate the existing booking rating
            var existingBookingRating = _unitOfWork.BookingRatingRepository
                .Get(ebr => ebr.Id == id && ebr.PetOwnerId == po.Id)
                .FirstOrDefault();

            if (existingBookingRating == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy đánh giá.");
            }

            // Map the updated values
            _mapper.Map(bookingRatingRequest, existingBookingRating);

            // Save changes
            await _unitOfWork.SaveAsync();

            // Recalculate the total rating
            UpdateTotalRatingByBookingRatingId(existingBookingRating.Id);

            // Create the response object
            var bookingRatingResponse = _mapper.Map<BookingRatingResponse>(existingBookingRating);

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
                .Select(br => br.Vote)
                .ToList();

            service.TotalRating = ratings.Any() ? (float)ratings.Average() : 0;
            _unitOfWork.ServiceRepository.Update(service);
            await _unitOfWork.SaveAsync();

            return true;
        }


        public void UpdateTotalRatingByBookingRatingId(long bookingRatingId)
        {
            var bookingRating = _unitOfWork.BookingRatingRepository.GetByID(bookingRatingId);
            var booking = _unitOfWork.BookingRepository.GetByID(bookingRating.BookingId);
            var storeService = _unitOfWork.StoreServiceRepository.GetByID(booking.StoreServiceId);
            var service = _unitOfWork.ServiceRepository.GetByID(storeService.ServiceId);

            var allStoreServices = _unitOfWork.StoreServiceRepository.Get(ss => ss.ServiceId == service.Id).ToList();
            var allStoreServiceIds = allStoreServices.Select(ss => ss.Id).ToList();

            // Get all Bookings related to those StoreServices
            var allBookings = _unitOfWork.BookingRepository.Get(b => allStoreServiceIds.Contains(b.StoreServiceId)).ToList();
            var allBookingIds = allBookings.Select(b => b.Id).ToList();

            // Get all BookingRatings for those Bookings
            var allBookingRatings = _unitOfWork.BookingRatingRepository.Get(br => allBookingIds.Contains(br.BookingId)).ToList();

            // Calculate total votes and total count
            var totalVotes = allBookingRatings.Sum(br => br.Vote);
            var totalCount = allBookingRatings.Count;

            // Update TotalRating of the Service
            service.TotalRating = totalCount > 0 ? (float)totalVotes / totalCount : 0f;

            // Save changes to the database
            _unitOfWork.ServiceRepository.Update(service);
            _unitOfWork.Save();
        }
    }
}
