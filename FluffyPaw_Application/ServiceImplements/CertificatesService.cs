using AutoMapper;
using FluffyPaw_Application.DTO.Request.CertificateRequest;
using FluffyPaw_Application.DTO.Response.CertificateResponse;
using FluffyPaw_Application.DTO.Response.ServiceResponse;
using FluffyPaw_Application.Services;
using FluffyPaw_Domain.CustomException;
using FluffyPaw_Domain.Entities;
using FluffyPaw_Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.ServiceImplements
{
    public class CertificatesService : ICertificateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFirebaseConfiguration _firebaseConfiguration;

        public CertificatesService(IUnitOfWork unitOfWork, IMapper mapper, IFirebaseConfiguration firebaseConfiguration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _firebaseConfiguration = firebaseConfiguration;
        }

        public async Task<List<CertificatesResponse>> GetAllCertificateByServiceId(long id)
        {
            var service = _unitOfWork.ServiceRepository.GetByID(id);
            if (service.Status = false)
            {
                throw new CustomException.InvalidDataException("Dịch vụ này chưa được xác thực.");
            }

            var certificateService = _unitOfWork.CertificateRepository.Get(ss => ss.ServiceId == id).ToList();
            if (!certificateService.Any())
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy certificate của dịch vụ này");
            }

            var certificatesResponses = _mapper.Map<List<CertificatesResponse>>(certificateService);
            return certificatesResponses;
        }

        public async Task<CertificatesResponse> CreateCertificate(CreateCertificateRequest certificateRequest)
        {
            var servie = _unitOfWork.ServiceRepository.GetByID(certificateRequest.ServiceId);

            var limitCertificate = _unitOfWork.CertificateRepository.Get(s => s.ServiceId == certificateRequest.ServiceId);
            if (limitCertificate.Count() >= 2)
            {
                throw new CustomException.InvalidDataException($"Số lượng certificate của dịch vụ {servie.Name} đã đạt tối đa.");
            }

            var existingCertificate = _unitOfWork.CertificateRepository.Get(c => c.Name.ToLower() == certificateRequest.Name.ToLower()
                                            && c.ServiceId == certificateRequest.ServiceId);
            if (existingCertificate.Any())
            {
                throw new CustomException.DataExistException($"Đã tồn tại Certificate {certificateRequest.Name} của dịch vụ này.");
            }

            var newCertificate = _mapper.Map<Certificate>(certificateRequest);
            newCertificate.File = await _firebaseConfiguration.UploadImage(certificateRequest.File);

            _unitOfWork.CertificateRepository.Insert(newCertificate);
            await _unitOfWork.SaveAsync();

            var certificateResponse = _mapper.Map<CertificatesResponse>(newCertificate);
            return certificateResponse;
        }

        public async Task<CertificatesResponse> UpdateCertificate(long id, UpdateCertificateRequest updateCertificateRequest)
        {
            var existingCertificate = _unitOfWork.CertificateRepository.Get(c => c.Id == id).First();
            if (existingCertificate == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy Certificate này.");
            }

            _mapper.Map(existingCertificate, updateCertificateRequest);
            existingCertificate.File = await _firebaseConfiguration.UploadImage(updateCertificateRequest.File);

            var certificatesResponse = _mapper.Map<CertificatesResponse>(existingCertificate);
            return certificatesResponse;
        }

        public async Task<bool> DeleteCertificate(long id)
        {
            var certificate = _unitOfWork.CertificateRepository.GetByID(id);
            if (certificate == null)
            {
                throw new CustomException.DataNotFoundException("Không tìm thấy certificate.");
            }

            _unitOfWork.ServiceRepository.Delete(certificate);

            _unitOfWork.Save();

            return true;
        }
    }
}
