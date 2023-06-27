using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.Dashboard;
using DTO.Inspection;
using DTO.Manday;
using DTO.RepoRequest.Enum;
using DTO.SharedInspection;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DTO.Common.Static_Data_Common;

namespace BI
{
    public class MandayManager : ApiCommonData, IMandayManager
    {
        private readonly IMandayRepository _repo = null;
        private readonly IInspectionBookingRepository _inspRepo = null;
        private readonly IDashboardRepository _dashboardRepo = null;
        private readonly ISupplierRepository _supRepo = null;
        private readonly IOfficeLocationRepository _officeRepo = null;
        private readonly ILocationRepository _locationRepo = null;
        private readonly ICustomerRepository _cusRepo = null;
        private readonly IQuotationRepository _quotationRepository = null;
        private readonly IReferenceManager _referenceManager = null;
        private readonly ISharedInspectionManager _sharedInspectionManager = null;

        public MandayManager(IMandayRepository repo, IInspectionBookingRepository inspRepo, IDashboardRepository dashboardRepo, ISupplierRepository supRepo,
            IOfficeLocationRepository officeRepo, ILocationRepository locationRepo, ICustomerRepository cusRepo,
            IQuotationRepository quotationRepository, IReferenceManager referenceManager, ISharedInspectionManager sharedInspectionManager)
        {
            _repo = repo;
            _inspRepo = inspRepo;
            _dashboardRepo = dashboardRepo;
            _supRepo = supRepo;
            _officeRepo = officeRepo;
            _locationRepo = locationRepo;
            _cusRepo = cusRepo;
            _quotationRepository = quotationRepository;
            _referenceManager = referenceManager;
            _sharedInspectionManager = sharedInspectionManager;
        }

        //get the data for the top level filters in manday dashboard
        public async Task<MandayDashboardResponse> GetMandayDashboardSearch(MandayDashboardRequest request)
        {
            var response = new MandayDashboardResponse();
            var result = new MandayDashboardItem();

            if (request != null)
            {
                try
                {
                    request.ChartType = MandayDashboardChartType.Other;
                    if (request.ServiceId == (int)Service.InspectionId)
                    {
                        //fetch the booking data based on request filters
                        var bookingData = CommonInspSearch(request);

                        //fetch the booking Ids
                        var bookingIds = bookingData.Select(x => x.Id);

                        //if (bookingIds.Any())
                        //{
                        IQueryable<InspectionMandayDashboard> mandaydata = null;
                        if (request.MandayType == (int)MandayType.EstimatedManday)
                        {
                            mandaydata = _dashboardRepo.GetInspectionManDays(bookingIds).AsNoTracking();
                        }
                        else if (request.MandayType == (int)MandayType.ActualManday)
                        {
                            mandaydata = _dashboardRepo.GetInspectionActualCount(bookingIds);
                        }
                        var inspectionIds = mandaydata.Select(x => x.InspectionId).Distinct();
                        result.ComparedTotalManday = !request.IsCompareData ? 0 : Math.Round(mandaydata.Where(x => x.ServiceDateTo <= request.ComparedServiceDateTo.ToDateTime() && x.ServiceDateTo >= request.ComparedServiceDateFrom.ToDateTime()).Sum(y => request.MandayType == (int)MandayType.EstimatedManday ? y.MandayCount : y.ActualMandayCount) ?? 0, 1);
                        //get the total manday for the booking list
                        result.TotalManday = Math.Round(mandaydata.Where(x => x.ServiceDateTo <= request.ServiceDateTo.ToDateTime() && x.ServiceDateTo >= request.ServiceDateFrom.ToDateTime()).Sum(y => request.MandayType == (int)MandayType.EstimatedManday ? y.MandayCount : y.ActualMandayCount) ?? 0, 1);

                        #region
                        //get Product list for bookin Ids
                        var POProducts = await _dashboardRepo.GetPOProducts(inspectionIds, InspectedStatusList);

                        if (request.IsCompareData)
                        {
                            var comparedPOProducts = POProducts.Where(x => x.ServiceDateTo <= request.ComparedServiceDateTo.ToDateTime() && x.ServiceDateTo >= request.ComparedServiceDateFrom.ToDateTime());
                            result.ComparedTotalReportCount = comparedPOProducts.Sum(x => x.ProductFbReportCount) + comparedPOProducts.Sum(x => x.ContainerFbReportCount);
                        }
                        var servicePOProducts = POProducts.Where(x => x.ServiceDateTo <= request.ServiceDateTo.ToDateTime() && x.ServiceDateTo >= request.ServiceDateFrom.ToDateTime());
                        result.TotalReportCount = servicePOProducts.Sum(x => x.ProductFbReportCount) + servicePOProducts.Sum(x => x.ContainerFbReportCount);

                        #endregion;
                        result.ComparedTotalCount = !request.IsCompareData ? 0 : await bookingData.Where(x => x.ServiceDateTo <= request.ComparedServiceDateTo.ToDateTime() && x.ServiceDateTo >= request.ComparedServiceDateFrom.ToDateTime()).Select(x => x.Id).Distinct().CountAsync();

                        result.TotalCount = await bookingData.Where(x => x.ServiceDateTo <= request.ServiceDateTo.ToDateTime() && x.ServiceDateTo >= request.ServiceDateFrom.ToDateTime() && inspectionIds.Contains(x.Id)).Select(x => x.Id).Distinct().CountAsync();

                        response.Data = result;
                        // }

                        response.Result = MandayDashboardResult.Success;
                    }

                    else if (request.ServiceId == (int)Service.AuditId)
                    {
                        //fetch the audit data based on request filters
                        var auditData = await CommonAuditSearch(request);
                        //select audit Ids
                        var auditIds = auditData.AsNoTracking().Select(x => x.AuditId);

                        if (auditIds.Any())
                        {
                            //get the total manday for the audit list
                            var auditMandays = await _repo.GetAuditManDays(auditIds);
                            result.TotalManday = auditMandays.Where(x => x.ServiceDateTo <= request.ServiceDateTo.ToDateTime() && x.ServiceDateTo >= request.ServiceDateFrom.ToDateTime()).Sum(y => y.MandayCount) ?? 0;
                            result.ComparedTotalManday = !request.IsCompareData ? 0 : auditMandays.Where(x => x.ServiceDateTo <= request.ComparedServiceDateTo.ToDateTime() && x.ServiceDateTo >= request.ComparedServiceDateFrom.ToDateTime()).Sum(y => y.MandayCount) ?? 0;

                            result.TotalCount = auditData.Count(x => x.ServiceDateTo <= request.ServiceDateTo.ToDateTime() && x.ServiceDateTo >= request.ServiceDateFrom.ToDateTime());
                            result.ComparedTotalCount = !request.IsCompareData ? 0 : auditData.Count(x => x.ServiceDateTo <= request.ComparedServiceDateTo.ToDateTime() && x.ServiceDateTo >= request.ComparedServiceDateFrom.ToDateTime());

                            result.TotalReportCount = result.TotalCount;
                            result.ComparedTotalReportCount = result.ComparedTotalCount;
                        }
                        response.Data = result;
                        response.Result = MandayDashboardResult.Success;
                    }
                }
                catch (Exception ex)
                {
                    response.Result = MandayDashboardResult.Fail;
                }
            }

            if (response.Data == null)
            {
                response.Result = MandayDashboardResult.NotFound;
            }

            return response;
        }

        //fetch the services
        public async Task<DataSourceResponse> GetServices()
        {
            var response = new DataSourceResponse();
            response.DataSourceList = await _repo.GetServices();

            if (response.DataSourceList == null || !response.DataSourceList.Any())
                response.Result = DataSourceResult.CannotGetList;

            response.Result = DataSourceResult.Success;
            return response;

        }

        //get all the office locations
        public async Task<DataSourceResponse> GetOfficeLocations()
        {
            var response = new DataSourceResponse();
            response.DataSourceList = await _repo.GetOfficeLocations();

            if (response.DataSourceList == null || !response.DataSourceList.Any())
                response.Result = DataSourceResult.CannotGetList;

            response.Result = DataSourceResult.Success;
            return response;

        }

        //get the bookings
        private IQueryable<InspTransaction> CommonInspSearch(MandayDashboardRequest request)
        {
            var sharedInspectionModel = new SharedInspectionModel()
            {
                StatusIdlst = request.StatusIdList != null && request.StatusIdList.Any() ? request.StatusIdList : InspectedStatusList,
                SelectedCountryIdList = request.CountryIdList,
                Officeidlst = request.OfficeIdList,
                CustomerList = request.CustomerIdList,
                SupplierId = request.SupplierId,
            };


            var inspections = _sharedInspectionManager.GetAllInspectionQuery();
            inspections = _sharedInspectionManager.GetInspectionQuerywithRequestFilters(sharedInspectionModel, inspections);
            //when comapre data is checked and only other type means:- Manday By Country, Manday By Customer and Manday search
            if (request.IsCompareData && request.ChartType == MandayDashboardChartType.Other)
            {

                var fromDate = request.ServiceDateFrom.ToDateTime();
                var toDate = request.ServiceDateTo.ToDateTime();

                var comparedFromDate = request.ComparedServiceDateFrom.ToDateTime();
                var comparedToDate = request.ComparedServiceDateTo.ToDateTime();

                inspections = inspections.Where(x => !((x.ServiceDateFrom > toDate) || (x.ServiceDateTo < fromDate)) || !((x.ServiceDateFrom > comparedToDate) || (x.ServiceDateTo < comparedFromDate)));
            }
            else
            {
                var fromDate = request.ServiceDateFrom.ToDateTime();
                var toDate = request.ServiceDateTo.ToDateTime();
                inspections = inspections.Where(x => !((x.ServiceDateFrom > toDate) || (x.ServiceDateTo < fromDate)));
            }

            return inspections;
        }

        //get the audit data
        private async Task<IQueryable<AuditResponseManday>> CommonAuditSearch(MandayDashboardRequest request)
        {
            //get all audits
            var auditData = _repo.GetAllAudits();

            auditData = auditData.Where(x => x.StatusId == (int)AuditStatus.Audited);

            //filter based on service date from and to             
            //when comapre data is checked and only other type means:- Manday By Country, Manday By Customer and Manday search
            if (request.IsCompareData && request.ChartType == MandayDashboardChartType.Other)
            {

                var fromDate = request.ServiceDateFrom.ToDateTime();
                var toDate = request.ServiceDateTo.ToDateTime();

                var comparedFromDate = request.ComparedServiceDateFrom.ToDateTime();
                var comparedToDate = request.ComparedServiceDateTo.ToDateTime();
                auditData = auditData.Where(x => !((x.ServiceDateFrom > toDate) || (x.ServiceDateTo < fromDate)) || !((x.ServiceDateFrom > comparedToDate) || (x.ServiceDateTo < comparedFromDate)));
            }
            else
            {

                var fromDate = request.ServiceDateFrom.ToDateTime();
                var toDate = request.ServiceDateTo.ToDateTime();
                auditData = auditData.Where(x => !((x.ServiceDateFrom > toDate) || (x.ServiceDateTo < fromDate)));
            }


            //filter based on factory country
            if (request.CountryIdList != null && request.CountryIdList.Any())
            {
                //get the factory id with the country in the list
                var factoryList = await _supRepo.GetFactoryByCountryId(request.CountryIdList);
                auditData = auditData.Where(x => factoryList.Contains(x.FactoryId));
            }
            //filter based on officeId
            if (request.OfficeIdList != null && request.OfficeIdList.Any())
            {
                auditData = auditData.Where(x => request.OfficeIdList.Contains(x.OfficeId.GetValueOrDefault()));
            }
            //filter based on customer Id
            if (request.CustomerIdList != null && request.CustomerIdList.Any())
            {
                auditData = auditData.Where(x => request.CustomerIdList.Contains(x.CustomerId));
            }

            return auditData;
        }

        //Common method for Man day year chart and export to fetch the manday data by year
        private async Task<List<MandayYearChartItem>> CommonMandayYearChart(MandayDashboardRequest request)
        {
            var data = new List<MandayYearChartItem>();
            request.ChartType = MandayDashboardChartType.MandayByYear;
            try
            {
                if (request.ServiceId == (int)Service.InspectionId)
                {
                    //fetch the booking details based on request
                    var bookingData = CommonInspSearch(request);

                    //filter based on the inner factory country dropdown in the chart
                    if (request.MandayYearSubCountryId.HasValue && request.MandayYearSubCountryId > 0)
                    {
                        var list = new[] { request.MandayYearSubCountryId.Value };
                        //fetch the factory Ids based on country
                        var factoryList = await _supRepo.GetFactoryByCountryId(list.ToList());
                        bookingData = bookingData.Where(x => x.FactoryId > 0 && factoryList.Contains(x.FactoryId.GetValueOrDefault()));
                    }
                    //filter based on the inner customer dropdown in the chart
                    if (request.MandayYearSubCustomerId.HasValue && request.MandayYearSubCustomerId > 0)
                    {
                        bookingData = bookingData.Where(x => x.CustomerId == request.MandayYearSubCustomerId);
                    }

                    var bookingIdList = bookingData.Select(x => x.Id);

                    if (request.MandayType == (int)MandayType.EstimatedManday)
                        data = await _repo.GetMonthlyInspManDays(bookingIdList);
                    else if (request.MandayType == (int)MandayType.ActualManday)
                        data = await _repo.GetMonthlyInspActualManDays(bookingIdList);
                }

                else if (request.ServiceId == (int)Service.AuditId)
                {
                    //fetch the audit details based on request
                    var auditData = await CommonAuditSearch(request);

                    //filter based on the inner factory country dropdown in the chart
                    if (request.MandayYearSubCountryId.HasValue && request.MandayYearSubCountryId > 0)
                    {
                        var list = new[] { request.MandayYearSubCountryId.Value };
                        //fetch the factory Ids based on country
                        var factoryList = await _supRepo.GetFactoryByCountryId(list.ToList());
                        auditData = auditData.Where(x => factoryList.Contains(x.FactoryId));
                    }
                    //filter based on the inner customer dropdown in the chart
                    if (request.MandayYearSubCustomerId.HasValue && request.MandayYearSubCustomerId > 0)
                    {
                        auditData = auditData.Where(x => x.CustomerId == request.MandayYearSubCustomerId);
                    }

                    var auditIdList = auditData.Select(x => x.AuditId);
                    data = await _repo.GetMonthlyAuditManDays(auditIdList);
                }
            }

            catch (Exception ex)
            {
                data = null;
            }

            return data;
        }

        //get the data for the man day year chart
        public async Task<MandayYearChartResponse> GetMandayYearChart(MandayDashboardRequest request)
        {
            var response = new MandayYearChartResponse();
            var mandayYear = new List<MandayYear>();
            var data = new List<MandayYearChartItem>();
            try
            {
                data = await CommonMandayYearChart(request);
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
                    MandayCount = request.MandayType == (int)MandayType.EstimatedManday ? _data.Where(x => x.Year == key).Sum(x => x.MonthManDay) : Math.Round(_data.Where(x => x.Year == key).Sum(x => x.MonthActualManDay), 1),
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

        //common method for man day customer chart and export to fetch man day by customer
        private async Task<IQueryable<MandayCustomerChart>> CommonMandayCustomerChart(MandayDashboardRequest request)
        {

            request.ChartType = MandayDashboardChartType.Other;
            IQueryable<MandayCustomerChart> data = null;
            try
            {
                if (request.ServiceId == (int)Service.InspectionId)
                {
                    //fetch data based on request
                    var bookingIdList = await CommonInspectionData(request);
                    if (request.MandayType == (int)MandayType.EstimatedManday)
                        //get the manday count
                        data = _repo.GetCustomerInspManDays(bookingIdList);
                    else if (request.MandayType == (int)MandayType.ActualManday)
                        //get the actual manday count
                        data = _repo.GetCustomerActualManDays(bookingIdList);
                }

                else if (request.ServiceId == (int)Service.AuditId)
                {
                    //fetch data based on request                    
                    var auditData = await CommonAuditSearch(request);
                    //filter based on the inner factory country dropdown in the chart
                    if (request.MandayCustomerSubCountryId.HasValue && request.MandayCustomerSubCountryId > 0)
                    {
                        var list = new[] { request.MandayCustomerSubCountryId.Value };
                        //fetch the factory Id based on country
                        var factoryList = await _supRepo.GetFactoryByCountryId(list.ToList());
                        auditData = auditData.Where(x => factoryList.Contains(x.FactoryId));
                    }

                    var bookingIdList = auditData.Select(x => x.AuditId);
                    //get the manday count
                    data = _repo.GetCustomerAuditManDays(bookingIdList);
                }
            }
            catch (Exception ex)
            {
                data = null;
            }

            return data;
        }

        //fetch data for man day customer chart
        public async Task<MandayCustomerChartResponse> GetMandayCustomerChart(MandayDashboardRequest request)
        {
            var response = new MandayCustomerChartResponse();
            var data = new List<MandayCustomerChart>();
            try
            {
                //fetch data based on request
                var result = await CommonMandayCustomerChart(request);
                data = await result.ToListAsync();
                if (data == null || !data.Any())
                {
                    response.Result = MandayDashboardResult.NotFound;
                    return response;
                }

                Random r = new Random();

                IEnumerable<MandayCustomerChart> comparedMandayCustomerData = null;
                if (request.IsCompareData)
                {
                    //based on the compared service from and to date get the data
                    var comparedMandayCustomerDateData = data.Where(x => x.ServiceDateTo <= request.ComparedServiceDateTo.ToDateTime() && x.ServiceDateTo >= request.ComparedServiceDateFrom.ToDateTime());
                    comparedMandayCustomerData = comparedMandayCustomerDateData.GroupBy(p => p.CustomerName, (key, _data) =>
                    new MandayCustomerChart
                    {
                        CustomerName = key,
                        MandayCount = Math.Round(_data.Where(x => x.CustomerName == key).Sum(x => x.MandayCount), 1),
                        Percentage = GetCustomerPercentage(_data.Where(x => x.CustomerName == key).Sum(x => x.MandayCount), comparedMandayCustomerDateData.Sum(x => x.MandayCount)),
                        Color = string.Format("{0:X}", "#" + r.Next(0, 0xFFFFF))
                    }).OrderByDescending(x => x.MandayCount).ToList();
                }
                //get the man day count of service from and to date
                var serviceMandayCustomerDateData = data.Where(x => x.ServiceDateTo <= request.ServiceDateTo.ToDateTime() && x.ServiceDateTo >= request.ServiceDateFrom.ToDateTime()).ToList();
                //add the group by data
                var actualMandayCustomerData = serviceMandayCustomerDateData.GroupBy(p => p.CustomerName, (key, _data) =>
                {
                    var mandayCustomerChart = new MandayCustomerChart
                    {
                        CustomerName = key,
                        MandayCount = Math.Round(_data.Sum(x => x.MandayCount), 1),
                        Percentage = GetCustomerPercentage(_data.Sum(x => x.MandayCount), serviceMandayCustomerDateData.Sum(x => x.MandayCount)),
                        Color = string.Format("{0:X}", "#" + r.Next(0, 0xFFFFF)),

                    };
                    mandayCustomerChart.ComparedPercentage = !request.IsCompareData ? 0 : GetMandayComparisonData(mandayCustomerChart.MandayCount, comparedMandayCustomerData?.FirstOrDefault(x => x.CustomerName == key)?.MandayCount);
                    return mandayCustomerChart;
                }).OrderByDescending(x => x.MandayCount).Take(50);

                response.Data = actualMandayCustomerData;
                response.Total = Math.Round(serviceMandayCustomerDateData.Sum(x => x.MandayCount), 1);
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

        //Get the manday % in Customer and Employee man day chart
        private double GetCustomerPercentage(double manday, double totalManday)
        {
            double res = (manday / totalManday);

            return Math.Round(res * 100, 2);
        }

        //fetch man day details for Man day based on country and province chart
        public async Task<MandayCountryChartResponse> GetMandayCountryChart(MandayDashboardRequest request)
        {
            var response = new MandayCountryChartResponse();
            var data = new List<MandayCountryChart>();
            try
            {
                request.ChartType = MandayDashboardChartType.Other;

                if (request.ServiceId == (int)Service.InspectionId)
                {
                    //fetch data based on request
                    var bookingData = CommonInspSearch(request);

                    IQueryable<int> idList = null;
                    int mandayCountrySubCountryId = 0;
                    //filter if province selected, get data for the selected province
                    if (request.MandayCountrySubProvinceId.HasValue && request.MandayCountrySubProvinceId > 0)
                    {
                        var factoryList = await _supRepo.GetSupplierByProvinceId(request.MandayCountrySubProvinceId.Value);
                        bookingData = bookingData.Where(x => x.FactoryId > 0 && factoryList.Contains(x.FactoryId.GetValueOrDefault()));
                        idList = bookingData.Select(x => x.Id);
                        mandayCountrySubCountryId = request.MandayCountrySubCountryId.Value;
                    }
                    //filter if country selected, get province wise data for the selected country
                    else if (request.MandayCountrySubCountryId.HasValue && request.MandayCountrySubCountryId > 0)
                    {
                        var list = new[] { request.MandayCountrySubCountryId.Value };
                        var factoryList = await _supRepo.GetFactoryByCountryId(list.ToList());
                        bookingData = bookingData.Where(x => x.FactoryId > 0 && factoryList.Contains(x.FactoryId.GetValueOrDefault()));
                        idList = bookingData.Select(x => x.Id);

                        mandayCountrySubCountryId = request.MandayCountrySubCountryId.Value;
                    }
                    //get country wise data
                    else
                    {
                        idList = bookingData.Select(x => x.Id);
                    }

                    if (request.MandayType == (int)MandayType.EstimatedManday)
                        data = await _repo.GetCountryInspManDays(idList, mandayCountrySubCountryId);
                    else if (request.MandayType == (int)MandayType.ActualManday)
                        data = await _repo.GetCountryActualInspManDays(idList, mandayCountrySubCountryId);

                }

                else if (request.ServiceId == (int)Service.AuditId)
                {
                    //fetch data based on request
                    var auditData = await CommonAuditSearch(request);
                    //filter if province selected, get data for the selected province
                    if (request.MandayCountrySubProvinceId.HasValue && request.MandayCountrySubProvinceId > 0)
                    {
                        var factoryList = await _supRepo.GetSupplierByProvinceId(request.MandayCountrySubProvinceId.Value);
                        auditData = auditData.Where(x => factoryList.Contains(x.FactoryId));
                        var idList = auditData.Select(x => x.AuditId);
                        data = await _repo.GetCountryAuditManDays(idList, request.MandayCountrySubCountryId.Value);
                    }
                    //filter if country selected, get province wise data for the selected country
                    else if (request.MandayCountrySubCountryId.HasValue && request.MandayCountrySubCountryId > 0)
                    {
                        var list = new[] { request.MandayCountrySubCountryId.Value };
                        var factoryList = await _supRepo.GetFactoryByCountryId(list.ToList());
                        auditData = auditData.Where(x => factoryList.Contains(x.FactoryId));
                        var idList = auditData.Select(x => x.AuditId);
                        data = await _repo.GetCountryAuditManDays(idList, request.MandayCountrySubCountryId.Value);
                    }
                    //get country wise data
                    else
                    {
                        var idList = auditData.Select(x => x.AuditId);
                        data = await _repo.GetCountryAuditManDays(idList, 0);
                    }
                }

                Random r = new Random();
                IEnumerable<MandayCountryChart> compareMandayCountryData = null;
                if (request.IsCompareData)
                {
                    //get the compared from and to country data
                    var compareMandayCountryDateData = data.Where(x => x.ServiceDateTo <= request.ComparedServiceDateTo.ToDateTime() && x.ServiceDateTo >= request.ComparedServiceDateFrom.ToDateTime());
                    compareMandayCountryData = compareMandayCountryDateData.GroupBy(p => p.Name, (key, _data) =>
                     new MandayCountryChart
                     {
                         Name = key,
                         MandayCount = Math.Round(_data.Where(x => x.Name == key).Sum(x => x.MandayCount), 1),
                     }).OrderByDescending(x => x.MandayCount);
                }
                //get the manday count
                var serviceMandayCountryDateData = data.Where(x => x.ServiceDateTo <= request.ServiceDateTo.ToDateTime() && x.ServiceDateTo >= request.ServiceDateFrom.ToDateTime());
                var actualMandayCountryData = serviceMandayCountryDateData.GroupBy(p => p.Name, (key, _data) =>
                {
                    var mandayCountryChart = new MandayCountryChart
                    {
                        Name = key,
                        MandayCount = Math.Round(_data.Where(x => x.Name == key).Sum(x => x.MandayCount), 1),
                        Color = string.Format("{0:X}", "#" + r.Next(0, 0xFFFFF)),
                        ComparedMandayCount = !request.IsCompareData ? 0 : compareMandayCountryData.FirstOrDefault(x => x.Name == key)?.MandayCount
                    };

                    mandayCountryChart.ComparedPercentage = !request.IsCompareData ? 0 : GetMandayComparisonData(mandayCountryChart.MandayCount, mandayCountryChart.ComparedMandayCount.GetValueOrDefault());
                    return mandayCountryChart;
                }).OrderByDescending(x => x.MandayCount).Take(20);

                response.Data = actualMandayCountryData;
                response.TotalCount = Math.Round(serviceMandayCountryDateData.Sum(x => x.MandayCount), 1);
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

        //common method for employee type chart and export to fetch manday based on employee type
        private async Task<List<EmployeeTypes>> CommonMandayEmployeeTypeChart(MandayDashboardRequest request)
        {
            var data = new List<EmployeeTypes>();
            request.ChartType = MandayDashboardChartType.MandayByEmployeeType;
            try
            {
                if (request.ServiceId == (int)Service.InspectionId)
                {
                    //fetch the booking data based on the request
                    var bookingData = CommonInspSearch(request);

                    //filter for inner customer dropdown in the chart
                    if (request.MandayEmployeeTypeSubCustomerId.HasValue && request.MandayEmployeeTypeSubCustomerId > 0)
                    {
                        bookingData = bookingData.Where(x => x.CustomerId == request.MandayEmployeeTypeSubCustomerId);
                    }

                    //filter for inner year dropdown in the chart
                    if (request.MandayEmployeeTypeSubYear > 0)
                    {
                        bookingData = bookingData.Where(x => x.ServiceDateFrom.Year == request.MandayEmployeeTypeSubYear && x.ServiceDateTo.Year == request.MandayEmployeeTypeSubYear);
                    }
                    //if no year selected and data range has more than a year, fetch service date from year manday count
                    else if (request.ServiceDateFrom.Year != request.ServiceDateTo.Year)
                    {
                        bookingData = bookingData.Where(x => x.ServiceDateFrom.Year == request.ServiceDateFrom.Year && x.ServiceDateTo.Year == request.ServiceDateFrom.Year);
                    }

                    var bookingIds = bookingData.Select(x => x.Id);

                    if (request.MandayType == (int)MandayType.EstimatedManday)
                        //get the manday by employee type and month
                        data = await _repo.GetMandayInspByEmployeeType(bookingIds, request.ServiceDateFrom.ToDateTime(), request.ServiceDateTo.ToDateTime());
                    else
                        //get the actual manday by employee type and month
                        data = await _repo.GetActualMandayInspByEmployeeType(bookingIds, request.ServiceDateFrom.ToDateTime(), request.ServiceDateTo.ToDateTime());
                }

                else if (request.ServiceId == (int)Service.AuditId)
                {
                    //fetch the audit data based on the request
                    var auditData = await CommonAuditSearch(request);

                    //filter for inner customer dropdown in the chart
                    if (request.MandayEmployeeTypeSubCustomerId.HasValue && request.MandayEmployeeTypeSubCustomerId > 0)
                    {
                        auditData = auditData.Where(x => x.CustomerId == request.MandayEmployeeTypeSubCustomerId);
                    }
                    //filter for inner year dropdown in the chart
                    if (request.MandayEmployeeTypeSubYear > 0)
                    {
                        auditData = auditData.Where(x => x.ServiceDateFrom.Year == request.MandayEmployeeTypeSubYear && x.ServiceDateTo.Year == request.MandayEmployeeTypeSubYear);
                    }
                    //if no year selected and data range has more than a year, fetch service date from year manday count
                    else if (request.ServiceDateFrom.Year != request.ServiceDateTo.Year)
                    {
                        auditData = auditData.Where(x => x.ServiceDateFrom.Year == request.ServiceDateFrom.Year && x.ServiceDateTo.Year == request.ServiceDateFrom.Year);
                    }

                    var auditIds = auditData.Select(x => x.AuditId);

                    //get the manday by employee type and month
                    data = await _repo.GetMandayAuditByEmployeeType(auditIds);

                }

                //set the manday % for each month
                foreach (var item in data)
                {
                    var manday = GetCustomerPercentage((int)item.MandayCount, (int)data.Where(y => y.Month == item.Month).Sum(y => y.MandayCount));
                    item.MandayPercentage = manday;
                }
            }

            catch (Exception ex)
            {
                data = null;
            }

            return data;
        }

        public async Task<MandayEmployeeTypeChartResponse> GetManDayEmployeeTypeChart(MandayDashboardRequest request)
        {
            var response = new MandayEmployeeTypeChartResponse();
            var data = new List<EmployeeTypes>();
            var mandayYear = new List<MandayYear>();

            try
            {
                //fetch the man day based on employee type and month
                data = await CommonMandayEmployeeTypeChart(request);

                if (data == null || !data.Any())
                {
                    response.Result = MandayDashboardResult.NotFound;
                    return response;
                }

                int count = 1;

                var items = data.GroupBy(p => p.EmployeeType, (key, _data) =>
                new EmployeeTypeYear
                {
                    EmployeeType = key,
                    MandayCount = GetCustomerPercentage((int)_data.Where(x => x.EmployeeType == key).Sum(x => x.MandayCount), (int)data.Sum(x => x.MandayCount)),
                    Color = MandayDashboardColorList.GetValueOrDefault(count++),
                    MonthlyData = _data,
                }).OrderByDescending(x => x.MandayCount).ToList();

                response.Data = items;

                //form x axis month data for the chart
                for (int i = 0; i < 12; i++)
                {
                    MandayYear res = new MandayYear();
                    res.year = DateTime.Now.Year;
                    res.month = i;
                    res.MonthName = MonthData.GetValueOrDefault(i + 1);
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

        //get details for the man day year details export
        public async Task<MandayYearExport> ExportMandayYearChart(MandayDashboardRequest request)
        {
            var response = new MandayYearExport();
            var data = new List<MandayYearChartItem>();
            try
            {
                //fetch the data based on filter
                data = await CommonMandayYearChart(request);

                response.Data = data.OrderBy(x => x.Year).ThenBy(x => x.Month).ToList();
                response.Total = request.MandayType == (int)MandayType.EstimatedManday ? data.Sum(x => x.MonthManDay) : data.Sum(x => x.MonthActualManDay);
                //get the dropdown data for the export file
                var result = await SetExportFilter(request);
                response.RequestFilters = result;
            }
            catch (Exception ex)
            {
                data = null;
            }
            return response;
        }

        //get details for the man day country details export
        public async Task<MandayCountryChartExportResponse> ExportMandayCountryChart(MandayDashboardRequest request)
        {
            var data = new List<MandayCountryChartExport>();
            var response = new MandayCountryChartExportResponse();

            bool iscountrySelected = (request.MandayCountrySubCountryId != null && request.MandayCountrySubCountryId > 0) ||
                (request.MandayCountrySubProvinceId != null && request.MandayCountrySubProvinceId > 0) ? true : false;

            request.ChartType = MandayDashboardChartType.Other;
            if (request.ServiceId == (int)Service.InspectionId)
            {
                //fetch the data based on request
                var bookingData = CommonInspSearch(request);
                //if province is selected, fetch data for the selected province
                if (request.MandayCountrySubProvinceId.HasValue && request.MandayCountrySubProvinceId > 0)
                {
                    var factoryList = await _supRepo.GetSupplierByProvinceId(request.MandayCountrySubProvinceId.Value);
                    bookingData = bookingData.Where(x => x.FactoryId > 0 && factoryList.Contains(x.FactoryId.GetValueOrDefault()));
                }
                //if country is selected, get the data for the selected country by province
                if (request.MandayCountrySubCountryId.HasValue && request.MandayCountrySubCountryId > 0)
                {
                    var list = new[] { request.MandayCountrySubCountryId.Value };
                    //fetch the factory Id by country
                    var factoryList = await _supRepo.GetFactoryByCountryId(list.ToList());
                    bookingData = bookingData.Where(x => x.FactoryId > 0 && factoryList.Contains(x.FactoryId.GetValueOrDefault()));
                }
                var idList = bookingData.Select(x => x.Id);

                if (request.MandayType == (int)MandayType.EstimatedManday)
                    data = await _repo.GetCountryInspManDaysExport(idList, iscountrySelected);
                else
                    data = await _repo.GetCountryInspActualManDaysExport(idList, iscountrySelected);
            }

            else if (request.ServiceId == (int)Service.AuditId)
            {
                //fetch the data based on request
                var auditData = await CommonAuditSearch(request);
                //if province is selected, fetch data for the selected province
                if (request.MandayCountrySubProvinceId.HasValue && request.MandayCountrySubProvinceId > 0)
                {
                    var factoryList = await _supRepo.GetSupplierByProvinceId(request.MandayCountrySubProvinceId.Value);
                    auditData = auditData.Where(x => factoryList.Contains(x.FactoryId));
                }
                //if country is selected, get the data for the selected country by province
                if (request.MandayCountrySubCountryId.HasValue && request.MandayCountrySubCountryId > 0)
                {
                    var list = new[] { request.MandayCountrySubCountryId.Value };
                    //fetch the factory Id by country
                    var factoryList = await _supRepo.GetFactoryByCountryId(list.ToList());
                    auditData = auditData.Where(x => factoryList.Contains(x.FactoryId));
                }

                var idList = auditData.Select(x => x.AuditId);
                data = await _repo.GetCountryAuditManDaysExport(idList, iscountrySelected);
            }
            //get the manday count by country and province by compared data
            List<MandayCountryChartExport> compareMandayCountryData = null;
            if (request.IsCompareData)
            {
                var comparedMandayDateData = data.Where(x => x.ServiceDateTo <= request.ComparedServiceDateTo.ToDateTime() && x.ServiceDateTo >= request.ComparedServiceDateFrom.ToDateTime());
                compareMandayCountryData = comparedMandayDateData.GroupBy(p => new { p.CountryName, p.ProvinceName }, p => p, (key, _data) =>
                   new MandayCountryChartExport
                   {
                       CountryName = key.CountryName,
                       MandayCount = Math.Round(_data.Where(x => x.CountryName == key.CountryName && x.ProvinceName == key.ProvinceName).Sum(x => x.MandayCount), 1)
                   }).OrderBy(x => x.CountryName).ToList();
            }

            //get the manday count by country and province
            var serviceMandayCountryDateData = data.Where(x => x.ServiceDateTo <= request.ServiceDateTo.ToDateTime() && x.ServiceDateTo >= request.ServiceDateFrom.ToDateTime());
            var actualMandayCountryData = serviceMandayCountryDateData.GroupBy(p => new { p.CountryName, p.ProvinceName }, p => p, (key, _data) =>
               new MandayCountryChartExport
               {
                   CountryName = key.CountryName,
                   ProvinceName = key.ProvinceName,
                   MandayCount = Math.Round(_data.Where(x => x.CountryName == key.CountryName && x.ProvinceName == key.ProvinceName).Sum(x => x.MandayCount), 1),
                   ComparedMandayCount = compareMandayCountryData?.FirstOrDefault(x => x.CountryName == key.CountryName)?.MandayCount
               }).OrderBy(x => x.CountryName).ToList();

            if (request.IsCompareData)
            {
                //if data not availabe in actual, but it's available in compared
                var notAvailableActualMandayCountryData = compareMandayCountryData.Where(x => !(actualMandayCountryData.Select(y => y.CountryName).Contains(x.CountryName))).Select(x => new MandayCountryChartExport()
                {
                    CountryName = x.CountryName,
                    ProvinceName = x.ProvinceName,
                    MandayCount = 0,
                    ComparedMandayCount = x.MandayCount
                }).ToList();

                if (notAvailableActualMandayCountryData.Any())
                    actualMandayCountryData.AddRange(notAvailableActualMandayCountryData);
            }


            response.Data = actualMandayCountryData;
            response.Total = actualMandayCountryData.Sum(x => x.MandayCount);
            response.IsCompareData = request.IsCompareData;
            //get the request dropdown data for the export file
            response.RequestFilters = await SetExportFilter(request);
            return response;
        }


        //get details for the man day employee type details export
        public async Task<MandayEmployeeTypeChartExport> ExportManDayEmployeeTypeChart(MandayDashboardRequest request)
        {
            var data = new List<EmployeeTypes>();
            var response = new MandayEmployeeTypeChartExport();
            try
            {
                //fetch the data based on filter
                data = await CommonMandayEmployeeTypeChart(request);
                //get the manday % by employee type
                response.Data = data.GroupBy(p => p.MonthName, (key, _data) =>
                new MandayEmployeeTypeChartExportItem
                {
                    MonthName = key,
                    MonthCount = _data.Where(x => x.MonthName == key).Sum(x => x.MandayPercentage),
                    Data = _data.Where(x => x.MonthName == key).ToList()
                }).OrderByDescending(x => x.MonthCount).ToList();

                response.Total = response.Data.Sum(x => x.MonthCount);
                response.EmployeeTypeNames = data.Select(x => x.EmployeeType).Distinct().ToList();
                //get the request dropdown data for the export file
                response.RequestFilters = await SetExportFilter(request);
            }

            catch (Exception ex)
            {
                data = null;
            }

            return response;
        }

        //Get the dropdown values to specify in the export file
        public async Task<MandayDashboardRequestExport> SetExportFilter(MandayDashboardRequest request)
        {
            var response = new MandayDashboardRequestExport();

            //top office filter
            if (request.OfficeIdList != null && request.OfficeIdList.Any())
            {
                var officeList = await _officeRepo.GetLocationListByIds(request.OfficeIdList.Select(y => y.GetValueOrDefault()).Distinct().ToList());
                response.OfficeIdList = string.Join(", ", officeList.Select(x => x.Name));
            }


            //top factory country filter
            if (request.CountryIdList != null && request.CountryIdList.Any())
            {
                var countryList = await _locationRepo.GetCountryByIds(request.CountryIdList);
                response.CountryIdList = string.Join(", ", countryList.Select(x => x.Name));
            }
            //man day country chart inner country filter
            if (request.MandayCountrySubCountryId != null && request.MandayCountrySubCountryId > 0)
            {
                var list = new[] { request.MandayCountrySubCountryId.Value };
                var countryList = await _locationRepo.GetCountryByIds(list.ToList());
                response.MandayCountrySubCountry = countryList.Where(x => x.Id == request.MandayCountrySubCountryId).Select(x => x.Name).FirstOrDefault();
            }
            //man day customer chart inner country filter
            else if (request.MandayCustomerSubCountryId != null && request.MandayCustomerSubCountryId > 0)
            {
                var list = new[] { request.MandayCustomerSubCountryId.Value };
                var countryList = await _locationRepo.GetCountryByIds(list.ToList());
                response.MandayCustomerSubCountry = countryList.Where(x => x.Id == request.MandayCustomerSubCountryId).Select(x => x.Name).FirstOrDefault();
            }
            //man day year chart inner country filter
            else if (request.MandayYearSubCountryId != null && request.MandayYearSubCountryId > 0)
            {
                var list = new[] { request.MandayYearSubCountryId.Value };
                var countryList = await _locationRepo.GetCountryByIds(list.ToList());
                response.MandayYearSubCountry = countryList.Where(x => x.Id == request.MandayYearSubCountryId).Select(x => x.Name).FirstOrDefault();
            }

            if (request.CustomerIdList != null && request.CustomerIdList.Any())
            {
                var customers = await _cusRepo.GetCustomerById(request.CustomerIdList);
                response.CustomerList = string.Join(", ", customers.Select(x => x.Name));
            }

            //man day year chart - inner customer filter
            if (request.MandayYearSubCustomerId != null && request.MandayYearSubCustomerId > 0)
            {
                var customer = await _cusRepo.GetCustomerItemById(request.MandayYearSubCustomerId.Value);
                response.MandayYearSubCustomer = customer.CustomerName;
            }
            //man day employee type chart - inner customer filter
            else if (request.MandayEmployeeTypeSubCustomerId != null && request.MandayEmployeeTypeSubCustomerId > 0)
            {
                var customer = await _cusRepo.GetCustomerItemById(request.MandayEmployeeTypeSubCustomerId.Value);
                response.MandayEmployeeTypeSubCustomer = customer.CustomerName;
            }
            //man day country chart - inner province filter
            if (request.MandayCountrySubProvinceId != null && request.MandayCountrySubProvinceId > 0)
            {
                var province = _locationRepo.GetProvinceDetails(request.MandayCountrySubProvinceId.Value);
                response.MandayCountrySubProvince = province.ProvinceName;
            }
            var serviceList = await _referenceManager.GetServices();

            response.Service = serviceList.Where(x => x.Id == request.ServiceId).Select(x => x.Name).FirstOrDefault();
            response.ServiceDateFrom = request.ServiceDateFrom == null ? "" : request.ServiceDateFrom.ToDateTime().ToString(StandardDateFormat);
            response.ServiceDateTo = request.ServiceDateTo == null ? "" : request.ServiceDateTo.ToDateTime().ToString(StandardDateFormat);
            response.ComparedServiceDateFrom = request.ComparedServiceDateFrom == null || request.ChartType != MandayDashboardChartType.Other ? "" : request.ComparedServiceDateFrom.ToDateTime().ToString(StandardDateFormat);
            response.ComparedServiceDateTo = request.ComparedServiceDateTo == null || request.ChartType != MandayDashboardChartType.Other ? "" : request.ComparedServiceDateTo.ToDateTime().ToString(StandardDateFormat);
            response.MandayEmployeeTypeSubYear = request.MandayEmployeeTypeSubYear;
            response.MandayType = request.MandayType;
            return response;
        }

        //get details for the man day customer details export
        public async Task<MandayCustomerChartExportResponse> ExportMandayCustomerChart(MandayDashboardRequest request)
        {
            var response = new MandayCustomerChartExportResponse();
            var data = new List<MandayCustomerChart>();
            try
            {
                //fetch the data based on request
                var result = await CommonMandayCustomerChart(request);
                var reportData = new List<MandayCustomerChartData>();
                if (request.ServiceId == (int)Service.InspectionId)
                {

                    //get the report data
                    reportData = await _repo.GetCustomerInspReportsData(result.Select(x => x.InspectionId).Distinct());
                }

                data = await result.ToListAsync();


                //compared date get the manday count by customer

                IEnumerable<MandayCustomerChart> comparedMandayCustomerData = null;
                if (request.IsCompareData)
                {
                    //compared date data                   
                    var comapredMandayCustomerDateReportData = reportData.Where(x => x.ServiceDateTo <= request.ComparedServiceDateTo.ToDateTime() && x.ServiceDateTo >= request.ComparedServiceDateFrom.ToDateTime());
                    var comapredMandayCustomerDateData = data.Where(x => x.ServiceDateTo <= request.ComparedServiceDateTo.ToDateTime() && x.ServiceDateTo >= request.ComparedServiceDateFrom.ToDateTime());
                    var comapredMandayCustomerDateMandayCount = Math.Round(comapredMandayCustomerDateData.Sum(x => x.MandayCount), 1);
                    comparedMandayCustomerData = comapredMandayCustomerDateData.GroupBy(p => p.CustomerName, (key, _data) =>
                    {
                        var result = comapredMandayCustomerDateReportData.FirstOrDefault(x => x.CustomerName == key);

                        var sumMandayCount = Math.Round(_data.Where(x => x.CustomerName == key).Sum(x => x.MandayCount), 1);
                        return new MandayCustomerChart
                        {
                            CustomerName = key,
                            MandayCount = sumMandayCount,
                            Percentage = GetCustomerPercentage(sumMandayCount, comapredMandayCustomerDateMandayCount),
                            InspectionCount = _data.Count(x => x.CustomerName == key),
                            ReportCount = result?.ReportCount,
                            InspectedQty = result?.InspectedQty,
                            OrderQty = result?.OrderQty,
                            PresentedQty = result?.PresentedQty
                        };
                    }).OrderByDescending(x => x.MandayCount);
                }

                //get the manday count by customer
                var serviceDateMandayCustomerReportData = reportData.Where(x => x.ServiceDateTo <= request.ServiceDateTo.ToDateTime() && x.ServiceDateTo >= request.ServiceDateFrom.ToDateTime());
                var serviceDateMandayCustomerData = data.Where(x => x.ServiceDateTo <= request.ServiceDateTo.ToDateTime() && x.ServiceDateTo >= request.ServiceDateFrom.ToDateTime());
                var serviceDateMandayCustomerTotalManday = Math.Round(serviceDateMandayCustomerData.Sum(x => x.MandayCount), 1);
                var items = serviceDateMandayCustomerData.GroupBy(p => p.CustomerName, (key, _data) =>
                {
                    var cData = comparedMandayCustomerData?.FirstOrDefault(x => x.CustomerName == key);
                    var result = serviceDateMandayCustomerReportData.Where(x => x.CustomerName == key);
                    var mandayCount = Math.Round(_data.Where(x => x.CustomerName == key).Sum(x => x.MandayCount), 1);
                    var mandayCustomerChart = new MandayCustomerChart
                    {
                        CustomerName = key,
                        MandayCount = mandayCount,
                        Percentage = GetCustomerPercentage(mandayCount, serviceDateMandayCustomerTotalManday),
                        InspectionCount = _data.Select(x => x.InspectionId).Distinct().Count(),
                        ReportCount = result?.Sum(x => x.ReportCount),
                        InspectedQty = result?.Sum(x => x.InspectedQty),
                        OrderQty = result?.Sum(x => x.OrderQty),
                        PresentedQty = result?.Sum(x => x.PresentedQty),
                        ComparedInspectedQty = cData?.InspectedQty,
                        ComparedInspectionCount = cData?.InspectionCount,
                        ComparedMandayCount = cData?.MandayCount,
                        ComparedPercentage = cData?.Percentage,
                        ComparedPresentedQty = cData?.PresentedQty,
                        ComparedReportCount = cData?.ReportCount,
                        ComparedOrderQty = cData?.OrderQty,
                    };
                    return mandayCustomerChart;
                }).OrderByDescending(x => x.MandayCount).ToList();


                if (request.IsCompareData)
                {
                    //if data not availabe in actual, but it's available in compared
                    var notAvailableActualMandayCustomerData = comparedMandayCustomerData.Where(x => !(items.Select(y => y.CustomerName).Contains(x.CustomerName))).Select(x => new MandayCustomerChart()
                    {
                        CustomerName = x.CustomerName,
                        MandayCount = 0,
                        Percentage = 0,
                        InspectionCount = 0,
                        ReportCount = 0,
                        InspectedQty = 0,
                        OrderQty = 0,
                        PresentedQty = 0,
                        ComparedInspectedQty = x.InspectedQty,
                        ComparedInspectionCount = x.InspectionCount,
                        ComparedMandayCount = x.MandayCount,
                        ComparedPercentage = x.Percentage,
                        ComparedPresentedQty = x.PresentedQty,
                        ComparedReportCount = x.ReportCount,
                        ComparedOrderQty = x.OrderQty
                    }).ToList();

                    if (notAvailableActualMandayCustomerData.Any())
                        items.AddRange(notAvailableActualMandayCustomerData);
                }


                response.ServiceId = request.ServiceId;
                response.IsCompareData = request.IsCompareData;
                response.Data = items;
                response.Total = Math.Round(items.Sum(x => x.MandayCount), 1);
                response.Result = MandayDashboardResult.Success;
                //get the request dropdown data for the export file
                response.RequestFilters = await SetExportFilter(request);
            }

            catch (Exception ex)
            {
                response.Result = MandayDashboardResult.Fail;
            }

            return response;
        }

        private async Task<IQueryable<int>> CommonInspectionData(MandayDashboardRequest request)
        {
            //fetch data based on request
            var bookingData = CommonInspSearch(request);

            //filter based on the inner factory country dropdown in the chart
            if (request.MandayCustomerSubCountryId.HasValue && request.MandayCustomerSubCountryId > 0)
            {
                var list = new[] { request.MandayCustomerSubCountryId.Value };
                //fetch the factory Id based on country
                var factoryList = await _supRepo.GetFactoryByCountryId(list.ToList());
                bookingData = bookingData.Where(x => x.FactoryId > 0 && factoryList.Contains(x.FactoryId.GetValueOrDefault()));
            }

            return bookingData.Select(x => x.Id);
        }
        private async Task<List<MandayCustomerChartData>> CommonReportCustomerChart(MandayDashboardRequest request)
        {
            var data = new List<MandayCustomerChartData>();
            try
            {
                request.ChartType = MandayDashboardChartType.Other;
                if (request.ServiceId == (int)Service.InspectionId)
                {
                    //fetch data based on request
                    var bookingIdList = await CommonInspectionData(request);

                    //get the report data
                    data = await _repo.GetCustomerInspReportsData(bookingIdList);
                }
            }
            catch (Exception ex)
            {
                data = null;
            }

            return data;
        }

        /// <summary>
        /// calculate the percentage 
        /// </summary>
        /// <param name="actualMandayCount"></param>
        /// <param name="compareMandaycount"></param>
        /// <returns></returns>
        private double GetMandayComparisonData(double actualMandayCount, double? compareMandaycount)
        {
            var compareMandayDefaultCount = compareMandaycount.GetValueOrDefault();
            double result = compareMandayDefaultCount > 0 ? Convert.ToDouble(actualMandayCount - compareMandayDefaultCount) / compareMandayDefaultCount : 0;
            return Math.Round(result * 100, 1);
        }
    }
}
