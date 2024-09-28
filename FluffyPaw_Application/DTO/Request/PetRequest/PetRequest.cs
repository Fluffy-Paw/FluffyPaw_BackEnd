using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.PetRequest
{
    public class PetRequest : IMapFrom<Pet>, IMapFrom<PetCategory>, IMapFrom<PetType>, IMapFrom<BehaviorCategory>
    {
        public string? Image {  get; set; }

        public long PetCategoryId { get; set; }

        public long PetTypeId { get; set; }

        public long BehaviorCategoryId { get; set; }

        public string Name { get; set; }

        public string Sex { get; set; }

        public float Weight { get; set; }

        public DateTimeOffset Dob { get; set; }

        public string? Allergy { get; set; }

        public string MicrochipNumber { get; set; }

        public string? Decription { get; set; }

        public bool IsNeuter { get; set; }
    }
}
