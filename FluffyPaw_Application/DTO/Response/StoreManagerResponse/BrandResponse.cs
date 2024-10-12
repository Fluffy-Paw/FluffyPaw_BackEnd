using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.BrandResponse
{
    public class BrandResponse : IMapFrom<Brand>, IMapFrom<Account>, IMapFrom<Identification>
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public string FullName { get; set; }

        public DateTimeOffset CreateDate { get; set; }

        public string Name { get; set; }

        public string Logo { get; set; }

        public string Hotline { get; set; }

        public string BrandEmail { get; set; }

        public string BusinessLicense { get; set; }

        public string MST { get; set; }

        public bool Status { get; set; }

        public string Front { get; set; }

        public string Back { get; set; }
    }
}
