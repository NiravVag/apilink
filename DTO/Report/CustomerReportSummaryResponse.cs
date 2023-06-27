using DTO.Inspection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DTO.Report
{
    public class CustomerReportSummaryResponse
    {
        public IEnumerable<CustomerReportItem> Data { get; set; }

        public int TotalCount { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public IEnumerable<InspectionStatus> InspectionStatuslst { get; set; }

        public CustomerReportSummaryResponseResult Result { get; set; }
    }

    public class CustomerReportItem
    {
        public int BookingId { get; set; }

        public string CustomerBookingNo { get; set; }

        public int? CustomerId { get; set; }
        public int? FactoryId { get; set; }

        public int? FbMissionId { get; set; }

        public string CustomerName { get; set; }

        public string SupplierName { get; set; }

        public string FactoryName { get; set; }

        public string ProductCategory { get; set; }

        public string ServiceType { get; set; }

        public int? ServiceTypeId { get; set; }

        public string ServiceDateFrom { get; set; }

        public string ServiceDateTo { get; set; }

        public string ReportNo { get; set; }

        public string Office { get; set; }

        public int StatusId { get; set; }

        public string StatusName { get; set; }

        public int? BookingCreatedBy { get; set; }

        public bool? IsPicking { get; set; }

        public int? PreviousBookingNo { get; set; }

        public string ReportSummaryLink { get; set; }

        public string OverAllStatus { get; set; }

        //public int FbMissionId { get; set; }

        public IEnumerable<ReportProductItem> ReportProducts { get; set; }

        public string ReportDate { get; set; }
        public int ProductCount { get; set; }
        public IEnumerable<InspectionCsData> InspectionCsList { get; set; }
        public int? BookingType { get; set; }
        public bool? IsEAQF { get; set; }

    }

    public class ReportProductItem
    {
        public int Id { get; set; }
        public int bookingId { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductQuantity { get; set; }
        public string ProductCategoryName { get; set; }
        public string ProductSubCategoryName { get; set; }
        public int FbReportId { get; set; }
        public int ApiReportId { get; set; }
        public string ReportStatus { get; set; }
        public string FillingStatus { get; set; }
        public string ReviewStatus { get; set; }
        public int? CombineProductId { get; set; }
        public string ColorCode { get; set; }
        public string PONumber { get; set; }
        public double? InspectedQuantity { get; set; }
        public string finalReportLink { get; set; }
        public string finalManualReportLink { get; set; }
        public int CombineProductCount { get; set; }
        public bool IsParentProduct { get; set; }
        public string Result { get; set; }
        public string ReportTitle { get; set; }
        public string ResultColor { get; set; }
        public int? CombineAqlQuantity { get; set; }
        public string ReportStatusColor { get; set; }
        public string FillingStatusColor { get; set; }
        public string ReviewStatusColor { get; set; }
        public string QcName { get; set; }
        public string ContainerName { get; set; }
        public int? ReportStatusId { get; set; }
    }

    public enum CustomerReportSummaryResponseResult
    {
        Success = 1,
        NotFound = 2,
        Other = 3
    }

    public class InternalFBReportSummaryResponse
    {
        public IEnumerable<InternalFBReportItem> Data { get; set; }

        public int TotalCount { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public IEnumerable<InspectionStatus> InspectionStatuslst { get; set; }

        public InternalReportSummaryResponseResult Result { get; set; }
    }

    public class InternalFBReportItem
    {
        public int BookingId { get; set; }

        public string CustomerBookingNo { get; set; }

        public int? CustomerId { get; set; }

        public int? FactoryId { get; set; }

        public int? SupplierId { get; set; }

        public string CustomerName { get; set; }

        public string SupplierName { get; set; }

        public string FactoryName { get; set; }

        public string ServiceType { get; set; }

        public string ServiceDateFrom { get; set; }

        public string ServiceDateTo { get; set; }

        public int? ReportNo { get; set; }

        public string Office { get; set; }

        public int StatusId { get; set; }

        public string StatusName { get; set; }

        public int? BookingCreatedBy { get; set; }

        public bool IsPicking { get; set; }

        public int? PreviousBookingNo { get; set; }

        public IEnumerable<InternalReportProductItem> ReportProducts { get; set; }
    }

    public class InternalReportProductItem
    {
        public int bookingId { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductQuantity { get; set; }
        public string ProductCategoryName { get; set; }
        public string ProductSubCategoryName { get; set; }
        public int FbReportId { get; set; }
        public string ReportStatus { get; set; }
        public int? CombineProductId { get; set; }
        public string ColorCode { get; set; }
        public string PONumber { get; set; }
        public double? InspectedQuantity { get; set; }
        public string finalReportLink { get; set; }
        public int CombineProductCount { get; set; }
        public bool IsParentProduct { get; set; }
        public string Result { get; set; }
        public string ReportTitle { get; set; }
        public string FillingStatus { get; set; }
        public string ReviewStatus { get; set; }
        public string ResultColor { get; set; }
        public string ContainerName { get; set; }
    }

    public enum InternalReportSummaryResponseResult
    {
        Success = 1,
        NotFound = 2,
        Failure = 3,
        Other = 4
    }

    public class ExportInspectionReportData
    {
        public string Customer { get; set; }
        public string CustomerBookingNo { get; set; }
        public int BookingNo { get; set; }
        public string Supplier { get; set; }
        public string Factory { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public string ProductCategory { get; set; }
        public string ProductId { get; set; }
        public string ProductDescription { get; set; }
        public int BookingQuantity { get; set; }
        public int? FbReportId { get; set; }
        public string PoNumber { get; set; }
        public int ProductRefId { get; set; }
        public int? ContainerRefId { get; set; }
        public int SupplierId { get; set; }
        public int CustomerId { get; set; }
    }

    #region ExportReportObjects

    public class ExportFBReportRepo
    {
        public int? FbReportId { get; set; }
        public int? InspectionId { get; set; }
        public double? InspectedQuantity { get; set; }
        public string ReportNo { get; set; }
        public string FillingStatus { get; set; }
        public string ReviewStatus { get; set; }
        public string ReportStatus { get; set; }
        public string Result { get; set; }
    }

    public class ExportCustomerReportData
    {
        [Description("Customer")]
        public string Customer { get; set; }
        [Description("Customer Booking No")]
        public string CustomerBookingNo { get; set; }
        [Description("Booking No")]
        public int BookingNo { get; set; }
        [Description("Supplier")]
        public string Supplier { get; set; }
        [Description("Supplier Code")]
        public string SupplierCode { get; set; }
        [Description("Factory")]
        public string Factory { get; set; }
        [Description("Service Date From")]
        public DateTime ServiceDateFrom { get; set; }
        [Description("Service Date To")]
        public DateTime ServiceDateTo { get; set; }
        [Description("Service Type")]
        public string ServiceType { get; set; }
        [Description("Product Category")]
        public string ProductCategory { get; set; }
        [Description("Product Id")]
        public string ProductId { get; set; }
        [Description("Product Description")]
        public string ProductDescription { get; set; }
        [Description("PO Number")]
        public string PoNumber { get; set; }
        [Description("Quantity")]
        public int Quantity { get; set; }
        [Description("Container Name")]
        public string ContainerName { get; set; }
        [Description("Inspected Quantity")]
        public double? InspectedQuantity { get; set; }
        [Description("Report No")]
        public string ReportNo { get; set; }
        [Description("Report Status")]
        public string ReportStatus { get; set; }
        [Description("Result")]
        public string Result { get; set; }
    }

    public class ExportFillingReportData
    {
        [Description("Customer")]
        public string Customer { get; set; }
        [Description("Customer Booking No")]
        public string CustomerBookingNo { get; set; }
        [Description("Booking No")]
        public int BookingNo { get; set; }
        [Description("Supplier")]
        public string Supplier { get; set; }
        [Description("Factory")]
        public string Factory { get; set; }
        [Description("Service Date From")]
        public DateTime ServiceDateFrom { get; set; }
        [Description("Service Date To")]
        public DateTime ServiceDateTo { get; set; }
        [Description("Service Type")]
        public string ServiceType { get; set; }
        [Description("Product Category")]
        public string ProductCategory { get; set; }
        [Description("Product Id")]
        public string ProductId { get; set; }
        [Description("Product Description")]
        public string ProductDescription { get; set; }
        [Description("PO Number")]
        public string PoNumber { get; set; }
        [Description("Quantity")]
        public int Quantity { get; set; }
        [Description("Container Name")]
        public string ContainerName { get; set; }
        [Description("Inspected Quantity")]
        public double? InspectedQuantity { get; set; }
        [Description("Report No")]
        public string ReportNo { get; set; }
        [Description("Filling Status")]
        public string FillingStatus { get; set; }
        [Description("Review Status")]
        public string ReviewStatus { get; set; }
        [Description("Report Status")]
        public string ReportStatus { get; set; }
        [Description("Result")]
        public string Result { get; set; }
        [Description("Cs Names")]
        public string CsNames { get; set; }


    }

    public class ProductPOList
    {
        public int ProductRefId { get; set; }
        public int? FbReportId { get; set; }
        public int PoId { get; set; }
        public string PoNumber { get; set; }
    }

    public class ExportReportData
    {
        public List<ExportCustomerReportData> customerReportData { get; set; }
        public List<ExportFillingReportData> fillingReportData { get; set; }
    }

    #endregion

}
