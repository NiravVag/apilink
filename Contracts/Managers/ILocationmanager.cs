using DTO.HumanResource;
using DTO.ExchangeRate;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DTO.Location;
using DTO.CommonClass;

namespace Contracts.Managers
{
    public interface ILocationManager
    {
        /// <summary>
        /// Get states by country
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        StatesResponse GetStates(int countryId);

        /// <summary>
        /// Get cities by state
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        CitiesResponse GetCities(int stateId);

        /// <summary>
        /// Get cities byu term
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        Task<CitiesResponse> GetCitiesByTerm(string term);

        /// <summary>
        /// Get cities by Country
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        CitiesResponse GetCitiesByCountry(int countryId);


        /// <summary>
        /// Get all countries
        /// </summary>
        /// <returns></returns>
        IEnumerable<Country> GetCountries();


        /// <summary>
        /// Get country 
        /// </summary>
        /// <returns></returns>
        CountrySummaryResponse GetCountrySummary();

        /// <summary>
        /// Get search country  
        /// </summary>
        /// <returns></returns>
        CountrySearchResponse GetCountrySearchSummary(CountrySearchRequest reason);

        /// <summary>
        /// Edit country response 
        /// </summary>
        /// <returns></returns>
        EditCountryResponse GetEditCountry(int? id);

        /// <summary>
        /// Get Search province
        /// </summary>
        /// <returns></returns>
        ProvinceSearchResponse GetProvinceSearchSummary(ProvinceSearchRequest request);

        /// <summary>
        /// Edit province response 
        /// </summary>
        /// <returns></returns>
        EditProvinceResponse GetEditProvince(int? id);

        /// <summary>
        /// Get Search City
        /// </summary>
        /// <returns></returns>
        Task<CitySearchResponse> GetCitySearchSummary(CitySearchRequest request);

        /// <summary>
        /// Edit city response 
        /// </summary>
        /// <returns></returns>
        EditCityResponse GetEditCity(int? id);

        /// <summary>
        /// Save Country
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        SaveCountryResponse SaveCountry(Country country);

        /// <summary>
        /// Save Province
        /// </summary>
        /// <param name="provinse"></param>
        /// <returns></returns>
        SaveProvinceResponse SaveProvince(State provinse);

        /// <summary>
        /// Save City
        /// </summary>
        /// <param name="city"></param>
        /// <returns>SaveCityResponse</returns>
        SaveCityResponse SaveCity(City city);

        /// <summary>
        /// Get country for County
        /// </summary>
        /// <returns></returns>
        CountrySummaryResponse GetCountriesForCounty(int id);

        /// <summary>
        /// Save County
        /// </summary>
        /// <param name="county"></param>
        /// <returns>SaveCityResponse</returns>
        Task<SaveCountyResponse> SaveCounty(County county);

        /// <summary>
        /// Return County
        /// </summary>
        /// <param name="city"></param>
        /// <returns>SaveCityResponse</returns>
        Task<CountiesResponse> GetCountiesByCity(int cityId);

        /// <summary>
        /// Get Search county
        /// </summary>
        /// <returns></returns>
        Task<CountySearchResponse> GetCountySearch(CountySearchRequest request);

        /// <summary>
        /// Edit county response 
        /// </summary>
        /// <returns></returns>
        Task<EditCountyResponse> GetEditCounty(int? id);

        /// <summary>
        /// Delete county 
        /// </summary>
        /// <returns></returns>
        Task<SaveCountyResponse> DeleteCounty(int? id);

        /// <summary>
        /// Edit town response 
        /// </summary>
        /// <returns></returns>
        Task<EditTownResponse> GetEditTown(int? id);

        /// <summary>
        /// Save Town
        /// </summary>
        /// <param name="county"></param>
        /// <returns>SaveCityResponse</returns>
        Task<SaveTownResponse> SaveTown(Town county);

        /// <summary>
        /// Search town
        /// </summary>
        /// <returns></returns>
        Task<TownSearchResponse> GetTownSearch(TownSearchRequest request);

        /// <summary>
        /// Get Town by county
        /// </summary>
        /// <param name="county"></param>
        /// <returns>SaveCityResponse</returns>
        Task<TownSearchResponse> GetTownByCounty(int countyId);

        /// <summary>
        /// Delete Town  
        /// </summary>
        /// <returns></returns>
        Task<SaveTownResponse> DeleteTown(int? id);

        /// <summary>
        /// Get Countries
        /// </summary>
        /// /// <param name="countryname"></param>
        /// <returns></returns>
        Task<DataSourceResponse> GetCountries(string name);

        /// <summary>
        /// Get Countries by name
        /// </summary>
        /// /// <param name="request"></param>
        /// <returns></returns>
        Task<DataSourceResponse> GetCountryDataSource(CommonCountrySourceRequest request);

        /// <summary>
        /// Get Countries by Office Ids
        /// </summary>
        /// /// <param name="request"></param>
        /// <returns></returns>
        Task<ParentDataSourceResponse> GetCountriesByOffice(List<int> officeIdList);

        /// <summary>
        /// Get zones by active
        /// </summary>
        /// /// <param name="request"></param>
        Task<DataSourceResponse> GetZoneDataSource(CommonZoneSourceRequest request);
        /// <summary>
        /// Get zones by office
        /// </summary>
        /// /// <param name="request"></param>
        Task<DataSourceResponse> GetZoneOfficeDataSource(CommonOfficeZoneSourceRequest request);
        
        /// <summary>
        /// Get county by City
        /// </summary>
        /// /// <param name="request"></param>
        Task<DataSourceResponse> GetCountyByCityDatasource(CommonCountyByCitySourceRequest request);

        /// <summary>
        /// get province list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DataSourceResponse> GetProvinceDataSource(CommonProvinceSourceRequest request);

        /// <summary>
        /// Get the city list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DataSourceResponse> GetCityDataSource(CommonCitySourceRequest request);

        /// <summary>
        /// get office id by city id
        /// </summary>
        /// <returns></returns>
        Task<int?> GetOfficeIdByCityId(int cityid);

        Task<DataSourceResponse> GetTownDataSource(CommonTownSourceRequest request);

        Task<DataSourceResponse> GetTownsByCountyId(int countryId);

        Task<DataSourceResponse> GetStartPortDataSource(StartPortSourceRequest request);
        Task<DataSourceResponse> GetOfficeCountryDataSource(CommonCountrySourceRequest request);

        Task<ProvinceListResponse> GetProvinceByCountryIds(ProvinceListSearchRequest request);

        Task<CityListResponse> GetCityByProvinceIds(CityListSearchRequest request);
        Task<CitySearchResponse> GetCityListBySearch(CommonCitySourceRequest request);
    }
}
