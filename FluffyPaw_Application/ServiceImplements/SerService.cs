using AutoMapper;
using FluffyPaw_Application.DTO.Request.ServiceRequest;
using FluffyPaw_Application.DTO.Request.ServiceTypeRequest;
using FluffyPaw_Application.DTO.Response.ServiceResponse;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.CustomException;
using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.ServiceImplements
{
    public class SerService : ISerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthentication _authentication;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SerService(IUnitOfWork unitOfWork, IMapper mapper, IAuthentication authentication, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authentication = authentication;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<SerResponse>> GetAllServiceBySM()
        {
            var accountId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);

            var BrandId = _unitOfWork.BrandRepository.Get(sm => sm.AccountId == accountId).FirstOrDefault();

            var storeService = _unitOfWork.ServiceRepository.Get(ss => ss.BrandId == BrandId.Id,
                includeProperties: "Certificate").ToList();

            if (!storeService.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy dịch vụ của doanh nghiệp");
            }

            var serviceType = _unitOfWork.ServiceTypeRepository.GetByID(storeService.First().ServiceTypeId);

            var serviceResponse = _mapper.Map<List<SerResponse>>(storeService);
            foreach( SerResponse serResponse in serviceResponse )
            {
                serResponse.ServiceTypeName = serviceType.Name;
            }

            return serviceResponse;
        }

        public async Task<List<SerResponse>> GetAllServiceBySMId(long id)
        {
            var storeService = _unitOfWork.ServiceRepository.Get(ss => ss.BrandId == id).ToList();

            if (storeService == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy dịch vụ của doanh nghiệp");
            }

            var serviceResponse = _mapper.Map<List<SerResponse>>(storeService);
            return serviceResponse;
        }

        public async Task<SerResponse> CreateService(SerRequest serviceRequest)
        {
            var accountId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);

            var BrandId = _unitOfWork.BrandRepository.Get(sm => sm.AccountId == accountId).FirstOrDefault();

            var existingService = _unitOfWork.ServiceRepository.Get(p => p.Name.ToLower() == serviceRequest.Name.ToLower() 
            && p.BrandId == BrandId.Id).FirstOrDefault();

            if (existingService != null)
            {
                throw new CustomException.DataExistException($"Loại dịch vụ '{serviceRequest.Name}' đã tồn tại.");
            }

            var newService = _mapper.Map<Service>(serviceRequest);
            newService.BrandId = BrandId.Id;

            _unitOfWork.ServiceRepository.Insert(newService);
            await _unitOfWork.SaveAsync();
            foreach ( var certificate in serviceRequest.CertificateDtos)
            {
                var newCertificate = _mapper.Map<Certificate>(certificate);
                _unitOfWork.CertificateRepository.Insert(newCertificate);
            }

            await _unitOfWork.SaveAsync();

            var serviceResponse = _mapper.Map<SerResponse>(newService);

            return serviceResponse;
        }

        public async Task<UpdateServiceResponse> UpdateService(long id, UpdateServiceRequest updateServiceRequest)
        {
            var existingService = _unitOfWork.ServiceRepository.GetByID(id);

            if (existingService == null)
            {
                throw new CustomException.DataNotFoundException($"Không tìm thấy dịch vụ.");
            }

            _mapper.Map(updateServiceRequest, existingService);
            existingService.Status = false;
            _unitOfWork.Save();

            var serviceResponse = _mapper.Map<UpdateServiceResponse>(existingService);
            return serviceResponse;
        }

        public async Task<bool> DeleteService(long id)
        {
            var service = _unitOfWork.ServiceRepository.GetByID(id);
            if (service == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy dịch vụ.");
            }

            _unitOfWork.ServiceRepository.Delete(service);

            _unitOfWork.Save();

            return true;
        }
    }
}
