using FluffyPaw_Domain.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Entities
{
    public class Tracking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long BookingId { get; set; }

        public DateTimeOffset UploadDate { get; set; }

        public string Description { get; set; }

        public bool Status { get; set; }

        [ForeignKey("BookingId")]
        public virtual Booking Booking { get; set; }

        public Tracking()
        {
            UploadDate = CoreHelper.SystemTimeNow;
        }
    }
}
