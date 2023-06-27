using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_EXT_TRAN_Tax")]
    public partial class InvExtTranTax
    {
        public int Id { get; set; }
        [Column("ExtraFee_Id")]
        public int? ExtraFeeId { get; set; }
        public int? TaxId { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("InvExtTranTaxes")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("ExtraFeeId")]
        [InverseProperty("InvExtTranTaxes")]
        public virtual InvExfTransaction ExtraFee { get; set; }
        [ForeignKey("TaxId")]
        [InverseProperty("InvExtTranTaxes")]
        public virtual InvTranBankTax Tax { get; set; }
    }
}