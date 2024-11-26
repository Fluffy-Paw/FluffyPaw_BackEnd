using AutoMapper;
using FluffyPaw_Application.DTO.Request.ServiceRequest;
using FluffyPaw_Application.DTO.Response.CertificateResponse;
using FluffyPaw_Application.DTO.Response.ServiceResponse;
using FluffyPaw_Application.DTO.Response.StoreServiceResponse;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.CustomException;
using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Enums;
using FluffyPaw_Domain.Interfaces;
using FluffyPaw_Repository.Enum;
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
        private readonly IFirebaseConfiguration _firebaseConfiguration;

        public SerService(IUnitOfWork unitOfWork, IMapper mapper, IAuthentication authentication,
                        IHttpContextAccessor httpContextAccessor, IFirebaseConfiguration firebaseConfiguration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authentication = authentication;
            _httpContextAccessor = httpContextAccessor;
            _firebaseConfiguration = firebaseConfiguration;
        }

        public async Task<List<SerResponse>> GetAllServiceBySM()
        {
            var accountId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);

            var BrandId = _unitOfWork.BrandRepository.Get(sm => sm.AccountId == accountId).FirstOrDefault();

            var services = _unitOfWork.ServiceRepository.Get(ss => ss.BrandId == BrandId.Id && ss.Status  == true,
                includeProperties: "Certificate").ToList();

            if (!services.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy dịch vụ của doanh nghiệp");
            }

            var serviceResponses = new List<SerResponse>();
            foreach (var service in services)
            {
                var serviceType = _unitOfWork.ServiceTypeRepository.GetByID(service.ServiceTypeId);
                var serviceResponse = _mapper.Map<SerResponse>(service);
                serviceResponse.ServiceTypeName = serviceType.Name;
                serviceResponses.Add(serviceResponse);
            }

            return serviceResponses;
        }

        public async Task<List<SerResponse>> GetAllServiceBySMId(long id)
        {
            var services = _unitOfWork.ServiceRepository.Get(ss => ss.BrandId == id, includeProperties: "ServiceType").ToList();
            if (services == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy dịch vụ của doanh nghiệp");
            }

            var serviceResponses = new List<SerResponse>();
            foreach (var service in services)
            {
                var serviceType = _unitOfWork.ServiceTypeRepository.GetByID(service.ServiceTypeId);
                var serviceResponse = _mapper.Map<SerResponse>(service);
                serviceResponse.ServiceTypeName = serviceType.Name;
                serviceResponses.Add(serviceResponse);
            }

            return serviceResponses;
        }

        public async Task<List<SerResponse>> GetAllServiceByStoreId(long id)
        {
            var storeServices = _unitOfWork.StoreServiceRepository.Get(ss => ss.StoreId == id).ToList();
            if (storeServices == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy lịch trình của cửa hàng này.");
            }

            var groupedServices = storeServices.GroupBy(ss => ss.ServiceId);

            var serviceResponses = new List<SerResponse>();
            foreach (var group in groupedServices)
            {
                var serviceId = group.Key;

                var service = _unitOfWork.ServiceRepository
                    .Get(s => s.Id == serviceId, includeProperties: "ServiceType,Certificate")
                    .FirstOrDefault();

                if (service == null)
                {
                    throw new CustomException.DataNotFoundException("Không tìm thấy dịch vụ của cửa hàng này.");
                }

                var serviceResponse = _mapper.Map<SerResponse>(service);

                serviceResponse.ServiceTypeName = service.ServiceType.Name;

                serviceResponses.Add(serviceResponse);
            }
            if (!serviceResponses.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy dịch vụ nào cho cửa hàng này.");
            }

            return serviceResponses;
        }

        public async Task<SerResponse> CreateService(SerRequest serviceRequest)
        {
            var accountId = _authentication.GetUserIdFromHttpContext(_httpContextAccessor.HttpContext);

            var brand = _unitOfWork.BrandRepository.Get(sm => sm.AccountId == accountId && sm.Status == true).FirstOrDefault();
            if (brand == null)
            {
                throw new CustomException.DataNotFoundException("Brand này chưa được xác thực từ hệ thống.");
            }

            var existingService = _unitOfWork.ServiceRepository.Get(p => p.Name.ToLower() == serviceRequest.Name.ToLower()
            && p.BrandId == brand.Id).FirstOrDefault();

            if (existingService != null)
            {
                throw new CustomException.DataExistException($"Loại dịch vụ '{serviceRequest.Name}' đã tồn tại.");
            }

            var newService = _mapper.Map<Service>(serviceRequest);
            newService.BrandId = brand.Id;
            newService.Image = await _firebaseConfiguration.UploadImage(serviceRequest.Image);

            _unitOfWork.ServiceRepository.Insert(newService);
            await _unitOfWork.SaveAsync();

            if (serviceRequest.CertificateDtos != null)
            {
                foreach (var certificate in serviceRequest.CertificateDtos)
                {
                    var newCertificate = _mapper.Map<Certificate>(certificate);
                    _unitOfWork.CertificateRepository.Insert(newCertificate);
                }

                await _unitOfWork.SaveAsync();
            }

            var serviceType = _unitOfWork.ServiceTypeRepository.GetByID(serviceRequest.ServiceTypeId);

            var serviceResponse = _mapper.Map<SerResponse>(newService);
            serviceResponse.ServiceTypeName = serviceType.Name;
            return serviceResponse;
        }

        public async Task<UpdateServiceResponse> UpdateService(long id, UpdateServiceRequest updateServiceRequest)
        {
            var existingService = _unitOfWork.ServiceRepository.GetByID(id);

            if (existingService == null)
            {
                throw new CustomException.DataNotFoundException($"Không tìm thấy dịch vụ.");
            }

            var storeServices = _unitOfWork.StoreServiceRepository.Get(ss => ss.ServiceId == id
                                                        && ss.Status == StoreServiceStatus.Available.ToString()).ToList();
            if (storeServices.Any())
            {
                throw new CustomException.DataExistException($"Dịch vụ {existingService.Name} vẫn còn lịch trình khả dụng từ các cửa hàng.");
            }

            _mapper.Map(updateServiceRequest, existingService);
            existingService.Image = await _firebaseConfiguration.UploadImage(updateServiceRequest.Image);
            existingService.Status = false;
            _unitOfWork.Save();

            var serviceResponse = _mapper.Map<UpdateServiceResponse>(existingService);
            return serviceResponse;
        }

        public async Task<bool> DeActiveService(long id)
        {
            var service = _unitOfWork.ServiceRepository.GetByID(id);
            if (service == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy dịch vụ.");
            }

            var storeServices = _unitOfWork.StoreServiceRepository.Get(ss => ss.ServiceId == id
                                                        && ss.Status == StoreServiceStatus.Available.ToString(),
                                                        includeProperties: "Store,Service").ToList();
            if (storeServices.Any())
            {
                throw new CustomException.DataExistException($"Dịch vụ {service.Name} vẫn còn lịch trình khả dụng từ các cửa hàng.");
            }

            foreach ( var storeService in storeServices ) 
            {
                var bookings = _unitOfWork.BookingRepository.Get(b => b.StoreServiceId == storeService.Id
                                                && b.Status == BookingStatus.Pending.ToString()
                                                && b.Status == BookingStatus.Accepted.ToString()).ToList();
                if (bookings.Any())
                {
                    throw new CustomException.DataExistException($"Cửa hàng {storeService.Store.Name} đang còn lịch đặt từ khách hàng" +
                                            $" vào khung giờ {storeService.StartTime} của dịch vụ {storeService.Service.Name}.");
                }
            }

            service.Status = false;

            _unitOfWork.Save();

            return true;
        }

        public async Task<bool> DeleteService(long id)
        {
            var service = _unitOfWork.ServiceRepository.GetByID(id);
            if (service == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy dịch vụ.");
            }

            var storeServices = _unitOfWork.StoreServiceRepository.Get(ss => ss.ServiceId == id
                                                        && ss.Status == StoreServiceStatus.Available.ToString()).ToList();
            if (storeServices.Any())
            {
                throw new CustomException.DataExistException($"Dịch vụ {service.Name} vẫn còn lịch trình khả dụng từ các cửa hàng.");
            }

            _unitOfWork.ServiceRepository.Delete(service);

            _unitOfWork.Save();

            return true;
        }
    }
}
