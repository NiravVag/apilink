using Contracts.Repositories;
using DTO.CommonClass;
using DTO.Invoice;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class TravelMatrixRepository : Repository, ITravelMatrixRepository
    {
        public TravelMatrixRepository(API_DBContext context) : base(context)
        {

        }

        //get list from InvTmTypes(invoice travel matrix type) table
        public async Task<IEnumerable<CommonDataSource>> GetTravelMatrixTypes()
        {
            return await _context.InvTmTypes.Where(x => x.Active.Value).OrderBy(x => x.Name)
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync();
        }

        //get active data from invtmdetails table
        public IQueryable<TravelMatrixSearch> GetTravelMatrixData()
        {
            return _context.InvTmDetails.Where(x => x.Active.Value)
                .Select(x => new TravelMatrixSearch
                {
                    Id = x.Id,
                    CityId = x.CityId,
                    CountryId = x.CountryId,
                    CountyId = x.CountyId,
                    CountyCityId = x.County.CityId,
                    ProvinceId = x.ProvinceId,
                    CustomerId = x.CustomerId,
                    AirCost = x.AirCost,
                    BusCost = x.BusCost,
                    HotelCost = x.HotelCost,
                    TaxiCost = x.TaxiCost,
                    TrainCost = x.TrainCost,
                    DistanceKM = x.DistanceKm,
                    FixExchangeRate = x.FixedExchangeRate,
                    MarkUpCostAir = x.MarkUpAirCost,
                    SourceCurrencyId = x.SourceCurrencyId,
                    TravelCurrencyId = x.TravelCurrencyId,
                    MarkUpCost = x.MarkUpCost,
                    TravelTime = x.TravelTime,
                    OtherCost = x.OtherCost,
                    InspPortId = x.InspPortCountyId,
                    Remarks = x.Remarks,
                    TariffTypeId = x.TravelMatrixTypeId,
                    CustomerName = x.Customer.CustomerName,
                    CountryName = x.Country.CountryName,
                    ProvinceName = x.Province.ProvinceName,
                    CityName = x.City.CityName,
                    CountyName = x.County.CountyName,
                    TariffTypeName = x.TravelMatrixType.Name,
                    InspPortName = x.InspPortCounty.CountyName,
                    SourceCurrencyName = x.SourceCurrency.CurrencyName,
                    TravelCurrencyName = x.TravelCurrency.CurrencyName,
                    InspPortCityId = x.InspPortCityId,
                    InspPortCityName = x.InspPortCity.CityName
                });
        }

        //get InvTmDetails table details with list of ids and active
        public Task<List<InvTmDetail>> GetTravelMatrixDetails(IEnumerable<int?> ids)
        {
            return _context.InvTmDetails.Where(x => x.Active.Value && ids.Contains(x.Id)).ToListAsync();
        }

        public IQueryable<TravelMatrixData> GetAllTravelMatrix(ExistsTravelMatrixRequest request)
        {
            return _context.InvTmDetails.Where(x => x.Active.Value &&
                            request.CountryList.Contains(x.CountryId)
                            && request.ProvinceList.Contains(x.ProvinceId) &&
                            request.SourceCurrencyList.Contains(x.SourceCurrencyId) &&
                            request.TariffTypeList.Contains(x.TravelMatrixTypeId))
                .Select(x => new TravelMatrixData
                {

                    Id = x.Id,
                    CustomerId = x.CustomerId,
                    SourceCurrencyId = x.SourceCurrencyId,
                    CityId = x.CityId,
                    CityName = x.City.CityName,
                    CountryId = x.CountryId,
                    CountryName = x.Country.CountryName,
                    CountyId = x.CountyId,
                    CountyName = x.County.CountyName,
                    CustomerName = x.Customer.CustomerName,
                    ProvinceId = x.ProvinceId,
                    ProvinceName = x.Province.ProvinceName,
                    SourceCurrencyName = x.SourceCurrency.CurrencyName,
                    TravelCurrencyName = x.TravelCurrency.CurrencyName,
                    TariffTypeId = x.TravelMatrixTypeId,
                    TariffTypeName = x.TravelMatrixType.Name
                });
        }

        //get area details like country province city county
        public IQueryable<AreaDetails> GetDefaultAreaData()
        {
            //&& travelMatrix.Active.Value
            return (from country in _context.RefCountries
                    join province in _context.RefProvinces on country.Id equals province.CountryId into provinceGrouping
                    from province in provinceGrouping.DefaultIfEmpty()
                    join city in _context.RefCities on province.Id equals city.ProvinceId into cityGrouping
                    from city in cityGrouping.DefaultIfEmpty()
                    join county in _context.RefCounties on city.Id equals county.CityId into countyGrouping
                    from county in countyGrouping.DefaultIfEmpty()
                    where (country.Active && province.Active.Value && (city == null || city.Active) && (county == null || county.Active))
                    select new AreaDetails
                    {
                        CountryId = country.Id,
                        ProvinceId = province.Id,
                        CityId = city.Id,
                        CountryName=country.CountryName,
                        ProvinceName=province.ProvinceName,
                        CityName=city.CityName,
                        CountyId = county.Id,
                        AreaCountryName = country.CountryName,
                        AreaProvinceName = province.ProvinceName,
                        AreaCityName = city.CityName,
                        AreaCountyName = county.CountyName
                    });
        }

        /// <summary>
        /// Get Default Area By city config
        /// </summary>
        /// <returns></returns>
        public IQueryable<AreaDetails> GetDefaultAreaDataByCity()
        {
            //&& travelMatrix.Active.Value
            return (from country in _context.RefCountries
                    join province in _context.RefProvinces on country.Id equals province.CountryId into provinceGrouping
                    from province in provinceGrouping.DefaultIfEmpty()
                    join city in _context.RefCities on province.Id equals city.ProvinceId into cityGrouping
                    from city in cityGrouping.DefaultIfEmpty()
                    where (country.Active && province.Active.Value && (city == null || city.Active))
                    select new AreaDetails
                    {
                        CountryId = country.Id,
                        ProvinceId = province.Id,
                        CityId = city.Id,
                        CountryName = country.CountryName,
                        ProvinceName = province.ProvinceName,
                        CityName = city.CityName,
                        AreaCountryName = country.CountryName,
                        AreaProvinceName = province.ProvinceName,
                        AreaCityName = city.CityName,
                    });
        }

        //pass the country id list and get province data with parent id(country)
        public async Task<IEnumerable<ParentDataSource>> GetProvinceLists(IEnumerable<int> countryIds)
        {
            return await _context.RefProvinces.Where(x => countryIds.Contains(x.CountryId)).OrderBy(x => x.ProvinceName)
                .Select(x => new ParentDataSource
                {
                    Id = x.Id,
                    Name = x.ProvinceName,
                    ParentId = x.CountryId
                })
                .ToListAsync();
        }

        //pass the province id list and get city data with parent id(province)
        public async Task<IEnumerable<ParentDataSource>> GetCityLists(IEnumerable<int> provinceIds)
        {
            return await _context.RefCities.Where(x => provinceIds.Contains(x.ProvinceId)).OrderBy(x => x.CityName)
                .Select(x => new ParentDataSource
                {
                    Id = x.Id,
                    Name = x.CityName,
                    ParentId = x.ProvinceId
                })
                .ToListAsync();
        }

        //pass the city id list and get county data with parent id(county)
        public async Task<IEnumerable<ParentDataSource>> GetCountyLists(IEnumerable<int> cityIds)
        {
            return await _context.RefCounties.Where(x => cityIds.Contains(x.CityId)).OrderBy(x => x.CountyName)
                .Select(x => new ParentDataSource
                {
                    Id = x.Id,
                    Name = x.CountyName,
                    ParentId = x.CityId
                }).
                ToListAsync();
        }

        public async Task<IEnumerable<CommonDataSource>> GetCountyListsByCountyName(IEnumerable<int> cityIds, string countyName)
        {
            return await _context.RefCounties.Where(x => cityIds.Contains(x.CityId) &&
                              EF.Functions.Like(x.CountyName, $"%{countyName.Trim()}%")).OrderBy(x => x.CountyName)
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.CountyName
                }).
                ToListAsync();
        }

        /// <summary>
        /// Get travel matrix data by county config
        /// </summary>
        /// <param name="tmData"></param>
        /// <param name="areaData"></param>
        /// <returns></returns>
        public IQueryable<TravelMatrixSearch> GetTravelMatrixDefaultDataByCounty(IQueryable<TravelMatrixSearch> tmData, IQueryable<AreaDetails> areaData)
        {
            return (from areaDetails in areaData
                    join travelMatrix in tmData on areaDetails.CountyId equals travelMatrix.CountyId into travelMatrixGrouping
                    from travelMatrix in travelMatrixGrouping.DefaultIfEmpty()
                    orderby areaDetails.AreaCountryName, areaDetails.AreaProvinceName, areaDetails.AreaCityName, areaDetails.AreaCountyName
                    select new TravelMatrixSearch
                    {
                        CountryId = areaDetails.CountryId,
                        ProvinceId = areaDetails.ProvinceId,
                        CityId = areaDetails.CityId,
                        CountyId = areaDetails.CountyId,
                        Id = travelMatrix.Id,
                        CustomerId = travelMatrix.CustomerId,
                        AirCost = travelMatrix.AirCost,
                        BusCost = travelMatrix.BusCost,
                        HotelCost = travelMatrix.HotelCost,
                        TaxiCost = travelMatrix.TaxiCost,
                        TrainCost = travelMatrix.TrainCost,
                        DistanceKM = travelMatrix.DistanceKM,
                        FixExchangeRate = travelMatrix.FixExchangeRate,
                        MarkUpCostAir = travelMatrix.MarkUpCostAir,
                        SourceCurrencyId = travelMatrix.SourceCurrencyId,
                        TravelCurrencyId = travelMatrix.TravelCurrencyId,
                        MarkUpCost = travelMatrix.MarkUpCost,
                        TravelTime = travelMatrix.TravelTime,
                        OtherCost = travelMatrix.OtherCost,
                        InspPortId = travelMatrix.InspPortId,
                        Remarks = travelMatrix.Remarks,
                        TariffTypeId = travelMatrix.TariffTypeId,
                        CustomerName = travelMatrix.CustomerName,
                        CountryName = travelMatrix.CountryName,
                        ProvinceName = travelMatrix.ProvinceName,
                        CityName = travelMatrix.CityName,
                        CountyName = travelMatrix.CountyName,
                        TariffTypeName = travelMatrix.TariffTypeName,
                        InspPortName = travelMatrix.InspPortName,
                        InspPortCityId = travelMatrix.InspPortCityId,
                        InspPortCityName = travelMatrix.InspPortCityName,
                        SourceCurrencyName = travelMatrix.SourceCurrencyName,
                        TravelCurrencyName = travelMatrix.TravelCurrencyName
                    });
        }

        /// <summary>
        /// Get travel matrix data by city
        /// </summary>
        /// <param name="tmData"></param>
        /// <param name="areaData"></param>
        /// <returns></returns>
        public IQueryable<TravelMatrixSearch> GetTravelMatrixDefaultDataByCity(IQueryable<TravelMatrixSearch> tmData, IQueryable<AreaDetails> areaData)
        {
            return (from areaDetails in areaData
                    join travelMatrix in tmData on areaDetails.CityId equals travelMatrix.CityId into travelMatrixGrouping
                    from travelMatrix in travelMatrixGrouping.DefaultIfEmpty()
                    //where travelMatrix.CountyId == null && travelMatrix.CountyId == 0
                    orderby areaDetails.AreaCountryName, areaDetails.AreaProvinceName, areaDetails.AreaCityName
                    select new TravelMatrixSearch
                    {
                        CountryId = areaDetails.CountryId,
                        ProvinceId = areaDetails.ProvinceId,
                        CityId = areaDetails.CityId,
                        Id = travelMatrix.Id,
                        CustomerId = travelMatrix.CustomerId,
                        AirCost = travelMatrix.AirCost,
                        BusCost = travelMatrix.BusCost,
                        HotelCost = travelMatrix.HotelCost,
                        TaxiCost = travelMatrix.TaxiCost,
                        TrainCost = travelMatrix.TrainCost,
                        DistanceKM = travelMatrix.DistanceKM,
                        FixExchangeRate = travelMatrix.FixExchangeRate,
                        MarkUpCostAir = travelMatrix.MarkUpCostAir,
                        SourceCurrencyId = travelMatrix.SourceCurrencyId,
                        TravelCurrencyId = travelMatrix.TravelCurrencyId,
                        MarkUpCost = travelMatrix.MarkUpCost,
                        TravelTime = travelMatrix.TravelTime,
                        OtherCost = travelMatrix.OtherCost,
                        InspPortId = travelMatrix.InspPortId,
                        Remarks = travelMatrix.Remarks,
                        TariffTypeId = travelMatrix.TariffTypeId,
                        CustomerName = travelMatrix.CustomerName,
                        CountryName = areaDetails.CountryName,
                        ProvinceName = areaDetails.ProvinceName,
                        CityName = areaDetails.CityName,
                        TariffTypeName = travelMatrix.TariffTypeName,
                        InspPortName = travelMatrix.InspPortName,
                        InspPortCityId = travelMatrix.InspPortCityId,
                        InspPortCityName = travelMatrix.InspPortCityName,
                        SourceCurrencyName = travelMatrix.SourceCurrencyName,
                        TravelCurrencyName = travelMatrix.TravelCurrencyName
                    });
        }

        // get travel matrix details by county, matrix type and source currency
        public async Task<IEnumerable<TravelMatrixSearch>> GetMatrixData(QuotationMatrixRequest matrixRequest)
        {
            if (matrixRequest.MatrixTypeId != (int)TravelMatrixType.Customized)
            {
                return await _context.InvTmDetails.Where(x => x.Active.Value && x.CountyId == matrixRequest.CountyId && x.TravelMatrixTypeId ==
                            matrixRequest.MatrixTypeId && x.SourceCurrencyId == matrixRequest.CurrencyId)
               .Select(x => new TravelMatrixSearch
               {
                   Id = x.Id,
                   CityId = x.CityId,
                   CountryId = x.CountryId,
                   CountyId = x.CountyId,
                   ProvinceId = x.ProvinceId,
                   CustomerId = x.CustomerId,
                   AirCost = x.AirCost,
                   BusCost = x.BusCost,
                   HotelCost = x.HotelCost,
                   TaxiCost = x.TaxiCost,
                   TrainCost = x.TrainCost,
                   DistanceKM = x.DistanceKm,
                   FixExchangeRate = x.FixedExchangeRate,
                   MarkUpCostAir = x.MarkUpAirCost,
                   SourceCurrencyId = x.SourceCurrencyId,
                   TravelCurrencyId = x.TravelCurrencyId,
                   MarkUpCost = x.MarkUpCost,
                   TravelTime = x.TravelTime,
                   OtherCost = x.OtherCost,
                   InspPortId = x.InspPortCountyId,
                   Remarks = x.Remarks,
                   TariffTypeId = x.TravelMatrixTypeId,
                   CustomerName = x.Customer.CustomerName,
                   CountryName = x.Country.CountryName,
                   ProvinceName = x.Province.ProvinceName,
                   CityName = x.City.CityName,
                   CountyName = x.County.CountyName,
                   TariffTypeName = x.TravelMatrixType.Name,
                   InspPortName = x.InspPortCounty.CountyName,
                   SourceCurrencyName = x.SourceCurrency.CurrencyName,
                   TravelCurrencyName = x.TravelCurrency.CurrencyName
               }).ToListAsync();
            }
            else
            {
                return await _context.InvTmDetails.Where(x => x.Active.Value && x.CountyId == matrixRequest.CountyId && x.TravelMatrixTypeId ==
                            matrixRequest.MatrixTypeId && x.SourceCurrencyId == matrixRequest.CurrencyId && x.CustomerId != null && x.CustomerId.Value == matrixRequest.customerId)
               .Select(x => new TravelMatrixSearch
               {
                   Id = x.Id,
                   CityId = x.CityId,
                   CountryId = x.CountryId,
                   CountyId = x.CountyId,
                   ProvinceId = x.ProvinceId,
                   CustomerId = x.CustomerId,
                   AirCost = x.AirCost,
                   BusCost = x.BusCost,
                   HotelCost = x.HotelCost,
                   TaxiCost = x.TaxiCost,
                   TrainCost = x.TrainCost,
                   DistanceKM = x.DistanceKm,
                   FixExchangeRate = x.FixedExchangeRate,
                   MarkUpCostAir = x.MarkUpAirCost,
                   SourceCurrencyId = x.SourceCurrencyId,
                   TravelCurrencyId = x.TravelCurrencyId,
                   MarkUpCost = x.MarkUpCost,
                   TravelTime = x.TravelTime,
                   OtherCost = x.OtherCost,
                   InspPortId = x.InspPortCountyId,
                   Remarks = x.Remarks,
                   TariffTypeId = x.TravelMatrixTypeId,
                   CustomerName = x.Customer.CustomerName,
                   CountryName = x.Country.CountryName,
                   ProvinceName = x.Province.ProvinceName,
                   CityName = x.City.CityName,
                   CountyName = x.County.CountyName,
                   TariffTypeName = x.TravelMatrixType.Name,
                   InspPortName = x.InspPortCounty.CountyName,
                   SourceCurrencyName = x.SourceCurrency.CurrencyName,
                   TravelCurrencyName = x.TravelCurrency.CurrencyName
               }).ToListAsync();
            }
        }

        /// <summary>
        /// Get the travel matrix data
        /// </summary>
        /// <param name="matrixRequest"></param>
        /// <returns></returns>
        public IQueryable<TravelMatrixSearch> GetTravelMatrixData(QuotationMatrixRequest matrixRequest)
        {
            var queryResult = _context.InvTmDetails.Where(x => x.Active.Value && x.SourceCurrencyId == matrixRequest.CurrencyId);

            // matrix type customized then apply customer filter
            if (matrixRequest.MatrixTypeId == (int)TravelMatrixType.Customized)
            {
                queryResult = queryResult.Where(x => x.CustomerId == matrixRequest.customerId);
            }
            else  // matrix type not customized then apply actual type
            {
                queryResult = queryResult.Where(x => x.TravelMatrixTypeId == matrixRequest.MatrixTypeId);
            }

            return queryResult
           .Select(x => new TravelMatrixSearch
           {
               Id = x.Id,
               CityId = x.CityId,
               CountryId = x.CountryId,
               CountyId = x.CountyId,
               ProvinceId = x.ProvinceId,
               CustomerId = x.CustomerId,
               AirCost = x.AirCost,
               BusCost = x.BusCost,
               HotelCost = x.HotelCost,
               TaxiCost = x.TaxiCost,
               TrainCost = x.TrainCost,
               DistanceKM = x.DistanceKm,
               FixExchangeRate = x.FixedExchangeRate,
               MarkUpCostAir = x.MarkUpAirCost,
               SourceCurrencyId = x.SourceCurrencyId,
               TravelCurrencyId = x.TravelCurrencyId,
               MarkUpCost = x.MarkUpCost,
               TravelTime = x.TravelTime,
               OtherCost = x.OtherCost,
               InspPortId = x.InspPortCountyId,
               Remarks = x.Remarks,
               TariffTypeId = x.TravelMatrixTypeId,
               CustomerName = x.Customer.CustomerName,
               CountryName = x.Country.CountryName,
               ProvinceName = x.Province.ProvinceName,
               CityName = x.City.CityName,
               CountyName = x.County.CountyName,
               TariffTypeName = x.TravelMatrixType.Name,
               InspPortName = x.InspPortCounty.CountyName,
               InspPortCityId = x.InspPortCityId,
               InspPortCityName = x.InspPortCity.CityName,
               SourceCurrencyName = x.SourceCurrency.CurrencyName,
               TravelCurrencyName = x.TravelCurrency.CurrencyName
           });
        }
    }
}
