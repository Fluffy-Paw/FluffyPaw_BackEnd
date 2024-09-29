using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Entities
{
    public class PetType
    {
        public long Id { get; set; }

        public long PetCategoryId { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        [ForeignKey("PetCategoryId")]
        public virtual PetCategory PetCategory { get; set; }
    }
}
