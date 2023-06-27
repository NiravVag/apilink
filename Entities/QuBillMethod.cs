using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("QU_BillMethod")]
    public partial class QuBillMethod
    {
        public QuBillMethod()
        {
            CuPrDetails = new HashSet<CuPrDetail>();
            InvAutTranDetails = new HashSet<InvAutTranDetail>();
            QuQuotations = new HashSet<QuQuotation>();
        }

        public int Id { get; set; }
        [StringLength(200)]
        public string Label { get; set; }

        [InverseProperty("BillingMethod")]
        public virtual ICollection<CuPrDetail> CuPrDetails { get; set; }
        [InverseProperty("InvoiceMethodNavigation")]
        public virtual ICollection<InvAutTranDetail> InvAutTranDetails { get; set; }
        [InverseProperty("BillingMethod")]
        public virtual ICollection<QuQuotation> QuQuotations { get; set; }
    }
}