using CoreApiResponse;
using FluffyPaw_Application.DTO.Request.StoreManagerRequest;
using FluffyPaw_Application.DTO.Request.StoreServiceRequest;
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
    }
}
