using FluffyPaw_Application.DTO.Request.VacineRequest;
using FluffyPaw_Application.DTO.Response.VaccineResponse;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.Services
{
    public interface IVaccineService
    {
        Task<bool> AddVaccine(VaccineRequest vaccineRequest);
        
        Task<bool> RemoveVacine(long vaccineId);

        Task<VaccineHistory> CheckoutVaccine(long vaccineId);

        Task<IEnumerable<ListVaccineResponse>> GetVaccineHistories(long petId);

        Task<VaccineHistory> GetVaccineHistory(long vaccineId);

        Task<VaccineHistory> UpdateVaccineHistory(long vaccineId, VaccineRequest vaccineRequest);
    }
}
