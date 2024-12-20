using FluffyPaw_Application.DTO.Request.BookingRequest;
using FluffyPaw_Application.DTO.Response.BookingResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.Services
{
    public interface IBookingService
    {
        Task<BookingResponse> GetBookingById(long id);
        Task<BookingResponse> Checkin(CheckinRequest checkinRequest);
        Task<List<BookingResponse>> Checkout(CheckOutRequest checkRequest);
        Task<List<BookingRatingResponse>> GetAllBookingRatingByServiceId(long id);
        Task<List<BookingRatingResponse>> GetAllBookingRatingByStoreId(long id);
        Task<BookingRatingResponse> GetBookingRatingByBookingId(long id);
        Task<BookingRatingResponse> GetBookingRatingById(long id);
        Task<BookingRatingResponse> CreateBookingRatingByBookingId(long bookingId, BookingRatingRequest bookingRatingRequest);
        Task<BookingRatingResponse> UpdateBookingRatingById(long id, BookingRatingRequest bookingRatingRequest);
        Task<bool> DeleteBookingRating(long id);
    }
}
