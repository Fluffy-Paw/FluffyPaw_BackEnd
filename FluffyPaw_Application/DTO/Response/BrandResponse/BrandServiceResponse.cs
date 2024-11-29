using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.BrandResponse
{
    public class BrandServiceResponse : IMapFrom<Service>
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
