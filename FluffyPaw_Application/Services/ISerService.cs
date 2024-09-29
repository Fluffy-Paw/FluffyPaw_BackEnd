using FluffyPaw_Application.DTO.Request.ServiceRequest;
using FluffyPaw_Application.DTO.Request.ServiceTypeRequest;
using FluffyPaw_Application.DTO.Response.ServiceResponse;
using FluffyPaw_Application.DTO.Response.ServiceTypeResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.Services
{
    public interface ISerService
    {
        Task<List<ServiceResponse>> GetAllServiceBySM();
        Task<List<ServiceResponse>> GetAllServiceBySMId(long id);
        Task<ServiceResponse> CreateService(ServiceRequest serviceRequest);
        Task<UpdateServiceResponse> UpdateService(long id, UpdateServiceRequest updateServiceRequest);
        Task<bool> DeleteService(long id);
    }
}
