using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.Dashboard;
using DTO.ManagementDashboard;
using DTO.Manday;
using DTO.Report;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ManagementDashboardRepository : Repository, IManagementDashboardRepository
    {
        public ManagementDashboardRepository(API_DBContext context) : base(context)
        {
        }

        public async Task<int> QuotationRejectedByCustomerCount(List<int> bookingIds)
        {
            return await _context.QuTranStatusLogs.Where(x => bookingIds.Contains(x.BookingId.GetValueOrDefault()) && x.StatusId == (int)QuotationStatus.CustomerRejected).AsNoTracking()
                .CountAsync();
        }


        public async Task<int> QuotationRejectedByCustomerCountByBookingQuery(IQueryable<int> bookingIds)
        {
            return await _context.QuTranStatusLogs.Where(x => bookingIds.Contains(x.BookingId.GetValueOrDefault()) && x.StatusId == (int)QuotationStatus.CustomerRejected).AsNoTracking()
                .CountAsync();
        }

        /// <summary>
        /// Get the Product Category data for the inspections
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        public async Task<List<ProductCategoryDashboardItem>> GetProductCategoryDashboard(IEnumerable<int> inspectionIds)
        {
            return await _context.InspProductTransactions.Where(x => inspectionIds.Contains(x.Inspection.Id)).
                                    Where(x => x.Active.HasValue && x.Active.Value).
                                    Select(x => x.Product).Distinct().
                                    Where(x => x.ProductCategory != null).
                                    GroupBy(x => x.ProductCategory, p => p, (key, _data) =>
                                          new ProductCategoryDashboardItem
                                          {
                                              Id = key,
                                              StatusName = _data.Select(x => x.ProductCategoryNavigation.Name).FirstOrDefault(),
                                              TotalCount = _data.Count()
                                          }).AsNoTracking().OrderByDescending(x => x.TotalCount).ToListAsync();
        }

        public async Task<List<ProductCategoryDashboardItem>> GetProductCategoryDashboardByQuery(IQueryable<int> inspectionIds)
        {
            return await _context.InspProductTransactions.Where(x => inspectionIds.Contains(x.Inspection.Id)).
                                    Where(x => x.Active.HasValue && x.Active.Value && x.Product.ProductCategory > 0).
                                    Select(x => new CommonDataSource()
                                    {
                                        Id = x.Product.ProductCategory.GetValueOrDefault(),
                                        Name = x.Product.ProductCategoryNavigation.Name
                                    }).
                                    GroupBy(x => new { x.Id, x.Name }, p => p, (key, _data) =>
                                           new ProductCategoryDashboardItem
                                           {
                                               Id = key.Id,
                                               StatusName = key.Name,
                                               TotalCount = _data.Count()
                                           }).AsNoTracking().OrderByDescending(x => x.TotalCount).ToListAsync();
        }


        public async Task<List<CommonDataSource>> GetInspectionData(List<int> bookingIds)
        {
            return await _context.InspTransactions.Where(x => bookingIds.Contains(x.Id))
                .Select(x => new CommonDataSource
                {
                    Id = x.CustomerId,
                    Name = x.Customer.CustomerName
                }).AsNoTracking().ToListAsync();
        }

        //Get booking manday by month
        public async Task<List<MandayYearChartItem>> GetMonthlyInspManDays(IEnumerable<int> bookingIds)
        {
            var data = await _context.QuQuotationInsps.Where(x => bookingIds.Contains(x.IdBooking) && x.IdQuotationNavigation.IdStatus == (int)QuotationStatus.CustomerValidated).
                Select(k => new { k.IdBookingNavigation.ServiceDateFrom.Year, k.IdBookingNavigation.ServiceDateFrom.Month, k.NoOfManDay }).
                GroupBy(x => new { x.Year, x.Month }, (key, group) => new MandayYearChartItem
                {
                    Year = key.Year,
                    Month = key.Month,
                    MonthManDay = group.Sum(k => k.NoOfManDay).GetValueOrDefault(),
                    MonthName = MonthData.GetValueOrDefault(key.Month)
                }).AsNoTracking().ToListAsync();

            return data;
        }

        public async Task<List<MandayYearChartItem>> GetMonthlyInspManDaysByBookingQuery(IQueryable<int> bookingIds)
        {
            var data = await _context.QuQuotationInsps.Where(x => bookingIds.Contains(x.IdBooking) && x.IdQuotationNavigation.IdStatus == (int)QuotationStatus.CustomerValidated).
                Select(k => new { k.IdBookingNavigation.ServiceDateFrom.Year, k.IdBookingNavigation.ServiceDateFrom.Month, k.NoOfManDay }).
                GroupBy(x => new { x.Year, x.Month }, (key, group) => new MandayYearChartItem
                {
                    Year = key.Year,
                    Month = key.Month,
                    MonthManDay = group.Sum(k => k.NoOfManDay).GetValueOrDefault(),
                    MonthName = MonthData.GetValueOrDefault(key.Month)
                }).AsNoTracking().ToListAsync();

            return data;
        }

        public IQueryable<InspTranStatusLog> GetBookingStatusLogs(List<int> inspectionIdList)
        {
            return _context.InspTranStatusLogs.Where(x => inspectionIdList.Contains(x.BookingId)).AsNoTracking();
        }

        public IQueryable<InspTranStatusLog> GetBookingStatusLogsByQuery(IQueryable<int> inspectionIdList)
        {
            return _context.InspTranStatusLogs.Where(x => inspectionIdList.Contains(x.BookingId)).AsNoTracking();
        }

        public IQueryable<QuTranStatusLog> GetQuotationStatusLogs(List<int> inspectionIdList)
        {
            return _context.QuTranStatusLogs.Where(x => inspectionIdList.Contains(x.BookingId.GetValueOrDefault())).AsNoTracking();
        }

        public IQueryable<QuTranStatusLog> GetQuotationStatusLogsByQuery(IQueryable<int> inspectionIdList)
        {
            return _context.QuTranStatusLogs.Where(x => inspectionIdList.Contains(x.BookingId.GetValueOrDefault())).AsNoTracking();
        }

        public async Task<List<BookingCreatedData>> GetInspectionCreatedDate(List<int> bookingIds)
        {
            return await _context.InspTransactions.Where(x => bookingIds.Contains(x.Id))
                .Select(x => new BookingCreatedData
                {
                    BookingId = x.Id,
                    CreatedOn = x.CreatedOn
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<BookingCreatedData>> GetInspectionCreatedDateByQuery(IQueryable<int> bookingIds)
        {
            return await _context.InspTransactions.Where(x => bookingIds.Contains(x.Id))
                .Select(x => new BookingCreatedData
                {
                    BookingId = x.Id,
                    CreatedOn = x.CreatedOn
                }).AsNoTracking().ToListAsync();
        }

        public async Task<List<MandayYearChartItem>> GetCurrentYearBudgetManday(List<int> countryIdList)
        {
            var data = _context.RefBudgetForecasts
                .Where(x => x.Year == DateTime.Now.Year);

            if (countryIdList.Any())
            {
                data = data.Where(x => countryIdList.Contains(x.CountryId.GetValueOrDefault()));
            }

            return await data.GroupBy(x => new { x.Year, x.Month }, (key, group) => new MandayYearChartItem
            {
                Year = key.Year,
                Month = key.Month,
                MonthManDay = group.Sum(k => k.ManDay),
                MonthName = MonthData.GetValueOrDefault(key.Month)
            }).AsNoTracking().ToListAsync();


        }

        /// <summary>
        /// Get supplier Id by countryId
        /// </summary>
        /// <param name="countrylist"></param>
        /// <returns>supplierIds</returns>
        public async Task<List<int>> GetSupplierByCountryId(List<int> countrylist, int supType)
        {
            return await _context.SuAddresses.Where(x => countrylist.Contains(x.CountryId) && x.Supplier.TypeId == supType)
                .AsNoTracking().Select(x => x.SupplierId).Distinct().ToListAsync();
        }

        /// <summary>
        /// Get PO PRoducts(ProductId,CombineProductId) for  the bookings
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <param name="inspectedStatusIds"></param>
        /// <returns></returns>
        public async Task<int> GetProductCount(IEnumerable<int> inspectionIds, IEnumerable<int> inspectedStatusIds)
        {
            return await _context.InspProductTransactions.Where(x => x.Active.HasValue && x.Active.Value
                                                       && inspectionIds.Contains(x.InspectionId)
                                                       && inspectedStatusIds.Contains(x.Inspection.StatusId))
                                                       .AsNoTracking().Select(x => x.ProductId).Distinct()
                                                       .CountAsync();
        }

        public async Task<int> GetProductCountbyBookingQuery(IQueryable<int> inspectionIds, IEnumerable<int> inspectedStatusIds)
        {
            return await _context.InspProductTransactions.Where(x => x.Active.HasValue && x.Active.Value
                                                       && inspectionIds.Contains(x.InspectionId)
                                                       && inspectedStatusIds.Contains(x.Inspection.StatusId))
                                                       .AsNoTracking().Select(x => x.ProductId).Distinct()
                                                       .CountAsync();
        }

        /// <summary>
        /// Get report count for products
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <param name="inspectedStatusIds"></param>
        /// <returns></returns>
        public async Task<int> GetReportCount(IEnumerable<int> inspectionIds, IEnumerable<int> inspectedStatusIds)
        {
            return await _context.InspProductTransactions.Where(x => x.Active.HasValue && x.Active.Value
                                                       && inspectionIds.Contains(x.InspectionId)
                                                       && inspectedStatusIds.Contains(x.Inspection.StatusId) && x.FbReportId.HasValue
                                                       && x.FbReport.FbReportStatus == (int)FBStatus.ReportValidated && x.FbReport.Active.HasValue
                                                       && x.FbReport.Active.Value && x.FbReport.ResultId != null)
                                                       .AsNoTracking().Select(x => x.FbReportId).Distinct()
                                                       .CountAsync();
        }

        public async Task<int> GetReportCountbyBookingQuery(IQueryable<int> inspectionIds, IEnumerable<int> inspectedStatusIds)
        {
            return await _context.FbReportDetails.Where(x => x.Active.HasValue && x.Active.Value
                                                       && inspectionIds.Contains(x.Inspection.Id)
                                                       && inspectedStatusIds.Contains(x.Inspection.StatusId)
                                                       && x.FbReportStatus == (int)FBStatus.ReportValidated && x.Active.HasValue
                                                       && x.Active.Value && x.ResultId != null)
                                                       .AsNoTracking().Select(x => x.Id).Distinct()
                                                       .CountAsync();
        }

        /// <summary>
        /// Get report count for containers
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <param name="inspectedStatusIds"></param>
        /// <returns></returns>
        public async Task<int> GetReportCountForContainers(IEnumerable<int> inspectionIds, IEnumerable<int> inspectedStatusIds)
        {
            return await _context.InspContainerTransactions.Where(x => x.Active.HasValue && x.Active.Value
                                                       && inspectionIds.Contains(x.InspectionId)
                                                       && inspectedStatusIds.Contains(x.Inspection.StatusId) && x.FbReportId.HasValue
                                                       && x.FbReport.FbReportStatus == (int)FBStatus.ReportValidated && x.FbReport.Active.HasValue
                                                       && x.FbReport.Active.Value && x.FbReport.ResultId != null)
                                                       .AsNoTracking().Select(x => x.FbReportId).Distinct()
                                                       .CountAsync();
        }

        //Get the service Type of each booking. 
        public async Task<List<ServiceTypeChartData>> GetServiceType(IEnumerable<int> bookingIds)
        {
            return await _context.InspTranServiceTypes
                  .Where(x => bookingIds.Contains(x.InspectionId) && x.Active)
                  .Select(x => new
                  {
                      x.InspectionId,
                      x.ServiceTypeId,
                      x.ServiceType.Name

                  }).GroupBy(p => new { p.ServiceTypeId, p.Name }, (key, _data) => new ServiceTypeChartData
                  {
                      ServiceTypeId = key.ServiceTypeId,
                      ServiceTypeName = key.Name,
                      Count = _data.Count(),

                  }).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// get the service types by booking ids
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>        
        public async Task<List<ServiceTypeChartData>> GetServiceTypeByQuery(IQueryable<int> bookingIds)
        {
            return await _context.InspTranServiceTypes
                  .Where(x => bookingIds.Contains(x.InspectionId) && x.Active)
                  .Select(x => new
                  {
                      x.InspectionId,
                      x.ServiceTypeId,
                      x.ServiceType.Name
                  }).GroupBy(p => new { p.ServiceTypeId, p.Name }, (key, _data) => new ServiceTypeChartData
                  {
                      ServiceTypeId = key.ServiceTypeId,
                      ServiceTypeName = key.Name,
                      Count = _data.Count(),
                  }).AsNoTracking().ToListAsync();
        }
    }
}
