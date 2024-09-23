using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Entities
{
    public class VaccineHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long PetId { get; set; }

        public string Image { get; set; }

        public float PetCurrentWeight { get; set; }

        public DateTimeOffset VaccineDate { get; set; }

        public DateTimeOffset NextVaccineDate { get; set; }

        public string? Description { get; set; }

        public string Status { get; set; }

        [ForeignKey("PetId")]
        public virtual Pet Pet { get; set; }
    }
}
