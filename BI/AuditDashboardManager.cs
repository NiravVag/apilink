using BI.Utilities;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Audit;
using DTO.AuditDashboard;
using DTO.Common;
using DTO.RepoRequest.Audit;
using DTO.RepoRequest.Enum;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BI
{
    public class AuditDashboardManager : ApiCommonData, IAuditDashboardManager
    {
        private readonly IAuditDashboardRepository _auditDashboardRepository = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly IAuditRepository _auditRepository = null;
        private readonly IOfficeLocationManager _office = null;
        private readonly IHelper _helper = null;
        private readonly ILocationRepository _locationRepo = null;
        private readonly IConfiguration _configuration = null;
        private readonly ICustomerManager _cusManager = null;
        private readonly ISupplierManager _supManager = null;
        public AuditDashboardManager(IAuditDashboardRepository auditDashboardRepository, IAPIUserContext ApplicationContext, IAuditRepository auditRepository,
            IOfficeLocationManager office, IHelper helper, ILocationRepository locationRepo, IConfiguration configuration, ICustomerManager cusManager,
            ISupplierManager supManager)
        {
            _auditDashboardRepository = auditDashboardRepository;
            _ApplicationContext = ApplicationContext;
            _auditRepository = auditRepository;
            _office = office;
            _helper = helper;
            _locationRepo = locationRepo;
            _configuration = configuration;
            _cusManager = cusManager;
            _supManager = supManager;
        }
        public async Task<AuditMapGeoLocation> GetAuditCountryGeoCode(AuditDashboardMapFilterRequest request)
        {
            var response = new AuditMapGeoLocation();
            var auditQuery = _auditRepository.GetAuditMainData();
            var auditQueryRequest = GetAuditDashboardRequestMap(request);
            var audits = GetAuditQuerywithRequestFilters(auditQueryRequest, auditQuery);
            var auditIdlst = audits.Select(x => x.Id);
            var lstgeocode = await _auditDashboardRepository.GetAuditCountryGeoCode(auditIdlst);
            var grpbygeocode = lstgeocode.GroupBy(p => p.FactoryCountryId, (key, _data) => new AuditCountryGeoCode
            {
                FactoryCountryId = key,
                FactoryCountryName = _data.FirstOrDefault(x => x.FactoryCountryId == key)?.FactoryCountryName,
                FactoryCountryCode = _data.FirstOrDefault(x => x.FactoryCountryId == key)?.FactoryCountryCode,
                Latitude = _data.FirstOrDefault(x => x.FactoryCountryId == key)?.Latitude,
                Longitude = _data.FirstOrDefault(x => x.FactoryCountryId == key)?.Longitude,
                TotalCount = _data.Count()
            }).ToList();

            //Check geo country coordinates are null then need to update coordinates from MAPBOX API
            if (grpbygeocode != null && grpbygeocode.Any())
            {
                response.CountryGeoCodeResult = MapGeoLocationResult.Success;
                grpbygeocode = await UpdateMapBoxCountryGeoCode(grpbygeocode, response);
            }

            var grpByProvince = lstgeocode.GroupBy(p => p.FactoryProvinceId, (key, _data) => new AuditProvinceGeoCode
            {
                FactoryProvinceId = key,
                FactoryProvinceName = _data.FirstOrDefault(x => x.FactoryProvinceId == key)?.FactoryProvinceName,
                Latitude = _data.FirstOrDefault(x => x.FactoryProvinceId == key)?.ProvinceLatitude,
                Longitude = _data.FirstOrDefault(x => x.FactoryProvinceId == key)?.ProvinceLongitude,
                TotalCount = _data.Count()
            }).ToList();

            //Check geo Province coordinates are null then need to update coordinates from MAPBOX API
            if (grpByProvince != null && grpByProvince.Count > 0)
            {
                response.ProvinceGeoCodeResult = MapGeoLocationResult.Success;
                grpByProvince = await UpdateMapBoxProvinceGeoCode(grpByProvince, response);
            }

            var grpByFactory = lstgeocode.GroupBy(p => p.FactoryId, (key, _data) => new AuditFactoryGeoCode
            {
                FactoryId = key,
                FactoryName = _data.FirstOrDefault(x => x.FactoryId == key)?.FactoryName,
                Latitude = _data.FirstOrDefault(x => x.FactoryId == key)?.FactoryLatitude,
                Longitude = _data.FirstOrDefault(x => x.FactoryId == key)?.FactoryLongitude,
                TotalCount = _data.Count()
            }).ToList();

            response.CountryGeoCode = grpbygeocode;
            response.ProvinceGeoCode = grpByProvince.Where(x => x.Latitude.HasValue && x.Longitude.HasValue).ToList();
            response.FactoryGeoCode = grpByFactory.Where(x => x.Latitude.HasValue && x.Longitude.HasValue).ToList();
            return response;
        }

        public async Task<AuditDashboardResponse> GetAuditDashboardSummary(AuditDashboardMapFilterRequest request)
        {
            if (request == null)
                return new AuditDashboardResponse { Result = AuditDashboardResult.RequestError };

            var response = new AuditDashboardResponse();
            var result = new AuditDashboardItem();
            var auditQuery = _auditRepository.GetAuditMainData();
            var auditQueryRequest = GetAuditDashboardRequestMap(request);
            var audits = GetAuditQuerywithRequestFilters(auditQueryRequest, auditQuery);
            var auditData = await audits.Select(x => new AuditRepoItem()
            {
                AuditId = x.Id,
                CustomerName = x.Customer.CustomerName,
                FactoryName = x.Factory.SupplierName,
                PoNumber = x.PoNumber,
                ReportNo = x.ReportNo,
                ServiceDateFrom = x.ServiceDateFrom,
                ServiceDateTo = x.ServiceDateTo,
                SupplierName = x.Supplier.SupplierName,
                Office = x.Office.LocationName,
                StatusId = x.StatusId,
                BookingCreatedBy = x.CreatedByNavigation.UserTypeId,
                CustomerBookingNo = x.CustomerBookingNo,
            }).AsNoTracking().ToListAsync();

            if (!auditData.Any())
                return new AuditDashboardResponse { Result = AuditDashboardResult.NotFound };

            result.TotalAuditInProgressCount = auditData.Count(x => x.StatusId == (int)Entities.Enums.AuditStatus.Received || x.StatusId == (int)Entities.Enums.AuditStatus.Confirmed);
            result.TotalAuditPlanningCount = auditData.Count(x => x.StatusId == (int)Entities.Enums.AuditStatus.Scheduled);
            result.TotalAuditedCount = auditData.Count(x => x.StatusId == (int)Entities.Enums.AuditStatus.Audited);
            result.TotalFactoryCount = auditData.Select(x => x.FactoryName).Distinct().Count();
            response.Data = result;
            response.Result = AuditDashboardResult.Success; 
            return response;
        }

        public async Task<ResultAnalyticsAuditDashboardResponse> GetServiceTypeAuditDashboard(AuditDashboardMapFilterRequest request)
        {
            if (request == null)
                return new ResultAnalyticsAuditDashboardResponse { Result = AuditDashboardResult.RequestError };

            var result = new List<CustomerAPIRAAuditDashboard>();

            var auditQuery = _auditRepository.GetAuditMainData();
            var auditQueryRequest = GetAuditDashboardRequestMap(request);
            var auditData = GetAuditQuerywithRequestFilters(auditQueryRequest, auditQuery);

            var auditIds = auditData.Select(x => x.Id);

            var auditServiceData = await _auditDashboardRepository.GetAuditServiceTypeByQuery(auditIds);
            for (int i = 0; i < auditServiceData.Count; i++)
            {
                var item = auditServiceData[i];
                result.Add(new CustomerAPIRAAuditDashboard
                {
                    StatusName = item.Name,
                    TotalCount = item.Count,
                    StatusColor = CommonDashboardColor.GetValueOrDefault(i + 1)
                });
            }

            if (!result.Any())
                return new ResultAnalyticsAuditDashboardResponse { Result = AuditDashboardResult.NotFound };

            return new ResultAnalyticsAuditDashboardResponse
            {
                Data = result,
                Result = AuditDashboardResult.Success
            };
        }

        public async Task<ResultAnalyticsAuditDashboardResponse> GetAuditTypeAuditDashboard(AuditDashboardMapFilterRequest request)
        {
            if (request == null)
                return new ResultAnalyticsAuditDashboardResponse { Result = AuditDashboardResult.RequestError };

            var result = new List<CustomerAPIRAAuditDashboard>();

            var auditQuery = _auditRepository.GetAuditMainData();
            var auditQueryRequest = GetAuditDashboardRequestMap(request);
            var auditData = GetAuditQuerywithRequestFilters(auditQueryRequest, auditQuery);

            var auditTypelst = await auditData
                  .Select(x => new
                  {
                      x.Id,
                      x.AuditTypeId,
                      x.AuditType.Name
                  }).GroupBy(p => new { p.AuditTypeId, p.Name }, (key, _data) => new AuditChartData
                  {
                      Id = key.AuditTypeId,
                      Name = key.Name,
                      Count = _data.Count(),
                  }).AsNoTracking().ToListAsync();

            for (int i = 0; i < auditTypelst.Count; i++)
            {
                var item = auditTypelst[i];
                result.Add(new CustomerAPIRAAuditDashboard
                {
                    StatusName = item.Name,
                    TotalCount = item.Count,
                    StatusColor = CommonDashboardColor.GetValueOrDefault(i + 1)
                });
            }

            if (!result.Any())
                return new ResultAnalyticsAuditDashboardResponse { Result = AuditDashboardResult.NotFound };

            return new ResultAnalyticsAuditDashboardResponse
            {
                Data = result,
                Result = AuditDashboardResult.Success
            };
        }
        public async Task<AuditDashboardChartExport> ExportAuditTypeChart(AuditDashboardMapFilterRequest request)
        {
            if (request == null)
                return new AuditDashboardChartExport { Result = AuditDashboardResult.RequestError };

            var response = new AuditDashboardChartExport();

            var auditQuery = _auditRepository.GetAuditMainData();
            var auditQueryRequest = GetAuditDashboardRequestMap(request);
            var auditData = GetAuditQuerywithRequestFilters(auditQueryRequest, auditQuery);

            var auditTypelst = await auditData
                  .Select(x => new
                  {
                      x.Id,
                      x.AuditTypeId,
                      x.AuditType.Name
                  }).GroupBy(p => new { p.AuditTypeId, p.Name }, (key, _data) => new AuditChartData
                  {
                      Id = key.AuditTypeId,
                      Name = key.Name,
                      Count = _data.Count(),
                  }).AsNoTracking().ToListAsync();

            if (auditTypelst != null && auditTypelst.Any())
            {
                response.Data = auditTypelst.Select(x => new AuditDashboardExportItem
                {
                    Count = x.Count,
                    Name = x.Name
                }).ToList();

                response.Total = auditTypelst.Sum(x => x.Count);
            }
            //get the request details to be input in the excel
            response.RequestFilters = await SetExportFilter(request);

            return response;
        }

        public async Task<AuditDashboardChartExport> ExportServiceTypeChart(AuditDashboardMapFilterRequest request)
        {
            if (request == null)
                return new AuditDashboardChartExport { Result = AuditDashboardResult.RequestError };

            var response = new AuditDashboardChartExport();

            var auditQuery = _auditRepository.GetAuditMainData();
            var auditQueryRequest = GetAuditDashboardRequestMap(request);
            var auditData = GetAuditQuerywithRequestFilters(auditQueryRequest, auditQuery);

            var auditIds = auditData.Select(x => x.Id);

            //fetch the service type data
            var serviceType = await _auditDashboardRepository.GetAuditServiceTypeByQuery(auditIds);

            if (serviceType != null && serviceType.Any())
            {
                response.Data = serviceType.Select(x => new AuditDashboardExportItem
                {
                    Count = x.Count,
                    Name = x.Name
                }).ToList();

                response.Total = serviceType.Sum(x => x.Count);
            }
            //get the request details to be input in the excel
            response.RequestFilters = await SetExportFilter(request);

            return response;
        }
        public async Task<OverviewAuditDashboardResponse> OverviewDashboardSearch(AuditDashboardMapFilterRequest request)
        {
            if (request == null)
                return new OverviewAuditDashboardResponse { Result = AuditDashboardResult.RequestError };

            var response = new OverviewAuditDashboardResponse();
            var result = new OverviewAuditChart();

            var auditQuery = _auditRepository.GetAuditMainData();
            var auditQueryRequest = GetAuditDashboardRequestMap(request);
            var auditData = GetAuditQuerywithRequestFilters(auditQueryRequest, auditQuery);
            var auditlist = await auditData.ToListAsync();

            if (!auditlist.Any())
                return new OverviewAuditDashboardResponse { Result = AuditDashboardResult.NotFound };

            result.TotalBookingCount = auditlist.Count;
            result.TotalCustomerCount = auditlist.Select(x => x.CustomerId).Distinct().Count();
            result.TotalReports = auditlist.Select(x => x.ReportNo).Distinct().Count();
            response.Data = result;
            response.Result = AuditDashboardResult.Success; 
            return response;
        }

        private async Task<List<AuditProvinceGeoCode>> UpdateMapBoxProvinceGeoCode(List<AuditProvinceGeoCode> grpByProvince, AuditMapGeoLocation response)
        {
            var emptyGeoCodeLst = grpByProvince.Where(x => x.Longitude == null || x.Latitude == null).ToList();
            var emptyGeoIds = grpByProvince.Where(x => x.Longitude == null || x.Latitude == null).Select(x => x.FactoryProvinceId).ToList();
            if (emptyGeoCodeLst != null && emptyGeoCodeLst.Any())
            {
                grpByProvince = await GetMapBoxProvinceGeoCode(grpByProvince, response);//call MAPBOX API
                if (grpByProvince != null && grpByProvince.Any())
                {
                    var provinceLst = await _locationRepo.GetProvinceByIds(emptyGeoIds);

                    foreach (var item in grpByProvince.Where(x => emptyGeoIds.Contains(x.FactoryProvinceId)))
                    {
                        var country = provinceLst.FirstOrDefault(x => x.Id == item.FactoryProvinceId);
                        if(country != null)
                        {
                            country.Longitude = item.Longitude;
                            country.Latitude = item.Latitude;
                        }
                        
                    }

                    _auditDashboardRepository.EditEntities(provinceLst);
                    await _auditDashboardRepository.Save();
                }
            }
            return grpByProvince;
        }

        private async Task<List<AuditProvinceGeoCode>> GetMapBoxProvinceGeoCode(List<AuditProvinceGeoCode> provinceGeoCode, AuditMapGeoLocation response)
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

        private async Task<List<AuditCountryGeoCode>> UpdateMapBoxCountryGeoCode(List<AuditCountryGeoCode> grpbygeocode, AuditMapGeoLocation response)
        {
            var emptyGeoCodeLst = grpbygeocode.Where(x => x.Longitude == null || x.Latitude == null).ToList();
            var emptyGeoIds = grpbygeocode.Where(x => x.Longitude == null || x.Latitude == null).Select(x => x.FactoryCountryId).ToList();
            if (emptyGeoCodeLst != null && emptyGeoCodeLst.Any())
            {
                var shortCountryCodes = string.Join(",", emptyGeoCodeLst.Select(x => x.FactoryCountryCode));
                grpbygeocode = await GetMapBoxCountryGeoCode(grpbygeocode, shortCountryCodes, response);//call MAPBOX API

                var countryLst = await _locationRepo.GetCountriesByIds(emptyGeoIds);

                foreach (var item in grpbygeocode.Where(x => emptyGeoIds.Contains(x.FactoryCountryId)))
                {
                    var country = countryLst.FirstOrDefault(x => x.Id == item.FactoryCountryId);
                    if(country != null)
                    {
                        country.Longitude = item.Longitude;
                        country.Latitude = item.Latitude;
                    }
                }

                _auditDashboardRepository.EditEntities(countryLst);
                await _auditDashboardRepository.Save();
            }
            return grpbygeocode;
        }

        private async Task<List<AuditCountryGeoCode>> GetMapBoxCountryGeoCode(List<AuditCountryGeoCode> countryGeoCode, string shortCountryCodes, AuditMapGeoLocation response)
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
                                    if (item["properties"]["short_code"].ToString().ToLower() == geoCode.FactoryCountryCode.ToLower() && item["geometry"] != null && item["geometry"]["coordinates"] != null)
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

        private IQueryable<AudTransaction> GetAuditQuerywithRequestFilters(AuditSummaryRepoRequest request, IQueryable<AudTransaction> auditQuery)
        {
            auditQuery = auditQuery.Where(x => x.StatusId != (int)Entities.Enums.AuditStatus.Cancel);
            if (request != null && request.Customerlst != null && request.Customerlst.Any())
            {
                auditQuery = auditQuery.Where(x => request.Customerlst.Contains(x.CustomerId));
            }

            if (request != null && request.SupplierId > 0)
            {
                auditQuery = auditQuery.Where(x => x.SupplierId == request.SupplierId);
            }

            if (request != null && request.FactoryIdlst != null && request.FactoryIdlst.Any())
            {
                auditQuery = auditQuery.Where(x => request.FactoryIdlst.ToList().Contains(x.FactoryId));
            }

            if (request != null && request.OfficeIdlst != null && request.OfficeIdlst.Any())
            {
                auditQuery = auditQuery.Where(x => request.OfficeIdlst.ToList().Contains(x.OfficeId.Value));
            }

            if (request != null && request.Statuslst != null && request.Statuslst.Any())
            {
                auditQuery = auditQuery.Where(x => request.Statuslst.ToList().Contains(x.StatusId));
            }

            //apply factory country filter
            if (request.FactoryCountryIdList != null && request.FactoryCountryIdList.Any())
            {
                auditQuery = auditQuery.Where(x => x.Factory.SuAddresses.Any(y => request.FactoryCountryIdList.Contains(y.CountryId)));
            }

            //apply auditor(staff) filter
            if (request.AuditorIdList != null && request.AuditorIdList.Any())
            {
                auditQuery = auditQuery.Where(x => x.AudTranAuditors.Any(y => request.AuditorIdList.Contains(y.StaffId)));
            }

            //apply servicetype filter
            if (request.ServiceTypeIdList != null && request.ServiceTypeIdList.Any())
            {
                auditQuery = auditQuery.Where(x => x.AudTranServiceTypes.Any(y => y.Active && request.ServiceTypeIdList.Contains(y.ServiceTypeId)));
            }

            if (Enum.TryParse(request.SearchTypeId.ToString(), out SearchType _seachtypeenum))
            {
                switch (_seachtypeenum)
                {
                    case SearchType.BookingNo:
                        {
                            if (!string.IsNullOrEmpty(request.SearchText) && int.TryParse(request.SearchText, out int bookid))
                            {
                                auditQuery = auditQuery.Where(x => x.Id == bookid);
                            }
                            break;
                        }
                    case SearchType.ReportNo:
                        {
                            if (!string.IsNullOrEmpty(request.SearchText))
                            {
                                auditQuery = auditQuery.Where(x => x.ReportNo.Contains(request.SearchText));
                            }
                            break;
                        }
                    case SearchType.PoNo:
                        {
                            if (!string.IsNullOrEmpty(request.SearchText))
                            {
                                auditQuery = auditQuery.Where(x => x.PoNumber.Contains(request.SearchText));
                            }
                            break;
                        }
                    case SearchType.CustomerBookingNo:
                        {
                            if (!string.IsNullOrEmpty(request.SearchText))
                            {
                                auditQuery = auditQuery.Where(x => x.CustomerBookingNo.Contains(request.SearchText));
                            }
                            break;
                        }
                }
                if (Enum.TryParse(request.DatetypeId.ToString(), out SearchType _datesearchtype))
                {
                    switch (_datesearchtype)
                    {
                        case SearchType.ApplyDate:
                            {
                                if (request.Fromdate != null && request.Todate != null)
                                {
                                    auditQuery = auditQuery.Where(x => EF.Functions.DateDiffDay(request.Fromdate, x.CreatedOn) >= 0 &&
                                                    EF.Functions.DateDiffDay(x.CreatedOn, request.Todate) >= 0);
                                }
                                break;
                            }
                        case SearchType.ServiceDate:
                            {
                                if (request.Fromdate != null && request.Todate != null)
                                {
                                    auditQuery = auditQuery.Where(x => !((x.ServiceDateFrom > request.Todate.Value) || (x.ServiceDateTo < request.Fromdate.Value)));
                                }
                                break;
                            }
                    }
                }
            }
            return auditQuery;
        }

        private AuditSummaryRepoRequest GetAuditDashboardRequestMap(AuditDashboardMapFilterRequest request)
        {
            //filter data based on user type
            switch (_ApplicationContext.UserType)
            {
                case UserTypeEnum.Customer:
                    {
                        request.CustomerId = request?.CustomerId > 0 ? request?.CustomerId : _ApplicationContext.CustomerId;
                        break;
                    }
                case UserTypeEnum.Factory:
                    {
                        request.FactoryIdlst = request.FactoryIdlst != null && request.FactoryIdlst.Any() ? request.FactoryIdlst : new List<int>().Append(_ApplicationContext.FactoryId);
                        break;
                    }
                case UserTypeEnum.Supplier:
                    {
                        request.SupplierId = request.SupplierId > 0 ? request.SupplierId.Value : _ApplicationContext.SupplierId;
                        break;
                    }
            }
            var cuslist = new List<int>();
            if (_ApplicationContext.UserType == UserTypeEnum.InternalUser)
            {
                if (request.Officeidlst != null && !request.Officeidlst.Any())
                {
                    var _cusofficelist = _office.GetOfficesByUserId(_ApplicationContext.StaffId);
                    request.Officeidlst = _cusofficelist == null || !_cusofficelist.Any() ? request.Officeidlst : _cusofficelist.Select(x => (int?)x.Id);
                }
                if (request?.CustomerId != null)
                    cuslist.Add(request.CustomerId.Value);
            }
            else
            {
                if (request?.CustomerId != null)
                    cuslist.Add(request.CustomerId.Value);
            }
            return new AuditSummaryRepoRequest()
            {
                Customerlst = cuslist,
                SupplierId = request.SupplierId ?? 0,
                DatetypeId = request.DateTypeid,
                Fromdate = request.FromDate?.ToDateTime(),
                Todate = request.ToDate?.ToDateTime(),
                SearchText = request.SearchTypeText?.Trim(),
                SearchTypeId = request.SearchTypeId,
                FactoryIdlst = request.FactoryIdlst,
                Statuslst = request.StatusIdlst,
                OfficeIdlst = request.Officeidlst,
                AuditorIdList = request.AuditorIdList,
                FactoryCountryIdList = request.FactoryCountryIdList,
                ServiceTypeIdList = request.ServiceTypeIdList
            };
        }

        private async Task<AuditDashboardRequestExport> SetExportFilter(AuditDashboardMapFilterRequest request)
        {
            var response = new AuditDashboardRequestExport();

            response.ServiceDateFrom = request.FromDate == null ? "" : request.FromDate.ToDateTime().ToString(StandardDateFormat);
            response.ServiceDateTo = request.ToDate == null ? "" : request.ToDate.ToDateTime().ToString(StandardDateFormat);

            //get the customer name
            if (request.CustomerId > 0)
            {
                var customers = await _cusManager.GetCustomerByCustomerId(request.CustomerId.GetValueOrDefault());
                response.Customer = customers.DataSourceList != null && customers.DataSourceList.Any() ? string.Join(", ", customers.DataSourceList.Select(x => x.Name)) : "";
            }

            //get the country name
            if (request.FactoryCountryIdList != null && request.FactoryCountryIdList.Any())
            {
                var countryList = await _locationRepo.GetCountryByIds(request.FactoryCountryIdList.ToList());
                response.CountryList = countryList != null && countryList.Any() ? string.Join(", ", countryList.Select(x => x.Name)) : "";
            }

            //get the supplier name
            if (request.SupplierId > 0)
            {
                var supIdList = new[] { request.SupplierId.GetValueOrDefault() }.ToList();
                var supList = await _supManager.GetSupplierById(supIdList);
                response.Supplier = supList.DataSourceList != null && supList.DataSourceList.Any() ? string.Join(", ", supList.DataSourceList.Select(x => x.Name)) : "";
            }

            //get the factory names
            if (request.FactoryIdlst != null && request.FactoryIdlst.Any())
            {
                var factList = await _supManager.GetSupplierById(request.FactoryIdlst.ToList());
                response.FactoryList = factList.DataSourceList != null && factList.DataSourceList.Any() ? string.Join(", ", factList.DataSourceList.Select(x => x.Name)) : "";
            }

            return response;
        }
    }
}
