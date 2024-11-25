using CoreApiResponse;
using FluffyPaw_Application.DTO.Request.BookingRequest;
using FluffyPaw_Application.DTO.Response.BookingResponse;
using FluffyPaw_Application.ServiceImplements;
using FluffyPaw_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FluffyPaw_API.Controllers.Booking
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : BaseController
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("GetBookingById/{id}")]
        [Authorize(Roles = "Staff,PetOwner")]
        public async Task<IActionResult> GetBookingById(long id)
        {
            var bookings = await _bookingService.GetBookingById(id);
            return CustomResult("Tải dữ liệu thành công.", bookings);
        }

        [HttpPatch("Checkin")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> Checkin(CheckRequest checkRequest)
        {
            var booking = await _bookingService.Checkin(checkRequest);
            return CustomResult("Checkin thành công.", booking);
        }

        [HttpPatch("Checkout")]
        [Authorize(Roles = "Staff,PetOwner")]

        public async Task<IActionResult> Checkout(CheckRequest checkRequest)
        {
            var booking = await _bookingService.Checkout(checkRequest);
            return CustomResult("Checkout thành công.", booking);
        }

        [HttpGet("GetAllBookingRatingByServiceId/{id}")]
        [Authorize(Roles = "Staff,PetOwner")]
        public async Task<IActionResult> GetAllBookingRatingByServiceId(long id)
        {
            var bookingRatings = await _bookingService.GetAllBookingRatingByServiceId(id);
            return CustomResult("Tải dữ liệu thành công.", bookingRatings);
        }

        [HttpGet("GetBookingRatingById/{id}")]
        [Authorize(Roles = "Staff,PetOwner")]
        public async Task<IActionResult> GetBookingRatingById(long id)
        {
            var bookingRating = await _bookingService.GetBookingRatingById(id);
            return CustomResult("Tải dữ liệu thành công.", bookingRating);
        }

        [HttpPost("CreateBookingRatingByBookingId/{bookingId}")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> CreateBookingRatingByBookingId(long bookingId,[FromForm] BookingRatingRequest bookingRatingRequest)
        {
            var bookingRating = await _bookingService.CreateBookingRatingByBookingId(bookingId, bookingRatingRequest);
            return CustomResult("Đánh giá thành công.", bookingRating);
        }

        [HttpPatch("UpdateBookingRatingById/{id}")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> UpdateBookingRatingById(long id,[FromForm] BookingRatingRequest bookingRatingRequest)
        {
            var bookingRating = await _bookingService.UpdateBookingRatingById(id, bookingRatingRequest);
            return CustomResult("Chỉnh sửa đánh giá thành công.", bookingRating);
        }

        [HttpDelete("DeleteBookingRatingById/{id}")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> DeleteBookingRating(long id)
        {
            var bookingRating = await _bookingService.DeleteBookingRating(id);
            return CustomResult("Xoá đánh giá thành công.", bookingRating);
        }
    }
}
