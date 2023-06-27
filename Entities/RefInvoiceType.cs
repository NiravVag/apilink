using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_InvoiceType")]
    public partial class RefInvoiceType
    {
        public RefInvoiceType()
        {
            CuCustomers = new HashSet<CuCustomer>();
            EsDetails = new HashSet<EsDetail>();
            InvAutTranDetails = new HashSet<InvAutTranDetail>();
            InvDaInvoiceTypes = new HashSet<InvDaInvoiceType>();
            QuQuotations = new HashSet<QuQuotation>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("RefInvoiceTypes")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("InvoiceTypeNavigation")]
        public virtual ICollection<CuCustomer> CuCustomers { get; set; }
        [InverseProperty("InvoiceType")]
        public virtual ICollection<EsDetail> EsDetails { get; set; }
        [InverseProperty("InvoiceTypeNavigation")]
        public virtual ICollection<InvAutTranDetail> InvAutTranDetails { get; set; }
        [InverseProperty("InvoiceType")]
        public virtual ICollection<InvDaInvoiceType> InvDaInvoiceTypes { get; set; }
        [InverseProperty("PaymentTermsNavigation")]
        public virtual ICollection<QuQuotation> QuQuotations { get; set; }
    }
}