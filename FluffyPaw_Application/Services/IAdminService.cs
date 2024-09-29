using FluffyPaw_Application.DTO.Request.AdminRequest;
using FluffyPaw_Application.DTO.Request.AuthRequest;
using FluffyPaw_Application.DTO.Request.ServiceTypeRequest;
using FluffyPaw_Application.DTO.Response.ServiceResponse;
using FluffyPaw_Application.DTO.Response.ServiceTypeResponse;
using FluffyPaw_Application.DTO.Response.StoreManagerResponse;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.Services
{
    public interface IAdminService
    {
        Task<bool> CreateAdmin(AdminRequest adminRequest);
        IEnumerable<StoreManagerResponse> GetAllStoreManagerFalse();
        Task<bool> AcceptStoreManager(long id);
        Task<List<ServiceResponse>> GetAllServiceFalseBySMId(long id);
        Task<bool> AcceptStoreManagerService(long id);
        Task<bool> ActiveDeactiveAccount(long userId);
        Task<IEnumerable<Account>> GetAllAccounts();
    }
}
    