using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluffyPaw_Domain.Utils;

namespace FluffyPaw_Domain.Entities
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string? Avatar { get; set; }

        public string RoleName { get; set; }

        public string Email { get; set; }

        public DateTimeOffset CreateDate { get; set; }

        public bool Status { get; set; }

        public Account()
        {
            CreateDate = CoreHelper.SystemTimeNow;
        }
    }
}
