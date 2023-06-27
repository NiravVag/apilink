using Contracts.Repositories;
using DTO.Common;
using DTO.Dashboard;
using DTO.RejectionDashboard;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class RejectionDashboardRepository : Repository, IRejectionDashboardRepository
    {
        public RejectionDashboardRepository(API_DBContext context) : base(context)
        {
        }

        /// <summary>
        /// Get the Product Category data for the inspections
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        public async Task<List<ChartItem>> GetProductCategoryDashboard(IQueryable<int> inspectionIds, bool isExport, bool isOnlyRejectionResult = false)
        {
            List<ChartItem> data = null;
            var res = _context.InspProductTransactions.
                                    Where(x => x.Active.HasValue && x.Active.Value && x.Inspection.ProductCategoryId > 0 && inspectionIds.Contains(x.Inspection.Id)
                                    && x.FbReport.FbReportStatus == (int)FBStatus.ReportValidated).
                                    Select(x => new
                                    {
                                        ProductCategory = x.Inspection.ProductCategoryId,
                                        x.Inspection.ProductCategory.Name,
                                        x.FbReport.ResultId,
                                        x.FbReport.Result.ResultName,
                                        x.FbReportId
                                    });

            //if not export take top 5, else all the data
            if (!isExport)
            {
                data = await res.Select(x => new { x.ProductCategory, x.FbReportId }).GroupBy(x => x.ProductCategory, p => p, (key, _data) => new ChartItem
                {
                    NewId = key.Value,
                    TotalCount = _data.Select(x => x.FbReportId).Distinct().Count()
                }).OrderByDescending(x => x.TotalCount).Take(5).ToListAsync();

                res = res.Where(x => data.Select(y => y.NewId).Contains(x.ProductCategory.Value));
            }

            if (isOnlyRejectionResult)
            {
                res = res.Where(x => x.ResultId == 2);
            }

            var result = await res
                .Select(y => new { y.ResultId, y.ResultName, y.ProductCategory, y.Name, y.FbReportId })
                .GroupBy(x => new { x.ProductCategory, x.Name, x.ResultId, x.ResultName }, p => p, (key, _data) =>
                                          new ChartItem
                                          {
                                              Id = key.ProductCategory,
                                              Name = key.Name,
                                              ResultId = key.ResultId,
                                              ResultName = key.ResultName,
                                              TotalCount = _data.Select(x => x.FbReportId).Distinct().Count()
                                          }).AsNoTracking().OrderByDescending(x => x.TotalCount).ToListAsync();

            return result;
        }



        /// <summary>
        /// Get the supplier data for the inspections
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        public async Task<List<ChartItem>> GetSupplierDashboard(IQueryable<int> inspectionIds, bool isExport)
        {
            List<ChartItem> data = null;

            var res = _context.FbReportDetails.
                                    Where(x => x.Active.HasValue && x.Active.Value && x.FbReportStatus == (int)FBStatus.ReportValidated && inspectionIds.Contains(x.Inspection.Id));

            //if not export take top 15, else all the data
            if (!isExport)
            {
                data = await res.Select(x => new { x.Inspection.SupplierId, x.Id }).GroupBy(x => x.SupplierId, p => p, (key, _data) => new ChartItem
                {
                    NewId = key,
                    TotalCount = _data.Select(x => x.Id).Distinct().Count()
                }).OrderByDescending(x => x.TotalCount).Take(15).ToListAsync();

                res = res.Where(x => data.Select(y => y.NewId).Contains(x.Inspection.SupplierId));
            }

            return await res
                .Select(y => new { y.ResultId, y.Result.ResultName, y.Inspection.SupplierId, y.Inspection.Supplier.SupplierName, y.Id })
                .GroupBy(x => new { x.SupplierId, x.SupplierName, x.ResultId, x.ResultName }, p => p, (key, _data) =>
                                          new ChartItem
                                          {
                                              Id = key.SupplierId,
                                              Name = key.SupplierName,
                                              ResultId = key.ResultId,
                                              ResultName = key.ResultName,
                                              TotalCount = _data.Select(x => x.Id).Distinct().Count()
                                          }).AsNoTracking().ToListAsync();
        }

        public async Task<List<ChartItem>> GetFactoryDashboard(IQueryable<int> inspectionIds, bool isExport, bool isOnlyRejectionResult = false)
        {
            List<ChartItem> data = null;

            var res = _context.FbReportDetails.
                                    Where(x => x.Active.HasValue && x.Active.Value && x.FbReportStatus == (int)FBStatus.ReportValidated && inspectionIds.Contains(x.Inspection.Id));

            //if not export take top 15, else all the data
            if (!isExport)
            {
                data = await res.Select(x => new { x.Inspection.FactoryId, x.Id }).GroupBy(x => x.FactoryId, p => p, (key, _data) => new ChartItem
                {
                    NewId = key.GetValueOrDefault(),
                    TotalCount = _data.Select(x => x.Id).Distinct().Count()
                }).OrderByDescending(x => x.TotalCount).Take(15).ToListAsync();

                res = res.Where(x => data.Select(y => y.NewId).Contains(x.Inspection.SupplierId));
            }

            if (isOnlyRejectionResult)
            {
                res = res.Where(x => x.ResultId == 2);
            }

            return await res
                .Select(y => new { y.ResultId, y.Result.ResultName, y.Inspection.FactoryId, y.Inspection.Factory.SupplierName, y.Id })
                .GroupBy(x => new { x.FactoryId, x.SupplierName, x.ResultId, x.ResultName }, p => p, (key, _data) =>
                                          new ChartItem
                                          {
                                              Id = key.FactoryId,
                                              Name = key.SupplierName,
                                              ResultId = key.ResultId,
                                              ResultName = key.ResultName,
                                              TotalCount = _data.Select(x => x.Id).Distinct().Count()
                                          }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the report result by country
        /// </summary>
        /// <param name="bookingIdList"></param>
        /// <returns></returns>
        public async Task<List<CountryChartItem>> GetResultByCountry(List<int> bookingIdList)
        {
            var res = _context.FbReportDetails
                   .Join(_context.SuAddresses, prod => prod.Inspection.FactoryId, sup => sup.SupplierId,
                   (prod, sup) => new { FbReportDetails = prod, SuAddress = sup })
                   .Where(x => bookingIdList.Contains(x.FbReportDetails.InspectionId.Value) && x.FbReportDetails.FbReportStatus == (int)FBStatus.ReportValidated)
                   .Select(y => new
                   {
                       y.FbReportDetails.ResultId,
                       y.FbReportDetails.Result.ResultName,
                       y.SuAddress.CountryId,
                       y.SuAddress.Country.CountryName,
                       y.FbReportDetails.Id
                   });

            var data = await res.Select(x => new { x.CountryId, x.Id }).GroupBy(x => x.CountryId, p => p, (key, _data) => new ChartItem
            {
                NewId = key,
                TotalCount = _data.Select(x => x.Id).Distinct().Count()
            }).OrderByDescending(x => x.TotalCount).Take(15).ToListAsync();

            return await res.Where(x => data.Select(y => y.NewId).Contains(x.CountryId))
                .Select(y => new { y.ResultId, y.ResultName, y.CountryId, y.CountryName, y.Id })
                .GroupBy(x => new { x.ResultId, x.ResultName, x.CountryId, x.CountryName }, p => p, (key, _data) =>
                        new CountryChartItem
                        {
                            CountryId = key.CountryId,
                            CountryName = key.CountryName,
                            ResultId = key.ResultId,
                            ResultName = key.ResultName,
                            TotalCount = _data.Select(x => x.Id).Distinct().Count()
                        }).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// Get the report result by country
        /// </summary>
        /// <param name="bookingIdList"></param>
        /// <returns></returns>
        public async Task<List<CountryChartItem>> GetQueryableResultByCountry(IQueryable<int> bookingIdList)
        {
            var res = _context.FbReportDetails
                   .Join(_context.SuAddresses, prod => prod.Inspection.FactoryId, sup => sup.SupplierId,
                   (prod, sup) => new { FbReportDetails = prod, SuAddress = sup })
                   .Where(x => bookingIdList.Contains(x.FbReportDetails.InspectionId.Value) && x.FbReportDetails.FbReportStatus == (int)FBStatus.ReportValidated)
                   .Select(y => new
                   {
                       y.FbReportDetails.ResultId,
                       y.FbReportDetails.Result.ResultName,
                       y.SuAddress.CountryId,
                       y.SuAddress.Country.CountryName,
                       y.FbReportDetails.Id
                   });

            var data = await res.Select(x => new { x.CountryId, x.Id }).GroupBy(x => x.CountryId, p => p, (key, _data) => new ChartItem
            {
                NewId = key,
                TotalCount = _data.Select(x => x.Id).Distinct().Count()
            }).OrderByDescending(x => x.TotalCount).Take(15).ToListAsync();

            return await res.Where(x => data.Select(y => y.NewId).Contains(x.CountryId))
                .Select(y => new { y.ResultId, y.ResultName, y.CountryId, y.CountryName, y.Id })
                .GroupBy(x => new { x.ResultId, x.ResultName, x.CountryId, x.CountryName }, p => p, (key, _data) =>
                        new CountryChartItem
                        {
                            CountryId = key.CountryId,
                            CountryName = key.CountryName,
                            ResultId = key.ResultId,
                            ResultName = key.ResultName,
                            TotalCount = _data.Select(x => x.Id).Distinct().Count()
                        }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the report result by Province
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<CountryChartItem>> GetResultByProvince(IQueryable<int> bookingIdList, int? countryId)
        {
            var res = _context.FbReportDetails
                   .Join(_context.SuAddresses, prod => prod.Inspection.FactoryId, sup => sup.SupplierId,
                   (prod, sup) => new { FbReportDetails = prod, SuAddress = sup })
                   .Where(x => bookingIdList.Contains(x.FbReportDetails.InspectionId.Value) && x.SuAddress.CountryId == countryId
                    && x.FbReportDetails.FbReportStatus == (int)FBStatus.ReportValidated)
                   .Select(y => new
                   {
                       y.FbReportDetails.ResultId,
                       y.FbReportDetails.Result.ResultName,
                       y.SuAddress.CountryId,
                       y.SuAddress.Country.CountryName,
                       y.SuAddress.RegionId,
                       y.SuAddress.Region.ProvinceName,
                       y.FbReportDetails.Id
                   });

            var data = await res.Select(x => new { x.RegionId, x.Id }).GroupBy(x => x.RegionId, p => p, (key, _data) => new ChartItem
            {
                NewId = key,
                TotalCount = _data.Select(x => x.Id).Distinct().Count()
            }).OrderByDescending(x => x.TotalCount).Take(15).ToListAsync();

            return await res.Where(x => data.Select(y => y.NewId).Contains(x.RegionId))
                .Select(y => new { y.ResultId, y.ResultName, y.CountryId, y.CountryName, y.RegionId, y.ProvinceName, y.Id })
                .GroupBy(x => new { x.ResultId, x.ResultName, x.CountryId, x.CountryName, x.RegionId, x.ProvinceName }, p => p, (key, _data) =>
                        new CountryChartItem
                        {
                            CountryId = key.CountryId,
                            CountryName = key.CountryName,
                            ProvinceId = key.RegionId,
                            ProvinceName = key.ProvinceName,
                            ResultId = key.ResultId,
                            ResultName = key.ResultName,
                            TotalCount = _data.Select(x => x.Id).Distinct().Count()
                        }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the report Result by both country and province
        /// </summary>
        /// <param name="bookingIdList"></param>
        /// <returns></returns>
        public IQueryable<CountryChartItem> ExportResultByCountryProvince(List<int> bookingIdList)
        {
            return _context.FbReportDetails
                    .Join(_context.SuAddresses, prod => prod.Inspection.FactoryId, sup => sup.SupplierId,
                    (prod, sup) => new { FbReportDetails = prod, SuAddress = sup })
                    .Where(x => bookingIdList.Contains(x.FbReportDetails.InspectionId.Value) && x.FbReportDetails.FbReportStatus == (int)FBStatus.ReportValidated)
                    .Select(y => new
                    {
                        y.FbReportDetails.ResultId,
                        y.FbReportDetails.Result.ResultName,
                        y.SuAddress.CountryId,
                        y.SuAddress.Country.CountryName,
                        y.SuAddress.RegionId,
                        y.SuAddress.Region.ProvinceName,
                        y.FbReportDetails.Id
                    })
                        .GroupBy(x => new { x.ResultId, x.ResultName, x.CountryId, x.CountryName, x.RegionId, x.ProvinceName }, p => p, (key, _data) =>
                         new CountryChartItem
                         {
                             CountryId = key.CountryId,
                             CountryName = key.CountryName,
                             ProvinceId = key.RegionId,
                             ProvinceName = key.ProvinceName,
                             ResultId = key.ResultId,
                             ResultName = key.ResultName,
                             TotalCount = _data.Select(x => x.Id).Distinct().Count()
                         }).AsNoTracking();
        }

        /// <summary>
        /// Get the report Result by both country and province
        /// </summary>
        /// <param name="bookingIdList"></param>
        /// <returns></returns>
        public IQueryable<CountryChartItem> ExportQueryablesultByCountryProvince(IQueryable<int> bookingIdList)
        {
            return _context.FbReportDetails
                    .Join(_context.SuAddresses, prod => prod.Inspection.FactoryId, sup => sup.SupplierId,
                    (prod, sup) => new { FbReportDetails = prod, SuAddress = sup })
                    .Where(x => bookingIdList.Contains(x.FbReportDetails.InspectionId.Value) && x.FbReportDetails.FbReportStatus == (int)FBStatus.ReportValidated)
                    .Select(y => new
                    {
                        y.FbReportDetails.ResultId,
                        y.FbReportDetails.Result.ResultName,
                        y.SuAddress.CountryId,
                        y.SuAddress.Country.CountryName,
                        y.SuAddress.RegionId,
                        y.SuAddress.Region.ProvinceName,
                        y.FbReportDetails.Id
                    })
                        .GroupBy(x => new { x.ResultId, x.ResultName, x.CountryId, x.CountryName, x.RegionId, x.ProvinceName }, p => p, (key, _data) =>
                         new CountryChartItem
                         {
                             CountryId = key.CountryId,
                             CountryName = key.CountryName,
                             ProvinceId = key.RegionId,
                             ProvinceName = key.ProvinceName,
                             ResultId = key.ResultId,
                             ResultName = key.ResultName,
                             TotalCount = _data.Select(x => x.Id).Distinct().Count()
                         }).AsNoTracking();
        }

        /// <summary>
        /// Get the inspection rejected  details (take only failed results from summary)
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <param name="inspectedStatusIds"></param>
        /// <returns></returns>
        public async Task<List<RejectChartMonthItem>> GetCustomerReportReject(IQueryable<int> bookingIdList, int fbResultId)
        {
            var mainTypeId = (int)InspSummaryType.Main;
            int reportValidatedStatusId = (int)FBStatus.ReportValidated;

            return await _context.FbReportInspSummaries
                        .Where(x => bookingIdList.Contains(x.FbReportDetail.InspectionId.Value)
                        && x.FbReportDetail.ResultId == fbResultId &&
                        x.FbReportDetail.Active.HasValue && x.FbReportDetail.Active.Value
                        && x.FbReportInspsumTypeId == mainTypeId
                         && x.FbReportDetail.FbReportStatus == reportValidatedStatusId && x.Active.Value && x.ResultId == fbResultId)
                          .Select(y => new RejectChartMonthItem
                          {
                              Name = y.Name,
                              Year = y.FbReportDetail.Inspection.ServiceDateTo.Year,
                              FbReportDetailId = y.FbReportDetailId,
                              Month = y.FbReportDetail.Inspection.ServiceDateTo.Month
                          })
                          .AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// Get the inspection rejected  details (take only failed results from summary)
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <param name="inspectedStatusIds"></param>
        /// <returns></returns>
        public async Task<List<RejectChartMonthItem>> GetCustomerReportRejectSubcatogory(IQueryable<int> bookingIdList, RejectChartSubcatogoryRequest request)
        {
            var mainTypeId = (int)InspSummaryType.Main;
            int reportValidatedStatusId = (int)FBStatus.ReportValidated;

            //TODO-REMOVE: for checking only fail and pending for FB_Report_Problematic_Remarks,bcz don't have the result id
            var filterResult = request.FbResultId == (int)FBReportResult.Fail ? FBReportResult.Fail.ToString().ToLower() : FBReportResult.Pending.ToString().ToLower();

            return await _context.FbReportInspSummaries
                        .Join(_context.FbReportProblematicRemarks, summary => summary.Id, probSummary => probSummary.FbReportSummaryId,
                        (summary, probSummary) => new { FbReportInspSummaries = summary, FbReportProblematicRemarks = probSummary })
                        .Where(x => request.ResultNames.Contains(x.FbReportInspSummaries.Name)
                        && x.FbReportProblematicRemarks.SubCategory != null && x.FbReportProblematicRemarks.Active.HasValue && x.FbReportProblematicRemarks.Active.Value
                        && x.FbReportInspSummaries.FbReportDetail.ResultId == request.FbResultId
                          && x.FbReportInspSummaries.FbReportDetail.Active.HasValue && x.FbReportInspSummaries.FbReportDetail.Active.Value
                        && x.FbReportInspSummaries.FbReportInspsumTypeId == mainTypeId
                        && x.FbReportInspSummaries.FbReportDetail.FbReportStatus == reportValidatedStatusId
                        && bookingIdList.Contains(x.FbReportInspSummaries.FbReportDetail.Inspection.Id)
                        && x.FbReportInspSummaries.ResultId == request.FbResultId && x.FbReportInspSummaries.Active == true && x.FbReportProblematicRemarks.Result.ToLower() == filterResult)
                .Select(y => new RejectChartMonthItem
                {
                    ReasonName = y.FbReportInspSummaries.Name,
                    Name = y.FbReportProblematicRemarks.SubCategory,
                    Month = y.FbReportInspSummaries.FbReportDetail.Inspection.ServiceDateTo.Month,
                    Year = y.FbReportInspSummaries.FbReportDetail.Inspection.ServiceDateTo.Year,
                    FbReportDetailId = y.FbReportProblematicRemarks.Id
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the inspection rejected  details (take only failed results from summary)
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <param name="inspectedStatusIds"></param>
        /// <returns></returns>
        public async Task<List<RejectChartMonthItem>> GetCustomerReportRejectSubcatogory2(IQueryable<int> bookingIdList, RejectChartSubcatogory2Request request)
        {
            var mainTypeId = (int)InspSummaryType.Main;
            int reportValidatedStatusId = (int)FBStatus.ReportValidated;

            //for checking only fail and pending for FB_Report_Problematic_Remarks,bcz don't have the result id
            var filterResult = request.FbResultId == (int)FBReportResult.Fail ? FBReportResult.Fail.ToString().ToLower() : FBReportResult.Pending.ToString().ToLower();

            return await _context.FbReportInspSummaries
                        .Join(_context.FbReportProblematicRemarks, summary => summary.Id, probSummary => probSummary.FbReportSummaryId,
                        (summary, probSummary) => new { FbReportInspSummaries = summary, FbReportProblematicRemarks = probSummary })
                        .Where(x => request.ResultNames.Contains(x.FbReportInspSummaries.Name) && request.SubCatogory.Contains(x.FbReportProblematicRemarks.SubCategory)
                         && x.FbReportProblematicRemarks.SubCategory != null && x.FbReportProblematicRemarks.Active.HasValue && x.FbReportProblematicRemarks.Active.Value && x.FbReportProblematicRemarks.SubCategory2 != null
                         && x.FbReportInspSummaries.FbReportDetail.ResultId == request.FbResultId
                           && x.FbReportInspSummaries.FbReportDetail.Active.HasValue && x.FbReportInspSummaries.FbReportDetail.Active.Value
                         && x.FbReportInspSummaries.FbReportInspsumTypeId == mainTypeId
                         && x.FbReportInspSummaries.FbReportDetail.FbReportStatus == reportValidatedStatusId
                         && bookingIdList.Contains(x.FbReportInspSummaries.FbReportDetail.Inspection.Id)
                         && x.FbReportInspSummaries.ResultId == request.FbResultId && x.FbReportInspSummaries.Active == true && x.FbReportProblematicRemarks.Result.ToLower() == filterResult)
                 .Select(y => new RejectChartMonthItem
                 {
                     ReasonName = y.FbReportInspSummaries.Name,
                     Subcatogory = y.FbReportProblematicRemarks.SubCategory,
                     Name = y.FbReportProblematicRemarks.SubCategory2,
                     Month = y.FbReportInspSummaries.FbReportDetail.Inspection.ServiceDateTo.Month,
                     Year = y.FbReportInspSummaries.FbReportDetail.Inspection.ServiceDateTo.Year,
                     FbReportDetailId = y.FbReportProblematicRemarks.Id
                 }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the inspection rejected details by supplier and factory (take only failed results from summary)
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <param name="inspectedStatusIds"></param>
        /// <returns></returns>
        public async Task<List<RejectionFactoryData>> GetCustomerReportRejectPopUpData(IQueryable<int> bookingIdList, string rejectReasonName, int fbReportResultId)
        {
            var mainTypeId = (int)InspSummaryType.Main;
            int reportValidatedStatusId = (int)FBStatus.ReportValidated;

            var tempinsplst = await (from sum in _context.FbReportInspSummaries
                                     join photo in _context.FbReportInspSummaryPhotos on sum.Id equals photo.FbReportSummaryId
                                     where (bookingIdList.Contains(sum.FbReportDetail.InspectionId.Value) && sum.FbReportDetail.ResultId == fbReportResultId &&
                                    sum.FbReportDetail.Active.HasValue && sum.FbReportDetail.Active.Value &&
                                    photo.Active.Value && sum.Active.HasValue && sum.Active.Value && sum.ResultId == fbReportResultId && sum.Name == rejectReasonName &&
                                    sum.FbReportInspsumTypeId == mainTypeId && sum.FbReportDetail.FbReportStatus == reportValidatedStatusId)
                                     select sum.FbReportDetail.InspectionId).Distinct().ToListAsync();


            return await _context.FbReportInspSummaries
                        .Where(x => bookingIdList.Contains(x.FbReportDetail.InspectionId.Value)
                        && x.FbReportDetail.ResultId == fbReportResultId &&
                        x.FbReportDetail.Active.HasValue && x.FbReportDetail.Active.Value
                        && x.ResultId == fbReportResultId && x.Active == true
                        && x.Name == rejectReasonName && x.FbReportInspsumTypeId == mainTypeId)
                        .Select(y => new
                        {
                            Month = y.FbReportDetail.Inspection.ServiceDateTo.Month,
                            SupId = y.FbReportDetail.Inspection.SupplierId,
                            SupName = y.FbReportDetail.Inspection.Supplier.SupplierName,
                            FactId = y.FbReportDetail.Inspection.FactoryId,
                            FactName = y.FbReportDetail.Inspection.Factory.SupplierName,
                            ReportId = y.FbReportDetailId,
                            BookingId = y.FbReportDetail.InspectionId,
                            PhotoAvailable = tempinsplst.Contains(y.FbReportDetail.InspectionId.Value) ? 1 : 0
                        })
                        .GroupBy(p => new { p.SupId, p.SupName, p.FactId, p.FactName, p.Month }, (key, _data) => new RejectionFactoryData
                        {
                            SupplierId = key.SupId,
                            SupplierName = key.SupName,
                            FactoryId = key.FactId,
                            FactoryName = key.FactName,
                            RejectionCount = _data.Select(x => x.ReportId).Distinct().Count(),
                            ReportCount = _data.Select(x => x.ReportId).Distinct().Count(),
                            BookingCount = _data.Select(x => x.BookingId).Distinct().Count(),
                            PhotoCount = _data.Sum(x => x.PhotoAvailable),
                            Month = key.Month
                        }).AsNoTracking().OrderByDescending(c => c.RejectionCount).ToListAsync();
        }
        /// <summary>
        /// Get the inspection rejected details by supplier and factory (take only failed results from summary)
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <param name="inspectedStatusIds"></param>
        /// <returns></returns>
        public async Task<List<RejectionFactoryData>> GetCustomerReportSubcatogoryPopUpData(IQueryable<int> bookingIdList, string rejectReasonName, int fbReportResultId, List<string> summaryNames)
        {
            var mainTypeId = (int)InspSummaryType.Main;
            int reportValidatedStatusId = (int)FBStatus.ReportValidated;

            //TODO-REMOVE: for checking only fail and pending for FB_Report_Problematic_Remarks,bcz don't have the result id
            var filterResult = fbReportResultId == (int)FBReportResult.Fail ? FBReportResult.Fail.ToString().ToLower() : FBReportResult.Pending.ToString().ToLower();

            var tempinsplst = await (from sum in _context.FbReportInspSummaries
                                     join photo in _context.FbReportInspSummaryPhotos on sum.Id equals photo.FbReportSummaryId
                                     where (bookingIdList.Contains(sum.FbReportDetail.InspectionId.Value) && sum.FbReportDetail.ResultId == fbReportResultId &&
                                    sum.FbReportDetail.Active.HasValue && sum.FbReportDetail.Active.Value &&
                                    photo.Active.Value && sum.Active.HasValue && sum.Active.Value && sum.ResultId == fbReportResultId && summaryNames.Contains(sum.Name) &&
                                    sum.FbReportInspsumTypeId == mainTypeId && sum.FbReportDetail.FbReportStatus == reportValidatedStatusId)
                                     select sum.FbReportDetail.InspectionId).Distinct().ToListAsync();


            return await _context.FbReportInspSummaries
                        .Join(_context.FbReportProblematicRemarks, summary => summary.Id, probSummary => probSummary.FbReportSummaryId,
                        (summary, probSummary) => new { FbReportInspSummaries = summary, FbReportProblematicRemarks = probSummary })
                        .Where(x => bookingIdList.Contains(x.FbReportInspSummaries.FbReportDetail.InspectionId.Value)
                        && x.FbReportInspSummaries.FbReportDetail.ResultId == fbReportResultId &&
                        x.FbReportInspSummaries.FbReportDetail.Active.HasValue && x.FbReportInspSummaries.FbReportDetail.Active.Value &&
                        x.FbReportInspSummaries.ResultId == fbReportResultId && x.FbReportInspSummaries.Active == true
                        && summaryNames.Contains(x.FbReportInspSummaries.Name) && x.FbReportInspSummaries.FbReportInspsumTypeId == mainTypeId
                        && x.FbReportProblematicRemarks.SubCategory == rejectReasonName && x.FbReportProblematicRemarks.Result.ToLower() == filterResult)
                        .Select(y => new
                        {
                            Month = y.FbReportInspSummaries.FbReportDetail.Inspection.ServiceDateTo.Month,
                            SupId = y.FbReportInspSummaries.FbReportDetail.Inspection.SupplierId,
                            SupName = y.FbReportInspSummaries.FbReportDetail.Inspection.Supplier.SupplierName,
                            FactId = y.FbReportInspSummaries.FbReportDetail.Inspection.FactoryId,
                            FactName = y.FbReportInspSummaries.FbReportDetail.Inspection.Factory.SupplierName,
                            ReportId = y.FbReportInspSummaries.FbReportDetailId,
                            Id = y.FbReportProblematicRemarks.Id,
                            BookingId = y.FbReportInspSummaries.FbReportDetail.InspectionId,
                            PhotoAvailable = tempinsplst.Contains(y.FbReportInspSummaries.FbReportDetail.InspectionId.Value) ? 1 : 0
                        })
                        .GroupBy(p => new { p.SupId, p.SupName, p.FactId, p.FactName, p.Month }, (key, _data) => new RejectionFactoryData
                        {
                            SupplierId = key.SupId,
                            SupplierName = key.SupName,
                            FactoryId = key.FactId,
                            FactoryName = key.FactName,
                            RejectionCount = _data.Select(x => x.Id).Distinct().Count(),
                            ReportCount = _data.Select(x => x.ReportId).Distinct().Count(),
                            BookingCount = _data.Select(x => x.BookingId).Distinct().Count(),
                            PhotoCount = _data.Sum(x => x.PhotoAvailable),
                            Month = key.Month
                        }).AsNoTracking().OrderByDescending(c => c.RejectionCount).ToListAsync();
        }
        /// <summary>
        /// Get the inspection rejected details by supplier and factory (take only failed results from summary)
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <param name="inspectedStatusIds"></param>
        /// <returns></returns>
        public async Task<List<RejectionFactoryData>> GetCustomerReportSubcatogory2PopUpData(IQueryable<int> bookingIdList, string rejectReasonName, int fbReportResultId, List<string> summaryNames, List<string> subcatogory)
        {
            var mainTypeId = (int)InspSummaryType.Main;
            int reportValidatedStatusId = (int)FBStatus.ReportValidated;

            //TODO-REMOVE: for checking only fail and pending for FB_Report_Problematic_Remarks,bcz don't have the result id
            var filterResult = fbReportResultId == (int)FBReportResult.Fail ? FBReportResult.Fail.ToString().ToLower() : FBReportResult.Pending.ToString().ToLower();

            var tempinsplst = await (from sum in _context.FbReportInspSummaries
                                     join photo in _context.FbReportInspSummaryPhotos on sum.Id equals photo.FbReportSummaryId
                                     where (bookingIdList.Contains(sum.FbReportDetail.InspectionId.Value) && sum.FbReportDetail.ResultId == fbReportResultId &&
                                    sum.FbReportDetail.Active.HasValue && sum.FbReportDetail.Active.Value &&
                                    photo.Active.Value && sum.Active.HasValue && sum.Active.Value && sum.ResultId == fbReportResultId && summaryNames.Contains(sum.Name) &&
                                    sum.FbReportInspsumTypeId == mainTypeId && sum.FbReportDetail.FbReportStatus == reportValidatedStatusId)
                                     select sum.FbReportDetail.InspectionId).Distinct().ToListAsync();


            return await _context.FbReportInspSummaries
                        .Join(_context.FbReportProblematicRemarks, summary => summary.Id, probSummary => probSummary.FbReportSummaryId,
                        (summary, probSummary) => new { FbReportInspSummaries = summary, FbReportProblematicRemarks = probSummary })
                        .Where(x => bookingIdList.Contains(x.FbReportInspSummaries.FbReportDetail.InspectionId.Value)
                        && x.FbReportInspSummaries.FbReportDetail.ResultId == fbReportResultId &&
                        x.FbReportInspSummaries.FbReportDetail.Active.HasValue && x.FbReportInspSummaries.FbReportDetail.Active.Value &&
                        x.FbReportInspSummaries.ResultId == fbReportResultId && x.FbReportInspSummaries.Active == true
                        && summaryNames.Contains(x.FbReportInspSummaries.Name) && x.FbReportInspSummaries.FbReportInspsumTypeId == mainTypeId
                        && x.FbReportProblematicRemarks.SubCategory2 == rejectReasonName && subcatogory.Contains(x.FbReportProblematicRemarks.SubCategory) && x.FbReportProblematicRemarks.Result.ToLower() == filterResult)
                        .Select(y => new
                        {
                            Month = y.FbReportInspSummaries.FbReportDetail.Inspection.ServiceDateTo.Month,
                            SupId = y.FbReportInspSummaries.FbReportDetail.Inspection.SupplierId,
                            SupName = y.FbReportInspSummaries.FbReportDetail.Inspection.Supplier.SupplierName,
                            FactId = y.FbReportInspSummaries.FbReportDetail.Inspection.FactoryId,
                            FactName = y.FbReportInspSummaries.FbReportDetail.Inspection.Factory.SupplierName,
                            ReportId = y.FbReportInspSummaries.FbReportDetailId,
                            Id = y.FbReportProblematicRemarks.Id,
                            BookingId = y.FbReportInspSummaries.FbReportDetail.InspectionId,
                            PhotoAvailable = tempinsplst.Contains(y.FbReportInspSummaries.FbReportDetail.InspectionId.Value) ? 1 : 0
                        })
                        .GroupBy(p => new { p.SupId, p.SupName, p.FactId, p.FactName, p.Month }, (key, _data) => new RejectionFactoryData
                        {
                            SupplierId = key.SupId,
                            SupplierName = key.SupName,
                            FactoryId = key.FactId,
                            FactoryName = key.FactName,
                            RejectionCount = _data.Select(x => x.Id).Distinct().Count(),
                            ReportCount = _data.Select(x => x.ReportId).Distinct().Count(),
                            BookingCount = _data.Select(x => x.BookingId).Distinct().Count(),
                            PhotoCount = _data.Sum(x => x.PhotoAvailable),
                            Month = key.Month
                        }).AsNoTracking().OrderByDescending(c => c.RejectionCount).ToListAsync();
        }
        /// <summary>
        /// Get the reportno
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <param name="rejectReasonName"></param>
        /// <returns></returns>
        public async Task<List<RejectionReportData>> GetReportByInspectionIds(IQueryable<int> bookingIdList, string rejectReasonName, int fbReportResultId)
        {
            return await (from sum in _context.FbReportInspSummaries
                          where (bookingIdList.Contains(sum.FbReportDetail.InspectionId.Value) && sum.FbReportDetail.ResultId == fbReportResultId &&
                         sum.FbReportDetail.Active.HasValue && sum.FbReportDetail.Active.Value &&
                         sum.FbReportDetail.FinalReportPath != null && sum.Active.HasValue && sum.Active.Value && sum.ResultId == fbReportResultId && sum.Name == rejectReasonName &&
                         sum.FbReportInspsumTypeId == (int)InspSummaryType.Main && sum.FbReportDetail.FbReportStatus == (int)FBStatus.ReportValidated)
                          select new RejectionReportData
                          {
                              ReportNo = sum.FbReportDetail.ReportTitle,
                              ReportLink = sum.FbReportDetail.FinalReportPath,
                              FinalManualReportLink = sum.FbReportDetail.FinalManualReportPath,
                              FactoryId = sum.FbReportDetail.Inspection.FactoryId,
                              SupplierId = sum.FbReportDetail.Inspection.SupplierId
                          }).Distinct().AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// Get the reportno
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <param name="rejectReasonName"></param>
        /// <returns></returns>
        public async Task<List<RejectionReportData>> GetSubcatogoryReportByInspectionIds(IQueryable<int> bookingIdList, string rejectReasonName, int fbReportResultId, List<string> summaryNames)
        {
            return await _context.FbReportProblematicRemarks.Where(x => x.SubCategory == rejectReasonName
                         && x.SubCategory != null && x.Active.HasValue && x.Active.Value
                         && x.FbReportSummary.FbReportDetail.ResultId == fbReportResultId
                           && x.FbReportSummary.FbReportDetail.Active.HasValue && x.FbReportSummary.FbReportDetail.Active.Value
                         && x.FbReportSummary.FbReportInspsumTypeId == (int)InspSummaryType.Main
                         && x.FbReportSummary.FbReportDetail.FbReportStatus == (int)FBStatus.ReportValidated
                         && bookingIdList.Contains(x.FbReportSummary.FbReportDetail.Inspection.Id)
                          && summaryNames.Contains(x.FbReportSummary.Name)
                         && x.FbReportSummary.ResultId == fbReportResultId && x.FbReportSummary.Active == true)
                         .Select(y => new RejectionReportData
                         {
                             ReportNo = y.FbReportSummary.FbReportDetail.ReportTitle,
                             ReportLink = y.FbReportSummary.FbReportDetail.FinalReportPath,
                             FinalManualReportLink = y.FbReportSummary.FbReportDetail.FinalManualReportPath,
                             FactoryId = y.FbReportSummary.FbReportDetail.Inspection.FactoryId,
                             SupplierId = y.FbReportSummary.FbReportDetail.Inspection.SupplierId
                         }).Distinct().AsNoTracking().ToListAsync();


        }
        /// <summary>
        /// Get the reportno
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <param name="rejectReasonName"></param>
        /// <returns></returns>
        public async Task<List<RejectionReportData>> GetSubcatogory2ReportByInspectionIds(IQueryable<int> bookingIdList, string rejectReasonName, int fbReportResultId, List<string> summaryNames, List<string> subcatogory)
        {
            return await _context.FbReportProblematicRemarks.Where(x => x.SubCategory2 == rejectReasonName
                       && x.SubCategory != null && x.Active.HasValue && x.Active.Value
                       && x.FbReportSummary.FbReportDetail.ResultId == fbReportResultId
                         && x.FbReportSummary.FbReportDetail.Active.HasValue && x.FbReportSummary.FbReportDetail.Active.Value
                       && x.FbReportSummary.FbReportInspsumTypeId == (int)InspSummaryType.Main
                       && x.FbReportSummary.FbReportDetail.FbReportStatus == (int)FBStatus.ReportValidated
                       && bookingIdList.Contains(x.FbReportSummary.FbReportDetail.Inspection.Id)
                        && summaryNames.Contains(x.FbReportSummary.Name)
                        && subcatogory.Contains(x.SubCategory)
                       && x.FbReportSummary.ResultId == fbReportResultId && x.FbReportSummary.Active == true)
                         .Select(y => new RejectionReportData
                         {
                             ReportNo = y.FbReportSummary.FbReportDetail.ReportTitle,
                             ReportLink = y.FbReportSummary.FbReportDetail.FinalReportPath,
                             FinalManualReportLink = y.FbReportSummary.FbReportDetail.FinalManualReportPath,
                             FactoryId = y.FbReportSummary.FbReportDetail.Inspection.FactoryId,
                             SupplierId = y.FbReportSummary.FbReportDetail.Inspection.SupplierId
                         }).Distinct().AsNoTracking().ToListAsync();


        }


        /// <summary>
        /// Get the rejection photos
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <param name="inspectedStatusIds"></param>
        /// <returns></returns>
        public async Task<List<RejectionImageResult>> GetReportRejectImageData(IQueryable<int> bookingIdList, List<string> rejectReasonName, int fbReportResultId)
        {
            return await (from fbsummary in _context.FbReportInspSummaries
                          join fbPhotos in _context.FbReportInspSummaryPhotos on fbsummary.Id equals fbPhotos.FbReportSummaryId
                          where bookingIdList.Contains(fbsummary.FbReportDetail.InspectionId.Value) && rejectReasonName.Contains(fbsummary.Name) && fbsummary.FbReportDetail.ResultId == fbReportResultId && fbsummary.ResultId == fbReportResultId
                          && fbsummary.FbReportDetail.Active == true && fbsummary.Active == true && fbPhotos.Active.Value && fbsummary.FbReportInspsumTypeId == (int)InspSummaryType.Main
                           && fbsummary.FbReportDetail.FbReportStatus == (int)FBStatus.ReportValidated
                          select new RejectionImageResult
                          {
                              ImagePath = fbPhotos.Photo,
                              Description = fbPhotos.Description
                          }).Distinct().AsNoTracking().ToListAsync();

        }

        /// <summary>
        /// Get the inspection rejected  details by subcategory2, subcategory and fbReportInspSummary Name
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <param name="inspectedStatusIds"></param>
        /// <returns></returns>
        public IQueryable<ExportRejectionTableRepoData> ExportCustomerReportRejectSubcatogory2(IQueryable<int> bookingIdList, RejectChartSubcatogory2Request request)
        {
            var mainTypeId = (int)InspSummaryType.Main;
            int reportValidatedStatusId = (int)FBStatus.ReportValidated;

            return (from summary in _context.FbReportInspSummaries
                    join probSummary in _context.FbReportProblematicRemarks.Where(x => x.Active.Value) on summary.Id equals probSummary.FbReportSummaryId into data
                    from prob in data.DefaultIfEmpty()
                    where (summary.FbReportDetail.ResultId == request.FbResultId
                               && summary.FbReportDetail.Active.HasValue && summary.FbReportDetail.Active.Value
                             && summary.FbReportInspsumTypeId == mainTypeId
                             && summary.FbReportDetail.FbReportStatus == reportValidatedStatusId
                             && bookingIdList.Contains(summary.FbReportDetail.Inspection.Id)
                             && summary.ResultId == request.FbResultId && summary.Active == true)
                    select new ExportRejectionTableRepoData
                    {
                        ReasonName = summary.Name,
                        SubCategory = prob.SubCategory,
                        SubCategory2 = prob.SubCategory2,
                        Month = summary.FbReportDetail.Inspection.ServiceDateTo.Month,
                        Year = summary.FbReportDetail.Inspection.ServiceDateTo.Year,
                        ReportId = summary.FbReportDetailId
                    });
        }

        public IQueryable<RejectionRateData> GetQueryableReportRejectionRate(IQueryable<int> bookingIdList)
        {
            var data = from fbreport in _context.FbReportDetails
                       join cubrand in _context.InspTranCuBrands.Where(x => x.Active) on fbreport.InspectionId.GetValueOrDefault() equals cubrand.InspectionId
                       join suaddress in _context.SuAddresses on fbreport.Inspection.FactoryId equals suaddress.SupplierId
                       where fbreport.Active.HasValue && fbreport.Active.Value && suaddress.AddressTypeId == (int)Supplier_Address_Type.HeadOffice
                       && bookingIdList.Contains(fbreport.InspectionId.GetValueOrDefault())
                       select new RejectionRateData
                       {
                           FactoryCountryId = suaddress.CountryId,
                           FactoryCountryName = suaddress.Country.CountryName,
                           FactoryId = fbreport.Inspection.FactoryId.GetValueOrDefault(),
                           FactoryName = fbreport.Inspection.Factory.SupplierName,
                           SupplierId = fbreport.Inspection.SupplierId,
                           SupplierName = fbreport.Inspection.Supplier.SupplierName,
                           BrandId = cubrand.BrandId,
                           BrandName = cubrand.Brand.Name,
                           InspectionId = fbreport.InspectionId,
                           PresentedQty = fbreport.PresentedQty,
                           InspectedQty = fbreport.InspectedQty,
                           OrderQty = fbreport.OrderQty,
                           ReportId = fbreport.Id,
                           ResultId = fbreport.ResultId,
                           ResultName = fbreport.Result.ResultName
                       };
            return data;
        }

        public IQueryable<RejectionRateData> GetQueryableCusDecisionRejectionRate(IQueryable<int> bookingIdList)
        {
            var data = from fbreport in _context.FbReportDetails
                       join cubrand in _context.InspTranCuBrands.Where(x => x.Active) on fbreport.InspectionId.GetValueOrDefault() equals cubrand.InspectionId
                       join suaddress in _context.SuAddresses on fbreport.Inspection.FactoryId equals suaddress.SupplierId
                       join cusdec in _context.InspRepCusDecisions.Where(x => x.Active.HasValue && x.Active.Value) on fbreport.Id equals cusdec.ReportId
                       where fbreport.Active.HasValue && fbreport.Active.Value && suaddress.AddressTypeId == (int)Supplier_Address_Type.HeadOffice
                       && bookingIdList.Contains(fbreport.InspectionId.GetValueOrDefault())
                       select new RejectionRateData
                       {
                           FactoryCountryId = suaddress.CountryId,
                           FactoryCountryName = suaddress.Country.CountryName,
                           FactoryId = fbreport.Inspection.FactoryId.GetValueOrDefault(),
                           FactoryName = fbreport.Inspection.Factory.SupplierName,
                           SupplierId = fbreport.Inspection.SupplierId,
                           SupplierName = fbreport.Inspection.Supplier.SupplierName,
                           BrandId = cubrand.BrandId,
                           BrandName = cubrand.Brand.Name,
                           InspectionId = fbreport.InspectionId,
                           PresentedQty = fbreport.PresentedQty,
                           InspectedQty = fbreport.InspectedQty,
                           OrderQty = fbreport.OrderQty,
                           ReportId = cusdec.ReportId,
                           CusDecisionId = cusdec.Id,
                           CustomerResultId = cusdec.CustomerResultId,
                           CustomDecisionName = cusdec.CustomerResult.CustomDecisionName
                       };
            return data;
        }
    }
}
