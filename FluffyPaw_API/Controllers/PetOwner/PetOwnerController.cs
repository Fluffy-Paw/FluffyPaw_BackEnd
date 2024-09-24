using CoreApiResponse;
using FluffyPaw_Application.DTO.Request.PetOwnerRequest;
using FluffyPaw_Application.ServiceImplements;
using FluffyPaw_Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FluffyPaw_API.Controllers.PetOwner
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetOwnerController : BaseController
    {
        private readonly IPetOwnerService _petOwnerService;

        public PetOwnerController(IPetOwnerService petOwnerService)
        {
            _petOwnerService = petOwnerService;
        }

        [HttpGet("GetPetOwnerDetail")]
        public async Task<IActionResult> GetPetOwnerDetail(long id)
        {
            var po = await _petOwnerService.GetPetOwnerDetail(id);
            return CustomResult("Lấy thông tin thành công.", po);
        }

        [HttpPatch("UpdatePetOwnerAccount")]
        public async Task<IActionResult> UpdatePetOwnerAccount(PetOwnerRequest petOwnerRequest)
        {
            var po = await _petOwnerService.UpdatePetOwnerAccount(petOwnerRequest);
            return CustomResult("Cập nhật thành công.", po);
        }


    }
}
