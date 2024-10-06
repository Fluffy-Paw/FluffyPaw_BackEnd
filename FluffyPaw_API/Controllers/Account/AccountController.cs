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

        //[HttpGet("GetAllAccount")]
        //[Authorize(Roles = "Admin")]
        //public IActionResult GetAllAccount()
        //{
        //    var brands = _accountService.GetAllAccounts();
        //    return CustomResult("Tải dữ liệu thành công.", brands);
        //}

        [HttpGet("GetBrands")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetBrands()
        {
            var brands = _accountService.GetBrands();
            return CustomResult("Danh sách Brand:", brands);
        }

        [HttpGet("GetStores")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetStores()
        {
            var stores = _accountService.GetStores();
            return CustomResult("Danh sách Store:", stores);
        }

        [HttpGet("GetStoresByBrandId/{brandId}")]
        [Authorize(Roles = "Admin,StoreManager")]
        public IActionResult GetStoresByBrandId(long brandId)
        {
            var stores = _accountService.GetStores();
            return CustomResult("Danh sách Store:", stores);
        }

        [HttpGet("GetPetOwners")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetPetOwners()
        {
            var account = _accountService.GetPetOwners();
            return CustomResult("Danh sách PO:", account);
        }

        [HttpPatch("UpdatePassword")]
        [Authorize]
        public IActionResult UpdatePassword(string oldPassword, string newPassword)
        {
            _accountService.ChangePassword(oldPassword, newPassword);
            return CustomResult("Cập nhật mật khẩu thành công");
        }
    }
}
