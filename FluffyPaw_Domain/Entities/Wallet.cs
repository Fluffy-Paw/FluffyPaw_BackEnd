using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Entities
{
    public class Wallet
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public double Balance { get; set; }

        public string? BankName { get; set; }

        public string? Number {  get; set; }

        public string? QR {  get; set; }

        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
    }
}
