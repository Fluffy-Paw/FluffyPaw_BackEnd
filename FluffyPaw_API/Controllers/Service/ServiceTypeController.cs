using CoreApiResponse;
using FluffyPaw_Application.DTO.Request.ServiceRequest;
using FluffyPaw_Application.DTO.Response.ServiceResponse;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.Entities;
using FluffyPaw_Repository.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FluffyPaw_API.Controllers.Service
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceTypeController : BaseController
    {
        private readonly IServiceTypeService _serviceTypeService;

        public ServiceTypeController(IServiceTypeService serviceTypeService)
        {
            _serviceTypeService = serviceTypeService;
        }

        [HttpGet("GetAllServiceType")]
        public IActionResult GetAllServiceType() 
        {
            var serviceTypes = _serviceTypeService.GetAllServiceType();
            return CustomResult("Tải dữ liệu thành công.", serviceTypes);
        }

        [HttpGet("GetServiceTypeById/{id}")]
        public IActionResult GetServiceTypeById(long id)
        {
            var serviceType = _serviceTypeService.GetServiceTypeById(id);
            return CustomResult("Tải dữ liệu thành công.", serviceType);
        }

        [HttpPost("CreateServiceType")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateServiceType(ServiceTypeRequest serviceTypeRequest)
        {
            ServiceTypeResponse serviceType = await _serviceTypeService.CreateServiceType(serviceTypeRequest);
            return CustomResult("Tạo loại dịch vụ thành công.", serviceType);
        }

        [HttpPatch("UpdateServiceType/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateServiceType(long id, ServiceTypeRequest serviceTypeRequest)
        {
            var serviceType = await _serviceTypeService.UpdateServiceType(id, serviceTypeRequest);
            return CustomResult("Cập nhật loại dịch vụ thành công.", serviceType);
        }

        [HttpDelete("DeleteServiceType/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteServiceType(long id)
        {
            var serviceType = await _serviceTypeService.DeleteServiceType(id);
            return CustomResult("Xóa loại dịch vụ thành công.");
        }
    }
}
