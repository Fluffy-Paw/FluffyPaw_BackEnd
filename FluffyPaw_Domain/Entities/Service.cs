using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Entities
{
    public class Service
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long ServiceTypeId { get; set; }

        public long BrandId { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public TimeSpan Duration { get; set; }

        public double Cost { get; set; }

        public string Description { get; set; }

        public int BookingCount { get; set; }

        public float TotalRating { get; set; }

        public bool Status { get; set; }

        [ForeignKey("ServiceTypeId")]
        public virtual ServiceType ServiceType { get; set; }

        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }

        public ICollection<Certificate> Certificate { get; set; }
    }
}
