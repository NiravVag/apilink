using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.Dashboard;
using DTO.Inspection;
using DTO.ManagementDashboard;
using DTO.Manday;
using DTO.RepoRequest.Enum;
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
    public class ManagementDashboardManager : ApiCommonData, IManagementDashboardManager
    {
        private readonly IManagementDashboardRepository _repo;
        private readonly IMandayManager _mandayManager = null;
        private readonly IInspectionBookingRepository _inspRepo = null;
        private readonly IDashboardRepository _dashboardRepo = null;
        private readonly IDashboardManager _dashboardManager = null;
        private readonly IInspectionBookingManager _inspManager = null;
        private readonly ICustomerManager _cusManager = null;
        private readonly ILocationRepository _locationRepo = null;
        private readonly ISupplierManager _supManager = null;
        private readonly IAPIUserContext _applicationContext = null;
        private readonly ISharedInspectionManager _sharedInspection = null;
        private readonly IHumanResourceRepository _humanResourceRepository = null;

        public ManagementDashboardManager(IMandayManager mandayManager, IInspectionBookingRepository inspRepo,
            IDashboardRepository dashboardRepo,
            IDashboardManager dashboardManager,
            IInspectionBookingManager inspManager,
            IManagementDashboardRepository repo,
            ICustomerManager cusManager,
            ILocationRepository locationRepo,
            ISupplierManager supManager,
            ISharedInspectionManager sharedInspectionManager,
            IHumanResourceRepository humanResourceRepository,
            IAPIUserContext applicationContext)
        {
            _mandayManager = mandayManager;
            _inspRepo = inspRepo;
            _dashboardRepo = dashboardRepo;
            _dashboardManager = dashboardManager;
            _inspManager = inspManager;
            _repo = repo;
            _cusManager = cusManager;
            _locationRepo = locationRepo;
            _supManager = supManager;
            _applicationContext = applicationContext;
            _sharedInspection = sharedInspectionManager;
            _humanResourceRepository = humanResourceRepository;
        }

        //get the data for the man day year chart
        public async Task<MandayYearChartManagementDashboardResponse> GetMandayYearChart(ManagementDashboardRequest request)
        {
            var response = new MandayYearChartManagementDashboardResponse();
            var mandayYear = new List<MandayYear>();

            if (request.MandayChartType == (int)MandayChartYear.YTD)
            {
                request.ServiceDateFrom = Static_Data_Common.GetCustomDate(new DateTime(DateTime.Now.Year, 1, 1));
                request.ServiceDateTo = Static_Data_Common.GetCustomDate(DateTime.Today);
            }

            // get the inspection Query and get the query request
            var inspectionQuery = _sharedInspection.GetAllInspectionQuery();
            var inspectionQueryRequest = _sharedInspection.GetManagementDashboardInspectionRequestMap(request);
            //get the booking data query
            var bookingData = _sharedInspection.GetInspectionQuerywithRequestFilters(inspectionQueryRequest, inspectionQuery);

            bookingData = applyExternalFilters(request, bookingData);

            var currentYearBookingIds = bookingData.Select(x => x.Id);

            var data = await _repo.GetMonthlyInspManDaysByBookingQuery(currentYearBookingIds);

            if (request.MandayChartType == (int)MandayChartYear.MTD)
            {
                inspectionQueryRequest.FromDate = Static_Data_Common.GetCustomDate(request.ServiceDateFrom.ToDateTime().AddYears(-1));
                inspectionQueryRequest.ToDate = Static_Data_Common.GetCustomDate(request.ServiceDateTo.ToDateTime().AddYears(-1));

                request.ServiceDateFrom = inspectionQueryRequest.FromDate;
                request.ServiceDateTo = inspectionQueryRequest.ToDate;

                var bookingDataLastYear = _sharedInspection.GetInspectionQuerywithRequestFilters(inspectionQueryRequest, inspectionQuery);
                bookingDataLastYear = applyExternalFilters(request, bookingDataLastYear);
                var bookingIdsLastYear = bookingDataLastYear.Select(x => x.Id);

                data.AddRange(await _repo.GetMonthlyInspManDaysByBookingQuery(bookingIdsLastYear));
            }

            if (data == null || !data.Any())
            {
                return new MandayYearChartManagementDashboardResponse { Result = MandayDashboardResult.NotFound };
            }

            var budgetManday = await _repo.GetCurrentYearBudgetManday(request.CountryIdList);

            var index = 1;
            //get the per year man day count along with month wise data
            var items = data.GroupBy(p => p.Year, (key, _data) =>
            new MandayYearChart
            {
                Year = key,
                MandayCount = _data.Where(x => x.Year == key).Sum(x => x.MonthManDay),
                MonthlyData = _data.Where(x => x.Year == key).ToList(),
                Color = MandayDashboardColorList.GetValueOrDefault(index++)
            }).OrderByDescending(x => x.MandayCount).Take(2);

            response.Budget = new MandayYearChart
            {
                Year = DateTime.Now.Year,
                MandayCount = budgetManday.Sum(x => x.MonthManDay),
                MonthlyData = budgetManday,
                Color = MandayDashboardColorList.GetValueOrDefault(index++)
            };

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

            return response;
        }

        /// <summary>
        /// Apply dashboard filters
        /// </summary>
        /// <param name="request"></param>
        /// <param name="bookingData"></param>
        /// <returns></returns>
        private IQueryable<InspTransaction> applyExternalFilters(ManagementDashboardRequest request, IQueryable<InspTransaction> bookingData)
        {
            if ((request.ServiceDateFrom?.ToDateTime() != null && request.ServiceDateTo?.ToDateTime() != null))
            {
                bookingData = bookingData.Where(x => x.ServiceDateTo <= request.ServiceDateTo.ToDateTime() && x.ServiceDateTo >= request.ServiceDateFrom.ToDateTime());
            }

            // filter based on status - inspected and validated

            if (request.StatusIdList == null || !request.StatusIdList.Any())
            {
                bookingData = bookingData.Where(x => InspectedStatusList.Contains(x.StatusId));
            }
            if (_applicationContext.RoleList.Contains((int)RoleEnum.OperationManagement) && _applicationContext.LocationList != null && _applicationContext.LocationList.Any())
            {
                bookingData = bookingData.Where(x => _applicationContext.LocationList.ToList().Contains(x.OfficeId.GetValueOrDefault()));
            }

            return bookingData;
        }

        //get the data for the top level filters in manday dashboard
        public async Task<ManagementDashboardResponse> GetManagementDashboardSearch(ManagementDashboardRequest request)
        {
            var response = new ManagementDashboardResponse();
            var result = new ManagementDashboardItem();

            if (request != null)
            {
                try
                {

                    // get the inspection Query and get the query request
                    var inspectionQuery = _sharedInspection.GetAllInspectionQuery();
                    var inspectionQueryRequest = _sharedInspection.GetManagementDashboardInspectionRequestMap(request);
                    //get the booking data query
                    var bookingData = _sharedInspection.GetInspectionQuerywithRequestFilters(inspectionQueryRequest, inspectionQuery);

                    bookingData = applyExternalFilters(request, bookingData);

                    var bookingIds = bookingData.Select(x => x.Id);



                    if (bookingIds.Any())
                    {
                        //get the total manday for the booking list
                        result.TotalManday = await _dashboardRepo.GetInspectionManDaysQuery(bookingIds);

                        //get Product list for bookin Ids
                        result.TotalProductCount = await _repo.GetProductCountbyBookingQuery(bookingIds, InspectedStatusList);

                        result.TotalInspCount = await bookingIds.CountAsync();

                        result.TotalFactoryCount = bookingData.Select(x => x.FactoryId).Distinct().Count();

                        result.TotalCustomerCount = bookingData.Select(x => x.CustomerId).Distinct().Count();

                        result.TotalReportCount = await _repo.GetReportCountbyBookingQuery(bookingIds, InspectedStatusList);

                    }

                    // get last year data
                    var lastYearinspectionQuery = _sharedInspection.GetAllInspectionQuery();
                    var lastYearinspectionQueryRequest = _sharedInspection.GetManagementDashboardInspectionRequestMap(request);

                    lastYearinspectionQueryRequest.FromDate = Static_Data_Common.GetCustomDate(request.ServiceDateFrom.ToDateTime().AddYears(-1));
                    lastYearinspectionQueryRequest.ToDate = Static_Data_Common.GetCustomDate(request.ServiceDateTo.ToDateTime().AddYears(-1));

                    var bookingDataLastYear = _sharedInspection.GetInspectionQuerywithRequestFilters(lastYearinspectionQueryRequest, lastYearinspectionQuery);

                    request.ServiceDateFrom = lastYearinspectionQueryRequest.FromDate;
                    request.ServiceDateTo = lastYearinspectionQueryRequest.ToDate;
                    bookingDataLastYear = applyExternalFilters(request, bookingDataLastYear);

                    var bookingIdsLastYear = bookingDataLastYear.Select(x => x.Id);

                    if (bookingIdsLastYear.Any())
                    {
                        result.TotalMandayLastYear = await _dashboardRepo.GetInspectionManDaysQuery(bookingIdsLastYear);

                        //get Product list for bookin Ids
                        result.TotalProductCountLastYear = await _repo.GetProductCountbyBookingQuery(bookingIdsLastYear, InspectedStatusList);

                        result.TotalInspCountLastYear = await bookingIdsLastYear.CountAsync();

                        result.TotalFactoryCountLastYear = bookingDataLastYear.Select(x => x.FactoryId).Distinct().Count();

                        result.TotalCustomerCountLastYear = bookingDataLastYear.Select(x => x.CustomerId).Distinct().Count();

                        result.TotalReportCountLastYear = await _repo.GetReportCountbyBookingQuery(bookingIdsLastYear, InspectedStatusList);

                    }

                    //get the comparison of factory count between this year and last year
                    if (result.TotalFactoryCount != 0)
                    {
                        var denominator = result.TotalFactoryCount >= result.TotalFactoryCountLastYear ? result.TotalFactoryCount : result.TotalFactoryCountLastYear;
                        var res = denominator > 0 ? ((double)result.TotalFactoryCount - result.TotalFactoryCountLastYear) / denominator : 0;
                        result.FactoryDifferencePercentage = Math.Round(res * 100);
                    }

                    //get the comparison of inspections count between this year and last year
                    if (result.TotalInspCount != 0)
                    {
                        var denominator = result.TotalInspCount >= result.TotalInspCountLastYear ? result.TotalInspCount : result.TotalInspCountLastYear;
                        var res = denominator > 0 ? ((double)result.TotalInspCount - result.TotalInspCountLastYear) / denominator : 0;
                        result.InspectionDifferencePercentage = Math.Round(res * 100);
                    }

                    //get the comparison of manday count between this year and last year
                    if (result.TotalManday != 0)
                    {
                        var denominator = result.TotalManday > result.TotalMandayLastYear ? result.TotalManday : result.TotalMandayLastYear;
                        var res = denominator > 0 ? ((double)result.TotalManday - result.TotalMandayLastYear) / denominator : 0;
                        result.MandayDifferencePercentage = Math.Round(res * 100);
                    }

                    //get the comparison of product count between this year and last year
                    if (result.TotalProductCount != 0)
                    {
                        var denominator = result.TotalProductCount > result.TotalProductCountLastYear ? result.TotalProductCount : result.TotalProductCountLastYear;
                        var res = denominator > 0 ? (double)(result.TotalProductCount - result.TotalProductCountLastYear) / denominator : 0;
                        result.ProductDifferencePercentage = Math.Round(res * 100);
                    }

                    //get the comparison of customer count between this year and last year
                    if (result.TotalCustomerCount != 0)
                    {
                        var denominator = result.TotalCustomerCount > result.TotalCustomerCountLastYear ? result.TotalCustomerCount : result.TotalCustomerCountLastYear;
                        var res = denominator > 0 ? (double)(result.TotalCustomerCount - result.TotalCustomerCountLastYear) / denominator : 0;
                        result.CustomerDifferencePercentage = Math.Round(res * 100);
                    }

                    //get the comparison of report count between this year and last year
                    if (result.TotalReportCount != 0)
                    {
                        var denominator = result.TotalReportCount > result.TotalReportCountLastYear ? result.TotalReportCount : result.TotalReportCountLastYear;
                        var res = denominator > 0 ? (double)(result.TotalReportCount - result.TotalReportCountLastYear) / denominator : 0;
                        result.ReportDifferencePercentage = Math.Round(res * 100);
                    }

                    var staffAccess = await _humanResourceRepository.GetOfficesByStaff(_applicationContext.StaffId);
                    response.InspectionIdList = await bookingData.Select(x => x.Id).ToListAsync();
                    response.OfficeIds = staffAccess.Select(x => x.LocationId).ToList();
                    response.Data = result;
                    response.Result = ManagementDashboardResult.Success;
                }
                catch (Exception ex)
                {
                    response.Result = ManagementDashboardResult.Fail;
                }
            }

            if (response.Data == null)
            {
                response.Result = ManagementDashboardResult.NotFound;
            }

            return response;
        }

        //get the reject reasons data
        public async Task<InspectionRejectDashboardResponse> GetInspectionRejectChartByQuery(ManagementDashboardRequest request)
        {
            // get the inspection Query and get the query request
            var inspectionQuery = _sharedInspection.GetAllInspectionQuery();
            var inspectionQueryRequest = _sharedInspection.GetManagementDashboardInspectionRequestMap(request);
            //get the booking data query
            var bookingData = _sharedInspection.GetInspectionQuerywithRequestFilters(inspectionQueryRequest, inspectionQuery);

            bookingData = applyExternalFilters(request, bookingData);

            var bookingIds = bookingData.Select(x => x.Id);

            var response = await _dashboardManager.GetInspectionRejectDashBoardByBookingQuery(bookingIds);

            if (response == null || !response.Any())
            {
                return new InspectionRejectDashboardResponse { Result = ManagementDashboardResult.NotFound };
            }

            return new InspectionRejectDashboardResponse
            {
                Data = response,
                Result = ManagementDashboardResult.Success
            };
        }

        //get the servicce type data
        public async Task<ResultAnalyticsDashboardResponse> GetServiceTypeChart(ManagementDashboardRequest request)
        {
            var res = new List<CustomerAPIRADashboard>();

            // get the inspection Query and get the query request
            var inspectionQuery = _sharedInspection.GetAllInspectionQuery();
            var inspectionQueryRequest = _sharedInspection.GetManagementDashboardInspectionRequestMap(request);
            //get the booking data query
            var bookingData = _sharedInspection.GetInspectionQuerywithRequestFilters(inspectionQueryRequest, inspectionQuery);

            bookingData = applyExternalFilters(request, bookingData);

            var bookingIds = bookingData.Select(x => x.Id);

            var servData = await _repo.GetServiceTypeByQuery(bookingIds);
            Random r = new Random();
            for (int i = 0; i < servData.Count; i++)
            {
                var item = servData[i];
                res.Add(new CustomerAPIRADashboard
                {
                    StatusName = item.ServiceTypeName,
                    TotalCount = item.Count,
                    StatusColor = CommonDashboardColor.GetValueOrDefault(i + 1)
                });
            }

            if (!res.Any())
            {
                return new ResultAnalyticsDashboardResponse { Result = ManagementDashboardResult.NotFound };
            }

            return new ResultAnalyticsDashboardResponse
            {
                Data = res,
                Result = ManagementDashboardResult.Success
            };
        }

        public async Task<ResultAnalyticsDashboardResponse> GetServiceTypeChart(List<int> inspectionIdList)
        {
            var res = new List<CustomerAPIRADashboard>();
            var servData = await _repo.GetServiceType(inspectionIdList);
            Random r = new Random();
            for (int i = 0; i < servData.Count; i++)
            {
                var item = servData[i];
                res.Add(new CustomerAPIRADashboard
                {
                    StatusName = item.ServiceTypeName,
                    TotalCount = item.Count,
                    StatusColor = CommonDashboardColor.GetValueOrDefault(i + 1)
                });
            }

            if (!res.Any())
            {
                return new ResultAnalyticsDashboardResponse { Result = ManagementDashboardResult.NotFound };
            }

            return new ResultAnalyticsDashboardResponse
            {
                Data = res,
                Result = ManagementDashboardResult.Success
            };
        }

        public async Task<ResultAnalyticsDashboardResponse> GetServiceTypeChartByQuery(IQueryable<int> inspectionIdList)
        {
            var res = new List<CustomerAPIRADashboard>();
            var servData = await _repo.GetServiceTypeByQuery(inspectionIdList);
            Random r = new Random();
            for (int i = 0; i < servData.Count; i++)
            {
                var item = servData[i];
                res.Add(new CustomerAPIRADashboard
                {
                    StatusName = item.ServiceTypeName,
                    TotalCount = item.Count,
                    StatusColor = CommonDashboardColor.GetValueOrDefault(i + 1)
                });
            }

            if (!res.Any())
            {
                return new ResultAnalyticsDashboardResponse { Result = ManagementDashboardResult.NotFound };
            }

            return new ResultAnalyticsDashboardResponse
            {
                Data = res,
                Result = ManagementDashboardResult.Success
            };
        }

        //get the overview dashboard data
        public async Task<OverviewDashboardResponse> OverviewDashboardSearch(ManagementDashboardRequest request)
        {
            var response = new OverviewDashboardResponse();
            var result = new OverviewChart();

            if (request != null)
            {
                try
                {
                    // get the inspection Query and get the query request
                    var inspectionQuery = _sharedInspection.GetAllInspectionQuery();
                    var inspectionQueryRequest = _sharedInspection.GetManagementDashboardInspectionRequestMap(request);
                    //get the booking data query
                    var bookingData = _sharedInspection.GetInspectionQuerywithRequestFilters(inspectionQueryRequest, inspectionQuery);
                    bookingData = applyExternalFilters(request, bookingData);
                    var bookingIds = bookingData.Select(x => x.Id);
                    result.TotalBookingCount = await bookingIds.CountAsync();
                    //get the customer information
                    result.TotalCustomerCount = await bookingData.Select(x => x.CustomerId).Distinct().CountAsync();

                    //get the number of quotations rejjected by the customer
                    result.QuotationRejectedByCustomerCount = await _repo.QuotationRejectedByCustomerCountByBookingQuery(bookingIds);
                    response.Data = result;
                    response.Result = ManagementDashboardResult.Success;
                }

                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return response;
        }

        /// <summary>
        /// get ProductCategoryDashboard data by inspection query
        /// </summary>
        /// <param name="inspectionIdList"></param>
        /// <returns></returns>
        public async Task<ProductCategoryDashboardResponse> ProductCategoryDashboardSearchbyQuery(IQueryable<int> inspectionIdList)
        {



            //get the product category data based on booking Ids
            var res = await _repo.GetProductCategoryDashboardByQuery(inspectionIdList);

            if (res == null || !res.Any())
            {
                return new ProductCategoryDashboardResponse { Result = ManagementDashboardResult.NotFound };
            }

            Random r = new Random();

            foreach (var item in res)
            {
                item.StatusColor = CommonDashboardColor.GetValueOrDefault(r.Next(1, 30));
            }

            return new ProductCategoryDashboardResponse
            {
                Data = res,
                Result = ManagementDashboardResult.Success
            };
        }

        //get the product category  data
        public async Task<ProductCategoryDashboardResponse> ProductCategoryDashboardSearch(List<int> inspectionIdList)
        {
            if (!inspectionIdList.Any())
            {
                return new ProductCategoryDashboardResponse { Result = ManagementDashboardResult.NotFound };
            }
            //get the product category data based on booking Ids
            var res = await _repo.GetProductCategoryDashboard(inspectionIdList);

            if (res == null || !res.Any())
            {
                return new ProductCategoryDashboardResponse { Result = ManagementDashboardResult.NotFound };
            }

            Random r = new Random();

            foreach (var item in res)
            {
                item.StatusColor = CommonDashboardColor.GetValueOrDefault(r.Next(1, 30));
            }

            return new ProductCategoryDashboardResponse
            {
                Data = res,
                Result = ManagementDashboardResult.Success
            };
        }





        //get the product category  data
        public async Task<ProductCategoryDashboardResponse> ProductCategoryDashboardSearchByBookingRequest(ManagementDashboardRequest request)
        {

            // get the inspection Query and get the query request
            var inspectionQuery = _sharedInspection.GetAllInspectionQuery();
            var inspectionQueryRequest = _sharedInspection.GetManagementDashboardInspectionRequestMap(request);
            //get the booking data query
            var bookingData = _sharedInspection.GetInspectionQuerywithRequestFilters(inspectionQueryRequest, inspectionQuery);
            bookingData = applyExternalFilters(request, bookingData);
            var bookingIds = bookingData.Select(x => x.Id);

            //get the product category data based on booking Ids
            var res = await _repo.GetProductCategoryDashboardByQuery(bookingIds);

            if (res == null || !res.Any())
            {
                return new ProductCategoryDashboardResponse { Result = ManagementDashboardResult.NotFound };
            }

            Random r = new Random();

            foreach (var item in res)
            {
                item.StatusColor = CommonDashboardColor.GetValueOrDefault(r.Next(1, 30));
            }

            return new ProductCategoryDashboardResponse
            {
                Data = res,
                Result = ManagementDashboardResult.Success
            };
        }

        //get the result dahsboard data
        public async Task<ResultDashboardResponse> GetResultDashboard(ManagementDashboardRequest request)
        {
            // get the inspection Query and get the query request
            var inspectionQuery = _sharedInspection.GetAllInspectionQuery();
            var inspectionQueryRequest = _sharedInspection.GetManagementDashboardInspectionRequestMap(request);
            //get the booking data query
            var bookingData = _sharedInspection.GetInspectionQuerywithRequestFilters(inspectionQueryRequest, inspectionQuery);

            bookingData = applyExternalFilters(request, bookingData);
            var bookingIds = bookingData.Select(x => x.Id);

            var res = await _dashboardManager.GetAPIRADashboardByQuery(bookingIds);

            if (res == null || !res.Any())
            {
                return new ResultDashboardResponse { Result = ManagementDashboardResult.NotFound };
            }

            return new ResultDashboardResponse
            {
                Data = res.OrderByDescending(x => x.TotalCount).ToList(),
                Result = ManagementDashboardResult.Success
            };
        }

        //get the result dahsboard data
        public async Task<ResultDashboardResponse> GetResultDashboardByQuery(IQueryable<int> inspectionIdList)
        {
            var res = await _dashboardManager.GetAPIRADashboardByQuery(inspectionIdList);

            if (res == null || !res.Any())
            {
                return new ResultDashboardResponse { Result = ManagementDashboardResult.NotFound };
            }

            return new ResultDashboardResponse
            {
                Data = res.OrderByDescending(x => x.TotalCount).ToList(),
                Result = ManagementDashboardResult.Success
            };
        }

        //get the average time between booking status change
        public async Task<AverageBookingTimeResponse> GetAverageBookingStatusChangeTime(ManagementDashboardRequest request)
        {
            AverageBookingTimeItem res = new AverageBookingTimeItem();
            double createdToVerifiedsum = 0;
            double verifiedToConfirmedsum = 0;
            double confirmedToScheeduledsum = 0;
            List<int> lstiterateitems = new List<int>();

            // get the inspection Query and get the query request
            var inspectionQuery = _sharedInspection.GetAllInspectionQuery();
            var inspectionQueryRequest = _sharedInspection.GetManagementDashboardInspectionRequestMap(request);
            //get the booking data query
            var bookingDataWithFilter = _sharedInspection.GetInspectionQuerywithRequestFilters(inspectionQueryRequest, inspectionQuery);

            bookingDataWithFilter = applyExternalFilters(request, bookingDataWithFilter);

            var bookingIds = bookingDataWithFilter.Select(x => x.Id);

            var bookingCount = await bookingIds.CountAsync();

            //get the status log details by booking Ids
            var data = _repo.GetBookingStatusLogsByQuery(bookingIds);

            if (!data.Any())
                return new AverageBookingTimeResponse { Result = ManagementDashboardResult.NotFound };

            double counter = 0;

            //the below foreach is to get the number of times the service date has changed

            var bookingData = data.Where(z => z.ServiceDateFrom != null && z.ServiceDateTo != null)
                .GroupBy(x => new { x.ServiceDateFrom, x.ServiceDateTo, x.BookingId }, (_key, group) => new
                {
                    key = _key
                });

            if (bookingData != null)
            {
                counter = await bookingData.Select(x => x.key).Distinct().CountAsync();
                counter = counter != 0 && bookingCount != 0 ? Math.Round(counter / bookingCount, 1) : 0;
            }

            //counter = await 

            //get the average service date change 
            res.DateRevisions = counter;

            if (!data.Any())
            {
                return new AverageBookingTimeResponse { Result = ManagementDashboardResult.NotFound };
            }


            var bookingCreatedData = await data.Where(x => x.StatusId == (int)BookingStatus.Received && x.StatusChangeDate.HasValue).Select(y =>
              new BookingByStatusChangeDate()
              {
                  BookingId = y.BookingId,
                  StatusChangeDate = y.StatusChangeDate
              }).Distinct().ToListAsync();

            var bookingVerifieddData = await data.Where(x => x.StatusId == (int)BookingStatus.Verified && x.StatusChangeDate.HasValue).Select(y =>
             new BookingByStatusChangeDate()
             {
                 BookingId = y.BookingId,
                 StatusChangeDate = y.StatusChangeDate
             }).Distinct().ToListAsync();

            var bookingConfirmeddData = await data.Where(x => x.StatusId == (int)BookingStatus.Confirmed && x.StatusChangeDate.HasValue).Select(y =>
            new BookingByStatusChangeDate()
            {
                BookingId = y.BookingId,
                StatusChangeDate = y.StatusChangeDate
            }).Distinct().ToListAsync();

            var bookingScheduleddData = await data.Where(x => x.StatusId == (int)BookingStatus.AllocateQC && x.StatusChangeDate.HasValue).Select(y =>
              new BookingByStatusChangeDate()
              {
                  BookingId = y.BookingId,
                  StatusChangeDate = y.StatusChangeDate
              }).Distinct().ToListAsync();

            //the below foreach loop is to get the number of days between each status change
            Parallel.ForEach(bookingCreatedData, item =>
            {
                if (!lstiterateitems.Contains(item.BookingId))
                {
                    //get the status change date from requested to verified status
                    var verifiedValue = bookingVerifieddData.Where(x => x.BookingId == item.BookingId).OrderByDescending(x => x.StatusChangeDate).FirstOrDefault();

                    //get the number of days 
                    createdToVerifiedsum = createdToVerifiedsum + (verifiedValue != null ? (verifiedValue.StatusChangeDate.GetValueOrDefault() - item.StatusChangeDate.GetValueOrDefault()).TotalDays : 0);

                    //get the status change date from verified to confirmed status
                    var confirmedValue = bookingConfirmeddData.Where(x => x.BookingId == item.BookingId).OrderByDescending(x => x.StatusChangeDate).FirstOrDefault();

                    //get the number of days 
                    verifiedToConfirmedsum = verifiedToConfirmedsum + (confirmedValue != null && verifiedValue != null ? (confirmedValue.StatusChangeDate.GetValueOrDefault() - verifiedValue.StatusChangeDate.GetValueOrDefault()).TotalDays : 0);

                    //get the status change date from confirmed to scheduled status
                    var scheduledValue = bookingScheduleddData.Where(x => x.BookingId == item.BookingId).OrderByDescending(x => x.StatusChangeDate).FirstOrDefault();

                    //get the number of days 
                    confirmedToScheeduledsum = confirmedToScheeduledsum + (scheduledValue != null && confirmedValue != null ? (scheduledValue.StatusChangeDate.GetValueOrDefault() - confirmedValue.StatusChangeDate.GetValueOrDefault()).TotalDays : 0);
                    lstiterateitems.Add(item.BookingId);
                }
            });

            //get the average number of days from requested status to verified status
            res.RequestToVerified = createdToVerifiedsum != 0 || bookingCreatedData.Count != 0 ? Math.Round(createdToVerifiedsum / bookingCreatedData.Count, 1) : 0;

            //get the average number of days from verfied status to confirmed status
            res.VerifiedToConfirmed = verifiedToConfirmedsum != 0 || bookingCreatedData.Count != 0 ? Math.Round(verifiedToConfirmedsum / bookingCreatedData.Count, 1) : 0;

            //get the average number of days from confirmed status to scheduled status
            res.ConfirmedToScheduled = confirmedToScheeduledsum != 0 || bookingCreatedData.Count != 0 ? Math.Round(confirmedToScheeduledsum / bookingCreatedData.Count, 1) : 0;

            return new AverageBookingTimeResponse
            {
                Data = res,
                Result = ManagementDashboardResult.Success
            };
        }

        //get the avergate time between status change in quotation
        public async Task<AverageQuotationTimeResponse> GetAverageQuotationStatusChangeTime(ManagementDashboardRequest request)
        {
            AverageQuotationTimeItem res = new AverageQuotationTimeItem();
            double createdToVerifiedsum = 0;
            double verifiedToSentToClientsum = 0;
            double sentToClientToValidatedsum = 0;
            List<int> lstiterateitems = new List<int>();

            // get the inspection Query and get the query request
            var inspectionQuery = _sharedInspection.GetAllInspectionQuery();
            var inspectionQueryRequest = _sharedInspection.GetManagementDashboardInspectionRequestMap(request);
            //get the booking data query
            var bookingDataWithFilter = _sharedInspection.GetInspectionQuerywithRequestFilters(inspectionQueryRequest, inspectionQuery);
            bookingDataWithFilter = applyExternalFilters(request, bookingDataWithFilter);

            var bookingIds = bookingDataWithFilter.Select(x => x.Id);

            if (!bookingIds.Any())
                return new AverageQuotationTimeResponse { Result = ManagementDashboardResult.NotFound };

            var data = _repo.GetQuotationStatusLogsByQuery(bookingIds);
            var bookingIdWithQuotation = await data.Select(x => x.BookingId).Distinct().ToListAsync();

            if (!data.Any())
            {
                return new AverageQuotationTimeResponse { Result = ManagementDashboardResult.NotFound };
            }

            var bookingData = await _repo.GetInspectionCreatedDateByQuery(bookingIds);

            var quotationVerifieddData = await data.Where(x => x.StatusId == (int)QuotationStatus.QuotationVerified || x.StatusId == (int)QuotationStatus.QuotationCreated).Select(y =>
              new BookingByStatusChangeDate()
              {
                  BookingId = y.BookingId.Value,
                  StatusChangeDate = y.StatusChangeDate
              }).ToListAsync();

            var quotationSenttoclientData = await data.Where(x => x.StatusId == (int)QuotationStatus.SentToClient).Select(y =>
                new BookingByStatusChangeDate()
                {
                    BookingId = y.BookingId.Value,
                    StatusChangeDate = y.StatusChangeDate
                }).ToListAsync();

            var quotationSentToClientData = await data.Where(x => x.StatusId == (int)QuotationStatus.CustomerValidated).Select(y =>
               new BookingByStatusChangeDate()
               {
                   BookingId = y.BookingId.Value,
                   StatusChangeDate = y.StatusChangeDate
               }).ToListAsync();

            //the below foreach loop is to get the number of days between each status change
            Parallel.ForEach(bookingIdWithQuotation, item =>
            {
                if (!lstiterateitems.Contains(item.GetValueOrDefault()))
                {

                    //get the status change date from booking created to quotation verified status
                    var quotCreatedDate = quotationVerifieddData.Where(x => x.BookingId == item && x.StatusChangeDate.HasValue).OrderByDescending(x => x.StatusChangeDate).FirstOrDefault();
                    var bookingCreatedDate = bookingData.FirstOrDefault(x => x.BookingId == item);

                    createdToVerifiedsum = createdToVerifiedsum + (quotCreatedDate != null ? (quotCreatedDate.StatusChangeDate.Value - bookingCreatedDate.CreatedOn.GetValueOrDefault()).Days : 0);

                    //get the status change date from verified to sent to client status
                    var quotSentToClientDate = quotationSenttoclientData.Where(x => x.BookingId == item && x.StatusChangeDate.HasValue).OrderByDescending(x => x.StatusChangeDate).FirstOrDefault();

                    verifiedToSentToClientsum = verifiedToSentToClientsum + (quotSentToClientDate != null && quotCreatedDate != null ? (quotSentToClientDate.StatusChangeDate.Value - quotCreatedDate.StatusChangeDate.Value).Days : 0);

                    //get the status change date from sent to client to validated status
                    var quotValidatedDate = quotationSentToClientData.Where(x => x.BookingId == item).OrderByDescending(x => x.StatusChangeDate).FirstOrDefault();

                    sentToClientToValidatedsum = sentToClientToValidatedsum + (quotValidatedDate != null && quotSentToClientDate != null ? (quotValidatedDate.StatusChangeDate.Value - quotSentToClientDate.StatusChangeDate.Value).Days : 0);
                    lstiterateitems.Add(item.GetValueOrDefault());
                }
            });

            //get the average number of days from booking created to quotation verified status
            res.RequestToVerified = createdToVerifiedsum != 0 || bookingIdWithQuotation.Count != 0 ? Math.Round(createdToVerifiedsum / bookingIdWithQuotation.Count, 1) : 0;

            //get the average number of days from verified to sent to client status
            res.VerifiedToSentToClient = verifiedToSentToClientsum != 0 || bookingIdWithQuotation.Count != 0 ? Math.Round(verifiedToSentToClientsum / bookingIdWithQuotation.Count, 1) : 0;

            //get the average number of days from sent to client to validated status
            res.SentToClientToValidated = sentToClientToValidatedsum != 0 || bookingIdWithQuotation.Count != 0 ? Math.Round(sentToClientToValidatedsum / bookingIdWithQuotation.Count, 1) : 0;

            res.TotalDays = Convert.ToInt32(res.RequestToVerified + res.VerifiedToSentToClient + res.SentToClientToValidated);

            return new AverageQuotationTimeResponse
            {
                Data = res,
                Result = ManagementDashboardResult.Success
            };
        }

        //export product category chart
        public async Task<ManagementDashboardChartExport> ExportProductCategoryChart(ManagementDashboardRequest request)
        {
            var response = new ManagementDashboardChartExport();

            // get the inspection Query and get the query request
            var inspectionQuery = _sharedInspection.GetAllInspectionQuery();
            var inspectionQueryRequest = _sharedInspection.GetManagementDashboardInspectionRequestMap(request);
            //get the booking data query
            var bookingData = _sharedInspection.GetInspectionQuerywithRequestFilters(inspectionQueryRequest, inspectionQuery);

            bookingData = applyExternalFilters(request, bookingData);

            var bookingIds = bookingData.Select(x => x.Id);


            //fetch the product category data
            var res = await ProductCategoryDashboardSearchbyQuery(bookingIds);

            if (res.Data != null && res.Data.Any())
            {
                response.Data = res.Data.Select(x => new ManagementDashboardExportItem
                {
                    Count = x.TotalCount,
                    Name = x.StatusName
                }).ToList();

                response.Total = res.Data.Sum(x => x.TotalCount);
            }

            //get the request details to be input in the excel
            response.RequestFilters = await SetExportFilter(request);

            return response;
        }

        public async Task<ManagementDashboardChartExport> ExportServiceTypeChart(ManagementDashboardRequest request)
        {
            var response = new ManagementDashboardChartExport();

            // get the inspection Query and get the query request
            var inspectionQuery = _sharedInspection.GetAllInspectionQuery();
            var inspectionQueryRequest = _sharedInspection.GetManagementDashboardInspectionRequestMap(request);
            //get the booking data query
            var bookingData = _sharedInspection.GetInspectionQuerywithRequestFilters(inspectionQueryRequest, inspectionQuery);

            bookingData = applyExternalFilters(request, bookingData);

            var inspectionIdList = bookingData.Select(x => x.Id);

            //fetch the service type data
            var res = await GetServiceTypeChartByQuery(inspectionIdList);

            if (res.Data != null && res.Data.Any())
            {
                response.Data = res.Data.Select(x => new ManagementDashboardExportItem
                {
                    Count = x.TotalCount,
                    Name = x.StatusName
                }).ToList();

                response.Total = res.Data.Sum(x => x.TotalCount);
            }
            //get the request details to be input in the excel
            response.RequestFilters = await SetExportFilter(request);

            return response;
        }

        public async Task<ManagementDashboardChartExport> ExportResultChart(ManagementDashboardRequest request)
        {
            var response = new ManagementDashboardChartExport();

            // get the inspection Query and get the query request
            var inspectionQuery = _sharedInspection.GetAllInspectionQuery();
            var inspectionQueryRequest = _sharedInspection.GetManagementDashboardInspectionRequestMap(request);
            //get the booking data query
            var bookingData = _sharedInspection.GetInspectionQuerywithRequestFilters(inspectionQueryRequest, inspectionQuery);

            bookingData = applyExternalFilters(request, bookingData);

            var inspectionIdList = bookingData.Select(x => x.Id);

            //fetch the result data
            var res = await GetResultDashboardByQuery(inspectionIdList);

            if (res.Data != null && res.Data.Any())
            {
                response.Data = res.Data.Select(x => new ManagementDashboardExportItem
                {
                    Count = x.TotalCount,
                    Name = x.StatusName
                }).ToList();

                response.Total = res.Data.Sum(x => x.TotalCount);
            }
            //get the request details to be input in the excel
            response.RequestFilters = await SetExportFilter(request);

            return response;
        }

        public async Task<ManagementDashboardRequestExport> SetExportFilter(ManagementDashboardRequest request)
        {
            var response = new ManagementDashboardRequestExport();

            response.ServiceDateFrom = request.ServiceDateFrom == null || !CheckDate(request.ServiceDateFrom) ? "" : request.ServiceDateFrom.ToDateTime().ToString(StandardDateFormat);
            response.ServiceDateTo = request.ServiceDateTo == null || !CheckDate(request.ServiceDateTo) ? "" : request.ServiceDateTo.ToDateTime().ToString(StandardDateFormat);

            //get the customer name
            if (request.CustomerId > 0)
            {
                var customers = await _cusManager.GetCustomerByCustomerId(request.CustomerId.GetValueOrDefault());
                response.Customer = customers.DataSourceList != null && customers.DataSourceList.Any() ? string.Join(", ", customers.DataSourceList.Select(x => x.Name)) : "";
            }

            //get the country name
            if (request.CountryIdList != null && request.CountryIdList.Any())
            {
                var countryList = await _locationRepo.GetCountryByIds(request.CountryIdList);
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
            if (request.FactoryIdList != null && request.FactoryIdList.Any())
            {
                var factList = await _supManager.GetSupplierById(request.FactoryIdList);
                response.FactoryList = factList.DataSourceList != null && factList.DataSourceList.Any() ? string.Join(", ", factList.DataSourceList.Select(x => x.Name)) : "";
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
        /// 
        /// </summary>
        /// <param name="countrylist"></param>
        /// <param name="supType"></param>
        /// <returns></returns>
        public async Task<List<int>> GetSupplierByCountryId(List<int> countrylist, int supType)
        {
            return await _repo.GetSupplierByCountryId(countrylist, supType);
        }
    }
}
