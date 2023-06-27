using DTO.Common;
using DTO.Inspection;
using DTO.User;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.EmailSend
{
    public class EmailSendSummaryItem
    {
        public int BookingId { get; set; }
        public string CustomerBookingNo { get; set; }
        public int ReportCount { get; set; }
        public int CustomerId { get; set; }
        public int SuccessReportCount { get; set; }
        public string CustomerName { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int FactoryId { get; set; }
        public string FactoryName { get; set; }
        public string ServiceDate { get; set; }
        public string ServiceType { get; set; }
        public string Office { get; set; }
        public int StatusId { get; set; }
        public string ServiceFromDate { get; set; }
        public string ServiceToDate { get; set; }
    }

    public class EmailSendSummaryResponse
    {
        public List<EmailSendSummaryItem> Data { get; set; }
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public IEnumerable<InspectionStatus> InspectionStatuslst { get; set; }
        public EmailSendSummaryResponseResult Result { get; set; }
    }

    public class AeListResponse
    {
        public List<AEDetails> Data { get; set; }
        public EmailSendSummaryResponseResult Result { get; set; }
    }

    public enum EmailSendSummaryResponseResult
    {
        Success = 1,
        NotFound = 2,
        Fail = 3,
        RequestNotCorrectFormat = 4
    }

    public class EmailSendSummaryRequest
    {
        public int SearchTypeId { get; set; }

        public string SearchTypeText { get; set; }

        public int? CustomerId { get; set; }

        public int? SupplierId { get; set; }

        public IEnumerable<int> FactoryIdlst { get; set; }

        public IEnumerable<int> StatusIdlst { get; set; }

        public int DateTypeid { get; set; }

        public DateObject FromDate { get; set; }

        public DateObject ToDate { get; set; }

        public IEnumerable<int?> Officeidlst { get; set; }

        public int? Index { get; set; }

        public int? pageSize { get; set; }

        public IEnumerable<int> ServiceTypelst { get; set; }

        public IEnumerable<int> UserIdList { get; set; }

        public IEnumerable<int> SelectedCountryIdList { get; set; }

        public IEnumerable<int> SelectedBrandIdList { get; set; }

        public IEnumerable<int> SelectedDeptIdList { get; set; }

        public IEnumerable<int?> SelectedCollectionIdList { get; set; }

        public IEnumerable<int> SelectedBuyerIdList { get; set; }

        public IEnumerable<int> SelectedAPIResultIdList { get; set; }

        public int ServiceId { get; set; }
    }

    public class ReportSendTypeResponse
    {
        public bool RuleFound { get; set; }
        public bool SendMultipleEmail { get; set; }
    }

    public class ReportEmailSendType
    {
        public int? ReportSendType { get; set; }
        public int CustomerId { get; set; }
        public int ServiceId { get; set; }
    }

    public class InvoiceBookingEmailSend
    {
        public int? bookingId { get; set; }
        public string InvoiceNo { get; set; }
    }
    // email preview model definition

    public class EmailPreviewRequest
    {
        public int EmailRuleId { get; set; }
        public List<EmailReportPreviewDetail> EmailReportPreviewData { get; set; }
        public List<EmailReportPreviewAttachment> EmailReportAttachment { get; set; }
        public int EsTypeId { get; set; }
        public int? InvoiceType { get; set; }
    }

    public class EmailReportPreviewDetail
    {
        public int BookingId { get; set; }
        public int ReportId { get; set; }
        public string ExtraFileName { get; set; }
        public string InvoiceNo { get; set; }
        public int? ReportVersion { get; set; }
        public int? ReportRevision { get; set; }
    }

    public class EmailReportPreviewAttachment
    {
        public int? BookingId { get; set; }
        public string InvoiceNo { get; set; }
        public int? ReportId { get; set; }
        public string FileType { get; set; }
        public string FileLink { get; set; }
        public string FileName { get; set; }
    }


    //email preview response model definition
    public class EmailPreviewResponse
    {
        public List<string> EmailToList { get; set; }
        public List<string> EmailCCList { get; set; }
        public List<string> EmailBCCList { get; set; }
        public int RuleId { get; set; }
        public int EmailValidOption { get; set; }
        public List<EmailBody> EmailBodyTempList { get; set; }
        public EmailPreviewResponseResult Result { get; set; }
    }




    public class EmailPreviewBody
    {
        public string EmailBody { get; set; }
        public string ReportId { get; set; }
        public string BookingId { get; set; }
        public List<string> ReportLinks { get; set; }
    }

    public class EmailBody
    {
        public EmailBody()
        {
            this.ProductList = new List<InspectionProductRepo>();
            this.ContainerList = new List<InspectionContainerRepo>();
            this.ReportList = new List<ReportDetailsRepo>();
        }
        public int EmailId { get; set; }
        public string RecipientName { get; set; }
        public string EmailCount { get; set; }
        public string EmailSubject { get; set; }
        public int EmailValidOption { get; set; }
        public EmailRuleDataRepo EmailRuleData { get; set; }
        public List<InspectionProductRepo> ProductList { get; set; }
        public List<InspectionContainerRepo> ContainerList { get; set; }
        public List<ReportDetailsRepo> ReportList { get; set; }
        public List<EmailAttachments> AttachmentList { get; set; }
        public List<EmailAttachments> UserUploadFileList { get; set; }
        public List<ReportBooking> ReportBookingList { get; set; }
        public List<ReportSummaryLink> ReportSummaryLinkList { get; set; }
        public string SenderEmail { get; set; }
        public string Supplier { get; set; }
        public string Customer { get; set; }
        public string Factory { get; set; }
        public string UpdatedDate { get; set; }
        public string UpdatedTime { get; set; }
        public List<CusotmerDecisionDataList> CusotmerDecisionDataList { get; set; }
        public int ColumnCount { get; set; }

        public List<string> ReportHeader { get; set; }

        // Added for invoice status later we wil make this for common
        public string EntityName { get; set; }
        public string InvoiceNumber { get; set; }
        public string PreInvoiceEmailContent1 { get; set; }
        public string QuotationInternalContact { get; set; }
        public string PreInvoiceEmailContent2 { get; set; }

        public string CustomerDecisionUrl { get; set; }
        public int CustomerId { get; set; }
        public InvoiceSendEmailBankDetails InvoiceBank { get; set; }
    }

    public class InvoiceSendEmailBankDetails
    {
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string BankName { get; set; }
        public string SwiftCode { get; set; }
        public string BankAddress { get; set; }
    }

    public class EmailPreviewDataResponse
    {
        public List<string> EmailToList { get; set; }
        public List<string> EmailCCList { get; set; }
        public List<string> EmailBCCList { get; set; }
        public string EmailSubject { get; set; }
        public int RuleId { get; set; }
        public int EmailId { get; set; }
        public string EmailBody { get; set; }
        public string InvoiceNo { get; set; }
        public int EmailValidOption { get; set; }
        public List<EmailAttachments> AttachmentList { get; set; }
        public List<ReportBooking> ReportBookingList { get; set; }
        public int CustomerId { get; set; }
    }

    public class EmailDataResponse
    {
        public List<EmailPreviewDataResponse> Data { get; set; }
        public EmailPreviewResponseResult Result { get; set; }
    }

    public class EmailAttachments
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileLink { get; set; }
    }


    public enum InvoiceFileType
    {
        Invoice = 1,
        ManualInvoice = 2,
        CreditNote = 3
    }

    public class ReportSummaryLink
    {
        public int BookingId { get; set; }
        public string SummaryLink { get; set; }
    }

    public class CommonReportLinkData
    {
        public string Id { get; set; }
        public string ReportLink { get; set; }
    }

    public class ReportBooking
    {
        public int ReportId { get; set; }
        public string ReportName { get; set; }
        public int? ReportVersion { get; set; }
        public int? ReportRevision { get; set; }
        public int InspectionId { get; set; }
        public int AuditId { get; set; }
        public int EsTypeId { get; set; }
    }

    public enum EmailPreviewResponseResult
    {
        success = 1,
        failure = 2,
        emailrulenotvalid = 3,
        inspectionsummarylinknotavailable = 4,
        multipleRuleFound = 5
    }

    public class CusotmerDecisionEmail
    {
        public string EntityName { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public string FactoryName { get; set; }
        public string UpdatedDate { get; set; }
        public string UpdatedTime { get; set; }
        public List<CusotmerDecisionDataList> cusotmerDecisionDataList { get; set; }

    }

    public class CusotmerDecisionDataList
    {
        public string Country { get; set; }

        public string InspectionDate { get; set; }
        public int Order { get; set; }
        public string CusotmerPO { get; set; }
        public int Style { get; set; }
        public string Color { get; set; }
        public string ColorCode { get; set; }
        public string ServiceType { get; set; }
        public List<string> ReportResultHeader { get; set; }

        public List<string> ReportResultData { get; set; }

        public string FinalResult { get; set; }
        public string ResultColor { get; set; }
        public string FinalDecision { get; set; }
        public string Comments { get; set; }
        public string DateReceived { get; set; }
        public int PresentedQty { get; set; }
        public int OrderQty { get; set; }
        public int InspectedQty { get; set; }
        public int DefectId { get; set; }
        public string DefectDesc { get; set; }
        public int CriticalDefect { get; set; }
        public int MajorDefect { get; set; }
        public int MinorDefect { get; set; }
        public string Observation { get; set; }
        public string MajorDefects { get; set; }
        public string MinorDefects { get; set; }
        public string CriticalDefects { get; set; }

    }
}
