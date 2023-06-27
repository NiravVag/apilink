using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.ExtraFees;
using DTO.Invoice;
using DTO.ManagementDashboard;
using DTO.QuantitativeDashboard;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class QuantitativeDashboardRepository : Repository, IQuantitativeDashboardRepository
    {
        public QuantitativeDashboardRepository(API_DBContext context) : base(context)
        {
        }

        public async Task<int> GetQcCount(List<int> bookingIdList)
        {
            return await _context.SchScheduleQcs.Where(x => x.Active && bookingIdList.Contains(x.BookingId) && x.Qctype == (int)QCType.QC)
                .Select(x => x.Qcid)
                .Distinct().CountAsync();
        }
        public async Task<int> GetQueryableQcCount(IQueryable<int> bookingIdList)
        {
            return await _context.SchScheduleQcs.Where(x => x.Active && bookingIdList.Contains(x.BookingId) && x.Qctype == (int)QCType.QC)
                .Select(x => x.Qcid)
                .Distinct().CountAsync();
        }

        //get the factory country
        public async Task<List<MandayCountry>> GetFactoryCountry(List<int> bookingIdList)
        {
            return await (from insp in _context.InspTransactions
                          join supAddress in _context.SuAddresses on insp.FactoryId equals supAddress.SupplierId
                          join quot in _context.QuQuotationInsps on insp.Id equals quot.IdBooking
                          where quot.IdQuotationNavigation.IdStatus == (int)QuotationStatus.CustomerValidated &&
                          bookingIdList.Contains(insp.Id)
                          select new
                          {
                              quot.NoOfManDay,
                              supAddress.CountryId,
                              supAddress.Country.CountryName,
                              insp.ServiceDateFrom.Year
                          }).GroupBy(p => new { p.CountryId, p.CountryName }, (key, _data) => new MandayCountry
                          {
                              Manday = _data.Sum(x => x.NoOfManDay),
                              CountryId = key.CountryId,
                              CountryName = key.CountryName,
                          })
                          .AsNoTracking().ToListAsync();
        }
        //get the factory country
        public async Task<List<MandayCountry>> GetQueryableFactoryCountry(IQueryable<int> bookingIdList)
        {
            //this is due to low performance when pass as iqueriable
            var booklst = await bookingIdList.ToListAsync();
            return await (from insp in _context.InspTransactions
                          join supAddress in _context.SuAddresses on insp.FactoryId equals supAddress.SupplierId
                          join quot in _context.QuQuotationInsps on insp.Id equals quot.IdBooking
                          where quot.IdQuotationNavigation.IdStatus == (int)QuotationStatus.CustomerValidated &&
                          booklst.Contains(insp.Id)
                          select new
                          {
                              quot.NoOfManDay,
                              supAddress.CountryId,
                              supAddress.Country.CountryName,
                              insp.ServiceDateFrom.Year
                          }).GroupBy(p => new { p.CountryId, p.CountryName }, (key, _data) => new MandayCountry
                          {
                              Manday = _data.Sum(x => x.NoOfManDay),
                              CountryId = key.CountryId,
                              CountryName = key.CountryName,
                          })
                          .AsNoTracking().ToListAsync();
        }

        //get the turnover data from audit table
        public async Task<List<TurnOverItem>> GetTotalInvoiceFeeData(List<int> bookingIdList)
        {
            return await (from inv in _context.InvAutTranDetails
                          where bookingIdList.Contains(inv.InspectionId.GetValueOrDefault()) && inv.InvoiceStatus != (int)InvoiceStatus.Cancelled
                          select new TurnOverItem
                          {
                              InvoiceTo = inv.InvoiceTo,
                              TotalInvoiceFee = inv.TotalInvoiceFees,
                              CurrencyId = inv.InvoiceCurrency
                          })
                            .GroupBy(p => new { p.InvoiceTo, p.CurrencyId }, (key, _data) => new TurnOverItem
                            {
                                InvoiceTo = key.InvoiceTo,
                                CurrencyId = key.CurrencyId,
                                TotalInvoiceFee = _data.Sum(x => x.TotalInvoiceFee),
                                Count = _data.Count()
                            }).AsNoTracking().ToListAsync();
        }
        //get the turnover data from audit table
        public async Task<List<TurnOverItem>> GetQueryableTotalInvoiceFeeData(IQueryable<int> bookingIdList)
        {
            return await (from inv in _context.InvAutTranDetails
                          where bookingIdList.Contains(inv.InspectionId.GetValueOrDefault()) && inv.InvoiceStatus != (int)InvoiceStatus.Cancelled
                          select new TurnOverItem
                          {
                              InvoiceTo = inv.InvoiceTo,
                              TotalInvoiceFee = inv.TotalInvoiceFees,
                              CurrencyId = inv.InvoiceCurrency
                          })
                            .GroupBy(p => new { p.InvoiceTo, p.CurrencyId }, (key, _data) => new TurnOverItem
                            {
                                InvoiceTo = key.InvoiceTo,
                                CurrencyId = key.CurrencyId,
                                TotalInvoiceFee = _data.Sum(x => x.TotalInvoiceFee),
                                Count = _data.Count()
                            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get service type list with count in invoice based on booking id list
        /// </summary>
        /// <param name="bookingIdList"></param>
        /// <returns></returns>
        public async Task<List<TurnOverItem>> GetServiceTypeData(List<int> bookingIdList)
        {
            return await (from inv in _context.InvAutTranDetails
                          join servType in _context.InspTranServiceTypes on inv.InspectionId equals servType.InspectionId
                          where bookingIdList.Contains(inv.InspectionId.GetValueOrDefault()) && inv.InvoiceStatus != (int)InvoiceStatus.Cancelled
                          select new
                          {
                              servType.ServiceTypeId,
                              servType.ServiceType.Name,
                          })
                            .GroupBy(p => new { p.ServiceTypeId, p.Name }, (key, _data) => new TurnOverItem
                            {
                                ServicetypeId = key.ServiceTypeId,
                                ServiceTypeName = key.Name,
                                Count = _data.Count()
                            }).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// get service type list with count in invoice based on booking id list
        /// </summary>
        /// <param name="bookingIdList"></param>
        /// <returns></returns>
        public async Task<List<TurnOverItem>> GetQueryableServiceTypeData(IQueryable<int> bookingIdList)
        {
            return await (from inv in _context.InvAutTranDetails
                          join servType in _context.InspTranServiceTypes on inv.InspectionId equals servType.InspectionId
                          where bookingIdList.Contains(inv.InspectionId.GetValueOrDefault()) && inv.InvoiceStatus != (int)InvoiceStatus.Cancelled
                          select new
                          {
                              servType.ServiceTypeId,
                              servType.ServiceType.Name,
                          })
                            .GroupBy(p => new { p.ServiceTypeId, p.Name }, (key, _data) => new TurnOverItem
                            {
                                ServicetypeId = key.ServiceTypeId,
                                ServiceTypeName = key.Name,
                                Count = _data.Count()
                            }).AsNoTracking().ToListAsync();
        }

        //get the turnover data from audit table
        public async Task<List<TurnOverItem>> GetExtraFeeData(List<int> bookingIdList)
        {
            return await (from exf in _context.InvExfTransactions
                          where bookingIdList.Contains(exf.InspectionId.GetValueOrDefault()) && exf.StatusId != (int)ExtraFeeStatus.Cancelled
                          select new TurnOverItem
                          {
                              InvoiceTo = exf.BilledTo,
                              TotalInvoiceFee = exf.TotalExtraFee,
                              Year = exf.Inspection.ServiceDateFrom.Year,
                              CurrencyId = exf.CurrencyId
                          })
                            .GroupBy(p => new { p.InvoiceTo, p.Year, p.CurrencyId }, (key, _data) => new TurnOverItem
                            {
                                InvoiceTo = key.InvoiceTo,
                                TotalInvoiceFee = _data.Sum(x => x.TotalInvoiceFee),
                                Year = key.Year,
                                Count = _data.Count(),
                                CurrencyId = key.CurrencyId
                            }).AsNoTracking().ToListAsync();
        }
        //get the turnover data from audit table
        public async Task<List<TurnOverItem>> GetQueryableExtraFeeData(IQueryable<int> bookingIdList)
        {
            return await (from exf in _context.InvExfTransactions
                          where bookingIdList.Contains(exf.InspectionId.GetValueOrDefault()) && exf.StatusId != (int)ExtraFeeStatus.Cancelled
                          select new TurnOverItem
                          {
                              InvoiceTo = exf.BilledTo,
                              TotalInvoiceFee = exf.TotalExtraFee,
                              Year = exf.Inspection.ServiceDateFrom.Year,
                              CurrencyId = exf.CurrencyId
                          })
                            .GroupBy(p => new { p.InvoiceTo, p.Year, p.CurrencyId }, (key, _data) => new TurnOverItem
                            {
                                InvoiceTo = key.InvoiceTo,
                                TotalInvoiceFee = _data.Sum(x => x.TotalInvoiceFee),
                                Year = key.Year,
                                Count = _data.Count(),
                                CurrencyId = key.CurrencyId
                            }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get order quantity by month wise
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<OrderQtyChartItem>> GetMonthlyInspOrderQuantity(IEnumerable<int> bookingIds)
        {
            return await _context.InspProductTransactions.Where(x => bookingIds.Contains(x.InspectionId))
            .Select(k => new
            { k.Inspection.ServiceDateTo.Year, k.Inspection.ServiceDateTo.Month, k.AqlQuantity, k.CombineAqlQuantity, k.CombineProductId }).
                GroupBy(x => new { x.Year, x.Month }, (key, group) => new OrderQtyChartItem
                {
                    Year = key.Year,
                    Month = key.Month,
                    MonthOrderQuantity = group.Where(k => k.CombineProductId > 0).Sum(k => k.CombineAqlQuantity.GetValueOrDefault()) +
                                    group.Where(k => !(k.CombineProductId > 0)).Sum(k => k.AqlQuantity.GetValueOrDefault()),
                    MonthName = MonthData.GetValueOrDefault(key.Month)
                }).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// get order quantity by month wise
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<OrderQtyChartItem>> GetQueryableMonthlyInspOrderQuantity(IQueryable<int> bookingIds)
        {
            return await _context.InspProductTransactions.Where(x => bookingIds.Contains(x.InspectionId))
            .Select(k => new
            { k.Inspection.ServiceDateTo.Year, k.Inspection.ServiceDateTo.Month, k.AqlQuantity, k.CombineAqlQuantity, k.CombineProductId }).
                GroupBy(x => new { x.Year, x.Month }, (key, group) => new OrderQtyChartItem
                {
                    Year = key.Year,
                    Month = key.Month,
                    MonthOrderQuantity = group.Where(k => k.CombineProductId > 0).Sum(k => k.CombineAqlQuantity.GetValueOrDefault()) +
                                    group.Where(k => k.CombineProductId == null || !(k.CombineProductId > 0)).Sum(k => k.AqlQuantity.GetValueOrDefault()),
                    MonthName = MonthData.GetValueOrDefault(key.Month)
                }).AsNoTracking().ToListAsync();
        }


        /// <summary>
        /// get the product sub category by inspection ids
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        public async Task<List<ProductCategoryDashboardItem>> GetQueryableProductSubCategoryDashboard(IQueryable<int> InspectionIdList, IEnumerable<int> prodCategoryIds)
        {
            return await _context.InspProductTransactions.Where(x => InspectionIdList.Contains(x.Inspection.Id) &&
                                 x.Active.HasValue && x.Active.Value
                                && x.Product.ProductSubCategory > 0 && prodCategoryIds.Contains(x.Product.ProductCategory.Value)).
                                    Select(x => new CommonDataSource()
                                    {
                                        Id = x.Product.ProductSubCategory.GetValueOrDefault(),
                                        Name = x.Product.ProductSubCategoryNavigation.Name
                                    })
                                    .
                                    GroupBy(x => new { x.Id, x.Name }, p => p, (key, _data) =>
                                          new ProductCategoryDashboardItem
                                          {
                                              Id = key.Id,
                                              StatusName = key.Name,
                                              TotalCount = _data.Count()
                                          }).AsNoTracking().OrderByDescending(x => x.TotalCount).ToListAsync();
        }

        /// <summary>
        /// get the product category and product sub category by inspection ids
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        public IQueryable<ProductCategoryDashboardExportItem> GetProductCategoryDashboardExport(List<int> bookingIdList)
        {
            return _context.InspProductTransactions.Where(x => bookingIdList.Contains(x.Inspection.Id) && x.Active.HasValue && x.Active.Value).
                                        Select(x => x.Product).Distinct().
                                        Where(x => x.ProductCategory != null && x.ProductSubCategory != null).
                                        GroupBy(x => new { x.ProductCategory, x.ProductSubCategory }, p => p, (key, _data) =>
                                              new ProductCategoryDashboardExportItem
                                              {
                                                  Id = key.ProductCategory,
                                                  ProductCategoryName = _data.Select(x => x.ProductCategoryNavigation.Name).FirstOrDefault(),
                                                  ProductSubCategoryName = _data.Select(x => x.ProductSubCategoryNavigation.Name).FirstOrDefault(),
                                                  TotalCount = _data.Count()
                                              }).AsNoTracking().OrderBy(x => x.ProductCategoryName);
        }
        /// <summary>
        /// get the product category and product sub category by inspection ids
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        public IQueryable<ProductCategoryDashboardExportItem> GetQueryableProductCategoryDashboardExport(IQueryable<int> bookingIdList)
        {
            return _context.InspProductTransactions.Where(x => bookingIdList.Contains(x.Inspection.Id) && x.Active.HasValue && x.Active.Value
            && x.Product.ProductCategory > 0 && x.Product.ProductSubCategory > 0).
                                        Select(x =>
                                        new ProductCategoryDashboardExportItem
                                        {
                                            Id = x.Product.ProductCategory,
                                            SubCategoryId = x.Product.ProductSubCategory,
                                            ProductCategoryName = x.Product.ProductCategoryNavigation.Name,
                                            ProductSubCategoryName = x.Product.ProductSubCategoryNavigation.Name,
                                        })
                                        .GroupBy(x => new { x.Id, x.SubCategoryId, x.ProductCategoryName, x.ProductSubCategoryName }, p => p, (key, _data) =>
                                              new ProductCategoryDashboardExportItem
                                              {
                                                  Id = key.Id,
                                                  ProductCategoryName = key.ProductCategoryName,
                                                  ProductSubCategoryName = key.ProductSubCategoryName,
                                                  TotalCount = _data.Count()
                                              }).AsNoTracking().OrderBy(x => x.ProductCategoryName);
        }
    }
}
