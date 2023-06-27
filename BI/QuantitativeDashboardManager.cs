using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.Dashboard;
using DTO.FinanceDashboard;
using DTO.Invoice;
using DTO.ManagementDashboard;
using DTO.Manday;
using DTO.ProductManagement;
using DTO.QuantitativeDashboard;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using static DTO.Common.Static_Data_Common;

namespace BI
{
    public class QuantitativeDashboardManager : ApiCommonData, IQuantitativeDashboardManager
    {
        private readonly IQuantitativeDashboardRepository _repo = null;
        private readonly IDashboardManager _dashboardManager = null;
        private readonly IDashboardRepository _dashboardRepo = null;
        private readonly IManagementDashboardRepository _mgmtDashboardRepo = null;
        private readonly ISupplierManager _supManager = null;
        private readonly ICustomerManager _cusManager = null;
        private readonly ILocationRepository _locationRepo = null;
        private readonly IManagementDashboardManager _mgmtDashboardManager = null;
        private readonly ICustomerBrandManager _cusBrandManager = null;
        private readonly ICustomerBuyerManager _cusBuyerManager = null;
        private readonly ICustomerCollectionManager _cusCollectionManager = null;
        private readonly ICustomerDepartmentManager _cusDeptManager = null;
        private readonly IProductManagementManager _productManager = null;
        private readonly IExchangeRateManager _exchangeRateManager = null;
        private readonly ISharedInspectionManager _sharedInspection = null;
        private readonly ICustomerProductManager _customerProductManager = null;
        private readonly IReferenceManager _referenceManager=null;
        private readonly DashBoardMap DashBoardMap = null;

        public QuantitativeDashboardManager(IQuantitativeDashboardRepository repo, IDashboardManager dashboardManager,
            IDashboardRepository dashboardRepo,
            IManagementDashboardRepository mgmtDashboardRepo,
            ISupplierManager supManager,
            ICustomerManager cusManager,
            ILocationRepository locationRepo,
            IManagementDashboardManager mgmtDashboardManager,
            ICustomerBrandManager cusBrandManager,
            ICustomerBuyerManager cusBuyerManager,
            ICustomerCollectionManager cusCollectionManager,
            ICustomerDepartmentManager cusDeptManager,
            IProductManagementManager productManager,
            IExchangeRateManager exchangeRateManager, ISharedInspectionManager sharedInspection,
            ICustomerProductManager customerProductManager,IReferenceManager referenceManager)
        {
            _repo = repo;
            _dashboardManager = dashboardManager;
            _dashboardRepo = dashboardRepo;
            _mgmtDashboardRepo = mgmtDashboardRepo;
            _supManager = supManager;
            _cusManager = cusManager;
            _locationRepo = locationRepo;
            _mgmtDashboardManager = mgmtDashboardManager;
            _cusBrandManager = cusBrandManager;
            _cusBuyerManager = cusBuyerManager;
            _cusCollectionManager = cusCollectionManager;
            _cusDeptManager = cusDeptManager;
            _productManager = productManager;
            _exchangeRateManager = exchangeRateManager;
            _sharedInspection = sharedInspection;
            _customerProductManager = customerProductManager;
            _referenceManager = referenceManager;
            DashBoardMap = new DashBoardMap();
        }

        private async Task<List<BookingDetail>> GetBookingDataByRequest(QuantitativeDashboardRequest request)
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

        public async Task<QuantitativeDashboardResponse> GetQuantitativeDashboardSummary(QuantitativeDashboardRequest request)
        {
            var response = new QuantitativeDashboardResponse();
            var result = new QuantitativeDashboardItem();

            if (request != null)
            {
                var bookingData = await GetBookingDataByRequest(request);
                //fetch the booking Ids
                var bookingIds = bookingData.Select(x => x.InspectionId).ToList();

                if (bookingIds.Any())
                {
                    //get the total manday for the booking list
                    result.TotalManday = await _dashboardRepo.GetInspectionManDays(bookingIds).AsNoTracking().SumAsync(x=>x.MandayCount) ?? 0;

                    result.TotalInspCount = bookingIds.Count;

                    result.TotalFactoryCount = bookingData.Select(x => x.FactoryId).Distinct().Count();

                    int prodReportcount = await _mgmtDashboardRepo.GetReportCount(bookingIds, InspectedStatusList);
                    int containerReportCount = await _mgmtDashboardRepo.GetReportCountForContainers(bookingIds, InspectedStatusList);
                    result.TotalReportCount = prodReportcount + containerReportCount;

                    result.TotalVendorCount = bookingData.Select(x => x.SupplierId).Distinct().Count();

                    result.TotalQcCount = await _repo.GetQcCount(bookingIds);

                }
                response.InspectionIdList = bookingIds;
                response.Data = result;
                response.Result = QuantitativeDashboardResult.Success;
            }

            if (response.Data == null)
            {
                response.Result = QuantitativeDashboardResult.NotFound;
            }

            return response;
        }
        public async Task<QuantitativeDashboardResponse> GetAllQuantitativeDashboardSummary(QuantitativeDashboardFilterRequest request)
        {
            if (request == null)
                return new QuantitativeDashboardResponse() { Result = QuantitativeDashboardResult.RequestNotCorrectFormat };

            var inspectionQuery = _sharedInspection.GetAllInspectionQuery();
            var inspectionQueryRequest = _sharedInspection.GetQuantitativeDashBoardInspectionQueryRequestMap(request);
            var bookingData = _sharedInspection.GetInspectionQuerywithRequestFilters(inspectionQueryRequest, inspectionQuery);

            if (CheckDate(request.ServiceDateTo) && CheckDate(request.ServiceDateFrom))
            {
                bookingData = bookingData.Where(x => x.ServiceDateTo <= request.ServiceDateTo.ToDateTime() && x.ServiceDateTo >= request.ServiceDateFrom.ToDateTime());
            }

            //get booking ids
            var bookingIds = bookingData.Select(x => x.Id);


            var response = new QuantitativeDashboardResponse();
            var result = new QuantitativeDashboardItem();

            //get the total manday for the booking list
            result.TotalManday = await _dashboardRepo.GetInspectionManDaysQuery(bookingIds);

            result.TotalInspCount = await bookingIds.CountAsync();

            result.TotalFactoryCount = await bookingData.Select(x => x.FactoryId).Distinct().CountAsync();

            result.TotalVendorCount = await bookingData.Select(x => x.SupplierId).Distinct().CountAsync();

            result.TotalQcCount = await _repo.GetQueryableQcCount(bookingIds);

            result.TotalReportCount = await _mgmtDashboardRepo.GetReportCountbyBookingQuery(bookingIds, InspectedStatusList);

            response.Data = result;
            response.Result = QuantitativeDashboardResult.Success;

            if (response.Data == null)
            {
                response.Result = QuantitativeDashboardResult.NotFound;
            }

            return response;
        }

        //get the data for the man day year chart
        public async Task<QuantitativeMandayYearChartResponse> GetMandayYearChart(QuantitativeDashboardFilterRequest request)
        {
            var response = new QuantitativeMandayYearChartResponse();
            var mandayYear = new List<MandayYear>();

            var fromDate = request.ServiceDateFrom;
            var toDate = request.ServiceDateTo;

            //assign the dates and get manday list
            var data = await AssignDatesGetMandayData(request);

            if (data.CurrentMandayList == null || !data.CurrentMandayList.Any())
            {
                response.Result = QuantitativeDashboardResult.NotFound;
                return response;
            }
            IEnumerable<QuantitativeMandayYearChart> items;
            //= Enumerable.Empty<QuantitativeMandayYearChart>();

            //if year not same year, then we should pass data.PreviousMandayList to calc the percentage
            if (fromDate.Year != toDate.Year)
            {
                items = MandayYearGroupByData(data.CurrentMandayList, data.PreviousMandayList);
            }
            else
            {
                items = MandayYearGroupByData(data.CurrentMandayList, null);
            }

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
            response.Result = QuantitativeDashboardResult.Success;

            if (response.Data == null || !response.Data.Any())
            {
                response.Result = QuantitativeDashboardResult.NotFound;
            }

            return response;
        }

        /// <summary>
        /// group by the data by year and calc percentage
        /// </summary>
        /// <param name="mandayList"></param>
        /// <param name="percentageMandayList"></param>
        /// <returns></returns>
        public IEnumerable<QuantitativeMandayYearChart> MandayYearGroupByData(List<MandayYearChartItem> mandayList, List<MandayYearChartItem> percentageMandayList)
        {
            var index = 1;

            percentageMandayList = percentageMandayList ?? mandayList;

            //get the per year man day count along with month wise data and get percentage 
            return mandayList.GroupBy(p => p.Year, (key, _data) =>
            new QuantitativeMandayYearChart
            {
                Year = key,
                MandayCount = _data.Sum(x => x.MonthManDay),
                MonthlyData = _data.ToList(),
                Percentage = GetPercentage(_data.Sum(x => x.MonthManDay), percentageMandayList.Where(x => x.Year == key - 1).Sum(x => x.MonthManDay)),
                Color = MandayDashboardColorList.GetValueOrDefault(index++)
            }).OrderByDescending(x => x.Year).Take(QuantitativeDashboardMandayYearCount);
        }

        /// <summary>
        /// get percentage value of last year
        /// </summary>
        /// <param name="curManday"></param>
        /// <param name="lastYearManday"></param>
        /// <returns></returns>
        private int GetPercentage(double curManday, double lastYearManday)
        {
            int percentage = 0;

            if (lastYearManday > 0)
            {
                double res = curManday - lastYearManday;
                double percentageCalc = 0;

                //we are taking greater value to divide and get correct percentage
                if (curManday > lastYearManday)
                {
                    percentageCalc = curManday > 0 ? res / curManday : 0;
                }
                else
                {
                    percentageCalc = lastYearManday > 0 ? res / lastYearManday : 0;
                }
                percentage = (int)Math.Round(percentageCalc * 100);
            }
            else
            {
                percentage = 0;
            }
            return percentage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<QuantitativeMandayYearExport> ExportMandayChart(QuantitativeDashboardFilterRequest request)
        {
            var response = new QuantitativeMandayYearExport();

            response.RequestFilters = await SetDashboardExportFilter(request);

            var mandayList = await AssignDatesGetMandayData(request);

            response.Data = mandayList.CurrentMandayList;

            response.Total = response.Data.Sum(x => x.MonthManDay);

            return response;

        }

        /// <summary>
        /// get manday data based on filters and year wise filters
        /// </summary>
        /// <param name="dateList"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<List<MandayYearChartItem>> GetMandayData(List<DateList> dateList, QuantitativeDashboardFilterRequest request)
        {
            var mandayYearItem = new List<MandayYearChartItem>();

            //loop the date list to fetch the manday count 
            foreach (var dateItem in dateList)
            {
                request.ServiceDateFrom = dateItem.FromDate;
                request.ServiceDateTo = dateItem.ToDate;
                var bookingIds = GetQueryableBookingIdList(request);

                mandayYearItem.AddRange(await _dashboardRepo.GetQueryableMonthlyInspManDays(bookingIds));

            }
            mandayYearItem = DashBoardMap.MapMonthlyInspManDays(mandayYearItem);

            return mandayYearItem;

        }
        /// <summary>
        /// assign the previous year date and get previous date for percentage and get manday data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<MandayListCurrenctPrevious> AssignDatesGetMandayData(QuantitativeDashboardFilterRequest request)
        {
            var fromDate = request.ServiceDateFrom;
            var toDate = request.ServiceDateTo;
            var previousDateList = new List<DateList>();

            var mandayItem = new MandayListCurrenctPrevious();

            var currentList = AssignDates(fromDate, toDate);

            //if not same year, we are making previous year with current list of dates 
            if (fromDate.Year != toDate.Year)
            {
                previousDateList = AssignPreviousYearDate(currentList);
            }

            //get manday count list by year and month
            mandayItem.CurrentMandayList = await GetMandayData(currentList, request);

            //get manday count list by year and month
            mandayItem.PreviousMandayList = await GetMandayData(previousDateList, request);

            return mandayItem;
        }

        /// <summary>
        /// Assign the from date and to date based on year 
        /// </summary>
        /// <param name="request"></param>
        private List<DateList> AssignDates(DateObject ServiceDateFrom, DateObject ServiceDateTo)
        {
            var dateListbyYear = new List<DateList>();

            for (var year = ServiceDateTo.Year; year >= (ServiceDateTo.Year - NumberTwo); year--)
            {
                if (ServiceDateTo.Year == year)
                {
                    //if from and to date same years we are getting same date for current year
                    if (ServiceDateFrom.Year == ServiceDateTo.Year)
                    {
                        //same year date
                        dateListbyYear.Add(new DateList()
                        {
                            FromDate = ServiceDateFrom,
                            ToDate = ServiceDateTo
                        });
                    }
                    //different year dates
                    else
                    {
                        //currenct year of exact start year date
                        dateListbyYear.Add(new DateList()
                        {
                            FromDate = new DateObject() { Year = ServiceDateTo.Year, Day = NumberOne, Month = NumberOne },
                            ToDate = ServiceDateTo
                        });
                    }
                }
                else
                {
                    //currenct year of previous full years date
                    dateListbyYear.Add(new DateList()
                    {
                        FromDate = new DateObject() { Year = year, Day = NumberOne, Month = NumberOne },
                        ToDate = new DateObject() { Year = year, Day = DecLastDay, Month = DecLastMonth }
                    });
                }
            }
            return dateListbyYear;
        }

        /// <summary>
        /// request has list of dates, we are converting the previous year to the list
        /// </summary>
        /// <param name="dateListbyYear"></param>
        /// <returns></returns>
        private List<DateList> AssignPreviousYearDate(List<DateList> dateListbyYear)
        {
            var previousYeardateList = new List<DateList>();

            var index = 0;

            //get the previous year of the (dateListbyYear)date list to get the manday count for percentage calc
            foreach (var _dateItem in dateListbyYear)
            {
                if (index == 0)
                {
                    //currenct year of exact previous year date
                    previousYeardateList.Add(new DateList()
                    {
                        FromDate = new DateObject() { Year = _dateItem.FromDate.Year - NumberOne, Day = _dateItem.FromDate.Day, Month = _dateItem.FromDate.Month },
                        ToDate = new DateObject() { Year = _dateItem.ToDate.Year - NumberOne, Day = _dateItem.ToDate.Day, Month = _dateItem.ToDate.Month }
                    });
                }
                else
                {
                    //whole year date for previous years
                    previousYeardateList.Add(new DateList()
                    {
                        FromDate = new DateObject() { Year = _dateItem.FromDate.Year - NumberOne, Day = NumberOne, Month = NumberOne },
                        ToDate = new DateObject() { Year = _dateItem.ToDate.Year - NumberOne, Day = DecLastDay, Month = DecLastMonth }
                    });
                }
                index++;
            }
            return previousYeardateList;
        }


        /// <summary>
        /// Get the dropdown values to specify in the export file 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<QuantitativeDashboardRequestExport> SetDashboardExportFilter(QuantitativeDashboardFilterRequest request)
        {
            var response = new QuantitativeDashboardRequestExport();

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


            //selected product name  list\            
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

        public async Task<QuantitativeDashboardCommonResponse> GetMandayByCountry(QuantitativeDashboardFilterRequest request)
        {
            var dataList = new List<QuantitativeDashboardCommonItem>();

            var mandayCountryData = await MandayCountryYearData(request);

            Random r = new Random();

            Parallel.ForEach(mandayCountryData.CurrentYearList, item =>
            {
                dataList.Add(new QuantitativeDashboardCommonItem
                {
                    Name = item.CountryName,
                    Count = item.Manday.GetValueOrDefault(),
                    Color = CommonDashboardColor.GetValueOrDefault(r.Next(1, 20)),
                    Percentage = GetPercentage(item.Manday.GetValueOrDefault(), mandayCountryData.LastYearList.Where(x => x.CountryId == item.CountryId).Sum(x => x.Manday.GetValueOrDefault()))
                });
            });

            // Converts the all capital country names to Pascal Case (CHINA - China)
            TextInfo myTI = new CultureInfo(EnglishUS, false).TextInfo;
            foreach (var item in dataList)
            {
                var str = myTI.ToTitleCase(myTI.ToLower(item.Name));
                item.Name = str;
            }

            if (dataList == null || !dataList.Any())
            {
                return new QuantitativeDashboardCommonResponse { Result = QuantitativeDashboardResult.NotFound };
            }

            return new QuantitativeDashboardCommonResponse
            {
                Data = dataList,
                Result = QuantitativeDashboardResult.Success
            };
        }

        /// <summary>
        /// get manday by country based request and previous year
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<MandayCountryYearData> MandayCountryYearData(QuantitativeDashboardFilterRequest request)
        {
            var response = new MandayCountryYearData();
            var bookingIdList = GetQueryableBookingIdList(request);


            var _previousRequest = new QuantitativeDashboardFilterRequest()
            {
                CustomerId = request.CustomerId,
                ServiceDateFrom = Static_Data_Common.GetCustomDate(request.ServiceDateFrom.ToDateTime().AddYears(-1)),
                ServiceDateTo = Static_Data_Common.GetCustomDate(request.ServiceDateTo.ToDateTime().AddYears(-1)),
                SelectedBrandIdList = request.SelectedBrandIdList,
                SelectedBuyerIdList = request.SelectedBuyerIdList,
                SelectedCollectionIdList = request.SelectedCollectionIdList,
                SelectedCountryIdList = request.SelectedCountryIdList,
                SelectedDeptIdList = request.SelectedDeptIdList,
                SupplierId = request.SupplierId
            };
            var bookingIdsLastYear = GetQueryableBookingIdList(request);

            response.CurrentYearList = await _repo.GetQueryableFactoryCountry(bookingIdList);

            response.LastYearList = await _repo.GetQueryableFactoryCountry(bookingIdsLastYear);

            return response;
        }

        public async Task<QuantitativeCommonExport> ExportMandayCountryChart(QuantitativeDashboardFilterRequest request)
        {
            var response = new QuantitativeCommonExport();
            var dataList = new List<QuantitativeDashboardCommonItem>();

            var mandayCountryData = await MandayCountryYearData(request);

            if (!mandayCountryData.CurrentYearList.Any())
            {
                return new QuantitativeCommonExport();
            }

            Parallel.ForEach(mandayCountryData.CurrentYearList, item =>
            {
                dataList.Add(new QuantitativeDashboardCommonItem
                {
                    Name = item.CountryName,
                    Count = item.Manday.GetValueOrDefault(),
                    Percentage = GetPercentage(item.Manday.GetValueOrDefault(), mandayCountryData.LastYearList.Where(x => x.CountryId == item.CountryId).Sum(x => x.Manday.GetValueOrDefault())),
                    LastYearCount = mandayCountryData.LastYearList.Where(x => x.CountryId == item.CountryId).Sum(x => x.Manday.GetValueOrDefault())
                });
            });

            if (!dataList.Any())
            {
                return new QuantitativeCommonExport();
            }

            response.Data = dataList;
            response.Total = dataList.Sum(x => x.Count);
            response.RequestFilters = await SetDashboardExportFilter(request);

            return response;
        }

        //fetch the total invoice amount
        public async Task<TurnOverDataResponse> GetTurnOverSummary(QuantitativeDashboardFilterRequest request)
        {
            var response = new TurnOverData();
            var res = new TurnOverDataItem();
            var list = new List<QuantitativeDashboardCommonItem>();
            Dictionary<int, double> dicexrate = new Dictionary<int, double>();
            var Exchangecurrency = new List<int>();

            var bookingIdList = GetQueryableBookingIdList(request);

            var currentyeardata = await _repo.GetQueryableTotalInvoiceFeeData(bookingIdList);
            if (currentyeardata != null && currentyeardata.Any())
                Exchangecurrency.AddRange(currentyeardata.Select(x => x.CurrencyId.GetValueOrDefault()).Distinct());
            var currentExtraFee = await _repo.GetQueryableExtraFeeData(bookingIdList);
            if (currentExtraFee != null && currentExtraFee.Any())
                Exchangecurrency.AddRange(currentExtraFee.Select(x => x.CurrencyId.GetValueOrDefault()).Distinct());

            request.ServiceDateFrom = Static_Data_Common.GetCustomDate(request.ServiceDateFrom.ToDateTime().AddYears(-1));
            request.ServiceDateTo = Static_Data_Common.GetCustomDate(request.ServiceDateTo.ToDateTime().AddYears(-1));
            var bookingIdsLastYear = GetQueryableBookingIdList(request);

            var lastYearData = await _repo.GetQueryableTotalInvoiceFeeData(bookingIdsLastYear);
            var lastYearExtraFee = await _repo.GetQueryableExtraFeeData(bookingIdsLastYear);
            if (lastYearData != null && lastYearData.Any())
                Exchangecurrency.AddRange(lastYearData.Select(x => x.CurrencyId.GetValueOrDefault()).Distinct());
            if (lastYearExtraFee != null && lastYearExtraFee.Any())
                Exchangecurrency.AddRange(lastYearExtraFee.Select(x => x.CurrencyId.GetValueOrDefault()).Distinct());

            dicexrate.Add((int)CurrencyMaster.USD, 1);

            var exchangeList = new List<ExchangeCurrency>();
            foreach (var item in Exchangecurrency)
            {
                if (!exchangeList.Select(x => x.Currency).Contains(item))
                {
                    exchangeList.Add(new ExchangeCurrency()
                    {
                        TargetCurrency = (int)CurrencyMaster.USD,
                        Currency = item
                    });
                }
            }

            exchangeList = exchangeList.Distinct().ToList();

            dicexrate = await _exchangeRateManager.GetExchangeRateList(exchangeList, DateTime.Now, ExhangeRateTypeEnum.ExpenseClaim);

            foreach (var item in currentyeardata)
            {
                if (dicexrate.TryGetValue(item.CurrencyId.GetValueOrDefault(), out double exrate))
                {
                    item.TotalInvoiceFee = item.TotalInvoiceFee * exrate;
                }
            }
            foreach (var item in currentExtraFee)
            {
                if (dicexrate.TryGetValue(item.CurrencyId.GetValueOrDefault(), out double exrate))
                {
                    item.TotalInvoiceFee = item.TotalInvoiceFee * exrate;
                }
            }
            foreach (var item in lastYearData)
            {
                if (dicexrate.TryGetValue(item.CurrencyId.GetValueOrDefault(), out double exrate))
                {
                    item.TotalInvoiceFee = item.TotalInvoiceFee * exrate;
                }
            }
            foreach (var item in lastYearExtraFee)
            {
                if (dicexrate.TryGetValue(item.CurrencyId.GetValueOrDefault(), out double exrate))
                {
                    item.TotalInvoiceFee = item.TotalInvoiceFee * exrate;
                }
            }

            res.TotalTurnOver = Math.Round(currentyeardata.Sum(x => x.TotalInvoiceFee.GetValueOrDefault()) + currentExtraFee.Sum(y => y.TotalInvoiceFee.GetValueOrDefault()), 2);//add extra fee

            res.CustomerTurnOver = Math.Round(currentyeardata.Where(x => x.InvoiceTo == (int)InvoiceTo.Customer).Sum(x => x.TotalInvoiceFee.GetValueOrDefault())
            + currentExtraFee.Where(x => x.InvoiceTo == (int)InvoiceTo.Customer).Sum(x => x.TotalInvoiceFee.GetValueOrDefault()), 2);//add extra fee

            res.SupplierTurnOver = Math.Round(currentyeardata.Where(x => x.InvoiceTo == (int)InvoiceTo.Supplier).Sum(x => x.TotalInvoiceFee.GetValueOrDefault())
            + currentExtraFee.Where(x => x.InvoiceTo == (int)InvoiceTo.Supplier).Sum(x => x.TotalInvoiceFee.GetValueOrDefault()), 2);//add extra fee

            res.TotalTurnOverPercentage = GetPercentage(res.TotalTurnOver, (lastYearData.Sum(x => x.TotalInvoiceFee.GetValueOrDefault()) + lastYearExtraFee.Sum(x => x.TotalInvoiceFee.GetValueOrDefault())));

            res.CustomerTurnOverPercentage = GetPercentage(res.CustomerTurnOver, (lastYearData.Where(x => x.InvoiceTo == (int)InvoiceTo.Customer).Sum(x => x.TotalInvoiceFee.GetValueOrDefault())
                + lastYearExtraFee.Where(x => x.InvoiceTo == (int)InvoiceTo.Customer).Sum(x => x.TotalInvoiceFee.GetValueOrDefault())));

            res.SupplierTurnOverPercentage = GetPercentage(res.SupplierTurnOver, (lastYearData.Where(x => x.InvoiceTo == (int)InvoiceTo.Supplier).Sum(x => x.TotalInvoiceFee.GetValueOrDefault())
                + lastYearExtraFee.Where(x => x.InvoiceTo == (int)InvoiceTo.Supplier).Sum(x => x.TotalInvoiceFee.GetValueOrDefault())));

            response.TurnOverDataItem = res;

            var serviceTypeList = await _repo.GetQueryableServiceTypeData(bookingIdList);

            if (currentyeardata == null || !currentyeardata.Any() || !serviceTypeList.Any())
            {
                return new TurnOverDataResponse { Result = QuantitativeDashboardResult.NotFound };
            }

            var _totalcount = serviceTypeList.Sum(x => x.Count);

            Random r = new Random();

            // Parallel.ForEach(serviceTypeList, item =>
            //{
            //var random = 1;
            for (int i = 0; i < serviceTypeList.Count; i++)
            {

                var item = serviceTypeList[i];
                list.Add(new QuantitativeDashboardCommonItem
                {
                    Name = item.ServiceTypeName,
                    Count = Convert.ToInt32(Math.Round(((Double)item.Count / (Double)_totalcount) * 100)),
                    //Color = CommonDashboardColor.GetValueOrDefault(random),
                });
            }
            //});

            response.ServiceTypeChartData = list;

            return new TurnOverDataResponse
            {
                Data = response,
                Result = QuantitativeDashboardResult.Success
            };
        }

        public async Task<QuantitativeCommonExport> ExportTurnOverByServiceTypeChart(QuantitativeDashboardFilterRequest request)
        {
            var response = new QuantitativeCommonExport();
            var dataList = new List<QuantitativeDashboardCommonItem>();

            var bookingIdList = GetQueryableBookingIdList(request);

            var data = await _repo.GetQueryableServiceTypeData(bookingIdList);

            if (data == null || !data.Any())
            {
                return new QuantitativeCommonExport();
            }

            var _totalCount = data.Sum(x => x.Count);

            Parallel.ForEach(data, item =>
            {
                dataList.Add(new QuantitativeDashboardCommonItem
                {
                    Name = item.ServiceTypeName,
                    Count = item.Count,
                    Percentage = Math.Round((((double)item.Count / (double)_totalCount) * 100)),
                });
            });

            response.Data = dataList;
            response.Total = _totalCount;
            response.RequestFilters = await SetDashboardExportFilter(request);

            return response;
        }

        //get the service type dashboard data by inspection
        public async Task<QuantitativeDashboardCommonResponse> GetServiceTypeChart(QuantitativeDashboardFilterRequest request)
        {
            var bookingIdList = GetQueryableBookingIdList(request);
            var res = await _mgmtDashboardManager.GetServiceTypeChartByQuery(bookingIdList);

            if (res == null || res.Data == null || !res.Data.Any())
            {
                return new QuantitativeDashboardCommonResponse { Result = QuantitativeDashboardResult.NotFound };
            }

            return new QuantitativeDashboardCommonResponse
            {
                Data = res.Data.ConvertAll(x => new QuantitativeDashboardCommonItem
                {
                    Name = x.StatusName,
                    Color = x.StatusColor,
                    Count = x.TotalCount
                }).ToList(),
                Result = QuantitativeDashboardResult.Success

            };
        }

        public async Task<QuantitativeCommonExport> ExportInspectionByServiceTypeChart(QuantitativeDashboardFilterRequest request)
        {
            var response = new QuantitativeCommonExport();


            //fetch the service type data
            var res = await GetServiceTypeChart(request);

            if (res.Data != null && res.Data.Any())
            {
                response.Data = res.Data.Select(x => new QuantitativeDashboardCommonItem
                {
                    Count = x.Count,
                    Name = x.Name
                }).ToList();

                response.Total = res.Data.Sum(x => x.Count);
            }
            //get the request details to be input in the excel
            response.RequestFilters = await SetDashboardExportFilter(request);

            return response;
        }

        /// <summary>
        /// country quantity list 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<OrderQuantityCountChartExport> GetBookingQuantityDashboardExport(QuantitativeDashboardFilterRequest request)
        {
            var response = new OrderQuantityCountChartExport();

            response.RequestFilters = await SetDashboardExportFilter(request);

            var mandayList = await AssignDatesGetOrderQuantityData(request);

            response.Data = mandayList.CurrentOrderQtyList;

            response.Total = response.Data.Sum(x => x.MonthOrderQuantity);

            response.Result = QuantitativeDashboardResult.Success;

            return response;
        }

        /// <summary>
        /// get product category list with count
        /// </summary>
        /// <param name="inspectionIdList"></param>
        /// <returns></returns>
        public async Task<ProductCategoryDashboardResponse> GetProductCategoryList(ProductCategoryChartRequest request)
        {
            var response = new ProductCategoryDashboardResponse();
            var bookingIdList = GetQueryableBookingIdList(request.SearchRequest);

            if (request.SearchRequest != null && request.SearchRequest.SelectedProdCategoryIdList != null && request.SearchRequest.SelectedProdCategoryIdList.Any())
            {
                response.Data = await _repo.GetQueryableProductSubCategoryDashboard(bookingIdList, request.SearchRequest.SelectedProdCategoryIdList);
                if (response.Data == null || !response.Data.Any())
                {
                    response.Result = ManagementDashboardResult.NotFound;
                }
                else
                {
                    response.Result = ManagementDashboardResult.Success;
                }

            }

            else
            {

                response = await _mgmtDashboardManager.ProductCategoryDashboardSearchbyQuery(bookingIdList);
            }

            return response;
        }

        /// <summary>
        /// get prodcut category list with count and top filters details for export
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<QuantitativeProductCategoryChartExport> ProductCategoryListExport(QuantitativeDashboardFilterRequest quantitativerequest)
        {
            var response = new QuantitativeProductCategoryChartExport();
            var data = new List<ProductCategoryDashboardExportItem>();

            //get booking details by request
            var bookingIdList = GetQueryableBookingIdList(quantitativerequest);

            //fetch the product category details
            var res = _repo.GetQueryableProductCategoryDashboardExport(bookingIdList);

            if (quantitativerequest.SelectedProdCategoryIdList != null && quantitativerequest.SelectedProdCategoryIdList.Any())
            {
                data = res.Where(x => x.Id.HasValue && quantitativerequest.SelectedProdCategoryIdList.Contains(x.Id.Value)).ToList();
            }

            else
            {
                data = res.ToList();
            }



            if (res != null && res.Any())
            {
                response.Data = data;

                //get the request details to be input in the excel
                response.RequestFilters = await SetDashboardExportFilter(quantitativerequest);
                response.Result = QuantitativeDashboardResult.Success;
                return response;
            }

            return new QuantitativeProductCategoryChartExport() { Result = QuantitativeDashboardResult.NotFound };
        }

        public async Task<ProductCategoryResponse> GetProductCategoryList()
        {
            return _productManager.GetProductCategorySummary();
        }

        /// <summary>
        /// get the data for the order quantity year chart
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<PiecesInspectedChartResponse> GetBookingQuantityData(QuantitativeDashboardFilterRequest request)
        {
            var response = new PiecesInspectedChartResponse();
            var mandayYear = new List<MandayYear>();

            //Assign the dates and get order qty list
            var data = await AssignDatesGetOrderQuantityData(request);

            if (data.CurrentOrderQtyList == null || !data.CurrentOrderQtyList.Any())
            {
                response.Result = QuantitativeDashboardResult.NotFound;
                return response;
            }

            var index = 1;

            //if year not same year, then we should pass data.PreviousMandayList to calc the percentage
            response.Data = data.CurrentOrderQtyList.GroupBy(p => p.Year, (key, _data) =>
            new OrderQuantityYearChart
            {
                Year = key,
                OrderCount = _data.Sum(x => x.MonthOrderQuantity),
                MonthlyData = _data.ToList(),
                Color = MandayDashboardColorList.GetValueOrDefault(index++)
            }).OrderByDescending(x => x.Year).Take(QuantitativeDashboardMandayYearCount);

            //get the x axis month data for the chart
            for (int i = 0; i < 12; i++)
            {
                MandayYear res = new MandayYear();
                res.year = DateTime.Now.Year;
                res.month = i;
                mandayYear.Add(res);
            }
            response.MonthYearXAxis = mandayYear;
            response.Result = QuantitativeDashboardResult.Success;

            if (response.Data == null || !response.Data.Any())
            {
                response.Result = QuantitativeDashboardResult.NotFound;
            }

            return response;
        }

        /// <summary>
        /// assign the previous year date and get previous date get order data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<OrderQtyListCurrenctPrevious> AssignDatesGetOrderQuantityData(QuantitativeDashboardFilterRequest request)
        {
            var fromDate = request.ServiceDateFrom;
            var toDate = request.ServiceDateTo;
            var previousDateList = new List<DateList>();

            var mandayItem = new OrderQtyListCurrenctPrevious();

            var currentList = AssignDates(fromDate, toDate);

            //if not same year, we are making previous year with current list of dates 
            if (fromDate.Year != toDate.Year)
            {
                previousDateList = AssignPreviousYearDate(currentList);
            }

            //get order count list by year and month
            mandayItem.CurrentOrderQtyList = await GetOrderQuantityData(currentList, request);

            //get order count list by year and month
            mandayItem.PreviousOrderQtyList = await GetOrderQuantityData(previousDateList, request);

            return mandayItem;
        }

        /// <summary>
        /// get order quantity data based on filters and year wise filters
        /// </summary>
        /// <param name="dateList"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private async Task<List<OrderQtyChartItem>> GetOrderQuantityData(List<DateList> dateList, QuantitativeDashboardFilterRequest request)
        {
            var mandayYearItem = new List<OrderQtyChartItem>();

            //loop the date list to fetch the order count 
            foreach (var dateItem in dateList)
            {
                request.ServiceDateFrom = dateItem.FromDate;
                request.ServiceDateTo = dateItem.ToDate;
                var bookingIdList = GetQueryableBookingIdList(request);

                mandayYearItem.AddRange(await _repo.GetQueryableMonthlyInspOrderQuantity(bookingIdList));

            }
            mandayYearItem = DashBoardMap.MapMonthlyInspOrderQuantity(mandayYearItem);
            return mandayYearItem;
        }
        /// <summary>
        ///Get Queryable BookingId List
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private IQueryable<int> GetQueryableBookingIdList(QuantitativeDashboardFilterRequest request)
        {

            var inspectionQuery = _sharedInspection.GetAllInspectionQuery();
            var inspectionQueryRequest = _sharedInspection.GetQuantitativeDashBoardInspectionQueryRequestMap(request);
            var data = _sharedInspection.GetInspectionQuerywithRequestFilters(inspectionQueryRequest, inspectionQuery);

            if (CheckDate(request.ServiceDateTo) && CheckDate(request.ServiceDateFrom))
            {
                data = data.Where(x => x.ServiceDateTo <= request.ServiceDateTo.ToDateTime() && x.ServiceDateTo >= request.ServiceDateFrom.ToDateTime());
            }

            //get booking ids
            var bookingIds = data.Select(x => x.Id);

            return bookingIds;
        }

    }
}
