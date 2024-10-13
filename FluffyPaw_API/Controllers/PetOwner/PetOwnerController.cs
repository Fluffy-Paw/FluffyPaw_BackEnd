using CoreApiResponse;
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

        [HttpGet("GetStoreById/{id}")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> GetStoreById(long id)
        {
            var store = await _petOwnerService.GetStoreById(id);
            return CustomResult("Tải dữ liệu thành công.", store);
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

        [HttpPatch("CancelBooking/{id}")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> CancelBooking(long id)
        {
            var booking = await _petOwnerService.CancelBooking(id);
            return CustomResult("Cập nhật đặt lịch thành công.", booking);
        }
    }
}
