using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.FinanceDashboard;
using DTO.Manday;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DTO.Common.Static_Data_Common;

namespace BI
{
    public class FinanceDashboardManager : ApiCommonData, IFinanceDashboardManager
    {
        private readonly IFinanceDashboardRepository _repo = null;
        private readonly IInspectionBookingRepository _inspRepo = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly IManagementDashboardRepository _mgmtRepo = null;
        private readonly IExchangeRateManager _exchangeRateManager = null;
        private readonly ICustomerManager _cusManager = null;
        private readonly ILocationRepository _locationRepo = null;
        private readonly ISupplierManager _supManager = null;
        private readonly ICustomerBrandManager _cusBrandManager = null;
        private readonly ICustomerBuyerManager _cusBuyerManager = null;
        private readonly ICustomerDepartmentManager _cusDeptManager = null;
        private readonly IOfficeLocationRepository _officeRepo = null;
        private readonly IReferenceManager _referenceManager = null;
        private readonly ISharedInspectionManager _sharedInspection = null;
        private readonly ITenantProvider _tenant = null;
        private readonly IHumanResourceRepository _hrRepo = null;
        private readonly IScheduleRepository _schRepo = null;

        public FinanceDashboardManager(IFinanceDashboardRepository repo, IInspectionBookingRepository inspRepo, IManagementDashboardRepository mgmtRepo,
            IExchangeRateManager exchangeRateManager,
            ICustomerManager cusManager,
            ILocationRepository locationRepo,
            ISupplierManager supManager,
            ICustomerBrandManager cusBrandManager,
            ICustomerBuyerManager cusBuyerManager,
            ICustomerDepartmentManager cusDeptManager,
            IOfficeLocationRepository officeRepo,
            IReferenceManager referenceManager, ISharedInspectionManager sharedInspection, ITenantProvider tenant, IHumanResourceRepository hrRepo,
            IScheduleRepository schRepo)
        {
            _repo = repo;
            _inspRepo = inspRepo;
            _mgmtRepo = mgmtRepo;
            _exchangeRateManager = exchangeRateManager;
            _cusManager = cusManager;
            _locationRepo = locationRepo;
            _supManager = supManager;
            _cusBrandManager = cusBrandManager;
            _cusBuyerManager = cusBuyerManager;
            _cusDeptManager = cusDeptManager;
            _officeRepo = officeRepo;
            _referenceManager = referenceManager;
            _sharedInspection = sharedInspection;
            _tenant = tenant;
            _hrRepo = hrRepo;
            _schRepo = schRepo;
        }

        public async Task<FinanceDashboardBookingDataResponse> GetBookingIdList(FinanceDashboardRequest request)
        {
            var res = await _repo.GetFinanceDashboardBookingDetail(request);

            if (res == null || !res.Any())
            {
                return new FinanceDashboardBookingDataResponse { Result = FinanceDashboardResult.NotFound };
            }

            return new FinanceDashboardBookingDataResponse
            {
                Data = res.Select(x => x.InspectionId).Distinct().ToList(),
                Result = FinanceDashboardResult.Success
            };
        }

        public async Task<MandayYearChartFinanceDashboardResponse> GetBilledMandayData(FinanceDashboardRequest request)
        {
            var mandayYear = new List<MandayYear>();
            FinanceDashboardMandayData response = null;

            //get the data from 1st Jan request.servicedateto.year till servicedateto
            MandaySPRequest spRequest = new MandaySPRequest();
            spRequest.ServiceDateTo = request.ServiceDateTo.ToDateTime();
            spRequest.ServiceDateFrom = new DateTime(request.ServiceDateTo.Year, 1, 1);
            spRequest.CustomerId = request.CustomerId != null ? request.CustomerId : 0;
            spRequest.SupplierId = request.SupplierId != null ? request.SupplierId : 0;
            spRequest.FactoryIdList = request.FactoryIdList != null && request.FactoryIdList.Any() ? request.FactoryIdList.ConvertAll(x => new CommonId { Id = x }) : null;
            spRequest.FactoryCountryIdList = request.CountryIdList != null && request.CountryIdList.Any() ? request.CountryIdList.ConvertAll(x => new CommonId { Id = x }) : null;
            spRequest.OfficeIdList = request.OfficeIdList != null && request.OfficeIdList.Any() ? request.OfficeIdList.ConvertAll(x => new CommonId { Id = x }) : null;
            spRequest.ServiceTypeIdList = request.ServiceTypeList != null && request.ServiceTypeList.Any() ? request.ServiceTypeList.ConvertAll(x => new CommonId { Id = x.Value }) : null;
            spRequest.BrandIdList = request.BrandIdList != null && request.BrandIdList.Any() ? request.BrandIdList.ToList().ConvertAll(x => new CommonId { Id = x.Value }) : null;
            spRequest.DepartmentIdList = request.DeptIdList != null && request.DeptIdList.Any() ? request.DeptIdList.ToList().ConvertAll(x => new CommonId { Id = x.Value }) : null;
            spRequest.BuyerIdList = request.BuyerIdList != null && request.BuyerIdList.Any() ? request.BuyerIdList.ToList().ConvertAll(x => new CommonId { Id = x.Value }) : null;
            spRequest.EntityId = _tenant.GetCompanyId();


            response = await _repo.GetBilledMandayData(spRequest);

            if (response == null || !response.BilledManday.Any())
            {
                return new MandayYearChartFinanceDashboardResponse { Result = FinanceDashboardResult.NotFound };
            }

            //get the per year man day count along with month wise data
            var billedManday = response.BilledManday.GroupBy(p => p.Year, (key, _data) =>
            new MandayYearChart
            {
                Year = key,
                MandayCount = _data.Where(x => x.Year == key).Sum(x => x.MonthManDay),
                MonthlyData = _data.Where(x => x.Year == key).ToList()
            }).OrderByDescending(x => x.Year).Take(2);

            var billedMandayBudget = new MandayYearChart
            {
                Year = DateTime.Now.Year,
                MandayCount = response.BilledMandayBudget.Sum(x => x.MonthManDay),
                MonthlyData = response.BilledMandayBudget
            };

            //get the x axis month data for the chart
            for (int i = 0; i < 12; i++)
            {
                MandayYear res = new MandayYear();
                res.year = DateTime.Now.Year;
                res.month = i;
                res.MonthName = MonthData.GetValueOrDefault(i + 1);
                mandayYear.Add(res);
            }

            return new MandayYearChartFinanceDashboardResponse
            {
                BilledMandayData = billedManday,
                BilledMandayBudget = billedMandayBudget,
                MonthYearXAxis = mandayYear,
                Result = FinanceDashboardResult.Success
            };

        }

        private async Task<FinanceDashboardMandayData> CommonMandayRateData(FinanceDashboardRequest request)
        {
            FinanceDashboardMandayData response = null;

            //get the data from 1st Jan request.servicedateto.year till servicedateto
            MandaySPRequest spRequest = new MandaySPRequest();
            spRequest.ServiceDateTo = request.ServiceDateTo.ToDateTime();
            spRequest.ServiceDateFrom = new DateTime(request.ServiceDateTo.Year, 1, 1);
            spRequest.CustomerId = request.CustomerId != null ? request.CustomerId : 0;
            spRequest.SupplierId = request.SupplierId != null ? request.SupplierId : 0;
            spRequest.FactoryIdList = request.FactoryIdList != null && request.FactoryIdList.Any() ? request.FactoryIdList.ConvertAll(x => new CommonId { Id = x }) : null;
            spRequest.FactoryCountryIdList = request.CountryIdList != null && request.CountryIdList.Any() ? request.CountryIdList.ConvertAll(x => new CommonId { Id = x }) : null;
            spRequest.OfficeIdList = request.OfficeIdList != null && request.OfficeIdList.Any() ? request.OfficeIdList.ConvertAll(x => new CommonId { Id = x }) : null;
            spRequest.ServiceTypeIdList = request.ServiceTypeList != null && request.ServiceTypeList.Any() ? request.ServiceTypeList.ConvertAll(x => new CommonId { Id = x.Value }) : null;
            spRequest.BrandIdList = request.BrandIdList != null && request.BrandIdList.Any() ? request.BrandIdList.ToList().ConvertAll(x => new CommonId { Id = x.Value }) : null;
            spRequest.DepartmentIdList = request.DeptIdList != null && request.DeptIdList.Any() ? request.DeptIdList.ToList().ConvertAll(x => new CommonId { Id = x.Value }) : null;
            spRequest.BuyerIdList = request.BuyerIdList != null && request.BuyerIdList.Any() ? request.BuyerIdList.ToList().ConvertAll(x => new CommonId { Id = x.Value }) : null;
            spRequest.EntityId = _tenant.GetCompanyId();

            response = await _repo.GetMandayRateData(spRequest);

            var result = response.MandayRate.ConvertAll(x => new ExchangeCurrencyItem
            {
                CurrencyId = x.CurrencyId,
                Fee = x.InspFees,
                Id = x.Id,
                Year = x.Year
            }).ToList();

            result = await CurrencyConversionToUsd(result, (int)CurrencyMaster.USD);

            foreach (var item in response.MandayRate)
            {
                item.InspFees = result.Where(x => x.Id == item.Id && x.Year == item.Year).Select(x => x.Fee).FirstOrDefault();
                //item.MonthManDay = item.MonthManDay > 0 ? item.InspFees / item.MonthManDay:0;
            }

            var budgetResult = response.MandayRateBudget.ConvertAll(x => new ExchangeCurrencyItem
            {
                CurrencyId = x.CurrencyId,
                Fee = x.InspFees,
                Id = x.Id
            }).ToList();

            budgetResult = await CurrencyConversionToUsd(budgetResult, (int)CurrencyMaster.USD);

            foreach (var item in response.MandayRateBudget)
            {
                item.InspFees = budgetResult.Where(x => x.Id == item.Id).Select(x => x.Fee).FirstOrDefault();
                //item.MonthManDay = item.MonthManDay > 0 ? item.InspFees / item.MonthManDay :0;
            }

            response.MandayRate = response.MandayRate.GroupBy(p => new { p.Year, p.Month, p.MonthName }, (key, _data) => new MandayRateItem
            {
                Month = key.Month,
                Year = key.Year,
                MonthName = key.MonthName,
                MonthManDay = Math.Round(_data.Sum(x => x.InspFees) / _data.Sum(x => x.MonthManDay))
            }).ToList();

            response.MandayRateBudget = response.MandayRateBudget.GroupBy(p => new { p.Year, p.Month, p.MonthName }, (key, _data) => new MandayRateItem
            {
                Month = key.Month,
                Year = key.Year,
                MonthName = key.MonthName,
                MonthManDay = Math.Round(_data.Sum(x => x.InspFees) / _data.Sum(x => x.MonthManDay))
            }).ToList();

            return response;
        }

        public async Task<MandayYearChartFinanceDashboardResponse> GetMandayRateData(FinanceDashboardRequest request)
        {
            var mandayYear = new List<MandayYear>();
            var response = await CommonMandayRateData(request);

            if (response == null || !response.MandayRate.Any())
            {
                return new MandayYearChartFinanceDashboardResponse { Result = FinanceDashboardResult.NotFound };
            }

            //get the per year man day count along with month wise data
            var mandayRate = response.MandayRate.GroupBy(p => p.Year, (key, _data) =>
            new MandayYearRateChart
            {
                Year = key,
                MandayCount = Math.Round(_data.Where(x => x.Year == key).Sum(x => x.MonthManDay) / _data.Count()),
                MonthlyData = _data.Where(x => x.Year == key).ToList()
            }).OrderByDescending(x => x.Year).Take(2);

            var mandayRateBudget = new MandayYearRateChart
            {
                Year = DateTime.Now.Year,
                MandayCount = Math.Round(response.MandayRateBudget.Sum(x => x.MonthManDay)/ response.MandayRateBudget.Count(x => x.MonthManDay > 0)) ,
                MonthlyData = response.MandayRateBudget
            };

            //get the x axis month data for the chart
            for (int i = 0; i < 12; i++)
            {
                MandayYear res = new MandayYear();
                res.year = DateTime.Now.Year;
                res.month = i;
                res.MonthName = MonthData.GetValueOrDefault(i + 1);
                mandayYear.Add(res);
            }

            return new MandayYearChartFinanceDashboardResponse
            {
                MonthYearXAxis = mandayYear,
                MandayRateData = mandayRate,
                MandayRateBudget = mandayRateBudget,
                Result = FinanceDashboardResult.Success
            };

        }

        public async Task<FinanceDashboardTurnOverResponse> GetFinanceDashboardTurnOverData(List<int> bookingIdList)
        {
            var request = new TurnOverSpRequest
            {
                BookingIdList = bookingIdList != null && bookingIdList.Any() ? bookingIdList.ConvertAll(x => new CommonId { Id = x }) : null,
                EntityId = _tenant.GetCompanyId()
            };
            var res = await _repo.GetFinanceTurnOverData(request);

            if (res == null || !res.Any())
            {
                return new FinanceDashboardTurnOverResponse { Result = FinanceDashboardResult.NotFound };
            }

            var exchangeCurrencyData = res.ConvertAll(x => new ExchangeCurrencyItem
            {
                CurrencyId = x.InvoiceCurrencyId,
                Fee = x.TotalInvoiceFee,
                Id = x.BookingId,
                ExtraFee = x.TotalExtraFee,
                ExtraFeeCurrencyId = x.ExtraFeeCurrencyId
            }).ToList();

            //get the fees by applying currency exchange rate
            exchangeCurrencyData = await CurrencyConversionToUsd(exchangeCurrencyData, (int)CurrencyMaster.USD);

            foreach (var item in res)
            {
                var bookingData = exchangeCurrencyData.FirstOrDefault(x => x.Id == item.BookingId);
                if (bookingData != null)
                {
                    item.TotalInvoiceFee = bookingData.Fee;
                    item.TotalExtraFee = bookingData.ExtraFee;
                }
            }

            var countryChartData = res.GroupBy(p => p.CountryId, (key, _data) => new FinanceDashboardCommonItem
            {
                Name = _data.Select(x => x.CountryName).FirstOrDefault(),
                Count = Math.Round(_data.Sum(x => x.TotalInvoiceFee) + _data.Sum(x => x.TotalExtraFee), 2)
            }).Where(x => x.Count > 0).OrderByDescending(x => x.Count).ToList();

            var prodCategoryChartData = res.GroupBy(p => p.ProdCategoryId, (key, _data) => new FinanceDashboardCommonItem
            {
                Name = _data.Select(x => x.ProdCategoryName).FirstOrDefault(),
                Count = Math.Round(_data.Sum(x => x.TotalInvoiceFee) + _data.Sum(x => x.TotalExtraFee), 2)
            }).Where(x => x.Count > 0).OrderByDescending(x => x.Count).ToList();

            var serviceTypeChartData = res.GroupBy(p => p.ServiceTypeId, (key, _data) => new FinanceDashboardCommonItem
            {
                Name = _data.Select(x => x.ServiceTypeName).FirstOrDefault(),
                Count = Math.Round(_data.Sum(x => x.TotalInvoiceFee) + _data.Sum(x => x.TotalExtraFee), 2)
            }).Where(x => x.Count > 0).OrderByDescending(x => x.Count).ToList();

            return new FinanceDashboardTurnOverResponse
            {
                CountryData = countryChartData,
                ProductCategoryData = prodCategoryChartData,
                ServieTypeData = serviceTypeChartData,
                Result = FinanceDashboardResult.Success
            };
        }

        //get the fees by applying currency exchange rate
        public async Task<List<ExchangeCurrencyItem>> CurrencyConversionToUsd(List<ExchangeCurrencyItem> data, int targetCurrency)
        {
            //Dictionary<int, double> dicexrate = new Dictionary<int, double>();
            var Exchangecurrency = new List<int>();

            var exchangeList = new List<ExchangeCurrency>();

            if (data != null && data.Any())
            {
                //Exchangecurrency.AddRange(data.Select(x => x.CurrencyId).Distinct());

                //Exchangecurrency.AddRange(data.Select(x => x.ExtraFeeCurrencyId).Distinct());

                var currencyIdList = data.Where(x => x.CurrencyId > 0).Select(x => x.CurrencyId).Distinct().ToList();
                foreach (var item in currencyIdList)
                {
                    if (!exchangeList.Select(x => x.Currency).Contains(item))
                    {
                        exchangeList.Add(new ExchangeCurrency()
                        {
                            TargetCurrency = targetCurrency,
                            Currency = item
                        });
                    }
                }

                var extraFeeCurrencyIdList = data.Where(x => x.ExtraFeeCurrencyId > 0).Select(x => x.ExtraFeeCurrencyId).Distinct().ToList();
                foreach (var item in extraFeeCurrencyIdList)
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

                var dicexrate = await _exchangeRateManager.GetExchangeRateList(exchangeList, DateTime.Now, ExhangeRateTypeEnum.ExpenseClaim);

                foreach (var item in data.ToList())
                {
                    if (dicexrate.TryGetValue(item.CurrencyId, out double exrate))
                    {
                        item.Fee = item.Fee * exrate;
                    }

                    if (dicexrate.TryGetValue(item.ExtraFeeCurrencyId, out double exfExrate))
                    {
                        item.ExtraFee = item.ExtraFee * exfExrate;
                    }
                }
            }

            return data;
        }

        public async Task<FinanceDashboardRequestExport> SetExportFilter(FinanceDashboardRequest request)
        {
            var response = new FinanceDashboardRequestExport();

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

            //selected brand name list
            if (request.BrandIdList != null && request.BrandIdList.Any())
            {
                var brandNameList = await _cusBrandManager.GetBrandNameByBrandId(request.BrandIdList.Where(x => x.HasValue).Select(x => x.Value));
                response.BrandList = string.Join(", ", brandNameList);
            }

            //selected buyer name list
            if (request.BuyerIdList != null && request.BuyerIdList.Any())
            {
                var buyerNameList = await _cusBuyerManager.GetBuyerNameByBuyerIds(request.BuyerIdList.Where(x => x.HasValue).Select(x => x.Value));
                response.BuyerList = string.Join(", ", buyerNameList);
            }

            //selected department name list
            if (request.DeptIdList != null && request.DeptIdList.Any())
            {
                var deptNameList = await _cusDeptManager.GetDeptNameByDeptIds(request.DeptIdList.Where(x => x.HasValue).Select(x => x.Value));
                response.DeptList = string.Join(", ", deptNameList);
            }

            if (request.OfficeIdList != null && request.OfficeIdList.Any())
            {
                var officeList = await _officeRepo.GetLocationListByIds(request.OfficeIdList);
                response.OfficeList = string.Join(", ", officeList.Select(x => x.Name));
            }
            if (request.ServiceTypeList != null && request.ServiceTypeList.Any())
            {
                var serviceTypeList = await _referenceManager.GetServiceList();
                response.ServiceTypeList = string.Join(", ", serviceTypeList.DataSourceList.Where(x => request.ServiceTypeList.Contains(x.Id)).Select(x => x.Name));
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
        /// get the total Qc expense claim, travel fee and invoice fee
        /// </summary>
        /// <param name="bookingIdList"></param>
        /// <returns></returns>
        public async Task<ChargeBackChartResponse> GetChargeBackChartData(List<int> bookingIdList)
        {
            var response = new ChargeBackChartData();
            var request = new TurnOverSpRequest
            {
                BookingIdList = bookingIdList != null && bookingIdList.Any() ? bookingIdList.ConvertAll(x => new CommonId { Id = x }) : null,
                EntityId = _tenant.GetCompanyId()
            };

            var res = await _repo.GetChargeBackChartData(request);

            if (res == null)
            {
                return new ChargeBackChartResponse { Result = FinanceDashboardResult.NotFound };
            }

            var exchangeExpenseData = await CurrencyConversionToUsd(res.TotalExpenseAmount, (int)CurrencyMaster.USD);

            foreach (var item in res.TotalExpenseAmount)
            {
                var data = exchangeExpenseData.FirstOrDefault(x => x.CurrencyId == item.CurrencyId);
                item.Fee = data.Fee;
            }

            //get the fees by applying currency exchange rate
            var exchangeCurrencyData = res.InvoiceData.ConvertAll(x => new ExchangeCurrencyItem
            {
                CurrencyId = x.InvoiceCurrencyId,
                Fee = x.TotalInvoiceFee,
                Id = x.BookingId,
                ExtraFee = x.TotalExtraFee,
                ExtraFeeCurrencyId = x.ExtraFeeCurrencyId,
                PriceCardTravelIncluded = x.PriceCardTravelIncluded
            }).ToList();

            exchangeCurrencyData = await CurrencyConversionToUsd(exchangeCurrencyData, (int)CurrencyMaster.USD);

            response.TotalExpense = Math.Round(res.TotalExpenseAmount.Sum(x => x.Fee));
            response.ChargeBack = Math.Round(res.InvoiceData.Sum(x => x.TravelTotalFees));

            //var ratio = response.TotalExpense > 0 ? response.ChargeBack / response.TotalExpense : 0;

            if (response.TotalExpense >= response.ChargeBack)
            {
                response.ChargeBackRatio = response.TotalExpense > 0 ? (response.ChargeBack - response.TotalExpense) / response.TotalExpense : 0;
            }
            else if (response.TotalExpense < response.ChargeBack)
            {
                response.ChargeBackRatio = response.ChargeBack > 0 ? (response.ChargeBack - response.TotalExpense) / response.ChargeBack : 0;
            }
            response.ChargeBackRatio = Math.Round(response.ChargeBackRatio * 100);


            response.TotalRevenue = Math.Round(exchangeCurrencyData.Where(x => !x.PriceCardTravelIncluded).Sum(x => x.Fee) + exchangeCurrencyData.Sum(x => x.ExtraFee));
            response.RevenueChargeBackRatio = response.TotalRevenue != 0 ? response.ChargeBack / response.TotalRevenue : 0;

            response.RevenueChargeBackRatio = Math.Ceiling(response.RevenueChargeBackRatio * 100);

            return new ChargeBackChartResponse
            {
                Data = response,
                Result = FinanceDashboardResult.Success
            };
        }

        /// <summary>
        /// get the quotaiton count and rejected quotation count
        /// </summary>
        /// <param name="bookingIdList"></param>
        /// <returns></returns>
        public async Task<QuotationChartResponse> GetQuotationChartData(List<int> bookingIdList)
        {
            var request = new TurnOverSpRequest
            {
                BookingIdList = bookingIdList != null && bookingIdList.Any() ? bookingIdList.ConvertAll(x => new CommonId { Id = x }) : null,
                EntityId = _tenant.GetCompanyId()
            };

            var res = await _repo.GetQuotationChartData(request);

            if (res == null)
            {
                return new QuotationChartResponse { Result = FinanceDashboardResult.NotFound };
            }

            double percentage = res.QuotationCount > 0 ? (double)res.RejectedQuotationCount / (double)res.QuotationCount : 0;
            res.RejectionPercentage = Math.Round(percentage * 100, 1);

            return new QuotationChartResponse
            {
                Data = res,
                Result = FinanceDashboardResult.Success
            };
        }

        /// <summary>
        /// Export billed manday chart data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<FinanceDashboardMandayExportItem> ExportBilledMandayChart(FinanceDashboardRequest request)
        {
            MandaySPRequest spRequest = new MandaySPRequest();
            List<MandayYearChartItem> res;
            double total;
            double budgetMandayTotal;
            spRequest.ServiceDateTo = request.ServiceDateTo.ToDateTime();
            spRequest.ServiceDateFrom = new DateTime(request.ServiceDateTo.Year, 1, 1);
            spRequest.CustomerId = request.CustomerId != null ? request.CustomerId : 0;
            spRequest.SupplierId = request.SupplierId != null ? request.SupplierId : 0;
            spRequest.FactoryIdList = request.FactoryIdList != null && request.FactoryIdList.Any() ? request.FactoryIdList.ConvertAll(x => new CommonId { Id = x }) : null;
            spRequest.FactoryCountryIdList = request.CountryIdList != null && request.CountryIdList.Any() ? request.CountryIdList.ConvertAll(x => new CommonId { Id = x }) : null;
            spRequest.OfficeIdList = request.OfficeIdList != null && request.OfficeIdList.Any() ? request.OfficeIdList.ConvertAll(x => new CommonId { Id = x }) : null;
            spRequest.ServiceTypeIdList = request.ServiceTypeList != null && request.ServiceTypeList.Any() ? request.ServiceTypeList.ConvertAll(x => new CommonId { Id = x.Value }) : null;
            spRequest.BrandIdList = request.BrandIdList != null && request.BrandIdList.Any() ? request.BrandIdList.ToList().ConvertAll(x => new CommonId { Id = x.Value }) : null;
            spRequest.DepartmentIdList = request.DeptIdList != null && request.DeptIdList.Any() ? request.DeptIdList.ToList().ConvertAll(x => new CommonId { Id = x.Value }) : null;
            spRequest.BuyerIdList = request.BuyerIdList != null && request.BuyerIdList.Any() ? request.BuyerIdList.ToList().ConvertAll(x => new CommonId { Id = x.Value }) : null;
            spRequest.EntityId = _tenant.GetCompanyId();

            if (request.IsBilledMandayExport)
            {
                var result = await _repo.GetBilledMandayData(spRequest);

                res = result.BilledManday;
                total = res.Sum(x => x.MonthManDay);
                budgetMandayTotal = result.BilledMandayBudget.Sum(x => x.MonthManDay);

                foreach (var item in result.BilledMandayBudget)
                {
                    var data = res.FirstOrDefault(x => x.Year == DateTime.Now.Year && x.Month == item.Month);
                    if (data == null)
                    {
                        data = new MandayYearChartItem
                        {
                            Year = DateTime.Now.Year,
                            Month = item.Month,
                            MonthName = MonthData.GetValueOrDefault(item.Month),
                            BudgetManday = item.MonthManDay
                        };

                        res.Add(data);
                    }
                    data.BudgetManday = item.MonthManDay;
                }
                res = res.OrderBy(x => x.Year).ThenBy(x => x.Month).ToList();
            }

            else
            {
                var response = await CommonMandayRateData(request);

                res = response.MandayRate.ConvertAll(x => new MandayYearChartItem
                {
                    Year = x.Year,
                    Month = x.Month,
                    MonthManDay = (int)x.MonthManDay,
                    MonthName = x.MonthName
                });

                total = res.Sum(x => x.MonthManDay);
                budgetMandayTotal = (int)Math.Round(response.MandayRateBudget.Sum(x => x.MonthManDay));

                foreach (var item in response.MandayRateBudget)
                {
                    var data = res.FirstOrDefault(x => x.Year == DateTime.Now.Year && x.Month == item.Month);
                    if (data == null)
                    {
                        data = new MandayYearChartItem
                        {
                            Year = DateTime.Now.Year,
                            Month = item.Month,
                            MonthName = MonthData.GetValueOrDefault(item.Month),
                            BudgetManday = (int)item.MonthManDay
                        };

                        res.Add(data);
                    }
                    data.BudgetManday = (int)item.MonthManDay;
                }
                res = res.OrderBy(x => x.Year).ThenBy(x => x.Month).ToList();
            }

            var requestFilters = await SetExportFilter(request);

            return new FinanceDashboardMandayExportItem
            {
                Data = res,
                RequestFilters = requestFilters,
                Total = total,
                BudgetMandayTotal = budgetMandayTotal,
                IsBilledManday = request.IsBilledMandayExport
            };
        }

        public async Task<FinanceDashboardCommonExportItem> CountryTurnOverExport(FinanceDashboardRequest request)
        {
            var data = await GetBookingIdList(request);

            var bookingIdList = data.Data;

            var countryChartData = await GetFinanceDashboardTurnOverData(bookingIdList);

            var requestFilters = await SetExportFilter(request);

            return new FinanceDashboardCommonExportItem
            {
                Data = countryChartData.CountryData,
                RequestFilters = requestFilters,
                Total = (int)countryChartData.CountryData.Sum(x => x.Count)
            };
        }

        public async Task<FinanceDashboardCommonExportItem> ProductCategoryTurnOverExport(FinanceDashboardRequest request)
        {
            var data = await GetBookingIdList(request);

            var bookingIdList = data.Data;

            var spRequest = new TurnOverSpRequest
            {
                BookingIdList = bookingIdList != null && bookingIdList.Any() ? bookingIdList.ConvertAll(x => new CommonId { Id = x }) : null,
                EntityId = _tenant.GetCompanyId()
            };
            var res = await _repo.GetFinanceTurnOverData(spRequest);

            if (res == null || !res.Any())
            {
                return new FinanceDashboardCommonExportItem { };
            }

            //get the fees by applying currency exchange rate
            var exchangeCurrencyData = res.ConvertAll(x => new ExchangeCurrencyItem
            {
                CurrencyId = x.InvoiceCurrencyId,
                Fee = x.TotalInvoiceFee,
                Id = x.BookingId,
                ExtraFee = x.TotalExtraFee,
                ExtraFeeCurrencyId = x.ExtraFeeCurrencyId
            }).ToList();

            exchangeCurrencyData = await CurrencyConversionToUsd(exchangeCurrencyData, (int)CurrencyMaster.USD);

            foreach (var item in res)
            {
                var bookingData = exchangeCurrencyData.FirstOrDefault(x => x.Id == item.BookingId);
                item.TotalInvoiceFee = bookingData.Fee;
                item.TotalExtraFee = bookingData.ExtraFee;
            }

            var prodCategoryChartData = res.GroupBy(p => p.ProdCategoryId, (key, _data) => new FinanceDashboardCommonItem
            {
                Name = _data.Select(x => x.ProdCategoryName).FirstOrDefault(),
                Count = Math.Round(_data.Sum(x => x.TotalInvoiceFee) + _data.Sum(x => x.TotalExtraFee), 2)
            }).Where(x => x.Count > 0).ToList();

            var requestFilters = await SetExportFilter(request);

            return new FinanceDashboardCommonExportItem
            {
                Data = prodCategoryChartData,
                RequestFilters = requestFilters,
                Total = (int)prodCategoryChartData.Sum(x => x.Count)
            };
        }

        public async Task<FinanceDashboardCommonExportItem> ServiceTypeTurnOverExport(FinanceDashboardRequest request)
        {
            var data = await GetBookingIdList(request);

            var bookingIdList = data.Data;

            var spRequest = new TurnOverSpRequest
            {
                BookingIdList = bookingIdList != null && bookingIdList.Any() ? bookingIdList.ConvertAll(x => new CommonId { Id = x }) : null,
                EntityId = _tenant.GetCompanyId()
            };
            var res = await _repo.GetFinanceTurnOverData(spRequest);

            if (res == null || !res.Any())
            {
                return new FinanceDashboardCommonExportItem { };
            }

            //get the fees by applying currency exchange rate
            var exchangeCurrencyData = res.ConvertAll(x => new ExchangeCurrencyItem
            {
                CurrencyId = x.InvoiceCurrencyId,
                Fee = x.TotalInvoiceFee,
                Id = x.BookingId,
                ExtraFee = x.TotalExtraFee,
                ExtraFeeCurrencyId = x.ExtraFeeCurrencyId
            }).ToList();

            exchangeCurrencyData = await CurrencyConversionToUsd(exchangeCurrencyData, (int)CurrencyMaster.USD);

            foreach (var item in res)
            {
                var bookingData = exchangeCurrencyData.FirstOrDefault(x => x.Id == item.BookingId);
                item.TotalInvoiceFee = bookingData.Fee;
                item.TotalExtraFee = bookingData.ExtraFee;
            }

            var serviceTypeChartData = res.GroupBy(p => p.ServiceTypeId, (key, _data) => new FinanceDashboardCommonItem
            {
                Name = _data.Select(x => x.ServiceTypeName).FirstOrDefault(),
                Count = Math.Round(_data.Sum(x => x.TotalInvoiceFee) + _data.Sum(x => x.TotalExtraFee), 2)
            }).Where(x => x.Count > 0).ToList();

            var requestFilters = await SetExportFilter(request);

            return new FinanceDashboardCommonExportItem
            {
                Data = serviceTypeChartData,
                RequestFilters = requestFilters,
                Total = (int)serviceTypeChartData.Sum(x => x.Count)
            };
        }

        private async Task<RatioAnalysisTableData> CommonRatioAnalysis(FinanceDashboardSearchRequest request)
        {
            RatioAnalysisTableData paramList = new RatioAnalysisTableData();
            request.StatusIdList = new List<int> { (int)BookingStatus.Cancel, (int)BookingStatus.ReportSent, (int)BookingStatus.Inspected };
            var _bookingId = GetQueryableBookingIdList(request);

            paramList.BilledManday = await _repo.GetInspectionBilledManDays(_bookingId);

            if (paramList.BilledManday == null || !paramList.BilledManday.Any())
            {
                return null;
            }

            paramList.InspectionFeesList = await _repo.GetInspectionFees(_bookingId);

            var extraFeeData = await _repo.GetExtraFeeByInspection(_bookingId);

            //get the fees by applying currency exchange rate
            var exchangeCurrencyData = extraFeeData.ConvertAll(x => new ExchangeCurrencyItem
            {
                ExtraFeeCurrencyId = x.ExtraFeeCurrencyId.GetValueOrDefault(),
                ExtraFee = x.ExtraFees.GetValueOrDefault(),
                CustomerId = x.CustomerId
            }).ToList();

            exchangeCurrencyData = await CurrencyConversionToUsd(exchangeCurrencyData, (int)CurrencyMaster.USD);

            var expenseData = await _repo.GetBookingExpense(_bookingId);
            //get the fees by applying currency exchange rate
            paramList.ExpenseData = expenseData.ConvertAll(x => new ExchangeCurrencyItem
            {
                CurrencyId = x.CurrencyId,
                Fee = x.Fee,
                CustomerId = x.CustomerId
            }).ToList();

            paramList.ExpenseData = await CurrencyConversionToUsd(paramList.ExpenseData, (int)CurrencyMaster.USD);


            if (paramList.InspectionFeesList != null && paramList.InspectionFeesList.Any())
            {
                //get the fees by applying currency exchange rate
                var inspectionFeeData = paramList.InspectionFeesList.ConvertAll(x => new ExchangeCurrencyItem
                {
                    Fee = x.inspectionFees.GetValueOrDefault(),
                    CurrencyId = x.CurrencyId.GetValueOrDefault(),
                    ExtraFeeCurrencyId = x.CurrencyId.GetValueOrDefault(),
                    ExtraFee = x.TravelAir.GetValueOrDefault() + x.TravleLand.GetValueOrDefault() + x.HotelFee.GetValueOrDefault() + x.OtherFee.GetValueOrDefault(),
                    CustomerId = x.CustomerId
                }).ToList();

                inspectionFeeData = await CurrencyConversionToUsd(inspectionFeeData, (int)CurrencyMaster.USD);

                var inspData = inspectionFeeData.GroupBy(x => x.CustomerId, (key, _data) =>
                new ExchangeCurrencyItem()
                {
                    Fee = _data.Sum(x => x.Fee),
                    ExtraFee = _data.Sum(x => x.ExtraFee),
                    CustomerId = key
                }).ToList();

                //inspectionFeesGroupby = await ConvertToUsd(inspectionFeesGroupby);

                foreach (var item in paramList.InspectionFeesList)
                {
                    var extFeeData = exchangeCurrencyData.Where(x => x.CustomerId == item.CustomerId).Sum(x => x.ExtraFee);
                    var inspFeeData = inspData.Where(x => x.CustomerId == item.CustomerId).ToList();
                    item.inspectionFees = inspFeeData.Sum(x => x.Fee) + extFeeData;

                    item.TotalChargeBack = inspFeeData.Sum(x => x.ExtraFee);
                }
            }

            paramList.ScheduleManday = await _repo.GetInspectionScheduleManDays(_bookingId, request.RatioEmployeeTypeIdList);

            var qcList = await _schRepo.GetQCBookingDetailsByBookingQuery(_bookingId);
            qcList = qcList.Where(x => x.QCType == (int)QCType.QC).ToList();
            var prodMandayList = new List<ProductionManDay>();
            foreach (var qc in qcList)
            {
                var totalBooking = qcList.Where(x => x.Id == qc.Id && x.ServiceDate == qc.ServiceDate).Select(x => x.BookingId).Count();
                var productionManDay = new ProductionManDay()
                {
                    BookingId = qc.BookingId,
                    CustomerId = qc.CustomerId,
                    QcId = qc.Id,
                    ServiceDate = qc.ServiceDate,
                    ProductionManday = (double)NumberOne / (double)totalBooking
                };
                prodMandayList.Add(productionManDay);
            }
            paramList.ProductionManDayList = prodMandayList;

            return paramList;
        }

        // Get Ratio Analysis List
        public async Task<FinanceDashboardRatioAnalysisResponse> GetRatioAnalysisList(FinanceDashboardSearchRequest request)
        {
            List<FinanceDashboardRatioAnalysis> res = new List<FinanceDashboardRatioAnalysis>();
            var paramList = await CommonRatioAnalysis(request);
            if (paramList != null)
            {
                res = DashBoardMap.MapRatioAnalysisCalculation(paramList.BilledManday, paramList.InspectionFeesList, paramList.ScheduleManday, paramList.ExpenseData, paramList.ProductionManDayList);

                if (res == null || !res.Any())
                {
                    return new FinanceDashboardRatioAnalysisResponse { Result = FinanceDashboardResult.NotFound };
                }
            }
            return new FinanceDashboardRatioAnalysisResponse
            {
                Data = res,
                Result = FinanceDashboardResult.Success
            };
        }
        // Get Ratio Analysis List
        public async Task<List<FinanceDashboardExportRatioAnalysis>> ExportRatioAnalysisList(FinanceDashboardSearchRequest request)
        {
            List<FinanceDashboardExportRatioAnalysis> response = new List<FinanceDashboardExportRatioAnalysis>();
            var paramList = await CommonRatioAnalysis(request);
            if (paramList != null)
            {
                response = DashBoardMap.MapExportRatioAnalysisCalculation(paramList.BilledManday, paramList.InspectionFeesList, paramList.ScheduleManday, paramList.ExpenseData, paramList.ProductionManDayList);
            }

            return response;
        }
        private IQueryable<int> GetQueryableBookingIdList(FinanceDashboardSearchRequest request)
        {

            var inspectionQuery = _sharedInspection.GetAllInspectionQuery();
            var inspectionQueryRequest = _sharedInspection.GetFinanceDashBoardInspectionQueryRequestMap(request);
            var data = _sharedInspection.GetInspectionQuerywithRequestFilters(inspectionQueryRequest, inspectionQuery);

            if(request.RatioEmployeeTypeIdList != null && request.RatioEmployeeTypeIdList.Any())
                data = data.Where(x => x.SchScheduleQcs.Any(y => request.RatioEmployeeTypeIdList.Contains(y.Qc.EmployeeTypeId)));

            //get booking ids
            var bookingIds = data.Select(x => x.Id);

            return bookingIds;
        }

        public async Task<DataSourceResponse> GetEmployeeTypes()
        {
            var data =  await _hrRepo.GetEmployeeTypes();

            if (data == null || !data.Any())
            {
                return new DataSourceResponse { Result = DataSourceResult.CannotGetList };
            }

            return new DataSourceResponse
            {
                DataSourceList = data.Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.EmployeeTypeName
                }).ToList(),

                Result = DataSourceResult.Success
            };
        }
    }
}
