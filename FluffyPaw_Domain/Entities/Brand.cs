﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluffyPaw_Domain.Entities
{
    public class Brand
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long AccountId { get; set; }

        public string Name { get; set; }

        public string Logo { get; set; }

        public string Hotline { get; set; }

        public string BrandEmail { get; set; }

        public string BusinessLicense { get; set; }

        public string MST {  get; set; }

        public string Address { get; set; }

        public bool Status { get; set; }

        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
    }
}
