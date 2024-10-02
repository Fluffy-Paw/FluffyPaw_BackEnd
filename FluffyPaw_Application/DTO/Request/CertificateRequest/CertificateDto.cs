using FluffyPaw_Application.DTO.Request.FilesRequest;
using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.CertificateRequest
{
    public class CertificateDto : IMapFrom<Certificate>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public IFormFile File { get; set; }
    }
}
