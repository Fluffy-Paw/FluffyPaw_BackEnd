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
        //IEnumerable<ServiceResponse> GetAllCertificateByServiceId(long id);
        Task<CertificatesResponse> CreateCertificate(CertificateDto certificateDto);
        //Task<UpdateServiceResponse> UpdateCertificate(long id, CertificateDto certificateDto);
        //Task<bool> DeleteCertificate(long id);
    }
}
