using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_AUT_TRAN_Details")]
    public partial class InvAutTranDetail
    {
        public InvAutTranDetail()
        {
            InvAutTranContactDetails = new HashSet<InvAutTranContactDetail>();
            InvAutTranStatusLogs = new HashSet<InvAutTranStatusLog>();
            InvAutTranTaxes = new HashSet<InvAutTranTax>();
            InvCreTranClaimDetails = new HashSet<InvCreTranClaimDetail>();
            InvExfTransactions = new HashSet<InvExfTransaction>();
            InvTranFiles = new HashSet<InvTranFile>();
        }

        public int Id { get; set; }
        [StringLength(1000)]
        public string InvoiceNo { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? InvoiceDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PostedDate { get; set; }
        public double? UnitPrice { get; set; }
        public double? InspectionFees { get; set; }
        public double? TravelAirFees { get; set; }
        public double? TravelLandFees { get; set; }
        public double? HotelFees { get; set; }
        public double? OtherFees { get; set; }
        public double? Discount { get; set; }
        public double? TotalTaxAmount { get; set; }
        public double? TaxValue { get; set; }
        public double? TotalInvoiceFees { get; set; }
        public int? TotalSampleSize { get; set; }
        public int? PriceCardCurrency { get; set; }
        public int? InvoiceCurrency { get; set; }
        public double? ExchangeRate { get; set; }
        public double? RuleExchangeRate { get; set; }
        public int? InvoiceTo { get; set; }
        public int? InvoiceMethod { get; set; }
        public double? ManDays { get; set; }
        public int? TravelMatrixType { get; set; }
        [StringLength(1000)]
        public string InvoicedName { get; set; }
        [StringLength(2000)]
        public string InvoicedAddress { get; set; }
        public int? Office { get; set; }
        [StringLength(2000)]
        public string PaymentTerms { get; set; }
        [StringLength(1000)]
        public string PaymentDuration { get; set; }
        public int? BankId { get; set; }
        public bool? IsAutomation { get; set; }
        public bool? IsInspection { get; set; }
        public bool? IsTravelExpense { get; set; }
        public int? InspectionId { get; set; }
        public int? InvoiceStatus { get; set; }
        public int? InvoicePaymentStatus { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? InvoicePaymentDate { get; set; }
        public int? RuleId { get; set; }
        public int? CalculateInspectionFee { get; set; }
        public int? CalculateTravelExpense { get; set; }
        public int? CalculateHotelFee { get; set; }
        public int? CalculateDiscountFee { get; set; }
        public int? CalculateOtherFee { get; set; }
        public string Remarks { get; set; }
        public string Subject { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public double? TravelOtherFees { get; set; }
        public double? TravelTotalFees { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        [StringLength(1000)]
        public string ProrateBookingNumbers { get; set; }
        public int? InvoiceType { get; set; }
        public int? AuditId { get; set; }
        public int? ServiceId { get; set; }
        public int? EntityId { get; set; }
        public int? PriceCalculationType { get; set; }
        [Column("Additional_BD_Tax")]
        public double? AdditionalBdTax { get; set; }

        [ForeignKey("AuditId")]
        [InverseProperty("InvAutTranDetails")]
        public virtual AudTransaction Audit { get; set; }
        [ForeignKey("BankId")]
        [InverseProperty("InvAutTranDetails")]
        public virtual InvRefBank Bank { get; set; }
        [ForeignKey("CalculateDiscountFee")]
        [InverseProperty("InvAutTranDetailCalculateDiscountFeeNavigations")]
        public virtual InvRefFeesFrom CalculateDiscountFeeNavigation { get; set; }
        [ForeignKey("CalculateHotelFee")]
        [InverseProperty("InvAutTranDetailCalculateHotelFeeNavigations")]
        public virtual InvRefFeesFrom CalculateHotelFeeNavigation { get; set; }
        [ForeignKey("CalculateInspectionFee")]
        [InverseProperty("InvAutTranDetailCalculateInspectionFeeNavigations")]
        public virtual InvRefFeesFrom CalculateInspectionFeeNavigation { get; set; }
        [ForeignKey("CalculateOtherFee")]
        [InverseProperty("InvAutTranDetailCalculateOtherFeeNavigations")]
        public virtual InvRefFeesFrom CalculateOtherFeeNavigation { get; set; }
        [ForeignKey("CalculateTravelExpense")]
        [InverseProperty("InvAutTranDetailCalculateTravelExpenseNavigations")]
        public virtual InvRefFeesFrom CalculateTravelExpenseNavigation { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("InvAutTranDetails")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("InvAutTranDetails")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("InspectionId")]
        [InverseProperty("InvAutTranDetails")]
        public virtual InspTransaction Inspection { get; set; }
        [ForeignKey("InvoiceCurrency")]
        [InverseProperty("InvAutTranDetailInvoiceCurrencyNavigations")]
        public virtual RefCurrency InvoiceCurrencyNavigation { get; set; }
        [ForeignKey("InvoiceMethod")]
        [InverseProperty("InvAutTranDetails")]
        public virtual QuBillMethod InvoiceMethodNavigation { get; set; }
        [ForeignKey("InvoicePaymentStatus")]
        [InverseProperty("InvAutTranDetails")]
        public virtual InvPaymentStatus InvoicePaymentStatusNavigation { get; set; }
        [ForeignKey("InvoiceStatus")]
        [InverseProperty("InvAutTranDetails")]
        public virtual InvStatus InvoiceStatusNavigation { get; set; }
        [ForeignKey("InvoiceTo")]
        [InverseProperty("InvAutTranDetails")]
        public virtual QuPaidBy InvoiceToNavigation { get; set; }
        [ForeignKey("InvoiceType")]
        [InverseProperty("InvAutTranDetails")]
        public virtual RefInvoiceType InvoiceTypeNavigation { get; set; }
        [ForeignKey("Office")]
        [InverseProperty("InvAutTranDetails")]
        public virtual InvRefOffice OfficeNavigation { get; set; }
        [ForeignKey("PriceCalculationType")]
        [InverseProperty("InvAutTranDetails")]
        public virtual InvRefPriceCalculationType PriceCalculationTypeNavigation { get; set; }
        [ForeignKey("PriceCardCurrency")]
        [InverseProperty("InvAutTranDetailPriceCardCurrencyNavigations")]
        public virtual RefCurrency PriceCardCurrencyNavigation { get; set; }
        [ForeignKey("RuleId")]
        [InverseProperty("InvAutTranDetails")]
        public virtual CuPrDetail Rule { get; set; }
        [ForeignKey("ServiceId")]
        [InverseProperty("InvAutTranDetails")]
        public virtual RefService Service { get; set; }
        [ForeignKey("TravelMatrixType")]
        [InverseProperty("InvAutTranDetails")]
        public virtual InvTmType TravelMatrixTypeNavigation { get; set; }
        [InverseProperty("Invoice")]
        public virtual ICollection<InvAutTranContactDetail> InvAutTranContactDetails { get; set; }
        [InverseProperty("Invoice")]
        public virtual ICollection<InvAutTranStatusLog> InvAutTranStatusLogs { get; set; }
        [InverseProperty("Invoice")]
        public virtual ICollection<InvAutTranTax> InvAutTranTaxes { get; set; }
        [InverseProperty("Invoice")]
        public virtual ICollection<InvCreTranClaimDetail> InvCreTranClaimDetails { get; set; }
        [InverseProperty("Invoice")]
        public virtual ICollection<InvExfTransaction> InvExfTransactions { get; set; }
        [InverseProperty("Invoice")]
        public virtual ICollection<InvTranFile> InvTranFiles { get; set; }
    }
}