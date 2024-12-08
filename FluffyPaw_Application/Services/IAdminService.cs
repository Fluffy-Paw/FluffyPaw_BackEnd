using FluffyPaw_Application.DTO.Request.AdminRequest;
using FluffyPaw_Application.DTO.Request.AuthRequest;
using FluffyPaw_Application.DTO.Request.ServiceRequest;
using FluffyPaw_Application.DTO.Response.ServiceResponse;
using FluffyPaw_Application.DTO.Response.BrandResponse;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluffyPaw_Application.DTO.Response.StoreManagerResponse;
using FluffyPaw_Application.DTO.Response.NotificationResponse;
using FluffyPaw_Application.DTO.Request.WalletRequest;

namespace FluffyPaw_Application.Services
{
    public interface IAdminService
    {
        Task<bool> CreateAdmin(AdminRequest adminRequest);
        Task<List<BrandResponse>> GetAllBrandFalse();
        Task<bool> AcceptBrand(long id);
        Task<List<SerResponse>> GetAllService();
        Task<List<SerResponse>> GetAllServiceFalse();
        Task<List<SerResponse>> GetAllServiceFalseByBrandId(long id);
        Task<bool> AcceptBrandService(long id);
        Task<bool> DeniedBrandService(long id, string description);
        Task<List<StoreResponse>> GetAllStoreFalse();
        Task<List<StoreResponse>> GetAllStoreFalseByBrandId(long id);
        Task<bool> AcceptStore(long id);
        Task<bool> DeniedStore(long id, string description);
        Task<bool> ActiveInactiveAccount(long userId);
        Task<string> DowngradeReputation(long userId);
        Task<List<WithdrawNotificationResponse>> GetWithdrawRequest();
        Task<bool> CheckoutWithdrawRequest(long id);
        Task<bool> DenyWithdrawRequest(DenyWithdrawRequest denyWithdrawRequest);
    }
}
    