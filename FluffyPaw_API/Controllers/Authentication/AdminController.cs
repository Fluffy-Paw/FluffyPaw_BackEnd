using CoreApiResponse;
using FluffyPaw_Application.DTO.Request.AdminRequest;
using FluffyPaw_Application.DTO.Request.AuthRequest;
using FluffyPaw_Application.DTO.Request.WalletRequest;
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
        private readonly IAuthService _authService;

        public AdminController(IAdminService adminService, IAuthService authService)
        {
            _adminService = adminService;
            _authService = authService;
        }

        [HttpPost("AdminLogin")]
        public async Task<IActionResult> AdminLogin([FromBody] LoginRequest loginRequest)
        {
            string tuple = await _authService.AdminLogin(loginRequest);
            return CustomResult("Đăng nhập thành công", tuple);
        }

        [HttpPost("CreateAdmin")]
        public async Task<IActionResult> CreateAdmin([FromBody] AdminRequest adminRequest)
        {
            var admin = await _adminService.CreateAdmin(adminRequest);
            return CustomResult("Đăng ký thành công.", admin);
        }

        [HttpGet("GetAllBrandFalse")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllBrandFalse()
        {
            var brandResponse = await _adminService.GetAllBrandFalse();
            return CustomResult("Tải dữ liệu thành công.", brandResponse);
        }

        [HttpPatch("AcceptBrand/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AcceptBrand(long id)
        {
            var brand = await _adminService.AcceptBrand(id);
            return CustomResult("Xác thực hoàn tất.", brand);
        }

        [HttpGet("GetAllServiceFalse")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllServiceFalse()
        {
            var services = await _adminService.GetAllServiceFalse();
            return CustomResult("Tải dữ liệu thành công.", services);
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
            var brandService = await _adminService.AcceptBrandService(id);
            return CustomResult("Xác thực hoàn tất.", brandService);
        }

        [HttpPatch("DeniedBrandService/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeniedBrandService(long id, string description)
        {
            var brandService = await _adminService.DeniedBrandService(id, description);
            return CustomResult("Từ chối hoàn tất.", brandService);
        }

        [HttpGet("GetAllStoreFalse")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllStoreFalse()
        {
            var stores = await _adminService.GetAllStoreFalse();
            return CustomResult("Tải dữ liệu thành công.", stores);
        }

        [HttpGet("GetAllStoreFalseByBrandId/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllStoreFalseByBrandId(long id)
        {
            var stores = await _adminService.GetAllStoreFalseByBrandId(id);
            return CustomResult("Tải dữ liệu thành công.", stores);
        }

        [HttpPatch("AcceptStore/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AcceptStore(long id)
        {
            var store = await _adminService.AcceptStore(id);
            return CustomResult("Xác thực hoàn tất.", store);
        }

        [HttpDelete("DeniedStore/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeniedStore(long id, string description)
        {
            var store = await _adminService.DeniedStore(id, description);
            return CustomResult("Từ chối hoàn tất.", store);
        }

        /*[HttpGet("GetAllAccount")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllAccount()
        {
            var account = _adminService.GetAllAccounts();
            return CustomResult("Tải dữ liệu thành công.", account);
        }*/

        [HttpPatch("ActiveInactiveAccount/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActiveDeactiveAccount(long id)
        {
            var user = await _adminService.ActiveInactiveAccount(id);
            if (user) return CustomResult("Đã chuyển thành Active.");
            else return CustomResult("Đã chuyển thành Inactive");
        }

        [HttpPatch("DowngradeReputation/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DowngradeReputation(long id)
        {
            var po = await _adminService.DowngradeReputation(id);
            if (po == "Ban") return CustomResult("Tài khoản đã bị khóa.");
            return CustomResult($"Uy tín hiện tại của account id {id} là {po}");
        }

        [HttpGet("GetWithdrawRequest")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetWithdrawRequest()
        {
            var requests = await _adminService.GetWithdrawRequest();
            return CustomResult("Tải dữ liệu thành công.", requests);
        }

        [HttpPatch("CheckoutWithdrawRequest/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CheckoutWithdrawRequest(long id)
        {
            var result = await _adminService.CheckoutWithdrawRequest(id);
            return CustomResult(result);
        }

        [HttpPost("DenyWithdrawRequest")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DenyWithdrawRequest([FromBody]DenyWithdrawRequest denyWithdrawRequest)
        {
            var result = await _adminService.DenyWithdrawRequest(denyWithdrawRequest);
            return CustomResult(result);
        }
    }
}
