using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.StoreManagerRequest
{
    public class UpdateStoreRequest : IMapFrom<Store>
    {
        public string Name { get; set; }

        public string Address { get; set; }

        public IFormFile OperatingLicense { get; set; }
    }
}