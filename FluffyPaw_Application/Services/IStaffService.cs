using FluffyPaw_Application.DTO.Request.StoreServiceRequest;
using FluffyPaw_Application.DTO.Response.StoreManagerResponse;
using FluffyPaw_Application.DTO.Response.StoreServiceResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.Services
{
    public interface IStaffService
    {
        Task<StoreResponse> GetStoreByStaff();
        Task<List<StoreServiceResponse>> CreateStoreService(CreateStoreServiceRequest createStoreServiceRequest);

    }
}
