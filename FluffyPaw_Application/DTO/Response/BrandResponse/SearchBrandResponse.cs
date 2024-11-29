using AutoMapper;
using FluffyPaw_Application.DTO.Response.ServiceResponse;
using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.BrandResponse
{
    public class SearchBrandResponse : IMapFrom<Brand>, IMapFrom<Service>
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public string Name { get; set; }

        public string Logo { get; set; }

        public string Hotline { get; set; }

        public string BrandEmail { get; set; }

        public string Address { get; set; }

        public double Rating { get; set; }

        public int TotalStore {  get; set; }

        public int TotalServices { get; set; }

        public List<BrandServiceResponse> ServiceNames {  get; set; }

    }
}
