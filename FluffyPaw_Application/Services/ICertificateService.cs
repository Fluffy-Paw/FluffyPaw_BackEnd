using FluffyPaw_Application.DTO.Request.CertificateRequest;
using FluffyPaw_Application.DTO.Request.ServiceRequest;
using FluffyPaw_Application.DTO.Response.CertificateResponse;
using FluffyPaw_Application.DTO.Response.ServiceResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.Services
{
    public interface ICertificateService
    {
        Task<List<CertificatesResponse>> GetAllCertificateByServiceId(long id);
        Task<CertificatesResponse> CreateCertificate(CreateCertificateRequest certificateRequest);
        Task<CertificatesResponse> UpdateCertificate(long id, UpdateCertificateRequest certificateRequest);
        Task<bool> DeleteCertificate(long id);
    }
}
