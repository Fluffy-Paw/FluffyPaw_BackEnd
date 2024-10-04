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
        Task<IEnumerable<ListPetResponse>> GetAllPetOfUser();
        Task<PetResponse> GetPet(long petId);
        Task<IEnumerable<PetType>> GetAllPetType(long categoryId);
        Task<IEnumerable<BehaviorCategory>> GetAllBehavior();
        Task<PetType> GetPetType(long petTypeId);
        Task<BehaviorCategory> GetBehavior(long behaviorId);
        Task<bool> ActiveDeactivePet(long petId);
        Task<long> AddBehavior(string Action);
    }
}
