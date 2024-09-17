using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.ServiceTypeRequest
{
    public class ServiceTypeRequest : IMapFrom<ServiceType>
    {
        public string Name { get; set; }
    }
}
