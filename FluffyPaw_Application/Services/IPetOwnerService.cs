using FluffyPaw_Application.DTO.Request.PetOwnerRequest;
using FluffyPaw_Application.DTO.Response.PetOwnerResponse;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.Services
{
    public interface IPetOwnerService
    {
        Task<PetOwner> UpdatePetOwnerAccount(PetOwnerRequest petOwnerRequest);
        Task<PetOwner> GetPetOwnerDetail(long accountId);
    }
}
