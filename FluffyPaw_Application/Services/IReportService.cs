using FluffyPaw_Application.DTO.Request.ReportRequest;
using FluffyPaw_Application.DTO.Response.ReportResponse;
using FluffyPaw_Application.DTO.Response.StoreManagerResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.Services
{
    public interface IReportService
    {
        Task<List<ReportResponse>> GetAllReport();
        Task<List<ReportResponse>> GetAllReportByStoreId(long id);
        Task<List<ReportResponse>> GetAllReportByPOId(long id);
        Task<ReportResponse> CreateReport(CreateReportRequest createReportRequest);
        Task<bool> DeleteReport(long id);

    }
}
