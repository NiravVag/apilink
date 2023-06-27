using DTO.Common;
using DTO.Invoice;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO.SaleInvoice
{
    public class SaleInvoiceSummary
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceCurrency { get; set; }
        public double TotalFee { get; set; }
        public string PaymentStatusName { get; set; }
        public int PaymentStatusId { get; set; }
        public string PaymentDate { get; set; }
        public string UniqueId { get; set; }
        public string InvoicedName { get; set; }
    }
    public class SaleInvoiceSummaryResponse
    {
        public List<SaleInvoiceSummary> Data { get; set; }
        public List<PaymentStatus> PaymentStatusCountList { get; set; }
        public SaleInvoiceSummaryResult Result { get; set; }
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
    }
    public class SaleInvoiceSummaryRequest
    {
        public int? DateTypeId { get; set; }
        public DateObject FromDate { get; set; }
        public DateObject ToDate { get; set; }
        public int? SearchTypeId { get; set; }
        public string SearchTypeText { get; set; }
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public IEnumerable<int> FactoryIdList { get; set; }
        public IEnumerable<int> PaymentStatusIdList { get; set; }
        public int? InvoiceTo { get; set; }
        [Required]
        public int ServiceId { get; set; }
        public bool IsExport { get; set; }
        public int? Index { get; set; }
        public int? pageSize { get; set; }
        public List<int> CustomerIdList { get; set; }
        public IEnumerable<int?> OfficeIdList { get; set; }
    }
    public class PaymentStatus
    {
        public int Id { get; set; }
        public string StatusName { get; set; }
        public int TotalCount { get; set; }
        public string StatusColor { get; set; }
    }
    public class SaleInvoiceFile
    {
        public string InvoiceNo { get; set; }
        public string UniqueId { get; set; }
    }
    public class SaleInvoiceSummarySearchResult
    {
        public List<InvoiceSummaryItem> InvoiceDataList { get; set; }
        public List<PaymentStatus> PaymentStatusCountList { get; set; }
        public List<InvoiceExtraFeeItem> InvoiceExtraFeeList { get; set; }
        public int TotalCount { get; set; }
    }

    public class ExportSalesInvoiceForInternalUserData
    {
        [Description("Invoice #")]
        public string InvoiceNo { get; set; }
        [Description("Invoice Name")]
        public string InvoiceName { get; set; }
        [Description("Invoice Date")]
        public string InvoiceDate { get; set; }
        [Description("Total Amount")]
        public double TotalFee { get; set; }
        [Description("Currency")]
        public string InvoiceCurrency { get; set; }
        [Description("Payment Status")]
        public string PaymentStatusName { get; set; }
        [Description("Payment Date")]
        public string PaymentDate { get; set; }
    }

    public class ExportSalesInvoiceForExternalUserData
    {
        [Description("Invoice #")]
        public string InvoiceNo { get; set; }
        [Description("Invoice Date")]
        public string InvoiceDate { get; set; }
        [Description("Total Amount")]
        public double TotalFee { get; set; }
        [Description("Currency")]
        public string InvoiceCurrency { get; set; }
        [Description("Payment Status")]
        public string PaymentStatusName { get; set; }
        [Description("Payment Date")]
        public string PaymentDate { get; set; }
    }

    public enum SaleInvoiceSummaryResult
    {
        Success = 1,
        NotFound = 2,
        Failure = 3,
        RequestNotCorrectFormat = 4
    }
}
