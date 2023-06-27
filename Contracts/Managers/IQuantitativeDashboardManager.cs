using DTO.ManagementDashboard;
using DTO.Manday;
using DTO.ProductManagement;
using DTO.QuantitativeDashboard;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IQuantitativeDashboardManager
    {
        Task<QuantitativeDashboardResponse> GetQuantitativeDashboardSummary(QuantitativeDashboardRequest request);

        Task<QuantitativeDashboardResponse> GetAllQuantitativeDashboardSummary(QuantitativeDashboardFilterRequest request);

        Task<QuantitativeMandayYearChartResponse> GetMandayYearChart(QuantitativeDashboardFilterRequest request);

        Task<QuantitativeMandayYearExport> ExportMandayChart(QuantitativeDashboardFilterRequest request);

        Task<QuantitativeDashboardCommonResponse> GetMandayByCountry(QuantitativeDashboardFilterRequest request);

        Task<QuantitativeCommonExport> ExportMandayCountryChart(QuantitativeDashboardFilterRequest request);

        Task<TurnOverDataResponse> GetTurnOverSummary(QuantitativeDashboardFilterRequest request);

        Task<QuantitativeCommonExport> ExportTurnOverByServiceTypeChart(QuantitativeDashboardFilterRequest request);

        Task<QuantitativeDashboardCommonResponse> GetServiceTypeChart(QuantitativeDashboardFilterRequest request);

        Task<QuantitativeCommonExport> ExportInspectionByServiceTypeChart(QuantitativeDashboardFilterRequest request);

        Task<PiecesInspectedChartResponse> GetBookingQuantityData(QuantitativeDashboardFilterRequest request);

        Task<OrderQuantityCountChartExport> GetBookingQuantityDashboardExport(QuantitativeDashboardFilterRequest request);

        Task<ProductCategoryDashboardResponse> GetProductCategoryList(ProductCategoryChartRequest request);

        Task<QuantitativeProductCategoryChartExport> ProductCategoryListExport(QuantitativeDashboardFilterRequest quantitativerequest);

        Task<ProductCategoryResponse> GetProductCategoryList();
    }
}
