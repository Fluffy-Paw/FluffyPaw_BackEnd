using FluffyPaw_Application.DTO.Request.ServiceRequest;
using FluffyPaw_Application.DTO.Request.StoreManagerRequest;
using FluffyPaw_Application.DTO.Response;
using FluffyPaw_Application.DTO.Response.BookingResponse;
using FluffyPaw_Application.DTO.Response.BrandResponse;
using FluffyPaw_Application.DTO.Response.ServiceResponse;
using FluffyPaw_Application.DTO.Response.StoreManagerResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.Services
{
    public interface IStoreManagerService
    {
        Task<BrandResponse> GetInfo();
        Task<SMAccountResponse> UpdateProfile(SMAccountRequest smAccountRequest);
        Task<int> GetTotalBooking();
        Task<int> GetTotalService();
        Task<int> GetTotalStore();
        Task<double> GetRevenue(RevenueRequest revenueRequest);
        Task<List<RevenueBookingResponse>> GetAllBookingByStore(long? id);
        Task<List<BillingRecordResponse>> GetAllBillingRecord();
        Task<List<StaffResponse>> GetAllStaffBySM();
        Task<List<StoreResponse>> GetAllStoreBySM();
        Task<List<StoreResponse>> GetAllStoreFalseBySM();
        Task<List<SerResponse>> GetAllServiceFalseBySM();
        Task<StoreResponse> CreateStore(StoreRequest storeRequest);
        Task<StoreResponse> UpdateStore(long id, UpdateStoreRequest updateStoreRequest);
        Task<bool> DeleteStore(long id);
        Task<StaffResponse> UpdateStaff(long id, UpdateStaffRequest updateStaffRequest);
    }
}
