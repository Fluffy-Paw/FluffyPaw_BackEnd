using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.PetResponse
{
    public class PetResponse : IMapFrom<Pet>, IMapFrom<PetType>, IMapFrom<PetCategory>, IMapFrom<BehaviorCategory>
    {
        public long Id { get; set; }

        public string? Image { get; set; }

        public string Name { get; set; }

        public string Sex { get; set; }

        public float Weight { get; set; }

        public DateTimeOffset Dob { get; set; }

        public int Age { get; set; }

        public string? Allergy { get; set; }

        public string? MicrochipNumber { get; set; }

        public string? Decription { get; set; }

        public bool IsNeuter { get; set; }

        public long PetCategoryId { get; set; }

        public string Status { get; set; }

        public PetType? PetType { get; set; }

        public BehaviorCategory? BehaviorCategory { get; set; }
    }
}
