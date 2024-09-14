using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Entities
{
    public class PetType
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public bool Status { get; set; }
    }
}
