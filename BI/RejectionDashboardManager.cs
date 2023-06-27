using BI.Utilities;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.Customer;
using DTO.Dashboard;
using DTO.Eaqf;
using DTO.InspectionCustomerDecision;
using DTO.Manday;
using DTO.RejectionDashboard;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static BI.TenantProvider;
using static DTO.Common.Static_Data_Common;

namespace BI
{
    public class RejectionDashboardManager : ApiCommonData, IRejectionDashboardManager
    {
        private readonly IRejectionDashboardRepository _repo = null;
        private readonly IDashboardManager _dashboardManager = null;
        private readonly ICustomerManager _cusManager = null;
        private readonly ILocationRepository _locationRepo = null;
        private readonly ISupplierManager _supManager = null;
        private readonly ICustomerBrandManager _cusBrandManager = null;
        private readonly ICustomerBuyerManager _cusBuyerManager = null;
        private readonly ICustomerCollectionManager _cusCollectionManager = null;
        private readonly ICustomerDepartmentManager _cusDeptManager = null;
        private readonly ISharedInspectionManager _sharedInspection = null;
        private readonly ICustomerProductManager _customerProductManager = null;
        private readonly IReferenceManager _referenceManager = null;
        private readonly ITenantProvider _filterService = null;
        private readonly IHelper _helper = null;
        private readonly IDefectDashboardManager _defectDashboardManager = null;

        public RejectionDashboardManager(IDashboardManager dashboardManager, IRejectionDashboardRepository repo, ICustomerManager cusManager,
            ILocationRepository locationRepo,
            ISupplierManager supManager,
            ICustomerBrandManager cusBrandManager,
            ICustomerBuyerManager cusBuyerManager,
            ICustomerCollectionManager cusCollectionManager,
        ICustomerDepartmentManager cusDeptManager, ISharedInspectionManager sharedInspection,
        ICustomerProductManager customerProductManager, IReferenceManager referenceManager,
        ITenantProvider filterService, IHelper helper, IDefectDashboardManager defectDashboardManager)
        {
            _repo = repo;
            _dashboardManager = dashboardManager;
            _cusManager = cusManager;
            _locationRepo = locationRepo;
            _supManager = supManager;
            _cusBrandManager = cusBrandManager;
            _cusBuyerManager = cusBuyerManager;
            _cusCollectionManager = cusCollectionManager;
            _cusDeptManager = cusDeptManager;
            _sharedInspection = sharedInspection;
            _customerProductManager = customerProductManager;
            _referenceManager = referenceManager;
            _filterService = filterService;
            _helper = helper;
            _defectDashboardManager = defectDashboardManager;
        }

        private async Task<List<BookingDetail>> GetBookingDataByRequest(RejectionDashboardFilterRequest request)
        {
            CustomerDashboardFilterRequest req = new CustomerDashboardFilterRequest
            {
                CustomerId = request.CustomerId,
                SupplierId = request.SupplierId,
                ServiceDateFrom = request.ServiceDateFrom,
                ServiceDateTo = request.ServiceDateTo,
                SelectedCountryIdList = request.SelectedCountryIdList,
                SelectedBrandIdList = request.SelectedBrandIdList,
                SelectedBuyerIdList = request.SelectedBuyerIdList,
                SelectedCollectionIdList = request.SelectedCollectionIdList,
                SelectedDeptIdList = request.SelectedDeptIdList,
                StatusIdList = InspectedStatusList
            };

            return await _dashboardManager.GetBookingDetails(req);
        }

        /// <summary>
        /// get the API Report Result Data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<RejectResultDashboardResponse> GetAPIResultDashboard(RejectionDashboardFilterRequest request)
        {
            CustomerDashboardFilterRequest req = new CustomerDashboardFilterRequest
            {
                CustomerId = request.CustomerId,
                SupplierId = request.SupplierId,
                ServiceDateFrom = request.ServiceDateFrom,
                ServiceDateTo = request.ServiceDateTo,
                SelectedCountryIdList = request.SelectedCountryIdList,
                SelectedBrandIdList = request.SelectedBrandIdList,
                SelectedBuyerIdList = request.SelectedBuyerIdList,
                SelectedCollectionIdList = request.SelectedCollectionIdList,
                SelectedDeptIdList = request.SelectedDeptIdList,
                StatusIdList = InspectedStatusList
            };

            //get the booking details
            var bookingData = await GetBookingDataByRequest(request);
            var inspectionIdList = bookingData.Where(x => InspectedStatusList.Contains(x.StatusId)).Select(x => x.InspectionId).Distinct().ToList();

            //get the API report result data
            var res = inspectionIdList != null && inspectionIdList.Any() ? await _dashboardManager.GetAPIRADashboard(req) : null;

            if (res == null || !res.Any())
            {
                return new RejectResultDashboardResponse { Result = RejectionDashboardResult.NotFound };
            }

            return new RejectResultDashboardResponse
            {
                Data = res.OrderByDescending(x => x.TotalCount).ToList(),
                BookingIdList = inspectionIdList,
                TotalReports = res.Sum(x => x.TotalCount),
                Result = RejectionDashboardResult.Success
            };
        }

        /// <summary>
        /// get the API Report Result Data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<RejectResultDashboardResponse> GetAllAPIResultDashboard(RejectionDashboardSearchRequest request)
        {
            //get the booking details

            var bookingIds = GetQueryableBookingIdList(request);

            //get the API report result data
            var res = await _dashboardManager.GetQueriableAPIRADashboard(bookingIds);
            var inspectionIdList = await bookingIds.ToListAsync();

            if (res == null || !res.Any())
            {
                return new RejectResultDashboardResponse { Result = RejectionDashboardResult.NotFound };
            }

            return new RejectResultDashboardResponse
            {
                Data = res.OrderByDescending(x => x.TotalCount).ToList(),
                BookingIdList = inspectionIdList,
                TotalReports = res.Sum(x => x.TotalCount),
                Result = RejectionDashboardResult.Success
            };
        }

        /// <summary>
        /// get the customer report result data
        /// </summary>
        /// <param name="inspectionIdList"></param>
        /// <returns></returns>
        public async Task<RejectResultDashboardResponse> GetCustomerResultDashboard(RejectionDashboardSearchRequest request)
        {
            //get the customer result data
            var bookingIds = GetQueryableBookingIdList(request);

            var res = await _dashboardManager.GetQueryableCustomerResultDashBoard(bookingIds);

            if (res == null || !res.Any())
            {
                return new RejectResultDashboardResponse { Result = RejectionDashboardResult.NotFound };
            }

            return new RejectResultDashboardResponse
            {
                Data = res.ConvertAll(x => new CustomerAPIRADashboard
                {
                    StatusName = x.StatusName,
                    StatusColor = x.StatusColor,
                    TotalCount = x.TotalCount
                }).OrderByDescending(x => x.TotalCount).ToList(),
                TotalReports = res.Sum(x => x.TotalCount),
                Result = RejectionDashboardResult.Success
            };
        }

        /// <summary>
        /// get the report result based on product category data
        /// </summary>
        /// <param name="inspectionIdList"></param>
        /// <returns></returns>
        public async Task<ProductCategoryChartResponse> GetProductCategoryResultDashboard(RejectionDashboardSearchRequest request)
        {
            //get the booking details

            var bookingIds = GetQueryableBookingIdList(request);


            var res = await _repo.GetProductCategoryDashboard(bookingIds, isExport: false);

            if (res == null || !res.Any())
            {
                return new ProductCategoryChartResponse { Result = RejectionDashboardResult.NotFound };
            }

            var legendList = res.Where(x => x.ResultId > 0).GroupBy(p => p.Id, (key, _data) =>
            new CommonDataSource
            {
                Id = _data.Sum(x => x.TotalCount),
                Name = _data.Select(x => x.Name).FirstOrDefault()
            }).ToList();

            //group by the top 7 data based on result
            var items = res.Where(x => x.ResultId > 0).GroupBy(p => p.ResultId, (key, _data) =>
            new ResultData
            {
                ResultName = _data.Select(x => x.ResultName).FirstOrDefault(),
                Count = _data.Sum(y => y.TotalCount),
                Color = CustomerAPIRADashboardColor.GetValueOrDefault(key.GetValueOrDefault(), ""),
                Data = _data
            }).OrderByDescending(x => x.Count).ToList();

            //fetch the product category names for the X axis
            var productCategoryList = res.Select(x => new CommonDataSource { Id = x.Id.GetValueOrDefault(), Name = x.Name });

            return new ProductCategoryChartResponse
            {
                Data = items,
                ProductCategoryList = productCategoryList.GroupBy(x => x.Id).Select(group => group.First()).Take(7),
                TotalReports = items.Sum(x => x.Count),
                LegendList = legendList,
                Result = RejectionDashboardResult.Success
            };
        }

        /// <summary>
        /// Export the api result data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<RejectionCommonExport> ExportAPIResultDashboard(RejectionDashboardSearchRequest request)
        {
            var response = new RejectionCommonExport();

            var bookingIds = GetQueryableBookingIdList(request);

            var res = await _dashboardManager.GetQueriableAPIRADashboard(bookingIds);

            if (res == null || !res.Any())
            {
                return new RejectionCommonExport { Data = null };
            }

            response.RequestFilters = await SetDashboardExportFilter(request);

            response.Data = res.ConvertAll(x => new RejectionDashboardCommonItem
            {
                Name = x.StatusName,
                Count = x.TotalCount
            });

            response.Total = res.Sum(x => x.TotalCount);

            return response;
        }

        /// <summary>
        /// export the customer result data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<RejectionCommonExport> ExportCustomerResultDashboard(RejectionDashboardSearchRequest request)
        {
            var response = new RejectionCommonExport();

            var bookingIds = GetQueryableBookingIdList(request);

            //get the API report result data
            var res = await _dashboardManager.GetQueryableCustomerResultDashBoard(bookingIds);

            if (res == null || !res.Any())
            {
                return new RejectionCommonExport { Data = null };
            }

            response.RequestFilters = await SetDashboardExportFilter(request);

            response.Data = res.ConvertAll(x => new RejectionDashboardCommonItem
            {
                Name = x.StatusName,
                Count = x.TotalCount
            });

            response.Total = res.Sum(x => x.TotalCount);

            return response;
        }

        /// <summary>
        /// Get the dropdown values to specify in the export file 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<RejectionDashboardRequestExport> SetExportFilter(RejectionDashboardFilterRequest request)
        {
            var response = new RejectionDashboardRequestExport();

            response.ServiceDateFrom = request.ServiceDateFrom == null || !CheckDate(request.ServiceDateFrom) ? "" : request.ServiceDateFrom.ToDateTime().ToString(StandardDateFormat);
            response.ServiceDateTo = request.ServiceDateTo == null || !CheckDate(request.ServiceDateTo) ? "" : request.ServiceDateTo.ToDateTime().ToString(StandardDateFormat);

            //get the customer name
            if (request.CustomerId > 0)
            {
                var customers = await _cusManager.GetCustomerByCustomerId(request.CustomerId.GetValueOrDefault());
                response.CustomerName = customers.DataSourceList != null && customers.DataSourceList.Any() ? string.Join(", ", customers.DataSourceList.Select(x => x.Name)) : "";
            }

            //get the country name
            if (request.SelectedCountryIdList != null && request.SelectedCountryIdList.Any())
            {
                var countryList = await _locationRepo.GetCountryByIds(request.SelectedCountryIdList.Where(x => x.HasValue).Select(x => x.Value).ToList());
                response.FactoryCountry = countryList != null && countryList.Any() ? string.Join(", ", countryList.Select(x => x.Name)) : "";
            }

            //get the supplier name
            if (request.SupplierId > 0)
            {
                var supIdList = new[] { request.SupplierId.GetValueOrDefault() }.ToList();
                var supList = await _supManager.GetSupplierById(supIdList);
                response.SupplierName = supList.DataSourceList != null && supList.DataSourceList.Any() ? string.Join(", ", supList.DataSourceList.Select(x => x.Name)) : "";
            }

            //selected brand name list
            if (request.SelectedBrandIdList != null && request.SelectedBrandIdList.Any())
            {
                var brandNameList = await _cusBrandManager.GetBrandNameByBrandId(request.SelectedBrandIdList.Where(x => x.HasValue).Select(x => x.Value));
                response.Brand = string.Join(", ", brandNameList);
            }

            //selected buyer name list
            if (request.SelectedBuyerIdList != null && request.SelectedBuyerIdList.Any())
            {
                var buyerNameList = await _cusBuyerManager.GetBuyerNameByBuyerIds(request.SelectedBuyerIdList.Where(x => x.HasValue).Select(x => x.Value));
                response.Buyer = string.Join(", ", buyerNameList);
            }

            //selected department name list
            if (request.SelectedDeptIdList != null && request.SelectedDeptIdList.Any())
            {
                var deptNameList = await _cusDeptManager.GetDeptNameByDeptIds(request.SelectedDeptIdList.Where(x => x.HasValue).Select(x => x.Value));
                response.Department = string.Join(", ", deptNameList);
            }

            //selected collection name list
            if (request.SelectedCollectionIdList != null && request.SelectedCollectionIdList.Any())
            {
                var collectionNameList = await _cusCollectionManager.GetCollectionNameByCollectionIds(request.SelectedCollectionIdList.Select(x => x.GetValueOrDefault()));
                response.Collection = string.Join(", ", collectionNameList);
            }

            return response;
        }
        /// <summary>
        /// Get the dropdown values to specify in the export file 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<RejectionDashboardRequestExport> SetDashboardExportFilter(RejectionDashboardSearchRequest request)
        {
            var response = new RejectionDashboardRequestExport();

            response.ServiceDateFrom = request.ServiceDateFrom == null || !CheckDate(request.ServiceDateFrom) ? "" : request.ServiceDateFrom.ToDateTime().ToString(StandardDateFormat);
            response.ServiceDateTo = request.ServiceDateTo == null || !CheckDate(request.ServiceDateTo) ? "" : request.ServiceDateTo.ToDateTime().ToString(StandardDateFormat);

            //get the customer name
            if (request.CustomerId > 0)
            {
                var customers = await _cusManager.GetCustomerByCustomerId(request.CustomerId.GetValueOrDefault());
                response.CustomerName = customers.DataSourceList != null && customers.DataSourceList.Any() ? string.Join(", ", customers.DataSourceList.Select(x => x.Name)) : "";
            }

            //get the country name
            if (request.SelectedCountryIdList != null && request.SelectedCountryIdList.Any())
            {
                var countryList = await _locationRepo.GetCountryByIds(request.SelectedCountryIdList.Select(x => x).ToList());
                response.FactoryCountry = countryList != null && countryList.Any() ? string.Join(", ", countryList.Select(x => x.Name)) : "";
            }
            //get the product name
            if (request.SelectedProductIdList != null && request.SelectedProductIdList.Any())
            {
                var productList = await _customerProductManager.GetProductNameByProductIds(request.SelectedProductIdList);
                response.Product = string.Join(", ", productList);
            }
            //get the product category name
            if (request.SelectedProdCategoryIdList != null && request.SelectedProdCategoryIdList.Any())
            {
                var productCategories = await _referenceManager.GetProdCategoriesByProdCategoryIds(request.SelectedProdCategoryIdList);
                response.ProductCategory = string.Join(", ", productCategories.Select(x => x.Name).ToList());
            }

            //get the supplier name
            if (request.SupplierId > 0)
            {
                var supIdList = new[] { request.SupplierId.GetValueOrDefault() }.ToList();
                var supList = await _supManager.GetSupplierById(supIdList);
                response.SupplierName = supList.DataSourceList != null && supList.DataSourceList.Any() ? string.Join(", ", supList.DataSourceList.Select(x => x.Name)) : "";
            }

            //selected brand name list
            if (request.SelectedBrandIdList != null && request.SelectedBrandIdList.Any())
            {
                var brandNameList = await _cusBrandManager.GetBrandNameByBrandId(request.SelectedBrandIdList.Select(x => x));
                response.Brand = string.Join(", ", brandNameList);
            }

            //selected buyer name list
            if (request.SelectedBuyerIdList != null && request.SelectedBuyerIdList.Any())
            {
                var buyerNameList = await _cusBuyerManager.GetBuyerNameByBuyerIds(request.SelectedBuyerIdList.Select(x => x));
                response.Buyer = string.Join(", ", buyerNameList);
            }

            //selected department name list
            if (request.SelectedDeptIdList != null && request.SelectedDeptIdList.Any())
            {
                var deptNameList = await _cusDeptManager.GetDeptNameByDeptIds(request.SelectedDeptIdList.Select(x => x));
                response.Department = string.Join(", ", deptNameList);
            }

            //selected collection name list
            if (request.SelectedCollectionIdList != null && request.SelectedCollectionIdList.Any())
            {
                var collectionNameList = await _cusCollectionManager.GetCollectionNameByCollectionIds(request.SelectedCollectionIdList.Select(x => x.GetValueOrDefault()));
                response.Collection = string.Join(", ", collectionNameList);
            }

            return response;
        }


        private static bool CheckDate(DateObject date)
        {
            try
            {
                var dt = date.ToDateTime();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// export the product category chart
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ChartExport> ExportProductCategoryDashboard(RejectionDashboardSearchRequest request)
        {
            var response = new ChartExport();

            //get the booking details

            var bookingIds = GetQueryableBookingIdList(request);

            var res = await _repo.GetProductCategoryDashboard(bookingIds, isExport: true);

            if (res == null || !res.Any())
            {
                return new ChartExport { Data = null };
            }

            var items = res.Where(x => x.ResultId > 0).GroupBy(p => p.Name, (key, _data) =>
                new ChartExportItem
                {
                    Name = key,
                    Count = _data.Sum(x => x.TotalCount),
                    Data = _data.ToList()
                }).OrderByDescending(x => x.Count).ToList();

            response.Data = items;

            response.RequestFilters = await SetDashboardExportFilter(request);

            response.Total = items.Sum(x => x.Count);

            response.ResultNames = res.Where(x => x.ResultId > 0).Select(x => x.ResultName).Distinct().ToList();

            return response;
        }

        /// <summary>
        /// fetch the result data by supplier
        /// </summary>
        /// <param name="inspectionIdList"></param>
        /// <returns></returns>
        public async Task<VendorChartResponse> GetVendorResultDashboard(RejectionDashboardSearchRequest request)
        {

            var bookingIds = GetQueryableBookingIdList(request);

            var res = await _repo.GetSupplierDashboard(bookingIds, isExport: false);

            if (res == null || !res.Any())
            {
                return new VendorChartResponse { Result = RejectionDashboardResult.NotFound };
            }

            // Converts the all capital country names to Pascal Case (CHINA - China)
            TextInfo myTI = new CultureInfo(EnglishUS, false).TextInfo;
            foreach (var item in res)
            {
                var str = myTI.ToTitleCase(myTI.ToLower(item.Name));
                item.Name = str;
            }

            //group the top 6 data by result 
            var items = res.Where(x => x.ResultId > 0).GroupBy(p => p.ResultId, (key, _data) =>
           new ResultData
           {
               ResultName = _data.Select(x => x.ResultName).FirstOrDefault(),
               Count = _data.Sum(y => y.TotalCount),
               Color = CustomerAPIRADashboardColor.GetValueOrDefault(key.GetValueOrDefault(), ""),
               Data = _data
           }).OrderByDescending(x => x.Count).ToList();

            //fetch the supplierlist for the Y axis
            var supplierList = res.Select(x => new CommonDataSource { Id = x.Id.GetValueOrDefault(), Name = x.Name });

            return new VendorChartResponse
            {
                Data = items,
                SupplierList = supplierList.GroupBy(x => x.Id).Select(group => group.First()).Take(15),
                TotalReports = items.Sum(x => x.Count),
                Result = RejectionDashboardResult.Success
            };
        }



        /// <summary>
        /// export the supplier/ vendor chart
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ChartExport> ExportVendorDashboard(RejectionDashboardSearchRequest request)
        {
            var response = new ChartExport();

            var bookingIds = GetQueryableBookingIdList(request);

            var res = await _repo.GetSupplierDashboard(bookingIds, isExport: true);

            if (res == null || !res.Any())
            {
                return new ChartExport { Data = null };
            }

            var items = res.Where(x => x.ResultId > 0).GroupBy(p => p.Name, (key, _data) =>
                new ChartExportItem
                {
                    Name = key,
                    Count = _data.Sum(x => x.TotalCount),
                    Data = _data.ToList()
                }).OrderByDescending(x => x.Count).ToList();

            response.Data = items;

            response.RequestFilters = await SetDashboardExportFilter(request);

            response.Total = items.Sum(x => x.Count);

            response.ResultNames = res.Where(x => x.ResultId > 0).Select(x => x.ResultName).Distinct().ToList();

            return response;
        }

        /// <summary>
        /// fetch the result data by country
        /// </summary>
        /// <param name="inspectionIdList"></param>
        /// <returns></returns>
        public async Task<CountryChartResponse> GetCountryResultDashboard(CountryChartRequest request)
        {
            List<CountryChartItem> res = null;
            List<CommonDataSource> countryList = null;
            List<ChartItem> chartList = new List<ChartItem>();

            var bookingIds = GetQueryableBookingIdList(request.SearchRequest);

            if (bookingIds == null || !bookingIds.Any())
            {
                return new CountryChartResponse { Result = RejectionDashboardResult.NotFound };
            }

            //if countryId, fetch the data based on province else fetch the data based on country
            if (!(request.CountryId > 0))
            {

                res = await _repo.GetQueryableResultByCountry(bookingIds);

                //fetch only the countries in the result for the inner dropdown list
                countryList = res.Select(x => new CommonDataSource { Id = x.CountryId.GetValueOrDefault(), Name = x.CountryName }).ToList();

                //if only one country, then fetch the province data
                if (countryList.Select(x => x.Id).Distinct().Count() == 1 && !request.ClearSelection)
                {
                    request.CountryId = countryList.Select(x => x.Id).FirstOrDefault();
                    res = await _repo.GetResultByProvince(bookingIds, request.CountryId);
                }
            }

            else
            {
                res = await _repo.GetResultByProvince(bookingIds, request.CountryId);
                countryList = res.Select(x => new CommonDataSource { Id = x.CountryId.GetValueOrDefault(), Name = x.CountryName }).ToList();
            }

            if (res == null || !res.Any())
            {
                return new CountryChartResponse { Result = RejectionDashboardResult.NotFound };
            }

            foreach (var item in res)
            {
                var totalCountryCount = request.CountryId > 0 ? res.Where(x => x.ProvinceId == item.ProvinceId && x.ResultId > 0).Sum(x => x.TotalCount) : res.Where(x => x.CountryId == item.CountryId && x.ResultId > 0).Sum(x => x.TotalCount);
                ChartItem data = new ChartItem()
                {
                    Id = request.CountryId > 0 ? item.ProvinceId : item.CountryId,
                    Name = request.CountryId > 0 ? item.ProvinceName : item.CountryName,
                    ResultId = item.ResultId,
                    ResultName = item.ResultName,
                    TotalCount = item.TotalCount,
                    Percentage = GetPercentage(item.TotalCount, totalCountryCount)
                };

                chartList.Add(data);
            }

            // Converts the all capital country names to Pascal Case (CHINA - China)
            TextInfo myTI = new CultureInfo(EnglishUS, false).TextInfo;
            foreach (var item in chartList)
            {
                var str = myTI.ToTitleCase(myTI.ToLower(item.Name));
                item.Name = str;
            }

            var items = chartList.Where(x => x.ResultId > 0).GroupBy(p => p.ResultId, (key, _data) =>
            new ResultData
            {
                ResultName = _data.Select(x => x.ResultName).FirstOrDefault(),
                Count = _data.Sum(y => y.TotalCount),
                Color = CustomerAPIRADashboardColor.GetValueOrDefault(key.GetValueOrDefault(), ""),
                Data = _data
            }).OrderByDescending(x => x.Count).ToList();


            //the Y axis is the country or the province
            var yAxisList = chartList.Select(x => new CommonDataSource { Id = x.Id.GetValueOrDefault(), Name = x.Name });

            return new CountryChartResponse
            {
                Data = items,
                YAxisData = yAxisList.GroupBy(x => x.Id).Select(group => group.First()),
                TotalReports = items.Sum(x => x.Count),
                SelectedCountryId = request.CountryId,
                CountryList = countryList.GroupBy(x => x.Id).Select(group => group.First()).ToList(),
                Result = RejectionDashboardResult.Success
            };
        }


        /// <summary>
        /// export the country chart
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CountryChartExport> ExportCountryDashboard(RejectionDashboardSearchRequest request)
        {
            var response = new CountryChartExport();

            var bookingIds = GetQueryableBookingIdList(request);

            var res = _repo.ExportQueryablesultByCountryProvince(bookingIds);


            if (request.CountryId > 0)
            {
                res = res.Where(x => x.CountryId == request.CountryId);
            }

            var data = await res.ToListAsync();

            if (data == null || !data.Any())
            {
                return new CountryChartExport();
            }
            var items = data.Where(x => x.ResultId > 0).GroupBy(p => new { p.CountryName, p.ProvinceName }, (key, _data) =>
                new CountryChartExportItem
                {
                    CountryName = key.CountryName,
                    ProvinceName = key.ProvinceName,
                    Count = _data.Sum(x => x.TotalCount),
                    Data = _data.ToList()
                }).OrderBy(x => x.CountryName).ToList();

            response.Data = items;

            response.RequestFilters = await SetDashboardExportFilter(request);

            response.Total = items.Sum(x => x.Count);

            response.ResultNames = res.Where(x => x.ResultId > 0).Select(x => x.ResultName).Distinct().ToList();

            return response;
        }

        /// <summary>
        /// get the reject or pass or fail report by inspection
        /// </summary>
        /// <param name="inspectionIdList"></param>
        /// <returns></returns>
        public async Task<RejectChartResponse> GetCustomerReportReject(RejectChartRequest request)
        {
            List<MandayYear> mandayYear = new List<MandayYear>();

            //start from the 1st of the month
            if (request.SearchRequest.ServiceDateFrom != null)
                request.SearchRequest.ServiceDateFrom.Day = 1;

            var bookingIdList = GetQueryableBookingIdList(request.SearchRequest);

            //get the data based on year, month and reject reason
            var res = await _repo.GetCustomerReportReject(bookingIdList, request.FbResultId);

            if (res == null || !res.Any())
            {
                return new RejectChartResponse { Result = RejectionDashboardResult.NotFound };
            }

            var groupbyRejection = res.GroupBy(p => new
            {
                p.Name,
                p.Year,
                p.Month
            }, p => p, (Key, _data) =>
                     new RejectChartMonthItem
                     {
                         Name = Key.Name,
                         MonthName = MonthData.GetValueOrDefault(Key.Month),
                         Year = Key.Year,
                         MonthCount = _data.Select(x => x.FbReportDetailId).Distinct().Count()
                     }).OrderByDescending(x => x.MonthCount);

            var yearList = groupbyRejection.Select(x => x.Year).Distinct().ToList();

            //get the months for the selected year for the column headers
            foreach (var year in yearList)
            {
                for (int i = 1; i <= 12; i++)
                {
                    MandayYear yearMonthList = new MandayYear();
                    yearMonthList.year = year;
                    yearMonthList.month = i;
                    yearMonthList.MonthName = MonthData.GetValueOrDefault(i) + " " + year % 100;
                    yearMonthList.Month_Year = new DateTimeFormatInfo().GetMonthName(i) + " " + year;
                    mandayYear.Add(yearMonthList);
                }
            }

            //group the data based on reject reason
            var result = groupbyRejection.GroupBy(x => x.Name, (key, _data) =>
            new RejectChartYearData
            {
                Name = key,
                Count = _data.Sum(x => x.MonthCount),
                MonthlyData = _data.ToList(),
                Percentage = GetPercentage(_data.Sum(x => x.MonthCount), groupbyRejection.Sum(x => x.MonthCount))
            }).OrderByDescending(x => x.Count).ToList();

            return new RejectChartResponse
            {
                Data = result,
                MonthNameList = mandayYear,
                RejectReasonList = result.Select(x => x.Name).Distinct().ToList(),
                Result = RejectionDashboardResult.Success
            };
        }

        /// <summary>
        /// get the reject or pass or fail report by inspection and subcatogory
        /// </summary>
        /// <param name="inspectionIdList"></param>
        /// <returns></returns>
        public async Task<RejectChartResponse> GetCustomerReportRejectSubcatogory(RejectChartSubcatogoryRequest request)
        {
            List<MandayYear> mandayYear = new List<MandayYear>();

            //start from the 1st of the month
            request.SearchRequest.ServiceDateFrom.Day = 1;
            var bookingIdList = GetQueryableBookingIdList(request.SearchRequest);

            //get the data based on year, month and reject reason
            var res = await _repo.GetCustomerReportRejectSubcatogory(bookingIdList, request);

            if (res == null || !res.Any())
            {
                return new RejectChartResponse { Result = RejectionDashboardResult.NotFound };
            }

            var resultGroupBy = res.GroupBy(p => new
            {
                p.Name,
                p.ReasonName,
                p.Year,
                p.Month
            }, (Key, _data) => new RejectChartMonthItem
            {
                ReasonName = Key.ReasonName,
                Name = Key.Name,
                Year = Key.Year,
                MonthName = MonthData.GetValueOrDefault(Key.Month),
                MonthCount = _data.Select(x => x.FbReportDetailId).Distinct().Count()
            }).OrderByDescending(x => x.MonthCount);

            if (resultGroupBy == null || !resultGroupBy.Any())
            {
                return new RejectChartResponse { Result = RejectionDashboardResult.NotFound };
            }

            var yearList = resultGroupBy.Select(x => x.Year).Distinct().ToList();

            //get the months for the selected year for the column headers
            foreach (var year in yearList)
            {
                for (int i = 1; i <= 12; i++)
                {
                    MandayYear yearMonthList = new MandayYear();
                    yearMonthList.year = year;
                    yearMonthList.month = i;
                    yearMonthList.MonthName = MonthData.GetValueOrDefault(i) + " " + year % 100;
                    yearMonthList.Month_Year = new DateTimeFormatInfo().GetMonthName(i) + " " + year;
                    mandayYear.Add(yearMonthList);
                }
            }

            //group the data based on reject reason
            var result = resultGroupBy.GroupBy(x => x.Name, (key, _data) =>
            new RejectChartYearData
            {
                ReasonName = _data.Select(x => x.ReasonName).FirstOrDefault(),
                Name = key,
                Count = _data.Sum(x => x.MonthCount),
                MonthlyData = _data.ToList(),
                Percentage = GetPercentage(_data.Sum(x => x.MonthCount), resultGroupBy.Sum(x => x.MonthCount))
            }).OrderByDescending(x => x.Count).ToList();

            return new RejectChartResponse
            {
                Data = result,
                MonthNameList = mandayYear,
                RejectReasonList = result.Select(x => x.Name).Distinct().ToList(),
                Result = RejectionDashboardResult.Success
            };
        }
        /// <summary>
        /// get the reject or pass or fail report by inspection and subcatogory2
        /// </summary>
        /// <param name="inspectionIdList"></param>
        /// <returns></returns>
        public async Task<RejectChartResponse> GetCustomerReportRejectSubcatogory2(RejectChartSubcatogory2Request request)
        {
            List<MandayYear> mandayYear = new List<MandayYear>();

            //start from the 1st of the month
            request.SearchRequest.ServiceDateFrom.Day = 1;
            var bookingIdList = GetQueryableBookingIdList(request.SearchRequest);

            //get the data based on year, month and reject reason
            var res = await _repo.GetCustomerReportRejectSubcatogory2(bookingIdList, request);

            if (res == null || !res.Any())
            {
                return new RejectChartResponse { Result = RejectionDashboardResult.NotFound };
            }

            var resultGroupBy = res.GroupBy(p =>
                         new
                         {
                             p.Name,
                             p.Subcatogory,
                             p.ReasonName,
                             p.Year,
                             p.Month
                         }, (Key, _data) => new RejectChartMonthItem
                         {
                             ReasonName = Key.ReasonName,
                             Subcatogory = Key.Subcatogory,
                             Name = Key.Name,
                             MonthName = MonthData.GetValueOrDefault(Key.Month),
                             Year = Key.Year,
                             MonthCount = _data.Select(x => x.FbReportDetailId).Distinct().Count()
                         }).OrderByDescending(x => x.MonthCount);

            if (resultGroupBy == null || !resultGroupBy.Any())
            {
                return new RejectChartResponse { Result = RejectionDashboardResult.NotFound };
            }

            var yearList = resultGroupBy.Select(x => x.Year).Distinct().ToList();

            //get the months for the selected year for the column headers
            foreach (var year in yearList)
            {
                for (int i = 1; i <= 12; i++)
                {
                    MandayYear yearMonthList = new MandayYear();
                    yearMonthList.year = year;
                    yearMonthList.month = i;
                    yearMonthList.MonthName = MonthData.GetValueOrDefault(i) + " " + year % 100;
                    yearMonthList.Month_Year = new DateTimeFormatInfo().GetMonthName(i) + " " + year;
                    mandayYear.Add(yearMonthList);
                }
            }

            //group the data based on reject reason
            var result = resultGroupBy.GroupBy(x => x.Name, (key, _data) =>
            new RejectChartYearData
            {
                ReasonName = _data.Select(x => x.ReasonName).FirstOrDefault(),
                Subcatogory = _data.Select(x => x.Subcatogory).FirstOrDefault(),
                Name = key,
                Count = _data.Sum(x => x.MonthCount),
                MonthlyData = _data.ToList(),
                Percentage = GetPercentage(_data.Sum(x => x.MonthCount), resultGroupBy.Sum(x => x.MonthCount))
            }).OrderByDescending(x => x.Count).ToList();

            return new RejectChartResponse
            {
                Data = result,
                MonthNameList = mandayYear,
                RejectReasonList = result.Select(x => x.Name).Distinct().ToList(),
                Result = RejectionDashboardResult.Success
            };
        }

        private double GetPercentage(double sum, double total)
        {
            var res = total != 0 ? sum / total : 0;
            var result = Math.Round(res * 100);
            return result;
        }

        /// <summary>
        /// fetch the reject data for each month and reason
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<RejectionPopUpResponse> GetRejectPopUpData(RejectionDashboardSearchRequest request)
        {
            RejectionPopUpData res = new RejectionPopUpData();

            //if we have 1 jan 2020 to 19 apr 2021 data, and 2020 apr data needs to be fetched
            if (request.Month == request.ServiceDateTo.Month)
            {
                //set the from date to 1st of the month
                DateTime fromDate = new DateTime(request.Year, request.Month, 1);
                request.ServiceDateFrom = Static_Data_Common.GetCustomDate(fromDate);

                //if the service date to year and the requested year is not the same, set the serviceto date to the last day of the month
                if (request.Year != request.ServiceDateTo.Year)
                {
                    request.ServiceDateTo = Static_Data_Common.GetCustomDate(fromDate.AddMonths(1).AddDays(-1));
                }
            }
            //set the service date from and to to the requested month
            else
            {
                DateTime fromDate = new DateTime(request.Year, request.Month, 1);
                request.ServiceDateFrom = Static_Data_Common.GetCustomDate(fromDate);
                request.ServiceDateTo = Static_Data_Common.GetCustomDate(fromDate.AddMonths(1).AddDays(-1));
            }

            ////get the booking data
            //var bookingData = await GetBookingDataByRequest(request);
            //var inspectionIdList = bookingData.Where(x => InspectedStatusList.Contains(x.StatusId)).Select(x => x.InspectionId).Distinct().ToList();

            var inspectionIdList = GetQueryableBookingIdList(request);



            //fetch the failed reports with failed reasons

            List<RejectionFactoryData> response = new List<RejectionFactoryData>();
            List<RejectionReportData> reportlst = new List<RejectionReportData>();

            if (request.SearchBy.Trim().ToLower() == "subcatogory2")
            {
                response = await _repo.GetCustomerReportSubcatogory2PopUpData(inspectionIdList, request.RejectReason, request.FbResultId, request.SummaryNames, request.SubcatogoryList);
                reportlst = await _repo.GetSubcatogory2ReportByInspectionIds(inspectionIdList, request.RejectReason, request.FbResultId, request.SummaryNames, request.SubcatogoryList);

            }
            else if (request.SearchBy.Trim().ToLower() == "subcatogory")
            {
                response = await _repo.GetCustomerReportSubcatogoryPopUpData(inspectionIdList, request.RejectReason, request.FbResultId, request.SummaryNames);
                reportlst = await _repo.GetSubcatogoryReportByInspectionIds(inspectionIdList, request.RejectReason, request.FbResultId, request.SummaryNames);
            }
            else if (request.SearchBy.Trim().ToLower() == "summary")
            {
                response = await _repo.GetCustomerReportRejectPopUpData(inspectionIdList, request.RejectReason, request.FbResultId);
                reportlst = await _repo.GetReportByInspectionIds(inspectionIdList, request.RejectReason, request.FbResultId);
            }

            if (response == null || !response.Any())
            {
                return new RejectionPopUpResponse { Result = RejectionDashboardResult.NotFound };
            }

            if (reportlst == null || !reportlst.Any())
            {
                return new RejectionPopUpResponse { Result = RejectionDashboardResult.NotFound };
            }

            //var reportlst = await _repo.GetReportByInspectionIds(inspectionIdList, request.RejectReason, request.FbResultId);

            response = response.Where(x => x.Month == request.ServiceDateFrom.Month).ToList();

            foreach (var data in response)
            {
                data.ReportInfo = new List<RejectionReportData>();
                data.ReportInfo = reportlst.Where(x => x.FactoryId == data.FactoryId && x.SupplierId == data.SupplierId).ToList();
            }

            res.InspectionCount = response.Sum(x => x.BookingCount);
            res.FactoryCount = response.Select(x => x.FactoryId).Distinct().Count();
            res.ReportCount = response.Sum(x => x.ReportCount);
            res.SupplierData = response;

            return new RejectionPopUpResponse
            {
                Data = res,
                SupplierList = res.SupplierData.Distinct().Select(x => new CommonDataSource { Id = x.SupplierId, Name = x.SupplierName }).ToList(),
                FactoryList = res.SupplierData.Where(x => x.FactoryId > 0).Distinct().Select(x => new CommonDataSource { Id = x.FactoryId.GetValueOrDefault(), Name = x.FactoryName }).ToList(),
                Result = RejectionDashboardResult.Success
            };
        }


        /// <summary>
        /// fetch the reject image
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<RejectionImageResponse> GetRejectionImages(RejectionDashboardSearchRequest request)
        {
            //if we have 2020 jan to 2021 apr data, and 2020 apr data needs to be fetched
            if (request.Month == request.ServiceDateTo.Month)
            {
                //set the from date to 1st of the month
                DateTime fromDate = new DateTime(request.Year, request.Month, 1);
                request.ServiceDateFrom = Static_Data_Common.GetCustomDate(fromDate);

                //if the service date to year and the requested year is not the same, set the serviceto date to the last day of the month
                if (request.Year != request.ServiceDateTo.Year)
                {
                    request.ServiceDateTo = Static_Data_Common.GetCustomDate(fromDate.AddMonths(1).AddDays(-1));
                }
            }
            //set the service date from and to to the requested month
            else
            {
                DateTime fromDate = new DateTime(request.Year, request.Month, 1);
                request.ServiceDateFrom = Static_Data_Common.GetCustomDate(fromDate);
                request.ServiceDateTo = Static_Data_Common.GetCustomDate(fromDate.AddMonths(1).AddDays(-1));
            }
            request.SupplierId = request.popUpSelectedPhotoSupplierId > 0 ? request.popUpSelectedPhotoSupplierId : request.SupplierId;
            request.FactoryId = request.popUpSelectedPhotoFactoryId > 0 ? request.popUpSelectedPhotoFactoryId : request.FactoryId;

            var inspectionIdList = GetQueryableBookingIdList(request);

            var reasonNames = (request.SummaryNames != null && request.SummaryNames.Any()) ? request.SummaryNames : new[] { request.RejectReason }.ToList();

            var res = await _repo.GetReportRejectImageData(inspectionIdList, reasonNames, request.FbResultId);

            if (res == null || !res.Any())
            {
                return new RejectionImageResponse { Result = RejectionDashboardResult.NotFound };
            }

            return new RejectionImageResponse
            {
                Data = res,
                Result = RejectionDashboardResult.Success
            };
        }
        /// <summary>
        ///Get Queryable BookingId List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private IQueryable<int> GetQueryableBookingIdList(RejectionDashboardSearchRequest request)
        {

            var inspectionQuery = _sharedInspection.GetAllInspectionQuery();
            var inspectionQueryRequest = _sharedInspection.GetRejectionDashBoardInspectionSearchRequestMap(request);

            var data = _sharedInspection.GetInspectionQuerywithRequestFilters(inspectionQueryRequest, inspectionQuery);

            if (request.ServiceDateTo != null && request.ServiceDateFrom != null && CheckDate(request.ServiceDateTo) && CheckDate(request.ServiceDateFrom))
            {
                data = data.Where(x => x.ServiceDateTo <= request.ServiceDateTo.ToDateTime() && x.ServiceDateTo >= request.ServiceDateFrom.ToDateTime());
            }

            //get booking ids
            var bookingIds = data.Select(x => x.Id);

            return bookingIds;
        }

        public async Task<List<ExportRejectionTableData>> ExportRejectionDashboardData(RejectChartSubcatogory2Request request)
        {
            request.SearchRequest.ServiceDateFrom.Day = 1;
            var bookingIdList = GetQueryableBookingIdList(request.SearchRequest);

            //get the data based on year, month and reject reason
            var res = _repo.ExportCustomerReportRejectSubcatogory2(bookingIdList, request);

            //filter by sub category
            if (request.SubCatogory != null && request.SubCatogory.Any())
            {
                res = res.Where(x => request.SubCatogory.Contains(x.SubCategory));
            }

            //filter by sub category 2
            if (request.ResultNames != null && request.ResultNames.Any())
            {
                res = res.Where(x => request.ResultNames.Contains(x.ReasonName));
            }

            var items = await res.ToListAsync();

            var result = items.GroupBy(p => new { p.Year, p.Month, p.SubCategory2, p.SubCategory, p.ReasonName }, (key, _data) =>
              new ExportRejectionTableData
              {
                  ReasonName = key.ReasonName,
                  SubCategory = key.SubCategory,
                  SubCategory2 = key.SubCategory2,
                  Month = MonthData.GetValueOrDefault(key.Month),
                  Year = key.Year,
                  RejectionCount = _data.Select(x => x.ReportId).Distinct().Count()
              }).OrderBy(x => x.ReasonName).ThenBy(x => x.SubCategory).ThenBy(x => x.SubCategory2).ThenBy(x => x.Month).ToList();


            if (res == null || !res.Any())
            {
                return null;
            }

            return result;
        }

        public async Task<RejectionRateResponse> GetReportRejectionRate(RejectionDashboardSearchRequest request)
        {
            if (request == null)
                return new RejectionRateResponse() { Result = RejectionRateResult.NotFound };

            if (request.Index == null || request.Index.Value <= 0)
                request.Index = 1;

            if (request.pageSize == null || request.pageSize.Value == 0)
                request.pageSize = 10;

            int skip = (request.Index.Value - 1) * request.pageSize.Value;

            int take = request.pageSize.Value;

            var bookingIdList = GetQueryableBookingIdList(request);

            var reportRejectionRate = _repo.GetQueryableReportRejectionRate(bookingIdList);

            if (request.SelectedBrandIdList != null && request.SelectedBrandIdList.Any())
            {
                reportRejectionRate = reportRejectionRate.Where(x => request.SelectedBrandIdList.Contains(x.BrandId));
            }

            var groupByRequestFilter = new GroupByRequestFilter();
            if (request.GroupByFilter != null)
            {
                groupByRequestFilter.FactoryCountry = request.GroupByFilter.Any(x => x == GroupByFilter.FactoryCountry);
                groupByRequestFilter.Supplier = request.GroupByFilter.Any(x => x == GroupByFilter.Supplier);
                groupByRequestFilter.Factory = request.GroupByFilter.Any(x => x == GroupByFilter.Factory);
                groupByRequestFilter.Brand = request.GroupByFilter.Any(x => x == GroupByFilter.Brand);
            }

            var rejectionRateReportResultInsp = await reportRejectionRate.GroupBy(x => new
            {
                FactoryCountryId = groupByRequestFilter.FactoryCountry ? x.FactoryCountryId : 0,
                FactoryCountryName = groupByRequestFilter.FactoryCountry ? x.FactoryCountryName : null,
                SupplierId = groupByRequestFilter.Supplier ? x.SupplierId : 0,
                SupplierName = groupByRequestFilter.Supplier ? x.SupplierName : null,
                FactoryId = groupByRequestFilter.Factory ? x.FactoryId : 0,
                FactoryName = groupByRequestFilter.Factory ? x.FactoryName : null,
                BrandId = groupByRequestFilter.Brand ? x.BrandId : 0,
                BrandName = groupByRequestFilter.Brand ? x.BrandName : null,
                ResultId = x.ResultId,
                ResultName = x.ResultName,
                InspectionId = x.InspectionId
            }).Select(x => new
            {
                FactoryCountryId = x.Key.FactoryCountryId,
                FactoryCountryName = x.Key.FactoryCountryName,
                SupplierId = x.Key.SupplierId,
                SupplierName = x.Key.SupplierName,
                FactoryId = x.Key.FactoryId,
                FactoryName = x.Key.FactoryName,
                BrandId = x.Key.BrandId,
                BrandName = x.Key.BrandName,
                ResultId = x.Key.ResultId,
                ResultName = x.Key.ResultName,
                InspectionId = x.Key.InspectionId,
                PresentedQty = x.Sum(x => x.PresentedQty),
                InspectedQty = x.Sum(x => x.InspectedQty),
                OrderQty = x.Sum(x => x.OrderQty),
                TotalCount = x.Select(x => x.ReportId).Distinct().Count(),
            }).ToListAsync();

            var rejectionRateReportResultLists = rejectionRateReportResultInsp.GroupBy(x => new
            {
                FactoryCountryId = groupByRequestFilter.FactoryCountry ? x.FactoryCountryId : 0,
                FactoryCountryName = groupByRequestFilter.FactoryCountry ? x.FactoryCountryName : null,
                SupplierId = groupByRequestFilter.Supplier ? x.SupplierId : 0,
                SupplierName = groupByRequestFilter.Supplier ? x.SupplierName : null,
                FactoryId = groupByRequestFilter.Factory ? x.FactoryId : 0,
                FactoryName = groupByRequestFilter.Factory ? x.FactoryName : null,
                BrandId = groupByRequestFilter.Brand ? x.BrandId : 0,
                BrandName = groupByRequestFilter.Brand ? x.BrandName : null,
                ResultId = x.ResultId,
                ResultName = x.ResultName
            }).Select(x => new RejectionRateReportResultList
            {
                FactoryCountryId = x.Key.FactoryCountryId,
                FactoryCountryName = x.Key.FactoryCountryName,
                SupplierId = x.Key.SupplierId,
                SupplierName = x.Key.SupplierName,
                FactoryId = x.Key.FactoryId,
                FactoryName = x.Key.FactoryName,
                BrandId = x.Key.BrandId,
                BrandName = x.Key.BrandName,
                ResultId = x.Key.ResultId,
                ResultName = x.Key.ResultName,
                InspectionCount = x.Select(x => x.InspectionId).Distinct().Count(),
                PresentedQty = x.Sum(x => x.PresentedQty),
                InspectedQty = x.Sum(x => x.InspectedQty),
                OrderQty = x.Sum(x => x.OrderQty),
                TotalCount = x.Sum(x => x.TotalCount)
            }).ToList();

            var fbReportResults = rejectionRateReportResultLists.Where(x => x.ResultId > 0).GroupBy(x => new
            {
                Id = x.ResultId.GetValueOrDefault(),
                Name = x.ResultName
            }).Select(x => new CommonDataSource()
            {
                Id = x.Key.Id,
                Name = x.Key.Name
            }).ToList();

            var rejectionRateGroupCount = rejectionRateReportResultLists.GroupBy(x => new
            {
                FactoryCountryId = x.FactoryCountryId,
                FactoryCountryName = x.FactoryCountryName,
                SupplierId = x.SupplierId,
                SupplierName = x.SupplierName,
                FactoryId = x.FactoryId,
                FactoryName = x.FactoryName,
                BrandId = x.BrandId,
                BrandName = x.BrandName
            }).Select(x => new RejectionRateGroupList
            {
                FactoryCountryId = x.Key.FactoryCountryId,
                FactoryCountryName = x.Key.FactoryCountryName,
                SupplierId = x.Key.SupplierId,
                SupplierName = x.Key.SupplierName,
                FactoryId = x.Key.FactoryId,
                FactoryName = x.Key.FactoryName,
                BrandId = x.Key.BrandId,
                BrandName = x.Key.BrandName
            }).Count();

            var rejectionRateGroupList = rejectionRateReportResultInsp.GroupBy(x => new
            {
                FactoryCountryId = x.FactoryCountryId,
                FactoryCountryName = x.FactoryCountryName,
                SupplierId = x.SupplierId,
                SupplierName = x.SupplierName,
                FactoryId = x.FactoryId,
                FactoryName = x.FactoryName,
                BrandId = x.BrandId,
                BrandName = x.BrandName
            }).Select(x => new RejectionRateGroupList
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
                PresentedQty = x.Select(x => x.PresentedQty).Sum(),
                InspectedQty = x.Select(x => x.InspectedQty).Sum(),
                OrderQty = x.Select(x => x.OrderQty).Sum(),
                TotalCount = x.Select(x => x.TotalCount).Sum()
            })
            .OrderBy(x => x.FactoryCountryName)
            .ThenBy(x => x.SupplierName)
            .ThenBy(x => x.FactoryName)
            .ThenBy(x => x.BrandName)
            .Skip(request.IsExport ? 0 : skip).Take(request.IsExport ? rejectionRateGroupCount : take).ToList();

            var cusDecisionRejectionRate = _repo.GetQueryableCusDecisionRejectionRate(bookingIdList);
            var cusDecisionRejectionRateList = await cusDecisionRejectionRate.GroupBy(x => new
            {
                FactoryCountryId = groupByRequestFilter.FactoryCountry ? x.FactoryCountryId : 0,
                FactoryCountryName = groupByRequestFilter.FactoryCountry ? x.FactoryCountryName : null,
                SupplierId = groupByRequestFilter.Supplier ? x.SupplierId : 0,
                SupplierName = groupByRequestFilter.Supplier ? x.SupplierName : null,
                FactoryId = groupByRequestFilter.Factory ? x.FactoryId : 0,
                FactoryName = groupByRequestFilter.Factory ? x.FactoryName : null,
                BrandId = groupByRequestFilter.Brand ? x.BrandId : 0,
                BrandName = groupByRequestFilter.Brand ? x.BrandName : null,
                CustomerResultId = x.CustomerResultId,
                CustomDecisionName = x.CustomDecisionName
            }).Select(x => new RejectionRateDecisionList
            {
                FactoryCountryId = x.Key.FactoryCountryId,
                FactoryCountryName = x.Key.FactoryCountryName,
                SupplierId = x.Key.SupplierId,
                SupplierName = x.Key.SupplierName,
                FactoryId = x.Key.FactoryId,
                FactoryName = x.Key.FactoryName,
                BrandId = x.Key.BrandId,
                BrandName = x.Key.BrandName,
                CustomerResultId = x.Key.CustomerResultId,
                CustomDecisionName = x.Key.CustomDecisionName,
                TotalDecisionCount = x.Select(x => x.ReportId).Distinct().Count()
            }).ToListAsync();

            var customerDefaultDecisionList = cusDecisionRejectionRateList.GroupBy(x => new
            {
                Id = x.CustomerResultId,
                Name = x.CustomDecisionName
            }).Select(x => new CustomerDecisionRepo()
            {
                Id = x.Key.Id,
                Name = x.Key.Name
            }).ToList();

            var data = new RejectionRateItem
            {
                ResultDataList = new RejectionRateList
                {
                    RejectionRateReportResultLists = rejectionRateReportResultLists,
                    RejectionRateDecisionLists = cusDecisionRejectionRateList
                },
                RejectionRateGroupList = rejectionRateGroupList,
                ReportResultNameList = fbReportResults,
                ReportDecisionNameList = customerDefaultDecisionList
            };

            return new RejectionRateResponse()
            {
                Data = data,
                Result = RejectionRateResult.Success,
                TotalCount = rejectionRateGroupCount,
                Index = request.Index.Value,
                PageSize = request.pageSize.Value,
                PageCount = (rejectionRateGroupCount / request.pageSize.Value) + (rejectionRateGroupCount % request.pageSize.Value > 0 ? 1 : 0)
            };
        }

        public async Task<DataTable> ExportReportRejectionRate(RejectionDashboardSearchRequest request)
        {
            var result = await GetReportRejectionRate(request);

            var groupByRequestFilter = new GroupByRequestFilter();
            if (request.GroupByFilter != null)
            {
                groupByRequestFilter.FactoryCountry = request.GroupByFilter.Any(x => x == GroupByFilter.FactoryCountry);
                groupByRequestFilter.Supplier = request.GroupByFilter.Any(x => x == GroupByFilter.Supplier);
                groupByRequestFilter.Factory = request.GroupByFilter.Any(x => x == GroupByFilter.Factory);
                groupByRequestFilter.Brand = request.GroupByFilter.Any(x => x == GroupByFilter.Brand);
            }

            var enumEntityName = (Company)_filterService.GetCompanyId();
            string entityName = enumEntityName.ToString().ToUpper();

            //convert the list to datatable
            var dataTable = _helper.ConvertToDataTableWithCaption(result.Data.RejectionRateGroupList);

            MapRejectionRateReportResult(dataTable, groupByRequestFilter, result.Data.ReportResultNameList, result.Data.ResultDataList.RejectionRateReportResultLists, entityName);

            MapRejectionRateDecisionResult(dataTable, groupByRequestFilter, result.Data.ReportDecisionNameList, result.Data.ResultDataList.RejectionRateDecisionLists);

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

        public DataTable MapRejectionRateDecisionResult(DataTable dataTable, GroupByRequestFilter groupByRequestFilter, List<CustomerDecisionRepo> reportDecisionNameList, List<RejectionRateDecisionList> rejectionRateDecisionLists)
        {
            if (reportDecisionNameList != null && reportDecisionNameList.Any())
            {
                foreach (var reportDecisionHeader in reportDecisionNameList)
                {
                    dataTable.Columns.Add(Customer + "-" + reportDecisionHeader.Name, typeof(int));
                    dataTable.Columns.Add(Customer + "-" + reportDecisionHeader.Name + "-" + Percentage, typeof(double));

                    if (rejectionRateDecisionLists != null && rejectionRateDecisionLists.Any())
                    {
                        foreach (DataRow row in dataTable.Rows)
                        {
                            var reportDecisionResList = rejectionRateDecisionLists.ToList();
                            if (groupByRequestFilter.FactoryCountry)
                                reportDecisionResList = reportDecisionResList.Where(x => x.FactoryCountryId == Convert.ToInt32(row["FactoryCountryId"].ToString())).ToList();

                            if (groupByRequestFilter.Supplier)
                                reportDecisionResList = reportDecisionResList.Where(x => x.SupplierId == Convert.ToInt32(row["SupplierId"].ToString())).ToList();

                            if (groupByRequestFilter.Factory)
                                reportDecisionResList = reportDecisionResList.Where(x => x.FactoryId == Convert.ToInt32(row["FactoryId"].ToString())).ToList();

                            if (groupByRequestFilter.Brand)
                                reportDecisionResList = reportDecisionResList.Where(x => x.BrandId == Convert.ToInt32(row["BrandId"].ToString())).ToList();

                            double totalCount = reportDecisionResList.Sum(x => x.TotalDecisionCount);

                            reportDecisionResList = reportDecisionResList.Where(x => x.CustomerResultId == reportDecisionHeader.Id).ToList();

                            row[Customer + "-" + reportDecisionHeader.Name] = reportDecisionResList?.FirstOrDefault(x => x.CustomDecisionName == reportDecisionHeader.Name)?.TotalDecisionCount ?? 0;

                            double count = reportDecisionResList?.FirstOrDefault(x => x.CustomDecisionName == reportDecisionHeader.Name)?.TotalDecisionCount ?? 0;
                            row[Customer + "-" + reportDecisionHeader.Name + "-" + Percentage] = Math.Round(count / totalCount * 100, 2);
                        }
                    }
                }
            }
            return dataTable;
        }

        public DataTable MapRejectionRateReportResult(DataTable dataTable, GroupByRequestFilter groupByRequestFilter, List<CommonDataSource> reportResultNameList, List<RejectionRateReportResultList> rejectionRateReportResultLists, string entityName)
        {
            if (reportResultNameList != null && reportResultNameList.Any())
            {
                foreach (var reportHeader in reportResultNameList)
                {
                    dataTable.Columns.Add(entityName + "-" + reportHeader.Name, typeof(int));
                    dataTable.Columns.Add(entityName + "-" + reportHeader.Name + "-" + Percentage, typeof(double));


                    if (rejectionRateReportResultLists != null && rejectionRateReportResultLists.Any())
                    {
                        foreach (DataRow row in dataTable.Rows)
                        {
                            var reportResList = rejectionRateReportResultLists.Where(x => x.ResultId == reportHeader.Id).ToList();

                            if (groupByRequestFilter.FactoryCountry)
                                reportResList = reportResList.Where(x => x.FactoryCountryId == Convert.ToInt32(row["FactoryCountryId"].ToString())).ToList();

                            if (groupByRequestFilter.Supplier)
                                reportResList = reportResList.Where(x => x.SupplierId == Convert.ToInt32(row["SupplierId"].ToString())).ToList();

                            if (groupByRequestFilter.Factory)
                                reportResList = reportResList.Where(x => x.FactoryId == Convert.ToInt32(row["FactoryId"].ToString())).ToList();

                            if (groupByRequestFilter.Brand)
                                reportResList = reportResList.Where(x => x.BrandId == Convert.ToInt32(row["BrandId"].ToString())).ToList();

                            row[entityName + "-" + reportHeader.Name] = reportResList?.FirstOrDefault(x => x.ResultName == reportHeader.Name)?.TotalCount ?? 0;

                            double count = reportResList?.FirstOrDefault(x => x.ResultName == reportHeader.Name)?.TotalCount ?? 0;
                            double totalCount = Convert.ToDouble(row["TotalCount"].ToString());
                            row[entityName + "-" + reportHeader.Name + "-" + Percentage] = Math.Round(count / totalCount * 100, 2);
                        }
                    }
                }
            }
            return dataTable;
        }


        public async Task<object> GetReportRejectionAnalytics(EaqfDashboardRequest eaqfDashboardRequest)
        {
            if (eaqfDashboardRequest == null)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Invalid Request" });
            }
            if (eaqfDashboardRequest.CustomerId <= 0)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Customer Id is invalid" });
            }
            RejectChartRequest rejectionDashboardSearchRequest = new RejectChartRequest();
            rejectionDashboardSearchRequest.FbResultId = 2;
            rejectionDashboardSearchRequest.SearchRequest = new RejectionDashboardSearchRequest();

            if (eaqfDashboardRequest.CustomerId > 0)
            {
                rejectionDashboardSearchRequest.SearchRequest.CustomerId = eaqfDashboardRequest.CustomerId;
            }

            DateTime fromDate;
            DateTime toDate;
            if (DateTime.TryParseExact(eaqfDashboardRequest.FromDate, StandardDateFormat, CultureInfo.InvariantCulture,
                                                                                    DateTimeStyles.None, out fromDate))
            {
                rejectionDashboardSearchRequest.SearchRequest.ServiceDateFrom = new DateObject() { Year = fromDate.Year, Month = fromDate.Month, Day = fromDate.Day };
            }
            else
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidserviceFromDate });
            }
            if (DateTime.TryParseExact(eaqfDashboardRequest.ToDate, StandardDateFormat, CultureInfo.InvariantCulture,
                                                                                    DateTimeStyles.None, out toDate))
            {
                rejectionDashboardSearchRequest.SearchRequest.ServiceDateTo = new DateObject() { Year = toDate.Year, Month = toDate.Month, Day = toDate.Day };
            }
            else
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidserviceToDate });
            }

            if (fromDate > toDate)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { GreterThanTodate });
            }

            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.Country))
            {
                // fetch valid country list
                List<int> countryList = new List<int>();
                var countryListRequest = eaqfDashboardRequest.Country.Split(',').Distinct().ToList();
                if (countryListRequest.Any())
                {
                    var validCountryList = await _locationRepo.GetCountriesByAlpha2CodeList(countryListRequest);
                    countryList.AddRange(validCountryList.Select(x => x.Id).ToList());
                    rejectionDashboardSearchRequest.SearchRequest.SelectedCountryIdList = countryList;
                }
            }

            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.ProductCategory))
            {
                List<int> productCategoryList = eaqfDashboardRequest.ProductCategory.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.SearchRequest.SelectedProdCategoryIdList = productCategoryList;
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.ProductName))
            {
                List<int> productNameList = eaqfDashboardRequest.ProductName.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.SearchRequest.SelectedProductIdList = productNameList;
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.Factory))
            {
                List<int> factoryList = eaqfDashboardRequest.Factory.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.SearchRequest.SelectedFactoryIdList = factoryList;
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.Vendor))
            {
                List<int> vendorList = eaqfDashboardRequest.Vendor.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.SearchRequest.SelectedSupplierIdList = vendorList;
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.ServiceType))
            {
                List<int> serviceTypeList = eaqfDashboardRequest.ServiceType.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.SearchRequest.SelectedServiceTypeIdList = serviceTypeList;
            }

            var dashBoardData = await GetCustomerReportReject(rejectionDashboardSearchRequest);

            List<EaqfDashboardResponse> dashboardResponses = new List<EaqfDashboardResponse>();

            if (dashBoardData != null && dashBoardData.Data != null)
            {
                foreach (var item in dashBoardData.Data)
                {
                    dashboardResponses.Add(new EaqfDashboardResponse()
                    {
                        Name = item.Name,
                        Count = item.Count,
                        Percentage = item.Percentage
                    });
                }
            }
            else
            {
                return new EaqfGetSuccessResponse()
                {
                    message = "Data Not found",
                    statusCode = HttpStatusCode.OK,
                    data = dashboardResponses
                };
            }

            return new EaqfGetSuccessResponse()
            {
                message = Success,
                statusCode = HttpStatusCode.OK,
                data = dashboardResponses
            };
        }

        public async Task<object> GetReportDefectAnalytics(EaqfDashboardRequest eaqfDashboardRequest)
        {
            if (eaqfDashboardRequest == null)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Invalid Request" });
            }
            if (eaqfDashboardRequest.CustomerId <= 0)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Customer Id is invalid" });
            }
            DTO.DefectDashboard.DefectDashboardFilterRequest rejectionDashboardSearchRequest = new DTO.DefectDashboard.DefectDashboardFilterRequest();

            if (eaqfDashboardRequest.CustomerId > 0)
            {
                rejectionDashboardSearchRequest.CustomerId = eaqfDashboardRequest.CustomerId;
            }
            DateTime fromDate;
            DateTime toDate;
            if (DateTime.TryParseExact(eaqfDashboardRequest.FromDate, StandardDateFormat, CultureInfo.InvariantCulture,
                                                                                    DateTimeStyles.None, out fromDate))
            {
                rejectionDashboardSearchRequest.FromDate = new DateObject() { Year = fromDate.Year, Month = fromDate.Month, Day = fromDate.Day };
            }
            else
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidserviceFromDate });
            }
            if (DateTime.TryParseExact(eaqfDashboardRequest.ToDate, StandardDateFormat, CultureInfo.InvariantCulture,
                                                                                    DateTimeStyles.None, out toDate))
            {
                rejectionDashboardSearchRequest.ToDate = new DateObject() { Year = toDate.Year, Month = toDate.Month, Day = toDate.Day };
            }
            else
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidserviceToDate });
            }

            if (fromDate > toDate)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { GreterThanTodate });
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.Country))
            {
                // fetch valid country list
                List<int> countryList = new List<int>();
                var countryListRequest = eaqfDashboardRequest.Country.Split(',').Distinct().ToList();
                if (countryListRequest.Any())
                {
                    var validCountryList = await _locationRepo.GetCountriesByAlpha2CodeList(countryListRequest);
                    countryList.AddRange(validCountryList.Select(x => x.Id).ToList());
                    rejectionDashboardSearchRequest.FactoryCountryIds = countryList;
                }
            }

            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.ProductCategory))
            {
                List<int> productCategoryList = eaqfDashboardRequest.ProductCategory.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.SelectedProdCategoryIdList = productCategoryList;
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.ProductName))
            {
                List<int> productNameList = eaqfDashboardRequest.ProductName.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.SelectedProductIdList = productNameList;
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.Factory))
            {
                List<int> factoryList = eaqfDashboardRequest.Factory.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.FactoryCountryIds = factoryList;
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.Vendor))
            {
                List<int> vendorList = eaqfDashboardRequest.Vendor.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.SupplierId = vendorList[0];
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.ServiceType))
            {
                List<int> serviceTypeList = eaqfDashboardRequest.ServiceType.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.ServiceTypelst = serviceTypeList;
            }

            var dashBoardData = await _defectDashboardManager.GetAllDefectCount(rejectionDashboardSearchRequest);

            List<EaqfDashboardResponse> dashboardResponses = new List<EaqfDashboardResponse>();

            if (dashBoardData != null && dashBoardData.ParetoList != null)
            {
                foreach (var item in dashBoardData.ParetoList)
                {
                    dashboardResponses.Add(new EaqfDashboardResponse()
                    {
                        Name = item.DefectName,
                        Count = item.DefectCount,
                        Percentage = item.Percentage
                    });
                }
            }
            else
            {
                return new EaqfGetSuccessResponse()
                {
                    message = "Data Not found",
                    statusCode = HttpStatusCode.OK,
                    data = dashboardResponses
                };
            }

            return new EaqfGetSuccessResponse()
            {
                message = Success,
                statusCode = HttpStatusCode.OK,
                data = dashboardResponses
            };
        }

        public async Task<object> GetReportDefectAnalyticsByProductCatgory(EaqfDashboardRequest eaqfDashboardRequest)
        {
            if (eaqfDashboardRequest == null)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Invalid Request" });
            }
            if (eaqfDashboardRequest.CustomerId <= 0)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Customer Id is invalid" });
            }
            DTO.DefectDashboard.DefectDashboardFilterRequest rejectionDashboardSearchRequest = new DTO.DefectDashboard.DefectDashboardFilterRequest();

            if (eaqfDashboardRequest.CustomerId > 0)
            {
                rejectionDashboardSearchRequest.CustomerId = eaqfDashboardRequest.CustomerId;
            }
            DateTime fromDate;
            DateTime toDate;
            if (DateTime.TryParseExact(eaqfDashboardRequest.FromDate, StandardDateFormat, CultureInfo.InvariantCulture,
                                                                                    DateTimeStyles.None, out fromDate))
            {
                rejectionDashboardSearchRequest.FromDate = new DateObject() { Year = fromDate.Year, Month = fromDate.Month, Day = fromDate.Day };
            }
            else
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidserviceFromDate });
            }
            if (DateTime.TryParseExact(eaqfDashboardRequest.ToDate, StandardDateFormat, CultureInfo.InvariantCulture,
                                                                                    DateTimeStyles.None, out toDate))
            {
                rejectionDashboardSearchRequest.ToDate = new DateObject() { Year = toDate.Year, Month = toDate.Month, Day = toDate.Day };
            }
            else
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidserviceToDate });
            }

            if (fromDate > toDate)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { GreterThanTodate });
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.Country))
            {
                // fetch valid country list
                List<int> countryList = new List<int>();
                var countryListRequest = eaqfDashboardRequest.Country.Split(',').Distinct().ToList();
                if (countryListRequest.Any())
                {
                    var validCountryList = await _locationRepo.GetCountriesByAlpha2CodeList(countryListRequest);
                    countryList.AddRange(validCountryList.Select(x => x.Id).ToList());
                    rejectionDashboardSearchRequest.FactoryCountryIds = countryList;
                }
            }

            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.ProductCategory))
            {
                List<int> productCategoryList = eaqfDashboardRequest.ProductCategory.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.SelectedProdCategoryIdList = productCategoryList;
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.ProductName))
            {
                List<int> productNameList = eaqfDashboardRequest.ProductName.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.SelectedProductIdList = productNameList;
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.Factory))
            {
                List<int> factoryList = eaqfDashboardRequest.Factory.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.FactoryIds = factoryList;
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.Vendor))
            {
                List<int> vendorList = eaqfDashboardRequest.Vendor.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.SupplierId = vendorList[0];
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.ServiceType))
            {
                List<int> serviceTypeList = eaqfDashboardRequest.ServiceType.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.ServiceTypelst = serviceTypeList;
            }

            var dashBoardData = await _defectDashboardManager.GetAllDefectCount(rejectionDashboardSearchRequest);

            List<EaqfDashboardResponse> dashboardResponses = new List<EaqfDashboardResponse>();

            if (dashBoardData != null && dashBoardData.ParetoList != null)
            {
                foreach (var item in dashBoardData.ParetoList)
                {
                    dashboardResponses.Add(new EaqfDashboardResponse()
                    {
                        Name = item.DefectName,
                        Count = item.DefectCount,
                        Percentage = item.Percentage
                    });
                }
            }
            else
            {
                return new EaqfGetSuccessResponse()
                {
                    message = "Data Not found",
                    statusCode = HttpStatusCode.OK,
                    data = dashboardResponses
                };
            }

            return new EaqfGetSuccessResponse()
            {
                message = Success,
                statusCode = HttpStatusCode.OK,
                data = dashboardResponses
            };
        }

        public async Task<object> GetReportResultAnalytics(EaqfDashboardRequest eaqfDashboardRequest)
        {
            if (eaqfDashboardRequest == null)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Invalid Request" });
            }
            if (eaqfDashboardRequest.CustomerId <= 0)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Customer Id is invalid" });
            }
            RejectionDashboardSearchRequest rejectionDashboardSearchRequest = new RejectionDashboardSearchRequest();

            if (eaqfDashboardRequest.CustomerId > 0)
            {
                rejectionDashboardSearchRequest.CustomerId = eaqfDashboardRequest.CustomerId;
            }

            DateTime fromDate;
            DateTime toDate;
            if (DateTime.TryParseExact(eaqfDashboardRequest.FromDate, StandardDateFormat, CultureInfo.InvariantCulture,
                                                                                    DateTimeStyles.None, out fromDate))
            {
                rejectionDashboardSearchRequest.ServiceDateFrom = new DateObject() { Year = fromDate.Year, Month = fromDate.Month, Day = fromDate.Day };
            }
            else
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidserviceFromDate });
            }
            if (DateTime.TryParseExact(eaqfDashboardRequest.ToDate, StandardDateFormat, CultureInfo.InvariantCulture,
                                                                                    DateTimeStyles.None, out toDate))
            {
                rejectionDashboardSearchRequest.ServiceDateTo = new DateObject() { Year = toDate.Year, Month = toDate.Month, Day = toDate.Day };
            }
            else
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidserviceToDate });
            }

            if (fromDate > toDate)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { GreterThanTodate });
            }

            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.Country))
            {
                // fetch valid country list
                List<int> countryList = new List<int>();
                var countryListRequest = eaqfDashboardRequest.Country.Split(',').Distinct().ToList();
                if (countryListRequest.Any())
                {
                    var validCountryList = await _locationRepo.GetCountriesByAlpha2CodeList(countryListRequest);
                    countryList.AddRange(validCountryList.Select(x => x.Id).ToList());
                    rejectionDashboardSearchRequest.SelectedCountryIdList = countryList;
                }
            }

            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.ProductCategory))
            {
                List<int> productCategoryList = eaqfDashboardRequest.ProductCategory.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.SelectedProdCategoryIdList = productCategoryList;
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.ProductName))
            {
                List<int> productNameList = eaqfDashboardRequest.ProductName.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.SelectedProductIdList = productNameList;
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.Factory))
            {
                List<int> factoryList = eaqfDashboardRequest.Factory.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.SelectedFactoryIdList = factoryList;
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.Vendor))
            {
                List<int> vendorList = eaqfDashboardRequest.Vendor.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.SelectedSupplierIdList = vendorList;
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.ServiceType))
            {
                List<int> serviceTypeList = eaqfDashboardRequest.ServiceType.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.SelectedServiceTypeIdList = serviceTypeList;
            }

            var dashBoardData = await GetAllAPIResultDashboard(rejectionDashboardSearchRequest);

            List<EaqfDashboardResponse> dashboardResponses = new List<EaqfDashboardResponse>();

            if (dashBoardData != null && dashBoardData.Data != null)
            {
                foreach (var item in dashBoardData.Data)
                {
                    dashboardResponses.Add(new EaqfDashboardResponse()
                    {
                        Name = item.StatusName,
                        Count = item.TotalCount,
                        Percentage = GetPercentage(item.TotalCount, dashBoardData.TotalReports)
                    });
                }
            }
            else
            {
                return new EaqfGetSuccessResponse()
                {
                    message = "Data Not found",
                    statusCode = HttpStatusCode.OK,
                    data = dashboardResponses
                };
            }

            return new EaqfGetSuccessResponse()
            {
                message = Success,
                statusCode = HttpStatusCode.OK,
                data = dashboardResponses
            };
        }

        public async Task<object> GetReportRejectionResultByProductCategory(EaqfDashboardRequest eaqfDashboardRequest)
        {
            if (eaqfDashboardRequest == null)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Invalid Request" });
            }
            if (eaqfDashboardRequest.CustomerId <= 0)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Customer Id is invalid" });
            }
            RejectionDashboardSearchRequest rejectionDashboardSearchRequest = new RejectionDashboardSearchRequest();

            if (eaqfDashboardRequest.CustomerId > 0)
            {
                rejectionDashboardSearchRequest.CustomerId = eaqfDashboardRequest.CustomerId;
            }

            DateTime fromDate;
            DateTime toDate;
            if (DateTime.TryParseExact(eaqfDashboardRequest.FromDate, StandardDateFormat, CultureInfo.InvariantCulture,
                                                                                    DateTimeStyles.None, out fromDate))
            {
                rejectionDashboardSearchRequest.ServiceDateFrom = new DateObject() { Year = fromDate.Year, Month = fromDate.Month, Day = fromDate.Day };
            }
            else
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidserviceFromDate });
            }
            if (DateTime.TryParseExact(eaqfDashboardRequest.ToDate, StandardDateFormat, CultureInfo.InvariantCulture,
                                                                                    DateTimeStyles.None, out toDate))
            {
                rejectionDashboardSearchRequest.ServiceDateTo = new DateObject() { Year = toDate.Year, Month = toDate.Month, Day = toDate.Day };
            }
            else
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidserviceToDate });
            }

            if (fromDate > toDate)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { GreterThanTodate });
            }

            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.Country))
            {
                // fetch valid country list
                List<int> countryList = new List<int>();
                var countryListRequest = eaqfDashboardRequest.Country.Split(',').Distinct().ToList();
                if (countryListRequest.Any())
                {
                    var validCountryList = await _locationRepo.GetCountriesByAlpha2CodeList(countryListRequest);
                    countryList.AddRange(validCountryList.Select(x => x.Id).ToList());
                    rejectionDashboardSearchRequest.SelectedCountryIdList = countryList;
                }
            }

            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.ProductCategory))
            {
                List<int> productCategoryList = eaqfDashboardRequest.ProductCategory.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.SelectedProdCategoryIdList = productCategoryList;
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.ProductName))
            {
                List<int> productNameList = eaqfDashboardRequest.ProductName.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.SelectedProductIdList = productNameList;
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.Factory))
            {
                List<int> factoryList = eaqfDashboardRequest.Factory.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.SelectedFactoryIdList = factoryList;
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.Vendor))
            {
                List<int> vendorList = eaqfDashboardRequest.Vendor.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.SelectedSupplierIdList = vendorList;
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.ServiceType))
            {
                List<int> serviceTypeList = eaqfDashboardRequest.ServiceType.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.SelectedServiceTypeIdList = serviceTypeList;
            }

            var bookingIds = GetQueryableBookingIdList(rejectionDashboardSearchRequest);

            var dashBoardData = await _repo.GetProductCategoryDashboard(bookingIds, isExport: false, isOnlyRejectionResult: true);

            List<EaqfDashboardResponse> dashboardResponses = new List<EaqfDashboardResponse>();

            if (dashBoardData != null)
            {
                // fetch only failed result
                var totalCount = dashBoardData.Sum(x => x.TotalCount);

                if (dashBoardData.Any())
                {
                    foreach (var item in dashBoardData)
                    {
                        dashboardResponses.Add(new EaqfDashboardResponse()
                        {
                            Name = item.Name,
                            Count = item.TotalCount,
                            Percentage = GetPercentage(item.TotalCount, totalCount)
                        });
                    }
                }
                else
                {
                    return new EaqfGetSuccessResponse()
                    {
                        message = "Data Not found",
                        statusCode = HttpStatusCode.OK,
                        data = dashboardResponses
                    };
                }
            }
            else
            {
                return new EaqfGetSuccessResponse()
                {
                    message = "Data Not found",
                    statusCode = HttpStatusCode.OK,
                    data = dashboardResponses
                };
            }

            return new EaqfGetSuccessResponse()
            {
                message = Success,
                statusCode = HttpStatusCode.OK,
                data = dashboardResponses
            };
        }

        public async Task<object> GetReportRejectionResultByFactory(EaqfDashboardRequest eaqfDashboardRequest)
        {
            if (eaqfDashboardRequest == null)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Invalid Request" });
            }
            if (eaqfDashboardRequest.CustomerId <= 0)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Customer Id is invalid" });
            }
            RejectionDashboardSearchRequest rejectionDashboardSearchRequest = new RejectionDashboardSearchRequest();

            if (eaqfDashboardRequest.CustomerId > 0)
            {
                rejectionDashboardSearchRequest.CustomerId = eaqfDashboardRequest.CustomerId;
            }
            DateTime fromDate;
            DateTime toDate;
            if (DateTime.TryParseExact(eaqfDashboardRequest.FromDate, StandardDateFormat, CultureInfo.InvariantCulture,
                                                                                    DateTimeStyles.None, out fromDate))
            {
                rejectionDashboardSearchRequest.ServiceDateFrom = new DateObject() { Year = fromDate.Year, Month = fromDate.Month, Day = fromDate.Day };
            }
            else
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidserviceFromDate });
            }
            if (DateTime.TryParseExact(eaqfDashboardRequest.ToDate, StandardDateFormat, CultureInfo.InvariantCulture,
                                                                                    DateTimeStyles.None, out toDate))
            {
                rejectionDashboardSearchRequest.ServiceDateTo = new DateObject() { Year = toDate.Year, Month = toDate.Month, Day = toDate.Day };
            }
            else
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidserviceToDate });
            }

            if (fromDate > toDate)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { GreterThanTodate });
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.Country))
            {
                // fetch valid country list
                List<int> countryList = new List<int>();
                var countryListRequest = eaqfDashboardRequest.Country.Split(',').Distinct().ToList();
                if (countryListRequest.Any())
                {
                    var validCountryList = await _locationRepo.GetCountriesByAlpha2CodeList(countryListRequest);
                    countryList.AddRange(validCountryList.Select(x => x.Id).ToList());
                    rejectionDashboardSearchRequest.SelectedCountryIdList = countryList;
                }
            }

            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.ProductCategory))
            {
                List<int> productCategoryList = eaqfDashboardRequest.ProductCategory.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.SelectedProdCategoryIdList = productCategoryList;
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.ProductName))
            {
                List<int> productNameList = eaqfDashboardRequest.ProductName.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.SelectedProductIdList = productNameList;
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.Factory))
            {
                List<int> factoryList = eaqfDashboardRequest.Factory.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.SelectedFactoryIdList = factoryList;
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.Vendor))
            {
                List<int> vendorList = eaqfDashboardRequest.Vendor.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.SelectedSupplierIdList = vendorList;
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.ServiceType))
            {
                List<int> serviceTypeList = eaqfDashboardRequest.ServiceType.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.SelectedServiceTypeIdList = serviceTypeList;
            }

            var bookingIds = GetQueryableBookingIdList(rejectionDashboardSearchRequest);

            var dashBoardData = await _repo.GetFactoryDashboard(bookingIds, isExport: false, isOnlyRejectionResult: true);

            List<EaqfDashboardResponse> dashboardResponses = new List<EaqfDashboardResponse>();

            if (dashBoardData != null)
            {
                // fetch only failed result
                var totalCount = dashBoardData.Sum(x => x.TotalCount);

                if (dashBoardData.Any())
                {
                    foreach (var item in dashBoardData)
                    {
                        dashboardResponses.Add(new EaqfDashboardResponse()
                        {
                            Name = item.Name,
                            Count = item.TotalCount,
                            Percentage = GetPercentage(item.TotalCount, totalCount)
                        });
                    }
                }
                else
                {
                    return new EaqfGetSuccessResponse()
                    {
                        message = "Data Not found",
                        statusCode = HttpStatusCode.OK,
                        data = dashboardResponses
                    };
                }
            }
            else
            {
                return new EaqfGetSuccessResponse()
                {
                    message = "Data Not found",
                    statusCode = HttpStatusCode.OK,
                    data = dashboardResponses
                };
            }

            return new EaqfGetSuccessResponse()
            {
                message = Success,
                statusCode = HttpStatusCode.OK,
                data = dashboardResponses
            };
        }
        public async Task<object> GetDefectTypeDetails(EaqfDashboardRequest eaqfDashboardRequest)
        {
            if (eaqfDashboardRequest == null)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Invalid Request" });
            }
            if (eaqfDashboardRequest.CustomerId <= 0)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { "Customer Id is invalid" });
            }
            DTO.DefectDashboard.DefectDashboardFilterRequest rejectionDashboardSearchRequest = new DTO.DefectDashboard.DefectDashboardFilterRequest();
            if (eaqfDashboardRequest.CustomerId > 0)
            {
                rejectionDashboardSearchRequest.CustomerId = eaqfDashboardRequest.CustomerId;
            }
            DateTime fromDate;
            DateTime toDate;
            if (DateTime.TryParseExact(eaqfDashboardRequest.FromDate, StandardDateFormat, CultureInfo.InvariantCulture,
                                                                                    DateTimeStyles.None, out fromDate))
            {
                rejectionDashboardSearchRequest.FromDate = new DateObject() { Year = fromDate.Year, Month = fromDate.Month, Day = fromDate.Day };
            }
            else
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidserviceFromDate });
            }
            if (DateTime.TryParseExact(eaqfDashboardRequest.ToDate, StandardDateFormat, CultureInfo.InvariantCulture,
                                                                                    DateTimeStyles.None, out toDate))
            {
                rejectionDashboardSearchRequest.ToDate = new DateObject() { Year = toDate.Year, Month = toDate.Month, Day = toDate.Day };
            }
            else
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { InvalidserviceToDate });
            }

            if (fromDate > toDate)
            {
                return BuildCommonEaqfResponse(HttpStatusCode.BadRequest, BadRequest, new List<string>() { GreterThanTodate });
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.Country))
            {
                // fetch valid country list
                List<int> countryList = new List<int>();
                var countryListRequest = eaqfDashboardRequest.Country.Split(',').Distinct().ToList();
                if (countryListRequest.Any())
                {
                    var validCountryList = await _locationRepo.GetCountriesByAlpha2CodeList(countryListRequest);
                    countryList.AddRange(validCountryList.Select(x => x.Id).ToList());
                    rejectionDashboardSearchRequest.FactoryCountryIds = countryList;
                }
            }

            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.ProductCategory))
            {
                List<int> productCategoryList = eaqfDashboardRequest.ProductCategory.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.SelectedProdCategoryIdList = productCategoryList;
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.ProductName))
            {
                List<int> productNameList = eaqfDashboardRequest.ProductName.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.SelectedProductIdList = productNameList;
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.Factory))
            {
                List<int> factoryList = eaqfDashboardRequest.Factory.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.FactoryCountryIds = factoryList;
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.Vendor))
            {
                List<int> vendorList = eaqfDashboardRequest.Vendor.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.SupplierId = vendorList[0];
            }
            if (!string.IsNullOrWhiteSpace(eaqfDashboardRequest.ServiceType))
            {
                List<int> serviceTypeList = eaqfDashboardRequest.ServiceType.Split(',').Select(int.Parse).Distinct().ToList();
                rejectionDashboardSearchRequest.ServiceTypelst = serviceTypeList;
            }

            var defectList = await _defectDashboardManager.GetDefectTypeList(rejectionDashboardSearchRequest);

            EaqfDefectTypeReport response = new EaqfDefectTypeReport();

            if (defectList != null && defectList.Any())
            {
                response.Critical = defectList.Sum(x => x.Critical);
                response.Major = defectList.Sum(x => x.Major);
                response.Minor = defectList.Sum(x => x.Minor);
                response.TotalCount = defectList.Sum(x => x.Critical) + defectList.Sum(x => x.Major) + defectList.Sum(x => x.Minor);
            }
            else
            {
                return new EaqfGetSuccessResponse()
                {
                    message = "Data Not found",
                    statusCode = HttpStatusCode.OK
                };
            }
            return new EaqfGetSuccessResponse()
            {
                message = Success,
                statusCode = HttpStatusCode.OK,
                data = response
            };
        }

        public EaqfErrorResponse BuildCommonEaqfResponse(HttpStatusCode statusCode, string message, List<string> errors)
        {
            return new EaqfErrorResponse()
            {
                errors = errors,
                statusCode = statusCode,
                message = message
            };
        }

        public EaqfSaveSuccessResponse BuildCommonEaqSuccessResponse(HttpStatusCode statusCode, string message)
        {
            return new EaqfSaveSuccessResponse()
            {
                statusCode = statusCode,
                message = message
            };
        }
    }
}
