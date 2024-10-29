﻿using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.ReportResponse
{
    public class ReportResponse : IMapFrom<Report>
    {
        public long Id { get; set; }

        public long SenderId { get; set; }

        public long TargetId { get; set; }

        public long ReportCategoryId { get; set; }

        public DateTimeOffset CreateDate { get; set; }

        public string Description { get; set; }

        public bool Status { get; set; }
    }
}