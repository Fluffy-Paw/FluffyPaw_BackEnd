using FluffyPaw_Application.DTO.Response;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.Services
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountResponse>> GetPetOwners();
        Task<IEnumerable<AccountResponse>> GetStoreManagers();
        Task<IEnumerable<AccountResponse>> GetStores();
        Task<IEnumerable<Account>> GetAllAccounts();
        Task<bool> ChangePassword(string oldPassword, string newPassword);
    }
}
