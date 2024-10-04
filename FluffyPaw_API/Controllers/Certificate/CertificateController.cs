using CoreApiResponse;
using FluffyPaw_Application.DTO.Request.CertificateRequest;
using FluffyPaw_Application.DTO.Request.ServiceRequest;
using FluffyPaw_Application.DTO.Response.CertificateResponse;
using FluffyPaw_Application.DTO.Response.ServiceResponse;
using FluffyPaw_Application.ServiceImplements;
using FluffyPaw_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FluffyPaw_API.Controllers.Certificate
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateController : BaseController
    {
        private readonly ICertificateService _certificateService;

        public CertificateController(ICertificateService certificateService)
        {
            _certificateService = certificateService;
        }

        [HttpGet("GetAllCertificateByServiceId/{id}")]
        [Authorize(Roles = "StoreManager,Staff,PetOwner")]
        public async Task<IActionResult> GetAllCertificateByServiceId(long id)
        {
            var services = await _certificateService.GetAllCertificateByServiceId(id);
            return CustomResult("Tải dữ liệu thành công.", services);
        }

        [HttpPost("CreateCertificate")]
        [Authorize(Roles = "StoreManager")]
        public async Task<IActionResult> CreateCertificate([FromForm] CreateCertificateRequest certificateRequest)
        {
            CertificatesResponse certificatesResponse = await _certificateService.CreateCertificate(certificateRequest);
            return CustomResult("Tạo certificate thành công.", certificatesResponse);
        }

        [HttpPatch("UpdateCertificate/{id}")]
        [Authorize(Roles = "StoreManager")]
        public async Task<IActionResult> UpdateCertificate(long id, [FromForm] UpdateCertificateRequest updateCertificateRequest)
        {
            CertificatesResponse certificate = await _certificateService.UpdateCertificate(id, updateCertificateRequest);
            return CustomResult("Cập nhật certificate thành công.", certificate);
        }

        [HttpDelete("DeleteCertificate/{id}")]
        [Authorize(Roles = "StoreManager")]
        public async Task<IActionResult> DeleteCertificate(long id)
        {
            var certificate = await _certificateService.DeleteCertificate(id);
            return CustomResult("Xóa certificate thành công.");
        }
    }
}
