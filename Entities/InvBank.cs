using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_Bank")]
    public partial class InvBank
    {
        public InvBank()
        {
            InvBankTaxes = new HashSet<InvBankTax>();
        }

        public int Id { get; set; }
        [StringLength(500)]
        public string AccountName { get; set; }
        [StringLength(500)]
        public string AccountNumber { get; set; }
        [StringLength(500)]
        public string BankName { get; set; }
        [StringLength(500)]
        public string SwiftCode { get; set; }
        [StringLength(1000)]
        public string BankAddress { get; set; }
        public int? AccountCurrency { get; set; }
        [StringLength(2000)]
        public string Remarks { get; set; }
        public bool? Active { get; set; }
        [StringLength(255)]
        public string ChopFileUniqueId { get; set; }
        [StringLength(255)]
        public string SignatureFileUniqueId { get; set; }
        [StringLength(255)]
        public string ChopFilename { get; set; }
        [StringLength(255)]
        public string SignatureFilename { get; set; }
        public string ChopFileUrl { get; set; }
        public string SignatureFileUrl { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("AccountCurrency")]
        [InverseProperty("InvBanks")]
        public virtual RefCurrency AccountCurrencyNavigation { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("InvBankCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InvBankDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("InvBankUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("Bank")]
        public virtual ICollection<InvBankTax> InvBankTaxes { get; set; }
    }
}