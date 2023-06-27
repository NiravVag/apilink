using DTO.Dashboard;
using DTO.DefectDashboard;
using DTO.FinanceDashboard;
using DTO.Inspection;
using DTO.InspectionCustomerDecision;
using DTO.ManagementDashboard;
using DTO.QuantitativeDashboard;
using DTO.RejectionDashboard;
using DTO.Report;
using DTO.Schedule;
using DTO.SharedInspection;
using Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface ISharedInspectionManager
    {
        SharedInspectionModel GetInspectionQueryRequestMap(InspectionSummarySearchRequest request);
        SharedInspectionModel GetScheduleInspectionQueryRequestMap(ScheduleSearchRequest request);
        SharedInspectionModel GetManagementDashboardInspectionRequestMap(ManagementDashboardRequest request);
        SharedInspectionModel GetDashBoardInspectionQueryRequestMap(DefectDashboardFilterRequest request);
        IQueryable<InspTransaction> GetAllInspectionQuery();
        IQueryable<FbReportDetail> GetAllFbReportDetails();
        IQueryable<InspTransaction> GetInspectionQuerywithRequestFilters(SharedInspectionModel request, IQueryable<InspTransaction> inspectionQuery);

        Stream GetAsStreamObject<T>(IEnumerable<T> result, string sheetName = "sheet1");
        Stream GetAsStreamObjectAndLoadDataTable(dynamic result, string sheetName = "sheet1");
        SharedInspectionModel GetCustomerDecisionQueryRequestMap(CustomerDecisionSummaryRequest request);
        Task<List<InspectionStatus>> GetInspectionStatusList(IQueryable<InspTransaction> bookingData);
        IQueryable<InspTransaction> GetInspectionsQuery(int bookingId);
        SharedInspectionModel GetRejectionDashBoardInspectionSearchRequestMap(RejectionDashboardSearchRequest request);
        SharedInspectionModel GetQuantitativeDashBoardInspectionQueryRequestMap(QuantitativeDashboardFilterRequest request);
        SharedInspectionModel GetFinanceDashBoardInspectionQueryRequestMap(FinanceDashboardSearchRequest request);
        SharedInspectionModel GetCustomerReportInspectionQueryRequestMap(int customerId, DateTime fromDate, DateTime toDate, CustomerReportDetailsRequest request);

        IQueryable<FbReportDetail> GetFbReportDetailswithRequestFilters(SharedInspectionModel request, IQueryable<FbReportDetail> fbReportDetailsQuery);
        SharedInspectionModel GetDashboardMapInspectionRequestMap(DashboardMapFilterRequest request);
    }
}
