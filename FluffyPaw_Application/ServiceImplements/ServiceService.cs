using AutoMapper;
using FluffyPaw_Application.DTO.Request.ServiceRequest;
using FluffyPaw_Application.DTO.Request.ServiceTypeRequest;
using FluffyPaw_Application.DTO.Response.ServiceResponse;
using FluffyPaw_Application.DTO.Response.ServiceTypeResponse;
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
    public class ServiceService : IServiceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthentication _authentication;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ServiceService(IUnitOfWork unitOfWork, IMapper mapper, IAuthentication authentication, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authentication = authentication;
            _httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<ServiceResponse> GetAllServiceBySM()
        {
            var storeManagerId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);

            var storeService = _unitOfWork.ServiceRepository.Get(ss => ss.StoreManagerId == storeManagerId).ToList();

            if (storeService == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy dịch vụ của doanh nghiệp");
            }

            var serviceResponse = _mapper.Map<IEnumerable<ServiceResponse>>(storeService);
            return serviceResponse;
        }

        public ServiceResponse GetAllServiceBySMId(long id)
        {
            var storeService = _unitOfWork.ServiceRepository.Get(ss => ss.StoreManagerId == id).ToList();

            if (storeService == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy dịch vụ của doanh nghiệp");
            }

            var serviceResponse = _mapper.Map<ServiceResponse>(storeService);
            return serviceResponse;
        }

        public async Task<ServiceResponse> CreateService(ServiceRequest serviceRequest)
        {
            var accountId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);

            var storeManagerId = _unitOfWork.StoreManagerRepository.Get(sm => sm.AccountId == accountId).FirstOrDefault();

            var existingService = _unitOfWork.ServiceRepository.Get(p => p.Name.ToLower() == serviceRequest.Name.ToLower()).FirstOrDefault();

            if (existingService != null)
            {
                throw new CustomException.DataExistException($"Loại dịch vụ '{serviceRequest.Name}' đã tồn tại.");
            }

            var newService = _mapper.Map<Service>(serviceRequest);
            newService.StoreManagerId = storeManagerId.Id;

            _unitOfWork.ServiceRepository.Insert(newService);
            await _unitOfWork.SaveAsync();
            foreach ( var certificate in serviceRequest.CertificateDtos)
            {
                var newCertificate = _mapper.Map<Certificate>(certificate);
                _unitOfWork.CertificateRepository.Insert(newCertificate);

                var newCertificateService = new CertificateService
                {
                    CertificateId = newCertificate.Id,
                    ServiceId = newService.Id,
                };

                _unitOfWork.CertificateServiceRepository.Insert(newCertificateService);

            }

            await _unitOfWork.SaveAsync();

            var serviceResponse = _mapper.Map<ServiceResponse>(newService);

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
