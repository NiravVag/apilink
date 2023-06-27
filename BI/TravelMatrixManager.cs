using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.Customer;
using DTO.Invoice;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BI
{
    public class TravelMatrixManager : ITravelMatrixManager
    {
        private ITravelMatrixRepository _travelMatrixRepository = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly ICustomerPriceCardManager _customerPriceCardManager = null;
      //  private readonly IQuotationManager _quotationManager = null;
        private readonly TravelMatrixMap TravelMatrixMap = null;
        private ITenantProvider _filterService = null;

        public TravelMatrixManager(ITravelMatrixRepository travelMatrixRepository, IAPIUserContext applicationContext,
            ICustomerPriceCardManager customerPriceCardManager, ITenantProvider filterService)
        {
            _travelMatrixRepository = travelMatrixRepository;
            _ApplicationContext = applicationContext;
            _customerPriceCardManager = customerPriceCardManager;
          //  _quotationManager = quotationManager;
            TravelMatrixMap = new TravelMatrixMap();
            _filterService = filterService;
        }

        //get list from InvTmTypes(invoice travel matrix type) table
        public async Task<DataSourceResponse> GetTravelMatrixTypes()
        {
            try
            {
                var data = await _travelMatrixRepository.GetTravelMatrixTypes();

                if (data != null)
                {
                    return new DataSourceResponse()
                    {
                        DataSourceList = data,
                        Result = DataSourceResult.Success
                    };
                }
                else
                {
                    return new DataSourceResponse()
                    {
                        DataSourceList = null,
                        Result = DataSourceResult.CannotGetList
                    };
                }
            }
            catch (Exception)
            {
                //throw ex;
                return new DataSourceResponse() { Result = DataSourceResult.Failed };
            }

        }

        //save and update details with exists check
        public async Task<TravelMatixSaveResponse> Save(IEnumerable<TravelMatrix> model)
        {
            try
            {
                var existModel = new List<TravelMatrixExists>();
                if (model == null)
                    return new TravelMatixSaveResponse() { Result = TravelMatrixResponseResult.RequestNotCorrectFormat };

                var travelMatrixData = await _travelMatrixRepository.GetTravelMatrixDetails(model.Where(x => x.Id > 0).Select(x => x.Id));

                //is exists check with DB
                var response = await ExistsData(model);
                var _entityId = _filterService.GetCompanyId();

                //add a new record
                foreach (var item in response.RequestModel.Where(item => item.Id == null || item.Id <= 0))
                {
                    var mapInsertData = TravelMatrixMap.GetSaveMap(item, _ApplicationContext.UserId, _entityId);
                    travelMatrixData.Add(mapInsertData);
                    _travelMatrixRepository.AddEntity(mapInsertData);
                }

                //update a record(exists in DB)
                foreach (var item in response.RequestModel.Where(item => item.Id > 0))
                {
                    var editData = travelMatrixData.Where(x => x.Id == item.Id).FirstOrDefault();

                    if (editData != null)
                    {
                        _travelMatrixRepository.EditEntity(TravelMatrixMap.GetUpdateMap(editData, item, _ApplicationContext.UserId));
                    }
                }

                await _travelMatrixRepository.Save();

                //exists data map
                if (response.AvailableTravelMatrix != null && response.AvailableTravelMatrix.Any())
                {
                    existModel = response.AvailableTravelMatrix.Select(x => TravelMatrixMap.ExistsMap(x)).ToList();
                }

                return new TravelMatixSaveResponse() { ExistsData = existModel, Result = TravelMatrixResponseResult.Success };
            }
            catch (Exception ex)
            {
                //throw ex;
                return new TravelMatixSaveResponse() { Result = TravelMatrixResponseResult.Error };
            }
        }

        //country, province, city, country, source currency, travel currency, typeid, customer id unique check with request model and db table(inv_tm_details)
        private async Task<ExistsTravelMatrix> ExistsData(IEnumerable<TravelMatrix> model)
        {
            var response = new ExistsTravelMatrix();

            var getAllData = _travelMatrixRepository.GetAllTravelMatrix(TravelMatrixMap.ExistsRequestMap(model));

            var requestModel = model.ToList();

            var existsData = new List<TravelMatrixData>();

            //new record exits check
            foreach (var travelMatrixItem in model)
            {
                var existsCustomizedNew = new List<TravelMatrixData>();

                var existsNew = await getAllData.Where(x => x.SourceCurrencyId == travelMatrixItem.SourceCurrencyId &&
                                x.CountryId == travelMatrixItem.CountryId && x.ProvinceId == travelMatrixItem.ProvinceId &&
                                x.CityId == travelMatrixItem.CityId && x.CountyId == travelMatrixItem.CountyId && x.TariffTypeId == travelMatrixItem.TariffTypeId
                                ).ToListAsync();

                //update record below will execute
                if (travelMatrixItem.Id > 0)
                {
                    existsNew = existsNew.Where(x => x.Id != travelMatrixItem.Id).ToList();
                }

                //if customized tariff type check customer
                if (travelMatrixItem.TariffTypeId == (int)TravelMatrixType.Customized)
                {
                    existsNew = existsNew.Where(x => x.TariffTypeId == (int)TravelMatrixType.Customized &&
                               x.CustomerId == travelMatrixItem.CustomerId).ToList();
                }
                if (existsNew.Any())
                {
                    existsData.Add(existsNew.FirstOrDefault());
                    requestModel.Remove(travelMatrixItem);
                }
            }

            response.RequestModel = requestModel;
            response.AvailableTravelMatrix = existsData;

            return response;
        }

        //search the travel martix data
        public async Task<TravelMatrixSearchResponse> Search(TravelMatrixSummary request)
        {
            List<TravelMatrixSearch> travelMatrixList = null;
            try
            {
                if (request == null)
                    return new TravelMatrixSearchResponse() { Result = TravelMatrixResponseResult.RequestNotCorrectFormat };

                if (request.Index == null || request.Index.Value <= 0)
                    request.Index = 1;

                if (request.PageSize == null || request.PageSize.Value <= 0)
                    request.PageSize = 20;

                int skip = (request.Index.Value - 1) * request.PageSize.Value;

                int take = request.PageSize.Value;

                var tmData = _travelMatrixRepository.GetTravelMatrixData();

                if (request != null && request.CustomerId > 0)
                {
                    tmData = tmData.Where(x => x.CustomerId == request.CustomerId);
                }
                if (request != null && request.TariffTypeId > 0)
                {
                    tmData = tmData.Where(x => x.TariffTypeId == request.TariffTypeId);
                }
                if (request != null && request.TravelCurrencyId > 0)
                {
                    tmData = tmData.Where(x => x.TravelCurrencyId == request.TravelCurrencyId);
                }
                if (request != null && request.SourceCurrencyId > 0)
                {
                    tmData = tmData.Where(x => x.SourceCurrencyId == request.SourceCurrencyId);
                }

                if (request != null && request.CountryId > 0)
                {
                    tmData = tmData.Where(x => x.CountryId == request.CountryId);
                }
                if (request != null && request.ProvinceId > 0)
                {
                    tmData = tmData.Where(x => x.ProvinceId == request.ProvinceId);
                }

                if (request != null && request.CityId > 0)
                {
                    tmData = tmData.Where(x => x.CityId == request.CityId);
                }
                if (request != null && request.CountyId > 0)
                {
                    tmData = tmData.Where(x => x.CountyId == request.CountyId);
                }

                IQueryable<AreaDetails> getAreaIdList = null;

                //get the data configured for the city level
                if (request.SearchTypeId == (int)TravelMatrixSearchEnum.City)
                {
                    getAreaIdList = _travelMatrixRepository.GetDefaultAreaDataByCity();

                    tmData = tmData.Where(x => x.CountyId == null);

                    if (request != null && request.CountryId > 0)
                    {
                        getAreaIdList = getAreaIdList.Where(x => x.CountryId == request.CountryId);
                    }
                    if (request != null && request.ProvinceId > 0)
                    {
                        getAreaIdList = getAreaIdList.Where(x => x.ProvinceId == request.ProvinceId);
                    }
                    if (request != null && request.CityId > 0)
                    {
                        getAreaIdList = getAreaIdList.Where(x => x.CityId == request.CityId);
                    }
                    if (request != null && request.CountyId > 0)
                    {
                        getAreaIdList = getAreaIdList.Where(x => x.CountyId == request.CountyId);
                    }

                    var tmWithDefaultData = _travelMatrixRepository.GetTravelMatrixDefaultDataByCity(tmData, getAreaIdList);

                    var defaultCount = await tmWithDefaultData.CountAsync();

                    bool isDataExist = defaultCount > 0;

                    if (defaultCount == 0)
                        return new TravelMatrixSearchResponse()
                        {
                            Result = TravelMatrixResponseResult.NotFound,
                        };

                    if (request.IsExport)
                        travelMatrixList = await tmWithDefaultData.ToListAsync();
                    else
                        travelMatrixList = await tmWithDefaultData.Skip(skip).Take(take).ToListAsync();

                    return new TravelMatrixSearchResponse()
                    {
                        Result = TravelMatrixResponseResult.DefaultData,
                        TotalCount = defaultCount,
                        Index = request.Index.Value,
                        PageSize = request.PageSize.Value,
                        PageCount = (defaultCount / request.PageSize.Value) + (defaultCount % request.PageSize.Value > 0 ? 1 : 0),
                        GetData = travelMatrixList,
                        IsDataExist = isDataExist
                    };

                }
                else if (request.SearchTypeId == (int)TravelMatrixSearchEnum.County)
                {
                    var defaultCount = await tmData.CountAsync();

                    if (defaultCount == 0)
                        return new TravelMatrixSearchResponse()
                        {
                            Result = TravelMatrixResponseResult.NotFound,
                        };

                    travelMatrixList = await GetTravelMatrixData(tmData, skip, take, request.IsExport);

                    bool isDataExist = defaultCount > 0;

                    return new TravelMatrixSearchResponse()
                    {
                        Result = TravelMatrixResponseResult.Success,
                        TotalCount = defaultCount,
                        Index = request.Index.Value,
                        PageSize = request.PageSize.Value,
                        PageCount = (defaultCount / request.PageSize.Value) + (defaultCount % request.PageSize.Value > 0 ? 1 : 0),
                        GetData = travelMatrixList,
                        IsDataExist = isDataExist
                    };
                }

                return new TravelMatrixSearchResponse() { Result = TravelMatrixResponseResult.NotFound };


            }
            catch (Exception ex)
            {
                return new TravelMatrixSearchResponse() { Result = TravelMatrixResponseResult.Error };
            }
        }

        /// <summary>
        /// Execut the travel matrix data
        /// </summary>
        /// <param name="tmData"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <param name="isExport"></param>
        /// <returns></returns>
        private async Task<List<TravelMatrixSearch>> GetTravelMatrixData(IQueryable<TravelMatrixSearch> tmData, int skip, int take, bool isExport = false)
        {
            List<TravelMatrixSearch> travelMatrixList = new List<TravelMatrixSearch>();
            if (isExport)
                travelMatrixList = await tmData.Select(x => new TravelMatrixSearch()
                {
                    CountryId = x.CountryId,
                    ProvinceId = x.ProvinceId,
                    CityId = x.CityId,
                    CountyId = x.CountyId,
                    Id = x.Id,
                    CustomerId = x.CustomerId,
                    AirCost = x.AirCost,
                    BusCost = x.BusCost,
                    HotelCost = x.HotelCost,
                    TaxiCost = x.TaxiCost,
                    TrainCost = x.TrainCost,
                    DistanceKM = x.DistanceKM,
                    FixExchangeRate = x.FixExchangeRate,
                    MarkUpCostAir = x.MarkUpCostAir,
                    SourceCurrencyId = x.SourceCurrencyId,
                    TravelCurrencyId = x.TravelCurrencyId,
                    MarkUpCost = x.MarkUpCost,
                    TravelTime = x.TravelTime,
                    OtherCost = x.OtherCost,
                    InspPortId = x.InspPortId,
                    Remarks = x.Remarks,
                    TariffTypeId = x.TariffTypeId,
                    CustomerName = x.CustomerName,
                    CountryName = x.CountryName,
                    ProvinceName = x.ProvinceName,
                    CityName = x.CityName,
                    CountyName = x.CountyName,
                    TariffTypeName = x.TariffTypeName,
                    InspPortName = x.InspPortName,
                    InspPortCityId = x.InspPortCityId,
                    InspPortCityName = x.InspPortCityName,
                    SourceCurrencyName = x.SourceCurrencyName,
                    TravelCurrencyName = x.TravelCurrencyName
                }).AsNoTracking().ToListAsync();
            else
                travelMatrixList = await tmData.Select(x => new TravelMatrixSearch()
                {
                    CountryId = x.CountryId,
                    ProvinceId = x.ProvinceId,
                    CityId = x.CityId,
                    CountyId = x.CountyId,
                    Id = x.Id,
                    CustomerId = x.CustomerId,
                    AirCost = x.AirCost,
                    BusCost = x.BusCost,
                    HotelCost = x.HotelCost,
                    TaxiCost = x.TaxiCost,
                    TrainCost = x.TrainCost,
                    DistanceKM = x.DistanceKM,
                    FixExchangeRate = x.FixExchangeRate,
                    MarkUpCostAir = x.MarkUpCostAir,
                    SourceCurrencyId = x.SourceCurrencyId,
                    TravelCurrencyId = x.TravelCurrencyId,
                    MarkUpCost = x.MarkUpCost,
                    TravelTime = x.TravelTime,
                    OtherCost = x.OtherCost,
                    InspPortId = x.InspPortId,
                    Remarks = x.Remarks,
                    TariffTypeId = x.TariffTypeId,
                    CustomerName = x.CustomerName,
                    CountryName = x.CountryName,
                    ProvinceName = x.ProvinceName,
                    CityName = x.CityName,
                    CountyName = x.CountyName,
                    TariffTypeName = x.TariffTypeName,
                    InspPortName = x.InspPortName,
                    InspPortCityId = x.InspPortCityId,
                    InspPortCityName = x.InspPortCityName,
                    SourceCurrencyName = x.SourceCurrencyName,
                    TravelCurrencyName = x.TravelCurrencyName
                }).AsNoTracking().Skip(skip).Take(take).ToListAsync();

            return travelMatrixList;
        }

        //get province list with country id
        public async Task<AreaDataResponse> GetProvinceLists(IEnumerable<int> countryIds)
        {
            try
            {
                var response = new AreaDataResponse
                {
                    AreaDataList = new List<AreaData>()
                };

                var provinceList = await _travelMatrixRepository.GetProvinceLists(countryIds);

                foreach (var countryId in countryIds)
                {
                    var provinceData = new AreaData();

                    provinceData.DataList = provinceList?.Where(x => x.ParentId == countryId)?.Select(x => TravelMatrixMap.GetAreaMap(x))?.ToList();

                    provinceData.Id = countryId;

                    response.AreaDataList.Add(provinceData);

                }
                response.Result = TravelMatrixResponseResult.Success;

                return response;
            }
            catch (Exception ex)
            {
                //throw ex;
                return new AreaDataResponse() { Result = TravelMatrixResponseResult.Error };
            }
        }

        //get city list with province id
        public async Task<AreaDataResponse> GetCityLists(IEnumerable<int> provinceIds)
        {
            try
            {
                var response = new AreaDataResponse
                {
                    AreaDataList = new List<AreaData>()
                };

                var cityList = await _travelMatrixRepository.GetCityLists(provinceIds);

                foreach (var provinceId in provinceIds)
                {
                    var cityData = new AreaData();

                    cityData.DataList = cityList?.Where(x => x.ParentId == provinceId)?.Select(x => TravelMatrixMap.GetAreaMap(x))?.ToList();

                    cityData.Id = provinceId;

                    response.AreaDataList.Add(cityData);

                }
                response.Result = TravelMatrixResponseResult.Success;

                return response;
            }
            catch (Exception ex)
            {
                //throw ex;
                return new AreaDataResponse() { Result = TravelMatrixResponseResult.Error };
            }
        }

        //get county list with city id
        public async Task<AreaDataResponse> GetCountyLists(IEnumerable<int> cityIds)
        {
            try
            {
                var response = new AreaDataResponse
                {
                    AreaDataList = new List<AreaData>()
                };

                var countyList = await _travelMatrixRepository.GetCountyLists(cityIds);

                foreach (var cityId in cityIds)
                {
                    var countyData = new AreaData();

                    countyData.DataList = countyList?.Where(x => x.ParentId == cityId)?.Select(x => TravelMatrixMap.GetAreaMap(x))?.ToList();

                    countyData.Id = cityId;

                    response.AreaDataList.Add(countyData);

                }
                response.Result = TravelMatrixResponseResult.Success;

                return response;
            }
            catch (Exception ex)
            {
                //throw ex;
                return new AreaDataResponse() { Result = TravelMatrixResponseResult.Error };
            }
        }

        //get county list with city id
        public async Task<DataSourceResponse> GetCountyListByCountry(int CountryId, string CountyName)
        {
            try
            {
                var response = new DataSourceResponse
                {
                };

                List<int> countryList = new List<int>() { CountryId };

                var provinceList = await _travelMatrixRepository.GetProvinceLists(countryList);

                var cityList = await _travelMatrixRepository.GetCityLists(provinceList.Select(x => x.Id));

                var countyList = await _travelMatrixRepository.GetCountyListsByCountyName(cityList.Select(x => x.Id), CountyName);

                response.DataSourceList = countyList;

                response.Result = DataSourceResult.Success;

                return response;
            }
            catch (Exception ex)
            {
                return new DataSourceResponse() { Result = DataSourceResult.Failed };
            }
        }


        //delete the travel matrix details logically
        public async Task<TravelMatixDeleteResponse> Delete(IEnumerable<int?> ids)
        {
            try
            {
                if (ids == null)
                    return new TravelMatixDeleteResponse() { Result = TravelMatrixResponseResult.RequestNotCorrectFormat };

                var matrixDetails = await _travelMatrixRepository.GetTravelMatrixDetails(ids);

                foreach (var item in matrixDetails)
                {

                    item.DeletedBy = _ApplicationContext.UserId;
                    item.DeletedOn = DateTime.Now;
                    item.Active = false;
                }

                _travelMatrixRepository.EditEntities(matrixDetails);

                await _travelMatrixRepository.Save();

                return new TravelMatixDeleteResponse() { Result = TravelMatrixResponseResult.Success };
            }
            catch (Exception ex)
            {
                //throw ex;
                return new TravelMatixDeleteResponse() { Result = TravelMatrixResponseResult.Error };
            }
        }

        //export summary details
        public IEnumerable<TravelMatrixExportSummary> ExportSummary(IEnumerable<TravelMatrixSearch> data)
        {
            return data.Select(x => TravelMatrixMap.MapExportSummary(x));
        }

       

       

        /// <summary>
        /// Get the travel matrix executed list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<TravelMatrixSearch>> GetTravelMatrixList(QuotationMatrixRequest request)
        {
            //get the travel matrix query
            var travelMatrixQuery = _travelMatrixRepository.GetTravelMatrixData(request);

            //apply the travel matrix county id
            if (request.CountyId > 0)
                travelMatrixQuery = travelMatrixQuery.Where(x => x.CountyId == request.CountyId);
            //apply the travel matrix city id
            else if (request.CityId > 0)
                travelMatrixQuery = travelMatrixQuery.Where(x => x.CityId == request.CityId && x.CountyId == null);
            //apply the travel matrix province id
            else if (request.ProvinceId > 0)
                travelMatrixQuery = travelMatrixQuery.Where(x => x.ProvinceId == request.ProvinceId && x.CityId == null
                                                                            && x.CountyId == null);

            return await travelMatrixQuery.AsNoTracking().ToListAsync();
        }
    }
}
