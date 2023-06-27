using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_Bank_Tax")]
    public partial class InvBankTax
    {
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
        [InverseProperty("InvBankTaxes")]
        public virtual InvBank Bank { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("InvBankTaxCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InvBankTaxDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("InvBankTaxUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}