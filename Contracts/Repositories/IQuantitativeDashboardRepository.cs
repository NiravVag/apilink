using DTO.ManagementDashboard;
using DTO.QuantitativeDashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IQuantitativeDashboardRepository
    {
        Task<int> GetQcCount(List<int> bookingIdList);
        Task<List<MandayCountry>> GetFactoryCountry(List<int> bookingIdList);
        Task<List<TurnOverItem>> GetTotalInvoiceFeeData(List<int> bookingIdList);
        Task<List<TurnOverItem>> GetExtraFeeData(List<int> bookingIdList);
        
        Task<List<TurnOverItem>> GetServiceTypeData(List<int> bookingIdList);

        Task<List<ProductCategoryDashboardItem>> GetQueryableProductSubCategoryDashboard(IQueryable<int> InspectionIdList, IEnumerable<int> prodCategoryIds);

        IQueryable<ProductCategoryDashboardExportItem> GetProductCategoryDashboardExport(List<int> bookingIdList);

        Task<List<OrderQtyChartItem>> GetMonthlyInspOrderQuantity(IEnumerable<int> bookingIds);
        Task<int> GetQueryableQcCount(IQueryable<int> bookingIdList);
        Task<List<MandayCountry>> GetQueryableFactoryCountry(IQueryable<int> bookingIdList);
        Task<List<TurnOverItem>> GetQueryableTotalInvoiceFeeData(IQueryable<int> bookingIdList);
        Task<List<TurnOverItem>> GetQueryableExtraFeeData(IQueryable<int> bookingIdList);
        Task<List<TurnOverItem>> GetQueryableServiceTypeData(IQueryable<int> bookingIdList);
        Task<List<OrderQtyChartItem>> GetQueryableMonthlyInspOrderQuantity(IQueryable<int> bookingIds);
        IQueryable<ProductCategoryDashboardExportItem> GetQueryableProductCategoryDashboardExport(IQueryable<int> bookingIdList);
    }
}
