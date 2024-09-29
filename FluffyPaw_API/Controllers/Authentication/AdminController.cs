using CoreApiResponse;
using FluffyPaw_Application.DTO.Request.AdminRequest;
using FluffyPaw_Application.DTO.Request.AuthRequest;
using FluffyPaw_Application.DTO.Response.BrandResponse;
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

        [HttpGet("GetAllBrandFalse")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllBrandFalse()
        {
            var BrandResponse = _adminService.GetAllBrandFalse();
            return CustomResult("Tải dữ liệu thành công.", BrandResponse);
        }

        [HttpPatch("AcceptBrand/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AcceptBrand(long id)
        {
            var Brand = await _adminService.AcceptBrand(id);
            return CustomResult("Xác thực hoàn tất.", Brand);
        }

        [HttpGet("GetAllServiceFalseByBrandId/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllServiceFalseByBrandId(long id)
        {
            var services = await _adminService.GetAllServiceFalseByBrandId(id);
            return CustomResult("Tải dữ liệu thành công.", services);
        }

        [HttpPatch("AcceptBrandService/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AcceptBrandService(long id)
        {
            var BrandService = await _adminService.AcceptBrandService(id);
            return CustomResult("Xác thực hoàn tất.", BrandService);
        }

        /*[HttpGet("GetAllAccount")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllAccount()
        {
            var account = _adminService.GetAllAccounts();
            return CustomResult("Tải dữ liệu thành công.", account);
        }*/

        [HttpPatch("ActiveDeactiveAccount/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActiveDeactiveAccount(long id)
        {
            var Brand = await _adminService.ActiveDeactiveAccount(id);
            if (Brand) return CustomResult("Đã chuyển thành Active.");
            else return CustomResult("Đã chuyển thành Deactive");
        }
    }
}
