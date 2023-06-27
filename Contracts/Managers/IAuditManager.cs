using DTO.References;
using DTO.Audit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DTO.File;
using DTO.Location;
using DTO.OfficeLocation;
using DTO.Inspection;
using DTO.RepoRequest.Audit;
using System.Linq;
using Entities;
using DTO.Eaqf;

namespace Contracts.Managers
{
    public interface IAuditManager
    {
        /// <summary>
        /// Get Evaluation Round
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>EvaluationRound</returns>
        Task<AuditEvaluationRoundResponse> GetEvaluationRound();

        /// <summary>
        /// Edit Audit
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>EditAuditResponse/returns>
        Task<EditAuditResponse> EditAudit(int? id);

        /// <summary>
        /// Get Audit Details By Customer Id
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>EditAuditResponse/returns>
        Task<EditAuidtCusDetails> GetAuditDetailsByCustomerId(int id);

        /// <summary>
        /// Get Supplier Details By Customer Id and supplier Id
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>EditAuditResponse/returns>
        Task<EditAuditSupDetails> GetSupplierDetailsByCustomerIdSupplierId(int? cusid, int supid);

        /// <summary>
        /// Get factory Details By Customer Id and supplier Id
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>EditAuditResponse/returns>
        Task<EditAuditFactDetails> GetFactoryDetailsByCustomerIdFactoryId(int? cusid, int factid);

        /// <summary>
        /// Save Audit Details
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>EditAuditResponse/returns>
        Task<SaveAuditResponse> SaveAudit(AuditDetails request);

        /// <summary>
        /// Upload files
        /// </summary>
        /// <param name="fileList"></param>
        /// <returns></returns>
        //Task UploadFiles(int auditid, Dictionary<Guid, byte[]> fileList);

        /// <summary>
        /// Download files
        /// </summary>
        /// <param name="fileList"></param>
        /// <returns></returns>
        //Task<FileResponse> GetFile(int id);

        /// <summary>
        /// Get Booking Rule
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>ProductionStatus</returns>
        Task<AuditBookingRuleResponse> GetAuditBookingRules(int customerid, int factid);

        /// <summary>
        /// Get Booking Rule
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>ProductionStatus</returns>
        Task<AuditCSResponse> GetAuditCS(int factid, int? cusid);

        /// <summary>
        /// Get Booking Contact Information
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>ProductionStatus</returns>
        Task<AuditBookingContactResponse> GetAuditBookingContactInformation(int factid);

        /// <summary>
        /// Get Auidt Summary
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>AuditSummaryResponse</returns>
        Task<AuditSummaryResponse> GetAuditSummary();

        /// <summary>
        /// Search Auidt Summary
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>AuditSummaryResponse</returns>
        Task<AuditSummarySearchResponse> SearchAuditSummary(AuditSummarySearchRequest request);

        /// <summary>
        /// Get Cancel Auidt Details
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>AuditSummaryResponse</returns>
        Task<EditCancelRescheduleAuditResponse> GetAuditCancelDetails(int id, int optypeid);

        /// <summary>
        /// Get Cancel / Postpone Auidt Reasons
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>AuditSummaryResponse</returns>
        Task<IEnumerable<AuditCancelRescheduleReasons>> GetCancelRescheduleAuditReason(int? customerid, int optypeid);

        /// <summary>
        /// Save Cancel / Postpone Auidt Reasons
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>AuditSummaryResponse</returns>
        Task<SaveCancelRescheduleAuditResponse> SaveAuditCancelDetails(AuditSaveCancelRescheduleItem request);

        /// <summary>
        /// Search Auidt office list
        /// </summary>
        /// <returns>OfficeSummaryResponse</returns>
        OfficeSummaryResponse GetAuditOffice();

        /// <summary>
        /// Get Audit Type
        /// </summary>
        /// <returns>AuditType</returns>
        Task<AuditTypeResponse> GetAuditType();

        /// <summary>
        /// Get Audit work process
        /// </summary>
        /// <returns>AuditType</returns>
        Task<AuditWorkprocessResponse> GetAuditWorkProcess();

        /// <summary>
        /// Audit basic details
        /// </summary>
        /// <returns>AuditBasicDetailsResponse</returns>
        Task<AuditBasicDetailsResponse> GetAuditBasicDetails(int id);

        /// <summary>
        /// Save Audit Report Details
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>AuditSummaryResponse</returns>
        Task<SaveAuditReportResponse> SaveAuditReportDetails(AuditReportDetails request);

        /// <summary>
        ///  Get Scheduled Auditors
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>AuditSummaryResponse</returns>
        Task<AuditorResponse> GetScheduledAuditors(int auditid);

        /// <summary>
        ///  Get Audit Report Details
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns>AuditSummaryResponse</returns>
        Task<AuditReportSummary> GetAuditReportSummary(int auditid);

        /// <summary>
        ///  Get Audit Report 
        /// </summary>
        /// <returns>GetAuditReport</returns>
        Task<FileResponse> GetAuditReport(int id);

        /// <summary>
        /// Upload Report files
        /// </summary>
        /// <param name="fileList"></param>
        /// <returns></returns>
        //Task UploadReportFiles(int auditid, Dictionary<Guid, byte[]> fileList);

        /// <summary>
        ///Get Audit status
        /// </summary>
        /// <returns></returns>
        Task<AuditStatusResponse> GetAuditStatuses();

        /// <summary>
        ///Get Audit status
        /// </summary>
        /// <returns></returns>
        Task<AuditServiceTypeResponse> GetAuditServiceType(int customerid);
        Task<BookingDataRepo> GetAuditDetails(int bookingId);
        Task<List<AuditServiceTypeRepoResponse>> GetServiceTypeDataByAudit(int auditId);
        Task<List<AuditFactoryCountryRepoResponse>> GetFactoryCountryDataByAudit(int auditId);
        IQueryable<int> GetAuditNo();
        Task<BookingNoDataSourceResponse> GetBookingNoDataSource(BookingNoDataSourceRequest request);
        Task<IEnumerable<AuditCusFactDetails>> GetAuditDetails(IEnumerable<int> auditIds);
        Task<List<FactoryCountry>> GetFactoryAddressDetailsByAuditIds(IEnumerable<int> auditIds);
        Task<List<AuditServiceTypeData>> GetAuditServiceTypeList(List<int> auditIds);
        Task<AuditProductCategoryResponse> GetProductCategory(int customerId, int serviceTypeId);
        Task<IEnumerable<EntMasterConfig>> GetMasterConfiguration();
        Task<List<string>> GetCCEmailConfigurationEmailsByCustomer(int customerId, int factoryId, int bookingStatusId);
        Task<BookingMailRequest> GetBookingMailDetail(int bookingId, AuditDetails request, bool? isEdit, int? userId = 0);
        Task<SetInspNotifyResponse> BookingTaskNotification(int id, int statusId, AuditDetails request);
        Task<object> SaveEaqfAuditBooking(SaveEaqfAuditRequest request);
        Task<object> GetEaqfAuditBookingDetails(GetEaqfInspectionBookingRequest request);
        Task<object> GetAuditEaqfReportBooking(string bookingIds);
    }
}
