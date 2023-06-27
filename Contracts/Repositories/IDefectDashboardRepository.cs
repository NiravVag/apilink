using DTO.CommonClass;
using DTO.DefectDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IDefectDashboardRepository
    {
        Task<List<DefectCategoryModel>> GetDefectCategoryList(IEnumerable<int> reportIdList);
        Task<List<DefectCategoryModel>> GetDefectCategoryQueryableList(IQueryable<int> reportIdList);

        Task<List<ReportSupplierDetails>> GetSupFactReportDetailsByProductLevel(IEnumerable<int> reportIds);

        IQueryable<ReportDefectListRepo> GetReportDefectList(IEnumerable<int> reportIds);
        IQueryable<ReportDefectListRepo> GetReportDefectQueryableList(IQueryable<int> reportIds);

        Task<List<ReportSupplierDetails>> GetSupFactReportDetailsByContainerLevel(IEnumerable<int> reportIds);

        IQueryable<CommonDataSource> GetDefectList();

        IQueryable<DefectPhotoRepo> GetDefectPhotoList(IEnumerable<int> reportIds);

        Task<List<CountryReport>> GetCountryProductReportData(IEnumerable<int> reportIds);

        Task<List<CountryReport>> GetCountryContainerReportData(IEnumerable<int> reportIds);

        Task<List<ParetoDefectRepo>> GetDefectListByReportIds(IEnumerable<int> reportIds);

        Task<List<DefectReportRepo>> GetReportDefectsList(IEnumerable<int> reportIds);
        Task<List<DefectReportRepo>> GetReportDefectsQueryableList(IQueryable<int> reportIds);

        Task<List<InspectionMonthRepo>> GetInspectionContainerDefectsList(IEnumerable<int> reportIds);

        Task<List<InspectionMonthRepo>> GetInspectionProductDefectsList(IEnumerable<int> reportIds);

        Task<List<InspectionMonthRepo>> GetInspectionDefectsList(IQueryable<int> reportIds);

        IQueryable<ParetoDefectRepo> GetTotalDefectList(IEnumerable<int> reportIds);
        IQueryable<ParetoDefectRepo> GetTotalDefectQueryableList(IQueryable<int> reportIds);
        Task<List<ReportSupplierDetails>> GetSupFactReportDetailsQueryableReport(IQueryable<int> reportIds);
        IQueryable<DefectPhotoRepo> GetDefectPhotoQueryableList(IQueryable<int> reportIds);
        Task<List<ParetoDefectRepo>> GetDefectListByQueryableReportIds(IQueryable<int> reportIds);
        Task<List<CountryReport>> GetCountryInspReportData(IQueryable<int> reportIds);
        IQueryable<DefectReportList> GetQueryableReportDefect(IQueryable<int> bookingIdList);
        IQueryable<ParetoDefectRepo> GetTotalDefectQueryableListbyProductCategory(IQueryable<int> reportIds);

    }
}
