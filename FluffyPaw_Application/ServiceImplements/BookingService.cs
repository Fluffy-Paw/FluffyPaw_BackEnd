using AutoMapper;
using FluffyPaw_Application.DTO.Request.BookingRequest;
using FluffyPaw_Application.DTO.Response.BookingResponse;
using FluffyPaw_Application.DTO.Response.StoreServiceResponse;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.CustomException;
using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Interfaces;
using FluffyPaw_Domain.Utils;
using FluffyPaw_Repository.Enum;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper, IAuthentication authentication,
                            IHttpContextAccessor httpContextAccessor, IJobScheduler jobScheduler,
                            INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authentication = authentication;
            _httpContextAccessor = httpContextAccessor;
            _jobScheduler = jobScheduler;
            _notificationService = notificationService;
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

        public async Task<List<BookingResponse>> Checkin(CheckRequest checkRequest)
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

            foreach (var id in checkRequest.Id)
            {
                var booking = _unitOfWork.BookingRepository.Get(b => b.Id == id && b.Status == BookingStatus.Accepted.ToString(),
                                                includeProperties: "StoreService,StoreService.Service").FirstOrDefault();
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

                var bookingResponse = _mapper.Map<BookingResponse>(booking);
                bookingResponse.CheckinTime = CoreHelper.SystemTimeNow;
                bookingResponses.Add(bookingResponse);
            }

            await _unitOfWork.SaveAsync();

            return bookingResponses;
        }

        public async Task<List<BookingResponse>> Checkout(CheckRequest checkRequest)
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

            foreach (var id in checkRequest.Id)
            {
                var booking = _unitOfWork.BookingRepository.Get(b => b.Id == id, includeProperties: "StoreService,StoreService.Service").FirstOrDefault();
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
                    booking.CheckOutTime = CoreHelper.SystemTimeNow;
                }

                else
                {
                    if (booking.Status != BookingStatus.Ended.ToString())
                    {
                        throw new CustomException.InvalidDataException("Đặt lịch này cần phải kết thúc trước khi check out.");
                    }

                    booking.CheckOut = true;
                    booking.CheckOutTime = CoreHelper.SystemTimeNow;
                }

                _unitOfWork.BookingRepository.Update(booking);

                var bookingResponse = _mapper.Map<BookingResponse>(booking);
                bookingResponse.CheckOutTime = CoreHelper.SystemTimeNow;
                bookingResponses.Add(bookingResponse);
            }

            await _unitOfWork.SaveAsync();

            return bookingResponses;
        }
    }
}
