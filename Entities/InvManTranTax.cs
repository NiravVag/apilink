using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_MAN_TRAN_TAX")]
    public partial class InvManTranTax
    {
        public int Id { get; set; }
        [Column("Man_InvoiceId")]
        public int? ManInvoiceId { get; set; }
        public int? TaxId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("InvManTranTaxes")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("ManInvoiceId")]
        [InverseProperty("InvManTranTaxes")]
        public virtual InvManTransaction ManInvoice { get; set; }
        [ForeignKey("TaxId")]
        [InverseProperty("InvManTranTaxes")]
        public virtual InvTranBankTax Tax { get; set; }
    }
}