using DTO.CommonClass;
using DTO.FullBridge;
using DTO.Kpi;
using DTO.Report;
using Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IFullBridgeRepository : IRepository
    {
        Task<InspProductTransaction> GetInspectionReports(int reportId);
        Task<int> UpdateInspectionStatus(InspTransaction entity);
        Task<FbReportDetail> GetInspectionFbReports(int reportId);
        Task<int> AddInspectionFbReport(FbReportDetail entity);
        Task<int> UpdateInspectionFbReport(FbReportDetail entity);
        Task<IEnumerable<FbReportDetail>> GetFbReportDetails();
        Task<InspPurchaseOrderTransaction> GetInspectionTransaction(int bookingId, string strPoNumber, int productId);
        Task<IEnumerable<FBReportDetails>> GetFbReportStatusCustomerReport();
        Task<FBReportDetails> GetFbReportStatusCustomerReportbyBooking(int bookingId);
        Task<IEnumerable<FBReportDetails>> GetFbReportStatusListCustomerReportbyBooking(List<int> bookingId);
        Task<int> RemoveInspectionFbReport(FbReportDetail entity);
        Task<InspPurchaseOrderTransaction> GetInspectionTransaction(int reportId, string strPoNumber, int productId, int containerId);
        Task<InspContainerTransaction> GetInspectionReportsFromContainer(int reportId);
        Task<int> GetContainersizeId(string containerSize);
        Task<FbReportDetail> GetInspectionFbReportsWithStatus(int reportId);
        Task<int?> GetProductIdByFbProductId(int? fbProductId);
        Task<List<ReportProductsAndPo>> GetInspectionProductContainerReportData(int reportId);
        Task<List<ReportProductsAndPo>> GetInspectionProductColorReportData(int reportId);
        Task<bool> checkAllTheReportsNotValidatedOrInvalidated(int bookingId);
        /// <summary>
        /// Get inspection id by fb report id
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns></returns>
        Task<int> GetInspectionIdByReportId(int reportId);

        Task<InspProductTransaction> GetInspectionReportsFactoryAddress(int reportId);

        Task<InspContainerTransaction> GetContainerReportsFactoryAddress(int reportId);

        Task<int> UpdateInspectionFactoryAddress(SuAddress entity);
        Task<List<ReportIdData>> getReportIdsbyBooking(int bookingId);
        Task<List<ReportIdData>> getReportIdsbyBookingServiceDates(DateTime bookingStartDate, DateTime bookingEndDate);
        Task<List<ReportIdData>> getNonValidatedReportIdsbyBooking(int bookingId);
        Task<int> GetApiReportIdbyFbReport(int fbReportId);

        Task<int?> GetAqlLevelbyFbAql(string fbAqlLevel);
        Task<List<FBReportDetails>> GetFbReportTitleListByReportIds(IEnumerable<int?> fbReportMapIdList);
        Task<List<SchScheduleC>> GetCSDetailList(int bookingId, List<int?> StaffIdList);
        Task<List<SchScheduleQc>> GetQCDetailList(int bookingId, List<int?> StaffIdList);
        Task<List<int?>> GetFBReportStaffList(List<int> fbUserIds);
        Task<AudTransaction> GetAuditFbReports(int reportId);
        Task<int> GetFbServiceTypeId(int serviceId);
        Task<List<AudTranCSDetail>> AudTranCSForAudit(int auditId);

        Task<bool> IsAnyReportsAvailableByReportStatuses(int bookingId, IEnumerable<int?> reportStatus);

        Task<List<AudTranAuditor>> GetAuditAuditorList(int auditId, List<int?> staffIdList);
        Task<List<AudTranC>> GetAuditCSList(int auditId, List<int?> staffIdList);
    }
}
