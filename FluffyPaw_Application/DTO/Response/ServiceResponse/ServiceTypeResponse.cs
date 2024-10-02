using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.ServiceResponse
{
    public class ServiceTypeResponse : IMapFrom<ServiceType>
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
