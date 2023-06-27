using DTO.CommonClass;
using DTO.Location;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface ILocationRepository : IRepository
    {
        /// <summary>
        /// Get all Area
        /// </summary>
        /// <returns></returns>
        IEnumerable<RefArea> GetAreaList();
        /// <summary>
        /// Get all countries
        /// </summary>
        /// <returns></returns>
        IEnumerable<RefCountry> GetCountryList();

        /// <summary>
        /// Get all provinces
        /// </summary>
        /// <returns></returns>
        IEnumerable<RefProvince> GetProvinceList();

        /// <summary>
        /// Get all city
        /// </summary>
        /// <returns></returns>
        IQueryable<RefCity> GetCityList();


        /// <summary>
        /// Get cities by term
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        Task<IEnumerable<RefCity>> GetCitiesByTerm(string term);

        /// <summary>
        /// Get city details by id
        /// </summary>
        /// <param name="cityid"></param>
        /// <returns></returns>
        RefCity GetCityDetails(int cityid);

        /// <summary>
        /// Get cities By states Id
        /// </summary>
        /// <returns></returns>
        IEnumerable<RefCity> GetCitiesByStateId(int stateId);

        /// <summary>
        /// Get provinces by country id
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        IEnumerable<RefProvince> GetProvincesByCountryId(int countryId);

        /// <summary>
        /// Get city by country id
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        IEnumerable<RefCity> GetCityByCountryId(int countryId);

        /// <summary>
        /// Get zone
        /// </summary>
        IEnumerable<RefZone> GetZone();

        /// <summary>
        /// Save New Country
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>id</returns>
        int SaveNewCountry(RefCountry entity);

        /// <summary>
        /// Save Edit Country
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        void SaveEditCountry(RefCountry entity);

        /// <summary>
        /// Get CountryDetails By Id
        /// </summary>
        /// <param name="country id"></param>
        /// <returns>RefCountry</returns>
        RefCountry GetCountryDetails(int id);

        /// <summary>
        /// Save New Province
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>id</returns>
        int SaveNewProvince(RefProvince entity);

        /// <summary>
        /// Save Edit Province
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        void SaveEditProvince(RefProvince entity);

        /// <summary>
        /// Get Province Details By Id
        /// </summary>
        /// <param name="province id"></param>
        /// <returns></returns>
        RefProvince GetProvinceDetails(int id);

        /// <summary>
        /// Save New City
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>id</returns>
        int SaveNewCity(RefCity entity);

        /// <summary>
        /// Save Edit City
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        void SaveEditCity(RefCity entity);

        /// <summary>
        /// Save New County
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>id</returns>
        Task<int> SaveNewCounty(RefCounty entity);

        /// <summary>
        /// Get County Details By Id
        /// </summary>
        /// <param name="county id"></param>
        /// <returns></returns>
        Task<RefCounty> GetCountyDetails(int id);

        /// <summary>
        /// Save Edit County
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task SaveEditCounty(RefCounty entity);

        /// <summary>
        /// Get county by city id
        /// </summary>
        /// <param name="countryId"></param>
        /// <returns></returns>
        Task<IEnumerable<RefCounty>> GetCountiesByCity(int cityId);

        /// <summary>
        /// Get all counties
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<RefCounty>> GetCountyList();

        /// <summary>
        /// Delete County
        /// </summary>
        /// <returns></returns>
        Task DeleteCounty(RefCounty entity);

        /// <summary>
        /// Get all towns
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<RefTown>> GetTownList();

        /// <summary>
        /// Save New Town
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>id</returns>
        Task<int> SaveNewTown(RefTown entity);

        /// <summary>
        /// Get Town Details By Id
        /// </summary>
        /// <param name="county id"></param>
        /// <returns></returns>
        Task<RefTown> GetTownDetails(int id);

        /// <summary>
        /// Save Edit Town
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task SaveEditTown(RefTown entity);

        /// <summary>
        /// Get town by city id
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        Task<IEnumerable<RefTown>> GetTownsByCounty(int cityId);


        /// <summary>
        /// Delete Town
        /// </summary>
        /// <returns></returns>
        Task DeleteTown(RefTown entity);

        Task<RefCity> GetCities(string cityName);

        Task<RefCountry> GetCountries(string countryName);

        /// <summary>
        /// Get Countries List
        /// </summary>
        /// <param name="countryName"></param>
        /// <returns></returns>
        Task<List<CommonDataSource>> GetCountriesByName(string countryName);

        /// <summary>
        /// Get Countries List by Idds
        /// </summary>
        /// <param name="countryIds"></param>
        /// <returns></returns>
        Task<List<CommonDataSource>> GetCountryByIds(List<int> countryIds);

        /// <summary>
        /// Get Countries 
        /// </summary>
        /// <returns></returns>
        IQueryable<RefCountry> GetCountryDataSource();

        /// <summary>
        /// Get Countries by Office
        /// </summary>
        /// <returns></returns>
        Task<List<ParentDataSource>> GetCountriesByOffice(List<int> officeIdList);

        /// <summary>
        /// Get zone by active
        /// </summary>
        /// <returns></returns>
        IQueryable<RefZone> GetZoneDataSource();
        /// <summary>
        /// Get zone by Office
        /// </summary>
        /// <returns></returns>
        IQueryable<RefZone> GetOfficeZoneDataSource(List<int?> officeIdList);
        /// <summary>
        /// Get Countries by Ids
        /// </summary>
        /// <returns></returns>
        Task<List<RefCountry>> GetCountriesByIds(List<int> countryIds);
        /// <summary>
        /// Get Provinces by Ids
        /// </summary>
        /// <returns></returns>
        Task<List<RefProvince>> GetProvinceByIds(List<int> provinceIds);

        /// <summary>
        //get Zone list by ID
        /// </summary>
        /// <returns></returns>
        Task<List<RefZone>> GetZoneByIds(List<int> zoneIdList);

        /// <summary>
        /// Get Province List
        /// </summary>
        /// <returns></returns>
        IQueryable<CommonProvinceDataSource> GetProvinceDataSource();

        /// <summary>
        /// get city list
        /// </summary>
        /// <returns></returns>
        IQueryable<CommonCityDataSource> GetCityDataSource();

        /// <summary>
        /// get office id by city id
        /// </summary>
        /// <returns></returns>
        Task<int?> GetOfficeIdByCityId(int cityid);

        IQueryable<CommonTownDataSource> GetTownDataSource();

       Task<IEnumerable<CommonDataSource>> GetTownsByCountyId(int countyId);

        IQueryable<CommonDataSource> GetStartPortDataSource();
        IQueryable<CommonDataSource> GetOfficeCountryDataSource();

        Task<List<CommonDataSource>> GetProvinceByCountryIds(List<int> countryIds);

        Task<List<CommonDataSource>> GetCityByProvinceIds(List<int> provinceIds);
        Task<RefCountry> GetCountriesByAlpha2Code(string alpha2Code);
        Task<CityDetails> GetCityByName(string term);
        Task<RefCity> GetCityByCityName(string cityName);
        IQueryable<RefCity> GetCityQueryableByCountryId(int countryId);

        Task<List<RefCountry>> GetCountriesByAlpha2CodeList(List<string> alpha2CodeList);
    }
}

