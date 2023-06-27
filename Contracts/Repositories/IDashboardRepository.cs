using DTO.Dashboard;
using DTO.DefectDashboard;
using DTO.Kpi;
using DTO.Manday;
using DTO.User;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IDashboardRepository : IRepository
    {
        /// <summary>
        /// Get the booking detail data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        IQueryable<InspTransaction> GetBookingDetail(CustomerDashboardFilterRequest request);

        /// <summary>
        /// Get the product count for the inspection ids
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        Task<int> GetProductCount(IEnumerable<int> inspectionIds);

        /// <summary>
        /// Get the inspection mandays
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        IQueryable<InspectionMandayDashboard> GetInspectionManDays(IEnumerable<int> inspectionIds);

        /// <summary>
        /// Get the API Result Data
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <param name="inspectionStatusList"></param>
        /// <returns></returns>
        IQueryable<FbReportDetail> GetAPIRADashboard(IQueryable<int> inspectionIds, IEnumerable<int> inspectionStatusList);

        /// <summary>
        /// Get the customer result data for the inspections
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <param name="inspectionStatusList"></param>
        /// <returns></returns>
        IQueryable<InspRepCusDecision> GetCustomerResult(IQueryable<int> inspectionIds, IEnumerable<int> inspectionStatusList);

        /// <summary>
        /// Get the customer decision data
        /// </summary>
        /// <param name="resultIds"></param>
        /// <returns></returns>
        Task<List<CustomerResultMasterRepo>> GetCustomerResultAnalysis(List<int> resultIds);

        /// <summary>
        /// Get the Product Category data for the inspections
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        IQueryable<InspProductTransaction> GetProductCategoryDashboard(IQueryable<int> inspectionIds);

        /// <summary>
        /// GetPOETDDateByInspectionId
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        Task<List<CustomerETDDataRepo>> GetPOETDDateByInspectionId(IEnumerable<int> inspectionIds);

        /// <summary>
        /// GetSupplierRevisionData
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        Task<List<SupplierBookingRevisionRepo>> GetSupplierRevisionData(IEnumerable<int> inspectionIds);
        /// <summary>
        /// Get the inspection reject list
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <param name="inspectedStatusIds"></param>
        /// <returns></returns>
        IQueryable<FbReportInspSummary> GetCustomerInspectionReject(IQueryable<int> inspectionIds, IEnumerable<int> inspectedStatusIds);
        /// <summary>
        /// Get the count of the quotation needs to be validated
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<int> GetPendingQuotations(int customerId);
        /// <summary>
        /// Get the count of the quotation which is validated
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<int> GetCompletedQuotations(int customerId);
        /// <summary>
        /// Get the man days group by service date
        /// </summary>
        /// <param name="ServiceDateFrom"></param>
        /// <param name="ServiceDateTo"></param>
        /// <param name="inspectedStatusIds"></param>
        /// <returns></returns>
        Task<List<InspectionManDaysRepo>> GetInspectionManDays(DateTime ServiceDateFrom, DateTime ServiceDateTo, IEnumerable<int> inspectedStatusIds, int customerId);
        /// <summary>
        /// Get the monthly inspection man days
        /// </summary>
        /// <param name="ServiceDateFrom"></param>
        /// <param name="ServiceDateTo"></param>
        /// <param name="inspectedStatusIds"></param>
        /// <returns></returns>
        Task<List<InspectionMonthlyManDaysRepo>> GetMonthlyInspectionManDays(DateTime ServiceDateFrom, DateTime ServiceDateTo, IEnumerable<int> inspectedStatusIds, int customerId);

        List<FBReportResultData> GetFbReportResults();

        /// <summary>
        /// Get the inspected booking count
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <param name="inspectedStatusIds"></param>
        /// <returns></returns>
        Task<int> GetInspectedBookingCount(IEnumerable<int> inspectionIds, IEnumerable<int> inspectedStatusIds);
        Task<double> GetInspectedManDaysCount(IEnumerable<int> inspectionIds, IEnumerable<int> inspectedStatusIds);

        Task<UserStaffDetails> GetCSDetails(int customerId);

        Task<List<POProductsRepo>> GetPOProducts(IEnumerable<int> inspectionIds, IEnumerable<int> inspectedStatusIds);

        Task<List<InspCountryGeoCode>> GetInspCountryGeoCode(IEnumerable<int> lstinspection);

        Task<List<InspCountryGeoCode>> GetInspCountryGeoCodeAllocated(int customerid);

        /// <summary>
        /// Get the report count for the inspectionids
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        Task<List<FbReportCustomerDashboard>> GetReportData(IEnumerable<int> inspectionIds);

        /// <summary>
        /// Get the fb report insp summary
        /// </summary>
        /// <param name="reportIds"></param>
        /// <returns></returns>
        Task<List<FBReportInspSubSummary>> GetFBInspSummaryResultbyReport(IEnumerable<int> fbReportIdList);

        /// <summary>
        /// get monthly insp mandays count
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        Task<List<MandayYearChartItem>> GetMonthlyInspManDays(IEnumerable<int> bookingIds);

        IQueryable<SuAddress> GetFactoryAddressById(IEnumerable<int> factoryIds);
        Task<List<CustomerResultRepo>> GetCustomerDecisionResult(IQueryable<int> inspectionIds, IEnumerable<int> inspectionStatusList);
        Task<List<CustomerAPIRADashboardRepo>> GetQueriableAPIRADashboard(IQueryable<int> inspectionIds, IEnumerable<int> inspectionStatusList);
        Task<List<CustomerResultRepo>> GetQueryableCustomerResult(IQueryable<int> inspectionIds, IEnumerable<int> inspectionStatusList);

        Task<double> GetInspectionManDaysQuery(IQueryable<int> inspectionIds);

        Task<List<CustomerAPIRADashboardRepo>> GetAPIRADashboardByQuery(IQueryable<int> inspectionIds, IEnumerable<int> inspectionStatusList);

        Task<List<InspectionRejectDashboard>> GetCustomerInspectionRejectByQuery(IQueryable<int> inspectionIds, IEnumerable<int> inspectedStatusIds);
        IQueryable<SuAddress> GetQueryableFactoryAddressById(IQueryable<int> factoryIds);
        Task<List<MandayYearChartItem>> GetQueryableMonthlyInspManDays(IQueryable<int> bookingIds);

        IQueryable<InspectionMandayDashboard> GetInspectionActualCount(IEnumerable<int> inspectionIds);
    }
}
