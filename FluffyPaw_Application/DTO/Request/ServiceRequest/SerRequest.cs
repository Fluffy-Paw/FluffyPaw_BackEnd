using FluffyPaw_Application.DTO.Request.CertificateRequest;
using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluffyPaw_Application.DTO.Request.CertificateRequest;
using Microsoft.AspNetCore.Http;

namespace FluffyPaw_Application.DTO.Request.ServiceRequest
{
    public class SerRequest : IMapFrom<Service>
    {
        public long ServiceTypeId { get; set; }

        public string Name { get; set; }

        public IFormFile Image { get; set; }

        public TimeSpan Duration { get; set; }

        public double Cost { get; set; }

        public string? Description { get; set; }

        public List<CertificateDto>? CertificateDtos { get; set; }
    }
}
