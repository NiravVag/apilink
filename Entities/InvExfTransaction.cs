using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_EXF_Transaction")]
    public partial class InvExfTransaction
    {
        public InvExfTransaction()
        {
            InvExfContactDetails = new HashSet<InvExfContactDetail>();
            InvExfTranDetails = new HashSet<InvExfTranDetail>();
            InvExfTranStatusLogs = new HashSet<InvExfTranStatusLog>();
            InvExtTranTaxes = new HashSet<InvExtTranTax>();
        }

        public int Id { get; set; }
        public int? InspectionId { get; set; }
        public int? CustomerId { get; set; }
        public int? StatusId { get; set; }
        public int? SupplierId { get; set; }
        public int? FactoryId { get; set; }
        public int? BilledTo { get; set; }
        public double? ExtraFeeSubTotal { get; set; }
        public double? TaxAmount { get; set; }
        public double? TotalExtraFee { get; set; }
        public bool? Active { get; set; }
        public int? CurrencyId { get; set; }
        public int? ServiceId { get; set; }
        public double? Tax { get; set; }
        public int? BankId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PaymentDate { get; set; }
        public string Remarks { get; set; }
        public int? BillingEntityId { get; set; }
        public int? InvoiceId { get; set; }
        public string ExtraFeeInvoiceNo { get; set; }
        public int? PaymentStatus { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? AuditId { get; set; }
        public int? OfficeId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ExtraFeeInvoiceDate { get; set; }
        public int? EntityId { get; set; }
        public int? InvoiceCurrencyId { get; set; }
        public double? ExchangeRate { get; set; }
        [StringLength(500)]
        public string BilledName { get; set; }
        [StringLength(1000)]
        public string BilledAddress { get; set; }
        [StringLength(500)]
        public string PaymentTerms { get; set; }
        public int? PaymentDuration { get; set; }

        [ForeignKey("AuditId")]
        [InverseProperty("InvExfTransactions")]
        public virtual AudTransaction Audit { get; set; }
        [ForeignKey("BankId")]
        [InverseProperty("InvExfTransactions")]
        public virtual InvRefBank Bank { get; set; }
        [ForeignKey("BilledTo")]
        [InverseProperty("InvExfTransactions")]
        public virtual QuPaidBy BilledToNavigation { get; set; }
        [ForeignKey("BillingEntityId")]
        [InverseProperty("InvExfTransactions")]
        public virtual RefBillingEntity BillingEntity { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("InvExfTransactionCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CurrencyId")]
        [InverseProperty("InvExfTransactionCurrencies")]
        public virtual RefCurrency Currency { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("InvExfTransactions")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InvExfTransactionDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("InvExfTransactions")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("FactoryId")]
        [InverseProperty("InvExfTransactionFactories")]
        public virtual SuSupplier Factory { get; set; }
        [ForeignKey("InspectionId")]
        [InverseProperty("InvExfTransactions")]
        public virtual InspTransaction Inspection { get; set; }
        [ForeignKey("InvoiceId")]
        [InverseProperty("InvExfTransactions")]
        public virtual InvAutTranDetail Invoice { get; set; }
        [ForeignKey("InvoiceCurrencyId")]
        [InverseProperty("InvExfTransactionInvoiceCurrencies")]
        public virtual RefCurrency InvoiceCurrency { get; set; }
        [ForeignKey("OfficeId")]
        [InverseProperty("InvExfTransactions")]
        public virtual InvRefOffice Office { get; set; }
        [ForeignKey("PaymentStatus")]
        [InverseProperty("InvExfTransactions")]
        public virtual InvPaymentStatus PaymentStatusNavigation { get; set; }
        [ForeignKey("ServiceId")]
        [InverseProperty("InvExfTransactions")]
        public virtual RefService Service { get; set; }
        [ForeignKey("StatusId")]
        [InverseProperty("InvExfTransactions")]
        public virtual InvExfStatus Status { get; set; }
        [ForeignKey("SupplierId")]
        [InverseProperty("InvExfTransactionSuppliers")]
        public virtual SuSupplier Supplier { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("InvExfTransactionUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("ExtraFee")]
        public virtual ICollection<InvExfContactDetail> InvExfContactDetails { get; set; }
        [InverseProperty("Exftransaction")]
        public virtual ICollection<InvExfTranDetail> InvExfTranDetails { get; set; }
        [InverseProperty("ExtraFee")]
        public virtual ICollection<InvExfTranStatusLog> InvExfTranStatusLogs { get; set; }
        [InverseProperty("ExtraFee")]
        public virtual ICollection<InvExtTranTax> InvExtTranTaxes { get; set; }
    }
}