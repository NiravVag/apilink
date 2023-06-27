using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_CRE_Transaction")]
    public partial class InvCreTransaction
    {
        public InvCreTransaction()
        {
            InvCreTranClaimDetails = new HashSet<InvCreTranClaimDetail>();
            InvCreTranContacts = new HashSet<InvCreTranContact>();
        }

        public int Id { get; set; }
        [StringLength(1000)]
        public string CreditNo { get; set; }
        public int? CreditTypeId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreditDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PostDate { get; set; }
        [StringLength(1000)]
        public string BillTo { get; set; }
        [StringLength(2000)]
        public string BilledAddress { get; set; }
        public int? Currency { get; set; }
        [StringLength(1000)]
        public string PaymentTerms { get; set; }
        public int? PaymentDuration { get; set; }
        [StringLength(2000)]
        public string InvoiceAdrress { get; set; }
        public int? Office { get; set; }
        public int? BankId { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        public int? DeletedBy { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        [StringLength(2000)]
        public string Subject { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("BankId")]
        [InverseProperty("InvCreTransactions")]
        public virtual InvRefBank Bank { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("InvCreTransactionCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CreditTypeId")]
        [InverseProperty("InvCreTransactions")]
        public virtual InvCreRefCreditType CreditType { get; set; }
        [ForeignKey("Currency")]
        [InverseProperty("InvCreTransactions")]
        public virtual RefCurrency CurrencyNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InvCreTransactionDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("InvCreTransactions")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("Office")]
        [InverseProperty("InvCreTransactions")]
        public virtual RefLocation OfficeNavigation { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("InvCreTransactionUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("Credit")]
        public virtual ICollection<InvCreTranClaimDetail> InvCreTranClaimDetails { get; set; }
        [InverseProperty("CreditedNavigation")]
        public virtual ICollection<InvCreTranContact> InvCreTranContacts { get; set; }
    }
}