using DTO.Common;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Quotation
{
    public class QuotationExportRepo
    {
        public int QuotationNo { get; set; }
        public int BookingNo { get; set; }
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public int FactoryId { get; set; }
        public int StatusId { get; set; }
        public int OfficeId { get; set; }
        public string Office { get; set; }
        public string Customer { get; set; }
        public string Supplier { get; set; }
        public string Factory { get; set; }
        public DateTime QuotationDate { get; set; }
        public string Status { get; set; }
        public double EstimatedManDay { get; set; }
        public double InspectionCost { get; set; }
        public double? Discount { get; set; }
        public double? TravelCost { get; set; }
        public double TotalFees { get; set; }
        public string Currency { get; set; }
        public string BillPaidBy { get; set; }
        public int BillPaidById { get; set; }
        public string FactoryAddress { get; set; }
        public string BillPaidByAddress { get; set; }
        public string BillPaidByContact { get; set; }
        public string APIRemark { get; set; }
        public string CustomerRemark { get; set; }
        public string CustomerLegalName { get; set; }
        public string SupplierLegalName { get; set; }
        public string FactoryLegalName { get; set; }
        public string BillingEntity { get; set; }
        public double? OtherCost { get; set; }
        public string PaymentTerm { get; set; }
        public DateTime? ValidatedOn { get; set; }
        public string ValidatedByName { get; set; }
        public int? ValidatedByUserType { get; set; }
    }

    public class QuotationBookingMapRepo
    {
        public int QuotationNo { get; set; }
        public int BookingNo { get; set; }
    }

    public class QuotationBookingRepo
    {
        public int BookingNo { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public string ServiceType { get; set; }
        public string ProductReference { get; set; }
        public string PoNumber { get; set; }
        public string InspectionStatus { get; set; }
        public DateTime ActualInspectionDate { get; set; }
        public string CustomerBookingNo { get; set; }
    }

    public class QuotationBookingProductRepo
    {
        public int BookingNo { get; set; }
        public int ProductRefId { get; set; }
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public string ReportResult { get; set; }
        public int BookingQty { get; set; }
        public int SampleSize { get; set; }
        public int? CombineAQL { get; set; }
        public string FactoryReference { get; set; }
        public DateTime? FBReportStartDate { get; set; }
        public DateTime? FBReportEndDate { get; set; }
        public int? SampleSize8h { get; set; }
        public double? TimePrepatation { get; set; }
        public int? ProdSubCategory3Id { get; set; }
        public int? CombineProductId { get; set; }
        public int ProdUnit { get; set; }
    }

    public class QuotationBookingDFRepo
    {
        public int BookingNo { get; set; }
        public string DFName { get; set; }
        public string DFValue { get; set; }
        public int ControlType { get; set; }
        public int DFSourceType { get; set; }
    }


    public class BookingProductPoRepo
    {
        public int ProductRefId { get; set; }
        public int? ContainerRefId { get; set; }
        public int BookingId { get; set; }
        public string PoName { get; set; }
        public int ReportId { get; set; }
    }

    public class BookingInspectionDate
    {
        public int BookingNo { get; set; }
        public string InspectionDate { get; set; }
    }

    public class QuotationInvoiceItem
    {
        public int QuotationId { get; set; }
        public string QuoInvoiceNo { get; set; }
        public DateTime? QuoInvoiceDate { get; set; }
        public string QuoInvoiceREmarks { get; set; }
        public double? Manday { get; set; }
        public int BookingId { get; set; }
        public double? TravelTime { get; set; }
        public double? TravelDistance { get; set; }
        public string CusBookingNo { get; set; }
        public int BookingStatusId { get; set; }
        public string BookingStatusName { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public string ServiceTypeName { get; set; }
        public string BrandName { get; set; }
        public string DepartmentName { get; set; }
        public double? CalculatedWorkingHours { get; set; }
        public double? CalculatedWorkingManday { get; set; }
        public int? Quantity { get; set; }
        public string BilledQtyType { get; set; }
    }
    public class QuotationAuditReportItem
    {
        public int BookingId { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
    }
    public class InvoiceInfo
    {
        public int? Inspectiono { get; set; }
        public int? AuditNo { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string InvoiceREmarks { get; set; }
    }
    public class QuotationEditInvoiceItem
    {
        public int QuotationId { get; set; }
        public string InvoiceNo { get; set; }
        public DateObject InvoiceDate { get; set; }
        public string InvoiceREmarks { get; set; }
    }

    public class Manday
    {
        public int BookingId { get; set; }
        public DateTime ServiceDate { get; set; }
        public double NoOfManday { get; set; }
        public int OfficeId { get; set; }
        public int ZoneId { get; set; }
    }
    public class ZoneManday
    {
        public int BookingId { get; set; }
        public int ZoneId { get; set; }
    }

    public class QuotationBookingContactRepo
    {
        public int QuotationId { get; set; }
        public string ContactName { get; set; }
        public string Email { get; set; }
        public bool InvoiceEmail { get; set; }
        public bool IsEmail { get; set; }
    }
    public class CSNameRepo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? BookingId { get; set; }
    }

}
