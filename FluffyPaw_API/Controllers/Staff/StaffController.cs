using CoreApiResponse;
using FluffyPaw_Application.DTO.Request.StoreManagerRequest;
using FluffyPaw_Application.DTO.Request.StoreServiceRequest;
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

        [HttpGet("GetStoreByStaff")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetStoreByStaff()
        {
            var store = await _staffService.GetStoreByStaff();
            return CustomResult("Tải dữ liệu thành công.", store);
        }

        [HttpPost("CreateStoreService")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> CreateStoreService([FromBody] CreateStoreServiceRequest createStoreServiceRequest)
        {
            var storeServices = await _staffService.CreateStoreService(createStoreServiceRequest);
            return CustomResult("Tạo lịch trình thành công", storeServices);
        }

        [HttpPatch("UpdateStoreService/{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> UpdateStoreService(long id, [FromBody] UpdateStoreServiceRequest updateStoreServiceRequest)
        {
            var storeService = await _staffService.UpdateStoreService(id, updateStoreServiceRequest);
            return CustomResult("Cập nhật lịch trình thành công.", storeService);
        }

        [HttpDelete("DeleteStoreService/{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> DeleteStoreService(long id)
        {
            var storeService = await _staffService.DeleteStoreService(id);
            return CustomResult("Xóa lịch trình thành công.");
        }

        [HttpGet("GetAllBookingByStoreServiceId/{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetAllBookingByStoreServiceId(long id)
        {
            var bookings = await _staffService.GetAllBookingByStoreServiceId(id);
            return CustomResult("Tải dữ liệu thành công.", bookings);
        }

        [HttpPatch("AcceptBooking")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> AcceptBooking(long id)
        {
            var booking = await _staffService.AcceptBooking(id);
            return CustomResult("Cập nhật đặt lịch thành công.", booking);
        }

        [HttpPatch("DeniedBooking")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> DeniedBooking(long id)
        {
            var booking = await _staffService.DeniedBooking(id);
            return CustomResult("Cập nhật đặt lịch thành công.", booking);
        }
    }
}
