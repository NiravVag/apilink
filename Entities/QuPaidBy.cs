using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("QU_PaidBy")]
    public partial class QuPaidBy
    {
        public QuPaidBy()
        {
            CuPrDetails = new HashSet<CuPrDetail>();
            InvAutTranDetails = new HashSet<InvAutTranDetail>();
            InvExfTransactions = new HashSet<InvExfTransaction>();
            InvManTransactions = new HashSet<InvManTransaction>();
            QuQuotations = new HashSet<QuQuotation>();
        }

        public int Id { get; set; }
        [StringLength(200)]
        public string Label { get; set; }

        [InverseProperty("BillingTo")]
        public virtual ICollection<CuPrDetail> CuPrDetails { get; set; }
        [InverseProperty("InvoiceToNavigation")]
        public virtual ICollection<InvAutTranDetail> InvAutTranDetails { get; set; }
        [InverseProperty("BilledToNavigation")]
        public virtual ICollection<InvExfTransaction> InvExfTransactions { get; set; }
        [InverseProperty("InvoiceTo")]
        public virtual ICollection<InvManTransaction> InvManTransactions { get; set; }
        [InverseProperty("BillingPaidBy")]
        public virtual ICollection<QuQuotation> QuQuotations { get; set; }
    }
}