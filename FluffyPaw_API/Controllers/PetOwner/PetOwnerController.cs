﻿using CoreApiResponse;
using FluffyPaw_Application.DTO.Request.BookingRequest;
using FluffyPaw_Application.DTO.Request.PetOwnerRequest;
using FluffyPaw_Application.DTO.Request.StoreServiceRequest;
using FluffyPaw_Application.ServiceImplements;
using FluffyPaw_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FluffyPaw_API.Controllers.PetOwner
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetOwnerController : BaseController
    {
        private readonly IPetOwnerService _petOwnerService;

        public PetOwnerController(IPetOwnerService petOwnerService)
        {
            _petOwnerService = petOwnerService;
        }

        [HttpGet("GetPetOwnerDetail")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> GetPetOwnerDetail()
        {
            var po = await _petOwnerService.GetPetOwnerDetail();
            return CustomResult("Lấy thông tin thành công.", po);
        }

        [HttpPatch("UpdatePetOwnerAccount")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> UpdatePetOwnerAccount([FromForm] PetOwnerRequest petOwnerRequest)
        {
            var po = await _petOwnerService.UpdatePetOwnerAccount(petOwnerRequest);
            return CustomResult("Cập nhật thành công.", po);
        }

        [HttpGet("GetAllStore")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> GetAllStore()
        {
            var stores = await _petOwnerService.GetAllStore();
            return CustomResult("Tải dữ liệu thành công.", stores);
        }

        [HttpGet("GetAllStoreByBrandId/{id}")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> GetAllStoreByBrandId(long id)
        {
            var stores = await _petOwnerService.GetAllStoreByBrandId(id);
            return CustomResult("Tải dữ liệu thành công.", stores);
        }

        [HttpGet("GetAllStoreByServiceTypeId/{id}")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> GetAllStoreByServiceTypeId(long id)
        {
            var stores = await _petOwnerService.GetAllStoreByServiceTypeId(id);
            return CustomResult("Tải dữ liệu thành công.", stores);
        }

        [HttpGet("GetStoreServiceById/{id}")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> GetStoreServiceById(long id)
        {
            var storeSerResponse = await _petOwnerService.GetStoreServiceById(id);
            return CustomResult("Tải dữ liệu thành công.", storeSerResponse);
        }

        [HttpGet("SuggestionSameTimeAndBrand/{id}")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> SuggestionSameTimeAndBrand(long id)
        {
            var storeSerResponses = await _petOwnerService.SuggestionSameTimeAndBrand(id);
            return CustomResult("Tải dữ liệu thành công.", storeSerResponses);
        }

        [HttpGet("SuggestionSameTime/{id}")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> SuggestionSameTime(long id)
        {
            var storeSerResponses = await _petOwnerService.SuggestionSameTime(id);
            return CustomResult("Tải dữ liệu thành công.", storeSerResponses);
        }

        [HttpGet("GetStoreById/{id}")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> GetStoreById(long id)
        {
            var store = await _petOwnerService.GetStoreById(id);
            return CustomResult("Tải dữ liệu thành công.", store);
        }

        [HttpGet("GetAllStoreServiceByServiceIdStoreId")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> GetAllStoreServiceByServiceIdStoreId(long serviceId, long storeId)
        {
            var storeServices = await _petOwnerService.GetAllStoreServiceByServiceIdStoreId(serviceId, storeId);
            return CustomResult("Tải dữ liệu thành công.", storeServices);
        }

        [HttpGet("GetAllStoreServiceByServiceId/{id}")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> GetAllStoreServiceByServiceId(long id)
        {
            var storeServices = await _petOwnerService.GetAllStoreServiceByServiceId(id);
            return CustomResult("Tải dữ liệu thành công.", storeServices);
        }

        [HttpGet("GetAllBooking")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> GetAllBooking()
        {
            var bookings = await _petOwnerService.GetAllBooking();
            return CustomResult("Tải dữ liệu thành công.", bookings);
        }

        [HttpGet("GetAllBookingByPetId/{id}")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> GetAllBookingByPetId(long id, string? bookingStatus)
        {
            var bookings = await _petOwnerService.GetAllBookingByPetId(id, bookingStatus);
            return CustomResult("Tải dữ liệu thành công.", bookings);
        }

        [HttpPost("CreateBooking")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> CreateBooking(CreateBookingRequest createBookingRequest)
        {
            var booking = await _petOwnerService.CreateBooking(createBookingRequest);
            return CustomResult("Đặt lịch thành công", booking);
        }

        [HttpPost("CreateBookingTimeSelection")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> CreateBookingTimeSelection(TimeSelectionRequest timeSelectionRequest)
        {
            var booking = await _petOwnerService.CreateBookingTimeSelection(timeSelectionRequest);
            return CustomResult("Đặt lịch thành công", booking);
        }

        [HttpPatch("CancelBooking/{id}")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> CancelBooking(long id)
        {
            var booking = await _petOwnerService.CancelBooking(id);
            return CustomResult("Hủy đặt lịch thành công.", booking);
        }

        [HttpGet("GetAllBillingRecord")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> GetAllBillingRecord()
        {
            var billingRecords = await _petOwnerService.GetAllBillingRecord();
            return CustomResult("Tải dữ liệu thành công.", billingRecords);
        }

        [HttpGet("GetAllTrackingByBookingId/{id}")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> GetAllTrackingByBookingId(long id)
        {
            var trackings = await _petOwnerService.GetAllTrackingByBookingId(id);
            return CustomResult("Tải dữ liệu thành công.", trackings);
        }

        [HttpGet("GetTrackingById/{id}")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> GetTrackingById(long id)
        {
            var tracking = await _petOwnerService.GetTrackingById(id);
            return CustomResult("Tải dữ liệu thành công.", tracking);
        }
    }
}
