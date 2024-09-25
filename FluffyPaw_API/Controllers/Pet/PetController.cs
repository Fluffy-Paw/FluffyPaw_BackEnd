using Microsoft.AspNetCore.Mvc;
using CoreApiResponse;
using FluffyPaw_Application.Services;
using FluffyPaw_Application.ServiceImplements;
using FluffyPaw_Application.DTO.Request.PetRequest;

namespace FluffyPaw_API.Controllers.Pet
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetController : BaseController
    {
        private readonly IPetService _petService;

        public PetController(IPetService petService)
        {
            _petService = petService;
        }

        [HttpGet("GetPet")]
        public async Task<IActionResult> GetPet(long ownerId)
        {
            var pet = await _petService.GetAllPetOfUser(ownerId);
            return CustomResult("Thú cưng của bạn:", pet);
        }

        [HttpPost("AddPet")]
        public async Task<IActionResult> AddPet([FromBody] PetRequest petRequest)
        {
            var pet = await _petService.CreateNewPet(petRequest);
            return CustomResult("Thêm thú cưng thành công.", pet);
        }

        [HttpPatch("UpdatePet")]
        public async Task<IActionResult> UpdatePet(long petId, [FromBody] PetRequest petRequest)
        {
            var pet = await _petService.UpdatePet(petId, petRequest);
            return CustomResult("Cập nhật thú cưng thành công.", pet);
        }

        [HttpDelete("DeletePet")]
        public async Task<IActionResult> DeletePet(long petId)
        {
            var pet = await _petService.DeletePet(petId);
            return CustomResult("Xóa thú cưng thành công.", pet);
        }

    }
}
