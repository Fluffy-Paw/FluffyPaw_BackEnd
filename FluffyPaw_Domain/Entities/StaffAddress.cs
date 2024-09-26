using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Entities
{
    public class StaffAddress
    {
        public long Id { get; set; }

        public long AccountId { get; set; }

        public long StoreManagerId { get; set; }

        public string StaffAddressName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public float TotalRating { get; set; }

        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }

        [ForeignKey("StoreManagerId")]
        public virtual StoreManager StoreManager { get; set; }
    }
}
