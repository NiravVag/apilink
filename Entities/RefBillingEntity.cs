using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_Billing_Entity")]
    public partial class RefBillingEntity
    {
        public RefBillingEntity()
        {
            CuPrDetails = new HashSet<CuPrDetail>();
            InvExfTransactions = new HashSet<InvExfTransaction>();
            InvRefBanks = new HashSet<InvRefBank>();
            QuQuotations = new HashSet<QuQuotation>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public bool Active { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("RefBillingEntities")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("BillingEntityNavigation")]
        public virtual ICollection<CuPrDetail> CuPrDetails { get; set; }
        [InverseProperty("BillingEntity")]
        public virtual ICollection<InvExfTransaction> InvExfTransactions { get; set; }
        [InverseProperty("BillingEntityNavigation")]
        public virtual ICollection<InvRefBank> InvRefBanks { get; set; }
        [InverseProperty("BillingEntityNavigation")]
        public virtual ICollection<QuQuotation> QuQuotations { get; set; }
    }
}