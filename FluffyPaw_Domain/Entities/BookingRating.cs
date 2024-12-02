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
    public class BookingRating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long BookingId { get; set; }

        public long PetOwnerId { get; set; }

        public int ServiceVote { get; set; }

        public int StoreVote { get; set; }

        public string? Description { get; set; }

        public string? Image { get; set; }

        public DateTimeOffset CreateDate { get; set; }

        [ForeignKey("BookingId")]
        public virtual Booking Booking { get; set; }

        [ForeignKey("PetOwnerId")]
        public virtual PetOwner PetOwner { get; set; }

        public BookingRating()
        {
            CreateDate = CoreHelper.SystemTimeNow;
        }
    }
}
