using DTO.CommonClass;
using DTO.EmailSend;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IEmailSendManager
    {
        Task<DataSourceResponse> GetBookingStatusList();

        Task<AeListResponse> GetAeList();

        Task<EmailSendSummaryResponse> GetEmailSendSummary(EmailSendSummaryRequest request);

        Task<EmailSendBookingReportResponse> GetBookingReportDetails(BookingReportRequest request);

        Task<DeleteResponse> Delete(int Id);

        Task<DataSourceResponse> GetFileTypeList();

        Task<EmailSendFileListResponse> GetEmailSendFileDetails(BookingReportRequest request);

        Task<EmailSendFileUploadResponse> Save(EmailSendFileUpload model);

        Task<ReportSendTypeResponse> ValidateMultipleEmailSendByCustomer(int customerId, int serviceId);

        Task<EmailRuleResponse> GetEmailRuleData(BookingReportRequest request);

        Task<EmailPreviewResponse> FetchEmaildetailsbyEmailRule(EmailPreviewRequest request);

        Task<bool> UpdateBookingStatusbyReportSent(List<int> bookingIds, int entityId, int createdBy);

        Task<EmailSendHistoryResponse> GetEmailSendHistory(int inspectionId, int reportId, int EmailTypeId);

        EmailSendConfigBooking GetEmailConfigurationListByBookingData(BookingDetails bookingList, List<EmailSendConfigDetails> emailConfigurationList, int customerResultId);

        EmailRuleResponse GetEmailRuleData(List<EmailSendConfigBooking> emailConfigList, List<int> bookingIdList);

        List<EmailSendConfigDetails> GetEmailConfigurationList(int customerId, List<EmailSendConfigBaseDetails> emailConfigBaseList, List<EmailSendCustomerConfigDetails> emailCustomerConfigDetails, List<EmailSendCustomerContactDetails> emailCustomerContactDetails,
                    List<EmailSendServiceTypeDetails> emailServiceTypeDetails, List<EmailSendFactoryCountryDetails> emailFactoryCountryDetails, List<EmailSendSupplierFactoryDetails> esSupplierFactoryDetails,
                    List<EmailSendOfficeDetails> esOfficeDetails, List<ESApiContacts> esApiContacts, List<EmailSendResultDetails> esReportResultDetails, List<ESProductCategoryDetails> esProductCategoryDetails,
                    List<ESSpecialRuleDetails> esSpecialRuleDetails, int emailTypeId, int? invoiceType = null, bool isAnyCustomerRuleConfigured = false);
        Task<int> AddOrUpdatedEmailInvoiceAttachment(InvoiceEmailAttachment invoiceEmailAttachmentRequest);
        Task<IEnumerable<int>> GetBookingNumbersByInvoice(List<string> InvoiceList);
        Task<EmailSendInvoiceResponse> GetBookingInvoiceDetails(EmailRuleRequestByInvoiceNumbers request);
        Task<DataSourceResponse> GetInvoiceFileTypeList();
        Task<EmailSendFileUploadResponse> SaveInvoiceAttachments(InvoiceSendFileUpload model);
        Task<EmailSendFileListResponse> GetInvoiceSendFileDetails(InvoiceSendFilesRequest request);
        Task<DeleteResponse> DeleteInvoiceFile(int Id);
        Task<IEnumerable<InvoiceBookingEmailSend>> GetBookingDataByInvoiceNoList(List<string> InvoiceList);
        Task<FbReportRevisionNoResponse> SetFbReportVersion(int apiReportId, int fbReportId, int requestVersion, string fbToken);

        Task<AutoCustomerDecisionResponse> AutoCustomerDecisionList(AutoCustomerDecisionRequest request);
        Task<bool> UpdateInvoiceStatusbyInvoiceSend(List<int> bookingIds, int entityId, int createdBy);
        Task<bool> CheckFbReportIsInvalidated(int fbReportId, string fbToken);
    }
}
