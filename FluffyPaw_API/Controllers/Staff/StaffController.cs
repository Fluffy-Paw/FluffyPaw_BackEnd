﻿using CoreApiResponse;
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

        [HttpGet("GetAllBookingByStore")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetAllBookingByStore()
        {
            var bookings = await _staffService.GetAllBookingByStore();
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
            return CustomResult("Cập nhật đặt lịch thành công.", booking);
        }

        [HttpGet("GetAllTrackingByBookingId/{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetAllTrackingByBookingId(long id)
        {
            var trackings = await _staffService.GetAllTrackingByBookingId(id);
            return CustomResult("Tải dữ liệu thành công.", trackings);
        }

        [HttpGet("GetTrackingById/{id}")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetTrackingById(long id)
        {
            var trackings = await _staffService.GetTrackingById(id);
            return CustomResult("Tải dữ liệu thành công.", trackings);
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
            return CustomResult("Xóa theo dõi thành công.");
        }
    }
}
