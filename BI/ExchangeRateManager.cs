using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.ExchangeRate;
using DTO.HumanResource;
using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO.Common;
using DTO.FinanceDashboard;

namespace BI
{
    public class ExchangeRateManager : IExchangeRateManager
    {
        private readonly IReferenceManager _referenceManager = null;
        private readonly IExchangeRateRepository _repo = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly CurrencyMap _currencymap = null;
        private ITenantProvider _filterService = null;
        private readonly LocationMap _locationmap = null;
        public ExchangeRateManager(IReferenceManager referenceManager, IExchangeRateRepository repo, IAPIUserContext IAPIUserContext, ITenantProvider filterService)
        {
            _referenceManager = referenceManager;
            _repo = repo;
            _ApplicationContext = IAPIUserContext;
            _currencymap = new CurrencyMap();
            _locationmap = new LocationMap();
            _filterService = filterService;
        }


        public async Task<ExchangeRateResponse> GetExchangeRateSummary()
        {
            var response = new ExchangeRateResponse(); 

            //CurrencyList
            response.CurrencyList = _referenceManager.GetCurrencies();

            if (response.CurrencyList == null || !response.CurrencyList.Any())
                return new ExchangeRateResponse { Result =  ExchangeRateResult.CannotGetCurrencyList };

            // Rate Type List
            var data = await _repo.GetRateTypeList();

            if(data == null || !data.Any())
                return new ExchangeRateResponse { Result = ExchangeRateResult.CannotGetRateTypeList };

            response.RateTypeList = data.Select(_currencymap.GetRateType);

            response.Result = ExchangeRateResult.Success;

            return response; 
        }

        public async Task<ExchangeDataResponse> GetExchangeResult(ExchangeDataRequest request)
        {

            if (request.Index == null || request.Index.Value == 0)
                request.Index = 1;

            if (request.pageSize == null || request.pageSize.Value == 0)
                request.pageSize = 20;


            var response = new ExchangeDataResponse(); 

            var data = await _repo.GetExchangeRateList(request.Currency.Id, request.FromDate.ToDateTime(), request.ToDate.ToDateTime(), request.ExchangeType.Id);

            if (data == null || !data.Any())
                return new ExchangeDataResponse { Result = ExchangeDataResult.NoDataFound };

            // Currency List
            response.CurrencyList = data.Select(x => x.Currencyid2Navigation).Distinct().Select(_locationmap.GetCurrency);

            // Data
            var lst = new List<CurrencyRateItem>(); 

            foreach(var item in data)
            {

                var dateitem = lst.FirstOrDefault(x => x.BeginDate.Equals(item.BeginDate.GetCustomDate()) && x.EndDate.Equals(item.EndDate.GetCustomDate()));

                if (dateitem == null)
                {
                    dateitem = new CurrencyRateItem { BeginDate = item.BeginDate.GetCustomDate(), EndDate = item.EndDate.GetCustomDate() };
                    lst.Add(dateitem);
                }

                if (dateitem.RateList == null)
                    dateitem.RateList = new List<RateItem>();

                dateitem.RateList.Add(new RateItem
                {
                    CurrencyId = item.Currencyid2,
                    Value = item.Rate,
                    ConversionId = item.Id
                });
            }

            response.TotalCount = lst.Count;

            int skip = (request.Index.Value - 1) * request.pageSize.Value;
            response.PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0);
            lst = lst.OrderByDescending(x => x.BeginDate.ToDateTime()).Skip(skip).Take(request.pageSize.Value).ToList();

            response.Data = lst;
            response.Result = ExchangeDataResult.Success;

            return response;
        }

        public async Task<SaveExchangeRateResponse> SaveExchangeRate(SaveConversionRequest request)
        {
            // Add new rows             
            var response = new SaveExchangeRateResponse();
            var lst = new List<EmExchangeRate>();
            IEnumerable<EmExchangeRate> convList = new HashSet<EmExchangeRate>();

            int _entityId = _filterService.GetCompanyId();
            // Add new dates and new currencies for existing dates
            foreach (var item in request.Data.Where(x => x.IsNew || (!x.IsNew && x.RateList != null && x.RateList.Any(y => y.ConversionId == 0)) ))
            {
                foreach (var conversion in item.RateList.Where(x => x.ConversionId == 0))
                {
                    var rateConv = new EmExchangeRate
                    {
                        Active = true,
                        CreateDate = DateTime.Now,
                        LastUpdateDate = DateTime.Now,
                        CurrencyId1 = request.CurrencyTargetId,
                        Currencyid2 = conversion.CurrencyId,
                        EntityId = _entityId,
                        BeginDate = item.BeginDate.ToDateTime(),
                        EndDate = item.EndDate.ToDateTime(),
                        UserId = _ApplicationContext.UserId,
                        ExRateTypeId = request.ExRateTypeId,
                        Rate = conversion.Value
                    };

                    _repo.AddEntity(rateConv);

                    lst.Add(rateConv);
                }
            }

            if (request.Data.Any(x => !x.IsNew))
            {
                convList = await _repo.GetConversionList(request.Data.Where(x => !x.IsNew).SelectMany(x => x.RateList).Select(x => x.ConversionId));

                if (convList == null)
                    return new SaveExchangeRateResponse { Result = SaveExchangeRateResult.CannotGetData };


                foreach (var item in request.Data.Where(x => !x.IsNew))
                {
                    foreach (var conversion in item.RateList)
                    {
                        var itemInfo = convList.FirstOrDefault(x => x.Id == conversion.ConversionId);

                        if (itemInfo != null)
                        {
                            itemInfo.LastUpdateDate = DateTime.Now;
                            itemInfo.Rate = conversion.Value;
                        } 

                    }
                }

                // Edit
                _repo.EditEntities(convList);
            }

             await _repo.Save();

            return new SaveExchangeRateResponse
            {
                AddList = lst.Any() ?  lst.Select(x => x.Id) : null,
                EditList = convList.Any() ?  convList.Select(x => x.Id) : null,
                Result = SaveExchangeRateResult.Success
            };
        }

        public async Task<RateMatrixResponse> GetMatrixRate(RateMatrixRequest request)
        {
            if (request.Currency == null || request.Currency.Id <= 0)
                return new RateMatrixResponse { Result = RateMatrixResult.CurrencyIsRequired };

            if(request.FromDate == null || request.ToDate == null)
                return new RateMatrixResponse { Result = RateMatrixResult.DateIsrequired };

            if(request.ExchangeType == null || request.ExchangeType.Id <=0)
                return new RateMatrixResponse { Result = RateMatrixResult.ExchangeTypeIsRequired };


            var data = await _repo.GetExchangeRateList(request.Currency.Id, request.FromDate.ToDateTime(), request.ToDate.ToDateTime(), request.ExchangeType.Id);

            if (data == null || !data.Any())
                return new RateMatrixResponse { Result =  RateMatrixResult.NotFound };

            var lst = new List<CurrencyItem>();

            // Target item 

            var currencyList = new List<CurrencyValue>();
            var currecyItems = new List<CurrencyItem>(); 

            var item = new CurrencyItem
            {
                Currency = _locationmap.GetCurrency(data.First().CurrencyId1Navigation),
                CurrencyValueList = currencyList
            };

            currencyList.Add(new CurrencyValue { Currency = item.Currency, Value = "1" });

            foreach(var element in data)
            {
                currencyList.Add(new CurrencyValue
                {
                    Currency = _locationmap.GetCurrency(element.Currencyid2Navigation),
                    Value = GetValue(element.Rate)
                });
            }

            currecyItems.Add(item);

            // Other values
            foreach(var element in data)
            {
                var currentValueList = new List<CurrencyValue>();
                var currentItem = new CurrencyItem
                {
                    Currency = _locationmap.GetCurrency(element.Currencyid2Navigation),
                    CurrencyValueList = currentValueList
                };

                var currentValue = Convert.ToDouble(currencyList.First(x => x.Currency.Id == element.Currencyid2).Value);

                foreach (var orgElement in currencyList)
                {
                    double value = currentValue == 0 ? 0 : Convert.ToDouble(orgElement.Value) / currentValue ; 

                    currentValueList.Add(new CurrencyValue
                    {
                        Currency = orgElement.Currency,
                        Value = GetValue(value)
                    }); 
                }

                currecyItems.Add(currentItem);
            }

            return new RateMatrixResponse
            {
                CurrencyTargetId = request.Currency.Id,
                Data = currecyItems,
                FromDate = request.FromDate,
                ToDate = request.ToDate,
                ExchangeTypeId = request.ExchangeType.Id,
                PageCount = currecyItems.Count,
                TotalCount = currecyItems.Count,
                Result = RateMatrixResult.Success
            };

        }

        public async Task<MatrixDataExport> GetMatrixRateExport(int currencyId, DateObject fromDate, DateObject toDate, int typeId )
        {
            var data = await _repo.GetExchangeRateList(currencyId, fromDate.ToDateTime(), toDate.ToDateTime(), typeId);

            if (data == null || !data.Any())
                return null;

            var lst =  data.Select(x => new CurrencyValue
            {
                Currency = _locationmap.GetCurrency(x.Currencyid2Navigation),
                Value = x.Rate.ToString()
            }).ToList();

            lst.Insert(0, new CurrencyValue
            {
                Currency = _locationmap.GetCurrency(data.First().CurrencyId1Navigation),
                Value ="1"
            });

            return new MatrixDataExport {
                CurrencyTarget = lst.First().Currency.CurrencyName,
                Data = lst,
                FromDate = fromDate.ToDateTime().ToString("dd MMM yyyy"),
                ToDate = toDate.ToDateTime().ToString("dd MMM yyyy")
            }; 
        }

        public async Task<string> GetExchangeRate(int targetId, int currencyId, DateObject date, ExhangeRateTypeEnum type)
        {
            if (targetId == currencyId)
                return "1";

            var data = await _repo.GetExchanges(targetId, currencyId, date.ToDateTime(), (int)type);

            if (data == null || !data.Any())
                return "1";

            // scenario 1 : target exists and currency2 exists an same row
            var target = data.FirstOrDefault(x => x.CurrencyId1 == targetId && x.Currencyid2 == currencyId);

            if (target != null)
                return GetValue(target.Rate);

            // scenario 2 :  target exists as currency 2 and currency Id as Currency 1
            target = data.FirstOrDefault(x => x.CurrencyId1 == currencyId && x.Currencyid2 == targetId);

            if (target != null)
                return GetValue(1 / target.Rate);

            // scenario 3 : Target and Currency exist as currency2 and they have common currency1 
            var itemsTarget = data.Where(x => x.Currencyid2 == targetId).ToArray();

            if (itemsTarget == null || itemsTarget.Length == 0) // No rate
                return "1";

            var itemCurrency = data.FirstOrDefault(x => x.Currencyid2 == currencyId && itemsTarget.Any(y => y.CurrencyId1 == x.CurrencyId1));

            if (itemCurrency == null)
                return "1";

            target = itemsTarget.First(x => x.CurrencyId1 == itemCurrency.CurrencyId1);

            double value = itemCurrency.Rate / target.Rate;
            return GetValue(value);

        }

        public async Task<decimal> GetFoodAllowance(DateObject date, int countryId)
        {
            return await _repo.GetFoodAllowance(date.ToDateTime(), countryId);
        }

        private string GetValue(double value)
        {
            string[] valueStr = value.ToString().Split(new char[] { ',', '.' }, StringSplitOptions.RemoveEmptyEntries);

            if (valueStr.Length > 1)
            {
                if(valueStr[1].Length > 6)
                    return string.Format("{0:0.000000}", value);
            }

            return value.ToString();
        }

        public async Task<Dictionary<int, double>> GetExchangeRateList(List<ExchangeCurrency> currencyList, DateTime date, ExhangeRateTypeEnum type)
        {
            Dictionary<int, double> dicexrate = new Dictionary<int, double>();

            if (currencyList != null && currencyList.Any())
            {
                if (currencyList.Count == 1 && currencyList.Select(x => x.TargetCurrency).FirstOrDefault() == currencyList.Select(x => x.Currency).FirstOrDefault())
                {
                    dicexrate.Add(currencyList.Select(x => x.Currency).FirstOrDefault(), Convert.ToDouble(1));
                    return dicexrate;
                }


                var data = await _repo.GetExchangesList(currencyList, date, (int)type);

                if (data == null || !data.Any())
                {
                    foreach (var item in currencyList)
                    {
                        if (!dicexrate.ContainsKey(item.Currency))
                        {
                            dicexrate.Add(item.Currency, Convert.ToDouble(1));
                        }
                    }
                    return dicexrate;
                }

                foreach (var item in currencyList)
                {
                    // scenario 1 : target exists and currency2 exists an same row
                    var target = data.FirstOrDefault(x => x.CurrencyId1 == item.TargetCurrency && x.Currencyid2 == item.Currency);

                    if (target != null)
                    {
                        var rate = GetValue(target.Rate);
                        if (!string.IsNullOrEmpty(rate) && !dicexrate.ContainsKey(item.Currency))
                        {
                            dicexrate.Add(item.Currency, Convert.ToDouble(rate));
                        }
                        continue;
                    }


                    // scenario 2 :  target exists as currency 2 and currency Id as Currency 1
                    target = data.FirstOrDefault(x => x.CurrencyId1 == item.Currency && x.Currencyid2 == item.TargetCurrency);

                    if (target != null)
                    {
                        var rate = GetValue(1 / target.Rate);
                        if (!string.IsNullOrEmpty(rate) && !dicexrate.ContainsKey(item.Currency))
                        {
                            dicexrate.Add(item.Currency, Convert.ToDouble(rate));
                        }
                        continue;
                    }

                    // scenario 3 : Target and Currency exist as currency2 and they have common currency1 
                    var itemsTarget = data.Where(x => x.Currencyid2 == item.TargetCurrency).ToArray();

                    if (!dicexrate.ContainsKey(item.Currency) && (itemsTarget == null || itemsTarget.Length == 0)) // No rate
                    {
                        dicexrate.Add(item.Currency, Convert.ToDouble(1));
                        continue;
                    }

                    var itemCurrency = data.FirstOrDefault(x => x.Currencyid2 == item.Currency && itemsTarget.Any(y => y.CurrencyId1 == x.CurrencyId1));

                    if (itemCurrency == null && !dicexrate.ContainsKey(item.Currency))
                    {
                        dicexrate.Add(item.Currency, Convert.ToDouble(1));
                        continue;
                    }

                    if (itemCurrency != null)
                    {
                        target = itemsTarget.First(x => x.CurrencyId1 == itemCurrency.CurrencyId1);

                        double value = itemCurrency.Rate / target.Rate;
                        var exchangerate = GetValue(value);

                        if (!string.IsNullOrEmpty(exchangerate) && !dicexrate.ContainsKey(item.Currency))
                        {
                            dicexrate.Add(item.Currency, Convert.ToDouble(exchangerate));
                        }
                    }                    
                }
            }
            return dicexrate;

        }
    }
}
