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
    public class BillingRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long WalletId { get; set; }

        public long BookingId { get; set; }

        public double Amount { get; set; }

        public string Description { get; set; }

        public DateTimeOffset CreateDate { get; set; }

        [ForeignKey("WalletId")]
        public virtual Wallet Wallet { get; set; }

        [ForeignKey("BookingId")]
        public virtual Booking Booking { get; set; }

        public BillingRecord()
        {
            CreateDate = CoreHelper.SystemTimeNow;
        }
    }
}
