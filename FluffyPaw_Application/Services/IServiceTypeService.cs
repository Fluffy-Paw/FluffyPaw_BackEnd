using FluffyPaw_Application.DTO.Request.ServiceTypeRequest;
using FluffyPaw_Application.DTO.Response.ServiceResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.Services
{
    public interface IServiceTypeService
    {
        IEnumerable<ServiceTypeResponse> GetAllServiceType();
        ServiceTypeResponse GetServiceTypeById(long id);
        Task<ServiceTypeResponse> CreateServiceType(ServiceTypeRequest serviceTypeRequest);
        Task<ServiceTypeResponse> UpdateServiceType(long id, ServiceTypeRequest serviceTypeRequest);
        Task<bool> DeleteServiceType(long id);
    }
}
