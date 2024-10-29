using CoreApiResponse;
using FluffyPaw_Application.DTO.Request.BookingRequest;
using FluffyPaw_Application.DTO.Request.ReportRequest;
using FluffyPaw_Application.ServiceImplements;
using FluffyPaw_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FluffyPaw_API.Controllers.Report
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : BaseController
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("GetAllReport")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllReport()
        {
            var reports = await _reportService.GetAllReport();
            return CustomResult("Lấy thông tin thành công.", reports);
        }

        [HttpGet("GetAllReportByStoreId/{id}")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> GetAllReportByStoreId(long id)
        {
            var reports = await _reportService.GetAllReportByStoreId(id);
            return CustomResult("Lấy thông tin thành công.", reports);
        }

        [HttpGet("GetAllReportByPOId/{id}")]
        [Authorize(Roles = "Admin,PetOwner")]
        public async Task<IActionResult> GetAllReportByPOId(long id)
        {
            var reports = await _reportService.GetAllReportByPOId(id);
            return CustomResult("Lấy thông tin thành công.", reports);
        }

        [HttpGet("GetAllReportCategoryName")]
        [Authorize(Roles = "Staff,PetOwner")]
        public async Task<IActionResult> GetAllReportCategoryName()
        {
            var reportCateogories = await _reportService.GetAllReportCategoryName();
            return CustomResult("Lấy thông tin thành công.", reportCateogories);
        }

        [HttpPost("CreateReport")]
        [Authorize(Roles = "Staff,PetOwner")]
        public async Task<IActionResult> CreateReport(CreateReportRequest createReportRequest)
        {
            var report = await _reportService.CreateReport(createReportRequest);
            return CustomResult("Báo cáo thành công", report);
        }

        [HttpPatch("DeleteReport/{id}")]
        [Authorize(Roles = "Staff,PetOwner")]
        public async Task<IActionResult> DeleteReport(long id)
        {
            var report = await _reportService.DeleteReport(id);
            return CustomResult("Xóa báo cáo thành công.");
        }
    }
}
