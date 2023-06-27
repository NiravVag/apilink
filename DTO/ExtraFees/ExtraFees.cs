using DTO.Common;
using DTO.CommonClass;
using DTO.Invoice;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DTO.ExtraFees
{
    public class ExtraFees
    {
        [RequiredGreaterThanZero(ErrorMessage = "EXTRA_FEE.MSG_CUSTOMER_REQ")]
        public int CustomerId { get; set; }

        [RequiredGreaterThanZero(ErrorMessage = "EXTRA_FEE.MSG_BILLED_TO_REQ")]
        public int BilledToId { get; set; }

        [RequiredGreaterThanZero(ErrorMessage = "EXTRA_FEE.MSG_CURRENCY_REQ")]
        public int CurrencyId { get; set; }

        [RequiredGreaterThanZero(ErrorMessage = "EXTRA_FEE.MSG_SERVICE_REQ")]
        public int ServiceId { get; set; }

        [RequiredGreaterThanZero(ErrorMessage = "EXTRA_FEE.MSG_BOOKING_NUMBER_REQ")]
        public int BookingNumberId { get; set; }

        public int Id { get; set; }

        public int? SupplierId { get; set; }
        public int? FactoryId { get; set; }
        public int? BillingEntityId { get; set; }
        public int? BankId { get; set; }
        public int? PaymentStatusId { get; set; }
        public int? InvoiceNumberId { get; set; }
        public double TaxValue { get; set; }
        public double TaxAmt { get; set; }
        public int? TaxId { get; set; }

        public DateObject ExtraFeeInvoiceDate { get; set; }

        [StringLength(5000)]
        public string ExtraFeeInvoiceNo { get; set; }

        [StringLength(5000)]
        public string Remarks { get; set; }

        [DateShouldBeGreaterInNew(otherPropertyName = "Id", ErrorMessage = "EXTRA_FEE.MSG_EXTRA_TYPE_FROMDATE_FUTURE_REQ")]
        public DateObject PaymentDate { get; set; }

        [RequiredGreaterThanZero(ErrorMessage = "EXTRA_FEE.MSG_SUBTOTAL_REQ", FieldType = typeof(double))]
        public double SubTotal { get; set; }

        //[RequiredGreaterThanZero(ErrorMessage = "EXTRA_FEE.MSG_TOTAL_FEES_REQ")]
        public double TotalFees { get; set; }

        [RequiredList(ErrorMessage = "EXTRA_FEE.MSG_EXTRA_TYPE_LIST_REQ")]
        public List<EditExtraFeeType> ExtraFeeTypeList { get; set; }

        [RequiredGreaterThanZero(ErrorMessage = "EXTRA_FEE.MSG_OFF_REQ")]
        public int OfficeId { get; set; }

        public List<int> ContactIdList { get; set; }
        public int? InvoiceCurrencyId { get; set; }
        public double? ExchangeRate { get; set; }
        [StringLength(500)]
        public string BilledName { get; set; }
        [StringLength(1000)]
        public string BilledAddress { get; set; }    
        [StringLength(500)]
        public string PaymentTerms { get; set; }        
        public int PaymentDuration { get; set; }
        public ExtraFees()
        {
            ExtraFeeTypeList = new List<EditExtraFeeType>();
        }
    }

    public class EditExtraFeeType
    {
        [RequiredGreaterThanZero(ErrorMessage = "EXTRA_FEE.MSG_TYPE_REQ")]
        public int? TypeId { get; set; }

        [RequiredGreaterThanZero(ErrorMessage = "EXTRA_FEE.MSG_FEES_REQ", FieldType = typeof(double))]
        public double? Fees { get; set; }

        [StringLength(5000)]
        public string Remarks { get; set; }
    }

    public class BookingRepo
    {
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public int? FactoryId { get; set; }
        public DateTime ServiceDate { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName{ get; set; }
        public string FactoryName{ get; set; }
    }

    public class BookingDataSourceRequest
    {
        public string SearchText { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public int? FactoryId { get; set; }
        public int? BookingId { get; set; }
        public int? ServiceId { get; set; }
    }

    public class BookingDataItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ServiceDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int FactoryId { get; set; }
        public string  FactoryName { get; set; }
    }
    public class BookingDataResponse
    {
        public List<BookingDataItem> Bookings { get; set; }
        public DataSourceResult Result { get; set; }
    }
    public class SaveResponse
    {
        public int Id { get; set; }
        public Result Result { get; set; }
    }

    public class ManualInvoiceResponse
    {
        public int Id { get; set; }
        public string ExtraFeeNumber { get; set; }
        public Result Result { get; set; }
    }

    public enum Result
    {
        Success = 1,
        NotFound = 2,
        Failure = 3,
        RequestNotCorrectFormat = 4,
        Exists = 5,
        InvoiceIdMapped = 6,
        DuplicateInvoice = 7
    }

    public enum ExtraFeeStatus
    {
        Pending = 1,
        Invoiced = 2,
        Cancelled = 3
    }
    public class TaxResponse
    {
        public List<InvoiceBankTax> TaxDetail { get; set; }
        public Result Result { get; set; }
    }

    public class EditResponse
    {
        public EditExtraFees Data { get; set; }
        public Result Result { get; set; }
    }

    public class CancelResponse
    {
        public Result Result { get; set; }
    }
    public class EditExtraFees
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public int? FactoryId { get; set; }
        public int? BilledToId { get; set; }
        public string BilledName { get; set; }
        public string BilledAddress { get; set; }
        public string PaymentTerms { get; set; }
        public int? PaymentDuration { get; set; }
        public int? CurrencyId { get; set; }
        public int? ServiceId { get; set; }
        public int? BillingEntityId { get; set; }
        public int? BankId { get; set; }
        public int? BookingNumberId { get; set; }
        public int? PaymentStatusId { get; set; }
        public int? InvoiceNumberId { get; set; }
        public double? TaxValue { get; set; }
        public double? TaxAmt { get; set; }
        public int? TaxId { get; set; }
        public string Remarks { get; set; }
        public DateObject PaymentDate { get; set; }
        public string ExtraFeeInvoiceNo { get; set; }
        public double? SubTotal { get; set; }
        public double? TotalFees { get; set; }
        public string StatusName { get; set; }
        public int? StatusId { get; set; }
        public List<EditExtraFeeType> ExtraFeeTypeList { get; set; }
        public int? OfficeId { get; set; }
        public List<int> ContactIdList { get; set; }
        public List<CommonDataSource> ContactList { get; set; }
        public DateObject ExtraFeeInvoiceDate { get; set; }
        public int? InvoiceCurrencyId { get; set; }
        public double? ExchangeRate { get; set; }
        public EditExtraFees()
        {
            ExtraFeeTypeList = new List<EditExtraFeeType>();
        }
    }
    public class ExtraFeeRequest
    {
        public int SearchTypeId { get; set; }
        public string SearchTypeText { get; set; }
        public int DateTypeid { get; set; }
        public DateObject FromDate { get; set; }
        public DateObject ToDate { get; set; }
        public int? serviceId { get; set; }
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public int? Index { get; set; }
        public int? pageSize { get; set; }
        public int? billedTo { get; set; }
        public List<int> statuslst { get; set; }
    }

    public class ExtraFeeSummaryItem
    {
        public int? BookingId { get; set; }
        public string CustomerBookingNo { get; set; }
        public string BilledTo { get; set; }
        public int? BilledToId { get; set; }
        public string ExFeeType { get; set; }
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public string CustomerName { get; set; }
        public double? TotalAmt { get; set; }
        public string Currency { get; set; }
        public string Service { get; set; }
        public int? ServiceId { get; set; }
        public int? ExfTranId { get; set; }
        public DateTime? ApplyDate { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public string InvoiceNo { get; set; }
        public string ExtraFeeInvoiceNo { get; set; }
        public string StatusName { get; set; }
        public int? StatusId { get; set; }
        public string SupplierName { get; set; }
        public string Remarks { get; set; }
        public double? ExtraTypefee { get; set; }
        public string ExtraTypeRemarks { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PaymentStatus { get; set; }
        public string InvoiceCurrency { get; set; }
        public double? ExchangeRate { get; set; }
    }

    public class ExtraFeeSummaryResponseItem
    {
        public int extrafeeid { get; set; }
        public int? BookingId { get; set; }
        public string CustomerBookingNo { get; set; }
        public string BilledTo { get; set; }
        public string ExFeeType { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public double? TotalAmt { get; set; }
        public string Currency { get; set; }
        public string Service { get; set; }
        public string InvoiceNumber { get; set; }
        public string ExtraFeeInvoiceNumber { get; set; }
        public string Remarks { get; set; }
        public int? StatusId { get; set; }
        public string InvoiceCurrency { get; set; }
        public double? ExchangeRate { get; set; }
    }

    public enum ExtraFeeSummaryResult
    {
        Success = 1,
        NotFound = 2,
        Fail = 3
    }
    public class ExtraFeeSummaryStatus
    {
        public int Id { get; set; }

        public string StatusName { get; set; }

        public string StatusColor { get; set; }

        public int TotalCount { get; set; }

        public int Priority { get; set; }
    }
    public class ExtraFeeResponse
    {
        public List<ExtraFeeSummaryResponseItem> Data { get; set; }
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public ExtraFeeSummaryResult Result { get; set; }
        public List<ExtraFeeSummaryStatus> StatusCountList { get; set; }
    }
    public class ExtraFeeSummaryExportItem
    {
        public int? BookingId { get; set; }
        public string CustomerBookingNo { get; set; }
        public string BilledTo { get; set; }
        public string ExFeeType { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public double? TotalAmt { get; set; }
        public string Currency { get; set; }
        public string Service { get; set; }
        public string InvoiceNumber { get; set; }
        public string ExtraFeeInvoiceNumber { get; set; }
        public string Remarks { get; set; }
        public double? ExtraTypefee { get; set; }
        public string ExtraTypeRemarks { get; set; }
        public string StatusName { get; set; }
        public string InvoiceDate { get; set; }
        public string PaymentDate { get; set; }
        public string PaymentStatus { get; set; }
        public string InvoiceCurrency { get; set; }
        public double? ExchangeRate { get; set; }
        public int? ExtraFeeId { get; set; }
    }

    public class EditExtraFeesRepo
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public int? FactoryId { get; set; }
        public int? BilledToId { get; set; }
        public int? CurrencyId { get; set; }
        public int? ServiceId { get; set; }
        public int? BillingEntityId { get; set; }
        public int? BankId { get; set; }
        public int? BookingNumberId { get; set; }
        public int? PaymentStatusId { get; set; }
        public int? InvoiceNumberId { get; set; }
        public double? TaxValue { get; set; }
        public double? TaxAmt { get; set; }
        public int? TaxId { get; set; }
        public string Remarks { get; set; }
        public DateTime? PaymentDate { get; set; }
        public DateTime? ExtraFeeInvoiceDate { get; set; }
        public string ExtraFeeInvoiceNo { get; set; }
        public double? SubTotal { get; set; }
        public double? TotalFees { get; set; }
        public string StatusName { get; set; }
        public int? StatusId { get; set; }
        public List<EditExtraFeeType> ExtraFeeTypeList { get; set; }
        public int? OfficeId { get; set; }
        public List<ExtraFeeContactData> ContactIdList { get; set; }
        public int? AuditId { get; set; }
        public int? InvoiceCurrencyId { get; set; }
        public double? ExchangeRate { get; set; }
        public string BilledName { get; set; }
        public string BilledAddress { get; set; }
        public string PaymentTerms { get; set; }
        public int? PaymentDuration { get; set; }
        public EditExtraFeesRepo()
        {
            ExtraFeeTypeList = new List<EditExtraFeeType>();
        }
    }
    public class ExtraFeesTaxData
    {
        public int? ExtraFeeId { get; set; }
        public int? TaxId { get; set; }
    }
    public class ExtraFeeData
    {
        public int? BookingId { get; set; }
        public int? InvoiceId { get; set; }
        public double? ExtraFee { get; set; }
        public int? BilledTo { get; set; }
        public string Remarks { get; set; }
        public int ExtraFeeId { get; set; }
        public string ExtraFeeCurrency { get; set; }
        public int? BankId { get; set; }
        public string BankName { get; set; }
        public string BankAddress { get; set; }
        public string BankSwiftCode { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public List<int?> BankTaxList { get; set; }
        public string OfficeName { get; set; }
        public string OfficeAddress { get; set; }
        public string OfficePhone { get; set; }
        public string OfficeFax { get; set; }
        public string OfficeWebsite { get; set; }
        public string OfficeMail { get; set; }
        public int? ServiceId { get; set; }
        public string BilledAddress { get; set; }
        public string PaymentTerms { get; set; }
    }

    public class ExtraFeeTypeData
    {
        public string ExtraFeeType { get; set; }
        public double? ExtraFee { get; set; }
        public string Remarks { get; set; }
        public int? ExtraFeeId { get; set; }
        public double? ExchangeRate { get; set; }
    }

    public class ExtraFeeContactData
    {
        public int? CustomerContactId { get; set; }
        public int? FactContactId { get; set; }
        public int? SupContactId { get; set; }
        public string CustomerContactName { get; set; }
        public string FactContactName { get; set; }
        public string SupContactName { get; set; }
    }

    public class BookingQuantity
    {
        public int? BookingId { get; set; }
        public double? InspectedQty { get; set; }
        public double? PresentedQty { get; set; }
        public int InspPOTransId { get; set; }

    }
}
