﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_REF_BillingFreequency")]
    public partial class InvRefBillingFreequency
    {
        public InvRefBillingFreequency()
        {
            CuPrDetails = new HashSet<CuPrDetail>();
        }

        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public bool? Active { get; set; }
        public int? Sort { get; set; }

        [InverseProperty("BillingFreequencyNavigation")]
        public virtual ICollection<CuPrDetail> CuPrDetails { get; set; }
    }
}