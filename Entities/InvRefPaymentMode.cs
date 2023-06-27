using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_REF_PaymentMode")]
    public partial class InvRefPaymentMode
    {
        public InvRefPaymentMode()
        {
            InvManTransactions = new HashSet<InvManTransaction>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public bool Active { get; set; }
        public int Sort { get; set; }

        [InverseProperty("PaymentModeNavigation")]
        public virtual ICollection<InvManTransaction> InvManTransactions { get; set; }
    }
}