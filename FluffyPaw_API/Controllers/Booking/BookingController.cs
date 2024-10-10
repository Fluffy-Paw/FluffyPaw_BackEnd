using CoreApiResponse;
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
    }
}
