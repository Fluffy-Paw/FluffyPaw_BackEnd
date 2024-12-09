using CoreApiResponse;
using FluffyPaw_Application.DTO.Request.BookingRequest;
using FluffyPaw_Application.DTO.Request.StoreManagerRequest;
using FluffyPaw_Application.DTO.Request.StoreServiceRequest;
using FluffyPaw_Application.DTO.Request.TrackingRequest;
using FluffyPaw_Application.DTO.Response.StoreManagerResponse;
using FluffyPaw_Application.ServiceImplements;
using FluffyPaw_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FluffyPaw_API.Controllers.Staff
{
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : BaseController
    {
        private readonly IStaffService _staffService;

        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        [HttpGet("GetAllServiceByBrandId/{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetAllServiceByBrandId(long id)
        {
            var services = await _staffService.GetAllServiceByBrandId(id);
            return CustomResult("Tải dữ liệu thành công.", services);
        }

        [HttpGet("GetStoreByStaff")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetStoreByStaff()
        {
            var store = await _staffService.GetStoreByStaff();
            return CustomResult("Tải dữ liệu thành công.", store);
        }

        [HttpGet("GetStoreImageById/{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetStoreImageById(long id)
        {
            var image = await _staffService.GetStoreImageById(id);
            return CustomResult("Tải dữ liệu thành công.", image);
        }

        [HttpPatch("UpdateStoreImage")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> UpdateStoreImage(long id, IFormFile file)
        {
            var fileResponse = await _staffService.UpdateStoreImage(id, file);
            return CustomResult("Tải dữ liệu thành công.", fileResponse);
        }

        [HttpDelete("DeleteImage/{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> DeleteImage(long id)
        {
            var status = await _staffService.DeleteImage(id);
            return CustomResult("Tải dữ liệu thành công.", status);
        }

        [HttpGet("GetAllStoreServiceByServiceId/{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetAllStoreServiceByServiceId(long id)
        {
            var storeSers = await _staffService.GetAllStoreServiceByServiceId(id);
            return CustomResult("Tải dữ liệu thành công.", storeSers);
        }

        [HttpPost("CreateStoreService")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> CreateStoreService([FromBody] CreateStoreServiceRequest createStoreServiceRequest)
        {
            var storeServices = await _staffService.CreateStoreService(createStoreServiceRequest);
            return CustomResult("Tạo lịch trình thành công", storeServices);
        }

        [HttpPost("CreateScheduleStoreService")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> CreateScheduleStoreService([FromBody] ScheduleStoreServiceRequest scheduleStoreServiceRequest)
        {
            var storeServices = await _staffService.CreateScheduleStoreService(scheduleStoreServiceRequest);
            return CustomResult("Tạo lịch trình thành công", storeServices);
        }

        [HttpPatch("UpdateStoreService/{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> UpdateStoreService(long id, [FromBody] UpdateStoreServiceRequest updateStoreServiceRequest)
        {
            var storeService = await _staffService.UpdateStoreService(id, updateStoreServiceRequest);
            return CustomResult("Cập nhật lịch trình thành công.");
        }

        [HttpDelete("DeleteStoreService/{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> DeleteStoreService(long id)
        {
            var storeService = await _staffService.DeleteStoreService(id);
            return CustomResult("Xóa lịch trình thành công.", storeService);
        }

        [HttpGet("GetAllBookingByStore")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetAllBookingByStore([FromQuery] FilterBookingRequest filterBookingRequest)
        {
            var bookings = await _staffService.GetAllBookingByStore(filterBookingRequest);
            return CustomResult("Tải dữ liệu thành công.", bookings);
        }

        [HttpGet("GetAllBookingByStoreServiceId/{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetAllBookingByStoreServiceId(long id)
        {
            var bookings = await _staffService.GetAllBookingByStoreServiceId(id);
            return CustomResult("Tải dữ liệu thành công.", bookings);
        }

        [HttpPatch("AcceptBooking/{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> AcceptBooking(long id)
        {
            var booking = await _staffService.AcceptBooking(id);
            return CustomResult("Cập nhật đặt lịch thành công.", booking);
        }

        [HttpPatch("DeniedBooking/{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> DeniedBooking(long id)
        {
            var booking = await _staffService.DeniedBooking(id);
            return CustomResult("Từ chối đặt lịch thành công.", booking);
        }

        [HttpPatch("CancelBooking/{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> CancelBooking(long id)
        {
            var (isSuccess, notice) = await _staffService.CancelBooking(id);
            var responseData = new
            {
                IsSuccess = isSuccess,
                Notice = notice
            };

            return CustomResult("Hủy đặt lịch thành công.", responseData);
        }

        [HttpGet("GetAllTrackingByBookingId/{id}")]
        [Authorize(Roles = "Staff,StoreManager")]
        public async Task<IActionResult> GetAllTrackingByBookingId(long id)
        {
            var trackings = await _staffService.GetAllTrackingByBookingId(id);
            return CustomResult("Tải dữ liệu thành công.", trackings);
        }

        [HttpGet("GetTrackingById/{id}")]
        [Authorize(Roles = "Staff,StoreManager")]
        public async Task<IActionResult> GetTrackingById(long id)
        {
            var tracking = await _staffService.GetTrackingById(id);
            return CustomResult("Tải dữ liệu thành công.", tracking);
        }

        [HttpPost("CreateTracking")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> CreateTracking([FromForm] TrackingRequest trackingRequest)
        {
            var tracking = await _staffService.CreateTracking(trackingRequest);
            return CustomResult("Tạo theo dõi thành công", tracking);
        }

        [HttpPatch("UpdateTracking/{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> UpdateTracking(long id, [FromForm] UpdateTrackingRequest updateTrackingRequest)
        {
            var tracking = await _staffService.UpdateTracking(id, updateTrackingRequest);
            return CustomResult("Cập nhật theo dõi thành công.", tracking);
        }

        [HttpDelete("DeleteTracking/{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> DeleteTracking(long id)
        {
            var tracking = await _staffService.DeleteTracking(id);
            return CustomResult("Xóa theo dõi thành công.", tracking);
        }
    }
}
