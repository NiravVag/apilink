using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.DefectDashboard;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class DefectDashboardRepository : Repository, IDefectDashboardRepository
    {
        public DefectDashboardRepository(API_DBContext context) : base(context)
        {

        }

        /// <summary>
        /// get defects category list
        /// </summary>
        /// <param name="reportIdList"></param>
        /// <returns></returns>
        public async Task<List<DefectCategoryModel>> GetDefectCategoryList(IEnumerable<int> reportIdList)
        {
            return await _context.FbReportInspDefects.Where(x => !string.IsNullOrWhiteSpace(x.CategoryName) && x.Active.Value &&
                    reportIdList.Contains(x.FbReportDetailId))
                    .Select(z => new
                    {
                        z.CategoryName
                    })
               .GroupBy(g => new { g.CategoryName }, (key, _data) => new DefectCategoryModel
               {
                   CategoryName = key.CategoryName,
                   DefectCountByCategory = _data.Count(),
               }).AsNoTracking().OrderByDescending(x => x.DefectCountByCategory).ToListAsync();
        }
        /// <summary>
        /// get defects category list
        /// </summary>
        /// <param name="reportIdList"></param>
        /// <returns></returns>
        public async Task<List<DefectCategoryModel>> GetDefectCategoryQueryableList(IQueryable<int> reportIdList)
        {
            return await _context.FbReportInspDefects.Where(x => !string.IsNullOrWhiteSpace(x.CategoryName) && x.Active.Value &&
                    reportIdList.Contains(x.FbReportDetailId))
                    .Select(z => new
                    {
                        z.CategoryName
                    })
               .GroupBy(g => new { g.CategoryName }, (key, _data) => new DefectCategoryModel
               {
                   CategoryName = key.CategoryName,
                   DefectCountByCategory = _data.Count(),
               }).AsNoTracking().OrderByDescending(x => x.DefectCountByCategory).ToListAsync();
        }

        /// <summary>
        /// get defect list
        /// </summary>
        /// <param name="reportIds"></param>
        /// <returns></returns>
        public async Task<List<DefectReportRepo>> GetReportDefectsList(IEnumerable<int> reportIds)
        {
            return await _context.FbReportInspDefects.
             Where(x => reportIds.Contains(x.FbReportDetailId) && x.Active.Value &&
             (x.Minor > 0 || x.Major > 0 || x.Critical > 0))
            .Select(x => new DefectReportRepo
            {
                Minor = x.Minor.GetValueOrDefault(),
                Major = x.Major.GetValueOrDefault(),
                Critical = x.Critical.GetValueOrDefault(),
                ReportId = x.FbReportDetailId
            }).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// get defect list
        /// </summary>
        /// <param name="reportIds"></param>
        /// <returns></returns>
        public async Task<List<DefectReportRepo>> GetReportDefectsQueryableList(IQueryable<int> reportIds)
        {
            return await _context.FbReportInspDefects.
             Where(x => reportIds.Contains(x.FbReportDetailId) && x.Active.Value &&
             (x.Minor > 0 || x.Major > 0 || x.Critical > 0))
            .Select(x => new DefectReportRepo
            {
                Minor = x.Minor.GetValueOrDefault(),
                Major = x.Major.GetValueOrDefault(),
                Critical = x.Critical.GetValueOrDefault(),
                ReportId = x.FbReportDetailId
            }).AsNoTracking().ToListAsync();
        }



        /// <summary>
        /// get inspection container details with report id by report id list
        /// </summary>
        /// <param name="reportIds"></param>
        /// <returns></returns>
        public async Task<List<InspectionMonthRepo>> GetInspectionContainerDefectsList(IEnumerable<int> reportIds)
        {
            return await _context.InspContainerTransactions.
             Where(x => reportIds.Contains(x.FbReportId.GetValueOrDefault()) && x.Active.Value)
            .Select(x => new InspectionMonthRepo
            {
                Year = x.Inspection.ServiceDateTo.Year,
                Month = x.Inspection.ServiceDateTo.Month,
                ReportId = x.FbReportId.GetValueOrDefault()
            }).Distinct().AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// get inspection container details with report id by report id list
        /// </summary>
        /// <param name="reportIds"></param>
        /// <returns></returns>
        public async Task<List<InspectionMonthRepo>> GetInspectionDefectsList(IQueryable<int> reportIds)
        {
            return await _context.FbReportDetails.
             Where(x => reportIds.Contains(x.Id) && x.Active.Value)
            .Select(x => new InspectionMonthRepo
            {
                Year = x.Inspection.ServiceDateTo.Year,
                Month = x.Inspection.ServiceDateTo.Month,
                ReportId = x.Id
            }).Distinct().AsNoTracking().ToListAsync();
        }


        /// <summary>
        /// get inspection product details with report id by report id list
        /// </summary>
        /// <param name="reportIds"></param>
        /// <returns></returns>
        public async Task<List<InspectionMonthRepo>> GetInspectionProductDefectsList(IEnumerable<int> reportIds)
        {
            return await _context.InspProductTransactions.
             Where(x => reportIds.Contains(x.FbReportId.GetValueOrDefault()) && x.Active.Value)
            .Select(x => new InspectionMonthRepo
            {
                Year = x.Inspection.ServiceDateTo.Year,
                Month = x.Inspection.ServiceDateTo.Month,
                ReportId = x.FbReportId.GetValueOrDefault()
            }).Distinct().AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get defect total list by report id list
        /// </summary>
        /// <param name="reportIds"></param>
        /// <returns></returns>
        public IQueryable<ParetoDefectRepo> GetTotalDefectList(IEnumerable<int> reportIds)
        {
            return _context.FbReportInspDefects.Where(y => y.FbReportDetailId > 0 && reportIds.Contains(y.FbReportDetailId) &&
            !string.IsNullOrWhiteSpace(y.Description)
            && y.Active.Value && (y.Minor > 0 || y.Major > 0 || y.Critical > 0))
            .Select(z => new ParetoDefectRepo
            {
                DefectName = z.Description,
                Critical = z.Critical,
                Major = z.Major,
                Minor = z.Minor,
            }).AsNoTracking();
        }
        /// <summary>
        /// get defect total list by report id list
        /// </summary>
        /// <param name="reportIds"></param>
        /// <returns></returns>
        public IQueryable<ParetoDefectRepo> GetTotalDefectQueryableList(IQueryable<int> reportIds)
        {
            return _context.FbReportInspDefects.Where(y => y.FbReportDetailId > 0 && reportIds.Contains(y.FbReportDetailId) &&
            !string.IsNullOrWhiteSpace(y.Description)
            && y.Active.Value && (y.Minor > 0 || y.Major > 0 || y.Critical > 0))
            .Select(z => new ParetoDefectRepo
            {
                DefectName = z.Description,
                Critical = z.Critical,
                Major = z.Major,
                Minor = z.Minor,
            }).AsNoTracking();
        }

        public IQueryable<ParetoDefectRepo> GetTotalDefectQueryableListbyProductCategory(IQueryable<int> reportIds)
        {
            return _context.FbReportInspDefects.Where(y => y.FbReportDetailId > 0 && reportIds.Contains(y.FbReportDetailId) &&
            !string.IsNullOrWhiteSpace(y.Description)
            && y.Active.Value && (y.Minor > 0 || y.Major > 0 || y.Critical > 0))
            .Select(z => new ParetoDefectRepo
            {
                DefectName = z.FbReportDetail.Inspection.ProductCategory.Name,
                Critical = z.Critical,
                Major = z.Major,
                Minor = z.Minor,
            }).AsNoTracking();
        }

        /// <summary>
        /// get defect list by report id list
        /// </summary>
        /// <param name="reportIds"></param>
        /// <returns></returns>
        public async Task<List<ParetoDefectRepo>> GetDefectListByReportIds(IEnumerable<int> reportIds)
        {
            return await _context.FbReportInspDefects.Where(y => y.FbReportDetailId > 0 && reportIds.Contains(y.FbReportDetailId) &&
            !string.IsNullOrWhiteSpace(y.Description)
            && y.Active.Value && (y.Minor > 0 || y.Major > 0 || y.Critical > 0))
                .Select(x => new ParetoDefectRepo
                {
                    DefectName = x.Description,
                    Critical = x.Critical.GetValueOrDefault(),
                    Minor = x.Minor.GetValueOrDefault(),
                    Major = x.Major.GetValueOrDefault(),
                    ReportId = x.FbReportDetailId
                }).AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// get defect list by report id list
        /// </summary>
        /// <param name="reportIds"></param>
        /// <returns></returns>
        public async Task<List<ParetoDefectRepo>> GetDefectListByQueryableReportIds(IQueryable<int> reportIds)
        {
            return await _context.FbReportInspDefects.Where(y => y.FbReportDetailId > 0 && reportIds.Contains(y.FbReportDetailId) &&
            !string.IsNullOrWhiteSpace(y.Description)
            && y.Active.Value && (y.Minor > 0 || y.Major > 0 || y.Critical > 0))
                .Select(x => new ParetoDefectRepo
                {
                    DefectName = x.Description,
                    Critical = x.Critical.GetValueOrDefault(),
                    Minor = x.Minor.GetValueOrDefault(),
                    Major = x.Major.GetValueOrDefault(),
                    ReportId = x.FbReportDetailId
                }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get defect list by report ids
        /// </summary>
        /// <param name="reportIds"></param>
        /// <returns></returns>
        /// int? defectId      
        public IQueryable<ReportDefectListRepo> GetReportDefectList(IEnumerable<int> reportIds)
        {
            return _context.FbReportInspDefects.
            Where(x => reportIds.Contains(x.FbReportDetailId) && x.Active.Value &&
            (x.Minor > 0 || x.Major > 0 || x.Critical > 0)).
            Select(x => new ReportDefectListRepo()
            {
                Critical = x.Critical,
                DefectName = x.Description,
                Major = x.Major,
                Minor = x.Minor,
                ReportId = x.FbReportDetailId
            });
        }

        /// <summary>
        /// get defect list by report ids
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        /// int? defectId      
        public IQueryable<ReportDefectListRepo> GetReportDefectQueryableList(IQueryable<int> bookingIds)
        {
            return _context.FbReportInspDefects.
            Where(x => bookingIds.Contains(x.FbReportDetail.InspectionId.GetValueOrDefault()) && x.Active.Value && x.FbReportDetail.Active.Value &&
            (x.Minor > 0 || x.Major > 0 || x.Critical > 0)).
            Select(x => new ReportDefectListRepo()
            {
                Critical = x.Critical,
                DefectName = x.Description,
                Major = x.Major,
                Minor = x.Minor,
                ReportId = x.FbReportDetailId
            });
        }

        /// <summary>
        /// get defect list 
        /// </summary>
        /// <returns></returns>
        public IQueryable<CommonDataSource> GetDefectList()
        {
            return _context.FbReportInspDefects.Where(x => x.Active.Value && x.DefectId > 0).Select(x => new CommonDataSource
            {
                Name = x.Description
            }).AsNoTracking();
        }

        /// <summary>
        /// get supplier and factory details for container table
        /// </summary>
        /// <param name="reportIds"></param>
        /// <returns></returns>
        public async Task<List<ReportSupplierDetails>> GetSupFactReportDetailsByContainerLevel(IEnumerable<int> reportIds)
        {
            return await _context.InspContainerTransactions.Where(x => x.Active.Value && reportIds.Contains(x.FbReportId.GetValueOrDefault())).
                Select(x => new ReportSupplierDetails
                {
                    SupplierId = x.Inspection.SupplierId,
                    FactoryId = x.Inspection.FactoryId,
                    FactoryName = x.Inspection.Factory.SupplierName,
                    SupplierName = x.Inspection.Supplier.SupplierName,
                    ReportId = x.FbReportId,
                    ReportNo = x.FbReport.ReportTitle,
                    ReportLink = x.FbReport.FinalReportPath,
                }).Distinct().AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get defect photo list with description
        /// </summary>
        /// <param name="reportIds"></param>
        /// <returns></returns>
        public IQueryable<DefectPhotoRepo> GetDefectPhotoList(IEnumerable<int> reportIds)
        {
            return _context.FbReportDefectPhotos.
                   Where(x => reportIds.Contains(x.Defect.FbReportDetailId) &&
                   x.Active.Value)
                   .Select(p => new DefectPhotoRepo
                   {
                       DefectPhotoPath = p.Path,
                       DefectName = p.Defect.Description,
                       Critical = p.Defect.Critical,
                       Major = p.Defect.Major,
                       Minor = p.Defect.Minor,
                       Description = p.Description
                   }).AsNoTracking();
        }
        /// <summary>
        /// get defect photo list with description
        /// </summary>
        /// <param name="reportIds"></param>
        /// <returns></returns>
        public IQueryable<DefectPhotoRepo> GetDefectPhotoQueryableList(IQueryable<int> reportIds)
        {
            return _context.FbReportDefectPhotos.
                   Where(x => reportIds.Contains(x.Defect.FbReportDetailId) &&
                   x.Active.Value)
                   .Select(p => new DefectPhotoRepo
                   {
                       DefectPhotoPath = p.Path,
                       DefectName = p.Defect.Description,
                       Critical = p.Defect.Critical,
                       Major = p.Defect.Major,
                       Minor = p.Defect.Minor,
                       Description = p.Description
                   });
        }

        /// <summary>
        /// get country details from product by report list
        /// </summary>
        /// <param name="reportIds"></param>
        /// <returns></returns>
        public async Task<List<CountryReport>> GetCountryProductReportData(IEnumerable<int> reportIds)
        {
            return await (from inspproducttran in _context.InspProductTransactions
                          join insptran in _context.InspTransactions on inspproducttran.InspectionId equals insptran.Id
                          join suaddress in _context.SuAddresses on insptran.FactoryId equals suaddress.SupplierId

                          where inspproducttran.Active.Value && suaddress.AddressTypeId == (int)Supplier_Address_Type.HeadOffice && inspproducttran.FbReportId > 0
                          && reportIds.Contains(inspproducttran.FbReportId.GetValueOrDefault())

                          select new CountryReport
                          {
                              CountryId = suaddress.CountryId,
                              CountryName = suaddress.Country.CountryName,
                              ReportId = inspproducttran.FbReportId
                          }).Distinct().AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// get country details from container by report list
        /// </summary>
        /// <param name="reportIds"></param>
        /// <returns></returns>
        public async Task<List<CountryReport>> GetCountryContainerReportData(IEnumerable<int> reportIds)
        {
            return await (from inspcontainertran in _context.InspContainerTransactions
                          join insptran in _context.InspTransactions on inspcontainertran.InspectionId equals insptran.Id
                          join suaddress in _context.SuAddresses on insptran.FactoryId equals suaddress.SupplierId

                          where inspcontainertran.Active.Value && suaddress.AddressTypeId == (int)Supplier_Address_Type.HeadOffice && inspcontainertran.FbReportId > 0
                          && reportIds.Contains(inspcontainertran.FbReportId.GetValueOrDefault())

                          select new CountryReport
                          {
                              CountryId = suaddress.CountryId,
                              CountryName = suaddress.Country.CountryName,
                              ReportId = inspcontainertran.FbReportId
                          }).Distinct().AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// get country details from container by report list
        /// </summary>
        /// <param name="reportIds"></param>
        /// <returns></returns>
        public async Task<List<CountryReport>> GetCountryInspReportData(IQueryable<int> reportIds)
        {


            return await _context.FbReportDetails.
           Where(x => reportIds.Contains(x.Id) && x.Active.Value
           && x.Inspection.Factory.SuAddresses.Any(y => y.AddressTypeId == (int)Supplier_Address_Type.HeadOffice))
          .Select(x => new CountryReport
          {
              CountryId = x.Inspection.Factory.SuAddresses.FirstOrDefault(y => y.AddressTypeId == (int)Supplier_Address_Type.HeadOffice).CountryId,
              CountryName = x.Inspection.Factory.SuAddresses.FirstOrDefault(y => y.AddressTypeId == (int)Supplier_Address_Type.HeadOffice).Country.CountryName,
              ReportId = x.Id
          }).Distinct().AsNoTracking().ToListAsync();


        }


        /// <summary>
        /// get product list by report ids
        /// </summary>
        /// <param name="reportIds"></param>
        /// <returns></returns>
        public async Task<List<ReportSupplierDetails>> GetSupFactReportDetailsByProductLevel(IEnumerable<int> reportIds)
        {
            return await _context.InspProductTransactions.Where(x => x.Active.Value && reportIds.Contains(x.FbReportId.GetValueOrDefault())).
                Select(x => new ReportSupplierDetails
                {
                    SupplierId = x.Inspection.SupplierId,
                    FactoryId = x.Inspection.FactoryId,
                    FactoryName = x.Inspection.Factory.SupplierName,
                    SupplierName = x.Inspection.Supplier.SupplierName,
                    ReportId = x.FbReportId,
                    ReportNo = x.FbReport.ReportTitle,
                    ReportLink = x.FbReport.FinalReportPath,
                }).Distinct().AsNoTracking().ToListAsync();
        }
        /// <summary>
        /// get inspection container details with report id by report id list
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task<List<ReportSupplierDetails>> GetSupFactReportDetailsQueryableReport(IQueryable<int> bookingIds)
        {
            return await _context.FbReportDetails.
             Where(x => bookingIds.Contains(x.InspectionId.GetValueOrDefault()) && x.Active.Value)
            .Select(x => new ReportSupplierDetails
            {
                SupplierId = x.Inspection.SupplierId,
                FactoryId = x.Inspection.FactoryId,
                FactoryName = x.Inspection.Factory.SupplierName,
                SupplierName = x.Inspection.Supplier.SupplierName,
                ReportId = x.Id,
                ReportNo = x.ReportTitle,
                ReportLink = x.FinalReportPath,
                FinalManualReportLink = x.FinalManualReportPath
            }).Distinct().AsNoTracking().ToListAsync();
        }

        public IQueryable<DefectReportList> GetQueryableReportDefect(IQueryable<int> bookingIdList)
        {
            var data = from fbreport in _context.FbReportDetails
                       join reportdefect in _context.FbReportInspDefects.Where(x => x.Active.Value && (x.Minor > 0 || x.Major > 0 || x.Critical > 0)) on fbreport.Id equals reportdefect.FbReportDetailId
                       join suaddress in _context.SuAddresses on fbreport.Inspection.FactoryId equals suaddress.SupplierId
                       join cubrand in _context.InspTranCuBrands.Where(x => x.Active) on fbreport.InspectionId.GetValueOrDefault() equals cubrand.InspectionId into Cubrand
                       from brand in Cubrand.DefaultIfEmpty()
                       where fbreport.Active.HasValue && fbreport.Active.Value && suaddress.AddressTypeId == (int)Supplier_Address_Type.HeadOffice
                       && bookingIdList.Contains(fbreport.InspectionId.GetValueOrDefault())
                       select new DefectReportList
                       {
                           FactoryCountryId = suaddress.CountryId,
                           FactoryCountryName = suaddress.Country.CountryName,
                           SupplierId = fbreport.Inspection.SupplierId,
                           SupplierName = fbreport.Inspection.Supplier.SupplierName,
                           FactoryId = fbreport.Inspection.FactoryId.GetValueOrDefault(),
                           FactoryName = fbreport.Inspection.Factory.SupplierName,
                           BrandId = brand != null ? brand.BrandId : 0,
                           BrandName = brand.Brand.Name,
                           InspectionId = fbreport.InspectionId.GetValueOrDefault(),
                           ReportId = fbreport.Id,
                           ReportDefectId = reportdefect.Id,
                           DefectName = reportdefect.Description,
                           Major = reportdefect.Major,
                           Minor = reportdefect.Minor,
                           Critical = reportdefect.Critical
                       };
            return data;
        }
    }
}
