using DTO.CommonClass;
using DTO.FinanceDashboard;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IFinanceDashboardManager
    {
        Task<MandayYearChartFinanceDashboardResponse> GetBilledMandayData(FinanceDashboardRequest request);
        Task<MandayYearChartFinanceDashboardResponse> GetMandayRateData(FinanceDashboardRequest request);
        Task<FinanceDashboardBookingDataResponse> GetBookingIdList(FinanceDashboardRequest request);
        Task<FinanceDashboardTurnOverResponse> GetFinanceDashboardTurnOverData(List<int> bookingIdList);
        Task<ChargeBackChartResponse> GetChargeBackChartData(List<int> bookingIdList);
        Task<QuotationChartResponse> GetQuotationChartData(List<int> bookingIdList);
        Task<FinanceDashboardMandayExportItem> ExportBilledMandayChart(FinanceDashboardRequest request);
        Task<FinanceDashboardCommonExportItem> CountryTurnOverExport(FinanceDashboardRequest request);
        Task<FinanceDashboardCommonExportItem> ProductCategoryTurnOverExport(FinanceDashboardRequest request);
        Task<FinanceDashboardCommonExportItem> ServiceTypeTurnOverExport(FinanceDashboardRequest request);
        Task<FinanceDashboardRatioAnalysisResponse> GetRatioAnalysisList(FinanceDashboardSearchRequest request);
        Task<List<FinanceDashboardExportRatioAnalysis>> ExportRatioAnalysisList(FinanceDashboardSearchRequest request);
        Task<DataSourceResponse> GetEmployeeTypes();

        Task<List<ExchangeCurrencyItem>> CurrencyConversionToUsd(List<ExchangeCurrencyItem> data, int targetCurrency);
    }
}
