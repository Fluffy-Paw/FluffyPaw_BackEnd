﻿using CoreApiResponse;
using FluffyPaw_Application.DTO.Request.AuthRequest;
using FluffyPaw_Application.DTO.Request.StoreManagerRequest;
using FluffyPaw_Application.DTO.Response.StoreManagerResponse;
using FluffyPaw_Application.ServiceImplements;
using FluffyPaw_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FluffyPaw_API.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreManagerController : BaseController
    {
        private readonly IStoreManagerService _storeManagerService;

        public StoreManagerController(IStoreManagerService storeManagerService)
        {
            _storeManagerService = storeManagerService;
        }

        [HttpGet("GetInfo")]
        [Authorize(Roles = "StoreManager")]
        public async Task<IActionResult> GetInfo()
        {
            var profile = await _storeManagerService.GetInfo();
            return CustomResult("Tải dữ liệu thành công.", profile);
        }

        [HttpPatch("UpdateProfile")]
        [Authorize(Roles = "StoreManager")]
        public async Task<IActionResult> UpdateProfile([FromForm] SMAccountRequest smAccountRequest)
        {
            var account = await _storeManagerService.UpdateProfile(smAccountRequest);
            return CustomResult("Cập nhật dữ liệu thành công.", account);
        }

        [HttpGet("GetTotalBooking")]
        [Authorize(Roles = "StoreManager")]
        public async Task<IActionResult> GetTotalBooking()
        {
            var totalBooking = await _storeManagerService.GetTotalBooking();
            return CustomResult("Tải dữ liệu thành công.", totalBooking);
        }

        [HttpGet("GetTotalService")]
        [Authorize(Roles = "StoreManager")]
        public async Task<IActionResult> GetTotalService()
        {
            var totalService = await _storeManagerService.GetTotalService();
            return CustomResult("Tải dữ liệu thành công.", totalService);
        }

        [HttpGet("GetTotalStore")]
        [Authorize(Roles = "StoreManager")]
        public async Task<IActionResult> GetTotalStore()
        {
            var totalStore = await _storeManagerService.GetTotalStore();
            return CustomResult("Tải dữ liệu thành công.", totalStore);
        }

        [HttpGet("GetRevenue")]
        [Authorize(Roles = "StoreManager")]
        public async Task<IActionResult> GetRevenue([FromQuery] RevenueRequest revenueRequest)
        {
            var revenue = await _storeManagerService.GetRevenue(revenueRequest);
            return CustomResult("Tải dữ liệu thành công.", revenue);
        }

        [HttpGet("GetAllBookingByStore")]
        [Authorize(Roles = "StoreManager")]
        public async Task<IActionResult> GetAllBookingByStore(long? id)
        {
            var revenueStore = await _storeManagerService.GetAllBookingByStore(id);
            return CustomResult("Tải dữ liệu thành công.", revenueStore);
        }


        [HttpGet("GetAllBillingRecord")]
        [Authorize(Roles = "StoreManager")]
        public async Task<IActionResult> GetAllBillingRecord()
        {
            var billingRecords = await _storeManagerService.GetAllBillingRecord();
            return CustomResult("Tải dữ liệu thành công.", billingRecords);
        }

        [HttpGet("GetAllStaffBySM")]
        [Authorize(Roles = "StoreManager")]
        public async Task<IActionResult> GetAllStaffBySM()
        {
            var staffs = await _storeManagerService.GetAllStaffBySM();
            return CustomResult("Tải dữ liệu thành công.", staffs);
        }

        [HttpGet("GetAllStoreBySM")]
        [Authorize(Roles = "StoreManager")]
        public async Task<IActionResult> GetAllStoreBySM()
        {
            var stores = await _storeManagerService.GetAllStoreBySM();
            return CustomResult("Tải dữ liệu thành công.", stores);
        }

        [HttpGet("GetAllStoreFalseBySM")]
        [Authorize(Roles = "StoreManager")]
        public async Task<IActionResult> GetAllStoreFalse()
        {
            var stores = await _storeManagerService.GetAllStoreFalseBySM();
            return CustomResult("Tải dữ liệu thành công.", stores);
        }

        [HttpGet("GetAllServiceFalseBySM")]
        [Authorize(Roles = "StoreManager")]
        public async Task<IActionResult> GetAllServiceFalseBySM()
        {
            var services = await _storeManagerService.GetAllServiceFalseBySM();
            return CustomResult("Tải dữ liệu thành công.", services);
        }

        [HttpPost("CreateStore")]
        [Authorize(Roles = "StoreManager")]
        public async Task<IActionResult> CreateStore([FromForm] StoreRequest storeRequest)
        {
            var store = await _storeManagerService.CreateStore(storeRequest);
            return CustomResult("Đăng ký chi nhánh thành công. Vui lòng đợi hệ thống xác thực", store);
        }

        [HttpPatch("UpdateStore/{id}")]
        [Authorize(Roles = "StoreManager")]
        public async Task<IActionResult> UpdateStore(long id, [FromForm] UpdateStoreRequest updateStoreRequest)
        {
            StoreResponse store = await _storeManagerService.UpdateStore(id, updateStoreRequest);
            return CustomResult("Cập nhật chi nhánh thành công.", store);
        }

        [HttpDelete("DeleteStore/{id}")]
        [Authorize(Roles = "Admin,StoreManager")]
        public async Task<IActionResult> DeleteStore(long id)
        {
            var store = await _storeManagerService.DeleteStore(id);
            return CustomResult("Xóa chi nhánh thành công.");
        }

        [HttpPatch("UpdateStaff/{id}")]
        [Authorize(Roles = "StoreManager")]
        public async Task<IActionResult> UpdateStaff(long id, [FromForm] UpdateStaffRequest updateStaffRequest)
        {
            StaffResponse store = await _storeManagerService.UpdateStaff(id, updateStaffRequest);
            return CustomResult("Cập nhật nhân viên thành công.", store);
        }


    }
}
