using FluffyPaw_Application.DTO.Request.PetRequest;
using FluffyPaw_Application.DTO.Response.PetResponse;
using FluffyPaw_Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.Services
{
    public interface IPetService
    {
        Task<bool> CreateNewPet(PetRequest petRequest);
        Task<PetResponse> UpdatePet(long petId, PetRequest pet);
        Task<bool> DeletePet(long petId);
        Task<IEnumerable<PetResponse>> GetAllPetOfUser();
    }
}
