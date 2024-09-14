using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Entities
{
    public class PetOwner
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long AccountId { get; set; }

        public string FullName { get; set; }

        public string Gender { get; set; }

        public DateTimeOffset Dob { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Status { get; set; }

        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
    }
}
