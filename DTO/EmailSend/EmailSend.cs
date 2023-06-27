using DTO.CommonClass;
using DTO.Quotation;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.EmailSend
{
    public class EmailSend
    {
        public int BookingId { get; set; }
        public int ReportId { get; set; }
        public string ReportLink { get; set; }
        public string FinalManualReportLink { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public IEnumerable<string> POIdList { get; set; }
        public List<CommonDataSource> POList { get; set; }
        public string POIds { get; set; }
        public string ContainerNumber { get; set; }
        public int? ContainerId { get; set; }
        public int TotalBookingQuantity { get; set; }
        public string ReportResult { get; set; }
        public string ReportStatus { get; set; }
        public int ReportSend { get; set; }
        public string ReportStatusColor { get; set; }
        public int CombineProductId { get; set; }
        public int? CombineProductCount { get; set; }
        public bool IsParentProduct { get; set; }
        public bool IsOkToSend { get; set; }
        public bool IsReportSend { get; set; }
        public string ReportName { get; set; }
        public int? FbReportId { get; set; }
        public int? ReportRevision { get; set; }
        public int? ReportSendRevision { get; set; }
        public int? RequestedReportRevision { get; set; }
        public int? ReportVersion { get; set; }
    }

    public enum EmailSendResult
    {
        Success = 1,
        NotFound = 2,
        Failure = 3,
        RequestNotCorrectFormat = 4,
        OneRuleFound = 5,
        MoreThanOneRuleFound = 6,
        NoRuleFound = 7,
        SomeBookingDoesNotHaveRule = 8,
        EachBookingHasDifferentRule = 9
    }

    public class EmailSendBookingReportResponse
    {
        public List<EmailSend> EmailSendList { get; set; }
        public List<CommonDataSource> BookingList { get; set; }
        public List<CommonDataSource> ProductList { get; set; }
        public List<CommonDataSource> PoList { get; set; }

        public EmailSendResult Result { get; set; }

        public EmailSendBookingReportResponse()
        {
            EmailSendList = new List<EmailSend>();
        }
    }


    public class EmailSendInvoiceResponse
    {
        public List<EmailSendInvoice> EmailSendList { get; set; }
        public EmailSendResult Result { get; set; }
        public EmailSendInvoiceResponse()
        {
            EmailSendList = new List<EmailSendInvoice>();
        }
    }

    public class EmailSendInvoiceRepo
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string BillTo { get; set; }
        public string BilledName { get; set; }
        public string InvoiceType { get; set; }
        public int? InvoiceTypeId { get; set; }
        public double? InvoiceTotal { get; set; }
        public string CurrencyCode { get; set; }
        public string InvoiceFileUrl { get; set; }
        public int BookingId { get; set; }
    }

    public class EmailSendInvoice
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string BillTo { get; set; }
        public string BilledName { get; set; }
        public string InvoiceType { get; set; }
        public double? InvoiceTotal { get; set; }
        public string CurrencyCode { get; set; }
        public string InvoiceFileUrl { get; set; }
        public int? InvoiceTypeId { get; set; }
    }
    public class BookingReportRequest
    {
        public IEnumerable<int> BookingIdList { get; set; }
        public int ServiceId { get; set; }
        public int EmailSendingtype { get; set; }
        public int? InvoiceType { get; set; }
    }

    public class InvoiceSendFilesRequest
    {
        public IEnumerable<string> InvoiceNoList { get; set; }
        public int ServiceId { get; set; }
        public int EmailSendingtype { get; set; }
    }

    public class EmailRuleRequestByInvoiceNumbers
    {
        public List<string> InvoiceList { get; set; }
        public int ServiceId { get; set; }
        public int EmailSendingtype { get; set; }
        public int? InvoiceType { get; set; }
    }

    public class ReportDetailsRepo
    {
        public int BookingId { get; set; }
        public int ReportId { get; set; }     
        public int? ReporVersion { get; set; }
        public string ReportName { get; set; }
        public string ReportResult { get; set; }
        public string ReportResultColor { get; set; }
        public string ReportStatus { get; set; }
        public string ReportLink { get; set; }
        public string FinalManualReportPath { get; set; }
        public string ReportSummaryLink { get; set; }
        public string ReportImagePath { get; set; }
        public int? ReportStatusId { get; set; }
        public string SupplierName { get; set; }
        public string ServiceType { get; set; }
        public string ServiceFrom { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public int CustomerDecisionResultId { get; set; }
        public string CustomerDecisionResult { get; set; }
        public DateTime? CustomerDecisionDate { get; set; }
        public string CustomerDecisionFormatDate { get; set; }
        public string CustomerDecisionComments { get; set; }
        public string CustomerDecisionResultColor { get; set; }
        public string Observations { get; set; }
        public int? CriticalDefect { get; set; }
        public int? MajorDefect { get; set; }
        public int? MinorDefect { get; set; }
        public string MajorDefects { get; set; }
        public string MinorDefects { get; set; }
        public string CriticalDefects { get; set; }
        public string InvoiceNo { get; set; }
        public double TotalInvoiceFees { get; set; }
        public string InvoiceCurrencyCode { get; set; }
        public int? FbReportId { get; set; }
        public int? ReportRevision { get; set; }
        public int? RequestedReportRevision { get; set; }
        public int? ReportSendRevisison { get; set; }
        public int ReportSendCount { get; set; }
        public int? ReportVersion { get; set; }
    }

    public class InspectionProductRepo
    {
        public int BookingId { get; set; }
        public int ProductId { get; set; }

        public int ProductRefId { get; set; }
        public string ProductName { get; set; }
        public string Etd { get; set; }
        public string DestinationCountry { get; set; }
        public string PoNumber { get; set; }
        public int PoId { get; set; }
        public string ProductDesc { get; set; }
        public int TotalBookingQty { get; set; }
        public int? CombineProductId { get; set; }
        public int? ReportId { get; set; }
        public int? CombineAqlQuantity { get; set; }
        public List<string> PoNumberList { get; set; }
        public double? PresentedQty { get; set; }
        public double? OrderQty { get; set; }
        public double? InspectedQty { get; set; }
        public string Country { get; set; }
        public string ServiceDate { get; set; }
        public string ServiceType { get; set; }
        public List<string> ReportResultHeader { get; set; }
        public List<string> ReportResultData { get; set; }
        public int ColumnCount { get; set; }
        public string Color { get; set; }
        public string ColorCode { get; set; }
    }


    public class EmailInspectionDetail
    {
        public int InspectionID { get; set; }
        public string CustomerBookingNo { get; set; }
        public string Supplier { get; set; }
        public string Customer { get; set; }
        public string Factory { get; set; }
        public string FactoryCountry { get; set; }
        public string Office { get; set; }
        public string ServiceType { get; set; }
        public string ServiceTypeCode { get; set; }
        public string ServiceDate { get; set; }
        public List<string> SupplierContact { get; set; }
        public List<string> CustomerContact { get; set; }
        public List<string> FactoryContact { get; set; }
        public List<string> Department { get; set; }
        public List<string> DepartmentCode { get; set; }
        public List<string> Buyer { get; set; }
        public List<string> Brand { get; set; }
        public string Collection { get; set; }
        public string SupplierCode { get; set; }
        public List<string> MerchandiserContactEmailList { get; set; }
        public List<QuotationBookingContactRepo> QuotationCustomerContactEmailList { get; set; }
        public List<QuotationBookingContactRepo> QuotationSupplierContactEmailList { get; set; }
        public List<QuotationBookingContactRepo> QuotationFactoryContactEmailList { get; set; }
        public List<string> QuotationInternalContactEmailList { get; set; }
        public List<string> InvoiceContactEmailList { get; set; }
        public string Invoice { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoicePostDate { get; set; }
        public string CustomerDecisionResult { get; set; }
        public List<string> ColorCode { get; set; }
        public List<string> ColorName { get; set; }
        public int CustomerId { get; set; }
        public InvoiceSendEmailBankDetails InvoiceBank { get; set; }
    }

    public class InspectionContainerRepo
    {
        public int ContainerRefId { get; set; }
        public int BookingId { get; set; }
        public string PoNumber { get; set; }
        public string ContainerNumber { get; set; }
        public int? ContainerId { get; set; }
        public int TotalBookingQty { get; set; }
        public int? ReportId { get; set; }
        public string ReportLink { get; set; }
        public List<string> PoNumberList { get; set; }
    }

    public class BookingReportDetails
    {
        public List<InspectionContainerRepo> InspectionContainerList { get; set; }
        public List<InspectionProductRepo> InspectionProductList { get; set; }
        public List<ReportDetailsRepo> ReportDetailsList { get; set; }
    }

    public class InspectionPurchaseOrderRepo
    {
        public int BookingId { get; set; }
    }

    public class EmailSendFileDetails
    {
        public int BookingId { get; set; }
        public int? ReportId { get; set; }
        public int? InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public int FileTypeId { get; set; }
        public string FileName { get; set; }
        public string FileLink { get; set; }
        public int EmailSendFileId { get; set; }
        public string FileTypeName { get; set; }
        public string ReportName { get; set; }
    }

    public class EmailSendFileDetailsRepo
    {
        public int? BookingId { get; set; }
        public int? ReportId { get; set; }
        public int? InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public int? FileTypeId { get; set; }
        public string FileTypeName { get; set; }
        public string FileName { get; set; }
        public string FileLink { get; set; }
        public int EmailSendFileId { get; set; }
        public string ReportName { get; set; }
    }

    public class EmailSendFileListResponse
    {
        public List<EmailSendFileDetails> EmailSendFileList { get; set; }
        public EmailSendResult Result { get; set; }
    }

    public class EmailSendFileUpload
    {
        public List<int> BookingIds { get; set; }
        public List<int> ReportIds { get; set; }
        public int FileTypeId { get; set; }
        public string FileName { get; set; }
        public string FileLink { get; set; }
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string UniqueId { get; set; }
    }

    public class InvoiceSendFileUpload
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public int FileTypeId { get; set; }
        public string FileName { get; set; }
        public string FileLink { get; set; }
        public int Id { get; set; }
        public int ServiceId { get; set; }
        public string UniqueId { get; set; }
    }

    public class EmailSendFileUploadResponse
    {
        public EmailSendResult Result { get; set; }
    }

    public class EmailRuleData
    {
        public int RuleId { get; set; }
        public string ServiceTypeName { get; set; }
        public string ProductCategoryName { get; set; }
        public string ApiResultName { get; set; }
        public string FactoryCountryName { get; set; }
        public string DepartmentName { get; set; }
        public string BrandName { get; set; }
        public string CollectionName { get; set; }
        public string BuyerName { get; set; }
        public string SpecialRuleName { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public string ReportInEmail { get; set; }
        public string ReportSendType { get; set; }
    }

    public class EmailRuleResponse
    {
        public List<EmailRuleData> EmailRuleList { get; set; }
        public EmailSendResult Result { get; set; }
        public int RuleId { get; set; }
        public List<int> BookingIdsWithoutRule { get; set; }
        public List<int> BookingIdsWithRule { get; set; }
        public List<RuleBookingIds> RuleBookingList { get; set; }

        public EmailRuleResponse()
        {
            RuleBookingList = new List<RuleBookingIds>();
            BookingIdsWithoutRule = new List<int>();
            BookingIdsWithRule = new List<int>();
        }
    }

    public class RuleBookingIds
    {
        public List<int> RuleIds { get; set; }
        public int BookingId { get; set; }
    }

    //get booking data starts model

    public class BookingDetails
    {
        public int BookingId { get; set; }
        public int SupplierId { get; set; }
        public int CustomerId { get; set; }
        public int? FactoryId { get; set; }
        public int FactoryCountryId { get; set; }
        public int ReportId { get; set; }
        public List<int> BrandIdList { get; set; }
        public List<int> ContainerReportResultIds { get; set; }
        public List<int> NonContainerReportResultIds { get; set; }
        public List<int> DepartmentIdList { get; set; }
        public List<int> BuyerIdList { get; set; }
        public int? CollectionId { get; set; }
        public int? OfficeId { get; set; }
        public List<int> ServiceTypeIdList { get; set; }
        public List<int> ProductCategoryIdList { get; set; }
        public string Supplier { get; set; }
        public string Customer { get; set; }
        public string Factory { get; set; }
    }

    public class EmailSendConfigDetails
    {
        public int BookingId { get; set; }
        public int Id { get; set; }
        public int Type { get; set; }
        public int CustomerId { get; set; }
        public int ServiceId { get; set; }
        public List<int?> OfficeIds { get; set; }
        public List<int> DepartmentIds { get; set; }
        public List<int> BrandIds { get; set; }
        public List<int> CollectionIds { get; set; }
        public List<int> BuyerIds { get; set; }
        public List<int> ServiceTypeIds { get; set; }
        public List<int> ProductCategoryIds { get; set; }
        public List<int?> CustomerResultIds { get; set; }
        public List<int> SupplierOrFactoryIds { get; set; }
        public List<int> FactoryCountryIds { get; set; }
        public List<int> CustomerContactIds { get; set; }
        public List<int> ApiContactIds { get; set; }
        public List<int> ApiDefaultContactIds { get; set; }

        public List<string> ServiceTypeNameList { get; set; }
        public List<string> ProductCategoryNameList { get; set; }
        public List<string> FactoryCountryNameList { get; set; }
        public List<string> DepartmentNameList { get; set; }
        public List<string> BrandNameList { get; set; }
        public List<string> CollectionNameList { get; set; }
        public List<string> BuyerNameList { get; set; }
        public List<string> ApiResultNameList { get; set; }
        public List<int> ApiResultIdList { get; set; }
        public List<string> SpecialRuleNameList { get; set; }
        public List<string> SupplierNameList { get; set; }
        public List<string> FactoryNameList { get; set; }
        public string ReportInEmail { get; set; }
        public string ReportSendType { get; set; }
    }

    public class EmailSendConfigBooking
    {
        public int BookingId { get; set; }
        public List<EmailSendConfigDetails> EmailSendConfigList { get; set; }
    }

    public class BookingReportMap
    {
        public int InspectionId { get; set; }
        public int? ReportId { get; set; }
        public int? ReportRevision { get; set; }
        public int? ReportVersion { get; set; }
    }

    public class EmailSendHistoryRepo
    {
        public string EmailSentBy { get; set; }
        public DateTime? EmailSentOn { get; set; }
        public int EmailStatus { get; set; }
    }

    public class EmailSendHistory
    {
        public string EmailSentBy { get; set; }
        public string EmailSentOn { get; set; }
        public string EmailStatus { get; set; }
    }

    public class EmailSendHistoryResponse
    {
        public List<EmailSendHistory> EmailSendHistoryList { get; set; }
        public EmailSendHistoryResult Result { get; set; }
    }

    public enum EmailSendHistoryResult
    {
        Success = 1,
        NotFound = 2
    }

    public class FbReportRevisionNoResponse
    {
        public FbReportRevisionNoRequestResult Result { get; set; }
        public int ReportId { get; set; }
        public int RevisionId { get; set; }

    }

    public enum FbReportRevisionNoRequestResult
    {
        Success = 1,
        NotFound = 2,
        Failed = 3
    }

    public class EmailRuleDataRepo
    {
        public int RuleId { get; set; }
        public int? CustomerId { get; set; }
        public int? ReportSendType { get; set; }
        public int? EmailSendType { get; set; }
        public bool? IsIncludeCc { get; set; }
        public bool? IsIncludeFc { get; set; }
        public bool? IsIncludeSc { get; set; }
        public int? ReportInEmail { get; set; }
        public double? EmailSize { get; set; }
        public int? NoOfReports { get; set; }
        public string RecipientName { get; set; }
        public string SubjectDelimeterName { get; set; }
        public string FileDelimeterName { get; set; }

        public int? EmailSubjectId { get; set; }
        public int? EmailFileId { get; set; }
        public List<int?> OfficeIdList { get; set; }
        public List<string> CustomerContactEmailList { get; set; }
        public List<string> ApiContactEmailList { get; set; }
        public List<int?> CustomerDecisionResults { get; set; }
        public bool? IsIncludeMc { get; set; }
        public bool? IsPictureFileInEmail { get; set; }
        public int? InvoiceType { get; set; }
    }

    public class EmailRuleTemplateDetailsRepo
    {
        public int RuleId { get; set; }
        public int FieldId { get; set; }
        public string FieldName { get; set; }
        public bool? FieldIsText { get; set; }
        public int? DateFormatId { get; set; }
        public bool? IsDateSeparator { get; set; }
        public string DateFormat { get; set; }
        public int? MaxItem { get; set; }
        public int? MaxChar { get; set; }
        public bool? IsTitle { get; set; }
        public string TitleCustomName { get; set; }
    }

    public class EmailSendConfigBaseDetails
    {
        public int BookingId { get; set; }
        public int Id { get; set; }
        public int Type { get; set; }
        public int CustomerId { get; set; }
        public int ServiceId { get; set; }
        public string ReportInEmail { get; set; }
        public string ReportSendType { get; set; }
        public int ReportSendTypeId { get; set; }
    }

    public class EmailSendCustomerConfigDetails
    {
        public int EsDetailsId { get; set; }
        public int? BrandId { get; set; }
        public int? DepartmentId { get; set; }
        public int? BuyerId { get; set; }
        public int? CollectionId { get; set; }
        public string BrandName { get; set; }
        public string DepartmentName { get; set; }
        public string BuyerName { get; set; }
        public string CollectionName { get; set; }
    }

    public class EmailSendCustomerContactDetails
    {
        public int CustomerContactId { get; set; }
        public string CustomerContactEmail { get; set; }
        public int ESDetailId { get; set; }
    }

    public class EmailSendFactoryCountryDetails
    {
        public int FactoryCountryId { get; set; }
        public string FactoryCountryName { get; set; }
        public int ESDetailId { get; set; }
    }

    public class EmailSendServiceTypeDetails
    {
        public int ServiceTypeId { get; set; }
        public string ServiceTypeName { get; set; }
        public int ESDetailId { get; set; }
    }

    public class EmailSendSupplierFactoryDetails
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int SupplierType { get; set; }
        public int ESDetailId { get; set; }
    }

    public class EmailSendOfficeDetails
    {
        public int? OfficeId { get; set; }
        public int ESDetailId { get; set; }
    }

    public class EmailSendResultDetails
    {
        public int? ApiResultId { get; set; }
        public string ApiResultName { get; set; }
        public int? CustomerResultId { get; set; }
        public int ESDetailId { get; set; }
    }

    public class ESProductCategoryDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ESDetailId { get; set; }
    }

    public class ESSpecialRuleDetails
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? ESDetailId { get; set; }
    }

    public class ESApiContacts
    {
        public int ApiContactId { get; set; }
        public string CompanyEmail { get; set; }
        public int? EsDetailId { get; set; }
    }

    public class ESToCCDetails
    {
        public int? RecipientId { get; set; }
        public bool? IsToValue { get; set; }
        public bool? IsCCValue { get; set; }
        public int? ESDetailId { get; set; }
    }

    //get booking data ends model
    public class DefectData
    {
        public int BookingId { get; set; }
        public int ReportId { get; set; }
        public int DefectId { get; set; }
        public string DefectDesc { get; set; }
        public int CriticalDefect { get; set; }
        public int MajorDefect { get; set; }
        public int MinorDefect { get; set; }
    }

    public class AutoCustomerDecisionRequest
    {
        public int CustomerId { get; set; }
        public List<int> ReportIdList { get; set; }
        public List<int> BookingIdList { get; set; }
    }

    public class AutoCustomerDecisionResponse
    {
        public List<AutoCustomerDecision> AutoCustomerDecisionList { get; set; }
        public AutoCustomerDecisionResult Result { get; set; }
    }

    public class AutoCustomerDecision
    {
        public int BookingId { get; set; }
        public List<int> ReportIdList { get; set; }
        public string Comments { get; set; }
    }

    public enum AutoCustomerDecisionResult
    {
        Success = 1,
        RequestShouldNotBeEmpty = 2,
        DataNotFound = 3,
        Failed = 4
    }
}
