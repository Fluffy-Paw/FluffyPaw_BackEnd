using CoreApiResponse;
using FluffyPaw_Application.DTO.Request.AuthRequest;
using FluffyPaw_Application.ServiceImplements;
using FluffyPaw_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FluffyPaw_API.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        //[HttpGet("GetAllAccount")]
        //[Authorize(Roles = "Admin")]
        //public IActionResult GetAllAccount()
        //{
        //    var brands = _accountService.GetAllAccounts();
        //    return CustomResult("Tải dữ liệu thành công.", brands);
        //}

        [HttpGet("GetBrands")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetBrands()
        {
            var brands = await _accountService.GetBrands();
            return CustomResult("Danh sách Brand:", brands);
        }

        [HttpGet("GetStores")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetStores()
        {
            var stores = await _accountService.GetStores();
            return CustomResult("Danh sách Store:", stores);
        }

        [HttpGet("GetStoresByBrandId/{brandId}")]
        [Authorize(Roles = "Admin,StoreManager")]
        public async Task<IActionResult> GetStoresByBrandId(long brandId)
        {
            var stores = await _accountService.GetStores();
            return CustomResult("Danh sách Store:", stores);
        }

        [HttpGet("GetPetOwners")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPetOwners()
        {
            var account = await _accountService.GetPetOwners();
            return CustomResult("Danh sách PO:", account);
        }

        [HttpPatch("UpdatePassword")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword([FromBody]UpdatePasswordRequest updatePasswordRequest)
        {
            await _accountService.ChangePassword(updatePasswordRequest.OldPassword, updatePasswordRequest.NewPassword);
            return CustomResult("Cập nhật mật khẩu thành công.");
        }

        [HttpPatch("ForgotPassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] LoginRequest ForgotPasswordRequest)
        {
            await _accountService.ForgotPassword(ForgotPasswordRequest);
            return CustomResult("Cập nhật mật khẩu thành công.");
        }
    }
}
