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
        Task<IEnumerable<PetOwner>> GetPetOwners();
        Task<IEnumerable<StoreManager>> GetStoreManagers();
        Task<IEnumerable<StaffAddress>> GetStaffAddresses();
    }
}
