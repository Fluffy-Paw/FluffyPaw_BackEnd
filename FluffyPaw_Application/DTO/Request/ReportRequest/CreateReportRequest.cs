using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.ReportRequest
{
    public class CreateReportRequest : IMapFrom<Report>
    {
        public long TargetId { get; set; }

        public long ReportCategoryId { get; set; }

        public string? Description { get; set; }

    }
}
