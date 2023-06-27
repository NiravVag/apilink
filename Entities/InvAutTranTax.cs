using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_AUT_TRAN_Tax")]
    public partial class InvAutTranTax
    {
        public int Id { get; set; }
        [Column("Invoice_Id")]
        public int? InvoiceId { get; set; }
        public int? TaxId { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("InvAutTranTaxes")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("InvoiceId")]
        [InverseProperty("InvAutTranTaxes")]
        public virtual InvAutTranDetail Invoice { get; set; }
        [ForeignKey("TaxId")]
        [InverseProperty("InvAutTranTaxes")]
        public virtual InvTranBankTax Tax { get; set; }
    }
}