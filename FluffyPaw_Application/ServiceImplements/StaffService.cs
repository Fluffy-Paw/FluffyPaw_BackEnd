using AutoMapper;
using FluffyPaw_Application.DTO.Request.StoreServiceRequest;
using FluffyPaw_Application.DTO.Response.FilesResponse;
using FluffyPaw_Application.DTO.Response.StoreManagerResponse;
using FluffyPaw_Application.DTO.Response.StoreServiceResponse;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.CustomException;
using FluffyPaw_Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.ServiceImplements
{
    public class StaffService : IStaffService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthentication _authentication;
        private readonly IHttpContextAccessor _contextAccessor;

        public StaffService(IUnitOfWork unitOfWork, IMapper mapper, IAuthentication authentication, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authentication = authentication;
            _contextAccessor = contextAccessor;
        }

        public async Task<StoreResponse> GetStoreByStaff()
        {
            var staff = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(staff);
            if (account == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy thông tin của Staff.");
            }

            var store = _unitOfWork.StoreRepository.Get(s => s.AccountId == account.Id && s.Status == true).First();
            if (store == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy cửa hàng.");
            }

            var storeResponse = _mapper.Map<StoreResponse>(store);

            var storeFiles = _unitOfWork.StoreFileRepository.Get(sf => sf.StoreId == store.Id, includeProperties: "Files")
                                .Select(sf => sf.Files)
                                .ToList();

            storeResponse.Files = _mapper.Map<List<FileResponse>>(storeFiles);

            return storeResponse;
        }

        public async Task<List<StoreServiceResponse>> CreateStoreService(CreateStoreServiceRequest createStoreServiceRequest)
        {
            var staff = _authentication.GetUserIdFromHttpContext(_contextAccessor.HttpContext);
            var account = _unitOfWork.AccountRepository.GetByID(staff);
            var store = _unitOfWork.StoreRepository.Get(s => s.AccountId == account.Id && s.Status == true).First();
            if (store == null)
            {
                throw new CustomException.DataNotFoundException("Cửa hàng đang bị hạn chế. Hãy thử lại sau.");
            }

            var existingService = _unitOfWork.ServiceRepository.Get(ex => ex.BrandId == store.BrandId
                                                        && ex.Status == true).ToList();
            if (existingService.)
            {
                throw new CustomException.DataNotFoundException("Dịch vụ này chưa được xác thực.");
            }



            var storeServiceResponses = _mapper.Map<List<StoreServiceResponse>>(createStoreServiceRequest);

            return storeServiceResponses;
        }
    }
}
