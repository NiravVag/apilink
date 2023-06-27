using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.Inspection;
using DTO.Manday;
using DTO.Schedule;
using DTO.UtilizationDashboard;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static DTO.Common.Static_Data_Common;

namespace BI
{
    public class UtilizationDashboardManager : ApiCommonData, IUtilizationDashboardManager
    {
        private readonly ISupplierManager _supManager = null;
        private readonly IKpiCustomRepository _kpiRepo = null;
        private readonly IUtilizationDashboardRepository _repo = null;
        private readonly IInspectionBookingRepository _inspRepo = null;
        private readonly IOfficeLocationRepository _officeRepo = null;
        private readonly ILocationRepository _locationRepo = null;
        private readonly IReferenceManager _referenceManager = null;

        public UtilizationDashboardManager(IKpiCustomRepository kpiRepo, IInspectionBookingRepository inspRepo, IUtilizationDashboardRepository repo,
            ISupplierManager supManager, IOfficeLocationRepository officeRepo, ILocationRepository locationRepo, IReferenceManager referenceManager)
        {
            _kpiRepo = kpiRepo;
            _inspRepo = inspRepo;
            _repo = repo;
            _supManager = supManager;
            _officeRepo = officeRepo;
            _locationRepo = locationRepo;
            _referenceManager = referenceManager;
        }

        public async Task<UtilizationResponse> GetCapacityUtilizationReport(UtilizationDashboardRequest request)
        {
            var response = new UtilizationResponse();
            var res = new List<UtilizationDashboard>();
            var graphDataList = new List<GradingData>();
            UtilizationGraphData graphResult = new UtilizationGraphData();

            try
            {
                //get the utilization data for the UI filter
                var resultCurrentYear = await GetUtilization(request);

                //Set the date range for previous year --  handle leap year
                if (request.ServiceDateFrom.Month == MonthData.FirstOrDefault(x => x.Value == Month.Feb.ToString()).Key && request.ServiceDateFrom.Day == Feb_29)
                {
                    request.ServiceDateFrom.Day = (int)Month.Feb; //sets the date to 28th - last day of the month if not leap year               
                }

                if (request.ServiceDateTo.Month == MonthData.FirstOrDefault(x => x.Value == Month.Feb.ToString()).Key && request.ServiceDateTo.Day == Feb_29)
                {
                    request.ServiceDateTo.Day = (int)Month.Feb; //sets the date to 28th - last day of the month if not leap year                  
                }
                request.ServiceDateFrom.Year = request.ServiceDateFrom.Year - 1;
                request.ServiceDateTo.Year = request.ServiceDateTo.Year - 1;

                //get the utilization data for the previous year with UI the same date range
                var resultLastYear = await GetUtilization(request);

                //get the officeId List
                var officeList = resultCurrentYear.Mandays.Select(x => x.LocationId).Distinct().ToList();

                //loop through each location to map the data
                foreach (var office in officeList)
                {
                    var holidayList = resultCurrentYear.HrHolidayList.Where(x => x.LocationId == office || x.LocationId == null).ToList();
                    var workingdays = GetWorkingDays(resultCurrentYear.SearchDateRange, holidayList);
                    // var outsourceCount = resultCurrentYear.QcList.Where(x => x.LocationId == office && x.EmployeeType == (int)StaffType.Outsource).Count();
                    //var empCount = resultCurrentYear.QcList.Where(x => x.LocationId == office).Count();
                    var utilizationCurrentYear = CalculateUtilizationRate(resultCurrentYear, office, workingdays);
                    var utilizationLastYear = CalculateUtilizationRate(resultLastYear, office, workingdays);

                    var permanentqcmanday = resultCurrentYear.Mandays.Where(x => x.LocationId == office && x?.StaffType == (int)StaffType.Permanent)?.Sum(x => x.ManDay);
                    var tempqcmanday = resultCurrentYear.Mandays.Where(x => x.LocationId == office && x?.StaffType != (int)StaffType.Permanent)?.Sum(x => x.ManDay);

                    var mapData = new UtilizationDashboard();
                    mapData.Office = resultCurrentYear.Mandays.Where(x => x.LocationId == office).Select(x => x.LocationName).FirstOrDefault();
                    mapData.HourMandDays = (int)permanentqcmanday.GetValueOrDefault(); //(int)resultCurrentYear.Mandays.Where(x => x.LocationId == office).Sum(x => x.ManDay);
                    mapData.WorkDays = workingdays;
                    mapData.Leaves = resultCurrentYear.LeaveCount.Where(x => x.LocationId == office).Select(x => x.NoOfDays).FirstOrDefault();
                    mapData.MaxPotential = workingdays * resultCurrentYear.QcList.Where(x => x.LocationId == office && x.EmployeeType == (int)StaffType.Permanent).Count();
                    mapData.OutsourceMandays = (int)tempqcmanday.GetValueOrDefault();
                    mapData.OutsourceMandaysPercentage = GetPercentage(tempqcmanday.GetValueOrDefault(), permanentqcmanday.GetValueOrDefault());
                    mapData.UtilizationRateCurrentYear = utilizationCurrentYear;
                    mapData.UtilizationRateLastYear = utilizationLastYear;
                    mapData.UtilizationPercentage = Math.Round(utilizationCurrentYear - utilizationLastYear, 2);

                    res.Add(mapData);
                }

                if (res == null || !res.Any())
                {
                    response.Data = null;
                    response.Result = UtilizationDashboardResult.NotFound;
                    return response;
                }

                response.Data = res;

                //Set the data for the Utilization graph - color, title, and value range
                var totalmanday = res.Sum(x => x.HourMandDays);
                var totalleave = res.Sum(x => x.Leaves);
                var totalavailablemanday = res.Sum(x => x.MaxPotential) - totalleave;
                graphResult.TotalUtilization = totalavailablemanday != 0 ? Math.Round(((double)totalmanday / totalavailablemanday) * 100, 2) : 0;
                graphResult.GradingData = UtilizationDashboardGraphData.ToList();
                response.GraphData = graphResult;

                response.Result = UtilizationDashboardResult.Success;
            }

            catch (Exception e)
            {
                throw e;
            }

            return response;
        }

        //Get the utilization data based on the request
        public async Task<UtilizationData> GetUtilization(UtilizationDashboardRequest request)
        {
            var response = new UtilizationData();
            DateTime serviceDateFrom = request.ServiceDateFrom.ToDateTime();
            DateTime serviceDateTo = request.ServiceDateTo.ToDateTime();

            //Get the inspection or audit data based on the request
            var inspectionOrAuditData = await CommonSearch(request);

            if (request.ServiceDateFrom != null && request.ServiceDateTo != null)
            {
                //get the individual dates from the UI date range filter
                List<DateTime> requestdaterange = Enumerable.Range(0, 1 + serviceDateTo.Subtract(serviceDateFrom).Days)
                         .Select(offset => serviceDateFrom.AddDays(offset)).ToList();

                response.SearchDateRange = requestdaterange;

                //Get the holiday List for the date rnage, office and country
                response.HrHolidayList = await _repo.GetHolidaysByRange(serviceDateFrom, serviceDateTo, request.OfficeIdList, request.CountryIdList);

                //Fetch the quotation man day for the bookings/ audits
                var idList = inspectionOrAuditData.Select(x => x.Id).Distinct().ToList();
                //response.Mandays = request.ServiceId == (int)Service.InspectionId ? await _repo.GetActualInspManDay(idList) : await GetAuditManDays(idList, request.ServiceId);
                response.Mandays = await GetManDays(idList, request.ServiceId, request);

                //get the QC details for the working days
                response.QcList = _repo.GetQCListByLocationForForecast(request.OfficeIdList, request.ServiceId).Result.ToList();

                //Fetch the leave data for the QC
                response.LeaveCount = await _repo.GetHrLeaves(serviceDateFrom, serviceDateTo, request.OfficeIdList, request.ServiceId);
            }
            return response;
        }

        //Get the number of working days
        private int GetWorkingDays(IEnumerable<DateTime> dateList, List<HrHoliday> holidayList)
        {
            int noOfWorkingDays = 0;
            List<DateTime> hoidayListDates = new List<DateTime>();

            //get the holiday dates - if it's a date range split it to individual dates
            foreach (var item in holidayList)
            {
                if (item.StartDate != item.EndDate)
                    hoidayListDates.AddRange(Enumerable.Range(0, 1 + item.EndDate.GetValueOrDefault().Subtract(item.StartDate.GetValueOrDefault()).Days)
                      .Select(offset => item.StartDate.GetValueOrDefault().AddDays(offset)).ToArray());

                else
                    hoidayListDates.Add(item.StartDate.GetValueOrDefault());
            }

            //Remove the holidays from the UI filter date range
            noOfWorkingDays = dateList.Except(hoidayListDates).ToList().Count();

            return noOfWorkingDays;
        }

        //get the bookings or audits
        private async Task<List<BookingAuditItems>> CommonSearch(UtilizationDashboardRequest request)
        {
            var idList = new List<int>();

            //Fetch all the bookings/ audits
            var data = request.ServiceId == (int)Service.InspectionId ? _repo.GetAllInspections() : _repo.GetAllAudits();

            //filter based on service date from and to 
            if (request.ServiceDateFrom != null && request.ServiceDateTo != null)
            {
                data = data.Where(x => x.ServiceDateTo <= request.ServiceDateTo.ToDateTime() && x.ServiceDateTo >= request.ServiceDateFrom.ToDateTime());
            }

            //filter based on officeId
            if (request.OfficeIdList != null && request.OfficeIdList.Any())
            {
                data = data.Where(x => request.OfficeIdList.Contains(x.OfficeId.GetValueOrDefault()));
            }

            //filter based on factory country
            if (request.CountryIdList != null && request.CountryIdList.Any())
            {
                //get the factory id with the country in the list
                var factoryList = await _supManager.GetFactoryByCountryId(request.CountryIdList);
                data = data.Where(x => x.FactoryId > 0 && factoryList.Contains(x.FactoryId.GetValueOrDefault()));
            }

            //filter based on status - inspected and validated (booking) or audited (audit)
            data = request.ServiceId == (int)Service.InspectionId ? data.Where(x => InspectedStatusList.Contains(x.StatusId)) : data.Where(x => x.StatusId == (int)AuditStatus.Audited);

            return await data.ToListAsync();
        }

        //Method to calculate the utilization rate 
        private double CalculateUtilizationRate(UtilizationData data, int locationId, int workingDays)
        {
            //Calculate the manday for the location
            var currentYearManday = data.Mandays.Where(x => x.LocationId == locationId && x?.StaffType == (int)StaffType.Permanent)?.Sum(x => x.ManDay);

            //maxpotential = noOfWorkingdays * No Of QC for the location
            var permEmpCount = data.QcList.Where(x => x.LocationId == locationId && x.EmployeeType == (int)StaffType.Permanent).Count();
            var currentYearMaxPotential = workingDays * permEmpCount;

            //Leave count for the location
            var currentYearLeaveCount = data.LeaveCount.Where(x => x.LocationId == locationId).Select(x => x.NoOfDays).FirstOrDefault();

            var totalAvailable = currentYearMaxPotential - currentYearLeaveCount;

            if (totalAvailable == 0)
                return 0;

            var res = ((double)currentYearManday.GetValueOrDefault() / (double)totalAvailable) * 100;
            return Math.Round(res, 2);
        }

        //Get the dropdown values to specify in the export file
        public async Task<MandayDashboardRequestExport> SetExportFilter(UtilizationDashboardRequest request)
        {
            var response = new MandayDashboardRequestExport();

            //top office filter
            if (request.OfficeIdList != null && request.OfficeIdList.Any())
            {
                var officeList = await _officeRepo.GetLocationListByIds(request.OfficeIdList);
                response.OfficeIdList = string.Join(", ", officeList.Select(x => x.Name));
            }

            //top factory country filter
            if (request.CountryIdList != null && request.CountryIdList.Any())
            {
                var countryList = await _locationRepo.GetCountryByIds(request.CountryIdList);
                response.CountryIdList = string.Join(", ", countryList.Select(x => x.Name));
            }

            //fetch all the services
            var serviceList = await _referenceManager.GetServices();

            response.Service = serviceList.Where(x => x.Id == request.ServiceId).Select(x => x.Name).FirstOrDefault();
            response.ServiceDateFrom = request.ServiceDateFrom == null ? "" : request.ServiceDateFrom.ToDateTime().ToString(StandardDateFormat);
            response.ServiceDateTo = request.ServiceDateTo == null ? "" : request.ServiceDateTo.ToDateTime().ToString(StandardDateFormat);

            return response;
        }

        //Method to fetch the details for export
        public async Task<UtilizationExportResponse> GetCapacityUtilizationReportExport(UtilizationDashboardRequest request)
        {
            var response = new UtilizationExportResponse();
            var res = new List<UtilizationDashboard>();

            try
            {
                //get the utilization data for the UI filter
                var resultCurrentYear = await GetUtilization(request);

                //Set the date range for previous year --  handle leap year
                if (request.ServiceDateFrom.Month == MonthData.FirstOrDefault(x => x.Value == Month.Feb.ToString()).Key && request.ServiceDateFrom.Day == Feb_29)
                {
                    request.ServiceDateFrom.Day = (int)Month.Feb; //sets the date to 28th - last day of the month if not leap year               
                }

                if (request.ServiceDateTo.Month == MonthData.FirstOrDefault(x => x.Value == Month.Feb.ToString()).Key && request.ServiceDateTo.Day == Feb_29)
                {
                    request.ServiceDateTo.Day = (int)Month.Feb; //sets the date to 28th - last day of the month if not leap year                  
                }
                request.ServiceDateFrom.Year = request.ServiceDateFrom.Year - 1;
                request.ServiceDateTo.Year = request.ServiceDateTo.Year - 1;

                //get the utilization data for the previous year with UI the same date range
                var resultLastYear = await GetUtilization(request);

                //get the officeId List
                var officeList = resultCurrentYear.Mandays.Select(x => x.LocationId).Distinct().ToList();

                //loop through each location to map the data
                foreach (var office in officeList)
                {
                    var holidayList = resultCurrentYear.HrHolidayList.Where(x => x.LocationId == office || x.LocationId == null).ToList();
                    var workingdays = GetWorkingDays(resultCurrentYear.SearchDateRange, holidayList);
                    // var outsourceCount = resultCurrentYear.QcList.Where(x => x.LocationId == office && x.EmployeeType == (int)StaffType.Outsource).Count();
                    //var empCount = resultCurrentYear.QcList.Where(x => x.LocationId == office).Count();
                    var utilizationCurrentYear = CalculateUtilizationRate(resultCurrentYear, office, workingdays);
                    var utilizationLastYear = CalculateUtilizationRate(resultLastYear, office, workingdays);

                    var permanentqcmanday = resultCurrentYear.Mandays.Where(x => x.LocationId == office && x?.StaffType == (int)StaffType.Permanent)?.Sum(x => x.ManDay);
                    var tempqcmanday = resultCurrentYear.Mandays.Where(x => x.LocationId == office && x?.StaffType != (int)StaffType.Permanent)?.Sum(x => x.ManDay);

                    var mapData = new UtilizationDashboard();
                    mapData.Office = resultCurrentYear.Mandays.Where(x => x.LocationId == office).Select(x => x.LocationName).FirstOrDefault();
                    mapData.HourMandDays = (int)permanentqcmanday.GetValueOrDefault(); //(int)resultCurrentYear.Mandays.Where(x => x.LocationId == office).Sum(x => x.ManDay);
                    mapData.WorkDays = workingdays;
                    mapData.Leaves = resultCurrentYear.LeaveCount.Where(x => x.LocationId == office).Select(x => x.NoOfDays).FirstOrDefault();
                    mapData.MaxPotential = workingdays * resultCurrentYear.QcList.Where(x => x.LocationId == office && x.EmployeeType == (int)StaffType.Permanent).Count();
                    mapData.OutsourceMandays = (int)tempqcmanday.GetValueOrDefault();
                    mapData.OutsourceMandaysPercentage = GetPercentage(tempqcmanday.GetValueOrDefault(), permanentqcmanday.GetValueOrDefault());
                    mapData.UtilizationRateCurrentYear = utilizationCurrentYear;
                    mapData.UtilizationRateLastYear = utilizationLastYear;
                    mapData.UtilizationPercentage = Math.Round(utilizationCurrentYear - utilizationLastYear, 2);
                    res.Add(mapData);
                }

                if (res == null || !res.Any())
                {
                    response.Data = null;
                    response.Result = UtilizationDashboardResult.NotFound;
                    return response;
                }

                response.Data = res;

                //Fetch the request filter values for export excel
                response.RequestFilter = await SetExportFilter(request);

                response.Result = UtilizationDashboardResult.Success;
            }

            catch (Exception e)
            {
                throw e;
            }

            return response;
        }

        private double GetPercentage(double outsourceManday, double totalManDay)
        {
            if (totalManDay == 0 || outsourceManday == 0)
                return 0;
            return Math.Round((outsourceManday / totalManDay) * 100);
        }


        //get the data for the man day year chart
        public async Task<MandayYearChartResponse> GetMandayYearChart(UtilizationDashboardRequest request)
        {
            var response = new MandayYearChartResponse();
            var mandayYear = new List<MandayYear>();
            var data = new List<MandayYearChartItem>();
            try
            {
                var bookingOrAuditData = await CommonSearch(request);
                var idList = bookingOrAuditData.Select(x => x.Id).Distinct().ToList();

                data = await GetMandayForYearChart(idList, request.ServiceId, request);

                if (data == null || !data.Any())
                {
                    response.Result = MandayDashboardResult.NotFound;
                    return response;
                }

                Random r = new Random();
                //get the per year man day count along with month wise data
                var items = data.GroupBy(p => p.Year, (key, _data) =>
                new MandayYearChart
                {
                    Year = key,
                    MandayCount = _data.Where(x => x.Year == key).Sum(x => x.MonthManDay),
                    MonthlyData = _data.Where(x => x.Year == key).ToList(),
                    Color = MandayDashboardColorList.GetValueOrDefault(r.Next(ColorRangeLow, ColorRangeHigh))
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
                throw ex;
            }
            if (response.Data == null || !response.Data.Any())
            {
                response.Result = MandayDashboardResult.NotFound;
            }
            return response;
        }

        //get details for the man day year details export
        public async Task<MandayYearExport> ExportMandayYearChart(UtilizationDashboardRequest request)
        {
            var response = new MandayYearExport();
            var data = new List<MandayYearChartItem>();
            try
            {
                //fetch the data based on filter
                var bookingOrAuditData = await CommonSearch(request);
                var idList = bookingOrAuditData.Select(x => x.Id).Distinct().ToList();

                data = await GetMandayForYearChart(idList, request.ServiceId, request);

                response.Data = data.OrderByDescending(x => x.MonthManDay);
                response.Total = data.Sum(x => x.MonthManDay);
                //get the dropdown data for the export file
                var result = await SetExportFilter(request);
                response.RequestFilters = result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        //Logic to get the actual mandays
        private async Task<List<QuotationManday>> GetManDays(List<int> auditIdList, int serviceId, UtilizationDashboardRequest request)
        {
            List<QuotationManday> mandayDataList = new List<QuotationManday>();

            //Fetch the Man day details from Au_Tran_Auditors 
            var inspectionOrAauditData = serviceId == (int)Service.AuditId ? await _repo.GetQuotationManDayAudit(auditIdList) : await _repo.GetMonthlyInspManDays(auditIdList, request);

            //loop to fetch the manday for each auditor, 1 auditor can be assigned to multiple audits on the same day
            foreach (var item in inspectionOrAauditData)
            {
                if (item.ServiceDateFrom == item.ServiceDateTo)
                {
                    QuotationManday mandayData = new QuotationManday();
                    mandayData.ServiceDate = item.ServiceDateTo;
                    mandayData.StaffId = item.StaffId;
                    mandayData.LocationId = item.LocationId;
                    mandayData.LocationName = item.LocationName;
                    mandayData.StaffType = item.StaffType;
                    mandayData.factoryId = item.factoryId;
                    mandayData.BookingId = item.BookingId;
                    mandayDataList.Add(mandayData);
                }

                //if from and to dates are different, each day will be 1 man day * number of auditors allocated
                else
                {
                    //get individual dates from the date range
                    var dateList = Enumerable.Range(0, 1 + item.ServiceDateTo.Subtract(item.ServiceDateFrom).Days)
                          .Select(offset => item.ServiceDateFrom.AddDays(offset)).ToList();

                    //loop the auditor for each date
                    foreach (var date in dateList)
                    {
                        QuotationManday mandayData = new QuotationManday();
                        mandayData.ServiceDate = date;
                        mandayData.StaffId = item.StaffId;
                        mandayData.LocationId = item.LocationId;
                        mandayData.LocationName = item.LocationName;
                        mandayData.StaffType = item.StaffType;
                        mandayData.factoryId = item.factoryId;
                        mandayData.BookingId = item.BookingId;
                        mandayDataList.Add(mandayData);

                    }
                }
            }

            //group the staff by service date and staffId - for each service date, 1 auditor can have a manday 1 even if he is assinged to more than 1 audits
            var items = mandayDataList.GroupBy(x => new { x.ServiceDate, x.StaffId, x.factoryId }, (key, _data) => new QuotationManday
            {
                ManDay = ActualMandayPerServiceDay, //it will be 1 as one auditor can have 1 manday per day
                StaffId = key.StaffId,
                LocationId = _data.Select(y => y.LocationId).FirstOrDefault(),
                LocationName = _data.Select(y => y.LocationName).FirstOrDefault(),
                ServiceDate = key.ServiceDate,
                StaffType = _data.Select(x => x.StaffType).FirstOrDefault()
            }).ToList();

            return items;
        }

        //fetch manday per year and month for the manday year graph
        private async Task<List<MandayYearChartItem>> GetMandayForYearChart(List<int> auditIds, int serviceId, UtilizationDashboardRequest request)
        {
            var mandayData = await GetManDays(auditIds, serviceId, request);

            return mandayData.GroupBy(x => new { x.ServiceDate.Year, x.ServiceDate.Month }, (key, group) => new MandayYearChartItem
            {
                Year = key.Year,
                Month = key.Month,
                MonthManDay = (int)group.Sum(x => x.ManDay),
                MonthName = MonthData.GetValueOrDefault(key.Month)
            }).ToList();
        }
    }
}
