using FluffyPaw_Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Entities
{
    public class Pet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long PetOwnerId { get; set; }

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

        public string Status { get; set; }

        [ForeignKey("PetOwnerId")]
        public virtual PetOwner PetOwner { get; set; }

        [ForeignKey("PetCategoryId")]
        public virtual PetCategory PetCategory { get; set; }

        [ForeignKey("PetTypeId")]
        public virtual PetType PetType { get; set; }

        [ForeignKey("BehaviorCategoryId")]
        public virtual BehaviorCategory BehavoirCategory { get; set; }
    }
}
