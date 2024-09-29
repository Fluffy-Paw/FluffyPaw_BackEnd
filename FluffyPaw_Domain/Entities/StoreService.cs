using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Entities
{
    public class StoreService
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long StoreId { get; set; }

        public long ServiceId { get; set; }

        public DateTimeOffset StartTime { get; set; }

        public int LimitPetOwner { get; set; }

        public int CurrentPetOwner { get; set; }

        public string Status { get; set; }

        [ForeignKey("StoreId")]
        public virtual Store Store { get; set; }

        [ForeignKey("ServiceId")]
        public virtual Service Service { get; set; }
    }
}
