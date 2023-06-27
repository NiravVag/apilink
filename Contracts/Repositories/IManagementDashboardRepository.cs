using DTO.CommonClass;
using DTO.ManagementDashboard;
using DTO.Manday;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IManagementDashboardRepository
    {
        Task<int> QuotationRejectedByCustomerCount(List<int> bookingIds);

        Task<List<ProductCategoryDashboardItem>> GetProductCategoryDashboard(IEnumerable<int> inspectionIds);

        Task<List<CommonDataSource>> GetInspectionData(List<int> bookingIds);

        Task<List<MandayYearChartItem>> GetMonthlyInspManDays(IEnumerable<int> bookingIds);

        IQueryable<InspTranStatusLog> GetBookingStatusLogs(List<int> inspectionIdList);

        IQueryable<QuTranStatusLog> GetQuotationStatusLogs(List<int> inspectionIdList);

        Task<List<BookingCreatedData>> GetInspectionCreatedDate(List<int> bookingIds);

        Task<List<MandayYearChartItem>> GetCurrentYearBudgetManday(List<int> countryIdList);

        Task<List<int>> GetSupplierByCountryId(List<int> countrylist, int supType);

        Task<int> GetReportCount(IEnumerable<int> inspectionIds, IEnumerable<int> inspectedStatusIds);

        Task<int> GetProductCount(IEnumerable<int> inspectionIds, IEnumerable<int> inspectedStatusIds);

        Task<int> GetReportCountForContainers(IEnumerable<int> inspectionIds, IEnumerable<int> inspectedStatusIds);

        Task<List<ServiceTypeChartData>> GetServiceType(IEnumerable<int> bookingIds);

        Task<int> GetProductCountbyBookingQuery(IQueryable<int> inspectionIds, IEnumerable<int> inspectedStatusIds);

        Task<int> GetReportCountbyBookingQuery(IQueryable<int> inspectionIds, IEnumerable<int> inspectedStatusIds);

        Task<List<MandayYearChartItem>> GetMonthlyInspManDaysByBookingQuery(IQueryable<int> bookingIds);

        Task<List<ProductCategoryDashboardItem>> GetProductCategoryDashboardByQuery(IQueryable<int> inspectionIds);

        Task<List<ServiceTypeChartData>> GetServiceTypeByQuery(IQueryable<int> bookingIds);

        Task<int> QuotationRejectedByCustomerCountByBookingQuery(IQueryable<int> bookingIds);

        IQueryable<InspTranStatusLog> GetBookingStatusLogsByQuery(IQueryable<int> inspectionIdList);

        IQueryable<QuTranStatusLog> GetQuotationStatusLogsByQuery(IQueryable<int> inspectionIdList);

        Task<List<BookingCreatedData>> GetInspectionCreatedDateByQuery(IQueryable<int> bookingIds);
    }
}
