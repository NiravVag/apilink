using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("EC_ExpenseClaimtype")]
    public partial class EcExpenseClaimtype
    {
        public EcExpenseClaimtype()
        {
            EcExpencesClaims = new HashSet<EcExpencesClaim>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public bool Active { get; set; }

        [InverseProperty("ClaimType")]
        public virtual ICollection<EcExpencesClaim> EcExpencesClaims { get; set; }
    }
}