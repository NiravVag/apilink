using BI.Maps;
using BI.Maps.APP;
using BI.Utilities;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.Dashboard;
using DTO.Manday;
using DTO.MobileApp;
using DTO.User;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using static DTO.Common.Static_Data_Common;

namespace BI
{
    public class DashboardManager : ApiCommonData, IDashboardManager
    {

        private readonly IDashboardRepository _repo = null;
        private readonly IReferenceRepository _refRepo = null;
        private readonly ISupplierManager _supManager = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly IUserRightsManager _userRightsManager = null;
        private readonly IHelper _helper = null;
        private readonly ILocationRepository _locationRepo = null;
        private readonly IConfiguration _configuration = null;
        private readonly DashBoardMap _dashboardmap = null;
        private readonly CustomerDashboardMobileMap CustomerDashboardMobileMap = null;
        private readonly ISharedInspectionManager _sharedInspectionManager = null;

        public DashboardManager(IDashboardRepository repo, IReferenceRepository refRepo, IAPIUserContext applicationContextService, ISupplierManager supManager, IUserRightsManager userRightsManager, IHelper helper, ILocationRepository locationRepo, IConfiguration configuration,
            ISharedInspectionManager sharedInspectionManager)
        {
            _repo = repo;
            _refRepo = refRepo;
            _ApplicationContext = applicationContextService;
            _supManager = supManager;
            _userRightsManager = userRightsManager;
            _helper = helper;
            _locationRepo = locationRepo;
            _configuration = configuration;
            _dashboardmap = new DashBoardMap();
            CustomerDashboardMobileMap = new CustomerDashboardMobileMap();
            _sharedInspectionManager = sharedInspectionManager;
        }

        /// <summary>
        /// Get the booking detail based on the filter request
        /// </summary>
        /// <param name="request"></param>
        /// <returns>booking detail response which has the booking base data</returns>
        public async Task<List<BookingDetail>> GetBookingDetails(CustomerDashboardFilterRequest request)
        {
            var bookingDetails = _repo.GetBookingDetail(request);


            return await bookingDetails.Select(x =>
                                    new BookingDetail
                                    {
                                        InspectionId = x.Id,
                                        CustomerId = x.CustomerId,
                                        SupplierId = x.SupplierId,
                                        FactoryId = x.FactoryId,
                                        CreationDate = x.CreatedOn.Value,
                                        ServiceDateFrom = x.ServiceDateFrom,
                                        ServiceDateTo = x.ServiceDateTo,
                                        StatusId = x.StatusId
                                    }).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the customer business overview dashboard
        /// </summary>
        /// <param name="request"></param>
        /// <param name="bookingDetails"></param>
        /// <returns>CustomerBusinessOVDashboard data</returns>
        public async Task<CustomerBusinessOVDashboard> GetCustomerBusinessOverview(CustomerDashboardFilterRequest request)
        {
            CustomerBusinessOVDashboard dashboard = new CustomerBusinessOVDashboard();
            BusinessOVBookingDetail lastYearBusinessData = null;
            //get the current year base data
            var currentYearBookingDetails = await GetBookingDetails(request);
            var currentYearBusinessData = await GetBusinessOvBaseData(currentYearBookingDetails);
            if (request != null)
            {
                if (currentYearBookingDetails != null && currentYearBookingDetails.Any())
                {
                    //last year booking data
                    if (request.ServiceDateFrom != null && request.ServiceDateTo != null)
                    {
                        request.ServiceDateFrom.Year = request.ServiceDateFrom.Year - 1;
                        request.ServiceDateTo.Year = request.ServiceDateTo.Year - 1;
                        var lastYearBookingDetails = await GetBookingDetails(request);
                        lastYearBusinessData = await GetBusinessOvBaseData(lastYearBookingDetails);
                    }
                }
                //create the final dashboard data with current year and last yer booking data
                dashboard = _dashboardmap.MapCustomerBusinessOVDashBoard(currentYearBusinessData, lastYearBusinessData);
            }
            return dashboard;

        }

        /// <summary>
        /// Get Base BookingDetails BusinessOVBookingDetail
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<BusinessOVBookingDetail> GetBusinessOvBaseData(List<BookingDetail> bookingDetails)
        {
            BusinessOVBookingDetail bookingDetail = new BusinessOVBookingDetail();
            if (bookingDetails != null && bookingDetails.Any())
            {
                //take the inspection ids
                var inspectionIds = bookingDetails.Select(x => x.InspectionId);

                bookingDetail.BookingCount = bookingDetails.Count(x => x.InspectionId > 0);
                bookingDetail.ProductsCount = await _repo.GetProductCount(inspectionIds);
                bookingDetail.FactoryCount = bookingDetails.Select(x => x.FactoryId).
                                            Distinct().Count();
                bookingDetail.ManDays = await _repo.GetInspectionManDays(inspectionIds).AsNoTracking().SumAsync(x => x.MandayCount) ?? 0;
            }

            return bookingDetail;
        }

        /// <summary>
        /// Get geo code for country
        /// </summary>
        /// <param name="inspectionids"></param>
        /// <returns></returns>
        public async Task<MapGeoLocation> GetInspCountryGeoCode(DashboardMapFilterRequest dashboardMapFilterRequest)
        {
            var response = new MapGeoLocation();
            var inspectionQuery = _sharedInspectionManager.GetAllInspectionQuery();
            var inspectionQueryRequest = _sharedInspectionManager.GetDashboardMapInspectionRequestMap(dashboardMapFilterRequest);
            var inspections = _sharedInspectionManager.GetInspectionQuerywithRequestFilters(inspectionQueryRequest, inspectionQuery);
            if (dashboardMapFilterRequest.DashboardType == (int)DashboardMapEnum.CustomerDashboard)
            {
                inspections = inspections.Where(x => x.StatusId != (int)BookingStatus.Cancel);
            }

            var lstgeocode = await _repo.GetInspCountryGeoCode(inspections.Select(x => x.Id));
            var grpbygeocode = lstgeocode.GroupBy(p => p.FactoryCountryId, (key, _data) => new InspCountryGeoCode
            {
                FactoryCountryId = key,
                FactoryCountryName = _data.Where(x => x.FactoryCountryId == key).Select(x => x.FactoryCountryName).FirstOrDefault(),
                FactoryCountryCode = _data.Where(x => x.FactoryCountryId == key).Select(x => x.FactoryCountryCode).FirstOrDefault(),
                Latitude = _data.Where(x => x.FactoryCountryId == key).Select(x => x.Latitude).FirstOrDefault(),
                Longitude = _data.Where(x => x.FactoryCountryId == key).Select(x => x.Longitude).FirstOrDefault(),
                TotalCount = _data.Count()
            }).ToList();

            //Check geo country coordinates are null then need to update coordinates from MAPBOX API
            if (grpbygeocode != null && grpbygeocode.Count() > 0)
            {
                response.CountryGeoCodeResult = MapGeoLocationResult.Success;
                grpbygeocode = await updateMapBoxCountryGeoCode(grpbygeocode, response);
            }

            var grpByProvince = lstgeocode.GroupBy(p => p.FactoryProvinceId, (key, _data) => new InspProvinceGeoCode
            {
                FactoryProvinceId = key,
                FactoryProvinceName = _data.Where(x => x.FactoryProvinceId == key).Select(x => x.FactoryProvinceName).FirstOrDefault(),
                Latitude = _data.Where(x => x.FactoryProvinceId == key).Select(x => x.ProvinceLatitude).FirstOrDefault(),
                Longitude = _data.Where(x => x.FactoryProvinceId == key).Select(x => x.ProvinceLongitude).FirstOrDefault(),
                TotalCount = _data.Count()
            }).ToList();

            //Check geo Province coordinates are null then need to update coordinates from MAPBOX API
            if (grpByProvince != null && grpByProvince.Count() > 0)
            {
                response.ProvinceGeoCodeResult = MapGeoLocationResult.Success;
                grpByProvince = await updateMapBoxProvinceGeoCode(grpByProvince, response);
            }

            var grpByFactory = lstgeocode.GroupBy(p => p.FactoryId, (key, _data) => new InspFactoryGeoCode
            {
                FactoryId = key,
                FactoryName = _data.Where(x => x.FactoryId == key).Select(x => x.FactoryName).FirstOrDefault(),
                Latitude = _data.Where(x => x.FactoryId == key).Select(x => x.FactoryLatitude).FirstOrDefault(),
                Longitude = _data.Where(x => x.FactoryId == key).Select(x => x.FactoryLongitude).FirstOrDefault(),
                TotalCount = _data.Count()
            }).ToList();

            //var provinceGeoCodes = await _repo.GetInspProvinceGeoCode(lstinsp);

            response.CountryGeoCode = grpbygeocode;
            response.ProvinceGeoCode = grpByProvince.Where(x => x.Latitude.HasValue && x.Longitude.HasValue).ToList();
            response.FactoryGeoCode = grpByFactory.Where(x => x.Latitude.HasValue && x.Longitude.HasValue).ToList();
            return response;
        }

        /// <summary>
        /// Update MAPBOX API geo code for country 
        /// </summary>
        /// <param name="shortCountryCodes"></param>
        /// <returns></returns>
        private async Task<List<InspCountryGeoCode>> updateMapBoxCountryGeoCode(List<InspCountryGeoCode> grpbygeocode, MapGeoLocation response)
        {
            var emptyGeoCodeLst = grpbygeocode.Where(x => x.Longitude == null || x.Latitude == null).ToList();
            var emptyGeoIds = grpbygeocode.Where(x => x.Longitude == null || x.Latitude == null).Select(x => x.FactoryCountryId).ToList();
            if (emptyGeoCodeLst != null && emptyGeoCodeLst.Count() > 0)
            {
                var shortCountryCodes = string.Join(",", emptyGeoCodeLst.Select(x => x.FactoryCountryCode));
                grpbygeocode = await GetMapBoxCountryGeoCode(grpbygeocode, shortCountryCodes, response);//call MAPBOX API

                var countryLst = await _locationRepo.GetCountriesByIds(emptyGeoIds);

                foreach (var item in grpbygeocode.Where(x => emptyGeoIds.Contains(x.FactoryCountryId)))
                {
                    countryLst.FirstOrDefault(x => x.Id == item.FactoryCountryId).Longitude = item.Longitude;
                    countryLst.FirstOrDefault(x => x.Id == item.FactoryCountryId).Latitude = item.Latitude;
                }

                _repo.EditEntities(countryLst);
                await _repo.Save();
            }
            return grpbygeocode;

        }

        /// <summary>
        /// Get MAPBOX API geo code for country 
        /// </summary>
        /// <param name="shortCountryCodes"></param>
        /// <returns></returns>
        private async Task<List<InspCountryGeoCode>> GetMapBoxCountryGeoCode(List<InspCountryGeoCode> countryGeoCode, string shortCountryCodes, MapGeoLocation response)
        {
            try
            {
                decimal number;
                //Mapbox Api URL from appsettings
                var mapBoxURL = string.Format(_configuration["UrlMapBoxGeoCountry"], shortCountryCodes);
                HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Get, "", null, mapBoxURL, "");

                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    var reportData = httpResponse.Content.ReadAsStringAsync();
                    JObject reportDataJson = JObject.Parse(reportData.Result);
                    if (reportDataJson != null && reportDataJson.GetValue("features") != null)
                    {
                        foreach (var geoCode in countryGeoCode)
                        {
                            if (geoCode.Longitude == null || geoCode.Latitude == null)
                            {

                                foreach (var item in reportDataJson.GetValue("features").ToArray())
                                {
                                    if (item["properties"]["short_code"].ToString().ToLower() == geoCode.FactoryCountryCode.ToLower())
                                    {
                                        if (item["geometry"] != null && item["geometry"]["coordinates"] != null)
                                        {
                                            if (Decimal.TryParse((item["geometry"]["coordinates"][0]).ToString(), out number))
                                                geoCode.Longitude = number;
                                            else
                                                geoCode.Longitude = null;

                                            if (Decimal.TryParse((item["geometry"]["coordinates"][1]).ToString(), out number))
                                                geoCode.Latitude = number;
                                            else
                                                geoCode.Latitude = null;


                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    response.CountryGeoCodeResult = MapGeoLocationResult.Failure;
                }
                return countryGeoCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update  MAPBOX API geo code for province 
        /// </summary>
        /// <param name="grpByProvince"></param>
        /// <returns></returns>
        private async Task<List<InspProvinceGeoCode>> updateMapBoxProvinceGeoCode(List<InspProvinceGeoCode> grpByProvince, MapGeoLocation response)
        {
            var emptyGeoCodeLst = grpByProvince.Where(x => x.Longitude == null || x.Latitude == null).ToList();
            var emptyGeoIds = grpByProvince.Where(x => x.Longitude == null || x.Latitude == null).Select(x => x.FactoryProvinceId).ToList();
            if (emptyGeoCodeLst != null && emptyGeoCodeLst.Count() > 0)
            {
                grpByProvince = await GetMapBoxProvinceGeoCode(grpByProvince, response);//call MAPBOX API
                if (grpByProvince != null && grpByProvince.Any())
                {
                    var provinceLst = await _locationRepo.GetProvinceByIds(emptyGeoIds);

                    foreach (var item in grpByProvince.Where(x => emptyGeoIds.Contains(x.FactoryProvinceId)))
                    {
                        provinceLst.FirstOrDefault(x => x.Id == item.FactoryProvinceId).Longitude = item.Longitude;
                        provinceLst.FirstOrDefault(x => x.Id == item.FactoryProvinceId).Latitude = item.Latitude;
                    }

                    _repo.EditEntities(provinceLst);
                    await _repo.Save();
                }
            }
            return grpByProvince;
        }

        /// <summary>
        /// Get MAPBOX API geo code for province 
        /// </summary>
        /// <param name="provinceGeoCode"></param>
        /// <returns></returns>
        private async Task<List<InspProvinceGeoCode>> GetMapBoxProvinceGeoCode(List<InspProvinceGeoCode> provinceGeoCode, MapGeoLocation response)
        {
            try
            {
                decimal number;
                foreach (var geoCode in provinceGeoCode)
                {
                    if (geoCode.Longitude == null || geoCode.Latitude == null)
                    {
                        var _provinceName = geoCode.FactoryProvinceName;
                        if (_provinceName.Contains("("))
                            _provinceName = _provinceName.Split("(")[0].ToString().Trim();

                        //Mapbox Api URL from appsettings
                        var mapBoxURL = string.Format(_configuration["UrlMapBoxGeoPalce"], _provinceName);

                        HttpResponseMessage httpResponse = await _helper.SendRequestToPartnerAPI(Method.Get, "", null, mapBoxURL, "");

                        if (httpResponse != null && httpResponse.StatusCode == HttpStatusCode.OK)
                        {
                            var reportData = httpResponse.Content.ReadAsStringAsync();
                            if (reportData != null)
                            {
                                JObject reportDataJson = JObject.Parse(reportData.Result);
                                if (reportDataJson != null && reportDataJson.GetValue("features") != null)
                                {

                                    var item = reportDataJson["features"][0];
                                    if (item != null && item["geometry"] != null && item["geometry"]["coordinates"] != null)
                                    {
                                        if (Decimal.TryParse((item["geometry"]["coordinates"][0]).ToString(), out number))
                                            geoCode.Longitude = number;
                                        else
                                            geoCode.Longitude = null;

                                        if (Decimal.TryParse((item["geometry"]["coordinates"][1]).ToString(), out number))
                                            geoCode.Latitude = number;
                                        else
                                            geoCode.Latitude = null;
                                    }
                                }
                                else
                                {
                                    response.ProvinceGeoCodeResult = MapGeoLocationResult.Failure;
                                }
                            }
                        }
                    }
                }
                return provinceGeoCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get geo code for country allocated inspection
        /// </summary>
        /// <param name="inspectionids"></param>
        /// <returns></returns>
        public async Task<List<InspCountryGeoCode>> GetAllocatedInspCountryGeoCode()
        {
            var lstgeocode = await _repo.GetInspCountryGeoCodeAllocated(_ApplicationContext.CustomerId);
            var grpbygeocode = lstgeocode.GroupBy(p => p.FactoryCountryId, (key, _data) => new InspCountryGeoCode
            {
                FactoryCountryId = key,
                FactoryCountryName = _data.Where(x => x.FactoryCountryId == key).Select(x => x.FactoryCountryName).FirstOrDefault(),
                Latitude = _data.Where(x => x.FactoryCountryId == key).Select(x => x.Latitude).FirstOrDefault(),
                Longitude = _data.Where(x => x.FactoryCountryId == key).Select(x => x.Longitude).FirstOrDefault(),
                TotalCount = _data.Count()
            }).ToList();
            return grpbygeocode;
        }

        /// <summary>
        /// Get the API Result Analysis data by inspectionIds
        /// </summary>
        /// <param name="inspectionids"></param>
        /// <returns></returns>
        public async Task<List<CustomerAPIRADashboard>> GetAPIRADashboard(CustomerDashboardFilterRequest request)
        {
            var bookingDetails = _repo.GetBookingDetail(request);

            var inspectionIds = bookingDetails.Select(x => x.Id);

            //Get the FBReport Result Data
            var fbReportResults = _repo.GetFbReportResults();

            //Get the inspected booking status list from the below dictionary
            var inspectedStatusList = InspectedStatusList.Select(x => x);

            //take the API Result Analysis Data
            var apiRADashboard = _repo.GetAPIRADashboard(inspectionIds, inspectedStatusList);

            //filter product name
            if (request.ProductIdList != null && request.ProductIdList.Any())
            {
                apiRADashboard = apiRADashboard.Where(x => x.Inspection.InspProductTransactions.Any(y => y.Active.Value && request.ProductIdList.Contains(y.ProductId)));
            }

            //filter product category
            if (request.ProdCategoryList != null && request.ProdCategoryList.Any())
            {
                apiRADashboard = apiRADashboard.Where(x => x.Inspection.InspProductTransactions.Any(y => y.Active.Value && request.ProdCategoryList.Contains(y.Product.ProductCategory.Value)));
            }

            var apiRADashboardList = await apiRADashboard.GroupBy(p => p.ResultId, (key, _data) =>
            new CustomerAPIRADashboardRepo
            {
                ResultId = key,
                TotalCount = _data.Select(x => x.Id).Distinct().Count()
            }).AsNoTracking().ToListAsync();

            //map the status color for the apiRADashboard data
            return _dashboardmap.MapStatusColorAPIRADashboard(apiRADashboardList, fbReportResults);
        }
        /// <summary>
        /// Get the API Result Analysis data by inspectionIds
        /// </summary>
        /// <param name="inspectionids"></param>
        /// <returns></returns>
        public async Task<List<CustomerAPIRADashboard>> GetQueriableAPIRADashboard(IQueryable<int> inspectionids)
        {
            //Get the FBReport Result Data
            var fbReportResults = _repo.GetFbReportResults();

            //Get the inspected booking status list from the below dictionary
            var inspectedStatusList = InspectedStatusList.Select(x => x);

            //take the API Result Analysis Data
            var apiRADashboard = await _repo.GetQueriableAPIRADashboard(inspectionids, inspectedStatusList);
            //map the status color for the apiRADashboard data
            return _dashboardmap.MapStatusColorAPIRADashboard(apiRADashboard, fbReportResults);
        }


        public async Task<List<CustomerAPIRADashboard>> GetAPIRADashboardByQuery(IQueryable<int> inspectionids)
        {
            //Get the FBReport Result Data
            var fbReportResults = _repo.GetFbReportResults();

            //Get the inspected booking status list from the below dictionary
            var inspectedStatusList = InspectedStatusList.Select(x => x);

            //take the API Result Analysis Data
            var apiRADashboard = await _repo.GetAPIRADashboardByQuery(inspectionids, inspectedStatusList);
            //map the status color for the apiRADashboard data
            return _dashboardmap.MapStatusColorAPIRADashboard(apiRADashboard, fbReportResults);
        }

        /// <summary>
        /// Get the customer result dashboards
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        public async Task<List<CustomerResultDashboard>> GetCustomerResultDashBoard(CustomerDashboardFilterRequest request)
        {
            var bookingDetails = _repo.GetBookingDetail(request);

            var inspectionIds = bookingDetails.Select(x => x.Id);

            //Get the inspected booking status list from the below dictionary
            var inspectedStatusList = InspectedStatusList.Select(x => x);

            //take the customer result data
            var customerResult = _repo.GetCustomerResult(inspectionIds, inspectedStatusList);

            //filter product name
            if (request.ProductIdList != null && request.ProductIdList.Any())
            {
                customerResult = customerResult.Where(x => x.Report.Inspection.InspProductTransactions.Any(y => y.Active.Value && request.ProductIdList.Contains(y.ProductId)));
            }

            //filter product category
            if (request.ProdCategoryList != null && request.ProdCategoryList.Any())
            {
                customerResult = customerResult.Where(x => x.Report.Inspection.InspProductTransactions.Any(y => y.Active.Value && request.ProdCategoryList.Contains(y.Product.ProductCategory.Value)));
            }

            var customerResultList = await customerResult.GroupBy(p => p.CustomerResultId, (key, _data) =>
            new CustomerResultRepo
            {
                Id = key,
                TotalCount = _data.Select(x => x.ReportId).Distinct().Count()
            }).AsNoTracking().ToListAsync();

            //take the customer resultIds
            var customerResultIds = customerResultList.Select(x => x.Id).ToList();

            //take the customer result data from the resultids
            var customerResultAnalysis = await _repo.GetCustomerResultAnalysis(customerResultIds);
            //map the customer result dashboard evaluate the name and assign the color
            return _dashboardmap.MapCustomerResultAnalysis(customerResultAnalysis, customerResultList);

        }
        /// <summary>
        /// Get the customer result dashboards
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        public async Task<List<CustomerResultDashboard>> GetQueryableCustomerResultDashBoard(IQueryable<int> inspectionIds)
        {
            //Get the inspected booking status list from the below dictionary
            var inspectedStatusList = InspectedStatusList.Select(x => x);

            //take the customer result data
            var customerResult = await _repo.GetQueryableCustomerResult(inspectionIds, inspectedStatusList);

            //take the customer resultIds
            var customerResultIds = customerResult.Select(x => x.Id).ToList();

            //take the customer result data from the resultids
            var customerResultAnalysis = await _repo.GetCustomerResultAnalysis(customerResultIds);
            //map the customer result dashboard evaluate the name and assign the color
            return _dashboardmap.MapCustomerResultAnalysis(customerResultAnalysis, customerResult);

        }

        /// <summary>
        /// Get the product category data by inspection ids
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        public async Task<List<ProductCategoryDashboard>> GetProductCategoryDashboard(CustomerDashboardFilterRequest request)
        {
            var bookingDetails = _repo.GetBookingDetail(request);

            var inspectionIds = bookingDetails.Select(x => x.Id);

            //Get the product category data
            var productCategoryDashboard = _repo.GetProductCategoryDashboard(inspectionIds);

            //filter product name
            if (request.ProductIdList != null && request.ProductIdList.Any())
            {
                productCategoryDashboard = productCategoryDashboard.Where(x => request.ProductIdList.Contains(x.ProductId));
            }

            //filter product category
            if (request.ProdCategoryList != null && request.ProdCategoryList.Any())
            {
                productCategoryDashboard = productCategoryDashboard.Where(x => request.ProdCategoryList.Contains(x.Product.ProductCategory.Value));
            }

            var productCategoryList = await productCategoryDashboard.GroupBy(p => p.Product.ProductCategory, (key, _data) =>
            new ProductCategoryDashboardRepo
            {
                Id = key,
                TotalCount = _data.Count()
            }).AsNoTracking().ToListAsync();

            var productCategoryData = await _refRepo.GetProductCategories();
            return _dashboardmap.MapProductCategoryDashboard(productCategoryList, productCategoryData);
        }

        /// <summary>
        /// Get the supplier performance based on booking leadtime and etd leadtime and supplier revision
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<SupplierPerformanceDashboard> GetSupplierPerformDashBoard(CustomerDashboardFilterRequest request)
        {
            var bookingDetails = await GetBookingDetails(request);
            //take the inspection ids
            var inspectionids = bookingDetails.Select(x => x.InspectionId);
            //get the etd data or inspection po details
            var POETDDate = await _repo.GetPOETDDateByInspectionId(inspectionids);
            //get the inspection data group by servicedatefrom and servicedateto from status_log table
            var supplierRevision = await _repo.GetSupplierRevisionData(inspectionids);
            //map the supplier performance dashboard
            return _dashboardmap.MapSupplierPerformance(bookingDetails, POETDDate, supplierRevision);

        }

        /// <summary>
        /// Get the inspection reject data by inspectionids
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        public async Task<List<InspectionRejectDashboard>> GetInspectionRejectDashBoard(CustomerDashboardFilterRequest request)
        {
            var bookingDetails = _repo.GetBookingDetail(request);

            var inspectionIds = bookingDetails.Select(x => x.Id);

            //Get the inspected booking status list from the below dictionary
            var inspectedStatusList = InspectedStatusList.Select(x => x);
            //get the customer inspectin reject details
            var inspectionRejectDashboard = _repo.GetCustomerInspectionReject(inspectionIds, inspectedStatusList);

            //filter product name
            if (request.ProductIdList != null && request.ProductIdList.Any())
            {
                inspectionRejectDashboard = inspectionRejectDashboard.Where(x => x.FbReportDetail.Inspection.InspProductTransactions.Any(y => y.Active.Value && request.ProductIdList.Contains(y.ProductId)));
            }

            //filter product category
            if (request.ProdCategoryList != null && request.ProdCategoryList.Any())
            {
                inspectionRejectDashboard = inspectionRejectDashboard.Where(x => x.FbReportDetail.Inspection.InspProductTransactions.Any(y => y.Active.Value && request.ProdCategoryList.Contains(y.Product.ProductCategory.Value)));
            }

            var inspectionRejectList = await inspectionRejectDashboard.GroupBy(p => p.Name, (key, _data) =>
            new InspectionRejectDashboard
            {
                StatusName = key,
                TotalCount = _data.Count()
            }).AsNoTracking().ToListAsync();

            return _dashboardmap.MapInspectionRejectDashboard(inspectionRejectList);
        }

        public async Task<List<InspectionRejectDashboard>> GetInspectionRejectDashBoardByBookingQuery(IQueryable<int> inspectionIds)
        {
            //Get the inspected booking status list from the below dictionary
            var inspectedStatusList = InspectedStatusList.Select(x => x);
            //get the customer inspectin reject details
            var inspectionRejectDashboard = await _repo.GetCustomerInspectionRejectByQuery(inspectionIds, inspectedStatusList);
            return _dashboardmap.MapInspectionRejectDashboard(inspectionRejectDashboard);
        }

        /// <summary>
        /// Get the pending quotation count
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<QuotationTaskData> GetQuotationTasks(int customerId)
        {
            QuotationTaskData quotationTaskData = new QuotationTaskData();
            //if the user has quotation confirmation role then take the task
            if (_ApplicationContext.RoleList.Contains((int)RoleEnum.QuotationConfirmation))
            {
                quotationTaskData.PendingQuotations = await _repo.GetPendingQuotations(customerId);
                quotationTaskData.CompletedQuotations = await _repo.GetCompletedQuotations(customerId);

            }
            return quotationTaskData;
        }

        /// <summary>
        /// Get the man day data by type(daily,weekly,monthly)
        /// </summary>
        /// <param name="manDaySearchData"></param>
        /// <returns></returns>
        public async Task<ManDayDashboard> GetManDaysData(int manDaySearchData, int customerId)
        {
            ManDayDashboard manDayDashboard = new ManDayDashboard();

            if (customerId == 0)
                customerId = _ApplicationContext.CustomerId;

            if (manDaySearchData == (int)ManDaySearchEnum.Weekly)
            {
                manDayDashboard = await GetWeeklyManDayData(customerId);
            }
            else if (manDaySearchData == (int)ManDaySearchEnum.Monthly)
            {
                manDayDashboard = await GetMonthlyManDayData(customerId);
            }
            else if (manDaySearchData == (int)ManDaySearchEnum.Daily)
            {
                manDayDashboard = await GetDailyManDayData(customerId);
            }
            return manDayDashboard;
        }

        private async Task<ManDayDashboard> GetDailyManDayData(int customerId)
        {
            //Get the inspected booking status list from the below dictionary
            var inspectedStatusList = InspectedStatusList.Select(x => x);

            ManDayDashboard manDayDashboard = new ManDayDashboard();
            List<ManDayData> mandaysList = new List<ManDayData>();
            //Get the current year data for past 7 days
            DateTime ServiceDateFrom = DateTime.Now.AddDays(-6);
            DateTime ServiceDateTo = DateTime.Now;
            var dailyManDays = await _repo.GetInspectionManDays(ServiceDateFrom, ServiceDateTo, inspectedStatusList, customerId);
            mandaysList = _dashboardmap.MapDailyManDayData(dailyManDays, ServiceDateFrom, ServiceDateTo);
            manDayDashboard.CurrentYearData = mandaysList;

            //Get the last year current data for the past 7 days from the current date's last year
            ServiceDateFrom = DateTime.Now.AddYears(-1).AddDays(-6);
            ServiceDateTo = DateTime.Now.AddYears(-1);
            dailyManDays = await _repo.GetInspectionManDays(ServiceDateFrom, ServiceDateTo, inspectedStatusList, customerId);
            mandaysList = _dashboardmap.MapDailyManDayData(dailyManDays, ServiceDateFrom, ServiceDateTo);
            manDayDashboard.LastYearData = mandaysList;

            return manDayDashboard;
        }

        private async Task<ManDayDashboard> GetWeeklyManDayData(int customerId)
        {
            //Get the inspected booking status list from the below dictionary
            var inspectedStatusList = InspectedStatusList.Select(x => x);
            ManDayDashboard manDayDashboard = new ManDayDashboard();
            List<ManDayData> mandaysList = new List<ManDayData>();
            //Get the current year current last 7 weeks mandaydata
            DateTime ServiceDateFrom = DateTime.Now.AddDays(-49);
            DateTime ServiceDateTo = DateTime.Now;

            var dailyManDays = await _repo.GetInspectionManDays(ServiceDateFrom, ServiceDateTo, inspectedStatusList, customerId);
            mandaysList = _dashboardmap.MapWeeklyManDayData(dailyManDays, ServiceDateFrom, ServiceDateTo);
            manDayDashboard.CurrentYearData = mandaysList;

            //Get the last year current last 7 weeks mandaydata
            ServiceDateFrom = DateTime.Now.AddYears(-1).AddDays(-49);
            ServiceDateTo = DateTime.Now.AddYears(-1);

            dailyManDays = await _repo.GetInspectionManDays(ServiceDateFrom, ServiceDateTo, inspectedStatusList, customerId);
            mandaysList = _dashboardmap.MapWeeklyManDayData(dailyManDays, ServiceDateFrom, ServiceDateTo);
            manDayDashboard.LastYearData = mandaysList;

            return manDayDashboard;
        }

        private async Task<ManDayDashboard> GetMonthlyManDayData(int customerId)
        {
            ManDayDashboard manDayDashboard = new ManDayDashboard();
            List<ManDayData> mandaysList = new List<ManDayData>();
            //Get the inspected booking status list from the below dictionary
            var inspectedStatusList = InspectedStatusList.Select(x => x);

            //Get the current year current last 7 months mandaydata
            DateTime ServiceDateFrom = DateTime.Now.AddMonths(-6);
            DateTime ServiceDateTo = DateTime.Now;
            var manDaysRepo = await _repo.GetMonthlyInspectionManDays(ServiceDateFrom, ServiceDateTo, inspectedStatusList, customerId);
            mandaysList = _dashboardmap.MapMonthlyManDayData(manDaysRepo, ServiceDateFrom, ServiceDateTo);
            manDayDashboard.CurrentYearData = mandaysList;

            //Get the last year current last 7 months mandaydata
            ServiceDateFrom = DateTime.Now.AddYears(-1).AddMonths(-6);
            ServiceDateTo = DateTime.Now.AddYears(-1);
            manDaysRepo = await _repo.GetMonthlyInspectionManDays(ServiceDateFrom, ServiceDateTo, inspectedStatusList, customerId);
            mandaysList = _dashboardmap.MapMonthlyManDayData(manDaysRepo, ServiceDateFrom, ServiceDateTo);
            manDayDashboard.LastYearData = mandaysList;
            return manDayDashboard;
        }

        /// <summary>
        /// Get the inspected bookings data
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        public async Task<InspectionManDayOverview> GetInspectionManDayOverview(CustomerDashboardFilterRequest request)
        {
            InspectionManDayOverview dashboarData = new InspectionManDayOverview();
            dashboarData.AverageExceeds = false;
            dashboarData.ManDayExceeds = false;
            if (request != null)
            {

                InspectionManDayData currentYearManDayData = new InspectionManDayData();
                InspectionManDayData lastYearManDayData = new InspectionManDayData();
                List<POProductsRepo> POProducts = null;
                //Get the current year data
                var currentYearBookingDetails = await GetBookingDetails(request);
                var inspectedStatusList = InspectedStatusList.Select(x => x);
                if (currentYearBookingDetails != null && currentYearBookingDetails.Any())
                {
                    var inspectionIds = currentYearBookingDetails.Select(x => x.InspectionId);
                    //get total inspected bookings and total manday for the current year
                    currentYearManDayData = await GetInspectionManDayData(inspectionIds);
                    POProducts = await _repo.GetPOProducts(inspectionIds, inspectedStatusList);
                }

                //last year booking data
                if (request.ServiceDateFrom != null && request.ServiceDateTo != null)
                {
                    if (currentYearBookingDetails != null && currentYearBookingDetails.Any())
                    {
                        request.ServiceDateFrom.Year = request.ServiceDateFrom.Year - 1;
                        request.ServiceDateTo.Year = request.ServiceDateTo.Year - 1;
                        //get total inspected bookings and total manday for the last year
                        var lastYearBookingDetails = await GetBookingDetails(request);
                        if (lastYearBookingDetails != null && lastYearBookingDetails.Any())
                        {
                            var inspectionIds = lastYearBookingDetails.Select(x => x.InspectionId);
                            lastYearManDayData = await GetInspectionManDayData(inspectionIds);
                        }
                        request.ServiceDateFrom.Year = request.ServiceDateFrom.Year + 1;
                        request.ServiceDateTo.Year = request.ServiceDateTo.Year + 1;
                    }
                }


                dashboarData = _dashboardmap.MapInspectionManDayOverview(currentYearManDayData, lastYearManDayData, POProducts);
            }

            return dashboarData;
        }

        private async Task<InspectionManDayData> GetInspectionManDayData(IEnumerable<int> inspectionIds)
        {
            //Get the inspected booking status list from the below dictionary
            var inspectedStatusList = InspectedStatusList.Select(x => x);
            InspectionManDayData manDayData = new InspectionManDayData();
            manDayData.TotalInspections = await _repo.GetInspectedBookingCount(inspectionIds, inspectedStatusList);
            manDayData.TotalManDays = await _repo.GetInspectedManDaysCount(inspectionIds, inspectedStatusList);
            return manDayData;
        }

        /// <summary>
        /// Get the customer mapped cs contact
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<UserStaffDetails> GetCustomerCsContact(int customerId)
        {
            return await _repo.GetCSDetails(customerId);
        }

        //Fetch the bookings and report fail reasons based on Factory 
        public async Task<CustomerFactoryDashboard> GetInspectedBookingsByFactory(CustomerDashboardFilterRequest request)
        {
            var response = new CustomerFactoryDashboard();

            try
            {
                //Fetch the Supplier details
                var supData = await _supManager.GetSupplierHeadOfficeAddress(request.FactoryId.Value);

                //Fetch the booking details
                var bookingDetails = await GetBookingDetails(request);

                //Get only the booking Id
                var bookingIds = bookingDetails.Where(x => x.FactoryId == request.FactoryId).Select(x => x.InspectionId).Distinct().ToList();

                //Get the Fb Report Ids for the bookings
                var fbReportData = await _repo.GetReportData(bookingIds);
                var fbReportIds = fbReportData.Where(x => x.FbReportId.HasValue).Select(x => x.FbReportId.GetValueOrDefault()).Distinct().ToList();

                //Fetch data from Fb_report_insp_summary for the failed report Ids
                var fbFailReasons = await _repo.GetFBInspSummaryResultbyReport(fbReportIds);

                //Group by the failed reasons to fetch the top 5 with the percentage
                var reportFailReasonList = fbFailReasons.GroupBy(p => p.Name, (key, _data) =>
                new DefectData
                {
                    DefectName = key,
                    DefectPercentage = Math.Round(((double)_data.Count() / (double)fbFailReasons.Count() * 100), 2),
                }).OrderByDescending(x => x.DefectPercentage).Take(FetchTop5).ToList();

                response = _dashboardmap.MapFactoryDefects(bookingIds, fbReportData, supData, reportFailReasonList);

                return response;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Customer dashboard data for mobile app
        public async Task<InspDashboardMobileResponse> GetMobileCustomerDashboard(InspDashboardMobileRequest request)
        {
            var response = new InspDashboardMobileResponse();
            var result = new DashBoardGraph();
            var data = new MobileDashboardResponse();

            try
            {
                var dashboardRequest = RequestMobileMap.MapDashboardRequest(request);

                //get the bookings
                data.bookingData = await GetBookingDetails(dashboardRequest);

                //fetch the booking Ids
                var inspectionIds = data.bookingData.Select(x => x.InspectionId).Distinct();

                //Get the man day data
                data.manDayData = await GetInspectionManDayOverview(dashboardRequest);

                //API Result Analysis - Fb report status count
                data.apiData = await GetAPIRADashboard(dashboardRequest);

                //get the rejection data from Fb_inspSummary 
                data.rejectData = await GetInspectionRejectDashBoard(dashboardRequest);

                //set the request dates to yesterday and 2 days after tomorrow to display booking count for yesterday, today and the coming 3 days
                var currentDateRequest = dashboardRequest;
                currentDateRequest.ServiceDateFrom = Static_Data_Common.GetCustomDate(DateTime.Now.AddDays(-1));
                currentDateRequest.ServiceDateTo = Static_Data_Common.GetCustomDate(DateTime.Now.AddDays(3));

                data.currentBookingData = await GetBookingDetails(currentDateRequest);

                //Get the quotations pending for customer validation
                var quotationCount = await GetQuotationTasks(request.customerId.GetValueOrDefault());
                data.pendingQuotationCount = quotationCount.PendingQuotations;

                result = CustomerDashboardMobileMap.InspDashboardMobileMap(data);

                var taskData = await _userRightsManager.GetTaskList();
                result.taskCount = taskData?.Data?.Where(x => x.Type == TaskType.QuotationCustomerConfirmed).Count() ?? 0;

                response.data = result;

                response.meta = new MobileResult { success = true, message = "" };
            }
            catch (Exception e)
            {
                response.meta = new MobileResult { success = false, message = "Customer Dashboard fetch failed." };
            }
            return response;
        }

        //get the data for the man day year chart
        public async Task<MandayYearChartResponse> GetMandayYearChart(CustomerDashboardFilterRequest request)
        {
            var response = new MandayYearChartResponse();
            var mandayYear = new List<MandayYear>();
            var data = new List<MandayYearChartItem>();
            try
            {
                request.StatusIdList = InspectedStatusList.Select(x => x);

                var bookingData = await GetBookingDetails(request);

                var bookingIdList = bookingData.Select(x => x.InspectionId);

                data = await _repo.GetMonthlyInspManDays(bookingIdList);

                if (data == null || !data.Any())
                {
                    response.Result = MandayDashboardResult.NotFound;
                    return response;
                }

                //Random r = new Random();
                var index = 1;
                //get the per year man day count along with month wise data
                var items = data.GroupBy(p => p.Year, (key, _data) =>
                new MandayYearChart
                {
                    Year = key,
                    MandayCount = _data.Where(x => x.Year == key).Sum(x => x.MonthManDay),
                    MonthlyData = _data.Where(x => x.Year == key).ToList(),
                    Color = MandayDashboardColorList.GetValueOrDefault(index++)
                }).OrderByDescending(x => x.MandayCount).Take(5);

                response.Data = items;
                //get the x axis month data for the chart
                for (int i = 0; i < 12; i++)
                {
                    MandayYear res = new MandayYear();
                    res.year = DateTime.Now.Year;
                    res.month = i;
                    mandayYear.Add(res);
                }
                response.MonthYearXAxis = mandayYear;
                response.Result = MandayDashboardResult.Success;
            }

            catch (Exception ex)
            {
                response.Result = MandayDashboardResult.Fail;
            }
            if (response.Data == null || !response.Data.Any())
            {
                response.Result = MandayDashboardResult.NotFound;
            }
            return response;
        }

        //get the pending customer validation quotations for the dashboard in mobile app
        public async Task<MobileTaskResponse> GetMobiletTaskList()
        {
            var response = new MobileTaskResponse();

            try
            {
                var res = await _userRightsManager.GetTaskList();

                response.data = CustomerDashboardMobileMap.MapTask(res.Data);
                response.meta = new MobileResult { success = true, message = "" };
            }
            catch (Exception e)
            {
                response.meta = new MobileResult { success = false, message = "Task list fetch failed" };
            }
            return response;
        }

        /// <summary>
        /// get factory address details
        /// </summary>
        /// <param name="factoryIds"></param>
        /// <returns></returns>
        public IQueryable<SuAddress> GetFactoryAddressById(IEnumerable<int> factoryIds)
        {
            return _repo.GetFactoryAddressById(factoryIds);
        }

        /// <summary>
        /// get customer decision count
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public async Task<CustomerDecisionCount> GetCustomerDecisionCount(int customerId)
        {
            CustomerDecisionCount customerDecisionCount = new CustomerDecisionCount();
            //if the user has edit inspection customer decision role then take the customer decision
            if (_ApplicationContext.RoleList.Contains((int)RoleEnum.EditInspectionCustomerDecision))
            {
                //get the Iqueryable inspection data
                var data = _sharedInspectionManager.GetAllInspectionQuery();

                //last thirty days inspection data
                data = data.Where(x => InspectedStatusList.Contains(x.StatusId) && x.FbReportDetails.Any(y => y.Active.Value && !y.InspRepCusDecisions.Any(z => z.Active.Value))
                        && (!((x.ServiceDateFrom > DateTime.Now.Date) || (x.ServiceDateTo < DateTime.Now.Date.AddDays(LastThirtyDays)))));

                if (customerId > 0)
                {
                    data = data.Where(x => x.CustomerId == customerId);
                }

                customerDecisionCount.PendingDecisionCount = await data.CountAsync();
            }
            return customerDecisionCount;
        }
    }
}
