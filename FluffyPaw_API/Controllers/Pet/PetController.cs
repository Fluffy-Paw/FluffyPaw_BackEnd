using Microsoft.AspNetCore.Mvc;
using CoreApiResponse;
using FluffyPaw_Application.Services;
using FluffyPaw_Application.ServiceImplements;
using FluffyPaw_Application.DTO.Request.PetRequest;
using Microsoft.AspNetCore.Authorization;

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

        [HttpGet("GetAllPets")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> GetAllPets()
        {
            var pet = await _petService.GetAllPetOfUser();
            return CustomResult("Thú cưng của bạn:", pet);
        }

        [HttpGet("GetPet/{petId}")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> GetPet(long petId)
        {
            var pet = await _petService.GetPet(petId);
            return CustomResult("Thú cưng của bạn:", pet);
        }

        [HttpPost("AddPet")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> AddPet([FromForm] PetRequest petRequest)
        {
            var pet = await _petService.CreateNewPet(petRequest);
            return CustomResult("Thêm thú cưng thành công.", pet);
        }

        [HttpPatch("UpdatePet/{petId}")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> UpdatePet(long petId, [FromForm] PetRequest petRequest)
        {
            var pet = await _petService.UpdatePet(petId, petRequest);
            return CustomResult("Cập nhật thú cưng thành công.", pet);
        }

        [HttpDelete("DeletePet/{petId}")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> DeletePet(long petId)
        {
            var pet = await _petService.DeletePet(petId);
            return CustomResult("Xóa thú cưng thành công.", pet);
        }

        [HttpGet("GetAllPetTypeByPetCategory/{petCategoryId}")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> GetAllPetTypeByPetCate(long petCategoryId)
        {
            var pet = await _petService.GetAllPetType(petCategoryId);
            return CustomResult("Giống loài:", pet);
        }

        [HttpGet("GetPetType/{petTypeID}")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> GetPetType(long petTypeID)
        {
            var pet = await _petService.GetPetType(petTypeID);
            return CustomResult("Giống loài:", pet);
        }

        [HttpGet("GetAllBehavior")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> GetAllBehavior()
        {
            var pet = await _petService.GetAllBehavior();
            return CustomResult("Sở thích:", pet);
        }

        [HttpGet("GetBehavior/{behaviorId}")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> GetBehavior(long behaviorId)
        {
            var pet = await _petService.GetBehavior(behaviorId);
            return CustomResult("Sở thích:", pet);
        }

        [HttpPatch("ActiveDeactivePet/{petId}")]
        [Authorize(Roles = "PetOwner")]
        public async Task<IActionResult> ActiveDeactivePet(long petId)
        {
            var result = await _petService.ActiveDeactivePet(petId);
            if (result) return CustomResult("Thông tin thú cưng đã kích hoạt.", result);
            else return CustomResult("Thông tin thú cưng đã tắt.", result);
        }
    }
}
