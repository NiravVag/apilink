using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_DIS_REF_Type")]
    public partial class InvDisRefType
    {
        public InvDisRefType()
        {
            InvDisTranDetails = new HashSet<InvDisTranDetail>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public bool? Active { get; set; }
        public int Sort { get; set; }

        [InverseProperty("DiscountTypeNavigation")]
        public virtual ICollection<InvDisTranDetail> InvDisTranDetails { get; set; }
    }
}