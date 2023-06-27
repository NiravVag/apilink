using DTO.Common;
using DTO.CommonClass;
using DTO.DynamicFields;
using DTO.Inspection;
using DTO.Invoice;
using DTO.Quotation;
using DTO.Report;
using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DTO.Kpi
{
    public class KpiResponse
    {
        public List<CommonDataSource> CustomerList { get; set; }
        public List<CommonDataSource> ServiceTypeList { get; set; }
        public List<CommonDataSource> OfficeList { get; set; }
    }

    public class KpiRequest
    {
        public DateObject FromDate { get; set; }
        public DateObject ToDate { get; set; }
        public int? CustomerId { get; set; }
        public int TemplateId { get; set; }
        public int? BookingNo { get; set; }
        public List<int> OfficeIdLst { get; set; }
        public List<int> ServiceTypeIdLst { get; set; }
        public string InvoiceNo { get; set; }
        public List<int> BrandIdList { get; set; }
        public List<int> DepartmentIdList { get; set; }
        public IEnumerable<int> CountryIds { get; set; }
        public IEnumerable<CustomerMandayGroupByFields> CustomerMandayGroupByFields { get; set; }
        public IEnumerable<int> StatusIds { get; set; }
        public List<int> InvoiceTypeIdList { get; set; }
        public List<int> PaymentStatusIdList { get; set; }
        public List<int> CustomerIdList { get; set; }
        public bool IsInvoice { get; set; }
    }

    public class ExpenseTemplateItem
    {
        [Description("Insp. No.")]
        public int BookingNo { get; set; }
        [Description("Customer Ref.")]
        public string CustomerBookingNo { get; set; }
        [Description("Customer Name")]
        public string CustomerName { get; set; }
        [Description("Buyer")]
        public string BuyerName { get; set; }
        [Description("Department")]
        public string DeptCode { get; set; }
        [Description("Customer Contact")]
        public string CustomerContact { get; set; }
        [Description("Collection")]
        public string CollectionName { get; set; }
        [Description("Inspection Office")]
        public string Office { get; set; }
        [Description("Supplier")]
        public string SupplierName { get; set; }
        [Description("Factory")]
        public string FactoryName { get; set; }
        [Description("Factory Address")]
        public string FactoryAddress { get; set; }
        [Description("Country")]
        public string FactoryCountry { get; set; }
        [Description("Province")]
        public string FactoryState { get; set; }
        [Description("City")]
        public string FactoryCity { get; set; }
        [Description("County")]
        public string FactoryCounty { get; set; }

        [Description("PO No")]
        public string PONumber { get; set; }
        [Description("Service Type")]
        public string ServiceTypeName { get; set; }
        [Description("Status")]
        public string bookingStatus { get; set; }
        [Description("Product Category")]
        public string ProductCategory { get; set; }
        [Description("Product Subcat 1")]
        public string ProductSubCategory { get; set; }
        [Description("Product Subcat 2")]
        public string ProductSubCategory2 { get; set; }
        [Description("Service Date From")]
        public DateTime? InspectionStartDate { get; set; }
        [Description("Service Date To")]
        public DateTime? InspectionEndDate { get; set; }
        [Description("Month")]
        public int Month { get; set; }
        [Description("Year")]
        public int Year { get; set; }
        [Description("Invoice No.")]
        public string InvoiceNumber { get; set; }
        [Description("Invoice Date")]
        public string InvoiceDate { get; set; }
        [Description("Charge To Cust/Supp")]
        public string PaidBy { get; set; }
        [Description("Mandays")]
        public double ManDay { get; set; }
        [Description("Inspection Fee")]
        public double? InspectionFee { get; set; }
        [Description("Travel costs")]
        public double? TravellingCost { get; set; }
        [Description("Other Expense")]
        public double? OtherFee { get; set; }
        [Description("Extra Fee Total")]
        public double? ExtraFee { get; set; }
        [Description("Discount")]
        public double? Discount { get; set; }
        [Description("Total Fee")]
        public double? TotalInspectionFee { get; set; }
        [Description("Inspection Fee (Invoice)")]
        public double? Invoice_InspectionFee { get; set; }
        [Description("Travel costs(Invoice)")]
        public double? Invoice_TravelFee { get; set; }
        [Description("Hotel costs(Invoice)")]
        public double? Invoice_HotelFee { get; set; }
        [Description("Other Expense(Invoice)")]
        public double? Invoice_OtherFee { get; set; }
        [Description("Discount(Invoice)")]
        public double? Invoice_Discount { get; set; }
        [Description("ExtraFees(Invoice)")]
        public double? Invoice_ExtraFee { get; set; }
        [Description("Total Fee(Invoice)")]
        public double? Invoice_TotalFee { get; set; }
        [Description("Currency(Invoice)")]
        public string Invoice_Currency { get; set; }
        [Description("Inspector Name")]
        public string QcmName { get; set; }
        [Description("Product Count")]
        public int? ProductCount { get; set; }
        [Description("Report Count")]
        public int TotalReports { get; set; }
        [Description("Sample Size Count")]
        public string SampleSize { get; set; }
        [Description("# Passed")]
        public int TotalReportPass { get; set; }
        [Description("# Pending")]
        public int TotalReportPending { get; set; }
        [Description("# Failed")]
        public int TotalReportFail { get; set; }
        [Description("# Missing")]
        public int TotalReportMissing { get; set; }


        //[Description("Buyer")]
        //public string ProductName { get; set; }
        //[Description("Buyer")]
        //public string ProductDescription { get; set; }
        //[Description("Buyer")]
        //public string FactoryRef { get; set; }
        //[Description("Buyer")]
        //public string ReportResult { get; set; }
        //[Description("Buyer")]
        //public string AQLLevelName { get; set; }
        //[Description("Buyer")]
        //public string Merchandise { get; set; }
        //[Description("Buyer")]
        //public string CusDecisionDate { get; set; }
        //[Description("Buyer")]
        //public string CusDecisionName { get; set; }


    }

    public class ExpencesClaimsItem
    {
        public int ExpenseId { get; set; }
        public string ClaimNo { get; set; }
        public int ClaimDetailId { get; set; }
        public double Amount { get; set; }
        public double? AmmountHk { get; set; }
        public int InspectionId { get; set; }
    }

    public class InspectionQcKpiExpenseDetails
    {
        public int QcId { get; set; }
        public int InspectionId { get; set; }
        public int ExpenseId { get; set; }
        public int ExpenseType { get; set; }
        public string ClaimNumber { get; set; }
        public DateTime ClaimDate { get; set; }
        public string OutsourceCompany { get; set; }
        public string ClaimStatus { get; set; }
        public string OrderStatus { get; set; }
        public int? NumberOfManDay { get; set; }
        public double ClaimAmount { get; set; }
        public double? ClaimAmountHK { get; set; }
        public double? ServiceTax { get; set; }
        public string ClaimRemarks { get; set; }
        public string PayrollCurrency { get; set; }
        public string StartCity { get; set; }
        public string EndCity { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string ExpenseTypeName { get; set; }
        public string TripTypeName { get; set; }
    }

    public class InspectionQcKpiInvoiceDetails
    {
        public int InspectionId { get; set; }
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceCurrency { get; set; }
        public string InvoiceCurrencyCode { get; set; }
        public double? InvoiceInspectionFees { get; set; }
        public double? InvoiceTravellingFees { get; set; }
        public double? InvoiceOtherFees { get; set; }
        public double? InvoiceTotalFees { get; set; }
        public double? InvoiceHotelFees { get; set; }
        public double? InvoiceTotalTax { get; set; }
        public double? InvoiceManDay { get; set; }
    }

    public class ReportResultTemplateItem
    {

        [Description("Insp. No.")]
        public int BookingNo { get; set; }
        [Description("Customer Ref.")]
        public string CustomerBookingNo { get; set; }
        [Description("Customer Name")]
        public string CustomerName { get; set; }
        [Description("Buyer")]
        public string BuyerName { get; set; }
        [Description("Department")]
        public string DeptCode { get; set; }
        [Description("Customer Contact")]
        public string CustomerContact { get; set; }
        [Description("Collection")]
        public string CollectionName { get; set; }
        [Description("Inspection Office")]
        public string Office { get; set; }
        [Description("Supplier")]
        public string SupplierName { get; set; }
        [Description("Factory")]
        public string FactoryName { get; set; }
        [Description("Country")]
        public string FactoryCountry { get; set; }
        [Description("Province")]
        public string FactoryState { get; set; }
        [Description("City")]
        public string FactoryCity { get; set; }

        [Description("Service Type")]
        public string ServiceTypeName { get; set; }
        [Description("Status")]
        public string bookingStatus { get; set; }

        [Description("Service Date From")]
        public DateTime? InspectionStartDate { get; set; }
        [Description("Service Date To")]
        public DateTime? InspectionEndDate { get; set; }
        [Description("Month")]
        public int Month { get; set; }
        [Description("Year")]
        public int Year { get; set; }

        [Description("Mandays")]
        public double ManDay { get; set; }

        [Description("Product Ref.")]
        public string ProductName { get; set; }
        [Description("ProductDescription")]
        public string ProductDescription { get; set; }
        [Description("Factory Ref.")]
        public string FactoryRef { get; set; }
        [Description("AQL")]
        public string AQLLevelName { get; set; }
        [Description("Overall result")]
        public string ReportResult { get; set; }
        [Description("PO No")]
        public string PONumber { get; set; }
        [Description("Invoice No.")]
        public string InvoiceNumber { get; set; }
        [Description("Invoice Date")]
        public string InvoiceDate { get; set; }

        [Description("Product Category")]
        public string ProductCategory { get; set; }
        [Description("Product Subcat 1")]
        public string ProductSubCategory { get; set; }
        [Description("Product Subcat 2")]
        public string ProductSubCategory2 { get; set; }
        [Description("Customer Decision Result")]
        public string CusDecisionName { get; set; }
        [Description("Customer Decision Date")]
        public DateTime? CusDecisionDate { get; set; }

        public int? ReportId { get; set; }

        [Description("Previous Booking No.")]
        public int? PreviousBookingNo { get; set; }

    }

    public class ExportTemplateItem
    {
        public string Office { get; set; }
        public int BookingNo { get; set; }
        public string CustomerBookingNo { get; set; }
        public string bookingStatus { get; set; }
        public string PONumber { get; set; }
        public string BuyerName { get; set; }
        public string BrandName { get; set; }
        public string DeptCode { get; set; }
        public string EciOffice { get; set; }
        public string Bdm { get; set; }
        public string Merchandise { get; set; }
        public string Merchandise2 { get; set; }
        public string QcmName { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        public string FactoryName { get; set; }
        public string FactoryCode { get; set; }
        public string FactoryCity { get; set; }
        public string FactoryState { get; set; }
        public string FactoryCountry { get; set; }
        public string FactoryAddress { get; set; }
        public string InspectionStartDate { get; set; }
        public string InspectionEndDate { get; set; }
        public string ReportDate { get; set; }
        public string ProductDescription { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSubCategory { get; set; }
        public string ProductSubCategory2 { get; set; }
        public string ShipmentQty { get; set; }
        public string PartialShiptment { get; set; }
        public string SampleSize { get; set; }
        public bool IsCombined { get; set; }
        public string WMCode { get; set; }
        public double InspectionFeePerUnit { get; set; }
        public double ManDay { get; set; }
        public double? InspectionFee { get; set; }
        public double WMDFee { get; set; }
        public string Quotationcomment { get; set; }
        public double? TravellingCost { get; set; }
        public double? TotalInspectionFee { get; set; }
        public int TotalReports { get; set; }
        public string PaidBy { get; set; }
        public string BookingRemarks { get; set; }
        public string ConfirmDate { get; set; }
        public string ReConfirmDate { get; set; }
        public bool IsPicking { get; set; }
        public string FactoryRef { get; set; }
        public double? TMDFee { get; set; }
        public double? NoOfTMD { get; set; }
        public double? AirTicket { get; set; }
        public double? HotelFee { get; set; }
        public double? TravelTime { get; set; }
        public string ReportNo { get; set; }
        public string ReportResult { get; set; }
        public string ReportRemarks { get; set; }
        public string ReportStatus { get; set; }
        public string FinalReportStatus { get; set; }
        public string Barcode { get; set; }
        public string Etd { get; set; }
        public string InspectionReportDate { get; set; }
        public int TotalReportPass { get; set; }
        public int TotalReportFail { get; set; }
        public int TotalReportPending { get; set; }
        public int TotalReportMissing { get; set; }
        public double ReportPassPercentage { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string MonthName { get; set; }
        public string SupContactDate { get; set; }
        public string SupContactDeadlineDate { get; set; }
        public int? ReinspectionId { get; set; }
        public string TotalQty { get; set; }
        public string CartonQty { get; set; }
        public string FactoryContact { get; set; }
        public string CustomerContact { get; set; }
        public string SupContact { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int? ProductCount { get; set; }
        public int? QcCount { get; set; }
        public int? CombineId { get; set; }
        public int CombineProductQty { get; set; }
        public List<InspectionBookingDFData> bookingDFList { get; set; }

        public string ProductName { get; set; }
        public string ServiceName { get; set; }
        public string ServiceTypeName { get; set; }
        public string AQLLevelName { get; set; }
        public string InspectionName { get; set; }

        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceSentDate { get; set; }
        public string QuotationDate { get; set; }
        public string CurrencyName { get; set; }
        public int? QuotationNumber { get; set; }

        //Gifi
        public string GifiOfficeName { get; set; }
        public string GifiQAContactName { get; set; }
        public string ColorTargetAvailable { get; set; }
        public string ColorCheckFinding { get; set; }
        public string GoldenSampleAvailable { get; set; }
        public string GoldenSampleFinding { get; set; }

        //ECI Remart template
        public string FBRemarkResult { get; set; }
        public int? FBRemarkNumber { get; set; }

        public int? CriticalMax { get; set; }
        public int? MajorMax { get; set; }
        public int? MinorMax { get; set; }

        public int? CriticalDefect { get; set; }
        public int? MajorDefect { get; set; }
        public int? MinorDefect { get; set; }

        public string CriticalResult { get; set; }
        public string MajorResult { get; set; }
        public string MinorResult { get; set; }

        public string CollectionName { get; set; }
        public string PriceCategory { get; set; }
        public string DefectDesc { get; set; }
        public string DefectCategory { get; set; }

        public List<FbReportInspSummaryResult> FbResult { get; set; }
        public int? SerialNo { get; set; }
        public string RemarkCategory { get; set; }
        public string RemarkSubCategory { get; set; }
        public string RemarkSubCategory2 { get; set; }
        public string CustomerRemarkCodeReference { get; set; }
        public string ProductUnitName { get; set; }
        public string FbReportComments { get; set; }

        public string BatteryType { get; set; }
        public string BatteryModel { get; set; }
        public string BatteryQuantity { get; set; }
        public string BatteryNetWeight { get; set; }

        public double? PieceNo { get; set; }
        public string MaterialGroup { get; set; }
        public string MaterialCode { get; set; }
        public string PackingLocation { get; set; }
        public string PackingQuantity { get; set; }
        public string PackingNetWeight { get; set; }

        public double? PercentageVolume { get; set; }
        public double? PercentageWeight { get; set; }

        public double? PCB { get; set; }
        public string SpecClientValuesLength { get; set; }
        public string SpecClientValuesWidth { get; set; }
        public string SpecClientValuesHeight { get; set; }
        public string SpecClientValuesWeight { get; set; }
        public double? SpecClientVolume { get; set; }

        public string MeasuredValuesLength { get; set; }
        public string MeasuredValuesWidth { get; set; }
        public string MeasuredValuesHeight { get; set; }
        public string MeasuredValuesWeight { get; set; }
        public double? MeasuredVolume { get; set; }

        public string OtherRemarks { get; set; }
        public int? AQLQuantity { get; set; }
        public double? InspectedQty { get; set; }
        public string RescheduleReason { get; set; }

        public string FirstServiceDateFrom { get; set; }
        public string FirstServiceDateTo { get; set; }

        public string InspMonthName { get; set; }
        public int? InspMonthNumber { get; set; }
        public double? OtherFee { get; set; }
        public string DestinationCountry { get; set; }
        public string OrderQty { get; set; }
        public string DeptDivision { get; set; }
        public string OfficeLocationName { get; set; }

        public int ReportId { get; set; }
        public int QuotationId { get; set; }
        public string ProrateBookingNo { get; set; }
        public List<FBReportInspSubSummary> ResultList { get; set; }
        public string InvoiceRemarks { get; set; }

        public string CusDecisionName { get; set; }
        public string CusDecisionDate { get; set; }

        public int? BookingFormSerial { get; set; }
        public string InvoiceTo { get; set; }
        public double? UnitPrice { get; set; }
        public double? ExtraFee { get; set; }
        public double? Discount { get; set; }

        public double? Invoice_InspectionFee { get; set; }
        public double? Invoice_TravelFee { get; set; }
        public double? Invoice_TravelLandFee { get; set; }
        public double? Invoice_HotelFee { get; set; }
        public double? Invoice_OtherFee { get; set; }
        public double? Invoice_Discount { get; set; }
        public double? Invoice_TotalFee { get; set; }
        public string Invoice_Currency { get; set; }
        public double? Invoice_ExtraFee { get; set; }

        public string ServiceDate { get; set; }
        public string ServiceFromDate { get; set; }
        public string ServiceToDate { get; set; }
        public int PoQuantity { get; set; }
        public int TotalDefects { get; set; }
        public int TotalQtyReworked { get; set; }
        public int TotalQtyReplaced { get; set; }
        public int TotalQtyRejected { get; set; }
        public int ProductId { get; set; }

        // inspection picking data
        public string PickingProductName { get; set; }
        public string PickingProductCategory { get; set; }
        public string PickingProductSubCategory { get; set; }
        public string PickingPoNumber { get; set; }
        public string PickingCustomerName { get; set; }
        public string PickingSupplierName { get; set; }
        public string PickingFactoryName { get; set; }
        public int PickingInspectionId { get; set; }
        public string PickingServiceDate { get; set; }
        public string PickingLabName { get; set; }
        public int PickingQuantity { get; set; }

        public string NewProduct { get; set; }
        public string Season { get; set; }
        public int? SeasonYear { get; set; }
        public string Color { get; set; }
        public string BookingCreationDate { get; set; }
        public string BookingCreatedBy { get; set; }
        public string TotalStaff { get; set; }
        public string CustomerProductCategory { get; set; }
        public double ProducedQty { get; set; }
        public string BillingMode { get; set; }
        public double? BillingManDays { get; set; }
        public string InspectionLocation { get; set; }
        public string Inspectors { get; set; }
        public string OfficeCountry { get; set; }
        public string InvoiceContact { get; set; }


        //Inspeciton Data        
        public string MajorDefects { get; set; }
        public string MinorDefects { get; set; }
        public string CriticalDefects { get; set; }
        public string CustomerResult { get; set; }
        public string ShipmentDate { get; set; }

        //customerDecision
        public string CustomerDecision { get; set; }
        public string CustomerDecisionComments { get; set; }
    }

    public class KpiReportRemarksTemplateRepo
    {
        public int BookingNo { get; set; }
        public string Office { get; set; }
        public DateTime? InspectionStartDate { get; set; }
        public DateTime InspectionEndDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerBookingNo { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public string ServiceTypeName { get; set; }
        public string ProductName { get; set; }
        public string FactoryCountry { get; set; }
        public string BuyerName { get; set; }
        public string CustomerContact { get; set; }
        public string ReportResult { get; set; }
        public string CollectionName { get; set; }
        public string BookingStatus { get; set; }
        public string PaidBy { get; set; }
        public string PONumber { get; set; }
        public string DeptCode { get; set; }
        public string ProductDescription { get; set; }
        public string FactoryRef { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSubCategory { get; set; }
        public string ProductSubCategory2 { get; set; }
        public int? SerialNo { get; set; }
        public int? FBRemarkNumber { get; set; }
        public string FBRemarkResult { get; set; }
        public string ReportRemarks { get; set; }
        public string RemarkCategory { get; set; }
        public string RemarkSubCategory { get; set; }
        public string RemarkSubCategory2 { get; set; }
        public string CustomerRemarkCodeReference { get; set; }
        public int? CombineId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int ReportId { get; set; }
        public int BillPaidBy { get; set; }
        public int? ProductId { get; set; }
    }

    public class KpiReportRemarksTemplate
    {
        [Description("Inspection Number")]
        public int BookingNo { get; set; }
        [Description("Customer Ref")]
        public string CustomerBookingNo { get; set; }
        [Description("Customer Name")]
        public string CustomerName { get; set; }
        [Description("Buyer")]
        public string BuyerName { get; set; }
        [Description("Department")]
        public string DeptCode { get; set; }
        [Description("Customer Contact")]
        public string CustomerContact { get; set; }
        [Description("Collection")]
        public string CollectionName { get; set; }
        [Description("Inspection Office")]
        public string Office { get; set; }
        [Description("Supplier")]
        public string SupplierName { get; set; }
        [Description("Factory")]
        public string FactoryName { get; set; }
        [Description("Country")]
        public string FactoryCountry { get; set; }
        [Description("Service Type")]
        public string ServiceTypeName { get; set; }
        [Description("Status")]
        public string BookingStatus { get; set; }
        [Description("Service Date From")]
        public DateTime? InspectionStartDate { get; set; }
        [Description("Service Date To")]
        public DateTime? InspectionEndDate { get; set; }
        [Description("Month")]
        public int Month { get; set; }
        [Description("Year")]
        public int Year { get; set; }
        [Description("Charge To Cust/Supp")]
        public string BillPaidBy { get; set; }
        [Description("PO NO")]
        public string PONumber { get; set; }
        [Description("Product Ref.")]
        public string ProductName { get; set; }
        [Description("Product Description")]
        public string ProductDescription { get; set; }
        [Description("Factory Ref.")]
        public string FactoryRef { get; set; }
        [Description("Combined with")]
        public int? CombinedWith { get; set; }
        [Description("Overall Result")]
        public string ReportResult { get; set; }
        [Description("Remark No")]
        public int? FBRemarkNumber { get; set; }
        [Description("Remark")]
        public string ReportRemarks { get; set; }
        [Description("Remark Cat")]
        public string RemarkCategory { get; set; }
        [Description("Remark Subcat")]
        public string RemarkSubCategory { get; set; }
        [Description("Remark Subcat 2")]
        public string RemarkSubCategory2 { get; set; }
        [Description("Reference")]
        public string CustomerRemarkCodeReference { get; set; }
        [Description("Remark Result")]
        public string FBRemarkResult { get; set; }
        [Description("Product Category")]
        public string ProductCategory { get; set; }
        [Description("Product Subcat 1")]
        public string ProductSubCategory { get; set; }
        [Description("Product Subcat 2")]
        public string ProductSubCategory2 { get; set; }
    }

    public class BookingShipment
    {
        public int BookingId { get; set; }
        public int ReportId { get; set; }
        public double ShipmentQty { get; set; }
        public CuProduct ProductId { get; set; }
        public double TotalCartons { get; set; }
        public double? InspectedQty { get; set; }
        public double? PresentedQty { get; set; }
        public double? OrderQty { get; set; }
    }

    public class KPITemplate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? TypeId { get; set; }
    }

    public class KPITemplateRequest
    {
        public int? CustomerId { get; set; }
    }

    public class KpiPoDetails
    {
        public int PoTransactionId { get; set; }
        public int BookingId { get; set; }
        public string PoNumber { get; set; }
        public int? PickingQuantity { get; set; }
        public int QuotationStatus { get; set; }
        public string FactoryReference { get; set; }
        public string ProductId { get; set; }
        public string QuotationStatusName { get; set; }
        public int ProductTransId { get; set; }
        public int BookingQty { get; set; }
        public int ProductRefId { get; set; }
        public DateTime? Etd { get; set; }
        public string DestinationCountry { get; set; }
        public string Color { get; set; }
    }


    public class KpiInspectionBookingItems
    {
        public int BookingId { get; set; }
        public int? CustomerId { get; set; }
        public int? FactoryId { get; set; }
        public int? SupplierId { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public string FactoryCountry { get; set; }
        public string FactoryProvince { get; set; }
        public string ServiceType { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public DateTime? FirstServiceDateFrom { get; set; }
        public DateTime? FirstServiceDateTo { get; set; }
        public string Office { get; set; }
        public string OfficeCountry { get; set; }
        public int? OfficeId { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int StatusPriority { get; set; }
        public int? BookingCreatedBy { get; set; }
        public bool IsPicking { get; set; }
        public DateTime ApplyDate { get; set; }
        public InspStatus Status { get; set; }
        public CuCustomer Customer { get; set; }
        public string CustomerBookingNo { get; set; }
        public string BookingAPiRemarks { get; set; }
        public ICollection<InspTranCuDepartment> DeptCode { get; set; }
        public int? InspectionType { get; set; }
        public string CollectionName { get; set; }
        public int? PriceCategoryId { get; set; }
        public string PriceCategory { get; set; }
        public string Season { get; set; }
        public int? SeasonYear { get; set; }
        public string CreatedBy { get; set; }
        public string InspectionLocation { get; set; }
        public DateTime? ShipmentDate { get; set; }
        public int? PreviousBookingNo { get; set; }
        public string CustomerProductCategory { get; set; }
        public string BookingType { get; set; }
        public string GapPaymentOption { get; set; }
    }

    public class KpiBookingProductsData
    {
        public int Id { get; set; }

        public int BookingId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string PoNumber { get; set; }

        public string ProductImage { get; set; }

        public string ProductDescription { get; set; }

        public string ProductCategory { get; set; }

        public string ProductSubCategory { get; set; }

        public string ProductSubCategory2 { get; set; }

        public string FactoryReference { get; set; }

        public string ProductImageUrl { get; set; }

        public int? BookingQuantity { get; set; }

        public int? BookingStatus { get; set; }

        public int? InspectedQuantity { get; set; }

        public string InspectionDate { get; set; }

        public int? ReportId { get; set; }

        public string ReportResult { get; set; }

        public int? ReportResultId { get; set; }

        public string ReportPath { get; set; }
        public string ReportNo { get; set; }
        public double? PresentedQty { get; set; }
        public double? OrderQty { get; set; }
        public double? InspectedQty { get; set; }
        public string ReportStatus { get; set; }
        public string ExternalReportNo { get; set; }
        public DateTime? ServiceStartDate { get; set; }

        public DateTime? ServiceEndDate { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? CombineProductId { get; set; }

        public int? CombineProductCount { get; set; }

        public int? CombineAqlQuantity { get; set; }

        public bool IsParentProduct { get; set; }

        public bool IsPlaceHolderVisible { get; set; }

        public DateTime? SRDate { get; set; }

        public int AqlQty { get; set; }

        public string AQLName { get; set; }

        public string Barcode { get; set; }

        public int? CriticalMax { get; set; }
        public int? MajorMax { get; set; }
        public int? MinorMax { get; set; }
        public int? FBReportDetailId { get; set; }
        public int? UnitCount { get; set; }
        public string UnitName { get; set; }
        public string ReportResultName { get; set; }
        public string CustomerBookingNumber { get; set; }
        public int? BookingFormSerial { get; set; }
        public DateTime? Etd { get; set; }

        public bool? IsNewProduct { get; set; }
    }

    public class KPIManday
    {
        public double TravelManday { get; set; }
        public int BookingId { get; set; }
        public int QuotationId { get; set; }
        public double Manday { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrice { get; set; }
        public double? TravelTime { get; set; }
        public double HotelCost { get; set; }
        public double InspFee { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public double? Discount { get; set; }

    }

    public class KPIQuotDetails
    {
        public List<KPIManday> MandayList { get; set; }
        public List<ClientQuotationItem> QuotDetails { get; set; }
    }

    public class KPIExpenseQuotDetails
    {
        public List<KPIManday> MandayList { get; set; }
        public List<ExpenseClientQuotationItem> QuotDetails { get; set; }
    }

    public class KPIMerchandiser
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string Name { get; set; }
    }

    public class BookingCustomerBuyer
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string BuyerName { get; set; }
    }
    public class BookingContainerItem
    {
        public int? ReportId { get; set; }
        public int BookingId { get; set; }
        public int ContainerId { get; set; }
        public int TotalBookingQty { get; set; }
        public int? ReportResultId { get; set; }

    }

    public class CommonIdDate
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string Name { get; set; }
    }

    public class Reinspection
    {
        public int BookingId { get; set; }
        public int ReInspectionbookingId { get; set; }
    }


    public class FbReportRemarks
    {
        public int ReportId { get; set; }
        public int? ProductId { get; set; }
        public string Remarks { get; set; }
        public string SubCategory { get; set; }
        public string SubCategory2 { get; set; }
        public string Result { get; set; }
        public int InspSummaryId { get; set; }
        public string Reference { get; set; }
    }

    public class FBSampleType
    {
        public string SampleType { get; set; }
        public string Description { get; set; }
        public int FBReportId { get; set; }
        public string Comments { get; set; }
        public int? ProductId { get; set; }
    }
    public class FBOtherInformation
    {
        public string SubCategory { get; set; }
        public string SubCategory2 { get; set; }
        public string Remarks { get; set; }
        public int FBReportId { get; set; }
        public int? ProductId { get; set; }
    }
    public class FBReportDefects
    {
        public int? Critical { get; set; }
        public int? Major { get; set; }
        public int? Minor { get; set; }
        public int FBReportDetailId { get; set; }
        public int InspPoId { get; set; }
        public int ProductId { get; set; }
        public string DefectCategory { get; set; }
        public string DefectDesc { get; set; }
        public string PoNumber { get; set; }
        public string Position { get; set; }
        public string DefectCode { get; set; }
        public int? DefectCheckpoint { get; set; }
    }
    public class FBReportInspSubSummary
    {
        public int Id { get; set; }
        public string Result { get; set; }
        public int FBReportId { get; set; }
        public string Name { get; set; }
        public string Remarks { get; set; }
        public string ScoreValue { get; set; }
    }

    //public class FbReportDetailsSummary
    //{
    //    public int InspectionId { get; set; }
    //    public string ReportNo { get; set; }
    //    public double ProducedQty { get; set; }
    //    public double OrderQty { get; set; }
    //    public double InspectedQty { get; set; }
    //}

    public class BookingContacts
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string StaffName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class FbReportInspSummaryResult
    {
        public string Name { get; set; }
        public string Result { get; set; }
    }

    public class FbReportInspectionResult
    {
        public int ReportId { get; set; }
        public string Name { get; set; }
        public string Result { get; set; }
    }

    public class ExportResult
    {
        public List<string> ResultName { get; set; }
        public List<ExportTemplateItem> Data { get; set; }
    }

    public class ExportReportResult
    {
        public List<string> ResultName { get; set; }
        public List<FbReportInspectionResult> ReportResultDataList { get; set; }
        public List<ReportResultTemplateItem> Data { get; set; }


    }

    public class DefectExportResult
    {
        public List<KpiDefectData> Data { get; set; }
        public List<KpiBookingDepartment> BookingDepartments { get; set; }
        public List<KpiBookingContact> BookingCustomerContacts { get; set; }
        public List<KpiBookingBuyer> BookingBuyer { get; set; }
    }


    public class RemarksExportResult
    {
        public List<KpiReportRemarksTemplate> Data { get; set; }
        public List<KpiBookingDepartment> BookingDepartments { get; set; }
        public List<KpiBookingContact> BookingCustomerContacts { get; set; }
        public List<KpiPoDetails> PoList { get; set; }
    }

    public class KpiReportBatteryItem
    {
        public int ReportId { get; set; }
        public int? ProductId { get; set; }
        public string BatteryType { get; set; }
        public string BatteryModel { get; set; }
        public string Quantity { get; set; }
        public string NetWeight { get; set; }
    }

    public class KpiReportPackingItem
    {
        public int ReportId { get; set; }
        public int? ProductId { get; set; }
        public double? PieceNo { get; set; }
        public string MaterialGroup { get; set; }
        public string MaterialCode { get; set; }
        public string Location { get; set; }
        public string Quantity { get; set; }
        public string NetWeight { get; set; }
    }

    public class RescheduleData
    {
        public string ReasonName { get; set; }
        public int BookingId { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    public class FbPackingDimention
    {
        public string SpecClientValuesLength { get; set; }
        public string SpecClientValuesWidth { get; set; }
        public string SpecClientValuesHeight { get; set; }
        public string MeasuredValuesLength { get; set; }
        public string MeasuredValuesWidth { get; set; }
        public string MeasuredValuesHeight { get; set; }
        public int? ProductId { get; set; }
        public int ReportId { get; set; }
    }

    public class FbPackingWeight
    {
        public string SpecClientValuesWeight { get; set; }
        public string MeasuredValuesWeight { get; set; }
        public int? ProductId { get; set; }
        public int ReportId { get; set; }
    }

    public class KpiInvoiceData
    {
        public int InvoiceId { get; set; }
        public int? BookingId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceRemarks { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public double? InspFees { get; set; }
        public double? TravelAirFee { get; set; }
        public double? TravelLandFee { get; set; }
        public double? TravelFee { get; set; }
        public double? HotelFee { get; set; }
        public double? TotalFee { get; set; }
        public int? CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string InvoiceType { get; set; }
        public double? OtherFee { get; set; }
        public int? BilledTo { get; set; }
        public string BilledToName { get; set; }
        public string BilledName { get; set; }
        public string BilledAddress { get; set; }
        public string PaymentTerms { get; set; }

        public string PaymentDuration { get; set; }
        public string InvoiceStatus { get; set; }
        public string PaymentStatusName { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string InvoiceName { get; set; }
        public double? Discount { get; set; }
        public double? TaxValue { get; set; }
        public double? UnitPrice { get; set; }
        public double? ManDay { get; set; }
        public string InvoiceMethod { get; set; }
        public int? InvoicePaymentStatus { get; set; }

        public string InvocieBillingEntity { get; set; }
    }

    public class KpiBookingInvoiceResponse
    {
        public List<KpiInspectionBookingItems> BookingItems { get; set; }
        public List<InvoiceBookingData> InvoiceBookingData { get; set; }
    }

    public class KpiExtraFeeData
    {
        public int? BookingId { get; set; }
        public int? InvoiceId { get; set; }
        public double? ExtraFee { get; set; }
        public int? BilledTo { get; set; }
        public string ExtraFeeInvoiceNo { get; set; }
        public int? CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public int? PaymentStatus { get; set; }
        public string ExtraFeeStatus { get; set; }
        public string BilledName { get; set; }
        public string PaymentTerms { get; set; }
        public string PaymentStatusName { get; set; }
        public int? PaymentDuration { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string BilledToName { get; set; }
        public string BilledToAddress { get; set; }
        public string BillingEntity { get; set; }
    }

    public class MdmDefectData
    {
        public int? BookingNo { get; set; }
        public string ServiceDate { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public string ServiceType { get; set; }
        public string InspectionStatus { get; set; }
        public string ProductRef { get; set; }
        public string ProductDesc { get; set; }
        public string PoNumber { get; set; }
        public int? PoQty { get; set; }
        public int? InspectedQty { get; set; }
        public int? TotalDefects { get; set; }
        public int? TotalCritical { get; set; }
        public int? TotalMajor { get; set; }
        public int? TotalMinor { get; set; }
        public int? TotalQtyReworked { get; set; }
        public int? TotalQtyReplaced { get; set; }
        public int? TotalQtyRejected { get; set; }
        public string FinalResult { get; set; }
        public int? Productid { get; set; }
        public int? Reportid { get; set; }
        public string Department { get; set; }
        public string FactoryCountry { get; set; }
    }


    public class KpiDefectData
    {
        [Description("Insp. No.")]
        public int BookingNo { get; set; }
        [Description("Customer Ref.")]
        public string CustomerBookingNo { get; set; }
        [Description("Customer Name")]
        public string CustomerName { get; set; }
        [Description("Buyer")]
        public string BuyerName { get; set; }
        [Description("Department")]
        public string DeptCode { get; set; }
        [Description("Customer Contact")]
        public string CustomerContact { get; set; }
        [Description("Collection")]
        public string CollectionName { get; set; }
        [Description("Inspection Office")]
        public string Office { get; set; }
        [Description("Supplier")]
        public string SupplierName { get; set; }
        [Description("Factory")]
        public string FactoryName { get; set; }
        [Description("Country")]
        public string FactoryCountry { get; set; }
        [Description("Service Type")]
        public string ServiceTypeName { get; set; }
        [Description("Status")]
        public string BookingStatus { get; set; }
        [Description("Service Date From")]
        public DateTime InspectionStartDate { get; set; }
        [Description("Service Date To")]
        public DateTime InspectionEndDate { get; set; }
        [Description("Month")]
        public int Month { get; set; }
        [Description("Year")]
        public int Year { get; set; }
        [Description("PO No")]
        public string PONumber { get; set; }
        [Description("Product Ref.")]
        public string ProductName { get; set; }
        [Description("ProductDescription")]
        public string ProductDescription { get; set; }
        [Description("Factory Ref.")]
        public string FactoryRef { get; set; }
        [Description("Overall result")]
        public string ReportResult { get; set; }
        [Description("Defect No.")]
        public int DefectId { get; set; }
        [Description("Defect Description")]
        public string DefectDesc { get; set; }
        [Description("Critical")]
        public int CriticalDefect { get; set; }
        [Description("Major")]
        public int MajorDefect { get; set; }
        [Description("Minor")]
        public int MinorDefect { get; set; }
        [Description("Defect Cat")]
        public string DefectCategory { get; set; }
        [Description("Product Category")]
        public string ProductCategory { get; set; }
        [Description("Product Subcat 1")]
        public string ProductSubCategory { get; set; }
        [Description("Product Subcat 2")]
        public string ProductSubCategory2 { get; set; }
    }

    public class KpiDefectDataRepo
    {
        public int PoId { get; set; }
        public int BookingNo { get; set; }
        public int ReportId { get; set; }
        public string ReportResult { get; set; }
        public int DefectId { get; set; }
        public string DefectDesc { get; set; }
        public int CriticalDefect { get; set; }
        public int MajorDefect { get; set; }
        public int MinorDefect { get; set; }
        public string DefectCategory { get; set; }
    }

    public class KpiDefectInspectionRepo
    {
        public int ReportId { get; set; }
        public string CustomerBookingNo { get; set; }
        public string CustomerName { get; set; }
        public string BuyerName { get; set; }
        public string DeptCode { get; set; }
        public string CustomerContact { get; set; }
        public string CollectionName { get; set; }
        public string Office { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public string FactoryCountry { get; set; }
        public string ServiceTypeName { get; set; }
        public string BookingStatus { get; set; }
        public DateTime InspectionStartDate { get; set; }
        public DateTime InspectionEndDate { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string ReportResult { get; set; }
    }

    public class KpiBookingDepartment
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public int BookingId { get; set; }
        public string DepartmentCode { get; set; }
    }

    public class KpiBookingBuyer
    {
        public int Id { get; set; }
        public string BuyerName { get; set; }
        public int BookingId { get; set; }
    }

    public class KpiBookingContact
    {
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public int BookingId { get; set; }
    }

    public class KpiDbRequest
    {
        public int? CustomerId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int TemplateId { get; set; }
        [MapData(Type = "Udt_Int")]
        public List<CommonId> OfficeIdList { get; set; }
        [MapData(Type = "Udt_Int")]
        public List<CommonId> ServiceTypeIdList { get; set; }
        public string InvoiceNo { get; set; }
        [MapData(Type = "Udt_Int")]
        public List<CommonId> BrandIdList { get; set; }
        [MapData(Type = "Udt_Int")]
        public List<CommonId> DepartmentIdList { get; set; }
        public int? EntityId { get; set; }
    }

    public class KpiBookingSPRequest
    {
        [MapData(Type = "Udt_Int")]
        public List<CommonId> BookingIdList { get; set; }
        public int? EntityId { get; set; }
    }

    public class InspectionPicking
    {
        public int ProductRefId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSubCategory { get; set; }
        public string PoNumber { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public int InspectionId { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public string LabName { get; set; }
        public int? LabCustomerId { get; set; }
        public int? LabAddressId { get; set; }
        public int? CustomerAddressId { get; set; }
        public int? PickingQuantity { get; set; }
    }

    public class InspectionPickingReport
    {
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSubCategory { get; set; }
        public string PoNumber { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public int InspectionId { get; set; }
        public string ServiceDate { get; set; }
        public string LabName { get; set; }
        public int PickingQuantity { get; set; }
    }

    public class InspectionPickingProducts
    {
        public int ProductId { get; set; }
        public int InspectionId { get; set; }
    }

    public class LabDetails
    {
        public int LabId { get; set; }
        public string LabName { get; set; }
    }

    public class AdeoFollowUpTemplate
    {
        [Description("ID")]
        public int Id { get; set; }
        [Description(" ETD")]
        public DateTime? Etd { get; set; }
        [Description(" Order number")]
        public string PoNumber { get; set; }
        [Description(" ADEO supplier name")]
        public string SupplierName { get; set; }
        [Description(" ADEO Supplier code")]
        public string SupplierCode { get; set; }
        [Description(" Order responsible")]
        public string OrderResponsible { get; set; }
        [Description(" Quality Manager")]
        public string QualityManager { get; set; }
        [Description(" Inspection Country")]
        public string FactoryCountry { get; set; }
        [Description(" Inspection company name")]
        public string InspCompanyName { get; set; }
        [Description(" Status of the order (planned / inspected / cancelled)")]
        public string BookingStatus { get; set; }
        [Description(" Supplier effective contact date")]
        public DateTime? SupContactDate { get; set; }
        [Description(" Deadline to be contacted by supplier")]
        public DateTime? SupContactDeadlineDate { get; set; }
        [Description(" Inspection date")]
        public DateTime? InspectionEndDate { get; set; }
        [Description(" Date of sending report")]
        public DateTime? ReportDate { get; set; }
        [Description(" Final decision date on pending")]
        public DateTime? ConfirmDate { get; set; }
        [Description(" Date of SR Emitted")]
        public DateTime? ReConfirmDate { get; set; }
        [Description(" Final status")]
        public string ReportStatus { get; set; }
        [Description(" Comments")]
        public string ProductName { get; set; }
        [Description(" Office Location")]
        public string Office { get; set; }

    }

    public class KpiAdeoFollowUpDataRepo
    {
        public int Id { get; set; }
        public DateTime? Etd { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public string PoNumber { get; set; }
        public string SupplierName { get; set; }
        public string BookingStatus { get; set; }
        public string ReportStatus { get; set; }
        public string ProductName { get; set; }
        public string Office { get; set; }
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public int? FactoryId { get; set; }
        public int BookingId { get; set; }
        public int StatusId { get; set; }
        public int? ReportId { get; set; }
        public int ProductId { get; set; }
        public string Barcode { get; set; }
        public int? ReportResultId { get; set; }
        public string FactoryName { get; set; }
        public string ProductDescription { get; set; }
        public int BookingQty { get; set; }
        public int? QuotManday { get; set; }
    }

    public class KpiAdeoTemplateRequest
    {
        public List<KpiBookingProductsData> ProductData { get; set; }
        public List<FactoryCountry> FactoryLocation { get; set; }
        public List<CommonIdDate> CusDecisionData { get; set; }
        public List<CommonIdDate> IcData { get; set; }
        public List<HrHolidayData> HolidayList { get; set; }
        public List<SupplierCode> SupCode { get; set; }
        public List<KpiBookingDepartment> DeptList { get; set; }
        public List<KPIMerchandiser> MerchandiserList { get; set; }
        public List<KpiInvoiceData> InvoiceData { get; set; }
        public List<KpiPoDetails> PoDetails { get; set; }
        public List<FbReportRemarks> RemarksData { get; set; }
        public List<ServiceTypeList> ServiceTypeData { get; set; }
        public List<Reinspection> ReinspectionData { get; set; }
        public List<KpiInspectionBookingItems> BookingData { get; set; }
        public List<KpiAdeoFollowUpDataRepo> FactoryProductData { get; set; }
        public List<QuotationManday> QuotationData { get; set; }
    }

    public class HrHolidayData
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class AdeoEanTemplate
    {
        [Description("InspectionID")]
        public int BookingId { get; set; }
        [Description("ProductID")]
        public string ProductName { get; set; }
        [Description("Remark")]
        public string Remarks { get; set; }
        [Description("Barcode")]
        public string Barcode { get; set; }
        [Description("ProductDescription")]
        public string ProductDescription { get; set; }
    }

    public class AdeoFranceInspSummaryTemplate
    {
        [Description("INSPECTION COMPANY")]
        public string InspectionCompany { get; set; }
        [Description("SOURCING OFFICE")]
        public string SourcingOffice { get; set; }
        [Description("INSPECTION NUMBER")]
        public int BookingId { get; set; }
        [Description("INSPECTION DATE")]
        public string ServiceDate { get; set; }
        [Description("SUPPLIER")]
        public string SupplierName { get; set; }
        [Description("ORDER N°")]
        public string PoNumber { get; set; }
        [Description("MONTH/YEAR OF ETD")]
        public DateTime? Etd { get; set; }
        [Description("EA NS")]
        public string BarCode { get; set; }
        [Description("FIRST STATUS")]
        public string ReportStatus { get; set; }
        [Description("Final status - Following PENDING status, ADEO decision")]
        public string FinalReportStatus { get; set; }
        [Description("DEPARTMENT")]
        public string DepartmentCode { get; set; }
    }

    public class AdeoMonthInspSumbySubconFactoTemplate
    {
        [Description("Group")]
        public string Group { get; set; }
        [Description("Department")]
        public string DeptCode { get; set; }
        [Description("Supplier Code 1")]
        public string SupplierCode { get; set; }
        [Description("Supplier SSM Code")]
        public string SupplierSsmCode { get; set; }
        [Description("Factory SSM Code")]
        public string FactorySsmCode { get; set; }
        [Description("Merchandiser")]
        public string Merchandise { get; set; }
        [Description("Merchandiser2")]
        public string Merchandise2 { get; set; }
        [Description("Supplier Name(Adeo)")]
        public string SupplierNameAdeo { get; set; }
        [Description("Supplier Name(Compass)")]
        public string SupplierName { get; set; }
        [Description("Subcontractor")]
        public string FactoryName { get; set; }
        [Description("Nb_PSI")]
        public int TotalReports { get; set; }
        [Description("Nb PSI Passed")]
        public int TotalReportPass { get; set; }
        [Description("Nb Failed")]
        public int TotalReportFail { get; set; }
        [Description("Nb Pending")]
        public int TotalReportPending { get; set; }
        [Description("Nb Missing")]
        public int TotalReportMissing { get; set; }
        [Description("PSI Pass Rate")]
        public string ReportPassPercentage { get; set; }
        [Description("Lab")]
        public string CompanyName { get; set; }
        [Description("Month")]
        public int Month { get; set; }
        [Description("Year")]
        public int Year { get; set; }
    }

    public class AdeoInspSumOverallTemplate
    {
        [Description("Company Name")]
        public string CompanyName { get; set; }
        [Description("PONo")]
        public string PoNumber { get; set; }
        [Description("Item Number")]
        public string ProductName { get; set; }
        [Description("Item Description")]
        public string ProductDescription { get; set; }
        [Description("Department")]
        public string DeptCode { get; set; }
        [Description("Supplier Name")]
        public string SupplierName { get; set; }
        [Description("Factory Name")]
        public string FactoryName { get; set; }
        [Description("Inspection Office")]
        public string Office { get; set; }
        [Description("Inspection Qty")]
        public int InspQty { get; set; }
        [Description("ServiceRequest")]
        public string ServiceTypeName { get; set; }
        [Description("Update Insp Date")]
        public string ServiceDate { get; set; }
        [Description("Month")]
        public string MonthName { get; set; }
        [Description("Status")]
        public string BookingStatus { get; set; }
        [Description("Inspection ID")]
        public int BookingId { get; set; }
        [Description("Result")]
        public string ReportResult { get; set; }
        [Description("Failed Reason")]
        public string FailedRemark { get; set; }
        [Description("Supplier Code")]
        public string SupCode { get; set; }
        [Description("Mandays")]
        public double Manday { get; set; }
        [Description("InspFee")]
        public double InspFees { get; set; }
        [Description("ChargeableCost")]
        public double TravellingCost { get; set; }
        [Description("Total Cost")]
        public double TotalInspFee { get; set; }
        [Description("Invoice No")]
        public string InvoiceNo { get; set; }

    }

    public class AdeoFailedPoTemplate
    {
        [Description("Inspection ID")]
        public int BookingId { get; set; }
        [Description("PONo")]
        public string PoNumber { get; set; }
        [Description("Item Number")]
        public string ProductName { get; set; }
        [Description("Inspetion Date")]
        public string ServiceDate { get; set; }
        [Description("Department")]
        public string DeptCode { get; set; }
        [Description("PSI Result")]
        public string ReportResult { get; set; }
        [Description("Supplier Name")]
        public string SupplierName { get; set; }
        [Description("Supplier Code")]
        public string SupplierCode { get; set; }
        [Description("Product")]
        public string ProductDescription { get; set; }
        [Description("Reason")]
        public string FailedReason { get; set; }
        [Description("Next PSI ID")]
        public string ReinspectionId { get; set; }
        [Description("Remarks")]
        public string ReportRemarks { get; set; }

    }

    public class CarrefourInvoiceResponse
    {
        [Description("Insp.company")]
        public string InspCompany { get; set; }

        [Description("Booking ID")]
        public int? BookingNo { get; set; }

        [Description("Inspection Office Location")]
        public string Office { get; set; }

        [Description("Supplier")]
        public string SupplierName { get; set; }

        [Description("Supplier code")]
        public string SupplierCode { get; set; }
        [Description("Factory")]
        public string FactoryName { get; set; }
        [Description("Province")]
        public string FactoryState { get; set; }
        [Description("City")]
        public string FactoryCity { get; set; }
        [Description("Country of Origin")]
        public string FactoryCountry { get; set; }
        [Description("Order no")]
        public string PONumber { get; set; }
        [Description("Dept")]
        public string DeptCode { get; set; }
        [Description("Division")]
        public string DeptDivision { get; set; }

        [Description("Product Description")]
        public string ProductDescription { get; set; }
        [Description("Order QTY")]
        public int? ProductCount { get; set; }
        [Description("Distribution Line")]
        public string DistributionLine { get; set; }
        [Description("Destination Country")]
        public string DestinationCountry { get; set; }

        [Description("Client Rate ")]
        public double? InspectionFee { get; set; }
        [Description("Travel")]
        public double? TravellingCost { get; set; }
        [Description("Hotel")]
        public double? HotelFee { get; set; }
        [Description("Other Fee")]
        public double? OtherFee { get; set; }
        [Description("Extra Fee")]
        public double ExtraFee { get; set; }
        [Description("TOTAL")]
        public double? TotalInspectionFee { get; set; }
        [Description("Range-used")]
        public int? PriceCategoryId { get; set; }
        [Description("Requested Inspection Date")]
        public DateTime? FirstServiceDateFrom { get; set; }
        [Description("If postponed indicate reason")]
        public string RescheduleReason { get; set; }

        [Description("Report no.")]
        public string CustomerBookingNumber { get; set; }
        [Description("Inspection Type")]
        public string ServiceTypeName { get; set; }
        [Description("Date from")]
        public DateTime? InspectionStartedDate { get; set; }
        [Description("Date to")]
        public DateTime? InspectionEndDate { get; set; }
        [Description("Inspected Quantity")]
        public double? InspectedQty { get; set; }
        [Description("Number of Inspectors")]
        public int? QcCount { get; set; }
        [Description("AQL Sample Size")]
        public int AQLQuantity { get; set; }
        [Description("Date")]
        public DateTime? InspectionStartDate { get; set; }
        [Description("Month")]
        public string InspMonthName { get; set; }
        [Description("Result")]
        public string ReportResult { get; set; }
        [Description("Reference sample")]
        public string GoldenSampleAvailable { get; set; }

        [Description("Technical File")]
        public string TechnicalFile { get; set; }
        [Description("Size Spec")]
        public string SizeSpec { get; set; }

        [Description("Insp Result")]
        public string InspectionReportResult { get; set; }
        [Description("AQL Result")]
        public string AQLResult { get; set; }

        [Description("AQL Cri.")]
        public int? CriticalDefect { get; set; }
        [Description("AQL Maj.")]
        public int? MajorDefect { get; set; }
        [Description("AQL Min.")]
        public int? MinorDefect { get; set; }
        [Description("Prod Conformity Result")]
        public string ProdConformityResult { get; set; }
        [Description("Size Spec Result")]
        public string SizeSpecResult { get; set; }
        [Description("Packaging Result")]
        public string PackagingResult { get; set; }
        [Description("Packing Result")]
        public string PackingResult { get; set; }
        [Description("Comments when Failed")]
        public string ReportRemarks { get; set; }
        [Description("Indicate combined inspections")]
        public string ProrateBookingNo { get; set; }
        [Description("Other Remark")]
        public string OtherRemark { get; set; }

        [Description("PCB")]
        public double PCB1 { get; set; }
        [Description("Master Length (cm)")]
        public string SpecClientValuesLength { get; set; }
        [Description("Master Width (cm)")]
        public string SpecClientValuesWidth { get; set; }
        [Description("Master Height (cm)")]
        public string SpecClientValuesHeight { get; set; }
        [Description("Volume master(m3)")]
        public double SpecClientVolume { get; set; }
        [Description("Master Weight (kg)")]
        public string SpecClientValuesWeight { get; set; }

        [Description("PCB")]
        public double PCB2 { get; set; }
        [Description(" Master Length (cm)")]
        public string MeasuredValuesLength { get; set; }
        [Description("Master Width (cm)")]
        public string MeasuredValuesWidth { get; set; }
        [Description("Master Height (cm)")]
        public string MeasuredValuesHeight { get; set; }
        [Description("Volume master (m3)")]
        public double MeasuredVolume { get; set; }
        [Description("Master Weight (kg)")]
        public string MeasuredValuesWeight { get; set; }
        [Description(" % Volume")]
        public double PercentageVolume { get; set; }
        [Description("% Weight")]
        public double PercentageWeight { get; set; }

        [Description("IM / warranty card")]
        public string WarrantyCard { get; set; }

        [Description("Golden sample")]
        public string GoldenSample { get; set; }
        [Description("Labeling")]
        public string Labeling { get; set; }
        [Description("Packaging")]
        public string Packaging { get; set; }
        [Description("Component / Construction")]
        public string ComponentConstruction { get; set; }
        [Description("Software")]
        public string Software { get; set; }
        [Description("Release OKQ with L/G")]
        public string ReleaseOKQ { get; set; }

        public int ReportId { get; set; }
        [Description("Invoice #")]
        public string InvoiceNumber { get; set; }

        [Description("Billed To")]
        public string InvoiceTo { get; set; }

        //public int? BookingFormSerial { get; set; }

        ////[Description("Office location")]
        //public string OfficeLocationName { get; set; }

        //[Description("Group")]
        //public string Group { get; set; }

        //[Description("Carrefour ref#")]
        //public string ProductName { get; set; }

        //[Description("Currency")]
        //public string CurrencyName { get; set; }

        //[Description("M")]
        //public int? InspMonthNumber { get; set; }

        //[Description("Category")]
        //public string Category { get; set; }

        //[Description("S.Size")]
        //public string SampleSize { get; set; }

        //[Description("Sample")]
        //public string Sample { get; set; }

        //[Description("File")]
        //public string File { get; set; }
    }

    public class CarrefourQuoationDetails
    {
        public int QuotationId { get; set; }
        public int BookingId { get; set; }
        public string BillPaidByName { get; set; }
        public int BillPaidById { get; set; }
    }

    public class GeneralInvoiceResponse
    {
        [Description("Invoice#")]
        public string InvoiceNumber { get; set; }
        [Description("Booking#")]
        public int BookingNo { get; set; }
        [Description("Quotation#")]
        public int QuotationId { get; set; }
        [Description("Customer Name")]
        public string CustomerName { get; set; }
        [Description("Contact Name")]
        public string CustomerContact { get; set; }

        [Description("Merchandiser")]
        public string Merchandiser { get; set; }
        [Description("Department")]
        public string DeptCode { get; set; }
        [Description("Brand")]
        public string BrandName { get; set; }
        [Description("Buyer")]
        public string BuyerName { get; set; }
        [Description("Collection")]
        public string CollectionName { get; set; }
        [Description("Supplier Name")]
        public string SupplierName { get; set; }
        [Description("Supplier Code")]
        public string SupplierCode { get; set; }
        [Description("Factory Name")]
        public string FactoryName { get; set; }

        [Description("Factory Country")]
        public string FactoryCountry { get; set; }
        [Description("Factory Province")]
        public string FactoryProvince { get; set; }
        [Description("Factory Reference")]
        public string FactoryRef { get; set; }
        [Description("Product Reference")]
        public string ProductName { get; set; }
        [Description("Product Description")]
        public string ProductDescription { get; set; }
        [Description("Po Number")]
        public string PONumber { get; set; }
        [Description("Service Request")]
        public string ServiceTypeName { get; set; }
        [Description("First Request Date")]
        public DateTime? FirstServiceDateFrom { get; set; }
        [Description("Inspection Start Date")]
        public DateTime? InspectionStartDate { get; set; }
        [Description("Inspection End Date")]
        public DateTime? InspectionEndDate { get; set; }
        [Description("Result")]
        public string ReportResult { get; set; }
        [Description("Lot Size")]
        public string OrderQty { get; set; }
        [Description("Sample Size")]
        public string SampleSize { get; set; }
        [Description("AQL")]
        public string AQLLevelName { get; set; }
        [Description("Man Day")]
        public double ManDay { get; set; }
        [Description("Inspection Cost USD")]
        public double InspectionFee { get; set; }
        [Description("Hotel Cost")]
        public double HotelFee { get; set; }
        [Description("Travelling Cost")]
        public double TravellingCost { get; set; }
        [Description("Extra Fee")]
        public double ExtraFee { get; set; }
        [Description("Other Fee")]
        public double OtherFee { get; set; }
        [Description("Total amount USD")]
        public double TotalInspectionFee { get; set; }
        public int ReportId { get; set; }
        [Description("Fee Remarks")]
        public string InvoiceRemarks { get; set; }

    }
    public class CarreFourECOPackTemplate
    {
        [Description("Booking No")]
        public int BookingNo { get; set; }
        [Description("Customer Booking No")]
        public string CustomerBookingNo { get; set; }
        [Description("Department")]
        public string DeptCode { get; set; }
        [Description("Supplier")]
        public string SupplierName { get; set; }
        [Description("Factory")]
        public string FactoryName { get; set; }
        [Description("API Confirm Service Date From")]
        public DateTime? InspectionStartDate { get; set; }
        [Description("API Confirm Service Date To")]
        public DateTime? InspectionEndDate { get; set; }
        [Description("Service Type")]
        public string ServiceTypeName { get; set; }
        [Description("Status")]
        public string bookingStatus { get; set; }
        [Description("Product Description")]
        public string ProductDescription { get; set; }
        [Description("ProductID")]
        public string ProductName { get; set; }
        [Description("Packaging-Piece Number")]
        public double? PieceNo { get; set; }
        [Description("Packaging-Material Group")]
        public string MaterialGroup { get; set; }
        [Description("Packaging-Material Code")]
        public string MaterialCode { get; set; }
        [Description("Packaging-Location")]
        public string PackingLocation { get; set; }
        [Description("Packaging-Quantity")]
        public string PackingQuantity { get; set; }
        [Description("Packaging-Net Weight (g) per Quantity")]
        public string PackingNetWeight { get; set; }
        [Description("Battery Type")]
        public string BatteryType { get; set; }
        [Description("Battery Model")]
        public string BatteryModel { get; set; }
        [Description("Battery-Quantity")]
        public string BatteryQuantity { get; set; }
        [Description("Battery-Net Weight (g) per Quantity")]
        public string BatteryNetWeight { get; set; }
        [Description("PO Number")]
        public string PONumber { get; set; }
        [Description("Order Quantity")]
        public string ShipmentQty { get; set; }
        [Description("Product Category")]
        public string ProductCategory { get; set; }
        [Description("Product Sub Category")]
        public string ProductSubCategory { get; set; }
        [Description("Product Sub Category 2")]
        public string ProductSubCategory2 { get; set; }
        [Description("Report Result")]
        public string ReportResult { get; set; }
        [Description("Office")]
        public string Office { get; set; }
        [Description("Inspectors")]
        public string QcName { get; set; }

    }

    public class KpiReportCommentsTemplateRepo
    {
        public int BookingNo { get; set; }
        public string Office { get; set; }
        public DateTime? InspectionStartDate { get; set; }
        public DateTime InspectionEndDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerBookingNo { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public string ServiceTypeName { get; set; }
        public string ProductName { get; set; }
        public string FactoryCountry { get; set; }
        public string BuyerName { get; set; }
        public string CustomerContact { get; set; }
        public string ReportResult { get; set; }
        public string CollectionName { get; set; }
        public string BookingStatus { get; set; }
        public string PaidBy { get; set; }
        public string PONumber { get; set; }
        public string DeptCode { get; set; }
        public string ProductDescription { get; set; }
        public string FactoryRef { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSubCategory { get; set; }
        public string ProductSubCategory2 { get; set; }
        public int? SerialNo { get; set; }
        public int? FBCommentNumber { get; set; }
        public string ReportComments { get; set; }
        public string RemarkCategory { get; set; }
        public string RemarkSubCategory { get; set; }
        public string RemarkSubCategory2 { get; set; }
        public string CustomerRemarkCodeReference { get; set; }
        public int? CombineId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int ReportId { get; set; }
        public int BillPaidBy { get; set; }
        public int? ProductId { get; set; }
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public int FactoryId { get; set; }
    }

    public class KpiReportCommentsTemplate
    {
        [Description("Inspection Number")]
        public int BookingNo { get; set; }
        [Description("Customer Ref")]
        public string CustomerBookingNo { get; set; }
        [Description("Customer Name")]
        public string CustomerName { get; set; }
        [Description("Buyer")]
        public string BuyerName { get; set; }
        [Description("Department")]
        public string DeptCode { get; set; }
        [Description("Customer Contact")]
        public string CustomerContact { get; set; }
        [Description("Merchandiser")]
        public string Merchandise { get; set; }
        [Description("Collection")]
        public string CollectionName { get; set; }
        [Description("Inspection Office")]
        public string Office { get; set; }
        [Description("Supplier")]
        public string SupplierName { get; set; }
        [Description("Supplier Code")]
        public string SupplierCode { get; set; }
        [Description("Factory")]
        public string FactoryName { get; set; }
        [Description("Factory Code")]
        public string FactoryCode { get; set; }
        [Description("Country")]
        public string FactoryCountry { get; set; }
        [Description("Service Type")]
        public string ServiceTypeName { get; set; }
        [Description("Status")]
        public string BookingStatus { get; set; }
        [Description("Service Date From")]
        public DateTime? InspectionStartDate { get; set; }
        [Description("Service Date To")]
        public DateTime? InspectionEndDate { get; set; }
        [Description("Month")]
        public int Month { get; set; }
        [Description("Year")]
        public int Year { get; set; }
        [Description("Charge To Cust/Supp")]
        public string BillPaidBy { get; set; }
        [Description("PO NO")]
        public string PONumber { get; set; }
        [Description("Product Ref.")]
        public string ProductName { get; set; }
        [Description("Product Description")]
        public string ProductDescription { get; set; }
        [Description("Factory Ref.")]
        public string FactoryRef { get; set; }
        [Description("Combined with")]
        public int? CombinedWith { get; set; }
        [Description("Overall Result")]
        public string ReportResult { get; set; }
        [Description("Comment No")]
        public int? FBCommentNumber { get; set; }
        [Description("Comment")]
        public string ReportComments { get; set; }
        [Description("Remark Cat")]
        public string RemarkCategory { get; set; }
        [Description("Remark Subcat")]
        public string RemarkSubCategory { get; set; }
        [Description("Remark Subcat 2")]
        public string RemarkSubCategory2 { get; set; }
        [Description("Reference")]
        public string CustomerRemarkCodeReference { get; set; }
        [Description("Product Category")]
        public string ProductCategory { get; set; }
        [Description("Product Subcat 1")]
        public string ProductSubCategory { get; set; }
        [Description("Product Subcat 2")]
        public string ProductSubCategory2 { get; set; }
    }

    public class InspCommentSummaryMapRequest
    {
        public List<KpiReportCommentsTemplateRepo> CommentData { get; set; }
        public List<KpiPoDetails> PoData { get; set; }
        public List<KpiBookingBuyer> BuyerData { get; set; }
        public List<KpiBookingDepartment> DeptData { get; set; }
        public List<KpiBookingContact> ContactData { get; set; }
        public List<KpiBookingProductsData> ProductData { get; set; }
        public List<ServiceTypeList> ServiceTypeData { get; set; }
        public List<FactoryCountry> FactoryLocation { get; set; }
        public List<CarrefourQuoationDetails> BillPaidByData { get; set; }
        public List<KPIMerchandiser> MerchandiserData { get; set; }
        public List<SupplierCode> SupplierCode { get; set; }
    }

    public class CustomerDecisionData
    {
        public int? BookingId { get; set; }
        public int ReportId { get; set; }
        public int CustomerDecisionResultId { get; set; }
        public string CustomerDecisionName { get; set; }
        public string CustomerDecisionComment { get; set; }
    }

    public class CustomerCulturaTemplate
    {
        [Description("Applicaiton Date")]
        public string BookingCreationDate { get; set; }
        [Description("Name")]
        public string SupplierName { get; set; }
        [Description("Factory Name")]
        public string FactoryName { get; set; }
        [Description("PO No")]
        public string PONumber { get; set; }
        [Description("Product Ref")]
        public string ProductName { get; set; }
        [Description("Type")]
        public string ServiceTypeName { get; set; }
        [Description("Inspection StartDate")]
        public string InspectionStartDate { get; set; }
        [Description("Inspection EndDate")]
        public string InspectionEndDate { get; set; }
        [Description("Party")]
        public string Party { get; set; }
        [Description("Manday Rate")]
        public double? MandayRate { get; set; }
        [Description("Manday Used")]
        public double? MandayUsed { get; set; }
        [Description("CC Fees")]
        public string CCFees { get; set; }
        [Description("Trans + Hotel Fees")]
        public double? Fee { get; set; }
        [Description("Total Amount USD")]
        public double? TotalInspectionFee { get; set; }
        [Description("Order Amount USD")]
        public double? InspectionFee { get; set; }
        [Description("Report Date")]
        public string ReportDate { get; set; }
        [Description("Final Result")]
        public string ReportResult { get; set; }
        [Description("Quotation No")]
        public int? QuotationId { get; set; }

        [Description("Report Title")]
        public string ReportTitle { get; set; }
    }

    public class ECITemplateResponse
    {
        [Description("INSPECTION DATE")]
        public DateTime? InspectionEndDate { get; set; }
        [Description("THIRD PARTY")]
        public string ThirdParty { get; set; }
        [Description("REPORT ID")]
        public int BookingNo { get; set; }
        [Description("OVERALL RESULT PER STYLE(PASS / FAIL)")]
        public string ReportResult { get; set; }
        [Description("DEPARTENT CODE +Space+ COMPANY NAME")]
        public string BuyerName { get; set; }
        [Description("COMPANY CODE / DIVISION / DEPARMTMENT Hierarchy Code")]
        public string Division { get; set; }
        [Description("BRAND")]
        public string Brand { get; set; }
        [Description("BDM")]
        public object Bdm { get; set; }
        [Description("SECTION")]
        public string Section { get; set; }
        [Description("MERCHANDISER ECI")]
        public string Merchandiser { get; set; }
        [Description("QCM")]
        public object QcmName { get; set; }
        [Description("SUPPLIER CODE")]
        public string SupplierCode { get; set; }
        [Description("SUPPLIER")]
        public string SupplierName { get; set; }
        [Description("FACTORY")]
        public string FactoryName { get; set; }
        [Description("RETAIL COUNTRY")]
        public string FactoryCountry { get; set; }
        [Description("SOURCING OFFICE")]
        public object EciOffice { get; set; }
        [Description("VPO NUMBER [Here is named only the FIRST PO]")]
        public string PONumber { get; set; }
        [Description("STYLE NAME OR NUMBER")]
        public string ProductName { get; set; }
        [Description("ITEM DESCRIPTION")]
        public string ProductDescription { get; set; }
        [Description("SOFT LINE / HARD LINE")]
        public string HardLine { get; set; }
        [Description("INSPECTION TYPE")]
        public string ServiceTypeName { get; set; }
        [Description("ORDER QTY(TOTAL UNITS NUMBER OF PACKS, SET OR PIECES)")]
        public double? OrderQty { get; set; }
        [Description("SHIP QTY")]
        public double? ShipmentQty { get; set; }
        [Description("% OVER / SHORT QTY")]
        public string OverOrShortQty { get; set; }
        [Description("OVER QTY")]
        public string OverQty { get; set; }
        [Description("SHORT QTY")]
        public string ShortQty { get; set; }
        [Description("Defect Qty (Critical)")]
        public int? CriticalDefect { get; set; }
        [Description("Defect Qty(Major)")]
        public int? MajorDefect { get; set; }
        [Description("Defect Qty (Minor)")]
        public int? MinorDefect { get; set; }
        [Description("ORDEN DE FABRICACIÓN")]
        public string OrdnanceDeFabrication { get; set; }
        [Description("SHORT DESCRIPTION [Pass by]")]
        public string ShortDescription { get; set; }
        [Description("SEASON")]
        public string Season { get; set; }
        [Description("IR CODE [Centric]")]
        public string IRCode { get; set; }
        [Description("Final Result")]
        public int ReportId { get; set; }
        public int ProductId { get; set; }
    }

    public class OrderStatusLogItem
    {
        [Description("Branch Name")]
        public string Office { get; set; }
        [Description("Customer Name")]
        public string CustomerName { get; set; }
        [Description("Brand")]
        public string BrandName { get; set; }
        [Description("Department")]
        public string DeptCode { get; set; }
        [Description("Product Sub Category")]
        public string ProductSubCategory { get; set; }
        [Description("Customer Product Category")]
        public string CustomerProductCategory { get; set; }
        [Description("Booking No")]
        public int BookingNo { get; set; }
        [Description("Style/Product")]
        public string ProductName { get; set; }
        [Description("Report No")]
        public string ReportNo { get; set; }
        [Description("Service From Date")]
        public string ServiceFromDate { get; set; }
        [Description("Service To Date")]
        public string ServiceToDate { get; set; }
        [Description("Season")]
        public string Season { get; set; }
        [Description("Produced Qty")]
        public double ProducedQty { get; set; }
        [Description("Order Qty")]
        public double OrderQty { get; set; }
        [Description("Inspected Qty")]
        public double InspectedQty { get; set; }
        [Description("Service Type")]
        public string ServiceTypeName { get; set; }
        [Description("Category")]
        public string ProductCategory { get; set; }
        [Description("Service")]
        public string ServiceName { get; set; }
        [Description("Color")]
        public string Color { get; set; }
        [Description("Supplier")]
        public string SupplierName { get; set; }
        [Description("Factory")]
        public string FactoryName { get; set; }
        [Description("City")]
        public string FactoryCity { get; set; }
        [Description("Inspection Country")]
        public string FactoryCountry { get; set; }
        [Description("Office Country")]
        public string OfficeCountry { get; set; }
        [Description("Booking Creation Date")]
        public string BookingCreationDate { get; set; }
        [Description("Booking Creation By")]
        public string BookingCreatedBy { get; set; }
        [Description("Invoice No")]
        public string InvoiceNumber { get; set; }
        [Description("Invoice Currency")]
        public string Invoice_Currency { get; set; }
        [Description("Billing Mode")]
        public string BillingMode { get; set; }
        [Description("Invoice To")]
        public string InvoiceTo { get; set; }
        [Description("Unit Price")]
        public double? UnitPrice { get; set; }
        [Description("Billing Man Days")]
        public double? BillingManDays { get; set; }
        [Description("Invoice Contact")]
        public string InvoiceContact { get; set; }
        [Description("Inspection Fee")]
        public double? Invoice_InspectionFee { get; set; }
        [Description("Land Transport Cost")]
        public double? Invoice_TravelLandFee { get; set; }
        [Description("Air Cost")]
        public double? AirTicket { get; set; }
        [Description("Hotel Cost")]
        public double? Invoice_HotelFee { get; set; }
        [Description("Travel Expense")]
        public double? Invoice_TravelFee { get; set; }
        [Description("Total Staff")]
        public int? TotalStaff { get; set; }
        [Description("Report Status")]
        public string ReportStatus { get; set; }
        [Description("Booking Status")]
        public string BookingStatus { get; set; }
        [Description("Invoice Sent Date")]
        public string InvoiceDate { get; set; }
        [Description("Inspectors")]
        public string Inspectors { get; set; }

        [Description("Inspection Location")]
        public string InspectionLocation { get; set; }
        public int ReportId { get; set; }
        [Description("Report Overall Result")]
        public string ReportResult { get; set; }
        [Description("Customer Decision")]
        public string CusDecisionName { get; set; }

        [Description("Customer Decision Date")]
        public string CusDecisionDate { get; set; }
    }

    public class KPIDefectPurchaseOrderRepo
    {
        public int PoId { get; set; }
        public string PONumber { get; set; }
        public int BookingId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string FactoryRef { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSubCategory { get; set; }
        public string ProductSubCategory2 { get; set; }

    }

    public enum KPICustomTemplate
    {
        ECI_YTD = 1,
        WareHouse = 3,
        GIFI_KPI = 4,
        AdeoEANCode = 5,
        AdeoFranceInspSummary = 6,
        AdeoMonthInspSumByFactory = 7,
        AdeoMonthInspSumByQA = 8,
        AdeoVietnamInspSumOverall = 9,
        AdeoInspFollowUp = 10,
        AdeoPOFailed = 11,
        StockQCPerformance = 12,
        StockMonthlyStatement = 13,
        AdeoSummaryRemarks = 14,
        ECIRemark = 15,
        InspDefectSummary = 16,
        InspResultSummary = 17,
        InspRemarksSummary = 18,
        Liverpool = 19,
        InspExpenseSummary = 20,
        CasinoSummary = 21,
        StockWeeklyStatement = 26,
        CarrefourInvoiceStatement = 27,
        GeneralInvoiceStatement = 28,
        CarrefourEcopack = 29,
        CarrefourDailyResult = 32,
        MDMKPI = 33,
        InspPickingSummary = 34,
        InspCommentSummary = 35,
        OrderStatusLog = 36,
        InspectionData = 37,
        ScheduleAnalysis = 38,
        ECI = 39,
        Cultura = 40,
        InspectionSummaryQC = 41,
        CustomerManday = 42,
        GapProductRefLevel = 43,
        XeroInvoice = 44,
        XeroExpense = 45,
        ARFollowUpReport = 46,
        GapFlashProcessAudit = 47,
        GapAudit = 48,
        InspectionExpenseSummary = 49
    }

    public class ScheduleAnalysisTemplate
    {
        [Description("Inspection#")]
        public int BookingNo { get; set; }
        [Description("Customer Booking#")]
        public string CustomerBookingNo { get; set; }
        [Description("Customer Name")]
        public string CustomerName { get; set; }
        [Description("Brand")]
        public string BrandName { get; set; }
        [Description("Department")]
        public string DeptCode { get; set; }
        [Description("Supplier")]
        public string SupplierName { get; set; }
        [Description("Supplier Grade")]
        public string SupplierGrade { get; set; }
        [Description("Factory Name")]
        public string FactoryName { get; set; }
        [Description("Factory City")]
        public string FactoryCity { get; set; }
        [Description("Factory Province")]
        public string FactoryProvince { get; set; }
        [Description("Factory Country")]
        public string FactoryCountry { get; set; }
        [Description("Factory Grade")]
        public string FactoryGrade { get; set; }
        [Description("Office")]
        public string Office { get; set; }
        [Description("Booking Type")]
        public string BookingType { get; set; }
        [Description("Apply Date")]
        public string BookingCreationDate { get; set; }
        [Description("Sch.Insp Date From")]
        public DateTime ShIpDateFrom { get; set; }
        [Description("Sch.Insp Date To")]
        public DateTime ShIpDateTo { get; set; }
        [Description("Actual Insp Date From & To")]
        public string ActualShIpDateFromAndTo { get; set; }
        [Description("Season")]
        public string Season { get; set; }
        [Description("Service Type")]
        public string ServiceTypeName { get; set; }
        [Description("Product Ref")]
        public string ProductRef { get; set; }
        [Description("PO number")]
        public string PONumber { get; set; }
        [Description("Product Category")]
        public string ProductCategory { get; set; }
        [Description("Product Name")]
        public string ProductName { get; set; }
        [Description("Colors")]
        public string Color { get; set; }
        [Description("Order Qty")]
        public double OrderQty { get; set; }
        [Description("Inspected Qty")]
        public double InspectedQty { get; set; }
        [Description("Produced Qty")]
        public double ProducedQty { get; set; }
        [Description("Report Result")]
        public string ReportResult { get; set; }
        [Description("Client Result")]
        public string ClientResult { get; set; }
        [Description("Inspectors")]
        public string Inspectors { get; set; }
        [Description("NO. Of Inspectors")]
        public int? InspectorCount { get; set; }
        [Description("Additional Inspectors")]
        public string AdditionalInspectors { get; set; }
        [Description("Expense Claim No")]
        public string ExpenseClaimNo { get; set; }
        [Description("Expense Claim Amount(Ammount HK)")]
        public double? ExpenseClaimAmount { get; set; }
        [Description("Invoice Date")]
        public string InvoiceDate { get; set; }
        [Description("Inspection Status")]
        public string BookingStatus { get; set; }
        [Description("Inspection Location")]
        public string InspectionLocation { get; set; }
        [Description("Invoice Billed Name")]
        public string InvoiceTo { get; set; }
        [Description("Invoice Billed To")]
        public string InvoiceBilledTo { get; set; }
        [Description("Invoice No")]
        public string InvoiceNumber { get; set; }
        [Description("Total Invoice Amount")]
        public double? Invoice_TotalFee { get; set; }
        [Description("Invoice Currency")]
        public string Invoice_Currency { get; set; }
        [Description("Payment Status")]
        public string Invoice_PaymentStatus { get; set; }
        [Description("Extra Fee Billed Name")]
        public string ExtraFeeBilledName { get; set; }
        [Description("Extra Fee Billed To")]
        public string ExtraFeeBilledTo { get; set; }
        [Description("Extra Fee Invoice No")]
        public string ExtraFeeInvoiceNumber { get; set; }
        [Description("Extra Fee Total Amount")]
        public double? ExtraFeeInvoice_TotalFee { get; set; }
        [Description("Extra Fee Currency")]
        public string ExtraFeeInvoice_Currency { get; set; }
        [Description("Extra Fee Payment Status")]
        public string ExtraFeeInvoice_PaymentStatus { get; set; }
        [Description("Quotation No")]
        public int? QuotationNumber { get; set; }
        [Description("Total Quotation Amount")]
        public double? QuotationPrice { get; set; }
        [Description("Quotation Currency")]
        public string CurrencyName { get; set; }
        [Description("Estimated Man Day")]
        public double? EstimatedManDay { get; set; }
        [Description("Actual Man Day")]
        public double? ActualManDay { get; set; }
        [Description("Gap Inspection Platform")]
        public string GapInspectionPlatform { get; set; }
        [Description("Gap Payment Options")]
        public string GapPaymentOptions { get; set; }
        [Description("GAP External Report Number")]
        public string GapExternalReportNo { get; set; }
        [Description("Sample Size")]
        public int SampleSize { get; set; }
    }

    public class InspectionSummaryQCTemplate
    {
        [Description("Office ")]
        public string Office { get; set; }
        [Description("Inspection#")]
        public int? BookingNo { get; set; }
        [Description("Order Status")]
        public string OrderStatus { get; set; }

        [Description("Service Date From")]
        public DateTime? ServiceDateFrom { get; set; }
        [Description("Service Date To")]
        public DateTime? ServiceDateTo { get; set; }
        [Description("Service Type")]
        public string ServiceTypeName { get; set; }
        [Description("Schedule Date")]
        public DateTime ScheduleDate { get; set; }
        [Description("QC Name")]
        public string QCName { get; set; }
        [Description("Customer")]
        public string CustomerName { get; set; }
        [Description("Supplier")]
        public string SupplierName { get; set; }
        [Description("Factory")]
        public string FactoryName { get; set; }
        [Description("Country Name")]
        public string FactoryCountry { get; set; }
        [Description("Factory City")]
        public string FactoryCity { get; set; }
        [Description("Factory Town")]
        public string FactoryTown { get; set; }

        [Description("Employee Type")]
        public string EmployeeTypeName { get; set; }
        [Description("Outsource Company")]
        public string OutsourceCompany { get; set; }

        [Description("Start Port")]
        public string StartPort { get; set; }
        [Description("Claim No.")]
        public string ClaimNumber { get; set; }
        [Description("Claim Date")]
        public DateTime? ClaimDate { get; set; }
        [Description("Claim Satus")]
        public string ClaimStatus { get; set; }
        //[Description("Start City")]
        //public string StartCity { get; set; }
        //[Description("End City")]
        //public string EndCity { get; set; }
        [Description("Payroll Currency")]
        public string PayrollCurrency { get; set; }
        [Description("No.Of Mandays")]
        public int? NumberOfManDay { get; set; }
        [Description("Insp Fees")]
        public double? InspectionFees { get; set; }
        [Description("Air")]
        public double? Air { get; set; }
        [Description("Land")]
        public double? Land { get; set; }
        [Description("Food")]
        public double? Food { get; set; }
        [Description("Hotel")]
        public double? Hotel { get; set; }
        [Description("Other")]
        public double? Other { get; set; }
        [Description("Service Tax")]
        public double? ServiceTax { get; set; }
        [Description("Claim Total")]
        public double? ClaimTotal { get; set; }

        [Description("Remarks")]
        public string ClaimRemarks { get; set; }

        [Description("Invoice No")]
        public string InvoiceNo { get; set; }

        [Description("Invoiced Currency")]
        public string InvoiceCurrency { get; set; }

        [Description("Invoiced Insp Fees")]
        public double? InvoiceInspectionFees { get; set; }

        [Description("Invoiced Travelling Fees")]
        public double? InvoiceTravellingFees { get; set; }

        [Description("Invoiced Other Fees")]
        public double? InvoiceOtherFees { get; set; }

        [Description("Invoiced Total Tax")]
        public double? InvoiceTotalTax { get; set; }

        [Description("Invoiced Total")]
        public double? InvoiceTotalFees { get; set; }
        [Description("Billed MD")]
        public double? InvoiceManDay { get; set; }
        [Description("Extra Fee Invoice No")]
        public string ExtraFeeInvoiceNo { get; set; }

        [Description("Extra Fee Invoice Status")]
        public string ExtraFeeInvoiceStatus { get; set; }

        [Description("Extra Fee Currency")]
        public string ExtraFeeCurrency { get; set; }

        [Description("Total Extra Fees")]
        public double? TotalExtraFees { get; set; }

    }

    public class CustomerMandayExportData
    {
        [Description("Customer")]
        public string Customer { get; set; }
        [Description("Service Type")]
        public string ServiceType { get; set; }
        [Description("Factory Country")]
        public string FactoryCountry { get; set; }
        [Description("Factory Province")]
        public string FactoryProvince { get; set; }
        [Description("Factory City")]
        public string FactoryCity { get; set; }
        [Description("Inspections")]
        public int? Inspection { get; set; }
        [Description("Reports")]
        public int Reports { get; set; }
        [Description("Order Qty")]
        public double? OrderQty { get; set; }
        [Description("Presented Qty")]
        public double? PresentedQty { get; set; }
        [Description("Inspected Qty")]
        public double? InspectedQty { get; set; }
        [Description("Estimated Manday")]
        public double? EstimatedManday { get; set; }
        [Description("Actual Manday")]
        public double? ActualManday { get; set; }
        [Description("ProductCount")]
        public int ProductCount { get; set; }
    }

    public enum CustomerMandayGroupByFields
    {
        Customer = 1,
        ServiceType = 2,
        FactoryCountry = 3,
        FactoryProvince = 4,
        FactoryCity = 5
    }
    public class GroupByCustomerMandayRequestFilter
    {
        public bool Customer { get; set; }
        public bool ServiceType { get; set; }
        public bool FactoryCountry { get; set; }
        public bool FactoryProvince { get; set; }
        public bool FactoryCity { get; set; }
    }
    public class CustomerMandayData
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int ServiceTypeId { get; set; }
        public string ServiceType { get; set; }
        public int FactoryProvinceId { get; set; }
        public string FactoryProvince { get; set; }
        public int FactoryCityId { get; set; }
        public string FactoryCity { get; set; }
        public int CountryId { get; set; }
        public string FactoryCountry { get; set; }
        public int ProductCount { get; set; }
        public int InspectionId { get; set; }
        public int ReportCount { get; set; }
        public double? OrderQty { get; set; }
        public double? PresentedQty { get; set; }
        public double? InspectedQty { get; set; }
        public double? EstimatedManday { get; set; }
        public double? ActualManday { get; set; }
    }
    public class InspectionProductCountDto
    {
        public int InspectionId { get; set; }
        public int ProductCount { get; set; }
    }

    public class GapCustomerKpiReportData
    {
        public int InspectionId { get; set; }
        public int ReportId { get; set; }
        public DateTime? InspectionStartedDate { get; set; }
        public DateTime? InspectionSubmittedDate { get; set; }
        public string InspectionStartTime { get; set; }
        public string InspectionEndTime { get; set; }
        public string ReportResult { get; set; }
        public string Region { get; set; }
        public string KeyStyleHighRisk { get; set; }
        public string DACorrelationDone { get; set; }
        public string FactoryTourDone { get; set; }
        public string DACorrelationRate { get; set; }
        public string DACorrelationEmail { get; set; }
        public string ExternalReportNumber { get; set; }
        public string ProductCategory { get; set; }
        public string LastAuditScore { get; set; }
        public string OtherCategory { get; set; }
        public string Market { get; set; }
        public string TotalScore { get; set; }
        public string Grade { get; set; }
        public string ReportNo { get; set; }
    }

    public class ExportGapCustomerProductRef
    {
        [Description("Inspection")]
        public int InspectionId { get; set; }
        [Description("Product Ref")]
        public string ProductRef { get; set; }
        [Description("PO Number")]
        public string PoNumber { get; set; }
        [Description("Supplier")]
        public string Supplier { get; set; }
        [Description("Supplier Code")]
        public string SupplierCode { get; set; }
        [Description("Factory")]
        public string Factory { get; set; }
        [Description("Factory Code")]
        public string FactoryCode { get; set; }
        [Description("Service Type")]
        public string ServiceType { get; set; }
        [Description("Inspection Started Date/Time")]
        public string InspectionStartedDateTime { get; set; }
        [Description("Submitted Inspection  Date/Time")]
        public string InspectionSubmittedDateTime { get; set; }
        [Description("Report Result")]
        public string ReportResult { get; set; }
        [Description("Qty to Inspect(PO Quantity)")]
        public double? OrderQty { get; set; }
        [Description("Quantity Inspected(Presented Qty)")]
        public double? PresentedQty { get; set; }
        [Description("Sample Size")]
        public double? InspectedQty { get; set; }
        [Description("Critical -VISUAL DEFECTS")]
        public int? CriticalVisualDefects { get; set; }
        [Description("Major - VISUAL DEFECTS")]
        public int? MajorVisualDefects { get; set; }
        [Description("Minor - VISUAL DEFECTS")]
        public int? MinorVisualDefects { get; set; }
        [Description("Total number of Visual Defect")]
        public int? TotalVisualDefects { get; set; }
        [Description("Total number of Measurement Defects")]
        public int? TotalDectiveUnits { get; set; }
        [Description("Actual Measured sample size")]
        public int ActualMeasuredSamplesize { get; set; }
        [Description("Visual sample size")]
        public int VisualSampleSize { get; set; }
        [Description("Measurement Defect Details")]
        public string MeasurementDefectDetails { get; set; }
        [Description("Critical Defects for Visual")]
        public string CriticalDefectsVisual { get; set; }
        [Description("Major Defects for Visual")]
        public string MajorDefectsVisual { get; set; }
        [Description("Minor Defects for Visual")]
        public string MinorDefectsVisual { get; set; }
        [Description("Factory Country")]
        public string FactoryCountry { get; set; }
        [Description("Region")]
        public string Region { get; set; }
        [Description("Office Country")]
        public string OfficeCountry { get; set; }
        [Description("Office")]
        public string Office { get; set; }
        [Description("Season")]
        public string Season { get; set; }
        [Description("Product Family")]
        public string ProductFamily { get; set; }
        [Description("Product Category")]
        public string ProductCategory { get; set; }
        [Description("Product Name")]
        public string ProductName { get; set; }
        [Description("Order Status")]
        public string InspectionStatus { get; set; }
        [Description("Key Style / High Risk")]
        public string KeyStyleHighRisk { get; set; }
        [Description("Brand")]
        public string Brand { get; set; }
        [Description("DA Correlation Done")]
        public string DACorrelationDone { get; set; }
        [Description("Factory Tour Done")]
        public string FactoryTourDone { get; set; }
        [Description("DAC Correlaction Rate")]
        public string DACCorrelactionRate { get; set; }
        [Description("DA Email")]
        public string DAEmail { get; set; }
        [Description("Inspector Name")]
        public string QcNames { get; set; }
        [Description("External Report Number")]
        public string ExternalReportNumber { get; set; }
        public int? ReportId { get; set; }
    }

    public class ExportARFollowUpReport
    {
        [Description("Invoice Type")]
        public string InvoiceType { get; set; }
        [Description("Quotation No")]
        public int? QuotationNo { get; set; }
        [Description("Customer Name")]
        public string CustomerName { get; set; }
        [Description("Booking No")]
        public int BookingNo { get; set; }
        [Description("PO#")]
        public string PoNo { get; set; }
        [Description("Supplier Name")]
        public string Supplier { get; set; }
        [Description("Factory Name")]
        public string Factory { get; set; }
        [Description("Factory City")]
        public string City { get; set; }
        [Description("Factory Country")]
        public string FactoryCountry { get; set; }
        [Description("Service Name")]
        public string ServiceName { get; set; }
        [Description("Service Type")]
        public string ServiceType { get; set; }
        [Description("Department")]
        public string Department { get; set; }
        [Description("Brand")]
        public string Brand { get; set; }
        [Description("Service From Date")]
        public string ServiceFromDate { get; set; }
        [Description("Service To Date")]
        public string ServiceToDate { get; set; }
        [Description("Invoice Date")]
        public string InvoiceDate { get; set; }
        [Description("Invoice No")]
        public string InvoiceNo { get; set; }

        [Description("Credit No")]
        public string CreditNo { get; set; }
        [Description("Invoice Amount")]
        public double? InvoiceAmount { get; set; }
        [Description("Currency")]
        public string Currency { get; set; }
        [Description("Invoice Billed To")]
        public string InvoiceBilledTo { get; set; }

        [Description("Invoice Billed Name")]
        public string InvoiceBilledToName { get; set; }
        [Description("Invoice Billed Address")]
        public string InvoiceBilledAddress { get; set; }
        [Description("Payment Terms")]
        public string PaymentTerms { get; set; }
        [Description("Office")]
        public string Office { get; set; }
        [Description("Company")]
        public string Company { get; set; }
        [Description("Invoice Status")]
        public string InvoiceStatus { get; set; }
        [Description("Payment Status")]
        public string PaymentStatus { get; set; }
        [Description("Payment Date")]
        public string PaymentDate { get; set; }
        [Description("Due Date")]
        public string DueDate { get; set; }
        [Description("Communication")]
        public string Communication { get; set; }
        [Description("In USD")]
        public double? InUSD { get; set; }
        [Description("DSO")]
        public string DSO { get; set; }
    }

    public class ExportGapCustomerFlashProcessAudit
    {
        [Description("Inspection")]
        public int InspectionId { get; set; }
        [Description("Report No")]
        public string ReportNo { get; set; }
        [Description("Service Type")]
        public string ServiceType { get; set; }
        [Description("Audit Product Category")]
        public string AuditProductCategory { get; set; }
        [Description("Pivot Audit#")]
        public string PivotAudit { get; set; }
        [Description("Started Audit Date / Time")]
        public string InspectionStartedTime { get; set; }
        [Description("Submitted Audit Date / Time")]
        public string InspectionSubmittedTime { get; set; }
        [Description("Last Audit Score")]
        public string LastAuditScore { get; set; }
        [Description("Auditor")]
        public string Auditor { get; set; }
        [Description("Supplier")]
        public string Supplier { get; set; }
        [Description("Supplier Code")]
        public string SupplierCode { get; set; }
        [Description("Factory")]
        public string Factory { get; set; }
        [Description("Factory Code")]
        public string FactoryCode { get; set; }
        [Description("Factory Country")]
        public string FactoryCountry { get; set; }

        [Description("Main Category")]
        public string MainCategory { get; set; }
        [Description("Other Category")]
        public string OtherCategory { get; set; }
        [Description("Market")]
        public string Market { get; set; }
        [Description("Total Score")]
        public string TotalScore { get; set; }
        [Description("Grade")]
        public string Grade { get; set; }
        public int ReportId { get; set; }

    }
    public class ExportInspectionData
    {
        [Description("Inspection")]
        public int BookingNo { get; set; }
        [Description("Product Code")]
        public string ProductName { get; set; }
        [Description("Product Description")]
        public string ProductDescription { get; set; }
        [Description("Customer Booking")]
        public string CustomerBookingNo { get; set; }
        [Description("Merchandiser")]
        public string Merchandise { get; set; }
        [Description("Customer Contact")]
        public string CustomerContact { get; set; }
        [Description("Supplier")]
        public string SupplierName { get; set; }
        [Description("Supplier Code")]
        public string SupplierCode { get; set; }
        [Description("Factory")]
        public string FactoryName { get; set; }
        [Description("Factory Code")]
        public string FactoryCode { get; set; }
        [Description("Service Type")]
        public string ServiceTypeName { get; set; }
        [Description("Inspection Status")]
        public string bookingStatus { get; set; }
        [Description("Service From Date")]
        public string ServiceFromDate { get; set; }
        [Description("Service To Date")]
        public string ServiceToDate { get; set; }
        [Description("FbResult")]
        public List<FbReportInspSummaryResult> FbResult { get; set; }
        [Description("Report Result")]
        public string ReportResult { get; set; }
        [Description("Customer Result")]
        public string CustomerResult { get; set; }
        [Description("Order Qty")]
        public double OrderQty { get; set; }
        [Description("Presented Qty")]
        public double ProducedQty { get; set; }
        [Description("Inspection Qty")]
        public double InspectedQty { get; set; }
        [Description("Critical")]
        public int? CriticalDefect { get; set; }
        [Description("Major")]
        public int? MajorDefect { get; set; }
        [Description("Minor")]
        public int? MinorDefect { get; set; }
        [Description("Critical Defects")]
        public string CriticalDefects { get; set; }
        [Description("Major Defects")]
        public string MajorDefects { get; set; }
        [Description("Minor Defects")]
        public string MinorDefects { get; set; }
        [Description("Factory Country")]
        public string FactoryCountry { get; set; }
        [Description("Office Country")]
        public string OfficeCountry { get; set; }
        [Description("Office")]
        public string Office { get; set; }
        [Description("Season")]
        public string Season { get; set; }
        [Description("Product Category")]
        public string ProductCategory { get; set; }
        [Description("Product Sub category")]
        public string ProductSubCategory { get; set; }
        [Description("Product Name")]
        public string ProductSubCategory2 { get; set; }
        [Description("Department")]
        public string DeptCode { get; set; }
        [Description("Brand")]
        public string BrandName { get; set; }
        [Description("Shipment Date")]
        public string ShipmentDate { get; set; }
    }
    public class GapKpiFbReportPackingPackagingLabellingProduct
    {
        public int FbReportDetailId { get; set; }

        public int SampleSize { get; set; }
        public int PackingType { get; set; }
    }

    public class KpiAuditBookingItems
    {
        public int AuditId { get; set; }
        public int? CustomerId { get; set; }
        public int? FactoryId { get; set; }
        public int? SupplierId { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public string ReportNo { get; set; }
        public string PivotAudit { get; set; }
        public string AuditStartTime { get; set; }
        public string AuditSubmittedTime { get; set; }
        public string LastAuditScore { get; set; }
        public string MainCategory { get; set; }
        public string OtherCategory { get; set; }
        public string Market { get; set; }
        public string Grade { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public string ScoreValues { get; set; }
        public string AuditProdutCategory { get; set; }

    }

    public class ExportGapCustomerProcessAudit
    {
        [Description("Audit")]
        public int AuditId { get; set; }
        [Description("Report No")]
        public string ReportNo { get; set; }
        [Description("Service Type")]
        public string ServiceType { get; set; }
        [Description("Audit Product Category")]
        public string AuditProductCategory { get; set; }
        [Description("Pivot Audit#")]
        public string PivotAudit { get; set; }
        [Description("Started Audit Date / Time")]
        public string AuditStartedTime { get; set; }
        [Description("Submitted Audit Date / Time")]
        public string AuditSubmittedTime { get; set; }
        [Description("Last Audit Score")]
        public string LastAuditScore { get; set; }
        [Description("Auditor")]
        public string Auditor { get; set; }
        [Description("Supplier")]
        public string Supplier { get; set; }
        [Description("Supplier Code")]
        public string SupplierCode { get; set; }
        [Description("Factory")]
        public string Factory { get; set; }
        [Description("Factory Code")]
        public string FactoryCode { get; set; }
        [Description("Factory Country")]
        public string FactoryCountry { get; set; }

        [Description("Main Category")]
        public string MainCategory { get; set; }
        [Description("Other Category")]
        public string OtherCategory { get; set; }
        [Description("Market")]
        public string Market { get; set; }
        [Description("Total Score")]
        public string TotalScore { get; set; }
        [Description("Grade")]
        public string Grade { get; set; }

    }

    public class InspectionSummaryExpenseTemplate
    {
        [Description("Office ")]
        public string Office { get; set; }
        [Description("Inspection#")]
        public int? BookingNo { get; set; }
        [Description("Order Status")]
        public string OrderStatus { get; set; }

        [Description("Service Date From")]
        public DateTime? ServiceDateFrom { get; set; }
        [Description("Service Date To")]
        public DateTime? ServiceDateTo { get; set; }
        [Description("Service Type")]
        public string ServiceTypeName { get; set; }
        [Description("Schedule Date")]
        public DateTime ScheduleDate { get; set; }
        [Description("QC Name")]
        public string QCName { get; set; }
        [Description("Customer")]
        public string CustomerName { get; set; }
        [Description("Supplier")]
        public string SupplierName { get; set; }
        [Description("Factory")]
        public string FactoryName { get; set; }
        [Description("Country Name")]
        public string FactoryCountry { get; set; }
        [Description("Factory City")]
        public string FactoryCity { get; set; }
        [Description("Factory Town")]
        public string FactoryTown { get; set; }

        [Description("Employee Type")]
        public string EmployeeTypeName { get; set; }
        [Description("Outsource Company")]
        public string OutsourceCompany { get; set; }

        [Description("Start Port")]
        public string StartPort { get; set; }
        [Description("Claim No.")]
        public string ClaimNumber { get; set; }
        [Description("Claim Date")]
        public DateTime? ClaimDate { get; set; }
        [Description("Claim Satus")]
        public string ClaimStatus { get; set; }
        [Description("Start City")]
        public string StartCity { get; set; }
        [Description("End City")]
        public string EndCity { get; set; }
        [Description("Payroll Currency")]
        public string PayrollCurrency { get; set; }
        [Description("No.Of Mandays")]
        public int? NumberOfManDay { get; set; }
        [Description("Expense Date")]
        public string ExpenseDate { get; set; }

        [Description("Expense Type")]
        public string ExpenseType { get; set; }

        [Description("Trip Type")]
        public string TripType { get; set; }
        [Description("Claim Amount")]
        public double? ClaimAmount { get; set; }

        [Description("Remarks")]
        public string ClaimRemarks { get; set; }

        [Description("Invoice No")]
        public string InvoiceNo { get; set; }

        [Description("Invoiced Currency")]
        public string InvoiceCurrency { get; set; }

        [Description("Invoiced Insp Fees")]
        public double? InvoiceInspectionFees { get; set; }

        [Description("Invoiced Travelling Fees")]
        public double? InvoiceTravellingFees { get; set; }

        [Description("Invoiced Other Fees")]
        public double? InvoiceOtherFees { get; set; }

        [Description("Invoiced Total Tax")]
        public double? InvoiceTotalTax { get; set; }

        [Description("Invoiced Total")]
        public double? InvoiceTotalFees { get; set; }
        [Description("Billed MD")]
        public double? InvoiceManDay { get; set; }
        [Description("Extra Fee Invoice No")]
        public string ExtraFeeInvoiceNo { get; set; }

        [Description("Extra Fee Invoice Status")]
        public string ExtraFeeInvoiceStatus { get; set; }

        [Description("Extra Fee Currency")]
        public string ExtraFeeCurrency { get; set; }

        [Description("Total Extra Fees")]
        public double? TotalExtraFees { get; set; }

        public int QcId { get; set; }
    }

    public class FbReportCheckpointResult
    {
        public int AuditId { get; set; }
        public string Name { get; set; }
        public string Result { get; set; }
    }

    public class InvoiceCommunication
    {
        public string InvoiceNo { get; set; }
        public string Comment { get; set; }
    }

    public enum KpiInvoicePaymentStatus
    {
        Pending = 1,
        Paid = 2
    }
}

