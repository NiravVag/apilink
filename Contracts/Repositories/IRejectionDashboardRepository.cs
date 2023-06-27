using DTO.RejectionDashboard;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IRejectionDashboardRepository
    {
        Task<List<ChartItem>> GetProductCategoryDashboard(IQueryable<int> inspectionIds, bool isExport, bool isOnlyRejectionResult = false);
        Task<List<ChartItem>> GetSupplierDashboard(IQueryable<int> inspectionIds, bool isExport);
        Task<List<CountryChartItem>> GetResultByCountry(List<int> bookingIdList);
        Task<List<CountryChartItem>> GetQueryableResultByCountry(IQueryable<int> bookingIdList);
        Task<List<CountryChartItem>> GetResultByProvince(IQueryable<int> bookingIdList, int? countryId);
        IQueryable<CountryChartItem> ExportResultByCountryProvince(List<int> bookingIdList);
        IQueryable<CountryChartItem> ExportQueryablesultByCountryProvince(IQueryable<int> bookingIdList);
        Task<List<RejectChartMonthItem>> GetCustomerReportReject(IQueryable<int> bookingIdList, int fbResultId);
        Task<List<RejectChartMonthItem>> GetCustomerReportRejectSubcatogory(IQueryable<int> bookingIdList, RejectChartSubcatogoryRequest request);
        Task<List<RejectChartMonthItem>> GetCustomerReportRejectSubcatogory2(IQueryable<int> bookingIdList, RejectChartSubcatogory2Request request);
        Task<List<RejectionFactoryData>> GetCustomerReportRejectPopUpData(IQueryable<int> bookingIdList, string rejectReasonName, int fbReportResultId);
        Task<List<RejectionImageResult>> GetReportRejectImageData(IQueryable<int> bookingIdList, List<string> rejectReasonName, int fbReportResultId);
        Task<List<RejectionReportData>> GetReportByInspectionIds(IQueryable<int> bookingIdList, string rejectReasonName, int fbReportResultId);
        Task<List<RejectionFactoryData>> GetCustomerReportSubcatogoryPopUpData(IQueryable<int> bookingIdList, string rejectReasonName, int fbReportResultId, List<string> summaryNames);
        Task<List<RejectionFactoryData>> GetCustomerReportSubcatogory2PopUpData(IQueryable<int> bookingIdList, string rejectReasonName, int fbReportResultId, List<string> summaryNames, List<string> subcatogory);
        Task<List<RejectionReportData>> GetSubcatogoryReportByInspectionIds(IQueryable<int> bookingIdList, string rejectReasonName, int fbReportResultId, List<string> summaryNames);
        Task<List<RejectionReportData>> GetSubcatogory2ReportByInspectionIds(IQueryable<int> bookingIdList, string rejectReasonName, int fbReportResultId, List<string> summaryNames, List<string> subcatogory);
        IQueryable<ExportRejectionTableRepoData> ExportCustomerReportRejectSubcatogory2(IQueryable<int> bookingIdList, RejectChartSubcatogory2Request request);
        IQueryable<RejectionRateData> GetQueryableReportRejectionRate(IQueryable<int> bookingIdList);
        IQueryable<RejectionRateData> GetQueryableCusDecisionRejectionRate(IQueryable<int> bookingIdList);
        Task<List<ChartItem>> GetFactoryDashboard(IQueryable<int> inspectionIds, bool isExport, bool isOnlyRejectionResult = false);
    }
}
