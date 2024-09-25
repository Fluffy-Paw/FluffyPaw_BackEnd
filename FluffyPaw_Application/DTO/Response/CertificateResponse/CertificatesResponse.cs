using AutoMapper;
using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.CertificateResponse
{
    public class CertificatesResponse : IMapFrom<Certificate>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string File { get; set; }
    }
}
