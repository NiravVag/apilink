using DTO.CommonClass;
using DTO.FullBridge;
using DTO.Inspection;
using DTO.Quotation;
using DTO.RepoRequest.Audit;
using DTO.Report;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IAuditRepository : IRepository
    {
        /// <summary>
        /// Get Evaluation Round
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>AudEvaluationRound</returns>
        Task<List<AudEvaluationRound>> GetEvaluationRound();

        /// <summary>
        /// Save Audit
        /// </summary>
        /// <param name="CuCustomer"></param>
        /// <param name=""></param>
        /// <returns>int</returns>
        Task<int> AddAudit(AudTransaction entity);


        /// <summary>
        /// Update Audit
        /// </summary>
        /// <param name="CuCustomer"></param>
        /// <param name=""></param>
        /// <returns>int</returns>
        Task<int> UpdateAudit(AudTransaction entity);

        /// <summary>
        /// Get Audit Details By Id
        /// </summary>
        /// <param name=""></param>
        /// <returns>int</returns>
        Task<AudTransaction> GetAuditDetailsById(int id);

        /// <summary>
        /// Get audit upload documentss 
        /// </summary>
        /// <param name=""></param>
        /// <returns>int</returns>
        Task<IEnumerable<AudTranFileAttachment>> GetReceptFiles(int auditid, IEnumerable<string> GuidList);

        /// <summary>
        /// Download the document
        /// </summary>
        /// <param name=""></param>
        /// <returns>int</returns>
        Task<AudTranFileAttachment> GetFile(int id);

        /// <summary>
        /// Get Booking rule
        /// </summary>
        /// <param name=""></param>
        /// <returns>int</returns>
        Task<List<AudBookingRule>> GetAuditBookingRule();

        /// <summary>
        /// Get Last Report No
        /// </summary>
        /// <param name=""></param>
        /// <returns>int</returns>
        Task<AudTransaction> GetCusLastReportNo(int customerid);

        /// <summary>
        /// Check Report NO exists or Not
        /// </summary>
        /// <param name=""></param>
        /// <returns>int</returns>
        Task<bool> IsReportNoExists(string reportno, int customerid);

        /// <summary>
        /// Get Booking Contacts
        /// </summary>
        /// <param name=""></param>
        /// <returns>list</returns>
        Task<List<AudBookingContact>> GetAuditBookingContacts(int OfficeId);


        /// <summary>
        /// Get Audit status
        /// </summary>
        /// <param name=""></param>
        /// <returns>list</returns>
        Task<List<AudStatus>> GetAuditStatus();

        /// <summary>
        /// Search Audit Summary
        /// </summary>
        /// <param name=""></param>
        /// <returns>list</returns>
        //Task<AuditSummaryRepoResponse> SearchAuditSummary(AuditSummaryRepoRequest request);

        /// <summary>
        /// Get Audit Cancel Details
        /// </summary>
        /// <param name=""></param>
        /// <returns>list</returns>
        Task<AudTransaction> GetAuditCancelDetails(int auditid);

        /// <summary>
        /// Get Audit cancel/re schedule reason
        /// </summary>
        /// <param name=""></param>
        /// <returns>list</returns>
        Task<List<AudCancelRescheduleReason>> GetAuditCancelRescheduleReasons(int? customerid, int optypeid);

        /// <summary>
        /// Get Audit Type
        /// </summary>
        /// <param name=""></param>
        /// <returns>list</returns>
        Task<List<AudType>> GetAuditType();

        /// <summary>
        /// Get Audit Work Process
        /// </summary>
        /// <param name=""></param>
        /// <returns>list</returns>
        Task<List<AudWorkProcess>> GetAuditWorkProcess();

        /// <summary>
        /// Get Audit Details For Report
        /// </summary>
        /// <param name=""></param>
        /// <returns>list</returns>
        Task<AudTransaction> GetAuditBasicDetails(int auditid);

        /// <summary>
        /// Get Scheduled Auditors
        /// </summary>
        /// <param name=""></param>
        /// <returns>list</returns>
        Task<List<AudTranAuditor>> GetScheduledAuditors(int auditid);

        /// <summary>
        /// Get Audit Report Details
        /// </summary>
        /// <param name=""></param>
        /// <returns>list</returns>
        Task<AudTransaction> GetAuditReportDetails(int auditid);

        /// <summary>
        /// Get Audit Report 
        /// </summary>
        /// <param name=""></param>
        /// <returns>report</returns>
        Task<AudTranReport1> GetAuditReport(int id);

        /// <summary>
        /// Get uploaded Audit Report 
        /// </summary>
        /// <param name=""></param>
        /// <returns>report</returns>
        Task<IEnumerable<AudTranReport1>> GetReportFiles(int auditid, IEnumerable<Guid> GuidList);

        /// <summary>
        /// Get Audit Details By List Id
        /// </summary>
        /// <param name=""></param>
        /// <returns>List<AudTransaction></returns>
        Task<List<AudTransaction>> GetAuditDetailsByListId(IEnumerable<int> ListAuditid);

        /// Get booking from and to date
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns>from and to date from booking table</returns>
        Task<BookingDate> getAuditBookingDateDetails(int bookingId);

        /// Get booking from and to date
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns> list of from and to date from booking table</returns>
        Task<List<BookingDate>> getListAuditBookingDateDetails(List<int> lstbookingId);

        Task<List<AuditServiceTypeRepoResponse>> GetServiceTypeDataByAudit(IQueryable<int> auditIds);

        Task<List<AuditAuditorRepoResponse>> GetAuditorDataByAudit(IQueryable<int> auditIds);

        Task<List<AuditFactoryCountryRepoResponse>> GetFactoryCountryDataByAudit(IQueryable<int> auditIds);

        Task<List<AuditQuotationRepoResponse>> GetQuotationDataByAudit(IQueryable<int> auditIds);

        IQueryable<AudTransaction> GetAuditMainData();
        Task<BookingDataRepo> GetAuditDetails(int AuditId);
        Task<List<AuditServiceTypeRepoResponse>> GetServiceTypeDataByAudit(int auditId);
        Task<List<AuditFactoryCountryRepoResponse>> GetFactoryCountryDataByAudit(int auditId);
        IQueryable<int> GetAuditNo();
        Task<List<AuditServiceTypeData>> GetAuditServiceTypeList(List<int> auditIds);
        Task<List<AuditCustomerContactData>> GetAuditContacts(List<int> auditIds);
        Task<List<AuditSupplierCustomerRepoResponse>> GetSupplierCustomerDataByAudit(IEnumerable<int> auditIds);
        Task<List<AuditFactoryCustomerRepoResponse>> GetFactoryCustomerDataByAudit(IEnumerable<int> auditIds);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="auditIds"></param>
        /// <returns></returns>
        Task<List<AudTranFaProfile>> GetAuditTranFaProfiles(List<int> auditIds);
        Task<IEnumerable<AuditCusFactDetails>> GetAuditDetails(IEnumerable<int> auditIds);
        Task<List<FactoryCountry>> GetFactoryAddressDetailsByAuditIds(IEnumerable<int> auditIds);
        Task<AudTransaction> GetAuditData(int auditid);
        Task<FbAuditData> GetAuditForFbReportDetails(int auditId);
        Task<List<AuditCustomerContactData>> GetAuditCustomerContacts(int auditId);
        Task<List<AuditCustomerContactData>> GetAuditSupplierContacts(int auditId);
        Task<List<AudTranAuditor>> GetAuditorDetails(int auditid);
        Task<List<AudTranC>> GetAuditCSDetails(int auditid);
        Task<List<AudTranFileAttachment>> GetAuditTranFiles(int auditid);
        IQueryable<AudCuProductCategory> GetProductCategory();
        Task<List<AudFbReportCheckpoint>> GetAuditFbReportCheckpointByAuditIds(IQueryable<int> auditIds);
        Task<InspectionBookingDetail> GetAuditBookingDetails(int bookingId);
        Task<ItUserMaster> GetUserName(int userId);
        Task<List<BookingServiceType>> GetAuditBookingServiceTypes(IEnumerable<int> bookingIds);
        Task<List<InspectionSupplierFactoryContacts>> GetFactoryContactsByBookingIds(List<int> bookingIds);
        Task<List<InspectionSupplierFactoryContacts>> GetSupplierContactsByBookingIds(List<int> bookingIds);
        Task<List<BookingFileAttachment>> GetAuditBookingMappedFiles(IEnumerable<int> bookingId);
        Task<AudBookingEmailConfiguration> GetCCEmailConfigurationEmailsByCustomer(int customerId, int? factoryCountryId, int bookingStatusId);
        Task<IEnumerable<MidTask>> GetTask(int bookingId, IEnumerable<int> typeIdList, bool isdone);
        Task<AudTransaction> GetAuditInspectionCustomerContactByID(int inspectionID);
        Task<CuCheckPoint> GetAuditServiceCustomersByCustomerId(int customerId);
        Task<List<CommonDataSource>> GetAuditCSByAuditIds(IEnumerable<int> auditids);
        Task<IEnumerable<ServiceTypeList>> GetServiceType(IEnumerable<int> bookingId);
        Task<IEnumerable<EaqfInvoiceDetails>> GetAuditEaqfBookingInvoiceDetails(List<int> bookingIds);
        Task<IEnumerable<EaqfAuditReportDetails>> GetAuditEaqfBookingReportDetails(List<int> bookingIds);
    }
}
