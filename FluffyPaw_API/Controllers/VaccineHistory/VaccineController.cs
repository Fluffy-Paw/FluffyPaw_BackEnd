using CoreApiResponse;
using FluffyPaw_Application.DTO.Request.VacineRequest;
using FluffyPaw_Application.ServiceImplements;
using FluffyPaw_Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace FluffyPaw_API.Controllers.VaccineHistory
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccineController : BaseController
    {
        private readonly IVaccineService _vaccineService;

        public VaccineController(IVaccineService vaccineService)
        {
            _vaccineService = vaccineService;
        }

        [HttpGet("GetAllVaccineHistories/{petId}")]
        public async Task<IActionResult> GetAllVaccineHistories(long petId)
        {
            var vaccine = await _vaccineService.GetVaccineHistories(petId);
            return CustomResult("Vaccine list:", vaccine);
        }

        [HttpGet("GetVaccineDetail/{vaccineId}")]
        public async Task<IActionResult> GetVaccineHistorieDetail(long vaccineId)
        {
            var vaccine = await _vaccineService.GetVaccineHistory(vaccineId);
            return CustomResult("Vaccine detail:", vaccine);
        }

        [HttpPost("AddVaccine")]
        public async Task<IActionResult> AddVaccine([FromForm] VaccineRequest vaccineRequest)
        {
            var vaccine = await _vaccineService.AddVaccine(vaccineRequest);
            return CustomResult("Thêm vaccine thành công.", vaccine);
        }

        [HttpPatch("UpdateVaccine/{vaccineId}")]
        public async Task<IActionResult> UpdateVaccine(long vaccineId, [FromForm] VaccineRequest vaccineRequest)
        {
            var vaccine = await _vaccineService.UpdateVaccineHistory(vaccineId, vaccineRequest);
            return CustomResult("Cập nhật vaccine thành công.", vaccine);
        }

        [HttpDelete("DeleteVaccine/{vaccineId}")]
        public async Task<IActionResult> DeleteVaccine(long vaccineId)
        {
            var vaccine = await _vaccineService.RemoveVacine(vaccineId);
            return CustomResult("Xóa vaccine thành công.", vaccine);
        }

        [HttpPatch("CheckoutVaccine/{vaccineId}")]
        public async Task<IActionResult> CheckoutVaccine(long vaccineId)
        {
            var vaccine = await _vaccineService.CheckoutVaccine(vaccineId);
            return CustomResult("Chúc mừng thú cưng đã tiêm vaccine.", vaccine);
        }
    }
}
