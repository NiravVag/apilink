using DTO.CommonClass;
using DTO.DefectDashboard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IDefectDashboardManager
    {
        Task<BookingReportResponse> GetBookingReportDetails(DefectDashboardRequest request);

        Task<DefectCategoryResponse> GetDefectCategoryList(IEnumerable<int> reportIdList);

        Task<DefectCategoryExportResponse> GetDefectCategoryExportList(DefectDashboardFilterRequest request);

        Task<DefectYearInnerCountResponse> GetDefectYearListByInnerFilter(DefectDashboardFilterRequest request);

        Task<DefectYearExportResponse> ExportDefectCountYearList(DefectDashboardFilterRequest request);

        Task<ParetoDefectResponse> GetParetoDefectList(IEnumerable<int> reportIdList);

        Task<ParetoDefectExportResponse> GetParetoDefectListExport(DefectDashboardFilterRequest request);

        Task<DefectPerformanceResponse> GetLowPerformanceDefectList(DefectPerformanceAnalysis request);

        Task<DataSourceResponse> GetDefectDataSource(CommonDataSourceRequest request);

        Task<DefectPerformanceExportResponse> GetLowPerformanceExport(DefectPerformanceAnalysis request);

        Task<DefectCountResponse> GetDefectCountByFilters(DefectPerformanceAnalysis request);

        Task<DefectPhotoResponse> GetDefectPhotoListByFilters(DefectPerformanceAnalysis request);

        Task<CountryDefectChartResponse> GetCountryDefectList(DefectDashboardFilterRequest request);

        Task<DefectCountryChartExportResponse> GetCountryDefectListExport(DefectDashboardFilterRequest request);
        Task<BookingReportResponse> GetAllBookingReportDetails(DefectDashboardFilterRequest request);
        Task<ParetoDefectResponse> GetAllDefectCount(DefectDashboardFilterRequest request);
        Task<DefectCategoryResponse> GetAllDefectCategoryList(DefectDashboardFilterRequest request);
        Task<ReportDefectResponse> GetReportDefectPareto(DefectDashboardFilterRequest request);
        Task<DataTable> ExportReportDefectPareto(DefectDashboardFilterRequest request);
        Task<List<DefectReportRepo>> GetDefectTypeList(DefectDashboardFilterRequest request);
        Task<ParetoDefectResponse> GetAllDefectCountByProductCategory(DefectDashboardFilterRequest request);
    }
}
