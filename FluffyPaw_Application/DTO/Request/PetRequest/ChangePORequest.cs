using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Application.DTO.Request.PetRequest
{
    public class ChangePORequest
    {
        [Required]
        public long PetId {  get; set; }
        [Required]
        public string NewOwnerUsername { get; set; }
        [Required]
        public string YourPassword { get; set;}
    }
}
