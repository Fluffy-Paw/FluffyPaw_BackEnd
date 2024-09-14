using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Entities
{
    public class StaffAddressService
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long StaffAddressId { get; set; }

        public long ServiceId { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public int LimitPetOwner { get; set; }

        public int CurrentPetOwner { get; set; }

        public float TotalRating { get; set; }

        public string Status { get; set; }

        [ForeignKey("StaffAddressId")]
        public virtual StaffAddress StaffAddress { get; set; }

        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; }
    }
}
