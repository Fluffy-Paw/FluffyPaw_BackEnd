using CoreApiResponse;
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

        [HttpGet("GetAllAccount")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllAccount()
        {
            var account = _accountService.GetAllAccounts();
            return CustomResult("Tải dữ liệu thành công.", account);
        }

        [HttpGet("GetStoreManagers")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetStoreManagers()
        {
            var account = _accountService.GetStoreManagers();
            return CustomResult("Tải dữ liệu thành công.", account);
        }

        [HttpGet("GetStaffAddresses")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetStaffAddresses()
        {
            var account = _accountService.GetStaffAddresses();
            return CustomResult("Tải dữ liệu thành công.", account);
        }

        [HttpGet("GetPetOwners")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetPetOwners()
        {
            var account = _accountService.GetPetOwners();
            return CustomResult("Tải dữ liệu thành công.", account);
        }

        [HttpPatch("UpdatePassword")]
        public IActionResult UpdatePassword(string oldPassword, string newPassword)
        {
            _accountService.ChangePassword(oldPassword, newPassword);
            return CustomResult("Cập nhật mật khẩu thành công");
        }
    }
}
