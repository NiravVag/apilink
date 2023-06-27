using DTO.Dashboard;
using DTO.ManagementDashboard;
using DTO.Manday;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IManagementDashboardManager
    {
        /// <summary>
        /// manday count by year 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<MandayYearChartManagementDashboardResponse> GetMandayYearChart(ManagementDashboardRequest request);

        /// <summary>
        /// Get the dashboard main search data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ManagementDashboardResponse> GetManagementDashboardSearch(ManagementDashboardRequest request);

        /// <summary>
        /// get the overview dashboard data
        /// </summary>
        /// <param name="inspectionIdList"></param>
        /// <returns></returns>
        Task<OverviewDashboardResponse> OverviewDashboardSearch(ManagementDashboardRequest request);
        /// <summary>
        ///  get the reject dashboard data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        Task<InspectionRejectDashboardResponse> GetInspectionRejectChartByQuery(ManagementDashboardRequest request);

        /// <summary>
        /// get the product category data
        /// </summary>
        /// <param name="inspectionIdList"></param>
        /// <returns></returns>
        Task<ProductCategoryDashboardResponse> ProductCategoryDashboardSearch(List<int> inspectionIdList);

        /// <summary>
        /// get the API result dashboard data
        /// </summary>
        /// <param name="inspectionIdList"></param>
        /// <returns></returns>
        Task<ResultDashboardResponse> GetResultDashboard(ManagementDashboardRequest request);

        /// <summary>
        /// get the service type data
        /// </summary>
        /// <param name="inspectionIdList"></param>
        /// <returns></returns>
        Task<ResultAnalyticsDashboardResponse> GetServiceTypeChart(ManagementDashboardRequest request);

        Task<ResultAnalyticsDashboardResponse> GetServiceTypeChart(List<int> inspectionIdList);

        /// <summary>
        /// get the average booking status change time
        /// </summary>
        /// <param name="inspectionIdList"></param>
        /// <returns></returns>
        Task<AverageBookingTimeResponse> GetAverageBookingStatusChangeTime(ManagementDashboardRequest request);

        /// <summary>
        /// get the average quotation status change time
        /// </summary>
        /// <param name="inspectionIdList"></param>
        /// <returns></returns>
        Task<AverageQuotationTimeResponse> GetAverageQuotationStatusChangeTime(ManagementDashboardRequest request);

        /// <summary>
        /// Export product category chart data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ManagementDashboardChartExport> ExportProductCategoryChart(ManagementDashboardRequest request);

        /// <summary>
        /// Export Service Type chart data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ManagementDashboardChartExport> ExportServiceTypeChart(ManagementDashboardRequest request);

        /// <summary>
        /// export result chart data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ManagementDashboardChartExport> ExportResultChart(ManagementDashboardRequest request);

        Task<List<int>> GetSupplierByCountryId(List<int> countrylist, int supType);

        Task<ProductCategoryDashboardResponse> ProductCategoryDashboardSearchbyQuery(IQueryable<int> inspectionIdList);

        Task<ResultDashboardResponse> GetResultDashboardByQuery(IQueryable<int> inspectionIdList);

        Task<ProductCategoryDashboardResponse> ProductCategoryDashboardSearchByBookingRequest(ManagementDashboardRequest request);
        Task<ResultAnalyticsDashboardResponse> GetServiceTypeChartByQuery(IQueryable<int> inspectionIdList);
    }
}
