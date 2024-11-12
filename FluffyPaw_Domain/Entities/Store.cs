using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Entities
{
    public class Store
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long AccountId { get; set; }

        public long BrandId { get; set; }

        public string Name { get; set; }

        public string OperatingLicense { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public float TotalRating { get; set; }

        public bool Status { get; set; }

        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }

        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }

        public ICollection<StoreFile> StoreFile { get; set; }
    }
}
