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

        [HttpGet("GetAllPets")]
        public async Task<IActionResult> GetAllPets()
        {
            var pet = await _petService.GetAllPetOfUser();
            return CustomResult("Thú cưng của bạn:", pet);
        }

        [HttpGet("GetPet")]
        public async Task<IActionResult> GetPet(long petId)
        {
            var pet = await _petService.GetPet(petId);
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

        [HttpGet("GetAllPetCategory")]
        public async Task<IActionResult> GetAllPetCategory()
        {
            var pet = await _petService.GetAllPetCategory();
            return CustomResult("Loại pet:", pet);
        }

        [HttpGet("GetPetCategory")]
        public async Task<IActionResult> GetPetCategory(long petCategoryId)
        {
            var pet = await _petService.GetPetCategory(petCategoryId);
            return CustomResult("Loại pet:", pet);
        }

        [HttpGet("GetAllPetType")]
        public async Task<IActionResult> GetAllPetType()
        {
            var pet = await _petService.GetAllPetType();
            return CustomResult("Giống loài:", pet);
        }

        [HttpGet("GetPetType")]
        public async Task<IActionResult> GetPetType(long petTypeID)
        {
            var pet = await _petService.GetPetType(petTypeID);
            return CustomResult("Giống loài:", pet);
        }

        [HttpGet("GetAllBehavior")]
        public async Task<IActionResult> GetAllBehavior()
        {
            var pet = await _petService.GetAllBehavior();
            return CustomResult("Sở thích:", pet);
        }

        [HttpGet("GetBehavior")]
        public async Task<IActionResult> GetBehavior(long behaviorId)
        {
            var pet = await _petService.GetBehavior(behaviorId);
            return CustomResult("Sở thích:", pet);
        }
    }
}
