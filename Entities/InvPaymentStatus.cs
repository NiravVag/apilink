using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_Payment_Status")]
    public partial class InvPaymentStatus
    {
        public InvPaymentStatus()
        {
            InvAutTranDetails = new HashSet<InvAutTranDetail>();
            InvExfTransactions = new HashSet<InvExfTransaction>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public bool Active { get; set; }

        [InverseProperty("InvoicePaymentStatusNavigation")]
        public virtual ICollection<InvAutTranDetail> InvAutTranDetails { get; set; }
        [InverseProperty("PaymentStatusNavigation")]
        public virtual ICollection<InvExfTransaction> InvExfTransactions { get; set; }
    }
}