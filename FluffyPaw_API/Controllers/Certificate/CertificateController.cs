using CoreApiResponse;
using FluffyPaw_Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FluffyPaw_API.Controllers.Certificate
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificateController : BaseController
    {
        private readonly ICertificateService _certificateService;

        public CertificateController(ICertificateService certificateService)
        {
            _certificateService = certificateService;
        }


    }
}
