using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.PetRequest
{
    public class PetTypeRequest
    {
        public long PetCategoryId { get; set; }
        public string Name { get; set; }

    }
}
