using DTO.DynamicFields;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DTO.Quotation
{
    

    public class BookingDynamicFields
    {
        public int BookingNo { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }
       
       
    }
    public class QuotationInspProdExportItem
    {
        [Description("Quotation Number")]
        public int QuotationNo { get; set; }
        [Description("Booking No")]
        public int BookingNo { get; set; }
        [Description("Customer Booking No")]
        public string CustomerBookingNo { get; set; }
        public string Customer { get; set; }
        [Description("Brand")]
        public string BrandName { get; set; }
        [Description("Department")]
        public string DepartmentName { get; set; }
        [Description("Buyer")]
        public string BuyerName { get; set; }

        public string Supplier { get; set; }
        [Description("Supplier Code")]
        public string SupplierCode { get; set; }
        public string Factory { get; set; }
        [Description("Country")]
        public string CountryName { get; set; }
        [Description("Province")]
        public string ProvinceName { get; set; }
        [Description("City")]
        public string CityName { get; set; }

        [Description("County")]
        public string CountyName { get; set; }
        [Description("Quotation Date")]
        public DateTime QuotationDate { get; set; }
        [Description("Service From Date")]
        public DateTime ServiceFromDate { get; set; }
        [Description("Service To Date")]
        public DateTime ServiceToDate { get; set; }
        [Description("Service Type")]
        public string ServiceType { get; set; }
        public string Office { get; set; }

        [Description("Quotation Status")]
        public string Status { get; set; }
        [Description("Total Man Day")]
        public double TotalManDay { get; set; }
        [Description("Inspection Cost")]
        public double InspectionCost { get; set; }
        public double? Discount { get; set; }
        [Description("Travel Cost")]
        public double? TravelCost { get; set; }

        [Description("Other Cost")]
        public double OtherCost { get; set; }
        [Description("Total Fees")]
        public double TotalFees { get; set; }
        public string Currency { get; set; }
        [Description("Bill Paid By")]
        public string BillPaidBy { get; set; }
        [Description("Bill Paid By Address")]
        public string BillPaidByAddress { get; set; }

        [Description("Bill Paid By Contact")]
        public string BillPaidByContact { get; set; }
        [Description("API Remark")]
        public string APIRemark { get; set; }
        [Description("Customer Remark")]
        public string CustomerRemark { get; set; }
        [Description("Customer Legal Name")]
        public string CustomerLegalName { get; set; }
        [Description("Supplier Legal Name")]
        public string SupplierLegalName { get; set; }

        [Description("Factory Legal Name")]
        public string FactoryLegalName { get; set; }
        [Description("Product Reference")]
        public string ProductReference { get; set; }
        [Description("Product Description")]
        public string ProductDesc { get; set; }
        [Description("Po Number")]
        public string PoNumber { get; set; }
        [Description("Container Name")]
        public string ContainerName { get; set; }

        [Description("Booking Quantity")]
        public int BookingQty { get; set; }
        [Description("Sample Size")]
        public int? SampleSize { get; set; }
        [Description("Report Result")]
        public string ReportStatus { get; set; }
        [Description("Factory Reference")]
        public string FactoryReference { get; set; }
        [Description("Actual Insp. Date")]
        public string ActualInspectionDate { get; set; }

        [Description("Inspection Status")]
        public string InspectionStatus { get; set; }
        [Description("Billing Entity")]
        public string BillingEntity { get; set; }
        [Description("Invoice No")]
        public string InvoiceNo { get; set; }
        [Description("Invoice Date")]
        public DateTime? InvoiceDate { get; set; }
        [Description("Invoice Remarks")]
        public string InvoiceRemarks { get; set; }

        [Description("Payment Term")]
        public string PaymentTerm { get; set; }
        [Description("Travel Distance")]
        public double? Traveldistance { get; set; }
        [Description("Travel Time")]
        public double? TravelTime { get; set; }
        [Description("Validated By")]
        public string ValidatedByName { get; set; }
        [Description("Validated On")]
        public DateTime? ValidatedOn { get; set; }
        [Description("Calculated Working Hours")]
        public double? CalculatedWorkingHours { get; set; }
        [Description("Calculated Working Manday")]
        public double? CalculatedWorkingManday { get; set; }
        [Description("Billed Qty Type")]
        public string BilledQtyType { get; set; }
        [Description("Quantity")]
        public int? Quantity { get; set; }
    }

    public class QuotationAuditExportItem
    {
        [Description("Quotation Number")]
        public int QuotationNo { get; set; }
        [Description("Booking No")]
        public int BookingNo { get; set; }
        public string Customer { get; set; }
        [Description("Brand")]
        public string BrandName { get; set; }
        [Description("Department")]
        public string DepartmentName { get; set; }
        public string Supplier { get; set; }
        [Description("Supplier Code")]
        public string SupplierCode { get; set; }
        public string Factory { get; set; }
        [Description("Quotation Date")]
        public DateTime QuotationDate { get; set; }
        [Description("Service From Date")]
        public DateTime ServiceFromDate { get; set; }
        [Description("Service To Date")]
        public DateTime ServiceToDate { get; set; }
        [Description("Service Type")]
        public string ServiceType { get; set; }
        public string Office { get; set; }
        [Description("Quotation Status")]
        public string Status { get; set; }
        [Description("Total Man Day")]
        public double TotalManDay { get; set; }
        [Description("Inspection Cost")]
        public double InspectionCost { get; set; }
        public double? Discount { get; set; }
        [Description("Travel Cost")]
        public double? TravelCost { get; set; }
        [Description("Other Cost")]
        public double OtherCost { get; set; }
        [Description("Total Fees")]
        public double TotalFees { get; set; }
        public string Currency { get; set; }
        [Description("Bill Paid By")]
        public string BillPaidBy { get; set; }
        [Description("Bill Paid By Address")]
        public string BillPaidByAddress { get; set; }
        [Description("Bill Paid By Contact")]
        public string BillPaidByContact { get; set; }
        [Description("API Remark")]
        public string APIRemark { get; set; }
        [Description("Customer Remark")]
        public string CustomerRemark { get; set; }
        [Description("Customer Legal Name")]
        public string CustomerLegalName { get; set; }
        [Description("Supplier Legal Name")]
        public string SupplierLegalName { get; set; }
        [Description("Factory Legal Name")]
        public string FactoryLegalName { get; set; }
        [Description("Country")]
        public string CountryName { get; set; }
        [Description("Province")]
        public string ProvinceName { get; set; }
        [Description("City")]
        public string CityName { get; set; }
        [Description("County")]
        public string CountyName { get; set; }
        [Description("Actual Audit Date")]
        public string ActualInspectionDate { get; set; }
        [Description("Audit Status")]
        public string InspectionStatus { get; set; }
        [Description("Billing Entity")]
        public string BillingEntity { get; set; }
        [Description("Invoice No")]
        public string InvoiceNo { get; set; }
        [Description("Invoice Date")]
        public DateTime?  InvoiceDate { get; set; }
        [Description("Invoice Remarks")]
        public string InvoiceRemarks { get; set; }
        [Description("Payment Term")]
        public string PaymentTerm { get; set; }
        [Description("Travel Distance")]
        public double? Traveldistance { get; set; }
        [Description("Travel Time")]
        public double? TravelTime { get; set; }
        [Description("Validated By")]
        public string ValidatedByName { get; set; }
        [Description("Validated On")]
        public DateTime? ValidatedOn { get; set; }
    }
    

        public class QuotationExportDataResponse
    {
        public List<QuotationInspProdExportItem> QuotationInspProdExportList { get; set; }
        public List<QuotationAuditExportItem> QuotationAuditExportList { get; set; }
        public QuotationExportResult Result { get; set; }
    }

    public enum QuotationExportResult
    {
        Success=1,
        NotFound=2
    }
}
