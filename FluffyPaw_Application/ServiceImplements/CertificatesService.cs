using AutoMapper;
using FluffyPaw_Application.DTO.Response.CertificateResponse;
using FluffyPaw_Application.Services;
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

        public async Task<CertificatesResponse> CreateCertificate(CertificateRequest certificateRequest)
        {

            var certificateResponse = _mapper.Map<CertificatesResponse>(certificateRequest);
            return certificateResponse;
        }
    }
}
