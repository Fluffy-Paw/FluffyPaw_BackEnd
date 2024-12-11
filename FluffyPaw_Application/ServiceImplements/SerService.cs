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
            var brand = _unitOfWork.BrandRepository.Get(sm => sm.AccountId == accountId).FirstOrDefault();
            var services = _unitOfWork.ServiceRepository.Get(ss => ss.BrandId == brand.Id,
                includeProperties: "Certificates").ToList();

            if (!services.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy dịch vụ của doanh nghiệp.");
            }

            var serviceResponses = new List<SerResponse>();

            foreach (var service in services)
            {
                var serviceType = _unitOfWork.ServiceTypeRepository.GetByID(service.ServiceTypeId);
                var serviceResponse = _mapper.Map<SerResponse>(service);
                serviceResponse.ServiceTypeName = serviceType?.Name;

                serviceResponse.Certificate = service.Certificates
                    .Select(certificate => _mapper.Map<CertificatesResponse>(certificate))
                    .ToList();

                var storeServices = _unitOfWork.StoreServiceRepository.Get(ss => ss.ServiceId == service.Id).ToList();
                double totalRevenue = 0;

                foreach (var storeService in storeServices)
                {
                    var bookings = _unitOfWork.BookingRepository.Get(b => b.StoreServiceId == storeService.Id &&
                                                        b.Status == BookingStatus.Ended.ToString()).ToList();

                    totalRevenue += bookings.Count * service.Cost;
                }

                serviceResponse.Revenue = totalRevenue;
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

        public async Task<List<SerStoResponse>> GetAllServiceByStoreId(long id)
        {
            var store = _unitOfWork.StoreRepository.Get(s => s.Id == id, includeProperties: "Brand").FirstOrDefault();
            if (store == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy cửa hàng này.");
            }

            var storeServices = _unitOfWork.StoreServiceRepository.Get(ss => ss.StoreId == id).ToList();
            if (storeServices == null || !storeServices.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy lịch trình của cửa hàng này.");
            }

            var groupedServices = storeServices
                .GroupBy(ss => ss.ServiceId)
                .Select(g => g.First())
                .ToList();

            var serStoResponses = new List<SerStoResponse>();

            foreach (var storeService in groupedServices)
            {
                var serviceId = storeService.ServiceId;

                var service = _unitOfWork.ServiceRepository
                    .Get(s => s.Id == serviceId, includeProperties: "ServiceType,Certificates")
                    .FirstOrDefault();

                if (service == null)
                {
                    throw new CustomException.DataNotFoundException("Không tìm thấy dịch vụ của cửa hàng này.");
                }

                var serStoResponse = _mapper.Map<SerStoResponse>(service);

                serStoResponse.BrandName = store.Brand.Name;
                serStoResponse.ServiceTypeName = service.ServiceType.Name;
                serStoResponse.StoreId = store.Id;
                serStoResponse.StoreName = store.Name;
                serStoResponse.StoreAddress = store.Address;

                // Get the certificates for the current service
                var certificates = _unitOfWork.CertificateRepository.Get(c => c.ServiceId == service.Id).ToList();

                serStoResponse.Certificate = certificates?
                    .Select(certificate => _mapper.Map<CertificatesResponse>(certificate))
                    .ToList();

                serStoResponses.Add(serStoResponse);
            }

            if (!serStoResponses.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy dịch vụ nào cho cửa hàng này.");
            }

            return serStoResponses;
        }

        public async Task<SerResponse> GetServiceById(long id)
        {
            var service = _unitOfWork.ServiceRepository.Get(s => s.Id == id, includeProperties: "Brand,ServiceType,Certificates").FirstOrDefault();
            if (service == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy dịch vụ này.");
            }

            var serviceType = _unitOfWork.ServiceTypeRepository.GetByID(service.ServiceTypeId);
            var certificates = _unitOfWork.CertificateRepository.Get(c => c.ServiceId == service.Id);
            var certificateResponses = _mapper.Map<List<CertificatesResponse>>(certificates);

            var serviceResponse = _mapper.Map<SerResponse>(service);
            serviceResponse.Certificate = certificateResponses;
            return serviceResponse;
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
            if (updateServiceRequest.Image != null)
            {
                existingService.Image = await _firebaseConfiguration.UploadImage(updateServiceRequest.Image);
            }
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

            foreach (var storeService in storeServices)
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
