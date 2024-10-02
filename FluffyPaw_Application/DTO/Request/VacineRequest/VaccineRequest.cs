using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.VacineRequest
{
    public class VaccineRequest : IMapFrom<VaccineHistory>, IMapFrom<Pet>
    {
        [Required(ErrorMessage = "Bạn chưa nhập petId")]
        public long PetId { get; set; }

        public IFormFile? Image { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập cân nặng")]
        public float PetCurrentWeight { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập ngày của vaccine")]
        public DateTimeOffset VaccineDate { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập Ngày chích tiếp theo")]
        public DateTimeOffset NextVaccineDate { get; set; }

        public string? Description { get; set; }
    }
}
