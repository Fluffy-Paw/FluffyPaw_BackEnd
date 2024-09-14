using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Entities
{
    public class Conversation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long PetOwnerId { get; set; }

        public long StaffAddressId { get; set; }

        public string LastMessege { get; set; }

        public bool IsOpen { get; set; }

        [ForeignKey("PetOwnerId")]
        public virtual PetOwner PetOwner { get; set; }

        [ForeignKey("StaffAddressId")]
        public virtual StaffAddress StaffAdddress { get; set; }
    }
}
