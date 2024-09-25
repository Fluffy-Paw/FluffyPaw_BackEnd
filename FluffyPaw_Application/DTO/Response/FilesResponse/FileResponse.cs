using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.FilesResponse
{
    public class FileResponse : IMapFrom<Files>
    {
        public long Id { get; set; }

        public string File { get; set; }

        public DateTimeOffset CreateDate { get; set; }

        public bool Status { get; set; }
    }
}
