using BI.Utilities;
using Components.Core.contracts;
using Contracts.Managers;
using Contracts.Repositories;
using CsvHelper;
using DTO.Common;
using DTO.Customer;
using DTO.Expense;
using DTO.FullBridge;
using DTO.Inspection;
using DTO.MasterConfig;
using DTO.RepoRequest.Enum;
using DTO.Report;
using DTO.Schedule;
using DTO.ScheduleJob;
using Entities;
using Entities.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BI
{
    public class ScheduleJobManager : ApiCommonData, IScheduleJobManager
    {
        private readonly CulturaPackingSettings _culturaPackingSettings = null;
        private readonly IScheduleJobRepository _repo = null;
        private readonly IInspectionBookingRepository _insprepo = null;
        private readonly IReportFastTransactionRepository _reportFastTransactionRepository = null;
        private readonly HttpContext _httpContext;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IKpiCustomRepository _kpiCustomRepository;
        private readonly ISharedInspectionManager _helper = null;
        private readonly IHelper _apiHelper = null;
        private static IConfiguration _configuration = null;
        private readonly IFBReportManager _fBReportManager;
        private readonly IBroadCastService _broadCastService;
        private readonly IUserRepository _userRepository;
        private readonly INotificationManager _notificationManager;
        private readonly IHumanResourceManager _hrManager = null;
        private readonly ITenantProvider _filterService = null;
        private readonly IExchangeRateManager _currencyManager = null;
        private readonly IExpenseRepository _expenseRepository = null;
        private readonly IClaimRepository _claimRepo = null;
        private readonly IHumanResourceRepository _hrRepo = null;
        private readonly IUserConfigRepository _userConfigRepository = null;
        private readonly IInspectionBookingManager _inspectionBookingManager = null;
        private readonly IFullBridgeRepository _fbRepo = null;

        public ScheduleJobManager(IOptions<CulturaPackingSettings> culturaPackingSettings, IConfiguration configuration, IHelper apiHelper, IFileManager fileManager,
            IHttpContextAccessor httpContextAccessor, IFBReportManager fBReportManager, IBroadCastService broadCastService, IUserRepository userRepository, INotificationManager notificationManager, IKpiCustomRepository kpiCustomRepository,
            IScheduleJobRepository repo, ISharedInspectionManager helper, IInspectionBookingRepository insprepo, IReportFastTransactionRepository reportFastTransactionRepository, IAPIUserContext ApplicationContext,
            IHumanResourceManager hrManager, ITenantProvider filterService, IExchangeRateManager currencyManager, IExpenseRepository expenseRepository, IClaimRepository claimRepo, IHumanResourceRepository hrRepo, IUserConfigRepository userConfigRepository,
             IInspectionBookingManager inspectionBookingManager, IScheduleRepository scheduleRepository, IFullBridgeRepository fbRepo)
        {
            _culturaPackingSettings = culturaPackingSettings.Value;
            _repo = repo;
            _helper = helper;
            _apiHelper = apiHelper;
            _configuration = configuration;
            _fBReportManager = fBReportManager;
            _broadCastService = broadCastService;
            _userRepository = userRepository;
            _notificationManager = notificationManager;
            _hrManager = hrManager;
            _filterService = filterService;
            _currencyManager = currencyManager;
            _insprepo = insprepo;
            _expenseRepository = expenseRepository;
            _claimRepo = claimRepo;
            _hrRepo = hrRepo;
            _userConfigRepository = userConfigRepository;
            _reportFastTransactionRepository = reportFastTransactionRepository;
            _httpContext = httpContextAccessor.HttpContext;
            _inspectionBookingManager = inspectionBookingManager;
            _scheduleRepository = scheduleRepository;
            _kpiCustomRepository = kpiCustomRepository;
            _fbRepo = fbRepo;
        }
        public async Task<bool> SaveCulturaPackingInfo()
        {
            try
            {
                DateTime startDate = DateTime.ParseExact(_culturaPackingSettings.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime endDate = DateTime.Now;

                var actualStartDate = DateTime.Now.AddDays(-_culturaPackingSettings.ScheduleInterval);
                if (actualStartDate > startDate)
                {
                    startDate = actualStartDate;
                }

                var reportProductQuery = _repo.GetCulturaScheduleJobData((int)CustomerList.Cultura);

                // fetch only validated report products
                reportProductQuery = reportProductQuery.Where(x => InspectedStatusList.Contains(x.Inspection.StatusId));

                // check the report data is already not exist in the log table
                reportProductQuery = reportProductQuery.Where(x => !x.FbReport.JobScheduleLogs.Any());

                reportProductQuery = reportProductQuery.Where(x => !((x.Inspection.ServiceDateFrom > endDate) || (x.Inspection.ServiceDateTo < startDate)));

                var reportProductData = await reportProductQuery.Select(x =>
                  new ScheduleJobResponse()
                  {
                      BookingId = x.InspectionId,
                      ReportId = x.FbReport.Id,
                      InspectionEndDate = x.FbReport.ServiceToDate.HasValue ? x.FbReport.ServiceToDate.Value : x.Inspection.ServiceDateTo,
                      ReportNumber = x.FbReport.ReportTitle,
                      ProductRefernce = x.Product.ProductId,
                      Ean = x.Product.FbReportProductBarcodesInfos.Where(y => y.FbReportId == x.FbReportId).FirstOrDefault(x => !string.IsNullOrEmpty(x.BarCode) && x.BarCode.Trim().StartsWith("3700")).BarCode,
                      MeasuredHeight = x.Product.FbReportPackingDimentions.Where(y => y.FbReportId == x.FbReportId).FirstOrDefault(x => x.Active.Value && x.PackingType == "outer-carton").MeasuredValuesH,
                      MeasuredLength = x.Product.FbReportPackingDimentions.Where(y => y.FbReportId == x.FbReportId).FirstOrDefault(x => x.Active.Value && x.PackingType == "outer-carton").MeasuredValuesL,
                      MeasuredWeight = x.Product.FbReportPackingDimentions.Where(y => y.FbReportId == x.FbReportId).FirstOrDefault(x => x.Active.Value && x.PackingType == "outer-carton").MeasuredValuesW,
                      MeasuredPackingWeight = x.Product.FbReportPackingWeights.Where(y => y.FbReportId == x.FbReportId).FirstOrDefault(x => x.Active.Value && x.PackingType == "outer-carton").MeasuredValues,
                      MeasuredPackingWeightUnit = x.Product.FbReportPackingWeights.Where(y => y.FbReportId == x.FbReportId).FirstOrDefault(x => x.Active.Value && x.PackingType == "outer-carton").Unit,
                  }).AsNoTracking().ToListAsync();
                //pick the product with ean value
                reportProductData = reportProductData.Where(x => x.Ean != null && x.Ean.Length > 0).ToList();

                foreach (ScheduleJobResponse item in reportProductData.ToList())
                {
                    item.Ean = !string.IsNullOrEmpty(item.Ean) ? item.Ean.Trim() : "";
                    item.ProductRefernce = item.ProductRefernce.Replace("\"", string.Empty).Trim();
                    item.MeasuredHeight = !string.IsNullOrEmpty(item.MeasuredHeight) ? item.MeasuredHeight.Trim() : "0";
                    item.MeasuredLength = !string.IsNullOrEmpty(item.MeasuredLength) ? item.MeasuredLength.Trim() : "0";
                    item.MeasuredWeight = !string.IsNullOrEmpty(item.MeasuredWeight) ? item.MeasuredWeight.Trim() : "0";
                    item.MeasuredPackingWeight = !string.IsNullOrEmpty(item.MeasuredPackingWeight) ? item.MeasuredPackingWeight.Trim() : "0";
                    if (item.MeasuredPackingWeightUnit == "g")
                    {
                        item.MeasuredPackingWeight = (Convert.ToDouble(item.MeasuredPackingWeight) / 1000).ToString();
                    }
                }

                if (reportProductData.Any())
                {
                    SaveReportsData(reportProductData);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Save auto qc info updates.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveScheduleAutoQcExpenseInfo(List<int> travelExpenseIds, List<int> foodExpenseIds)
        {
            try
            {
                // Get all the active pending Expense Item (Travel and Food Expense)
                var qcTravelExpenses = await _repo.GetAutoQcTravelExpenseData(travelExpenseIds);
                var qcFoodExpenses = await _repo.GetAutoQcFoodExpenseData(foodExpenseIds);

                // Merge both Travel and Food Expense into Pending Expense
                var pendinngExpenseList = getAutoPendingExpenseList(qcTravelExpenses, qcFoodExpenses);

                var qcList = pendinngExpenseList.Select(x => x.QcId.GetValueOrDefault()).Distinct().ToList();
                var bookingList = pendinngExpenseList.Select(x => x.BookingId).Distinct().ToList();

                // Get already available expense for Qc List and booking List
                var createdExpenseList = await _expenseRepository.GetExpenseClaimByQcAndBookingList(qcList, bookingList);

                // select only booking ids from existing expense
                var createdExpenseBookingIds = createdExpenseList.
                                                  SelectMany(x => x.EcExpensesClaimDetais).Select(x => x.InspectionId).Distinct().ToList();


                // check the expense already mapped to booking list - need to update those booking with existing expense
                var updatePendingExpenseList = pendinngExpenseList.Where(x => createdExpenseBookingIds.Contains(x.BookingId)).ToList();

                // check the expense not mapped to booking list - need to cerate as new expense
                var newPendingExpenseList = pendinngExpenseList.Where(x => !createdExpenseBookingIds.Contains(x.BookingId)).ToList();

                // Fetch startport and Endport details for later configuration
                var pendingExpenseQcList = pendinngExpenseList.Select(x => x.QcId).Distinct().ToList();
                var starPortIds = qcTravelExpenses.Where(x => pendingExpenseQcList.Contains(x.QcId) && x.StartPort != null)
                                    .Select(x => x.StartPort).Distinct().ToList();
                var townIds = qcTravelExpenses.Where(x => pendingExpenseQcList.Contains(x.QcId) && x.FactoryTown != null)
                                 .Select(x => x.FactoryTown).Distinct().ToList();

                var startPortCityList = await _repo.GetCityIdByStartPortList(starPortIds);
                var townCityList = await _repo.GetCityIdByTownIdList(townIds);


                // add new Expense
                if (newPendingExpenseList.Any())
                {
                    // group by qc id and service date and create the expense dynamically
                    var groupTravelExpensebyQcAndDate = newPendingExpenseList.GroupBy(x => new { x.QcId }).Select(y => y).ToList();

                    foreach (var newExpenseGroup in groupTravelExpensebyQcAndDate)
                    {

                        // get staff expense related base data
                        var staffId = newExpenseGroup.Select(x => x.QcId.Value).FirstOrDefault();
                        var expenseData = await getExpenseClaimData(staffId);

                        // build new expense entity
                        var expenseItem = new EcExpencesClaim
                        {
                            LocationId = expenseData.LocationId,
                            StaffId = expenseData.StaffId,
                            Active = true,
                            CountryId = expenseData.CountryId,
                            CreatedDate = DateTime.Now,
                            ExpensePurpose = "Auto Qc Expense for the date" + DateTime.Now.ToString(),
                            ClaimDate = expenseData.ClaimDate.ToDateTime(),
                            ClaimNo = expenseData.ClaimNo,
                            StatusId = (int)ExpenseClaimStatus.Pending,
                            PaymentTypeId = 1,
                            EntityId = _filterService.GetCompanyId(),
                            ClaimTypeId = expenseData.ClaimTypeId,
                            IsAutoExpense = true
                        };

                        foreach (var pendingExpense in newExpenseGroup)
                        {

                            if (pendingExpense.ExpenseTypeId == (int)AutoQcExpenseType.TravellAllowance)
                            {
                                await AddTravelPendingExpense(expenseItem, qcTravelExpenses, pendingExpense, expenseData.CurrencyId.GetValueOrDefault(), startPortCityList, townCityList);
                            }
                            else if (pendingExpense.ExpenseTypeId == (int)AutoQcExpenseType.FoodAllowance)
                            {
                                await AddFoodPendingExpense(expenseItem, qcFoodExpenses, pendingExpense, expenseData.CurrencyId.GetValueOrDefault());
                            }
                        }

                        _repo.Save(expenseItem, false);
                    }
                }

                // update pending  expense with existing expense

                if (updatePendingExpenseList.Any())
                {

                    foreach (var pendingExpense in updatePendingExpenseList)
                    {
                        var expenseItem = createdExpenseList.FirstOrDefault(x => x.StaffId == pendingExpense.QcId &&
                                                    x.EcExpensesClaimDetais.Any(x => x.InspectionId == pendingExpense.BookingId));

                        var expenseCurrency = createdExpenseList.Where(x => x.StaffId == pendingExpense.QcId &&
                                                 x.EcExpensesClaimDetais.Any(x => x.InspectionId == pendingExpense.BookingId))
                                                  .SelectMany(x => x.EcExpensesClaimDetais).FirstOrDefault();

                        if (expenseItem != null && expenseCurrency != null)
                        {
                            if (pendingExpense.ExpenseTypeId == (int)AutoQcExpenseType.TravellAllowance)
                            {
                                await AddTravelPendingExpense(expenseItem, qcTravelExpenses, pendingExpense, expenseCurrency.CurrencyId, startPortCityList, townCityList);
                            }
                            else if (pendingExpense.ExpenseTypeId == (int)AutoQcExpenseType.FoodAllowance)
                            {
                                await AddFoodPendingExpense(expenseItem, qcFoodExpenses, pendingExpense, expenseCurrency.CurrencyId);
                            }
                            // set auto expense 
                            expenseItem.IsAutoExpense = true;
                            _repo.Save(expenseItem);
                        }
                    }
                }


                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private async Task AddTravelPendingExpense(EcExpencesClaim expenseItem, List<EcAutQcTravelExpense> qcTravelExpenses,
            QcPendingExpenseData qcPendingExpenseData, int expenseCurrency, List<StartPortCity> startPortCityList, List<FactoryTownCity> townCityList)
        {
            var travelExpenseItem = qcTravelExpenses.FirstOrDefault(x =>
            x.QcId == qcPendingExpenseData.QcId &&
            x.ServiceDate == qcPendingExpenseData.ServiceDate &&
            x.InspectionId == qcPendingExpenseData.BookingId);

            if (travelExpenseItem != null)
            {


                var exchangeRate = await _currencyManager.GetExchangeRate(expenseCurrency,
                                    travelExpenseItem.TravelTariffCurrency.GetValueOrDefault(),
                                    travelExpenseItem.ServiceDate.GetCustomDate(), ExhangeRateTypeEnum.ExpenseClaim);

                // calculate amount by exchange rate
                var actualAmount = travelExpenseItem.TravelTariff.GetValueOrDefault();
                var amount = Math.Round(double.Parse(exchangeRate) * actualAmount * 100) / 100;

                var travelExpenseDetails = new EcExpensesClaimDetai
                {
                    Active = true,
                    Receipt = false,
                    ExpenseDate = travelExpenseItem.ServiceDate.GetValueOrDefault(),
                    Description = "Auto Qc Travel Expense",
                    EntityId = _filterService.GetCompanyId(),
                    ExchangeRate = double.Parse(exchangeRate),
                    AmmountHk = actualAmount,
                    Amount = amount,
                    TripType = travelExpenseItem.TripType,
                    ExpenseTypeId = (int)ClaimExpenseType.TravellingOtherModes,
                    CurrencyId = travelExpenseItem.TravelTariffCurrency != null ?
                    travelExpenseItem.TravelTariffCurrency.GetValueOrDefault() : qcPendingExpenseData.PayrollCurrency.GetValueOrDefault(),
                    StartCityId = startPortCityList.FirstOrDefault(x => x.StartPortId == travelExpenseItem.StartPort).CityId,
                    ArrivalCityId = townCityList.FirstOrDefault(x => x.TownId == travelExpenseItem.FactoryTown).CityId,
                    IsAutoExpense = true,
                    InspectionId = travelExpenseItem.InspectionId ?? 0
                };

                // set refernce for auto qc expense
                travelExpenseDetails.QcTravelExpense = travelExpenseItem;

                expenseItem.EcExpensesClaimDetais.Add(travelExpenseDetails);

                travelExpenseItem.IsExpenseCreated = true;


                _repo.AddEntity(travelExpenseDetails);

                _repo.EditEntity(travelExpenseItem);

            }
        }

        private async Task AddFoodPendingExpense(EcExpencesClaim expenseItem, List<EcAutQcFoodExpense> qcFoodExpenses,
          QcPendingExpenseData qcPendingExpenseData, int expenseCurrency)
        {
            var foodExpenseItem = qcFoodExpenses.FirstOrDefault(x => x.QcId == qcPendingExpenseData.QcId &&
                                    x.ServiceDate == qcPendingExpenseData.ServiceDate
                                    && x.InspectionId == qcPendingExpenseData.BookingId);

            if (foodExpenseItem != null)
            {

                var exchangeRate = await _currencyManager.GetExchangeRate(expenseCurrency,
                        foodExpenseItem.FoodAllowanceCurrency.GetValueOrDefault(),
                        foodExpenseItem.ServiceDate.GetCustomDate(), ExhangeRateTypeEnum.ExpenseClaim);

                var actualAmount = foodExpenseItem.FoodAllowance.GetValueOrDefault();
                var amount = Math.Round(double.Parse(exchangeRate) * actualAmount * 100) / 100;

                var foodAllowanceDetails = new EcExpensesClaimDetai
                {
                    Active = true,
                    Receipt = false,
                    ExpenseDate = foodExpenseItem.ServiceDate.Value,
                    Description = "Auto Qc Food allowance Expense",
                    EntityId = _filterService.GetCompanyId(),
                    ExchangeRate = double.Parse(exchangeRate),
                    AmmountHk = actualAmount,
                    Amount = amount,
                    ExpenseTypeId = (int)ClaimExpenseType.FoodAllowance,
                    CurrencyId = foodExpenseItem.FoodAllowanceCurrency != null ?
                    foodExpenseItem.FoodAllowanceCurrency.GetValueOrDefault() : qcPendingExpenseData.PayrollCurrency.GetValueOrDefault(),
                    IsAutoExpense = true,
                    InspectionId = foodExpenseItem.InspectionId ?? 0
                };


                foodExpenseItem.IsExpenseCreated = true;
                // set refernce for auto qc expense
                foodAllowanceDetails.QcFoodExpense = foodExpenseItem;
                expenseItem.EcExpensesClaimDetais.Add(foodAllowanceDetails);
                _repo.AddEntity(foodAllowanceDetails);
                // update edit entity
                _repo.EditEntity(foodExpenseItem);


            }
        }

        /// <summary>
        /// Merge Travel and Food expense as Pending expense List
        /// </summary>
        /// <param name="travelExpenseList"></param>
        /// <param name="foodExpenseList"></param>
        /// <returns></returns>
        public List<QcPendingExpenseData> getAutoPendingExpenseList(List<EcAutQcTravelExpense> travelExpenseList, List<EcAutQcFoodExpense> foodExpenseList)
        {
            List<QcPendingExpenseData> pendingExpenseList = new List<QcPendingExpenseData>();

            if (travelExpenseList.Any())
            {
                foreach (var item in travelExpenseList)
                {
                    pendingExpenseList.Add(new QcPendingExpenseData()
                    {
                        QcId = item.QcId,
                        BookingId = item.InspectionId.GetValueOrDefault(),
                        ExpenseTypeId = 1, // travel expense
                        ServiceDate = item.ServiceDate.GetValueOrDefault(),
                        PayrollCurrency = item.Qc.PayrollCurrencyId
                    });
                }
            }

            if (foodExpenseList.Any())
            {
                foreach (var item in foodExpenseList)
                {
                    pendingExpenseList.Add(new QcPendingExpenseData()
                    {
                        QcId = item.QcId,
                        BookingId = item.InspectionId.GetValueOrDefault(),
                        ExpenseTypeId = 2, // food expense
                        ServiceDate = item.ServiceDate.GetValueOrDefault(),
                        PayrollCurrency = item.Qc.PayrollCurrencyId
                    });
                }
            }

            return pendingExpenseList;
        }



        /// <summary>
        /// Get expense claim data.
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        private async Task<ExpenseClaim> getExpenseClaimData(int staffId)
        {
            var expense = new ExpenseClaim();

            var currentStaff = await _hrManager.GetStaffDetailsByStaffId(staffId);

            // get current HR 

            expense.Name = currentStaff.StaffName;
            expense.StaffId = currentStaff.Id;
            expense.StaffEmail = currentStaff.Email;

            // Location
            expense.LocationId = currentStaff.LocationId.Value;
            expense.LocationName = currentStaff.LocationName;
            expense.CountryId = currentStaff.CountryId;

            expense.CurrencyId = currentStaff.CurrencyId.Value;
            expense.CurrencyName = currentStaff.CurrencyName;

            expense.ClaimTypeId = (int)ClaimTypeEnum.Inspection;

            expense.ClaimDate = DateTime.Now.GetCustomDate();
            expense.ClaimNo = $"{(currentStaff.StaffName.Length >= 4 ? currentStaff.StaffName.Substring(0, 4) : currentStaff.StaffName)}-{DateTime.Now.ToString("yyyyMMdd-HHmmssfff")}";

            return expense;
        }

        /// <summary>
        /// Get inactivated travel tariff details
        /// </summary>
        /// <returns></returns>
        public async Task<List<ScheduleTravelTariffEmail>> GetInActivatTravelTariffList()
        {
            try
            {
                var dataList = await _repo.GetInActivatTravelTariffList();
                var travellTariffEmailUrl = _configuration["ScheduleTravellTariffEmail"].ToString();
                dataList.ForEach(x => x.TravelTariffUrl = string.Format(travellTariffEmailUrl, x.Id));
                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<JobConfiguration>> getJobConfigurationList()
        {
            try
            {
                var dataList = await _repo.GetScheduleJobConfigurationList();
                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<JobConfiguration>> GetJobConfigurations(List<int> typeIds)
        {
            try
            {
                var dataList = await _repo.GetScheduleJobConfigurations(typeIds);
                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<QcExpenseEmailData>> GetQcExpenseList()
        {
            try
            {
                var travelExpenseListQueryable = _repo.GetQcTravelExpenseData();
                var foodExpenseListQueryable = _repo.GetQcFoodExpenseData();
                var today = DateTime.Today;
                var tomorrow = today.AddDays(1);


                // apply date filter to get tomorrow expense data.

                var dataList = await travelExpenseListQueryable.Where(x => x.ServiceDate == tomorrow)
                                .Select(x => new QcExpenseEmailData()
                                {
                                    BookingId = x.InspectionId.GetValueOrDefault(),
                                    StartPort = x.StartPortNavigation.StartPortName,
                                    EndPort = x.FactoryTownNavigation.TownName,
                                    CustomerName = x.Inspection.Customer.CustomerName,
                                    TravelFee = x.TravelTariff.GetValueOrDefault(),
                                    TripMode = x.TripTypeNavigation.Name,
                                    OrderQty = x.Inspection.InspProductTransactions.Where(x=>x.Active==true).Select(x => x.TotalBookingQuantity).Sum(),
                                    QcId = x.QcId,
                                    QcName = x.Qc.PersonName,
                                    QcEmail = x.Qc.CompanyEmail,
                                    IsChinaCountry = x.Qc.NationalityCountryId == (int)CountryEnum.China,
                                    Currency=x.Qc.PayrollCurrency.CurrencyCodeA
                                }).AsNoTracking().ToListAsync();

                var rowFoodAllowanceList = await foodExpenseListQueryable.Where(x => x.ServiceDate == tomorrow)
                                .Select(x => new QcExpenseEmailData()
                                {
                                    BookingId = x.InspectionId.GetValueOrDefault(),
                                    QcId = x.QcId,
                                    QcName = x.Qc.PersonName,
                                    FoodAllowance = x.FoodAllowance,
                                    Currency= x.Qc.PayrollCurrency.CurrencyCodeA
                                }).AsNoTracking().ToListAsync();

                var bookingIds = dataList.Select(x => x.BookingId).Distinct().ToList();

                var serviceTypeList = await _insprepo.GetServiceType(bookingIds);
                var factoryDetails = await _insprepo.GetFactorycountryId(bookingIds);

                // update booking related items.
                foreach (var item in dataList)
                {
                    item.ServiceType = serviceTypeList.FirstOrDefault(x => x.InspectionId == item.BookingId)?.serviceTypeName;
                    item.FactoryENAddress = factoryDetails.FirstOrDefault(x => x.BookingId == item.BookingId)?.FactoryAdress;
                    item.FactoryLocalAddress = factoryDetails.FirstOrDefault(x => x.BookingId == item.BookingId)?.FactoryRegionalAddress;
                    item.FoodAllowance = rowFoodAllowanceList.FirstOrDefault(x => x.QcId == item.QcId)?.FoodAllowance;
                }

                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// get pending auto qc list by search filters
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        public async Task<List<QcPendingExpenseData>> GetPendingQcExpenseList(QcPendingExpenseRequest request)
        {
            try
            {
                var travelExpenseListQueryable = _repo.GetQcTravelExpenseData();
                var foodExpenseListQueryable = _repo.GetQcFoodExpenseData();

                var startDate = DateTime.Now.AddDays(-1);
                var endDate = DateTime.Now.AddDays(-1);

                // filter by service date
                if (request.StartDate != null && request.EndDate != null)
                {
                    startDate = request.StartDate.ToDateTime();
                    endDate = request.EndDate.ToDateTime();

                    // if the requested date is greater or equal set yesterday date.
                    if (startDate >= DateTime.Now)
                    {
                        startDate = DateTime.Now.AddDays(-1);
                    }

                    if (endDate >= DateTime.Now)
                    {
                        endDate = DateTime.Now.AddDays(-1);
                    }

                    travelExpenseListQueryable = travelExpenseListQueryable.
                          Where(x => x.ServiceDate >= startDate && x.ServiceDate <= endDate);
                    foodExpenseListQueryable = foodExpenseListQueryable.
                          Where(x => x.ServiceDate >= startDate && x.ServiceDate <= endDate);
                }

                else
                {
                    travelExpenseListQueryable = travelExpenseListQueryable.
                          Where(x => x.ServiceDate >= startDate && x.ServiceDate <= endDate);
                    foodExpenseListQueryable = foodExpenseListQueryable.
                          Where(x => x.ServiceDate >= startDate && x.ServiceDate <= endDate);
                }
                // filter by Booking Id
                // filter by Booking Id
                if (request.SearchTypeId == (int)SearchType.BookingNo)
                {
                    if (!string.IsNullOrEmpty(request.SearchTypeText?.Trim()) && int.TryParse(request.SearchTypeText?.Trim(), out int bookingId))
                    {
                        travelExpenseListQueryable = travelExpenseListQueryable.Where(x => x.InspectionId == bookingId);
                        foodExpenseListQueryable = foodExpenseListQueryable.Where(x => x.InspectionId == bookingId);
                    }
                }
                // filter by office 
                if (request.OfficeIdList != null && request.OfficeIdList.Any())
                {
                    travelExpenseListQueryable = travelExpenseListQueryable.Where(x => request.OfficeIdList.Contains(x.Qc.LocationId.GetValueOrDefault()));
                    foodExpenseListQueryable = foodExpenseListQueryable.Where(x => request.OfficeIdList.Contains(x.Qc.LocationId.GetValueOrDefault()));
                }
                // filter by Qc  
                if (request.QcIdList != null && request.QcIdList.Any())
                {
                    travelExpenseListQueryable = travelExpenseListQueryable.Where(x => request.QcIdList.Contains(x.QcId.GetValueOrDefault()));
                    foodExpenseListQueryable = foodExpenseListQueryable.Where(x => request.QcIdList.Contains(x.QcId.GetValueOrDefault()));
                }

                // filter by status  
                if (request.StatusId > 0)
                {
                    if (request.StatusId == (int)AutoQcPendingStatus.Configured)
                    {
                        travelExpenseListQueryable = travelExpenseListQueryable.Where(x => x.IsTravelAllowanceConfigured.Value);
                        foodExpenseListQueryable = foodExpenseListQueryable.Where(x => x.IsFoodAllowanceConfigured.Value);
                    }
                    else if (request.StatusId == (int)AutoQcPendingStatus.NotConfigured)
                    {
                        travelExpenseListQueryable = travelExpenseListQueryable.Where(x => !x.IsTravelAllowanceConfigured.Value);
                        foodExpenseListQueryable = foodExpenseListQueryable.Where(x => !x.IsFoodAllowanceConfigured.Value);
                    }
                }

                var travelAllowanceList = await travelExpenseListQueryable.Where(x => !x.IsExpenseCreated.Value)
                                .Select(x => new QcPendingExpenseData()
                                {
                                    Id = x.Id,
                                    ServiceDate = x.ServiceDate.GetValueOrDefault(),
                                    BookingId = x.InspectionId.GetValueOrDefault(),
                                    QcId = x.QcId,
                                    QcName = x.Qc.PersonName,
                                    StartPort = x.StartPortNavigation.StartPortName,
                                    FactoryTown = x.FactoryTownNavigation.TownName,
                                    FactoryCity = x.FactoryTownNavigation.County.City.CityName,
                                    FactoryProvince = x.FactoryTownNavigation.County.City.Province.ProvinceName,
                                    FactoryCounty = x.FactoryTownNavigation.County.CountyName,
                                    FactoryCountry = x.FactoryTownNavigation.County.City.Province.Country.CountryName,
                                    TravelAllowance = x.TravelTariff.GetValueOrDefault(),
                                    TripType = x.TripTypeNavigation.Name,
                                    CustomerName = x.Inspection.Customer.CustomerName,
                                    SupplierName = x.Inspection.Supplier.SupplierName,
                                    ExpenseTypeId = 1,
                                    Status = x.IsTravelAllowanceConfigured.GetValueOrDefault()
                                }).ToListAsync();

                var foodAllowanceList = await foodExpenseListQueryable.Where(x => !x.IsExpenseCreated.Value)
                                .Select(x => new QcPendingExpenseData()
                                {
                                    Id = x.Id,
                                    ServiceDate = x.ServiceDate.GetValueOrDefault(),
                                    BookingId = x.InspectionId.GetValueOrDefault(),
                                    QcId = x.QcId,
                                    QcName = x.Qc.PersonName,
                                    CustomerName = x.Inspection.Customer.CustomerName,
                                    SupplierName = x.Inspection.Supplier.SupplierName,
                                    Status = x.IsFoodAllowanceConfigured.GetValueOrDefault(),
                                    FactoryCounty = x.FactoryCountryNavigation.CountryName,
                                    FoodAllowance = x.FoodAllowance.GetValueOrDefault(),
                                    ExpenseTypeId = 2
                                }).ToListAsync();

                var expenseList = travelAllowanceList.Concat(foodAllowanceList).ToList();

                var bookingIds = expenseList.Select(x => x.BookingId).ToList();


                var factoryDetails = await _insprepo.GetFactorycountryId(bookingIds);

                // update factory country

                foreach (var item in expenseList)
                {
                    item.FactoryCountry = factoryDetails.FirstOrDefault(x => x.BookingId == item.BookingId)?.CountryName;
                }


                // filter by expense type  
                if (request.ExpenseTypeId > 0)
                {
                    if (request.ExpenseTypeId == (int)AutoQcExpenseType.TravellAllowance)
                    {
                        expenseList = expenseList.Where(x => x.ExpenseTypeId == (int)AutoQcExpenseType.TravellAllowance).ToList();
                    }
                    else if (request.ExpenseTypeId == (int)AutoQcExpenseType.FoodAllowance)
                    {
                        expenseList = expenseList.Where(x => x.ExpenseTypeId == (int)AutoQcExpenseType.FoodAllowance).ToList();
                    }
                }

                return expenseList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void SaveReportsData(List<ScheduleJobResponse> reportProductData)
        {
            List<JobScheduleLog> objDbLogList = new List<JobScheduleLog>();
            List<ScheduleJobCsvResponse> objCsvList = new List<ScheduleJobCsvResponse>();
            string strFileName = _culturaPackingSettings.FileName + "_" + DateTime.Now.Year + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + ".csv";
            foreach (var item in reportProductData)
            {
                objDbLogList.Add(new JobScheduleLog()
                {
                    BookingId = item.BookingId,
                    ReportId = item.ReportId,
                    FileName = strFileName,
                    CreatedOn = DateTime.Now,
                    ScheduleType = (int)ScheduleType.Cultura,
                    EntityId = _filterService.GetCompanyId()
                });

                objCsvList.Add(new ScheduleJobCsvResponse()
                {
                    ReportNumber = item.ReportNumber,
                    InspectionDate = item.InspectionEndDate.ToString(StandardDateFormat5),
                    ProductRefernce = item.ProductRefernce,
                    Ean = item.Ean,
                    MeasuredHeight = item.MeasuredHeight,
                    MeasuredLength = item.MeasuredLength,
                    MeasuredWeight = item.MeasuredWeight,
                    MeasuredPackingWeight = item.MeasuredPackingWeight
                });
            }

            using (var mem = new MemoryStream())
            using (var writer = new StreamWriter(mem))
            using (var csvWriter = new CsvWriter(writer))
            {
                csvWriter.Configuration.Delimiter = ",";
                csvWriter.Configuration.HasHeaderRecord = true;
                csvWriter.WriteRecords(objCsvList);
                writer.Flush();
                string strFilePath = Path.Combine(_culturaPackingSettings.FolderPath, strFileName);
                FileStream fs = new FileStream(strFilePath, FileMode.Create, FileAccess.ReadWrite);
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(mem.ToArray());
                bw.Close();
            }
            // save to db
            _repo.SaveList(objDbLogList, false);
        }

        public async Task<IQueryable<ClmTransaction>> GetAllClaims()
        {
            return await _claimRepo.GetAllClaimsQuery();
        }

        public async Task<List<ScheduleClaimReminderEmailItem>> GetClaimMailDetail(IQueryable<ClmTransaction> claimQuery, int statusId)
        {
            var currentDate = DateTime.Now;
            if (statusId == (int)ClaimStatus.Validated)
            {
                claimQuery = claimQuery.Where(x => x.StatusId == statusId && currentDate.AddDays(-7) > x.ClaimDate);
            }
            else
            {
                claimQuery = claimQuery.Where(x => x.StatusId == statusId && currentDate.AddDays(-14) > x.ClaimDate);
            }

            var scheduleClaimReminderData = await _repo.GetClaimReminderList(claimQuery);

            var masterConfigs = await _userConfigRepository.GetMasterConfiguration();
            var entityName = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();
            string baseUrl = _configuration["BaseUrl"];

            return scheduleClaimReminderData.Select(x => new ScheduleClaimReminderEmailItem
            {
                ClaimId = x.ClaimId,
                ClaimNo = x.ClaimNo,
                ClaimURL = baseUrl + string.Format(_configuration["UrlClaimRequest"], x.ClaimId, entityName),
                InspectionURL = baseUrl + string.Format(_configuration["UrlBookingRequest"], x.BookingId, entityName),
                ClaimDate = x.ClaimDate.Value.ToString(StandardDateFormat),
                BookingId = x.BookingId,
                CustomerId = x.CustomerId,
                SupplierId = x.SupplierId,
                CustomerName = x.CustomerName,
                SupplierName = x.SupplierName,
                FactoryName = x.FactoryName,
                InspectionDate = x.ServiceDateFrom == x.ServiceDateTo ? x.ServiceDateFrom.ToString(StandardDateFormat) : string.Join(" - ", x.ServiceDateFrom.ToString(StandardDateFormat), x.ServiceDateTo.ToString(StandardDateFormat)),
                StatusName = x.StatusName,
                Office = x.Office,
                OfficeId = x.OfficeId.GetValueOrDefault(),
                StatusId = x.StatusId,
                CreatedBy = x.CreatedBy.GetValueOrDefault()
            }).ToList();
        }

        public async Task<List<string>> GetUserEmailsByIds(List<int> userids)
        {
            var staffList = await _hrRepo.GetStaffListByUserIds(userids);
            return staffList.Select(x => x.EmaiLaddress).ToList();
        }

        /// <summary>
        /// get the fast report url and update into fb system
        /// </summary>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        public async Task<PushReportInfoToFastReportResponse> PushReportInfoToFastReport(string fbToken)
        {
            //check token is available or not
            if (!_httpContext.Request.Headers.TryGetValue("Authorization", out StringValues token))
            {
                return new PushReportInfoToFastReportResponse() { Result = false };

            }

            //check entityid is available or not
            if (!_httpContext.Request.Headers.TryGetValue("entityId", out StringValues requestEntityId))
                return new PushReportInfoToFastReportResponse() { Result = false };

            //fetch the not started or (failed and error count less than 3 reports) 
            var notStartedReports = await _reportFastTransactionRepository.GetNotStartedOrErrorReportFastTransactions();

            if (!notStartedReports.Any())
            {
                return new PushReportInfoToFastReportResponse() { Result = true };
            }
            //get the fb report map id by report id
            var fbReportData = await _reportFastTransactionRepository.GetFbReportIdsByBookingIds(notStartedReports.Where(x => x.ReportId.HasValue).Select(x => x.ReportId.Value));
            foreach (var reportFast in notStartedReports)
            {
                var fbReport = fbReportData.FirstOrDefault(x => x.ReportId == reportFast.ReportId);
                await GenerateAndUpdateFastReportUrl(reportFast, fbReport, token, fbToken, requestEntityId);
            }
            //get the it notification false and try count will be 3
            var failedReports = notStartedReports.Where(x => x.Status == (int)ReportFastStatus.Error && x.ItNotification == false && x.TryCount == 3).ToList();

            if (failedReports.Any())
            {
                //if any data available then add the notification 
                await AddNotification(failedReports);
            }
            _reportFastTransactionRepository.EditEntities(notStartedReports);
            await _repo.Save();
            return new PushReportInfoToFastReportResponse() { Result = true };
        }


        /// <summary>
        /// add notification for falied report
        /// </summary>
        /// <param name="failedReports"></param>
        /// <returns></returns>
        private async Task AddNotification(List<RepFastTransaction> failedReports)
        {
            //get the notification message
            var notificationMessage = _repo.GetSingle<MidNotificationMessage>(x => x.Id == (int)NotificationMessages.FastReportGenerationFailed);
            if (notificationMessage == null)
                return;

            //get it team users 
            var itUserList = await _userRepository.GetUsersByRoleId((int)RoleEnum.IT_Team);

            //format the message
            var message = string.Format(notificationMessage.Name, string.Join(",", failedReports.Select(y => y.ReportId)));
            //add the mid_notification
            await _notificationManager.AddNotification(NotificationType.FastReportGenerationFailed, 0, itUserList.Select(x => x.Id), message, notificationMessage: (int)NotificationMessages.FastReportGenerationFailed);


            var notification = new Notification
            {
                Title = "Fast Report Generation Failed",
                Message = message,
                Url = "",
                TypeId = "Notification"
            };
            //brodcast the it users
            _broadCastService.Broadcast(itUserList.Select(x => x.Id), notification);

            //true the it notification
            failedReports.ForEach(x => x.ItNotification = true);

            _reportFastTransactionRepository.EditEntities(failedReports);
        }

        /// <summary>
        /// get inspection custom report data
        /// </summary>
        /// <param name="repFast"></param>
        /// <param name="token"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> GetInspectionCustomReportUrl(int reportId, string token, string entityId)
        {
            string inspectionCustomReportUrl = Convert.ToString(_configuration["InspectionCustomReportUrl"]);
            var baseUrl = Convert.ToString(_configuration["ServerUrl"]);
            var url = string.Concat(baseUrl, string.Format(inspectionCustomReportUrl, reportId));
            using (var httpClient = new HttpClient())
            {

                if (!string.IsNullOrEmpty(token) && !string.IsNullOrWhiteSpace(token))
                    httpClient.DefaultRequestHeaders.Add("Authorization", token);

                // set entity id from the background job                    
                if (!string.IsNullOrWhiteSpace(value: entityId))
                {
                    httpClient.DefaultRequestHeaders.Add("entityId", entityId);
                }

                return await httpClient.GetAsync(url);
            }

        }
        /// <summary>
        /// change fast report status with log
        /// </summary>
        /// <param name="repFastTransaction"></param>
        /// <param name="reportFastStatus"></param>
        /// <param name="error"></param>
        private void ChangeReportFastStatus(RepFastTransaction repFastTransaction, ReportFastStatus reportFastStatus, string error = null)
        {
            repFastTransaction.Status = (int)reportFastStatus;
            if (reportFastStatus == ReportFastStatus.Error)
            {
                //increase try count and when try count is 3 then send notification to it team and after 3 try not process
                repFastTransaction.TryCount = ++repFastTransaction.TryCount;
            }

            var repFastTranLog = new RepFastTranLog()
            {
                CreatedOn = DateTime.Now,
                ReportId = repFastTransaction.ReportId,
                Status = (int)reportFastStatus,
                ErrorLog = reportFastStatus == ReportFastStatus.Error ? error : null
            };
            repFastTransaction.RepFastTranLogs.Add(repFastTranLog);
            _reportFastTransactionRepository.AddEntity(repFastTranLog);

        }




        /// <summary>
        /// Convert all the inspection booking related files and upload to cloud
        /// </summary>
        /// <param name="bookingIds"></param>
        /// <returns></returns>
        public async Task UploadInspectionAttachementsAsZipToCloud(FileAttachmentToZipRequest request)
        {

            try
            {
                //get the file attachments by booking ids(custom object inspection file attachment details)
                var inspFileAttachments = await _insprepo.GetFileAttachmentsByBookingIdsAndZipStatus(request?.InspectionId);

                //if booking attachment list is there and count greater than 1
                if (inspFileAttachments.Any())
                {

                    var filteredInspectionIds = inspFileAttachments.Select(x => x.InspectionId).Distinct().ToList();

                    foreach (var filteredInspectionId in filteredInspectionIds)
                    {
                        var filteredInspectionFileAttachments = inspFileAttachments.Where(x => x.InspectionId == filteredInspectionId).ToList();

                        //update the zip status to progress
                        await UpdateZipStatusToProgress(filteredInspectionFileAttachments);

                        await ProcessInspectionFilesToZipFile(filteredInspectionFileAttachments, filteredInspectionId, request.EntityId);
                    }
                }
            }
            catch (Exception)
            {

            }

        }

        /// <summary>
        /// Convert all the inspection related files to zip and upload to cloud
        /// </summary>
        /// <param name="inspFileAttachments"></param>
        /// <param name="inspectionId"></param>
        /// <returns></returns>
        private async Task ProcessInspectionFilesToZipFile(List<InspTranFileAttachment> inspFileAttachments, int inspectionId, int entityId)
        {
            //generate the zip file conversion request
            var requestZipFile = GenerateRequestZIPFile(inspFileAttachments);
            requestZipFile.EntityId = entityId;

            //read the file server configuration
            string baseUrl = _configuration["FileServer"];
            string requestUrl = _configuration["FileServerUploadZip"];

            HttpResponseMessage httpResponse = null;

            try
            {
                //get the http response data
                httpResponse = await _apiHelper.SendRequestToPartnerAPI(Method.Post, requestUrl, requestZipFile, baseUrl);
            }
            catch (Exception)
            {
                await UpdateInspectionFileZipTryCount(inspFileAttachments);
            }

            //if response is ok
            if (httpResponse != null && httpResponse.StatusCode == HttpStatusCode.OK)
            {
                //read the http content string
                var responseData = await httpResponse.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(responseData))
                {
                    var zipFileUploadResponse = JsonConvert.DeserializeObject<ZipFileUploadResponse>(responseData);

                    await ProcessZipFileResponseFromFileServer(zipFileUploadResponse, inspectionId, inspFileAttachments);
                }
            }
            else
            {
                await UpdateInspectionFileZipTryCount(inspFileAttachments);
            }
        }


        /// <summary>
        /// Generate the request to convert the files to zip files
        /// </summary>
        /// <param name="fileAttachments"></param>
        /// <returns></returns>
        private RequestZIPFile GenerateRequestZIPFile(List<InspTranFileAttachment> fileAttachments)
        {
            RequestZIPFile requestZipFile = new RequestZIPFile();

            requestZipFile.Container = (int)FileContainerList.InspectionBooking;
            requestZipFile.ZIPFileDataList = new List<ZIPFileData>();

            //add the file upload data(uniqueId,fileName)
            foreach (var item in fileAttachments)
            {

                ZIPFileData zipFileData = new ZIPFileData();
                zipFileData.UniqueId = item.UniqueId;
                zipFileData.FileName = item.FileName;

                requestZipFile.ZIPFileDataList.Add(zipFileData);
            }

            return requestZipFile;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="responseData"></param>
        /// <param name="inspectionId"></param>
        /// <param name="inspFileAttachments"></param>
        /// <param name="fileAttachmentCategoryId"></param>
        /// <returns></returns>
        private async Task ProcessZipFileResponseFromFileServer(ZipFileUploadResponse zipFileUploadResponse, int inspectionId, List<InspTranFileAttachment> inspFileAttachments)
        {
            //if result & fileuploaddata is not null
            if (zipFileUploadResponse != null && zipFileUploadResponse.FileUploadData != null)
            {
                if (zipFileUploadResponse.FileUploadData.Result == ZipFileUploadResponseResult.Sucess)
                {

                    var inspFileAttachment = inspFileAttachments.FirstOrDefault();

                    //add the file attachment zip details
                    AddFileAttachmentZipDetails(zipFileUploadResponse.FileUploadData, inspectionId, inspFileAttachment?.FileAttachmentCategoryId);

                    //update the inspection file zip status to completed
                    UpdateInspectionFileZipStatusToCompleted(inspFileAttachments, inspectionId);

                    await _repo.Save();
                }
            }
        }

        /// <summary>
        /// Update Inspection file attachment zip status to completed
        /// </summary>
        /// <param name="inspFileAttachments"></param>
        /// <param name="inspectionId"></param>
        private void UpdateInspectionFileZipStatusToCompleted(List<InspTranFileAttachment> inspFileAttachments, int inspectionId)
        {
            var inspectionFileAttachments = inspFileAttachments.Where(x => x.InspectionId == inspectionId).ToList();

            inspectionFileAttachments.ForEach(x => x.ZipStatus = (int)ZipStatus.Completed);

            _repo.EditEntities(inspectionFileAttachments);
        }

        /// <summary>
        /// Update the file attachment zip status to progress
        /// </summary>
        /// <param name="inspectionIds"></param>
        /// <returns></returns>
        private async Task UpdateZipStatusToProgress(List<InspTranFileAttachment> inspFileAttachments)
        {
            //update the zip status to in-progress
            inspFileAttachments.ForEach(x => x.ZipStatus = (int)ZipStatus.InProgress);

            //add to edit entities
            _repo.EditEntities(inspFileAttachments);

            //save the changes
            await _repo.Save();
        }

        /// <summary>
        /// Update the inspection files zip try count
        /// </summary>
        /// <param name="fileAttachments"></param>
        /// <returns></returns>
        private async Task UpdateInspectionFileZipTryCount(List<InspTranFileAttachment> fileAttachments)
        {
            foreach (var attachment in fileAttachments)
            {
                attachment.ZipTryCount = attachment.ZipTryCount + 1;
                attachment.ZipStatus = (int)ZipStatus.Pending;
            }

            _repo.EditEntities(fileAttachments);

            await _repo.Save();
        }


        /// <summary>
        /// Add the file attachment zip details
        /// </summary>
        /// <param name="fileUploadData"></param>
        /// <param name="inspectionId"></param>
        /// <param name="fileAttachmentCategoryId"></param>
        private void AddFileAttachmentZipDetails(ZipFileUploadData fileUploadData, int inspectionId, int? fileAttachmentCategoryId)
        {
            var fileAttachmentZipEntity = new InspTranFileAttachmentZip()
            {
                InspectionId = inspectionId,
                FileAttachmentCategoryId = fileAttachmentCategoryId,
                FileName = fileUploadData.FileName,
                FileUrl = fileUploadData.FileCloudUri,
                UniqueId = fileUploadData.FileUniqueId,
                CreatedOn = DateTime.Now,
                Active = true,
            };

            _repo.AddEntity(fileAttachmentZipEntity);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task ProcessInspectionFastReport(FastReportRequest request, string fbToken, string token)
        {
            if (request.EntityId.HasValue)
            {
                var fbReportData = await _reportFastTransactionRepository.GetFbReportIdsByBookingIds(request.ReportIds);
                var entityId = EncryptionDecryption.EncryptStringAES(request.EntityId.ToString());
                //add the log and update the status
                foreach (var reportId in request.ReportIds)
                {
                    var reportFast = AddReportFastTransaction(new ReportFastTransactionRequest() { BookingId = request.BookingId, ReportId = reportId, Status = ReportFastStatus.NotStarted });
                    var fbReport = fbReportData.FirstOrDefault(x => x.ReportId == reportFast.ReportId);
                    //generate and update fast report url in fb
                    await GenerateAndUpdateFastReportUrl(reportFast, fbReport, token, fbToken, entityId, false);

                    _reportFastTransactionRepository.AddEntity(reportFast);
                }

                await _repo.Save();
            }

        }
        private RepFastTransaction AddReportFastTransaction(ReportFastTransactionRequest input)
        {

            var reportFastTransaction = new RepFastTransaction()
            {
                BookingId = input.BookingId,
                ItNotification = false,
                CreatedOn = DateTime.Now,
                ReportId = input.ReportId,
                Status = (int)ReportFastStatus.NotStarted,
                TryCount = 0
            };

            var repFastTranLog = new RepFastTranLog()
            {
                CreatedOn = DateTime.Now,
                ReportId = input.ReportId,
                Status = (int)ReportFastStatus.NotStarted,
            };
            reportFastTransaction.RepFastTranLogs.Add(repFastTranLog);
            _reportFastTransactionRepository.AddEntity(repFastTranLog);

            return reportFastTransaction;
        }

        /// <summary>
        /// for generate the fast report url and update in fb
        /// </summary>
        /// <param name="reportFast"></param>
        /// <param name="fbReport"></param>
        /// <param name="token"></param>
        /// <param name="fbToken"></param>
        /// <param name="requestEntityId"></param>
        /// <param name="isSchedulerRequest"></param>
        /// <returns></returns>
        private async Task<bool> GenerateAndUpdateFastReportUrl(RepFastTransaction reportFast, FbReportIdDto fbReport, string token, string fbToken, string requestEntityId, bool isSchedulerRequest = true)
        {
            try
            {
                //add the log and update the status
                ChangeReportFastStatus(reportFast, ReportFastStatus.InProgress);

                if (!isSchedulerRequest)
                    token = "Bearer " + token;
                //get the fast report url 
                var response = await GetInspectionCustomReportUrl(reportFast.ReportId.Value, token, requestEntityId);
                var reportData = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    //change the status and add the log
                    ChangeReportFastStatus(reportFast, ReportFastStatus.Error, "GetInspectionCustomerReportUrl failed");
                    return false;
                }
                else
                {
                    JObject reportDataJson = JObject.Parse(reportData);
                    var reportPath = reportDataJson.GetValue("reportPath").ToString();
                    //checking the fast report url avaiable or not
                    if (!string.IsNullOrEmpty(reportPath) && !string.IsNullOrWhiteSpace(reportPath))
                    {
                        //update the fast report url in rep fast transaction table
                        reportFast.ReportLink = reportPath;
                        ChangeReportFastStatus(reportFast, ReportFastStatus.Completed);


                        if (fbReport != null && fbReport.FbReportId.HasValue)
                        {
                            //update fast report url in fb saas
                            var result = await _fBReportManager.SaveReportDataToFB(fbReport.FbReportId.Value, reportPath, fbToken);
                            if (result.isSuccess)
                            {
                                //change the status and add the log
                                ChangeReportFastStatus(reportFast, ReportFastStatus.PushedToFB);
                                return true;
                            }
                            else
                            {
                                //change the status and add the log
                                ChangeReportFastStatus(reportFast, ReportFastStatus.Error, result.error ?? "Report url not saved");
                                return false;
                            }
                        }
                        else
                        {
                            //change the status and add the log
                            ChangeReportFastStatus(reportFast, ReportFastStatus.Error, fbReport != null && !fbReport.FbReportId.HasValue ? "Report id not found" : "Inspection data not found");
                            return false;
                        }

                    }
                    else
                    {
                        //change the status and add the log
                        var error = reportDataJson.GetValue("errorInfo").ToString();
                        ChangeReportFastStatus(reportFast, ReportFastStatus.Error, error);
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                //change the status and add the log
                ChangeReportFastStatus(reportFast, ReportFastStatus.Error, ex.Message);
                return false;
            }
        }

        public async Task<List<BookingCsItem>> GetBookingCSItemList(List<int> bookingIds)
        {
            var bookingCsList = await _scheduleRepository.GetBookingCSItemList(bookingIds);

            return bookingCsList;
        }


        public async Task<List<ScheduleJobCsEmail>> ScheduleBookingCS()
        {
            var yesterdayDate = DateTime.Now.AddDays(-1).Date;
            var inspections = await _insprepo.GetBookingsByServiceDates(yesterdayDate);
            var bookingIds = inspections.Select(x => x.InspectionId).ToList();
            var scheduleCSs = await _scheduleRepository.GetCSBookingDetails(bookingIds, yesterdayDate);
            var scheduleQCs = await _scheduleRepository.GetQCBookingDetails(bookingIds);
            var poColors = await _insprepo.GetPOColorTransactionsByBookingIds(bookingIds);
            var serviceTypes = await _insprepo.GetServiceType(bookingIds);

            // get product list based on the booking ids
            var productList = await _insprepo.GetProductListByBookingByPO(bookingIds);

            foreach (var product in productList)
            {
                if (product.FbReportId == 0)
                {
                    product.FbReportId = product.FbContainerReportId != 0 ? product.FbContainerReportId : 0;
                }
            }

            // apply order by logic for combine process.
            productList = productList.OrderBy(x => x.CombineProductId).ThenByDescending(x => x.CombineAqlQuantity).ThenBy(x => x.ProductName).ToList();

            // get report Id from the product list
            var reportIds = productList.Where(x => x.FbReportId > 0).Select(x => x.FbReportId).Distinct().ToList();

            // get report data based on report ids
            var fbReportInfoList = await _fbRepo.GetFbReportStatusListCustomerReportbyBooking(reportIds);


            return scheduleCSs.Select(x => new { x.Id, x.Email, x.CompanyEmail, x.Name }).GroupBy(p => new { p.Id, p.Email, p.CompanyEmail, p.Name }, p => p, (key, _data) =>
                          new ScheduleStaffItem()
                          {
                              Id = key.Id,
                              Email = key.Email,
                              CompanyEmail = key.CompanyEmail,
                              Name = key.Name
                          }).Select(x => MapScheduleJobCsEmailDetails(x, inspections, scheduleCSs, scheduleQCs, poColors, serviceTypes, productList, fbReportInfoList)).ToList();


        }


        private ScheduleJobCsEmail MapScheduleJobCsEmailDetails(ScheduleStaffItem staffItem, List<ScheduleBookingDetailsRepoItem> bookingDetails, List<ScheduleStaffItem> csDetails,
            List<ScheduleStaffItem> qcDetails, List<InspectionPOColorTransaction> inspectionPOColorTransactions, IEnumerable<ServiceTypeList> serviceTypes,
            List<InternalReportProducts> productList, IEnumerable<FBReportDetails> fbReportInfoList)
        {

            var csBookingIds = csDetails.Where(x => x.Id == staffItem.Id).Select(y => y.BookingId).ToList();
            var inspections = bookingDetails.Where(x => csBookingIds.Contains(x.InspectionId)).ToList();
            var products = productList.Where(x => csBookingIds.Contains(x.BookingId) && x.FbReportId > 0).ToList();

            List<ScheduleJobCsInspectionDetails> scheduleJobCsInspectionDetails = new List<ScheduleJobCsInspectionDetails>();
            foreach (var item in products)
            {
                var inspection = inspections.FirstOrDefault(x => x.InspectionId == item.BookingId);
                var poColors = inspectionPOColorTransactions.Where(x => x.BookingId == item.BookingId && x.ProductId == item.ProductId).ToList();
                var report = fbReportInfoList.FirstOrDefault(x => x.ReportId == item.FbReportId);

                var inspectionDetails = new ScheduleJobCsInspectionDetails()
                {
                    BusinessLine = inspection.BusinessLine,
                    CustomerName = inspection.CustomerName,
                    FactoryName = inspection.FactoryName,
                    SupplierName = inspection.SupplierName,
                    InspectionId = inspection.InspectionId,
                    QcNames = string.Join(", ", qcDetails.Where(x => x.BookingId == inspection.InspectionId).Select(y => y.Name).Distinct().ToList()),
                    ScheduleDate = csDetails.FirstOrDefault(x => x.BookingId == inspection.InspectionId)?.ServiceDate.ToString(StandardDateFormat),
                    ProductName = item.ProductSubCategory2Name,
                    ColorCode = string.Join(", ", poColors.Select(y => y.ColorCode).Distinct().ToList()),
                    ColorName = string.Join(", ", poColors.Select(y => y.ColorName).Distinct().ToList()),
                    ProductRef = item.ProductName,
                    PoNumber = item.PONumber,
                    ReportNo = report?.ReportTitle,
                    FillingStatus = report?.FillingStatus,
                    ReviewStatus = report?.ReviewStatus,
                    ServiceType = string.Join(", ", serviceTypes.Where(x => x.InspectionId == inspection.InspectionId).Select(y => y.serviceTypeName).Distinct().ToList())
                };
                scheduleJobCsInspectionDetails.Add(inspectionDetails);
            }

            return new ScheduleJobCsEmail()
            {
                CSEmail = staffItem.CompanyEmail,
                CSName = staffItem.Name,
                ScheduleDate = csDetails.FirstOrDefault(x => x.Id == staffItem.Id)?.ServiceDate.ToString(StandardDateFormat),
                Inspections = scheduleJobCsInspectionDetails.OrderBy(x => x.InspectionId).ThenBy(x => x.ReportNo).ToList(),
                IsAnySoftLineItems = inspections.Any(x => x.BusinessLine == (int)BusinessLine.SoftLine)
            };
        }

        public async Task<List<InspectionDetail>> ScheduleSkipMSchart()
        {
            var today = DateTime.Now;
            var nextfiveday = DateTime.Now.AddDays(5);

            var inspList = _inspectionBookingManager.GetAllInspections();

            var inspectionBookingItems = await inspList.Where(x => x.StatusId != (int)BookingStatus.Cancel && ((x.ServiceDateFrom >= today && x.ServiceDateFrom <= nextfiveday)
                            || (today <= x.ServiceDateTo && x.ServiceDateTo <= nextfiveday))).AsNoTracking().ToListAsync();

            var bookingIds = inspectionBookingItems.Select(x => x.BookingId).ToList();

            var productList = await _repo.GetProductTransactionList(bookingIds);

            var serviceTypes = await _inspectionBookingManager.GetBookingServiceTypes(bookingIds);

            var qcList = await _scheduleRepository.GetQCBookingDetails(bookingIds);

            var bookingCsList = await _scheduleRepository.GetBookingCSItemList(bookingIds);

            string baseUrl = _configuration["BaseUrl"];
            var masterConfigs = await _inspectionBookingManager.GetMasterConfiguration();
            var entityName = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();

            return inspectionBookingItems.Select(x => new InspectionDetail
            {
                InspectionId = x.BookingId,
                CustomerId = x.CustomerId.Value,
                CustomerName = x.CustomerName,
                CustomerEmail = x.CustomerEmail,
                SupplierId = x.SupplierId,
                SupplierName = x.SupplierName,
                FactoryId = x.FactoryId,
                FactoryName = x.FactoryName,
                OfficeId = x.OfficeId,
                Office = x.Office,
                ServiceTypeId = serviceTypes.FirstOrDefault(y => y.BookingNo == x.BookingId)?.ServiceTypeId,
                ServiceType = serviceTypes.FirstOrDefault(y => y.BookingNo == x.BookingId)?.ServiceTypeName,
                QcName = string.Join(", ", qcList.Where(y => y.BookingId == x.BookingId).Select(y => y.Name).Distinct().ToArray()),
                ServiceDateFrom = x.ServiceDateFrom.ToString(StandardDateFormat),
                ServiceDateTo = x.ServiceDateTo.ToString(StandardDateFormat),
                InspectionURL = baseUrl + string.Format(_configuration["UrlBookingRequest"], x.BookingId, entityName),
                EntityName = entityName,
                CsList = bookingCsList.Where(y => y.BookingId == x.BookingId).ToList(),
                ProductList = productList.Where(y => y.InspectionId == x.BookingId).Select(x => new ProductDetail
                {
                    InspectionId = x.InspectionId,
                    ProductId = x.ProductId,
                    ProductRef = x.ProductRef,
                    ProductName = x.ProductName,
                    ProductURL = baseUrl + string.Format(_configuration["UrlProductRequest"], x.ProductId, entityName)
                }).ToList()
            }).ToList();
        }


        public ScheduleSkipMSchartEmailResponse ScheduleSkipMSchartForCustomer(List<InspectionDetail> inspectionList)
        {
            var response = new ScheduleSkipMSchartEmailResponse()
            {
                FromDate = DateTime.Now.ToString(StandardDateFormat),
                ToDate = DateTime.Now.AddDays(5).ToString(StandardDateFormat),
                CustomerName = inspectionList.FirstOrDefault()?.CustomerName,
                CustomerEmail = inspectionList.FirstOrDefault()?.CustomerEmail,
                InspectionList = inspectionList.Where(x => x.ProductList != null && x.ProductList.Any()).ToList(),
            };
            return response;
        }

        public async Task<JobConfiguration> GetJobConfiguration(int configureId)
        {
            try
            {
                var dataList = await _repo.GetScheduleJobConfiguration(configureId);
                return dataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<SchedulePlanningForCSResponse> SchedulePlanningForCS(string offices, string configureId)
        {
            if (string.IsNullOrEmpty(configureId))
                return new SchedulePlanningForCSResponse
                {
                    errors = new List<string>() { "ConfigurationId is required" },
                    message = "Bad Request",
                    statusCode = HttpStatusCode.BadRequest
                };

            int configId = Convert.ToInt32(configureId);

            if (configId <= 0)
                return new SchedulePlanningForCSResponse
                {
                    errors = new List<string>() { "Please enter a valid configurationId" },
                    message = "Bad Request",
                    statusCode = HttpStatusCode.BadRequest
                };

            // get office ids from the request
            var officeIdList = string.IsNullOrEmpty(offices) ? new List<int>() : offices.Split(',').Select(int.Parse).ToList();
            var jobConfigurationList = await GetJobConfigurations(new List<int>() { (int)ScheduleOptions.SchedulePlanningForCS });
            var jobConfiguration = jobConfigurationList.FirstOrDefault(x => x.Id == configId);
            
            if (jobConfiguration == null)
                return new SchedulePlanningForCSResponse
                {
                    errors = new List<string>() { "Configuration is not exist in our system" },
                    message = "Bad Request",
                    statusCode = HttpStatusCode.BadRequest
                };

            int intervalDay = NumberThree;
            if (jobConfiguration.ScheduleInterval != null && jobConfiguration.ScheduleInterval > 0)
                intervalDay = jobConfiguration.ScheduleInterval.GetValueOrDefault();

            var masterConfigs = await _inspectionBookingManager.GetMasterConfiguration();
            var entityName = masterConfigs.Where(x => x.Type == (int)EntityConfigMaster.Entity).Select(x => x.Value).FirstOrDefault();

            var schedulePlanningForCS = new SchedulePlanningForCS
            {
                JobConfiguration = jobConfiguration,
                FromDate = DateTime.Now.Date,
                ToDate = DateTime.Now.Date.AddDays(intervalDay - NumberOne),
                OfficeIdList = officeIdList,
                EntityName = entityName,
                SendDate = DateTime.Now,
                IntervalDay = intervalDay
            };

            return new SchedulePlanningForCSResponse
            {
                data = schedulePlanningForCS,
                errors = new List<string>() { "Success" },
                message = "Success",
                statusCode = HttpStatusCode.OK
            };
        }

        public async Task<List<SchedulePlanningFileDateForCS>> SchedulePlanningForCSFileData(SchedulePlanningForCS response)
        {
            var inspectionQuery = _inspectionBookingManager.GetAllInspections();
            inspectionQuery = inspectionQuery.Where(x => x.StatusId == (int)BookingStatus.AllocateQC && !((x.ServiceDateFrom > response.ToDate) || (x.ServiceDateTo < response.FromDate)));

            //apply office list filter
            if (response.OfficeIdList.Any())
            {
                inspectionQuery = inspectionQuery.Where(x => x.OfficeId != null && response.OfficeIdList.Contains(x.OfficeId.GetValueOrDefault()));
            }

            var inspectionList = await inspectionQuery.ToListAsync();
            
            var bookingIds = inspectionList.Select(x => x.BookingId);

            var poColorTransactionList = await _insprepo.GetPOColorTransactionsByBookingIds(bookingIds);
            var productList = await _insprepo.GetProductListByBooking(bookingIds);
            var serviceTypeList = await _insprepo.GetServiceType(bookingIds);
            var factoryLocationList = await _insprepo.GetFactorycountryId(bookingIds);
            var brandList = await _insprepo.GetBrandBookingIdsByBookingIds(bookingIds);

            var qcList = await _scheduleRepository.GetQCBookingDetails(bookingIds.ToList());
            var csList = await _scheduleRepository.GetCSBookingDetails(bookingIds.ToList());

            return MapSchedulePlanningForCSEmailDetails(poColorTransactionList, inspectionList, productList, serviceTypeList, factoryLocationList, brandList, qcList, csList);
        }

        private static List<SchedulePlanningFileDateForCS> MapSchedulePlanningForCSEmailDetails(List<InspectionPOColorTransaction> poColorTransactionList, List<InspectionBookingItems> inspectionList, IEnumerable<BookingProductsData> productList, IEnumerable<ServiceTypeList> serviceTypeList, List<FactoryCountry> factoryLocationList, List<BookingBrandAccess> brandList, List<ScheduleStaffItem> qcList, List<ScheduleStaffItem> csList)
        {
            var schedulePlanningForCSList = new List<SchedulePlanningFileDateForCS>();
            foreach (var poColorTransaction in poColorTransactionList)
            {
                var inspection = inspectionList.FirstOrDefault(x => x.BookingId == poColorTransaction.BookingId);
                var product = productList.FirstOrDefault(x => x.Id == poColorTransaction.ProductRefId);

                if (inspection != null)
                {
                    var serviceType = serviceTypeList.FirstOrDefault(x => x.InspectionId == inspection.BookingId);
                    var factoryLocation = factoryLocationList.FirstOrDefault(x => x.BookingId == inspection.BookingId);
                    var brandItems = brandList.Where(x => x.BookingId == inspection.BookingId).ToList();
                    var qcItems = qcList.Where(x => x.BookingId == inspection.BookingId).ToList();
                    var csItems = csList.Where(x => x.BookingId == inspection.BookingId).ToList();

                    var schedulePlanningForCS = new SchedulePlanningFileDateForCS()
                    {
                        Week = WeekNumber(inspection.ServiceDateFrom),
                        DayNo = inspection.ServiceDateFrom.DayOfWeek.ToString(),
                        ApplyDate = inspection.ApplyDate.ToString(StandardDateFormat),
                        ServiceDate = inspection.ServiceDateFrom == inspection.ServiceDateTo ? inspection.ServiceDateFrom.ToString(StandardDateFormat) : string.Join(" - ", inspection.ServiceDateFrom.ToString(StandardDateFormat), inspection.ServiceDateTo.ToString(StandardDateFormat)),
                        BookingId = inspection.BookingId,
                        ColorName = poColorTransaction.ColorName,
                        ServiceType = serviceType?.serviceTypeName,
                        QCNames = string.Join(", ", qcItems.Select(x => x.Name).Distinct()),
                        CSNames = string.Join(", ", csItems.Select(x => x.Name).Distinct()),
                        Customer = inspection.CustomerName,
                        Brand = string.Join(", ", brandItems.Select(x => x.BrandName).Distinct()),
                        Supplier = inspection.SupplierName,
                        Factory = inspection.FactoryName,
                        FactoryAddress = factoryLocation?.FactoryAdress,
                        FactoryChinaAddress = (factoryLocation?.FactoryCountryId == (int)ZohoCountryEnum.China) ? factoryLocation?.FactoryRegionalAddress : "",
                        Status = inspection.StatusName,
                        BookingQuantity = poColorTransaction.BookingQuantity,
                        Office = inspection.Office
                    };
                    if (product != null)
                    {
                        schedulePlanningForCS.PoNo = product.ProductName;
                        schedulePlanningForCS.IsMSChart = product.IsMSChart ? ScheduleMSChartYes : ScheduleMSChartNo;
                        schedulePlanningForCS.ProductSubCategory2 = product.ProductSubCategory2;
                        schedulePlanningForCS.UnitNameCount = product.UnitCount.GetValueOrDefault() > 0 ? product.UnitName + " (" + product.UnitCount + ")" : product.UnitName;
                        schedulePlanningForCS.ProductSubCategory = product.ProductSubCategory;
                    }
                    schedulePlanningForCSList.Add(schedulePlanningForCS);
                }
            }
            return schedulePlanningForCSList;
        }

        private static int WeekNumber(DateTime dateTime)
        {
            return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public async Task<InitialBookingExtractResponse> BBGInitialBookingExtractEmail()
        {
            var jobConfigurationList = await getJobConfigurationList();
            var jobConfiguration = jobConfigurationList.FirstOrDefault(x => x.Type == (int)ScheduleOptions.BBGInitialBookingExtract);
            if (jobConfiguration == null)
                return new InitialBookingExtractResponse
                {
                    errors = new List<string>() { "Configuration is not exist in our system" },
                    message = "Bad Request",
                    statusCode = HttpStatusCode.BadRequest
                };

            int intervalDay = NumberSeven;
            if (jobConfiguration.ScheduleInterval != null && jobConfiguration.ScheduleInterval > 0)
                intervalDay = jobConfiguration.ScheduleInterval.GetValueOrDefault();

            var customerIdList = string.IsNullOrEmpty(jobConfiguration.CustomerIds) ? new List<int>() : jobConfiguration.CustomerIds.Split(',').Select(int.Parse).ToList();

            var initialBookingExtract = new InitialBookingExtract
            {
                JobConfiguration = jobConfiguration,
                FromDate = DateTime.Now.Date.AddDays(NumberOne),
                ToDate = DateTime.Now.Date.AddDays(intervalDay),
                SendDate = DateTime.Now,
                IntervalDay = intervalDay,
                CustomerIds = customerIdList
            };

            return new InitialBookingExtractResponse
            {
                data = initialBookingExtract,
                message = "Success",
                statusCode = HttpStatusCode.OK
            };
        }
    }

    public class ScheduleJobCsvResponse
    {
        [CsvHelper.Configuration.Attributes.Name("Inspection number")]
        public string ReportNumber { get; set; }

        [CsvHelper.Configuration.Attributes.Name("Inspection date")]
        public string InspectionDate { get; set; }

        [CsvHelper.Configuration.Attributes.Name("Product refer")]
        public string ProductRefernce { get; set; }

        [CsvHelper.Configuration.Attributes.Name("EAN")]
        public string Ean { get; set; }

        [CsvHelper.Configuration.Attributes.Name("L")]
        public string MeasuredLength { get; set; }

        [CsvHelper.Configuration.Attributes.Name("W")]
        public string MeasuredWeight { get; set; }

        [CsvHelper.Configuration.Attributes.Name("H")]
        public string MeasuredHeight { get; set; }

        [CsvHelper.Configuration.Attributes.Name("GW")]
        public string MeasuredPackingWeight { get; set; }

    }
}
