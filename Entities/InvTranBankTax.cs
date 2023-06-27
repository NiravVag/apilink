using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_TRAN_Bank_Tax")]
    public partial class InvTranBankTax
    {
        public InvTranBankTax()
        {
            InvAutTranTaxes = new HashSet<InvAutTranTax>();
            InvExtTranTaxes = new HashSet<InvExtTranTax>();
            InvManTranTaxes = new HashSet<InvManTranTax>();
        }

        public int Id { get; set; }
        [Required]
        [Column("Tax_name")]
        [StringLength(500)]
        public string TaxName { get; set; }
        [Column("Tax_Value", TypeName = "decimal(18, 2)")]
        public decimal TaxValue { get; set; }
        public bool Active { get; set; }
        public int BankId { get; set; }
        [Column("From_Date", TypeName = "datetime")]
        public DateTime FromDate { get; set; }
        [Column("To_Date", TypeName = "datetime")]
        public DateTime? ToDate { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("BankId")]
        [InverseProperty("InvTranBankTaxes")]
        public virtual InvRefBank Bank { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("InvTranBankTaxCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InvTranBankTaxDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("InvTranBankTaxUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("Tax")]
        public virtual ICollection<InvAutTranTax> InvAutTranTaxes { get; set; }
        [InverseProperty("Tax")]
        public virtual ICollection<InvExtTranTax> InvExtTranTaxes { get; set; }
        [InverseProperty("Tax")]
        public virtual ICollection<InvManTranTax> InvManTranTaxes { get; set; }
    }
}