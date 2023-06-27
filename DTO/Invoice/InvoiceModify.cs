using DTO.Common;
using DTO.CommonClass;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Invoice
{
    #region InvoiceBaseDetailResponse

    public class InvoiceBaseDetailResponse
    {
        public InvoiceBaseDetail InvoiceBaseDetail { get; set; }

        public InvoiceBaseDetailResult Result { get; set; }
    }

    public enum InvoiceBaseDetailResult
    {
        Success = 1,
        NotFound = 2,
        InvoiceNoEmpty = 3
    }

    public class InvoiceBaseDetailRepo
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? PostDate { get; set; }
        public string Subject { get; set; }
        public int? InspectionId { get; set; }
        public int? AuditId { get; set; }
        public int? BilledTo { get; set; }
        public string BilledName { get; set; }
        public string BilledAddress { get; set; }
        public List<InvAutTranContactDetail> ContactList { get; set; }
        public string PaymentTerms { get; set; }
        public string PaymentDuration { get; set; }
        public string InvoiceStatus { get; set; }
        public int? Office { get; set; }
        public InvoiceBankDetail BankDetails { get; set; }
        public int? BillingMethod { get; set; }
        public string Currency { get; set; }
        public int? InvoiceCurrency { get; set; }
        public double? ExchangeRate { get; set; }
        public int? BankId { get; set; }
        public int? PaymentStatus { get; set; }
        public int? InvoiceType { get; set; }
        public DateTime? PaymentDate { get; set; }
        public double? TaxValue { get; set; }
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public int? FactoryId { get; set; }
        public double? TotalInvoiceFees { get; set; }
        public double? TotalTaxAmount { get; set; }
        public double? TotalTravelFees { get; set; }
        public bool? IsTravelExpense { get; set; }
        public bool? IsInspectionFees { get; set; }
        public int InvoicingRequest { get; set; }
        public string InvoiceCurrencyName { get; set; }
        public string BilledQuantityType { get; set; }
    }

    public class InvoiceBankDetail
    {
        public int? BankId { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string BankAddress { get; set; }
        public string AccountCurrency { get; set; }
    }

    public class InvoiceBaseDetail
    {
        public string InvoiceNo { get; set; }
        public string OldInvoiceNo { get; set; }
        public DateObject InvoiceDate { get; set; }
        public DateObject PostDate { get; set; }
        public string Subject { get; set; }
        public int? BillTo { get; set; }
        public int? InvoiceType { get; set; }
        public string BilledName { get; set; }
        public string BilledAddress { get; set; }
        public IEnumerable<int?> ContactIds { get; set; }
        public string PaymentTerms { get; set; }
        public string PaymentDuration { get; set; }
        public string InvoiceStatus { get; set; }
        public int? Office { get; set; }
        public InvoiceBankDetail BankDetails { get; set; }
        public int? BillMethod { get; set; }
        public string Currency { get; set; }
        public int? PaymentStatus { get; set; }
        public int? InvoiceCurrency { get; set; }
        public double? ExchangeRate { get; set; }
        public double? TaxValue { get; set; }
        public DateObject PaymentDate { get; set; }
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public int? FactoryId { get; set; }
        public double? TotalInvoiceFees { get; set; }
        public double? TotalTaxAmount { get; set; }
        public double? TotalTravelFees { get; set; }
        public bool? IsTravelExpense { get; set; }
        public bool? IsInspectionFees { get; set; }
        public int InvoicingRequest { get; set; }
        public string InvoiceCurrencyName { get; set; }
        public string BilledQuantityType { get; set; }
    }



    #endregion

    #region InvoiceBilledAddress
    public class InvoiceBilledAddressResponse
    {
        public IEnumerable<CommonDataSource> billedAddress { get; set; }

        public InvoiceBilledAddressResult Result { get; set; }
    }

    public enum InvoiceBilledAddressResult
    {
        Success = 1,
        AddressNotFound = 2,
        BillToIdCannotBeEmpty = 3,
        SearchIdCannotBeEmpty = 4
    }

    #endregion

    #region InvoiceBilledContacts
    public class InvoiceContactsResponse
    {
        public IEnumerable<CommonDataSource> Contacts { get; set; }

        public InvoiceBilledContactsResult Result { get; set; }
    }

    public enum InvoiceBilledContactsResult
    {
        Success = 1,
        ContactsNotFound = 2,
        BillToIdCannotBeEmpty = 3,
        SearchIdCannotBeEmpty = 4
    }

    #endregion

    public class InvoiceTransactionDetailRepo
    {
        public int Id { get; set; }
        public int? BookingNo { get; set; }
        public int? AuditNo { get; set; }
        public string CustomerBookingNo { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public string ServiceType { get; set; }
        public string Customer { get; set; }
        public string Supplier { get; set; }
        public string Factory { get; set; }
        public string PriceCategory { get; set; }
        public int? FactoryId { get; set; }
        public double? ManDay { get; set; }
        public double? UnitPrice { get; set; }
        public double? InspectionFees { get; set; }
        public double? AirCost { get; set; }
        public double? LandCost { get; set; }
        public double? HotelCost { get; set; }
        public double? OtherCost { get; set; }
        public double? Discount { get; set; }
        public double? TravelOtherFees { get; set; }
        public double? TravelTotalFees { get; set; }
        public double? ExtraFees { get; set; }
        public bool? IsTravelExpense { get; set; }
        public bool? IsInspectionFees { get; set; }
        public string Remarks { get; set; }
        public int? InvoiceCurrency { get; set; }
        public int? BankId { get; set; }
        public int? BilledTo { get; set; }

    }

    public class InvoiceTransactionDetailsResponse
    {
        public List<InvoiceTransactionDetails> transactionDetails { get; set; }
        public InvoiceTransactionDetailsResult Result { get; set; }
    }

    public enum InvoiceTransactionDetailsResult
    {
        Success = 1,
        DataNotFound = 2,
        InvoiceNoCannotBeEmptyOrZero = 3
    }


    public class InvoiceTransactionDetails
    {
        public int Id { get; set; }
        public int? BookingNo { get; set; }
        public string CustomerBookingNo { get; set; }
        public int? QuotationNo { get; set; }
        public string ServiceDateFrom { get; set; }
        public string ServiceDateTo { get; set; }
        public string ServiceType { get; set; }
        public string Customer { get; set; }
        public string Supplier { get; set; }
        public string Factory { get; set; }
        public string FactoryCountry { get; set; }
        public string FactoryProvince { get; set; }
        public string FactoryCounty { get; set; }
        public string FactoryCity { get; set; }
        public string FactoryTown { get; set; }
        public string PriceCategory { get; set; }
        public int? TotalBookingQty { get; set; }
        public double? TotalInspectedQty { get; set; }
        public double? TotalPresentedQty { get; set; }
        public double? ManDay { get; set; }
        public string UnitPrice { get; set; }
        public string InspectionFees { get; set; }
        public string AirCost { get; set; }
        public string LandCost { get; set; }
        public string HotelCost { get; set; }
        public string OtherCost { get; set; }
        public string Discount { get; set; }
        public string ExtraFees { get; set; }
        public string ExtraFeeSubTotal { get; set; }
        public string ExtraFeeTax { get; set; }
        public string TravelOtherFees { get; set; }
        public string TravelTotalFees { get; set; }
        public bool? IsTravelExpense { get; set; }
        public bool? IsInspectionFees { get; set; }
        public string Remarks { get; set; }
        public int ReportCount { get; set; }
    }

    public class BookingQuantityDetails
    {
        public int BookingNo { get; set; }
        public int ProductId { get; set; }
        public int? FbReportId { get; set; }
        public int? BookingQty { get; set; }
        public int? InspectedQt { get; set; }
    }

    public class InvoiceBookingQuantityDetails
    {
        public int BookingNo { get; set; }
        public int? BookingQty { get; set; }
        public double? InspectedQuantity { get; set; }
        public double? PresentedQuantity { get; set; }
    }

    public class InvoiceBookingQuotation
    {
        public int BookingNo { get; set; }
        public int QuotationNo { get; set; }
        public string QuotationStatus { get; set; }
        public int QuotationStatusId { get; set; }
        public int? PaymentTermsId { get; set; }
        public int BillTo { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        //public string SupplierAddress { get; set; }
        public int FactoryId { get; set; }
        public string FactoryName { get; set; }
        public string FactoryAddress { get; set; }
        //public int? SupplierContacts { get; set; }
        //public int? FactoryContacts { get; set; }

        public string QuotationBilledName { get; set; }
        public double QuotationTotalFees { get; set; }
        public string QuotationCurrencyName { get; set; }
        public string QuotationCurrencyCode { get; set; }
        public int QuotationCurrencyId { get; set; }
        public string BillingEntityName { get; set; }
        public int BillingEntityId { get; set; }
        public string BankName { get; set; }
        public int BankId { get; set; }
        public int BankcurrencyId { get; set; }
        public string BankcurrencyName { get; set; }
        public int ExchangeRate { get; set; }
        public List<BankTaxData> TaxList { get; set; }
        public string CustomerLegalName { get; set; }
        public string SupplierLegalName { get; set; }
        public string FactoryLegalName { get; set; }
    }

    public class InvoiceBookingServiceTypes
    {
        public int BookingNo { get; set; }
        public int ServiceTypeId { get; set; }
        public string ServiceType { get; set; }
    }

    public class InvoiceBookingMoreInfoResponse
    {
        public InvoiceBookingMoreInfo InvoiceBookingMoreInfo { get; set; }

        public InvoiceBookingMoreInfoResult Result { get; set; }
    }

    public enum InvoiceBookingMoreInfoResult
    {
        Success = 1,
        NotFound = 2,
        BookingNoCannotBeEmptyOrZero = 3
    }

    public class InvoiceBookingMoreInfo
    {
        public int BookingNo { get; set; }
        public string Brands { get; set; }
        public string Departments { get; set; }
        public string PriceCategory { get; set; }
        public string QCNames { get; set; }
        public string Collection { get; set; }
        public double? PresentedQuantity { get; set; }
    }

    public class InvoiceBookingProductsResponse
    {
        public List<InvoiceBookingProducts> InvoiceBookingProducts { get; set; }

        public InvoiceBookingProductResult Result { get; set; }
    }

    public class InvoiceBookingProducts
    {
        public int ProductId { get; set; }
        public int ProductRefId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string PoNumber { get; set; }
        public int BookingQuantity { get; set; }
        public double? PresentedQuantity { get; set; }
        public double? InspectionQuantity { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSubCategory { get; set; }
        public string ProductSubCategory2 { get; set; }
    }

    public enum InvoiceBookingProductResult
    {
        Success = 1,
        NotFound = 2,
        BookingNoCannotBeEmptyOrZero = 3
    }

    public class InvoiceBooking
    {
        public int BookingNo { get; set; }
        public string Brands { get; set; }
        public string Departments { get; set; }
        public string PriceCategory { get; set; }
        public string QCNames { get; set; }
        public string Collection { get; set; }
    }

    public class DeleteInvoiceDetailResponse
    {
        public DeleteInvoiceDetailResult Result { get; set; }
    }

    public enum DeleteInvoiceDetailResult
    {
        DeleteSuccess = 1,
        DeleteFailed = 2
    }

    public class InvoiceMoExistsResult
    {
        public bool isInvoiceNoExists { get; set; }

    }




    public class InvoiceBookingProductsData
    {
        public int PoTranId { get; set; }

        public int Id { get; set; }

        public int BookingId { get; set; }

        public string ProductId { get; set; }

        public string DestinationCountry { get; set; }

        public string PoNumber { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public string ProductCategory { get; set; }

        public string ProductSubCategory { get; set; }

        public string ProductSubCategory2 { get; set; }

        public int? ProductBookingQuantity { get; set; }

        public string ProductBarCode { get; set; }
        public string FactoryReference { get; set; }

        public DateTime? ETD { get; set; }
        public int POBookingQty { get; set; }
        public int POId { get; set; }

        public int? CombineProductId { get; set; }
        public int? CombineAQLQty { get; set; }
        public int? AQLQty { get; set; }
        public string ReportNumber { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public string FactoryName { get; set; }

    }

    public class FactoryContact
    {
        public int FactoryId { get; set; }
        public int FactoryContactId { get; set; }
    }

    public class SupplierContact
    {
        public int SupplierId { get; set; }
        public int SupplierContactId { get; set; }
    }

    public class SupplierAddressDetails
    {
        public int SupplierId { get; set; }
        public string SupplierAddress { get; set; }
    }
}
