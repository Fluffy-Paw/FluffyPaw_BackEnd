using AutoMapper;
using FluffyPaw_Application.DTO.Request.ServiceRequest;
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
    public class ServiceTypeService : IServiceTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthentication _authentication;

        public ServiceTypeService(IUnitOfWork unitOfWork, IMapper mapper, IAuthentication authentication)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authentication = authentication;
        }

        public IEnumerable<ServiceTypeResponse> GetAllServiceType()
        {
            var serviceTypes = _unitOfWork.ServiceTypeRepository.Get().ToList();
            var serviceTypeResponses = _mapper.Map<IEnumerable<ServiceTypeResponse>>(serviceTypes);
            return serviceTypeResponses;
        }

        public ServiceTypeResponse GetServiceTypeById(long id)
        {
            var serviceTypeId = _unitOfWork.ServiceTypeRepository.Get(st => st.Id == id).FirstOrDefault();
            if (serviceTypeId == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy dịch vụ.");
            }
            var serviceTypeResponse = _mapper.Map<ServiceTypeResponse>(serviceTypeId);
            return serviceTypeResponse;
        }

        public async Task<ServiceTypeResponse> CreateServiceType(ServiceTypeRequest serviceTypeRequest)
        {
            var existingServiceType = _unitOfWork.ServiceTypeRepository.Get().FirstOrDefault(p => p.Name.ToLower() == serviceTypeRequest.Name.ToLower());

            if (existingServiceType != null)
            {
                throw new CustomException.DataExistException($"Loại dịch vụ '{serviceTypeRequest.Name}' đã tồn tại.");
            }

            var newServiceType = _mapper.Map<ServiceType>(serviceTypeRequest);

            _unitOfWork.ServiceTypeRepository.Insert(newServiceType);
            _unitOfWork.Save();

            var serviceTypeResponse = _mapper.Map<ServiceTypeResponse>(newServiceType);
            return serviceTypeResponse;
        }

        public async Task<ServiceTypeResponse> UpdateServiceType(long id, ServiceTypeRequest serviceTypeRequest)
        {
            var existingServiceType = _unitOfWork.ServiceTypeRepository.GetByID(id);

            if (existingServiceType == null)
            {
                throw new CustomException.DataNotFoundException($"Không tìm thấy loại dịch vụ.");
            }

            var duplicateExists = _unitOfWork.ServiceTypeRepository.Exists(p =>
                p.Id != id &&
                p.Name.ToLower() == serviceTypeRequest.Name.ToLower()
            );

            if (duplicateExists)
            {
                throw new CustomException.DataExistException($"Loại dịch vụ '{serviceTypeRequest.Name}' đã tồn tại.");
            }

            _mapper.Map(serviceTypeRequest, existingServiceType);
            _unitOfWork.Save();

            var serviceTypeResponse = _mapper.Map<ServiceTypeResponse>(existingServiceType);
            return serviceTypeResponse;
        }

        public async Task<bool> DeleteServiceType(long id)
        {
            var serviceType = _unitOfWork.ServiceTypeRepository.GetByID(id);
            if (serviceType == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy dịch vụ.");
            }

            var services = _unitOfWork.ServiceRepository.Get(s => s.ServiceTypeId == serviceType.Id && s.Status == true);
            if (services.Any())
            {
                throw new CustomException.DataExistException($"Loại hình dịch vụ {serviceType.Name} đang được sử dụng bởi các thương hiệu.");
            }

            _unitOfWork.ServiceTypeRepository.Delete(serviceType);
            _unitOfWork.Save();

            return true;
        }
    }
}
