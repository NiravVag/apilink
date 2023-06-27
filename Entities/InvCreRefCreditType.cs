using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_CRE_REF_CreditType")]
    public partial class InvCreRefCreditType
    {
        public InvCreRefCreditType()
        {
            InvCreTransactions = new HashSet<InvCreTransaction>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public bool? Active { get; set; }
        public int Sort { get; set; }

        [InverseProperty("CreditType")]
        public virtual ICollection<InvCreTransaction> InvCreTransactions { get; set; }
    }
}