using CoreApiResponse;
using FluffyPaw_Application.DTO.Request.AdminRequest;
using FluffyPaw_Application.DTO.Request.AuthRequest;
using FluffyPaw_Application.DTO.Response.StoreManagerResponse;
using FluffyPaw_Application.ServiceImplements;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FluffyPaw_API.Controllers.Authentication
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : BaseController
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("CreateAdmin")]
        public async Task<IActionResult> CreateAdmin([FromBody] AdminRequest adminRequest)
        {
            var admin = await _adminService.CreateAdmin(adminRequest);
            return CustomResult("Đăng ký thành công.", admin);
        }

        [HttpGet("GetAllStoreManagerFalse")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllStoreManagerFalse()
        {
            var storeManagerResponse = _adminService.GetAllStoreManagerFalse();
            return CustomResult("Tải dữ liệu thành công.", storeManagerResponse);
        }

        [HttpPatch("AcceptStoreManager/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AcceptStoreManager(long id)
        {
            var storemanager = await _adminService.AcceptStoreManager(id);
            return CustomResult("Xác thực hoàn tất.", storemanager);
        }

        [HttpPatch("ActiveDeactiveAccount/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActiveDeactiveAccount(long id)
        {
            var storemanager = await _adminService.ActiveDeactiveAccount(id);
            if (storemanager) return CustomResult("Đã chuyển thành Active.");
            else return CustomResult("Đã chuyển thành Deactive");
        }
    }
}
