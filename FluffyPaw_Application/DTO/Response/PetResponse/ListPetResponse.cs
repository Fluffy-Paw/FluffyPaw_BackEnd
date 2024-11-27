using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Response.PetResponse
{
    public class ListPetResponse : IMapFrom<Pet>, IMapFrom<PetCategory>
    {
        public long Id { get; set; }

        public string? Image { get; set; }

        public string Name { get; set; }

        public long PetCategoryId { get; set; }

        public string BehaviorCategory { get; set; }

        public string Sex { get; set; }

        public float Weight { get; set; }

        public bool IsNeuter { get; set; }

        public string Status { get; set; }
    }
}
