using DTO.CommonClass;
using DTO.EmailSend;
using DTO.Inspection;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IEmailSendRepository : IRepository
    {
        Task<List<InspectionContainerRepo>> GetContainerDetails(IEnumerable<int> bookingIdList);

        Task<List<InspectionProductRepo>> GetProductDetails(IEnumerable<int> bookingIdList);

        Task<List<ReportDetailsRepo>> GetReportDetails(IEnumerable<int> reportIdList);

        Task<EsTranFile> GetEmailSendData(int emailSendFileId);

        Task<IEnumerable<CommonDataSource>> GetFileTypeList();

        Task<IEnumerable<EmailSendFileDetailsRepo>> GetEmailFileList(BookingReportRequest request);
        Task<List<ReportEmailSendType>> GetEmailDataByCustomer(int customerId, int serviceId);

        Task<List<BookingDetails>> GetBookingDetails(IEnumerable<int> bookingIds);

        Task<List<EmailSendConfigBaseDetails>> GetEmailConfigurationBaseDetails(int typeId, List<int> customerList, int serviceId, int? invoiceType = null, bool isAnyCustomerPreInvoiceRuleConfigured = false);
        Task<List<EmailSendCustomerConfigDetails>> GetESCustomerConfigDetails(List<int> esDetailsId);
        Task<List<EmailSendCustomerContactDetails>> GetESCustomerContactDetails(List<int> esDetailsId);
        Task<List<EmailSendServiceTypeDetails>> GetESServiceTypeDetails(List<int> esDetailsId);
        Task<List<EmailSendFactoryCountryDetails>> GetESFactoryCountryDetails(List<int> esDetailsId);
        Task<List<EmailSendSupplierFactoryDetails>> GetESSupplierOrFactoryDetails(List<int> esDetailsId);
        Task<List<EmailSendOfficeDetails>> GetESOfficeDetails(List<int> esDetailsId);
        Task<List<ESApiContacts>> GetESApiContactDetails(List<int> esDetailsId);
        Task<List<EmailSendResultDetails>> GetESReportResultDetails(List<int> esDetailsId);
        Task<List<ESProductCategoryDetails>> GetESProductCategoryDetails(List<int> esDetailsId);
        Task<List<ESSpecialRuleDetails>> GetESSpecialRuleDetails(List<int> esDetailsId);
        Task<List<ESToCCDetails>> GetESRecipientDetails(List<int> esDetailsId);
        //Task<EsDetail> GetEmailRuleDetails(int emailRuleId);
        Task<List<EsRecipientType>> GetEmailRecipientDetails(int emailRuleId);
        Task<List<string>> GetAPIDefaultContacts(List<int?> officeIds);
        Task<List<EmailInspectionDetail>> GetEmailInspectionDetails(IEnumerable<int> bookingIdList);
        Task<List<InspectionProductRepo>> GetNonContainerProductDetails(IEnumerable<int> bookingIdList);

        IQueryable<LogBookingReportEmailQueueData> GetLogBookingReportEmailQueues();

        Task<List<LogBookingReportEmailQueueData>> GetLogBookingReportEmailQueues(List<int> bookingIds);

        Task<List<LogEmailQueues>> GetLogEmailQueues(List<int?> emailLogIds);

        Task<List<ReportDetailsRepo>> GetContainerFbReportDetails(IEnumerable<int> reportIdList);

        Task<List<BookingReportMap>> GetContainerBookingReportMaps(List<int> bookingIds, int entityId);

        Task<List<BookingReportMap>> GetNonContainerBookingReportMaps(List<int> bookingIds, int entityId);

        Task<List<BookingReportMap>> GetEmailBookingReportMaps(List<int> bookingIds);

        Task<List<EmailSendHistoryRepo>> GetEmailSendHistory(int inspectionId, int reportId, int EmailTypeId);

        IQueryable<EmailRuleDataRepo> GetEmailRuleData(int emailRuleId);
        Task<List<EmailRuleTemplateDetailsRepo>> GetEmailRuleSubjectTemplateData(int? EmailSubjectId);
        Task<List<EmailRuleTemplateDetailsRepo>> GetEmailRuleFileTemplateData(int? EmailFileId);
        Task<List<DefectData>> GetDefectData(int bookingId);
        Task<List<InspPurchaseOrderTransaction>> GetPoTransactionbyBooking(int bookingId);
        Task<List<InspectionReportSummary>> GetInspectionSummaryData(int bookingId);
        Task<int> AddInvoiceEmailAttachment(InvTranFile entity);
        Task<List<InvTranFile>> GetInvoiceEmailAttachment(int invoiceId);
        Task<IEnumerable<int>> GetBookingNumbersbyInvoiceList(List<string> InvoiceList);
        Task<List<EmailSendInvoiceRepo>> GetEmailSendInvoiceList(List<string> invoiceList);
        Task<IEnumerable<CommonDataSource>> GetInvoiceFileTypeList();
        Task<IEnumerable<EmailSendFileDetailsRepo>> GetInvoiceSendFileList(InvoiceSendFilesRequest request);
        Task<InvTranFile> GetInvoiceSendData(int emailSendFileId);
        Task<IEnumerable<InvoiceBookingEmailSend>> GetBookingDataByInvoiceList(List<string> InvoiceList);
        Task<List<ReportDetailsRepo>> GetInvoiceReportDetails(List<string> InvoiceNoList);

        Task<FbReportDetail> GetFbReportInfo(int apiReportId);

        Task<bool> IsAnyCustomerPreInvoiceRuleConfigured(int typeId, List<int> customerList, int serviceId, int? invoiceType = null);
    }
}
