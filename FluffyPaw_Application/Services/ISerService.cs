using FluffyPaw_Application.DTO.Request.ServiceRequest;
using FluffyPaw_Application.DTO.Response.ServiceResponse;

namespace FluffyPaw_Application.Services
{
    public interface ISerService
    {
        Task<List<SerResponse>> GetAllServiceBySM();
        Task<List<SerResponse>> GetAllServiceBySMId(long id);
        Task<List<SerStoResponse>> GetAllServiceByStoreId(long id);
        Task<SerResponse> CreateService(SerRequest serviceRequest);
        Task<UpdateServiceResponse> UpdateService(long id, UpdateServiceRequest updateServiceRequest);
        Task<bool> DeActiveService(long id);
        Task<bool> DeleteService(long id);
    }
}
