using BI.Maps;
using BI.Utilities;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.Dashboard;
using DTO.DefectDashboard;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BI
{
    public class DefectDashboardManager : ApiCommonData, IDefectDashboardManager
    {
        private readonly IDefectDashboardRepository _repo = null;
        private readonly ISupplierManager _supplierManager = null;
        private readonly IInspectionBookingManager _inspManager = null;
        private readonly IDashboardManager _dashboardmanager = null;
        private readonly ILocationRepository _locationRepo = null;
        private readonly ICustomerRepository _cusRepo = null;
        private readonly ISharedInspectionManager _sharedInspection = null;
        private readonly DefectDashboardMap _defectdashboardmap = null;
        private readonly ICustomerProductManager _customerProductManager = null;
        private readonly IReferenceManager _referenceManager = null;
        private readonly IHelper _helper = null;

        public DefectDashboardManager(IDefectDashboardRepository repo, IInspectionBookingManager inspManager, IDashboardManager dashboardmanager,
            ISupplierManager supplierManager,
                ILocationRepository locationRepo, ICustomerRepository cusRepo, ISharedInspectionManager sharedInspection,
                ICustomerProductManager customerProductManager, IReferenceManager referenceManager, IHelper helper)
        {
            _repo = repo;
            _inspManager = inspManager;
            _dashboardmanager = dashboardmanager;
            _supplierManager = supplierManager;
            _locationRepo = locationRepo;
            _cusRepo = cusRepo;
            _sharedInspection = sharedInspection;
            _defectdashboardmap = new DefectDashboardMap();
            _customerProductManager = customerProductManager;
            _referenceManager = referenceManager;
            _helper = helper;
        }
        private async Task<List<BookingDetail>> GetBookingDataByRequest(DefectDashboardRequest request)
        {
            CustomerDashboardFilterRequest req = new CustomerDashboardFilterRequest
            {
                CustomerId = request.CustomerId,
                SupplierId = request.SupplierId,
                ServiceDateFrom = request.FromDate,
                ServiceDateTo = request.ToDate,
                SelectedCountryIdList = request.FactoryCountryIds,
                SelectedBrandIdList = request.SelectedBrandIdList,
                SelectedBuyerIdList = request.SelectedBuyerIdList,
                SelectedCollectionIdList = request.SelectedCollectionIdList,
                SelectedDeptIdList = request.SelectedDeptIdList,
                StatusIdList = InspectedStatusList,
                SelectedFactIdList = request.FactoryIds
            };

            return await _dashboardmanager.GetBookingDetails(req);
        }
        /// <summary>
        /// get booking and report details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BookingReportResponse> GetBookingReportDetails(DefectDashboardRequest request)
        {
            if (request == null)
                return new BookingReportResponse() { Result = DefectDashboardResult.RequestNotCorrectFormat };

            var bookingDetails = await GetBookingDataByRequest(request);

            if (bookingDetails == null || !bookingDetails.Any())
            {
                return new BookingReportResponse() { Result = DefectDashboardResult.NotFound };
            }

            //get booking ids
            var bookingIds = bookingDetails.Select(x => x.InspectionId);

            //get report details by booking ids
            var bookingReportList = await _inspManager.GetReportDataByBooking(bookingIds.ToList());

            if (!bookingReportList.Any(x => x.ReportId > 0))
            {
                return new BookingReportResponse() { Result = DefectDashboardResult.NotFound };
            }

            return new BookingReportResponse()
            {
                BookingReportModel = bookingReportList.Where(x => x.ReportId > 0).Select(x => _defectdashboardmap.BookingReportDataMap(x)).ToList(),
                // MonthXAxis = FrameLineGraphXAxis(request),
                Result = DefectDashboardResult.Success
            };
        }

        /// <summary>
        /// get booking and report details
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BookingReportResponse> GetAllBookingReportDetails(DefectDashboardFilterRequest request)
        {
            if (request == null)
                return new BookingReportResponse() { Result = DefectDashboardResult.RequestNotCorrectFormat };

            //get booking ids
            var bookingIds = GetQueryableBookingIdList(request);

            //get report details by booking ids
            var bookingReportList = await _inspManager.GetReportDataByQueryableBooking(bookingIds);

            if (bookingReportList == null || !bookingReportList.Any())
            {
                return new BookingReportResponse() { Result = DefectDashboardResult.NotFound };
            }

            return new BookingReportResponse()
            {
                BookingReportModel = bookingReportList.Select(x => _defectdashboardmap.BookingReportDataMap(x)).ToList(),
                MonthXAxis = FrameLineGraphXAxis(request),
                Result = DefectDashboardResult.Success
            };
        }


        /// <summary>
        /// defect category details
        /// </summary>
        /// <param name="reportIdList"></param>
        /// <returns></returns>
        public async Task<DefectCategoryResponse> GetDefectCategoryList(IEnumerable<int> reportIdList)
        {
            var defectCategoryListResponse = await GetDefectCategoryData(reportIdList);

            if (defectCategoryListResponse.Result == DefectDashboardResult.Success)
            {
                var defectCategoryList = defectCategoryListResponse.DefectCategoryList;

                //assign the color for each category
                for (int i = 1; i <= defectCategoryList.Count; i++)
                {
                    defectCategoryList[i - 1].Color = InspectionRejectDashboardColor.GetValueOrDefault(i);
                }

                return new DefectCategoryResponse() { Result = DefectDashboardResult.Success, DefectCategoryList = defectCategoryList };
            }

            return defectCategoryListResponse;
        }

        /// <summary>
        /// defect category details
        /// </summary>
        /// <param name="reportIdList"></param>
        /// <returns></returns>
        public async Task<DefectCategoryResponse> GetAllDefectCategoryList(DefectDashboardFilterRequest request)
        {
            if (request == null)
                return new DefectCategoryResponse() { Result = DefectDashboardResult.RequestNotCorrectFormat };


            var bookingIds = GetQueryableBookingIdList(request);

            var reportIdList = _inspManager.GetReportIdDataByQueryableBooking(bookingIds);
            if (reportIdList == null || !reportIdList.Any())
            {
                return new DefectCategoryResponse() { Result = DefectDashboardResult.NotFound };
            }

            var defectCategoryListResponse = await GetDefectCategoryQueryableData(reportIdList);

            if (defectCategoryListResponse.Result == DefectDashboardResult.Success)
            {
                var defectCategoryList = defectCategoryListResponse.DefectCategoryList;

                //assign the color for each category
                for (int i = 1; i <= defectCategoryList.Count; i++)
                {
                    defectCategoryList[i - 1].Color = InspectionRejectDashboardColor.GetValueOrDefault(i);
                }

                return new DefectCategoryResponse() { Result = DefectDashboardResult.Success, DefectCategoryList = defectCategoryList };
            }

            return defectCategoryListResponse;
        }


        /// <summary>
        /// get defect category list 
        /// </summary>
        /// <param name="reportIdList"></param>
        /// <returns></returns>
        private async Task<DefectCategoryResponse> GetDefectCategoryData(IEnumerable<int> reportIdList)
        {
            //get catgory defect details
            var defectCategoryList = await _repo.GetDefectCategoryList(reportIdList);

            if (!defectCategoryList.Any())
                return new DefectCategoryResponse() { Result = DefectDashboardResult.NotFound };

            return new DefectCategoryResponse() { Result = DefectDashboardResult.Success, DefectCategoryList = defectCategoryList };

        }
        /// <summary>
        /// get defect category list 
        /// </summary>
        /// <param name="reportIdList"></param>
        /// <returns></returns>
        private async Task<DefectCategoryResponse> GetDefectCategoryQueryableData(IQueryable<int> reportIdList)
        {
            //get catgory defect details
            var defectCategoryList = await _repo.GetDefectCategoryQueryableList(reportIdList);

            if (!defectCategoryList.Any())
                return new DefectCategoryResponse() { Result = DefectDashboardResult.NotFound };

            return new DefectCategoryResponse() { Result = DefectDashboardResult.Success, DefectCategoryList = defectCategoryList };

        }
        /// <summary>
        /// export the defect category details
        /// </summary>
        /// <param name="reportIdList"></param>
        /// <returns></returns>
        public async Task<DefectCategoryExportResponse> GetDefectCategoryExportList(DefectDashboardFilterRequest request)
        {
            // get booking report details 

            var reportDetails = await GetAllBookingReportDetails(request);

            if (reportDetails.Result == DefectDashboardResult.Success)
            {
                //get booking ids
                var bookingIds = GetQueryableBookingIdList(request);

                var reportIdList = _inspManager.GetReportIdDataByQueryableBooking(bookingIds);

                var defectCategoryListResponse = await GetDefectCategoryQueryableData(reportIdList);

                if (defectCategoryListResponse.Result == DefectDashboardResult.Success)
                {
                    //group by category
                    var categoryByDefect = defectCategoryListResponse.DefectCategoryList.Select(x => new DefectCategoryExport
                    {
                        CategoryName = x.CategoryName,
                        DefectCountByCategory = x.DefectCountByCategory,
                    }).OrderByDescending(x => x.DefectCountByCategory).ToList();

                    return new DefectCategoryExportResponse()
                    {
                        RequestFilters = await SetDashboardExportFilter(request),
                        Result = DefectDashboardResult.Success,
                        DefectCategoryList = categoryByDefect
                    };
                }
                return new DefectCategoryExportResponse() { Result = defectCategoryListResponse.Result };
            }
            return new DefectCategoryExportResponse() { Result = reportDetails.Result };
        }

        /// <summary>
        /// Assign the from date and to date based on inner year selected
        /// </summary>
        /// <param name="request"></param>
        private void AssignDates(DefectDashboardFilterRequest request)
        {
            if (request.InnerDefectYearId > 0)
            {
                //date inner selection
                if ((request.FromDate.Year != request.InnerDefectYearId) && (request.ToDate.Year != request.InnerDefectYearId))
                {
                    request.FromDate = new DateObject() { Year = request.InnerDefectYearId.GetValueOrDefault(), Day = 1, Month = 1 };
                    request.ToDate = new DateObject() { Year = request.InnerDefectYearId.GetValueOrDefault(), Day = 31, Month = 12 };
                }
                //if from date year not selected in inner filter change the from date to start of the year date
                if (request.FromDate.Year != request.InnerDefectYearId)
                {
                    request.FromDate = new DateObject() { Year = request.InnerDefectYearId.GetValueOrDefault(), Day = 1, Month = 1 };
                }
                //30-12 to 1-21
                else if (request.ToDate.Year != request.InnerDefectYearId)
                {
                    request.ToDate = new DateObject() { Year = request.InnerDefectYearId.GetValueOrDefault(), Day = 31, Month = 12 };
                }
            }
        }

        /// <summary>
        /// line graph x axis frame
        /// </summary>
        /// <param name="request"></param>
        private List<DefectYear> FrameLineGraphXAxis(DefectDashboardFilterRequest request)
        {
            //reqFromDate =   request.FromDate.Month
            var defectYear = new List<DefectYear>();

            for (int i = 0; i < 12; i++)
            {
                DefectYear res = new DefectYear
                {
                    Year = request.InnerDefectYearId.GetValueOrDefault(),
                    Month = i
                };
                defectYear.Add(res);
            }
            return defectYear;
        }

        /// <summary>
        /// get defect year by inner filters
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DefectYearInnerCountResponse> GetDefectYearListByInnerFilter(DefectDashboardFilterRequest request)
        {
            var yearListResponse = await GetDefectByYearList(request);

            if (yearListResponse.Result == DefectDashboardResult.Success)
            {
                var groupByMonthYear = new List<DefectMonth>();

                //group by the year and month wise data for major
                var groupByMajorMonthCount = yearListResponse.DefectCountList.
                        GroupBy(x => new { x.Year, x.Month }, (key, group) => new DefectMonth
                        {
                            Year = key.Year,
                            Month = key.Month,
                            DefectMonthCount = group.Sum(k => k.Major).GetValueOrDefault(),
                            MonthName = MonthData.GetValueOrDefault(key.Month),
                            DefectName = DefectMajor
                        }).ToList();

                //group by the year and month wise data for minor
                var groupByMinorMonthCount = yearListResponse.DefectCountList.
                        GroupBy(x => new { x.Year, x.Month }, (key, group) => new DefectMonth
                        {
                            Year = key.Year,
                            Month = key.Month,
                            DefectMonthCount = group.Sum(k => k.Minor).GetValueOrDefault(),
                            MonthName = MonthData.GetValueOrDefault(key.Month),
                            DefectName = DefectMinor
                        }).ToList();

                //group by the year and month wise data for critical
                var groupByCriticalMonthCount = yearListResponse.DefectCountList.
                         GroupBy(x => new { x.Year, x.Month }, (key, group) => new DefectMonth
                         {
                             Year = key.Year,
                             Month = key.Month,
                             DefectMonthCount = group.Sum(k => k.Critical).GetValueOrDefault(),
                             MonthName = MonthData.GetValueOrDefault(key.Month),
                             DefectName = DefectCritical
                         }).ToList();


                groupByMonthYear.AddRange(groupByCriticalMonthCount);
                groupByMonthYear.AddRange(groupByMajorMonthCount);
                groupByMonthYear.AddRange(groupByMinorMonthCount);

                var items = new List<DefectYearCountDataModel> { };

                items.Add(new DefectYearCountDataModel()
                {
                    DefectName = DefectTotalReports,
                    DefectYearCount = yearListResponse.ReportIdList.Distinct().Count(),
                    Color = DefectAnalysisColorList.Where(x => x.Key == DefectTotalReports).Select(x => x.Value).FirstOrDefault()
                });

                //get the per year defect count along with month wise defect data
                items.AddRange(groupByMonthYear.GroupBy(p => p.DefectName, (key, _data) =>
               new DefectYearCountDataModel
               {
                   DefectName = key,
                   DefectYearCount = _data.Where(x => x.DefectName == key).Sum(x => x.DefectMonthCount.GetValueOrDefault()),
                   DefectMonthList = _data.Where(x => x.DefectName == key).ToList(),
                   Color = DefectAnalysisColorList.Where(x => x.Key == key).Select(x => x.Value).FirstOrDefault()
               }).ToList());

                //skip used for remove total reports count inner list
                var yearList = items.Skip(1).Where(x => x.DefectMonthList.Any(y => y.DefectMonthCount > 0)).ToList();

                if (!yearList.Any())
                    return new DefectYearInnerCountResponse()
                    {
                        Result = DefectDashboardResult.NotFound
                    };

                return new DefectYearInnerCountResponse()
                {
                    MonthXAxis = yearListResponse.MonthXAxis,
                    DefectCountList = items,
                    Result = DefectDashboardResult.Success
                };
            }
            return new DefectYearInnerCountResponse() { Result = yearListResponse.Result };
        }

        /// <summary>
        /// defect count year list for export
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DefectYearExportResponse> ExportDefectCountYearList(DefectDashboardFilterRequest request)
        {
            var yearListResponse = await GetDefectByYearList(request);

            if (yearListResponse.Result == DefectDashboardResult.Success)
            {
                //group by the year and month wise data 
                var groupByMonthCount = yearListResponse.DefectCountList.
                        GroupBy(x => new { x.Year, x.Month }, (key, group) => new DefectMonthExport
                        {
                            Year = key.Year,
                            Month = key.Month,
                            Critical = group.Sum(k => k.Critical).GetValueOrDefault(),
                            Major = group.Sum(k => k.Major).GetValueOrDefault(),
                            Minor = group.Sum(k => k.Minor).GetValueOrDefault(),
                            MonthName = MonthData.GetValueOrDefault(key.Month),
                        }).OrderBy(x => x.Month).ToList();

                return new DefectYearExportResponse()
                {
                    Result = DefectDashboardResult.Success,
                    MonthDefectData = groupByMonthCount,
                    RequestFilters = await SetDashboardExportFilter(request)
                };
            }
            return new DefectYearExportResponse() { Result = yearListResponse.Result };
        }

        /// <summary>
        /// get defect list by inspection year and month
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DefectYearCountResponse> GetDefectYearList(DefectDashboardRequest request)
        {
            //assign the from date and to date based on inner filters
            // AssignDates(request);

            // get booking report details 
            var reportDetails = await GetBookingReportDetails(request);

            if (reportDetails.Result == DefectDashboardResult.Success)
            {
                var reportIdList = reportDetails.BookingReportModel.Select(x => x.ReportId.GetValueOrDefault()).ToList();

                var inspContainerList = await _repo.GetInspectionContainerDefectsList(reportIdList);

                var inspNonContainerList = await _repo.GetInspectionProductDefectsList(reportIdList);

                var reportList = await _repo.GetReportDefectsList(reportIdList);

                //get the defect count list which has non-container service type
                var defectContainerMonthlyList = inspContainerList.Where(x => reportList.Select(y => y.ReportId).Contains(x.ReportId))
                    .Select(x => new DefectMonthRepo()
                    {
                        Critical = reportList.Where(z => z.ReportId == x.ReportId).Sum(z => z.Critical),
                        Year = x.Year,
                        Month = x.Month,
                        Major = reportList.Where(z => z.ReportId == x.ReportId).Sum(z => z.Major),
                        Minor = reportList.Where(z => z.ReportId == x.ReportId).Sum(z => z.Minor),
                    });

                //get the defect count list which has container service type
                var defectNonContainerMonthlyList = inspNonContainerList.Where(x => reportList.Select(y => y.ReportId).Contains(x.ReportId))
                    .Select(x => new DefectMonthRepo()
                    {
                        Critical = reportList.Where(z => z.ReportId == x.ReportId).Sum(z => z.Critical),
                        Year = x.Year,
                        Month = x.Month,
                        Major = reportList.Where(z => z.ReportId == x.ReportId).Sum(z => z.Major),
                        Minor = reportList.Where(z => z.ReportId == x.ReportId).Sum(z => z.Minor),
                    }).ToList();

                //merge the above two list 
                var defectMonthlyList = defectNonContainerMonthlyList.Union(defectContainerMonthlyList);

                if (!defectMonthlyList.Any())
                {
                    return new DefectYearCountResponse() { Result = DefectDashboardResult.NotFound };
                }
                return new DefectYearCountResponse()
                {
                    Result = DefectDashboardResult.Success,
                    DefectCountList = defectMonthlyList.ToList(),
                    MonthXAxis = reportDetails.MonthXAxis,
                    ReportIdList = reportIdList
                };
            }
            return new DefectYearCountResponse() { Result = reportDetails.Result };
        }

        /// <summary>
        /// get defect list by inspection year and month
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DefectYearCountResponse> GetDefectByYearList(DefectDashboardFilterRequest request)
        {
            //assign the from date and to date based on inner filters
            AssignDates(request);

            // get booking report details 
            var reportDetails = await GetAllBookingReportDetails(request);

            if (reportDetails.Result == DefectDashboardResult.Success)
            {

                //get booking ids
                var bookingIds = GetQueryableBookingIdList(request);

                var reportIdList = _inspManager.GetReportIdDataByQueryableBooking(bookingIds);


                var inspDefectList = await _repo.GetInspectionDefectsList(reportIdList);

                if (inspDefectList == null || !inspDefectList.Any())
                {
                    return new DefectYearCountResponse() { Result = DefectDashboardResult.NotFound };
                }
                var reportList = await _repo.GetReportDefectsQueryableList(reportIdList);

                //get the defect count list which has non-container service type
                var defectMonthlyList = inspDefectList.Where(x => reportList.Select(y => y.ReportId).Contains(x.ReportId))
                    .Select(x => new DefectMonthRepo()
                    {
                        Critical = reportList.Where(z => z.ReportId == x.ReportId).Sum(z => z.Critical),
                        Year = x.Year,
                        Month = x.Month,
                        Major = reportList.Where(z => z.ReportId == x.ReportId).Sum(z => z.Major),
                        Minor = reportList.Where(z => z.ReportId == x.ReportId).Sum(z => z.Minor),
                    });


                if (!defectMonthlyList.Any())
                {
                    return new DefectYearCountResponse() { Result = DefectDashboardResult.NotFound };
                }
                return new DefectYearCountResponse()
                {
                    Result = DefectDashboardResult.Success,
                    DefectCountList = defectMonthlyList.ToList(),
                    MonthXAxis = reportDetails.MonthXAxis,
                    ReportIdList = reportIdList.ToList()
                };
            }
            return new DefectYearCountResponse() { Result = reportDetails.Result };
        }

        public async Task<List<DefectReportRepo>> GetDefectTypeList(DefectDashboardFilterRequest request)
        {

            //get booking ids
            var bookingIds = GetQueryableBookingIdList(request);

            var reportIdList = _inspManager.GetReportIdDataByQueryableBooking(bookingIds);

            var defectTypeList = await _repo.GetReportDefectsQueryableList(reportIdList);

            return defectTypeList;
        }


        /// <summary>
        /// Get the dropdown values to specify in the export file
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DefectDashboardFilterExport> SetExportFilter(DefectDashboardRequest request)
        {
            var response = new DefectDashboardFilterExport();

            //top factory country filter
            if (request.FactoryCountryIds != null && request.FactoryCountryIds.Any())
            {
                var countryList = await _locationRepo.GetCountryByIds(request.FactoryCountryIds.Where(x => x.HasValue).Select(x => x.Value).ToList());
                response.FactoryCountryName = string.Join(", ", countryList.Select(x => x.Name));
            }

            if (request.CustomerId > 0)
            {
                var customers = await _cusRepo.GetCustomerById(new int[] { request.CustomerId }.ToList());
                response.CustomerName = string.Join(", ", customers.Select(x => x.Name));
            }

            //top factory filter
            if (request.FactoryIds != null && request.FactoryIds.Any())
            {
                var factoryList = _supplierManager.GetSupplierList();

                //execute the supplier list to get factory name
                var factoryListData = await factoryList.Where(x => request.FactoryIds.Contains(x.Id) && x.TypeId == (int)Supplier_Type.Factory)
                   .Select(x => x.SupplierName).ToListAsync();

                response.FactoryName = string.Join(", ", factoryListData);
            }

            if (request.SupplierId > 0)
            {
                var factoryList = _supplierManager.GetSupplierList();

                //execute the supplier list to get supplier name
                var supplierListData = await factoryList.Where(x => request.SupplierId == x.Id && x.TypeId == (int)Supplier_Type.Supplier_Agent)
                   .Select(x => x.SupplierName).ToListAsync();

                response.SupplierName = string.Join(", ", supplierListData);
            }

            response.FromDate = request.FromDate == null ? "" : request.FromDate.ToDateTime().ToString(StandardDateFormat);
            response.ToDate = request.ToDate == null ? "" : request.ToDate.ToDateTime().ToString(StandardDateFormat);
            response.DefectCountYear = request.InnerDefectYearId.GetValueOrDefault();

            return response;
        }
        /// <summary>
        /// Get the dropdown values to specify in the export file
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DefectDashboardFilterExport> SetDashboardExportFilter(DefectDashboardFilterRequest request)
        {
            var response = new DefectDashboardFilterExport();

            //top factory country filter
            if (request.FactoryCountryIds != null && request.FactoryCountryIds.Any())
            {
                var countryList = await _locationRepo.GetCountryByIds(request.FactoryCountryIds.ToList());
                response.FactoryCountryName = string.Join(", ", countryList.Select(x => x.Name));
            }

            if (request.SelectedProductIdList != null && request.SelectedProductIdList.Any())
            {
                var productList = await _customerProductManager.GetProductNameByProductIds(request.SelectedProductIdList);
                response.Product = string.Join(", ", productList);
            }
            if (request.SelectedProdCategoryIdList != null && request.SelectedProdCategoryIdList.Any())
            {
                var productCategories = await _referenceManager.GetProdCategoriesByProdCategoryIds(request.SelectedProdCategoryIdList);
                response.ProductCategory = string.Join(", ", productCategories.Select(x => x.Name));
            }

            if (request.CustomerId > 0)
            {
                var customers = await _cusRepo.GetCustomerById(new int[] { request.CustomerId.GetValueOrDefault() }.ToList());
                response.CustomerName = string.Join(", ", customers.Select(x => x.Name));
            }

            //top factory filter
            if (request.FactoryIds != null && request.FactoryIds.Any())
            {
                var factoryList = _supplierManager.GetSupplierList();

                //execute the supplier list to get factory name
                var factoryListData = await factoryList.Where(x => request.FactoryIds.Contains(x.Id) && x.TypeId == (int)Supplier_Type.Factory)
                   .Select(x => x.SupplierName).ToListAsync();

                response.FactoryName = string.Join(", ", factoryListData);
            }

            if (request.SupplierId > 0)
            {
                var factoryList = _supplierManager.GetSupplierList();

                //execute the supplier list to get supplier name
                var supplierListData = await factoryList.Where(x => request.SupplierId == x.Id && x.TypeId == (int)Supplier_Type.Supplier_Agent)
                   .Select(x => x.SupplierName).ToListAsync();

                response.SupplierName = string.Join(", ", supplierListData);
            }

            response.FromDate = request.FromDate == null ? "" : request.FromDate.ToDateTime().ToString(StandardDateFormat);
            response.ToDate = request.ToDate == null ? "" : request.ToDate.ToDateTime().ToString(StandardDateFormat);
            response.DefectCountYear = request.InnerDefectYearId.GetValueOrDefault();

            return response;
        }


        /// <summary>
        /// //Get the dropdown values for low performance table to specify in the export file
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private DefectSubSupOrFactFilterExport SetSubSuppOrFactExportFilter(DefectPerformanceFilter request)
        {
            var response = new DefectSubSupOrFactFilterExport();

            if (!string.IsNullOrWhiteSpace(request.DefectName))
            {
                response.SubDefectName = request.DefectName;
            }

            if (request.TypeId > 0)
            {
                if (request.TypeId == (int)Supplier_Type.Factory)
                {
                    response.SubSupOrFact = Supplier_Type.Factory.ToString();
                }
                else if (request.TypeId == (int)Supplier_Type.Supplier_Agent)
                {
                    response.SubSupOrFact = Supplier_Type.Supplier_Agent.ToString();
                }
            }

            if (request.DefectSelected.Any())
            {
                var selecteddefectList = DefectNameList.Where(x => request.DefectSelected.Contains(x.Key))
                                                               .Select(x => x.Value).ToList();

                response.DefectSelectName = string.Join(", ", selecteddefectList);
            }

            return response;
        }


        /// <summary>
        /// pareto get defect count by name
        /// </summary>
        /// <param name="reportIdList"></param>
        /// <returns></returns>
        public async Task<ParetoDefectResponse> GetParetoDefectList(IEnumerable<int> reportIdList)
        {
            var iquerabledefectList = _repo.GetTotalDefectList(reportIdList);



            var defectList = await iquerabledefectList.Select(x => new
            {
                x.DefectName,
                x.Critical,
                x.Minor,
                x.Major
            })
              .GroupBy(p => p.DefectName, (key, _data) =>
            new ParetoDefect
            {
                DefectName = key,
                DefectCount = _data.Sum(x => x.Critical.GetValueOrDefault() + x.Major.GetValueOrDefault() + x.Minor.GetValueOrDefault()),
            }).OrderByDescending(x => x.DefectCount).Take(ParetoDefectCount).ToListAsync();

            if (!defectList.Any())
                return new ParetoDefectResponse() { Result = DefectDashboardResult.NotFound };

            int totaldefect = defectList.Sum(x => x.DefectCount);

            //assign the color for each defect
            for (int i = 0; i < defectList.Count; i++)
            {
                defectList[i].Color = ParetoDefectColorList.GetValueOrDefault(i + 1);

                //get the defect count and the defect %
                defectList[i].Percentage = GetDefectPercentage(defectList[i].DefectCount, totaldefect);
            }

            return new ParetoDefectResponse()
            {
                Result = DefectDashboardResult.Success,
                ParetoList = defectList
            };
        }

        /// <summary>
        /// pareto get defect count by name
        /// </summary>
        /// <param name="reportIdList"></param>
        /// <returns></returns>
        public async Task<ParetoDefectResponse> GetAllDefectCount(DefectDashboardFilterRequest request)
        {
            if (request == null)
                return new ParetoDefectResponse() { Result = DefectDashboardResult.RequestNotCorrectFormat };

            var bookingIds = GetQueryableBookingIdList(request);


            var reportIdList = _inspManager.GetReportIdDataByQueryableBooking(bookingIds);


            var iquerabledefectList = _repo.GetTotalDefectQueryableList(reportIdList);

            var defectList = await iquerabledefectList.Select(x => new
            {
                x.DefectName,
                x.Critical,
                x.Minor,
                x.Major
            })
              .GroupBy(p => p.DefectName, (key, _data) =>
            new ParetoDefect
            {
                DefectName = key,
                DefectCount = _data.Sum(x => x.Critical.GetValueOrDefault() + x.Major.GetValueOrDefault() + x.Minor.GetValueOrDefault()),
            }).OrderByDescending(x => x.DefectCount).Take(ParetoDefectCount).ToListAsync();

            if (!defectList.Any())
                return new ParetoDefectResponse() { Result = DefectDashboardResult.NotFound };

            int totaldefect = defectList.Sum(x => x.DefectCount);

            //assign the color for each defect
            for (int i = 0; i < defectList.Count; i++)
            {
                defectList[i].Color = ParetoDefectColorList.GetValueOrDefault(i + 1);

                //get the defect count and the defect %
                defectList[i].Percentage = GetDefectPercentage(defectList[i].DefectCount, totaldefect);
            }

            return new ParetoDefectResponse()
            {
                Result = DefectDashboardResult.Success,
                ParetoList = defectList
            };
        }

        public async Task<ParetoDefectResponse> GetAllDefectCountByProductCategory(DefectDashboardFilterRequest request)
        {
            if (request == null)
                return new ParetoDefectResponse() { Result = DefectDashboardResult.RequestNotCorrectFormat };

            var bookingIds = GetQueryableBookingIdList(request);

            var reportIdList = _inspManager.GetReportIdDataByQueryableBooking(bookingIds);

            var iquerabledefectList = _repo.GetTotalDefectQueryableListbyProductCategory(reportIdList);

            var defectList = await iquerabledefectList.Select(x => new
            {
                x.DefectName,
                x.Critical,
                x.Minor,
                x.Major
            })
              .GroupBy(p => p.DefectName, (key, _data) =>
            new ParetoDefect
            {
                DefectName = key,
                DefectCount = _data.Sum(x => x.Critical.GetValueOrDefault() + x.Major.GetValueOrDefault() + x.Minor.GetValueOrDefault()),
            }).OrderByDescending(x => x.DefectCount).Take(ParetoDefectCount).ToListAsync();

            if (!defectList.Any())
                return new ParetoDefectResponse() { Result = DefectDashboardResult.NotFound };

            int totaldefect = defectList.Sum(x => x.DefectCount);

            //assign the color for each defect
            for (int i = 0; i < defectList.Count; i++)
            {
                defectList[i].Color = ParetoDefectColorList.GetValueOrDefault(i + 1);

                //get the defect count and the defect %
                defectList[i].Percentage = GetDefectPercentage(defectList[i].DefectCount, totaldefect);
            }

            return new ParetoDefectResponse()
            {
                Result = DefectDashboardResult.Success,
                ParetoList = defectList
            };
        }


        /// <summary>
        /// Get the defect % in pareto chart
        /// </summary>
        /// <param name="defectCount"></param>
        /// <param name="totaldefectCount"></param>
        /// <returns></returns>
        private double GetDefectPercentage(int defectCount, int totaldefectCount)
        {
            double result = 0;
            if (totaldefectCount > 0 && defectCount > 0)
            {
                double res = ((double)defectCount / (double)totaldefectCount);

                result = Math.Round(res * 100, 2);
            }
            return result;
        }

        /// <summary>
        /// get the pareto defect count list for export
        /// </summary>
        /// <param name="reportIdList"></param>
        /// <returns></returns>
        public async Task<ParetoDefectExportResponse> GetParetoDefectListExport(DefectDashboardFilterRequest request)
        {

            var reportDetails = await GetAllBookingReportDetails(request);
            if (reportDetails.Result == DefectDashboardResult.Success)
            {
                var bookingIds = GetQueryableBookingIdList(request);

                var reportIdList = _inspManager.GetReportIdDataByQueryableBooking(bookingIds);

                var iqueryabledefectList = _repo.GetTotalDefectQueryableList(reportIdList);

                var defectList = await iqueryabledefectList.Select(x => new
                {
                    x.DefectName,
                    x.Critical,
                    x.Minor,
                    x.Major
                }).GroupBy(p => p.DefectName, (key, _data) =>
            new ParetoDefectExport
            {
                DefectName = key,
                DefectCount = _data.Sum(x => x.Critical.GetValueOrDefault() + x.Major.GetValueOrDefault() + x.Minor.GetValueOrDefault()),
                Critical = _data.Sum(x => x.Critical.GetValueOrDefault()),
                Major = _data.Sum(x => x.Major.GetValueOrDefault()),
                Minor = _data.Sum(x => x.Minor.GetValueOrDefault()),
            }).OrderByDescending(x => x.DefectCount).Take(ParetoDefectExportCount).ToListAsync();

                if (!defectList.Any())
                    return new ParetoDefectExportResponse() { Result = DefectDashboardResult.NotFound };

                //over all total defect count
                var totaldefectCount = defectList.Sum(x => x.DefectCount);

                for (int i = 0; i < defectList.Count; i++)
                {
                    //get the defect count and the defect %
                    defectList[i].Percentage = GetDefectPercentage(defectList[i].DefectCount, totaldefectCount);
                }
                return new ParetoDefectExportResponse()
                {
                    Result = DefectDashboardResult.Success,
                    ParetoList = defectList,
                    RequestFilters = await SetDashboardExportFilter(request)
                };
            }
            return new ParetoDefectExportResponse() { Result = reportDetails.Result };
        }

        /// <summary>
        /// get low performance factory and supplier list 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DefectPerformanceResponse> GetLowPerformanceDefectList(DefectPerformanceAnalysis request)
        {
            var response = await GetLowPerformanceDefectFilter(request);

            if (response.Result != DefectDashboardResult.Success)
            {
                return new DefectPerformanceResponse() { Result = DefectDashboardResult.NotFound };
            }
            return new DefectPerformanceResponse()
            {
                Result = DefectDashboardResult.Success,
                PerformanceDefectList = response.PerformanceDefectList
            };
        }

        /// <summary>
        /// get low performance details by supplier or factory
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DefectPerformanceResponse> GetLowPerformanceDefectFilter(DefectPerformanceAnalysis request)
        {
            if (request == null)
                return new DefectPerformanceResponse() { Result = DefectDashboardResult.RequestNotCorrectFormat };


            var SupFactDefectAnalysisList = new List<SupFactDefectAnalysis>();


            var bookingIds = GetQueryableBookingIdList(request.TopPerformanceFilter);


            //get inspection and report details
            var reportInspList = await _repo.GetSupFactReportDetailsQueryableReport(bookingIds);

            var iqueryabledefectList = _repo.GetReportDefectQueryableList(bookingIds);


            if (!string.IsNullOrWhiteSpace(request.InnerPerformanceFilter.DefectName))
            {
                iqueryabledefectList = iqueryabledefectList.Where(x => x.DefectName != null &&
                            EF.Functions.Like(x.DefectName, $"%{request.InnerPerformanceFilter.DefectName.Trim()}%"));
            }
            
            var defectList = await iqueryabledefectList
             .Select(x => new
             {
                 x.Critical,
                 x.DefectName,
                 x.Major,
                 x.Minor,
                 x.ReportId
             })
            .GroupBy(p => p.ReportId, (key, _data) => new ReportDefectDetailsRepo
            {
                Critical = _data.Sum(x => x.Critical),
                ReportId = key,
                Minor = _data.Sum(x => x.Minor),
                Major = _data.Sum(x => x.Major),
            }).AsNoTracking().ToListAsync();

            if (!defectList.Any())
                return new DefectPerformanceResponse() { Result = DefectDashboardResult.NotFound };

            var selectedList = DefectNameList.Where(x => request.InnerPerformanceFilter.DefectSelected.Contains(x.Key))
                                                       .Select(x => x.Value).ToList();

            if (request.InnerPerformanceFilter.TypeId == (int)Supplier_Type.Factory)
            {

                SupFactDefectAnalysisList = reportInspList.GroupBy(g => g.FactoryId, (key, _data) => new SupFactDefectAnalysis()
                {
                    SupOrFactId = key,
                    Critical = defectList.Where(x => _data.Select(y => y.ReportId).Contains(x.ReportId)).Sum(x => x.Critical.GetValueOrDefault()),
                    Major = defectList.Where(x => _data.Select(y => y.ReportId).Contains(x.ReportId)).Sum(x => x.Major.GetValueOrDefault()),
                    Minor = defectList.Where(x => _data.Select(y => y.ReportId).Contains(x.ReportId)).Sum(x => x.Minor.GetValueOrDefault()),
                    SupOrFactName = _data.Select(x => x.FactoryName).FirstOrDefault(),
                    TotalReports = defectList.Count(x => _data.Select(y => y.ReportId).Contains(x.ReportId)),
                    DefectReportInfo = _data.Where(x => defectList.Select(y => y.ReportId).ToList().Contains(x.ReportId)).Select(x => new ReportDefectInfo()
                    {
                        ReportNo = x.ReportNo,
                        ReportLink = x.ReportLink,
                        FinalManualReportLink = x.FinalManualReportLink
                    }).ToList(),

                    IsMajorShow = defectList.Any(x => selectedList.Contains(x.GetType().GetProperty(DefectMajor).Name)),
                    IsMinorShow = defectList.Any(x => selectedList.Contains(x.GetType().GetProperty(DefectMinor).Name)),
                    IsCriticalShow = defectList.Any(x => selectedList.Contains(x.GetType().GetProperty(DefectCritical).Name)),

                    TotalDefect = defectList.Where(x => _data.Select(y => y.ReportId).Contains(x.ReportId) &&
                                                selectedList.Contains(x.GetType().GetProperty(DefectMinor).Name)).Sum(x => x.Minor.GetValueOrDefault()) +
                                                 defectList.Where(x => _data.Select(y => y.ReportId).Contains(x.ReportId) &&
                                                selectedList.Contains(x.GetType().GetProperty(DefectCritical).Name)).Sum(x => x.Critical.GetValueOrDefault())
                                    + defectList.Where(x => _data.Select(y => y.ReportId).Contains(x.ReportId) &&
                                    selectedList.Contains(x.GetType().GetProperty(DefectMajor).Name)).Sum(x => x.Major.GetValueOrDefault())

                }).OrderByDescending(x => x.TotalDefect).Take(DefectCountBySupplierFactory).ToList();
            }
            else if (request.InnerPerformanceFilter.TypeId == (int)Supplier_Type.Supplier_Agent)
            {
                SupFactDefectAnalysisList = reportInspList.GroupBy(g => g.SupplierId, (key, _data) => new SupFactDefectAnalysis()
                {
                    SupOrFactId = key,
                    Critical = defectList.Where(x => _data.Select(y => y.ReportId).Contains(x.ReportId)).Sum(x => x.Critical.GetValueOrDefault()),
                    Major = defectList.Where(x => _data.Select(y => y.ReportId).Contains(x.ReportId)).Sum(x => x.Major.GetValueOrDefault()),
                    Minor = defectList.Where(x => _data.Select(y => y.ReportId).Contains(x.ReportId)).Sum(x => x.Minor.GetValueOrDefault()),
                    SupOrFactName = _data.Select(x => x.SupplierName).FirstOrDefault(),
                    TotalReports = defectList.Count(x => _data.Select(y => y.ReportId).Contains(x.ReportId)),
                    DefectReportInfo = _data.Where(x => defectList.Select(y => y.ReportId).ToList().Contains(x.ReportId)).Select(x => new ReportDefectInfo()
                    {
                        ReportNo = x.ReportNo,
                        ReportLink = x.ReportLink
                    }).ToList(),

                    IsMajorShow = defectList.Any(x => selectedList.Contains(x.GetType().GetProperty(DefectMajor).Name)),
                    IsMinorShow = defectList.Any(x => selectedList.Contains(x.GetType().GetProperty(DefectMinor).Name)),
                    IsCriticalShow = defectList.Any(x => selectedList.Contains(x.GetType().GetProperty(DefectCritical).Name)),

                    TotalDefect = defectList.Where(x => _data.Select(y => y.ReportId).Contains(x.ReportId) &&
                    selectedList.Contains(x.GetType().GetProperty(DefectMinor).Name)).Sum(x => x.Minor.GetValueOrDefault()) +
                                                 defectList.Where(x => _data.Select(y => y.ReportId).Contains(x.ReportId) &&
                                    selectedList.Contains(x.GetType().GetProperty(DefectCritical).Name)).Sum(x => x.Critical.GetValueOrDefault())
                                            + defectList.Where(x => _data.Select(y => y.ReportId).Contains(x.ReportId) &&
                                            selectedList.Contains(x.GetType().GetProperty(DefectMajor).Name)).Sum(x => x.Major.GetValueOrDefault())
                }).OrderByDescending(x => x.TotalDefect).Take(DefectCountBySupplierFactory).ToList();
            }


            return new DefectPerformanceResponse()
            {
                Result = DefectDashboardResult.Success,
                PerformanceDefectList = SupFactDefectAnalysisList.Where(x => x.TotalDefect > 0).ToList()
            };
        }

        /// <summary>
        /// get defect list 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetDefectDataSource(CommonDataSourceRequest request)
        {
            var response = new DataSourceResponse();

            var data = _repo.GetDefectList();

            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                data = data.Where(x => x.Name != null && EF.Functions.Like(x.Name, $"%{request.SearchText.Trim()}%"));
            }

            var defectList = await data.Select(x => x.Name).GroupBy(g => g, (key, _data) => new CommonDataSource()
            {
                Name = key
            }).OrderBy(x => x.Name).Skip(request.Skip).Take(request.Take).ToListAsync();

            if (!defectList.Any())
                response.Result = DataSourceResult.CannotGetList;

            response.DataSourceList = defectList;
            response.Result = DataSourceResult.Success;

            return response;
        }

        /// <summary>
        /// get low perfromance factory or supplier defect list for export
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DefectPerformanceExportResponse> GetLowPerformanceExport(DefectPerformanceAnalysis request)
        {
            var response = await GetLowPerformanceDefectFilter(request);

            if (response.Result == DefectDashboardResult.Success)
            {
                return new DefectPerformanceExportResponse()
                {
                    RequestFilters = await SetDashboardExportFilter(request.TopPerformanceFilter),
                    RequestSubFilters = SetSubSuppOrFactExportFilter(request.InnerPerformanceFilter),
                    Result = DefectDashboardResult.Success,
                    PerformanceDefectList = response.PerformanceDefectList
                };
            }
            return new DefectPerformanceExportResponse() { Result = DefectDashboardResult.NotFound };
        }

        /// <summary>
        /// get defect count with name list for each factory or supplier
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DefectCountResponse> GetDefectCountByFilters(DefectPerformanceAnalysis request)
        {
            if (request == null)
                return new DefectCountResponse() { Result = DefectDashboardResult.RequestNotCorrectFormat };

            if (request.InnerPerformanceFilter.TypeId == (int)Supplier_Type.Supplier_Agent)
            {
                request.TopPerformanceFilter.SupplierId = request.InnerPerformanceFilter.SupOrFactId;
            }
            else if (request.InnerPerformanceFilter.TypeId == (int)Supplier_Type.Factory)
            {
                request.TopPerformanceFilter.FactoryIds = new int[] { request.InnerPerformanceFilter.SupOrFactId };
            }

            var defectList = new List<DefectCount>();

            var bookingIds = GetQueryableBookingIdList(request.TopPerformanceFilter);
            

            //get report defect details
            var iqueryabledefectList = _repo.GetReportDefectQueryableList(bookingIds);

            //if defect name has, add where condition
            if (!string.IsNullOrWhiteSpace(request.InnerPerformanceFilter.DefectName))
            {
                iqueryabledefectList = iqueryabledefectList.Where(x => x.DefectName != null &&
                            EF.Functions.Like(x.DefectName, $"%{request.InnerPerformanceFilter.DefectName.Trim()}%"));
            }
            if (request.InnerPerformanceFilter.DefectSelect != null)
            {
                //get defect report details
                if (request.InnerPerformanceFilter.DefectSelect == DefectCriticalId)
                {
                    iqueryabledefectList = iqueryabledefectList.Where(x => x.Critical > 0);

                    defectList = await iqueryabledefectList
                     .Select(x => new
                     {
                         x.Critical,
                         x.DefectName,
                     })
                    .GroupBy(p => p.DefectName, (key, _data) => new DefectCount
                    {
                        Count = _data.Sum(x => x.Critical),
                        DefectName = key,
                    }).AsNoTracking().ToListAsync();
                }
                else if (request.InnerPerformanceFilter.DefectSelect == DefectMajorId)
                {
                    iqueryabledefectList = iqueryabledefectList.Where(x => x.Major > 0);

                    defectList = await iqueryabledefectList
                     .Select(x => new
                     {
                         x.Major,
                         x.DefectName,
                     })
                    .GroupBy(p => p.DefectName, (key, _data) => new DefectCount
                    {
                        Count = _data.Sum(x => x.Major),
                        DefectName = key,
                    }).AsNoTracking().ToListAsync();
                }
                else if (request.InnerPerformanceFilter.DefectSelect == DefectMinorId)
                {
                    iqueryabledefectList = iqueryabledefectList.Where(x => x.Minor > 0);

                    defectList = await iqueryabledefectList
                     .Select(x => new
                     {
                         x.Minor,
                         x.DefectName,
                     })
                    .GroupBy(p => p.DefectName, (key, _data) => new DefectCount
                    {
                        Count = _data.Sum(x => x.Minor),
                        DefectName = key,
                    }).AsNoTracking().ToListAsync();
                }
            }
            if (defectList.Any())
                return new DefectCountResponse()
                {
                    Result = DefectDashboardResult.Success,
                    PerformanceDefectList = defectList
                };
            return new DefectCountResponse() { Result = DefectDashboardResult.NotFound };


        }

        /// <summary>
        /// get defect photo list with description for each factory or supplier
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DefectPhotoResponse> GetDefectPhotoListByFilters(DefectPerformanceAnalysis request)
        {
            if (request == null)
                return new DefectPhotoResponse() { Result = DefectDashboardResult.RequestNotCorrectFormat };

            if (request.InnerPerformanceFilter.TypeId == (int)Supplier_Type.Supplier_Agent)
            {
                request.TopPerformanceFilter.SupplierId = request.InnerPerformanceFilter.SupOrFactId;
            }
            else if (request.InnerPerformanceFilter.TypeId == (int)Supplier_Type.Factory)
            {
                request.TopPerformanceFilter.FactoryIds = new int[] { request.InnerPerformanceFilter.SupOrFactId };
            }



            var defectList = new List<DefectPhoto>();


            //var reportIdList = reportDetails.BookingReportModel.Where(x => x.ReportId > 0).Select(x => x.ReportId.GetValueOrDefault()).ToList();

            //get defect details
            if (request.InnerPerformanceFilter.DefectSelect != null && request.InnerPerformanceFilter.DefectSelect > 0)
            {
                var bookingIds = GetQueryableBookingIdList(request.TopPerformanceFilter);
                if (bookingIds == null || !bookingIds.Any())
                {
                    return new DefectPhotoResponse() { Result = DefectDashboardResult.NotFound };
                }


                var reportIdList = _inspManager.GetReportIdDataByQueryableBooking(bookingIds);
                if (reportIdList == null || !reportIdList.Any())
                {
                    return new DefectPhotoResponse() { Result = DefectDashboardResult.NotFound };
                }


                var defectData = _repo.GetDefectPhotoQueryableList(reportIdList);
                //GetDefectPhotoQueryableList

                if (!string.IsNullOrWhiteSpace(request.InnerPerformanceFilter.DefectName))
                {
                    defectData = defectData.Where(x => x.DefectName != null && EF.Functions.Like(x.DefectName, $"%{request.InnerPerformanceFilter.DefectName.Trim()}%"));
                }

                if (request.InnerPerformanceFilter.DefectSelect == DefectCriticalId)
                {
                    defectData = defectData.Where(x => x.Critical > 0);
                }

                else if (request.InnerPerformanceFilter.DefectSelect == DefectMajorId)
                {
                    defectData = defectData.Where(x => x.Major > 0);
                }

                else if (request.InnerPerformanceFilter.DefectSelect == DefectMinorId)
                {
                    defectData = defectData.Where(x => x.Minor > 0);
                }

                defectList = await defectData.Select(x => new DefectPhoto()
                {
                    DefectPhotoPath = x.DefectPhotoPath,
                    Description = x.Description
                }).AsNoTracking().ToListAsync();
            }

            if (defectList.Any())
            {
                return new DefectPhotoResponse()
                {
                    Result = DefectDashboardResult.Success,
                    PerformanceDefectList = defectList
                };
            }
            return new DefectPhotoResponse() { Result = DefectDashboardResult.NotFound };

        }

        /// <summary>
        /// get defect list by country 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CountryDefectChartResponse> GetCountryDefectList(DefectDashboardFilterRequest request)
        {
            var countryDetails = await GetCountryDefectFilter(request);

            if (countryDetails.Result == DefectDashboardResult.Success)
            {
                //assign the color for each defect
                for (int i = 1; i <= countryDetails.Data.Count; i++)
                {
                    countryDetails.Data[i - 1].Color = DefectPerCountryColorList.GetValueOrDefault(i);
                }

                return new CountryDefectChartResponse()
                {
                    Result = DefectDashboardResult.Success,
                    Data = countryDetails.Data,
                    CountryList = countryDetails.CountryList
                };
            }
            return new CountryDefectChartResponse() { Result = DefectDashboardResult.NotFound };
        }

        /// <summary>
        /// get defects by country  list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CountryDefectChartResponse> GetCountryDefectFilter(DefectDashboardFilterRequest request)
        {

            var countryList = new List<CountryModel>();
            //reportid
            var bookingIds = GetQueryableBookingIdList(request);

            var reportIdList = _inspManager.GetReportIdDataByQueryableBooking(bookingIds);

            var defectList = await _repo.GetDefectListByQueryableReportIds(reportIdList);
            // GetDefectListByQueryableReportIds

            if (!defectList.Any())
                return new CountryDefectChartResponse() { Result = DefectDashboardResult.NotFound };

            //get product tran - report id and country id list
            var factoryCountryData = await _repo.GetCountryInspReportData(reportIdList);
            //GetCountryInspReportData



            var DefectCountryReportList = new List<DefectCountryReport>();

            if (!factoryCountryData.Any())
                return new CountryDefectChartResponse() { Result = DefectDashboardResult.NotFound };

            //loop the country and bind the defect with country by report id in another lsit
            foreach (var item in factoryCountryData)
            {
                var defectDetailsList = defectList.Where(x => x.ReportId == item.ReportId && !string.IsNullOrWhiteSpace(x.DefectName)).ToList();

                if (!DefectCountryReportList.Any(x => x.ReportId == item.ReportId))
                {
                    Parallel.ForEach(defectDetailsList, defectItem =>
                    {
                        var defectCountry = new DefectCountryReport
                        {
                            CountryId = item.CountryId,
                            CountryName = item.CountryName,
                            DefectName = defectItem.DefectName,
                            Count = defectItem.Critical.GetValueOrDefault() + defectItem.Major.GetValueOrDefault() + defectItem.Minor.GetValueOrDefault(),
                            ReportId = item.ReportId.GetValueOrDefault()
                        };
                        DefectCountryReportList.Add(defectCountry);
                    }
);
                }
            }

            //group the defect name
            var groupbyDefects = DefectCountryReportList.GroupBy(x => x.DefectName, (key, _data) => new CountryDefectModel()
            {
                DefectName = key,
                Count = _data.Sum(x => x.Count),
                //same defect name with different country 
                CountryDefectData = _data.Where(x => x.DefectName == key).GroupBy(x => x.CountryId, (key1, _data1) => new DefectCountModel()
                {
                    DefectName = key,
                    CountryId = key1,
                    Count = _data.Sum(y => y.Count)
                }).ToList()
            }).OrderByDescending(x => x.Count).Take(CountryDefectCount).ToList();

            //get countryids
            var countryIds = groupbyDefects.SelectMany(x => x.CountryDefectData.Select(y => y.CountryId)).Distinct().Take(DefectDashboardCountryCount).ToList();

            //get country list
            foreach (var _countryId in countryIds)
            {
                var countryData = new CountryModel()
                {
                    CountryId = factoryCountryData.Where(x => _countryId == x.CountryId).Select(x => x.CountryId).FirstOrDefault(),
                    CountryName = factoryCountryData.Where(x => _countryId == x.CountryId).Select(x => x.CountryName).FirstOrDefault()
                };

                countryList.Add(countryData);
            }

            return new CountryDefectChartResponse()
            {
                Result = DefectDashboardResult.Success,
                Data = groupbyDefects,
                CountryList = countryList
            };


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<DefectCountryChartExportResponse> GetCountryDefectListExport(DefectDashboardFilterRequest request)
        {
            //country defect apply filters
            var countryRes = await GetCountryDefectFilter(request);

            if (countryRes.Result == DefectDashboardResult.Success)
            {
                return new DefectCountryChartExportResponse()
                {
                    Result = DefectDashboardResult.Success,
                    RequestFilters = await SetDashboardExportFilter(request),
                    CountryNameList = countryRes.CountryList,
                    Data = countryRes.Data
                };
            }
            return new DefectCountryChartExportResponse() { Result = DefectDashboardResult.NotFound };
        }

        /// <summary>
        ///Get Queryable BookingId List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private IQueryable<int> GetQueryableBookingIdList(DefectDashboardFilterRequest request)
        {

            var inspectionQuery = _sharedInspection.GetAllInspectionQuery();
            var inspectionQueryRequest = _sharedInspection.GetDashBoardInspectionQueryRequestMap(request);
            var data = _sharedInspection.GetInspectionQuerywithRequestFilters(inspectionQueryRequest, inspectionQuery);

            if (request.FromDate?.ToDateTime() != null && request.ToDate?.ToDateTime() != null)
            {
                data = data.Where(x => x.ServiceDateTo <= request.ToDate.ToDateTime() && x.ServiceDateTo >= request.FromDate.ToDateTime());
            }

            //get booking ids
            var bookingIds = data.Select(x => x.Id);

            return bookingIds;
        }

        public async Task<ReportDefectResponse> GetReportDefectPareto(DefectDashboardFilterRequest request)
        {
            if (request == null)
                return new ReportDefectResponse() { Result = ReportDefectResult.NotFound };

            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            if (request.pageSize == null || request.pageSize.Value == 0)
                request.pageSize = 10;

            int skip = (request.Index.Value - 1) * request.pageSize.Value;

            int take = request.pageSize.Value;

            var bookingIds = GetQueryableBookingIdList(request);

            var reportDefect = _repo.GetQueryableReportDefect(bookingIds);

            if (request.SelectedBrandIdList != null && request.SelectedBrandIdList.Any())
            {
                reportDefect = reportDefect.Where(x => request.SelectedBrandIdList.Contains(x.BrandId));
            }

            var groupByRequestFilter = new GroupByRequestFilter();
            if (request.GroupByFilter != null && request.GroupByFilter.Any())
            {
                groupByRequestFilter.FactoryCountry = request.GroupByFilter.Any(x => x == GroupByFilter.FactoryCountry);
                groupByRequestFilter.Supplier = request.GroupByFilter.Any(x => x == GroupByFilter.Supplier);
                groupByRequestFilter.Factory = request.GroupByFilter.Any(x => x == GroupByFilter.Factory);
                groupByRequestFilter.Brand = request.GroupByFilter.Any(x => x == GroupByFilter.Brand);
            }

            var totalCount = await reportDefect.GroupBy(x => new
            {
                FactoryCountryId = groupByRequestFilter.FactoryCountry ? x.FactoryCountryId : 0,
                FactoryCountryName = groupByRequestFilter.FactoryCountry ? x.FactoryCountryName : null,
                SupplierId = groupByRequestFilter.Supplier ? x.SupplierId : 0,
                SupplierName = groupByRequestFilter.Supplier ? x.SupplierName : null,
                FactoryId = groupByRequestFilter.Factory ? x.FactoryId : 0,
                FactoryName = groupByRequestFilter.Factory ? x.FactoryName : null,
                BrandId = groupByRequestFilter.Brand ? x.BrandId : 0,
                BrandName = groupByRequestFilter.Brand ? x.BrandName : null
            }).Select(x => new ReportDefectData()
            {
                FactoryCountryId = x.Key.FactoryCountryId,
                FactoryCountryName = x.Key.FactoryCountryName,
                SupplierId = x.Key.SupplierId,
                SupplierName = x.Key.SupplierName,
                FactoryId = x.Key.FactoryId,
                FactoryName = x.Key.FactoryName,
                BrandId = x.Key.BrandId,
                BrandName = x.Key.BrandName,
                InspectionCount = x.Select(x => x.InspectionId).Distinct().Count(),
                ReportCount = x.Select(x => x.ReportId).Distinct().Count(),
            }).CountAsync();

            var reportDefectList = (await reportDefect.ToListAsync()).GroupBy(x => new
            {
                FactoryCountryId = groupByRequestFilter.FactoryCountry ? x.FactoryCountryId : 0,
                FactoryCountryName = groupByRequestFilter.FactoryCountry ? x.FactoryCountryName : null,
                SupplierId = groupByRequestFilter.Supplier ? x.SupplierId : 0,
                SupplierName = groupByRequestFilter.Supplier ? x.SupplierName : null,
                FactoryId = groupByRequestFilter.Factory ? x.FactoryId : 0,
                FactoryName = groupByRequestFilter.Factory ? x.FactoryName : null,
                BrandId = groupByRequestFilter.Brand ? x.BrandId : 0,
                BrandName = groupByRequestFilter.Brand ? x.BrandName : null
            }).Select(x => new ReportDefectData()
            {
                FactoryCountryId = x.Key.FactoryCountryId,
                FactoryCountryName = x.Key.FactoryCountryName,
                SupplierId = x.Key.SupplierId,
                SupplierName = x.Key.SupplierName,
                FactoryId = x.Key.FactoryId,
                FactoryName = x.Key.FactoryName,
                BrandId = x.Key.BrandId,
                BrandName = x.Key.BrandName,
                InspectionCount = x.Select(x => x.InspectionId).Distinct().Count(),
                ReportCount = x.Select(x => x.ReportId).Distinct().Count(),
                TotalDefectCount = x.Sum(x => x.Critical.GetValueOrDefault()) + x.Sum(x => x.Major.GetValueOrDefault()) + x.Sum(x => x.Minor.GetValueOrDefault()),
                Defects = x.GroupBy(z => new { z.DefectName }).Select(y => new ReportDefectCountData()
                {
                    DefectName = y.Key.DefectName,
                    Critical = y.Sum(a => a.Critical.GetValueOrDefault()),
                    Major = y.Sum(a => a.Major.GetValueOrDefault()),
                    Minor = y.Sum(a => a.Minor.GetValueOrDefault())
                }).OrderByDescending(x => x.DefectCount).Take(10).ToList()
            }).OrderByDescending(x => x.InspectionCount).ThenByDescending(x => x.ReportCount).Skip(skip).Take(take).ToList();

            return new ReportDefectResponse()
            {
                Data = reportDefectList,
                Result = ReportDefectResult.Success,
                TotalCount = totalCount,
                Index = request.Index.Value,
                PageSize = request.pageSize.Value,
                PageCount = (totalCount / request.pageSize.Value) + (totalCount % request.pageSize.Value > 0 ? 1 : 0),
            };
        }

        public async Task<DataTable> ExportReportDefectPareto(DefectDashboardFilterRequest request)
        {
            var bookingIds = GetQueryableBookingIdList(request);

            var reportDefect = _repo.GetQueryableReportDefect(bookingIds);

            if (request.SelectedBrandIdList != null && request.SelectedBrandIdList.Any())
            {
                reportDefect = reportDefect.Where(x => request.SelectedBrandIdList.Contains(x.BrandId));
            }

            var groupByRequestFilter = new GroupByRequestFilter();
            if (request.GroupByFilter != null && request.GroupByFilter.Any())
            {
                groupByRequestFilter.FactoryCountry = request.GroupByFilter.Any(x => x == GroupByFilter.FactoryCountry);
                groupByRequestFilter.Supplier = request.GroupByFilter.Any(x => x == GroupByFilter.Supplier);
                groupByRequestFilter.Factory = request.GroupByFilter.Any(x => x == GroupByFilter.Factory);
                groupByRequestFilter.Brand = request.GroupByFilter.Any(x => x == GroupByFilter.Brand);
            }

            var defectGroupList = await reportDefect.GroupBy(x => new
            {
                FactoryCountryId = groupByRequestFilter.FactoryCountry ? x.FactoryCountryId : 0,
                FactoryCountryName = groupByRequestFilter.FactoryCountry ? x.FactoryCountryName : null,
                SupplierId = groupByRequestFilter.Supplier ? x.SupplierId : 0,
                SupplierName = groupByRequestFilter.Supplier ? x.SupplierName : null,
                FactoryId = groupByRequestFilter.Factory ? x.FactoryId : 0,
                FactoryName = groupByRequestFilter.Factory ? x.FactoryName : null,
                BrandId = groupByRequestFilter.Brand ? x.BrandId : 0,
                BrandName = groupByRequestFilter.Brand ? x.BrandName : null
            }).Select(x => new ExportDefectGroupList()
            {
                FactoryCountryId = x.Key.FactoryCountryId,
                FactoryCountryName = x.Key.FactoryCountryName,
                SupplierId = x.Key.SupplierId,
                SupplierName = x.Key.SupplierName,
                FactoryId = x.Key.FactoryId,
                FactoryName = x.Key.FactoryName,
                BrandId = x.Key.BrandId,
                BrandName = x.Key.BrandName,
                InspectionCount = x.Select(x => x.InspectionId).Distinct().Count(),
                ReportCount = x.Select(x => x.ReportId).Distinct().Count(),
                TotalDefectCount = x.Sum(x => x.Critical.GetValueOrDefault()) + x.Sum(x => x.Major.GetValueOrDefault()) + x.Sum(x => x.Minor.GetValueOrDefault()),
            }).ToListAsync();

            var defectList = await reportDefect.GroupBy(x => new
            {
                FactoryCountryId = groupByRequestFilter.FactoryCountry ? x.FactoryCountryId : 0,
                FactoryCountryName = groupByRequestFilter.FactoryCountry ? x.FactoryCountryName : null,
                SupplierId = groupByRequestFilter.Supplier ? x.SupplierId : 0,
                SupplierName = groupByRequestFilter.Supplier ? x.SupplierName : null,
                FactoryId = groupByRequestFilter.Factory ? x.FactoryId : 0,
                FactoryName = groupByRequestFilter.Factory ? x.FactoryName : null,
                BrandId = groupByRequestFilter.Brand ? x.BrandId : 0,
                BrandName = groupByRequestFilter.Brand ? x.BrandName : null,
                x.DefectName
            }).Select(x => new ExportDefectList()
            {
                FactoryCountryId = x.Key.FactoryCountryId,
                FactoryCountryName = x.Key.FactoryCountryName,
                SupplierId = x.Key.SupplierId,
                SupplierName = x.Key.SupplierName,
                FactoryId = x.Key.FactoryId,
                FactoryName = x.Key.FactoryName,
                BrandId = x.Key.BrandId,
                BrandName = x.Key.BrandName,
                DefectName = x.Key.DefectName,
                Critical = x.Sum(a => a.Critical.GetValueOrDefault()),
                Major = x.Sum(a => a.Major.GetValueOrDefault()),
                Minor = x.Sum(a => a.Minor.GetValueOrDefault())
            }).ToListAsync();

            var dataTable = _helper.ConvertToDataTableWithCaption(defectList);

            MapReportDefectPareto(dataTable, groupByRequestFilter, defectGroupList);

            if (defectList != null && defectList.Any())
            {
                DataView dv = new DataView(dataTable);
                dv.Sort = ExportDefectParetoSortDataColumns;
                dataTable = dv.ToTable();

                List<string> removeColumn = new List<string>();
                removeColumn.AddRange(RemoveColumn);
                if (!groupByRequestFilter.FactoryCountry)
                    removeColumn.Add("FactoryCountryName");
                if (!groupByRequestFilter.Supplier)
                    removeColumn.Add("SupplierName");
                if (!groupByRequestFilter.Factory)
                    removeColumn.Add("FactoryName");
                if (!groupByRequestFilter.Brand)
                    removeColumn.Add("BrandName");
                RemoveCloumnToDataTable(dataTable, removeColumn);

                //moved the column to the end
                dataTable.Columns["DefectName"]?.SetOrdinal(dataTable.Columns.Count - 1);
                dataTable.Columns["DefectCount"]?.SetOrdinal(dataTable.Columns.Count - 1);
                dataTable.Columns["Total Defects %"]?.SetOrdinal(dataTable.Columns.Count - 1);
                dataTable.Columns["Critical"]?.SetOrdinal(dataTable.Columns.Count - 1);
                dataTable.Columns["Major"]?.SetOrdinal(dataTable.Columns.Count - 1);
                dataTable.Columns["Minor"]?.SetOrdinal(dataTable.Columns.Count - 1);
            }
            return dataTable;
        }

        public DataTable MapReportDefectPareto(DataTable dataTable, GroupByRequestFilter groupByRequestFilter, List<ExportDefectGroupList> defectGroupList)
        {
            if (defectGroupList != null && defectGroupList.Any())
            {
                dataTable.Columns.Add("Total Defects %", typeof(double));
                dataTable.Columns.Add("Inspections", typeof(int));
                dataTable.Columns.Add("Reports", typeof(int));

                foreach (DataRow row in dataTable.Rows)
                {
                    var rowData = defectGroupList.ToList();
                    if (groupByRequestFilter.FactoryCountry)
                        rowData = rowData.Where(x => x.FactoryCountryId == Convert.ToInt32(row["FactoryCountryId"].ToString())).ToList();

                    if (groupByRequestFilter.Supplier)
                        rowData = rowData.Where(x => x.SupplierId == Convert.ToInt32(row["SupplierId"].ToString())).ToList();

                    if (groupByRequestFilter.Factory)
                        rowData = rowData.Where(x => x.FactoryId == Convert.ToInt32(row["FactoryId"].ToString())).ToList();

                    if (groupByRequestFilter.Brand)
                        rowData = rowData.Where(x => x.BrandId == Convert.ToInt32(row["BrandId"].ToString())).ToList();

                    var data = rowData.First();
                    double defectCount = Convert.ToInt32(row["DefectCount"].ToString());

                    row["Inspections"] = data.InspectionCount;
                    row["Reports"] = data.ReportCount;
                    row["Total Defects %"] = Math.Round(defectCount / data.TotalDefectCount * 100, 2);
                }
            }
            return dataTable;
        }

        public DataTable RemoveCloumnToDataTable(DataTable dataTable, List<string> removedColumns)
        {
            // removed columns 
            foreach (var column in removedColumns)
            {
                dataTable.Columns.Remove(column);
            }
            return dataTable;
        }
    }
}
