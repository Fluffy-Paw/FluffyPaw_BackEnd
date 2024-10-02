using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Entities
{
    public class Identification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long AccountId { get; set; }

        public string FullName { get; set; }

        public string Front { get; set; }

        public string Back { get; set; }

        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
    }
}
