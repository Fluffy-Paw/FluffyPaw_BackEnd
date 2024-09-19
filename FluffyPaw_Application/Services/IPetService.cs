using FluffyPaw_Application.DTO.Request.PetRequest;
using FluffyPaw_Application.DTO.Response.PetResponse;
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
        Task<bool> UpdatePet(PetRequest petRequest);
        Task<bool> DeletePet(long petId);
        Task<IEnumerable<PetResponse>> GetAllPetOfUser(long userId);

    }
}
