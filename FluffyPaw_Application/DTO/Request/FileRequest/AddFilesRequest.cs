using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.FileRequest
{
    public class AddFilesRequest : IMapFrom<Files>
    {
        public IFormFile file {  get; set; }
    }
}
