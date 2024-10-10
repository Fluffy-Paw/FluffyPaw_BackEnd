using CoreApiResponse;
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
        public IActionResult GetAllStoreFalse()
        {
            var stores = _storeManagerService.GetAllStoreFalseBySM();
            return CustomResult("Tải dữ liệu thành công.", stores);
        }

        [HttpPost("CreateStore")]
        [Authorize(Roles = "StoreManager")]
        public async Task<IActionResult> CreateStore([FromForm] StoreRequest storeRequest)
        {
            var store = await _storeManagerService.CreateStore(storeRequest);
            return CustomResult("Đăng ký chi nhánh thành công. Vui lòng đợi hệ thống xác thực", store);
        }

        [HttpPatch("UpdateStore")]
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

        [HttpPatch("UpdateStaff")]
        [Authorize(Roles = "StoreManager")]
        public async Task<IActionResult> UpdateStaff(long id, [FromForm] UpdateStaffRequest updateStaffRequest)
        {
            StaffResponse store = await _storeManagerService.UpdateStaff(id, updateStaffRequest);
            return CustomResult("Cập nhật nhân viên thành công.", store);
        }


    }
}
