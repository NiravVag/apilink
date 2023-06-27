using DTO.Eaqf;
using DTO.RejectionDashboard;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IRejectionDashboardManager
    {
        Task<RejectResultDashboardResponse> GetAPIResultDashboard(RejectionDashboardFilterRequest request);
        Task<RejectResultDashboardResponse> GetCustomerResultDashboard(RejectionDashboardSearchRequest request);
        Task<RejectionCommonExport> ExportAPIResultDashboard(RejectionDashboardSearchRequest request);
        Task<RejectionCommonExport> ExportCustomerResultDashboard(RejectionDashboardSearchRequest request);
        Task<ProductCategoryChartResponse> GetProductCategoryResultDashboard(RejectionDashboardSearchRequest request);
        Task<ChartExport> ExportProductCategoryDashboard(RejectionDashboardSearchRequest request);
        Task<VendorChartResponse> GetVendorResultDashboard(RejectionDashboardSearchRequest request);
        Task<ChartExport> ExportVendorDashboard(RejectionDashboardSearchRequest request);
        Task<CountryChartResponse> GetCountryResultDashboard(CountryChartRequest request);
        Task<CountryChartExport> ExportCountryDashboard(RejectionDashboardSearchRequest request);
        Task<RejectChartResponse> GetCustomerReportReject(RejectChartRequest request);
        Task<RejectChartResponse> GetCustomerReportRejectSubcatogory(RejectChartSubcatogoryRequest request);
        Task<RejectChartResponse> GetCustomerReportRejectSubcatogory2(RejectChartSubcatogory2Request request);
        Task<RejectionPopUpResponse> GetRejectPopUpData(RejectionDashboardSearchRequest request);
        Task<RejectionImageResponse> GetRejectionImages(RejectionDashboardSearchRequest request);

        Task<RejectResultDashboardResponse> GetAllAPIResultDashboard(RejectionDashboardSearchRequest request);
        Task<List<ExportRejectionTableData>> ExportRejectionDashboardData(RejectChartSubcatogory2Request request);
        Task<RejectionRateResponse> GetReportRejectionRate(RejectionDashboardSearchRequest request);
        Task<DataTable> ExportReportRejectionRate(RejectionDashboardSearchRequest request);
        Task<object> GetReportResultAnalytics(EaqfDashboardRequest eaqfDashboardRequest);
        Task<object> GetReportRejectionAnalytics(EaqfDashboardRequest eaqfDashboardRequest);
        Task<object> GetReportRejectionResultByProductCategory(EaqfDashboardRequest eaqfDashboardRequest);
        Task<object> GetReportRejectionResultByFactory(EaqfDashboardRequest eaqfDashboardRequest);
        Task<object> GetReportDefectAnalytics(EaqfDashboardRequest eaqfDashboardRequest);
        Task<object> GetDefectTypeDetails(EaqfDashboardRequest eaqfDashboardRequest);
        Task<object> GetReportDefectAnalyticsByProductCatgory(EaqfDashboardRequest eaqfDashboardRequest);
    }
}
