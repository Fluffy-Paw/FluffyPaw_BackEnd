using CoreApiResponse;
using FluffyPaw_Application.DTO.Request.ServiceRequest;
using FluffyPaw_Application.DTO.Request.ServiceTypeRequest;
using FluffyPaw_Application.DTO.Response.ServiceResponse;
using FluffyPaw_Application.ServiceImplements;
using FluffyPaw_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FluffyPaw_API.Controllers.Service
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : BaseController
    {
        private readonly ISerService _SerService;

        public ServiceController(ISerService SerService)
        {
            _SerService = SerService;
        }

        [HttpGet("GetAllServiceBySM")]
        [Authorize(Roles = "StoreManager")]
        public async Task<IActionResult> GetAllServiceBySM()
        {
            var services = await _SerService.GetAllServiceBySM();
            return CustomResult("Tải dữ liệu thành công.", services);
        }


        [HttpGet("GetAllServiceBySMId/{id}")]
        [Authorize(Roles = "Staff")]
        public IActionResult GetAllServiceBySMId(long id)
        {
            var services = _SerService.GetAllServiceBySMId(id);
            return CustomResult("Tải dữ liệu thành công.", services);
        }

        [HttpPost("CreateService")]
        [Authorize(Roles = "StoreManager")]
        public async Task<IActionResult> CreateService([FromForm] SerRequest serviceRequest)
        {
            SerResponse service = await _SerService.CreateService(serviceRequest);
            return CustomResult("Tạo dịch vụ thành công. Vui lòng chờ hệ thống xác thực", service);
        }

        [HttpPatch("UpdateService/{id}")]
        [Authorize(Roles = "StoreManager")]
        public async Task<IActionResult> UpdateService(long id, [FromForm] UpdateServiceRequest updateServiceRequest)
        {
            UpdateServiceResponse service = await _SerService.UpdateService(id, updateServiceRequest);
            return CustomResult("Cập nhật dịch vụ thành công. Vui lòng chờ hệ thống xác thực", service);
        }

        [HttpDelete("DeleteService/{id}")]
        [Authorize(Roles = "Admin,StoreManager")]
        public async Task<IActionResult> DeleteService(long id)
        {
            var serviceType = await _SerService.DeleteService(id);
            return CustomResult("Xóa dịch vụ thành công.");
        }
    }
}
