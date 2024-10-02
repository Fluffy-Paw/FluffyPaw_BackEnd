using FluffyPaw_Application.DTO.Request.ServiceRequest;
using FluffyPaw_Application.DTO.Request.ServiceTypeRequest;
using FluffyPaw_Application.DTO.Response.ServiceResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.Services
{
    public interface ISerService
    {
        Task<List<SerResponse>> GetAllServiceBySM();
        Task<List<SerResponse>> GetAllServiceBySMId(long id);
        Task<SerResponse> CreateService(SerRequest serviceRequest);
        Task<UpdateServiceResponse> UpdateService(long id, UpdateServiceRequest updateServiceRequest);
        Task<bool> DeleteService(long id);
    }
}
