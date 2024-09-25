using FluffyPaw_Application.DTO.Response.CertificateResponse;
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
        Task<CertificatesResponse> CreateCertificate(CertificateRequest certificateRequest);
    }
}
