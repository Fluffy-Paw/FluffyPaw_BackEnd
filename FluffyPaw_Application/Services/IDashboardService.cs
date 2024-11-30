using FluffyPaw_Application.DTO.Response.DasboardResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.Services
{
    public interface IDashboardService
    {
        Task<SMDashboardResponse> GetMonthStaticsSM(int month);
        
        Task<SMDashboardResponse> GetAllStaticsSM();

        Task<AdminDashboardResponse> GetMonthStaticsAdmin(int month);

        Task<AdminDashboardResponse> GetAllStaticsAdmin();

        Task<SMDashboardResponse> GetAllStaticsStaff();
    }
}
