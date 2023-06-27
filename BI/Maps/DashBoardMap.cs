using DTO.Common;
using DTO.Dashboard;
using DTO.FinanceDashboard;
using DTO.Manday;
using DTO.QuantitativeDashboard;
using DTO.Schedule;
using DTO.Supplier;
using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI.Maps
{
    public  class DashBoardMap: ApiCommonData
    {

        /// <summary>
        /// Map currentyearbookingdetail and lastyearbooking detail and create the customer business overview data
        /// </summary>
        /// <param name="currentBookingDetail"></param>
        /// <param name="lastYearBookingDetail"></param>
        /// <returns>CustomerBusinessOVDashboard</returns>
        public  CustomerBusinessOVDashboard MapCustomerBusinessOVDashBoard(BusinessOVBookingDetail currentYearBusinessData,
                                                                        BusinessOVBookingDetail lastYearBusinessData)
        {
            CustomerBusinessOVDashboard businessOVDashboard = new CustomerBusinessOVDashboard();
            if (currentYearBusinessData != null)
            {
                businessOVDashboard.BookingCount = currentYearBusinessData.BookingCount;
                businessOVDashboard.FactoryCount = currentYearBusinessData.FactoryCount;
                businessOVDashboard.ManDays = currentYearBusinessData.ManDays;
                businessOVDashboard.ProductsCount = currentYearBusinessData.ProductsCount;
                if (lastYearBusinessData != null && currentYearBusinessData != null)
                {
                    if (currentYearBusinessData.ProductsCount > 0 && lastYearBusinessData.ProductsCount > 0)
                        businessOVDashboard.ProductPercentage = (currentYearBusinessData.ProductsCount / lastYearBusinessData.ProductsCount) * 100;
                    if (currentYearBusinessData.FactoryCount > 0 && lastYearBusinessData.FactoryCount > 0)
                        businessOVDashboard.FactoryPercentage = (currentYearBusinessData.FactoryCount / lastYearBusinessData.FactoryCount) * 100;
                    if (currentYearBusinessData.ManDays > 0 && lastYearBusinessData.ManDays > 0)
                        businessOVDashboard.ManDayPercentage = (currentYearBusinessData.ManDays / lastYearBusinessData.ManDays) * 100;
                }
            }

            return businessOVDashboard;

        }
        /// <summary>
        /// map the status color and status name to the API Result analysis dashboard
        /// </summary>
        /// <param name="customerAPIRAList"></param>
        /// <returns></returns>
        public  List<CustomerAPIRADashboard> MapStatusColorAPIRADashboard(List<CustomerAPIRADashboardRepo> customerAPIRAList,
                                                                                                List<FBReportResultData> resultData)
        {
            List<CustomerAPIRADashboard> dashBoardList = new List<CustomerAPIRADashboard>();
            if (customerAPIRAList != null && customerAPIRAList.Any())
            {
                foreach (var apiRAData in customerAPIRAList)
                {
                    CustomerAPIRADashboard dashboard = new CustomerAPIRADashboard();
                    dashboard.Id = apiRAData.ResultId;
                    dashboard.StatusName = resultData.FirstOrDefault(x => x.ResultId == apiRAData.ResultId).ResultName;
                    dashboard.TotalCount = apiRAData.TotalCount;
                    dashboard.StatusColor = CustomerAPIRADashboardColor.GetValueOrDefault((int)apiRAData.ResultId,"");
                    dashBoardList.Add(dashboard);
                }
            }

            return dashBoardList;
        }

        /// <summary>
        /// Map CustomerResultMaster with the data
        /// </summary>
        /// <param name="customerResultList"></param>
        /// <returns></returns>
        public  List<CustomerResultDashboard> MapCustomerResultAnalysis(List<CustomerResultMasterRepo> masterDataList,
                                                                                List<CustomerResultRepo> customerResultList)
        {
            List<CustomerResultDashboard> dashBoardList = new List<CustomerResultDashboard>();

            foreach (var result in customerResultList)
            {
                CustomerResultDashboard dashBoard = new CustomerResultDashboard();
                dashBoard.TotalCount = result.TotalCount;
                var resultData = masterDataList.FirstOrDefault(x => x.Id == result.Id);
                //if customdecision name is not there then take customer decision data
                dashBoard.StatusName = (resultData != null && !string.IsNullOrEmpty(resultData.CustomDecisionName)) ?
                    resultData.CustomDecisionName : resultData?.CustomerDecisionName;
                dashBoard.StatusColor = CustomerResultDashboardColor.GetValueOrDefault(resultData.CustomerDecisionName);
                dashBoardList.Add(dashBoard);
            }

            return dashBoardList;

        }

        ///// <summary>
        ///// map the status color to the API Result analysis dashboard
        ///// </summary>
        ///// <param name="customerAPIRAList"></param>
        ///// <returns></returns>
        //public  List<CustomerAPIRADashboard> MapProductCategoryDashboard(List<CustomerAPIRADashboard> customerAPIRAList)
        //{
        //    customerAPIRAList.ForEach(x => x.StatusColor = ProductCategoryDashboardColor.GetValueOrDefault(x.StatusName));
        //    return customerAPIRAList;
        //}

        public  List<InspectionRejectDashboard> MapInspectionRejectDashboard(List<InspectionRejectDashboard> inspectionRejectList)
        {
            int rowCount = 1;
            foreach (var data in inspectionRejectList)
            {
                data.StatusColor = InspectionRejectDashboardColor.GetValueOrDefault(rowCount);
                rowCount++;
            }
            return inspectionRejectList;
        }

        /// <summary>
        /// Map StatusColor and ImagePath to Productcategory dashboard data
        /// </summary>
        /// <param name="productCategoryList"></param>
        /// <returns></returns>
        public  List<ProductCategoryDashboard> MapProductCategoryDashboard(List<ProductCategoryDashboardRepo> productCategoryRepoList,
                                                                                                List<RefProductCategory> productCategoryMaster)
        {
            List<ProductCategoryDashboard> productCategoryList = new List<ProductCategoryDashboard>();
            if(productCategoryMaster!=null && productCategoryMaster.Any())
            {
                foreach (var categoryRepo in productCategoryRepoList)
                {
                    ProductCategoryDashboard category = new ProductCategoryDashboard();
                    category.Id = categoryRepo.Id ?? 0;
                    category.TotalCount = categoryRepo.TotalCount;
                    category.StatusColor = ProductCategoryDashboardColor.GetValueOrDefault(categoryRepo.Id ?? 0);
                    category.ImagePath = ProductCategoryImagePath.GetValueOrDefault(categoryRepo.Id ?? 0);
                    category.StatusName = productCategoryMaster.Where(x => x.Id == categoryRepo.Id.Value).FirstOrDefault().Name;
                    productCategoryList.Add(category);
                }
            }
            return productCategoryList.OrderByDescending(x=>x.TotalCount).ToList();
        }

        public  SupplierPerformanceDashboard MapSupplierPerformance(List<BookingDetail> bookingDetails,
                                                                          List<CustomerETDDataRepo> etdData, List<SupplierBookingRevisionRepo> bookingRevision)
        {
            SupplierPerformanceDashboard supplierPerformanceDashboard = new SupplierPerformanceDashboard();
            supplierPerformanceDashboard.BookingLeadTime = 0;
            supplierPerformanceDashboard.ETDLeadTime = 0;
            //sum the booking lead time of the filtered bookings
            var bookingLeadTime = bookingDetails.Where(x => x.ServiceDateFrom != null && x.CreationDate != null).Sum(x => Convert.ToInt32(x.ServiceDateFrom.Subtract(x.CreationDate).Days));
            if (bookingLeadTime > 0)
            {
                supplierPerformanceDashboard.BookingLeadTime = bookingLeadTime / bookingDetails.Count;
            }
            //sum the etd lead time of the filtered bookings
            var etdLeadTime = etdData.Sum(x => Convert.ToInt32(x.EtdDate.Value.Subtract(x.ServiceToDate).Days));
            if (etdLeadTime > 0)
                supplierPerformanceDashboard.ETDLeadTime = Convert.ToInt32(Math.Round((double)(etdLeadTime / etdData.Count)));
            //group the bookings with count greater than one.
            if (bookingRevision != null && bookingRevision.Any())
            {
                //Group the inspection booking and count and if any date is changed for the booking then count will be more than 1
                var supplierBookingRevision = bookingRevision.
                                          GroupBy(x => x.InspectionId, p => p, (key, _data) =>
                                          new SupplierBookingRevisionRepo
                                          {
                                              InspectionId = key,
                                              BookingCount = _data.Count()
                                          }).ToList();
                //take the booking count >1 which means servicedate for the bookings
                var bookingCount = supplierBookingRevision.Where(x => x.BookingCount > 1).Count();
                //sum the count of bookings changed where servicedate changed and calculate average
                if (bookingCount > 0)
                    supplierPerformanceDashboard.BookingRevisons = supplierBookingRevision.Where(x => x.BookingCount > 1).
                                                                    Sum(x => x.BookingCount) / bookingCount;
            }
            return supplierPerformanceDashboard;
        }

        /// <summary>
        /// Map the daily mandays between two dates
        /// </summary>
        /// <param name="manDayRepoList"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public  List<ManDayData> MapDailyManDayData(List<InspectionManDaysRepo> manDayRepoList, DateTime fromDate, DateTime toDate)
        {
            List<ManDayData> manDaysList = new List<ManDayData>();
            //Iterate the days and calculate the mandays
            int dayCount = 1;
            toDate = toDate.AddDays(1);
            foreach (var date in EachDay(fromDate, toDate))
            {
                ManDayData manDayData = new ManDayData();
                manDayData.TotalManDays = manDayRepoList.Where(x => x.ServiceDate.Day == date.Day).Sum(x => x.ManDays);
                manDayData.Name = DayData.GetValueOrDefault(dayCount);
                manDaysList.Add(manDayData);
                dayCount++;
            }
            return manDaysList;
        }

        public  List<ManDayData> MapWeeklyManDayData(List<InspectionManDaysRepo> manDayRepoList, DateTime fromDate, DateTime toDate)
        {
            List<ManDayData> manDaysList = new List<ManDayData>();
            DateTime firstDay = fromDate;
            //Get weekday manday data like Week1:20,Week1:10,Weak2:30 we will get duplicate since we take the diff 
            //from each booking service date
            var weeklyData = manDayRepoList.GroupBy(x => new
            {
                WeekNumber = (x.ServiceDate - firstDay).Days / 7,
                x.ManDays
            }).Select(x =>
               new InspectionWeeklyManDaysRepo
               {
                   WeekNumber = x.Key.WeekNumber,
                   ManDays = x.Sum(y => y.ManDays)
               }).ToList();
            //Caculate the mandays for every 7 weeks.above result gives data only for available weeks
            for (int weekCount = 0; weekCount < 7; weekCount++)
            {
                ManDayData manDayData = new ManDayData();
                manDayData.TotalManDays = weeklyData.Where(x => x.WeekNumber == weekCount).Sum(x => x.ManDays);
                var weekNumber = weekCount + 1;
                manDayData.Name = WeekData.GetValueOrDefault(weekNumber);
                manDaysList.Add(manDayData);
            }
            return manDaysList;
        }
        /// <summary>
        /// Map Manday data for each month between two dates
        /// </summary>
        /// <param name="manDayRepoList"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public  List<ManDayData> MapMonthlyManDayData(List<InspectionMonthlyManDaysRepo> manDayRepoList, DateTime fromDate, DateTime toDate)
        {
            List<ManDayData> manDaysList = new List<ManDayData>();
            foreach (var date in EachMonth(fromDate, toDate))
            {
                ManDayData manDayData = new ManDayData();
                manDayData.TotalManDays = manDayRepoList.Where(x => x.Month == date.Month).Sum(x => x.ManDays);
                manDayData.Name = MonthData.GetValueOrDefault(date.Month);
                manDaysList.Add(manDayData);
            }
            return manDaysList;
        }

        /// <summary>
        /// Gives Each Day IEnumerable DateTime Value to iterate between two dates
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public  IEnumerable<DateTime> EachDay(DateTime fromDate, DateTime toDate)
        {
            for (var day = fromDate.Date; day.Date < toDate.Date; day = day.AddDays(1))
                yield return day;
        }

        /// <summary>
        /// Give Each Month Date with IEnumerable DateTime Values to iterate between two dates
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public  IEnumerable<DateTime> EachMonth(DateTime fromDate, DateTime toDate)
        {
            for (var month = fromDate.Date; month.Date < toDate.Date || month.Month == toDate.Month; month = month.AddMonths(1))
                yield return month;
        }

        public  InspectionManDayOverview MapInspectionManDayOverview(InspectionManDayData currentYearManDayData,
                                                                            InspectionManDayData lastYearManDayData, List<POProductsRepo> POProducts)
        {
            InspectionManDayOverview dashboarData = new InspectionManDayOverview();
            //assign total inspection and manday    
            if (currentYearManDayData != null)
            {
                dashboarData.TotalInspections = currentYearManDayData.TotalInspections;
                dashboarData.TotalManDays = currentYearManDayData.TotalManDays;
                if (POProducts != null && POProducts.Any())
                    dashboarData.TotalReports = POProducts.Sum(x => x.ProductFbReportCount) + POProducts.Sum(x => x.ContainerFbReportCount);
            }
            //calculate total average percent and manday percent
            if (currentYearManDayData != null && lastYearManDayData != null)
            {
                if (currentYearManDayData.TotalInspections > 0 && lastYearManDayData.TotalInspections > 0)
                {
                    //formuls -> current year - last year / (current or last year, the one that is greater) * 100
                    var denominator = currentYearManDayData.TotalInspections > lastYearManDayData.TotalInspections ? currentYearManDayData.TotalInspections : lastYearManDayData.TotalInspections;
                    var res = (double)(currentYearManDayData.TotalInspections - lastYearManDayData.TotalInspections) / denominator;
                    dashboarData.AveragePercentage = Convert.ToInt32(Math.Round(res * 100));

                }
                if (currentYearManDayData.TotalManDays > 0 && lastYearManDayData.TotalManDays > 0)
                {
                    //formuls -> current year - last year / (current or last year, the one that is greater) * 100
                    var denominator = currentYearManDayData.TotalManDays > lastYearManDayData.TotalManDays ? currentYearManDayData.TotalManDays : lastYearManDayData.TotalManDays;
                    var res = (double)(currentYearManDayData.TotalManDays - lastYearManDayData.TotalManDays) / denominator;
                    dashboarData.TotalManDaysPercentage = Convert.ToInt32(Math.Round(res * 100));
                }

            }
            // setting this value to make percent arrow
            if (dashboarData.AveragePercentage >= 100 || dashboarData.AveragePercentage == 0)
                dashboarData.AverageExceeds = true;
            if (dashboarData.TotalManDaysPercentage >= 100 || dashboarData.TotalManDaysPercentage == 0)
                dashboarData.ManDayExceeds = true;

            return dashboarData;
        }

        //public  List<BookingDetail> MapBookingDetail(List<BookingDetailRepo> bookingRepoDetails)
        //{
        //    List<BookingDetail> bookingDetails = new List<BookingDetail>();
        //    foreach (var bookingRepoDetail in bookingRepoDetails)
        //    {
        //        BookingDetail bookingDetail = new BookingDetail();
        //        bookingDetail.CustomerId = bookingRepoDetail.CustomerId;
        //        bookingDetail.InspectionId = bookingRepoDetail.InspectionId;
        //        bookingDetail.ServiceDateFrom = GetCustomDate(bookingRepoDetail.ServiceDateFrom);
        //        bookingDetail.ServiceDateTo = GetCustomDate(bookingRepoDetail.ServiceDateTo);
        //        bookingDetail.CreationDate = GetCustomDate(bookingRepoDetail.CreationDate);
        //        bookingDetail.SupplierId = bookingRepoDetail.SupplierId;
        //        bookingDetail.FactoryId = bookingRepoDetail.FactoryId;
        //        bookingDetails.Add(bookingDetail);
        //    }
        //    return bookingDetails;
        //}

        public  CustomerFactoryDashboard MapFactoryDefects(List<int> bookingIdList, List<FbReportCustomerDashboard> fbReportData, SupplierAddress supData, List<DefectData> reportFailList)
        {
            List<ReportStatusCount> reportstatus = new List<ReportStatusCount>();
            var distinctReportStatus = Enum.GetValues(typeof(FBReportResult));

            //Fetch the report count for each report status
            foreach (var item in distinctReportStatus)
            {
                ReportStatusCount data = new ReportStatusCount();
                data.StatusName = item.ToString();
                data.ReportCount = fbReportData.Where(x => x.ResultId.HasValue && x.ResultId == (int)item).Select(x => x.FbReportId).Distinct().Count();
                data.StatusColor = ReportResultColor.Where(x => x.Key == (int)item).Select(x => x.Value).FirstOrDefault();
                reportstatus.Add(data);
            }

            return new CustomerFactoryDashboard
            {
                BookingCount = bookingIdList.Count(),
                TotalReportCount = fbReportData.Where(x => x.FbReportId.HasValue).Select(x => x.FbReportId).Distinct().Count(),
                FactoryAddress = supData.Address,
                FactoryRegionalAddress = supData.RegionalAddress,
                FactoryName = supData.SupplierName,
                FactoryRegionalName = supData.RegionalSupplierName,
                DefectList = reportFailList,
                Latitude = supData.Latitude,
                Longitude = supData.Longitude,
                TotalReportStatusCount = Enum.GetValues(typeof(FBReportResult)).Length,
                ResportStatusCount = reportstatus
            };
        }
        /// <summary>
        /// Map MapMonthlyInspManDays
        /// </summary>
        /// <param name="monthlyInspDataList"></param>
        /// <returns></returns>
        public  List<MandayYearChartItem> MapMonthlyInspManDays(List<MandayYearChartItem> monthlyInspDataList)
        {
            List<MandayYearChartItem> monthlyInspManDaysList = new List<MandayYearChartItem>();

            foreach (var item in monthlyInspDataList)
            {
                MandayYearChartItem monthlyInspManDays = new MandayYearChartItem();
                monthlyInspManDays.Year = item.Year;
                monthlyInspManDays.Month = item.Month;
                monthlyInspManDays.MonthManDay = item.MonthManDay;
                monthlyInspManDays.MonthName = MonthData.GetValueOrDefault(item.Month);
                monthlyInspManDaysList.Add(monthlyInspManDays);
            }
            return monthlyInspManDaysList;
        }
        /// <summary>
        /// Map MonthlyInspOrderQuantity
        /// </summary>
        /// <param name="monthlyInspDataList"></param>
        /// <returns></returns>
        public  List<OrderQtyChartItem> MapMonthlyInspOrderQuantity(List<OrderQtyChartItem> monthlyInspDataList)
        {
            List<OrderQtyChartItem> monthlyInspOrdQuantityList = new List<OrderQtyChartItem>();

            foreach (var item in monthlyInspDataList)
            {
                OrderQtyChartItem monthlyInspOrdQuantity = new OrderQtyChartItem();
                monthlyInspOrdQuantity.Year = item.Year;
                monthlyInspOrdQuantity.Month = item.Month;
                monthlyInspOrdQuantity.MonthOrderQuantity = item.MonthOrderQuantity;
                monthlyInspOrdQuantity.MonthName = MonthData.GetValueOrDefault(item.Month);
                monthlyInspOrdQuantityList.Add(monthlyInspOrdQuantity);
            }
            return monthlyInspOrdQuantityList;
        }
        /// <summary>
        /// Map Finance Dashboard Ratio Analysis
        /// </summary>
        /// <param name="billedManday"></param>
        /// <param name="scheduleManday"></param>
        /// <returns></returns>
        public static List<FinanceDashboardRatioAnalysis> MapRatioAnalysisCalculation(List<FinanceDashboardBilledMandayRepo> billedManday,
            List<FinanceDashboardInspectionFeesRepo> InspectionFeesList, List<FinanceDashboardScheduleMandayRepo> scheduleManday,
            List<ExchangeCurrencyItem> expenseData, List<ProductionManDay> productionManDayList)
        {
            List<FinanceDashboardRatioAnalysis> res = new List<FinanceDashboardRatioAnalysis>();
            FinanceDashboardRatioAnalysis ratioAnalysis =null;
            foreach (var item in billedManday)
            {

                var scheduleMandayData = scheduleManday.Where(x =>x.CustomerId == item.CustomerId).ToList();
                var productionManDay = productionManDayList.Where(x => x.CustomerId == item.CustomerId).ToList();
                var _actualManday = DefaultDecimalNumber;
                if (scheduleMandayData.Any())
                {
                    _actualManday = scheduleMandayData.Sum(x => x.ActualManDay);
                }
                double _productionManday = productionManDay.Sum(x=>x.ProductionManday);
                var _revenue = DefaultDecimalNumber;
                var inspectionFeeData = InspectionFeesList.FirstOrDefault(x => x.CustomerId == item.CustomerId);
                if (inspectionFeeData != null)
                {
                    _revenue = inspectionFeeData.inspectionFees.GetValueOrDefault() - inspectionFeeData.discount.GetValueOrDefault();
                }

                var _BilledAvg = DefaultDecimalNumber;
                _BilledAvg= Convert.ToDouble(_revenue / item.BilledManday);

                var _totalChargeback = inspectionFeeData?.TotalChargeBack ?? 0;

                var totalExp = expenseData?.Where(x => x.CustomerId == item.CustomerId).Sum(x => x.Fee) ?? 0;
                var netIncome = _revenue + _totalChargeback - totalExp;

                var _ratio = DefaultDecimalNumber;
                var ProductionAvg = DefaultDecimalNumber;
                if (_productionManday > 0) {
                    _ratio = Convert.ToDouble(item.BilledManday / _productionManday);
                    ProductionAvg = netIncome / _productionManday;
                }

                var _netMDRateUsd = DefaultDecimalNumber;
                _netMDRateUsd = Convert.ToDouble(netIncome / item.BilledManday);

                ratioAnalysis = new FinanceDashboardRatioAnalysis();
                ratioAnalysis.Customer = item.Customer;
                ratioAnalysis.BilledManday = item.BilledManday;
                ratioAnalysis.ProductionManday = Math.Round(_productionManday, NumberTwo);
                ratioAnalysis.ActualManday = Math.Round(_actualManday, NumberTwo);
                ratioAnalysis.Revenue = Math.Round(_revenue, NumberTwo);
                ratioAnalysis.Ratio = Math.Round(_ratio, NumberTwo);
                ratioAnalysis.BilledAvgManday = Math.Round(_BilledAvg, NumberTwo);
                ratioAnalysis.ProductionAvgManday = Math.Round(ProductionAvg, NumberTwo);
                ratioAnalysis.TotalExpense = Math.Round(totalExp, NumberTwo);
                ratioAnalysis.ChargeBack = Math.Round(_totalChargeback, NumberTwo);
                ratioAnalysis.NetIncome = Math.Round(netIncome, NumberTwo);
                ratioAnalysis.NetIncomeAvg = Math.Round(_netMDRateUsd, NumberTwo);

                res.Add(ratioAnalysis);
            }
            return res.Take(ApiCommonData.Top10Rows).OrderByDescending(x => x.BilledManday).ToList();
        }

        public static List<FinanceDashboardExportRatioAnalysis> MapExportRatioAnalysisCalculation(List<FinanceDashboardBilledMandayRepo> billedManday,
            List<FinanceDashboardInspectionFeesRepo> InspectionFeesList, List<FinanceDashboardScheduleMandayRepo> scheduleManday,
            List<ExchangeCurrencyItem> expenseData, List<ProductionManDay> productionManDayList)
        {
            List<FinanceDashboardExportRatioAnalysis> res = new List<FinanceDashboardExportRatioAnalysis>();
            FinanceDashboardExportRatioAnalysis ratioAnalysis = null;
            foreach (var item in billedManday)
            {

                var scheduleMandayData = scheduleManday.Where(x => x.CustomerId == item.CustomerId).ToList();
                var productionManDay = productionManDayList.Where(x => x.CustomerId == item.CustomerId).ToList();
                var _actualManday = DefaultDecimalNumber;
                if (scheduleMandayData.Any())
                {
                    _actualManday = scheduleMandayData.Sum(x => x.ActualManDay);
                }

                double _productionManday = productionManDay.Sum(x => x.ProductionManday);

                var _revenue = DefaultDecimalNumber;
                var inspectionFeeData = InspectionFeesList.FirstOrDefault(x => x.CustomerId == item.CustomerId);
                if (inspectionFeeData != null)
                {
                    _revenue = inspectionFeeData.inspectionFees.GetValueOrDefault() - inspectionFeeData.discount.GetValueOrDefault();

                }

                var _BilledAvg = DefaultDecimalNumber;
                _BilledAvg = Convert.ToDouble(_revenue / item.BilledManday);

                var _totalChargeback = inspectionFeeData?.TotalChargeBack ?? 0;

                var totalExp = expenseData?.Where(x => x.CustomerId == item.CustomerId).Sum(x => x.Fee) ?? 0;
                var netIncome = _revenue + _totalChargeback - totalExp;

                var _ratio = DefaultDecimalNumber;
                var ProductionAvg = DefaultDecimalNumber;
                if (_productionManday > 0)
                {
                    _ratio = Convert.ToDouble(item.BilledManday / _productionManday);
                    ProductionAvg = netIncome / _productionManday;
                }

                var _netMDRateUsd = DefaultDecimalNumber;
                _netMDRateUsd = Convert.ToDouble(netIncome / item.BilledManday);

                ratioAnalysis = new FinanceDashboardExportRatioAnalysis();

                ratioAnalysis.Customer = item.Customer;
                ratioAnalysis.BilledManday = item.BilledManday;
                ratioAnalysis.ProductionManday = Math.Round(_productionManday, NumberTwo) ;
                ratioAnalysis.ActualManday = Math.Round(_actualManday, NumberTwo);
                ratioAnalysis.Revenue = Math.Round(_revenue, NumberTwo);
                ratioAnalysis.Ratio = Math.Round(_ratio, NumberTwo);
                ratioAnalysis.BilledAvgManday = Math.Round(_BilledAvg, NumberTwo);
                ratioAnalysis.ProductionAvgManday = Math.Round(ProductionAvg, NumberTwo);
                ratioAnalysis.Expense = Math.Round(totalExp, NumberTwo);
                ratioAnalysis.Chargeback = Math.Round(_totalChargeback, NumberTwo);
                ratioAnalysis.Income = Math.Round(netIncome, NumberTwo);
                ratioAnalysis.NetIncomeMDRate = Math.Round(_netMDRateUsd, NumberTwo);

                res.Add(ratioAnalysis);
            }
            if(res.Any())
            {
                ratioAnalysis = new FinanceDashboardExportRatioAnalysis();

                ratioAnalysis.Customer = "Total";
                ratioAnalysis.BilledManday = res.Sum(x => x.BilledManday) ;
                ratioAnalysis.ProductionManday = res.Sum(x => x.ProductionManday);
                ratioAnalysis.Revenue = res.Sum(x => x.Revenue);
                ratioAnalysis.Ratio = res.Sum(x => x.Ratio);
                ratioAnalysis.BilledAvgManday = res.Sum(x => x.BilledAvgManday);
                ratioAnalysis.ProductionAvgManday = res.Sum(x => x.ProductionAvgManday);
                ratioAnalysis.Expense = res.Sum(x => x.Expense);
                ratioAnalysis.Chargeback = res.Sum(x => x.Chargeback);
                ratioAnalysis.Income = res.Sum(x => x.Income);
                ratioAnalysis.NetIncomeMDRate = res.Sum(x => x.NetIncomeMDRate);

                res.Add(ratioAnalysis);
            }

            return res;
        }

    }
}
