using DTO.Customer;
using System;
using System.Collections.Generic;

namespace DTO.Invoice
{
    public class InvoiceDetail
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
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
        public int? BilledQuantityType { get; set; }
        public double? BilledQuantity { get; set; }
        public double? ExchangeRate { get; set; }
        public double? RuleExchangeRate { get; set; }
        public int? InvoiceTo { get; set; }
        public int? InvoiceType { get; set; }
        public int? InvoiceMethod { get; set; }
        public double? ManDays { get; set; }
        public int? TravelMatrixType { get; set; }
        public string InvoicedName { get; set; }
        public string InvoicedAddress { get; set; }
        public int? Office { get; set; }
        public string PaymentTerms { get; set; }
        public string PaymentDuration { get; set; }
        public int? BankId { get; set; }
        public bool? IsAutomation { get; set; }
        public bool? IsInspection { get; set; }
        public bool? IsTravelExpense { get; set; }
        public int? InspectionId { get; set; }
        public int? AuditId { get; set; }
        public int? InvoiceStatus { get; set; }
        public int? InvoicePaymentStatus { get; set; }
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
        public DateTime? CreatedOn { get; set; }
        public double? TravelOtherFees { get; set; }
        public double? TravelTotalFees { get; set; }
        public string ProrateBookingNumbers { get; set; }

        // for internal logic
        public string GroupBy { get; set; }
        public DateTime InspectionDate { get; set; }
        public int? FactoryId { get; set; }
        public int FactoryCountryId { get; set; }
        public int FactoryCountyId { get; set; }
        public int FactoryProvinceId { get; set; }
        public int FactoryCityId { get; set; }
        public string FactoryCountryName { get; set; }
        public string FactoryCountryCode { get; set; }
        public CustomerPriceCardRepo Rule { get; set; }
        public int QuotationId { get; set; }
        public IEnumerable<int> BookingBuyerIds { get; set; }
        public IEnumerable<int> BookingDepartmentIds { get; set; }
        public IEnumerable<int> BookingBrandIds { get; set; }
        public int? PriceCalculationtype { get; set; }
        public double? AdditionalBdTax { get; set; }
    }
}
