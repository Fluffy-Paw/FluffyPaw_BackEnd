using CoreApiResponse;
using FluffyPaw_Application.DTO.Request.DashboardRequest;
using FluffyPaw_Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FluffyPaw_API.Controllers.Dashboard
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : BaseController
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("GetAllStaticsAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllStaticsAdmin()
        {
            var result = await _dashboardService.GetAllStaticsAdmin();
            return CustomResult(result);
        }

        [HttpGet("GetMonthStaticsAdmin/{month}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetMonthStaticsAdmin(int month)
        {
            var result = await _dashboardService.GetMonthStaticsAdmin(month);
            return CustomResult(result);
        }

        [HttpGet("GetAllStaticsSM")]
        [Authorize(Roles = "StoreManager")]
        public async Task<IActionResult> GetAllStaticsSM()
        {
            var result = await _dashboardService.GetAllStaticsSM();
            return CustomResult(result);
        }

        [HttpGet("GetAllStaticsStaff")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetAllStaticsStaff()
        {
            var result = await _dashboardService.GetAllStaticsStaff();
            return CustomResult(result);
        }

        [HttpPost("GetMonthlyRevenueStaff")]
        [Authorize(Roles = "Staff")]
        public async Task<IActionResult> GetMonthlyRevenueStaff([FromBody]MonthlyRevenueRequest monthlyRevenueRequest)
        {
            var result = await _dashboardService.GetMonthlyRevenueStaff(monthlyRevenueRequest);
            return CustomResult(result);
        }

        [HttpPost("GetMonthlyRevenueSM")]
        [Authorize(Roles = "StoreManager")]
        public async Task<IActionResult> GetMonthlyRevenueSM([FromBody] MonthlyRevenueRequest monthlyRevenueRequest)
        {
            var result = await _dashboardService.GetMonthlyRevenueSM(monthlyRevenueRequest);
            return CustomResult(result);
        }
    }
}
