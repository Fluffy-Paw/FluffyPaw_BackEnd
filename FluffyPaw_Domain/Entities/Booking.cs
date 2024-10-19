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
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long PetId { get; set; }

        public long StoreServiceId { get; set; }

        public string PaymentMethod { get; set; }

        public double Cost { get; set; }

        public string? Description { get; set; }

        public DateTimeOffset CreateDate { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public DateTimeOffset EndTime { get; set; }

        public bool Checkin { get; set; }

        public DateTimeOffset CheckinTime { get; set; }

        public string Status { get; set; }

        [ForeignKey("PetId")]
        public virtual Pet Pet { get; set; }

        [ForeignKey("StoreServiceId")]
        public virtual StoreService StoreService { get; set; }

        public Booking()
        {
            CreateDate = CoreHelper.SystemTimeNow;
            StartTime = CoreHelper.SystemTimeNow;
            EndTime = CoreHelper.SystemTimeNow;
            CheckinTime = CoreHelper.SystemTimeNow;
        }
    }
}
