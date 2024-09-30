using FluffyPaw_Application.Mapper;
using FluffyPaw_Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.PetRequest
{
    public class PetRequest : IMapFrom<Pet>, IMapFrom<PetType>, IMapFrom<BehaviorCategory>
    {
        public IFormFile? Image {  get; set; }

        [Required(ErrorMessage = "Vui lòng nhập chủng loại.")]
        public long PetTypeId { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập hành vi đặc biệt.")]
        public long BehaviorCategoryId { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập tên.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giới tính.")]
        public string Sex { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập cân nặng.")]
        public float Weight { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập ngày sinh.")]
        public DateTimeOffset Dob { get; set; }

        public string? Allergy { get; set; }

        public string? MicrochipNumber { get; set; }

        public string? Decription { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tình trạng thiến.")]
        public bool IsNeuter { get; set; }
    }
}
