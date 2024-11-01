using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.ReportResponse
{
    public class ReportCategoryResponse : IMapFrom<ReportCategory>
    {
        public long Id { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }
    }
}
