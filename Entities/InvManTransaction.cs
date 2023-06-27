using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_MAN_Transaction")]
    public partial class InvManTransaction
    {
        public InvManTransaction()
        {
            InvManTranDetails = new HashSet<InvManTranDetail>();
            InvManTranTaxes = new HashSet<InvManTranTax>();
        }

        public int Id { get; set; }
        [StringLength(100)]
        public string InvoiceNo { get; set; }
        [StringLength(500)]
        public string BilledName { get; set; }
        public int CustomerId { get; set; }
        public int InvoiceToId { get; set; }
        [Column(TypeName = "date")]
        public DateTime? InvoiceDate { get; set; }
        [StringLength(2000)]
        public string BilledAddress { get; set; }
        [StringLength(1000)]
        public string Attn { get; set; }
        [StringLength(2000)]
        public string Email { get; set; }
        public int? Currency { get; set; }
        public int? ServiceId { get; set; }
        public int? SupplierId { get; set; }
        public int? CountryId { get; set; }
        public int? ServiceType { get; set; }
        public int OfficeId { get; set; }
        [StringLength(1000)]
        public string PaymentTerms { get; set; }
        public int? PaymentDuration { get; set; }
        public int BankId { get; set; }
        [Column(TypeName = "date")]
        public DateTime? FromDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ToDate { get; set; }
        public double? TotalAmount { get; set; }
        public double? Tax { get; set; }
        public double? TaxAmount { get; set; }
        public int Status { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? EntityId { get; set; }
        public int? BookingNo { get; set; }
        public int? PaymentMode { get; set; }
        [StringLength(200)]
        public string PaymentRef { get; set; }
        [Column("IsEAQF")]
        public bool? IsEaqf { get; set; }
        public int? AuditId { get; set; }

        [ForeignKey("AuditId")]
        [InverseProperty("InvManTransactions")]
        public virtual AudTransaction Audit { get; set; }
        [ForeignKey("BankId")]
        [InverseProperty("InvManTransactions")]
        public virtual InvRefBank Bank { get; set; }
        [ForeignKey("BookingNo")]
        [InverseProperty("InvManTransactions")]
        public virtual InspTransaction BookingNoNavigation { get; set; }
        [ForeignKey("CountryId")]
        [InverseProperty("InvManTransactions")]
        public virtual RefCountry Country { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("InvManTransactionCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("Currency")]
        [InverseProperty("InvManTransactions")]
        public virtual RefCurrency CurrencyNavigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("InvManTransactions")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InvManTransactionDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("InvManTransactions")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("InvoiceToId")]
        [InverseProperty("InvManTransactions")]
        public virtual QuPaidBy InvoiceTo { get; set; }
        [ForeignKey("OfficeId")]
        [InverseProperty("InvManTransactions")]
        public virtual InvRefOffice Office { get; set; }
        [ForeignKey("PaymentMode")]
        [InverseProperty("InvManTransactions")]
        public virtual InvRefPaymentMode PaymentModeNavigation { get; set; }
        [ForeignKey("ServiceId")]
        [InverseProperty("InvManTransactions")]
        public virtual RefService Service { get; set; }
        [ForeignKey("ServiceType")]
        [InverseProperty("InvManTransactions")]
        public virtual RefServiceType ServiceTypeNavigation { get; set; }
        [ForeignKey("Status")]
        [InverseProperty("InvManTransactions")]
        public virtual InvStatus StatusNavigation { get; set; }
        [ForeignKey("SupplierId")]
        [InverseProperty("InvManTransactions")]
        public virtual SuSupplier Supplier { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("InvManTransactionUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("InvManual")]
        public virtual ICollection<InvManTranDetail> InvManTranDetails { get; set; }
        [InverseProperty("ManInvoice")]
        public virtual ICollection<InvManTranTax> InvManTranTaxes { get; set; }
    }
}