using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Managers;
using DTO.CommonClass;
using DTO.Location;
using LINK_UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LINK_UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ApiUserPolicy")]
    public class LocationController : ControllerBase
    {
        private ILocationManager _locationManager = null;

        public LocationController(ILocationManager locationManager)
        {
            _locationManager = locationManager;
        }

        [HttpGet()]
        [Right("country-summary")]
        public CountrySummaryResponse GetCountrySummary()
        {
            return _locationManager.GetCountrySummary();
        }

        [HttpPost("[action]")]
        [Right("country-summary")]
        public CountrySearchResponse GetSearchCountry([FromBody] CountrySearchRequest request)
        {
            if (request.Index == null)
                request.Index = 0;
            if (request.pageSize == null)
                request.pageSize = 10;

            return _locationManager.GetCountrySearchSummary(request);
        }

        [Right("edit-country")]
        [HttpGet("country/edit/{id}")]
        public EditCountryResponse GetEditCountry(int? id)
        {
            return _locationManager.GetEditCountry(id);
        }

        [Right("edit-country")]
        [HttpGet("country/edit")]
        public EditCountryResponse GetEditCountry()
        {
            return _locationManager.GetEditCountry(null);
        }

        [Right("province-summary")]
        [HttpGet("states/{id}")]
        public StatesResponse GetProvinces(int id)
        {
            return _locationManager.GetStates(id);
        }

        [Right("province-summary")]
        [HttpGet("towns/{id}")]
        public async Task<DataSourceResponse> GetTowns(int id)
        {
            return await _locationManager.GetTownsByCountyId(id);
        }

        [HttpPost("[action]")]
        [Right("province-summary")]
        public ProvinceSearchResponse GetProvinceSearch([FromBody] ProvinceSearchRequest request)
        {
            if (request.Index == null)
                request.Index = 0;
            if (request.pageSize == null)
                request.pageSize = 10;

            return _locationManager.GetProvinceSearchSummary(request);
        }

        [HttpGet("province/edit/{id}")]
        [Right("edit-province")]
        public EditProvinceResponse GetEditProvince(int? id)
        {
            return _locationManager.GetEditProvince(id);
        }

        [HttpGet("province/edit")]
        [Right("edit-province")]
        public EditProvinceResponse GetEditProvince()
        {
            return _locationManager.GetEditProvince(null);
        }

        [HttpPost("[action]")]
        [Right("city-summary")]
        public async Task<CitySearchResponse> GetCitySearch([FromBody] CitySearchRequest request)
        {
            if (request.Index == null)
                request.Index = 0;
            if (request.pageSize == null)
                request.pageSize = 10;

            return await _locationManager.GetCitySearchSummary(request);
        }

        [Right("city-summary")]
        [HttpGet("cities/{id}")]
        public CitiesResponse GetCities(int id)
        {
            return _locationManager.GetCities(id);
        }

        [Right("city-summary")]
        [HttpGet("country/cities/{id}")]
        public CitiesResponse GetCitiesByCountry(int id)
        {
            return _locationManager.GetCitiesByCountry(id);
        }

        [HttpGet("city/edit/{id}")]
        [Right("edit-city")]
        public EditCityResponse GetEditPCity(int? id)
        {
            return _locationManager.GetEditCity(id);
        }

        [HttpGet("city/edit")]
        [Right("edit-city")]
        public EditCityResponse GetEditCity()
        {
            return _locationManager.GetEditCity(null);
        }

        [Right("edit-country")]
        [HttpPost("country/save")]
        public SaveCountryResponse SaveCountry([FromBody] Country request)
        {
            return _locationManager.SaveCountry(request);
        }

        [Right("edit-province")]
        [HttpPost("province/save")]
        public SaveProvinceResponse SaveProvince([FromBody] State request)
        {
            return _locationManager.SaveProvince(request);
        }

        [Right("edit-city")]
        [HttpPost("city/save")]
        public SaveCityResponse SaveCity([FromBody] City request)
        {
            return _locationManager.SaveCity(request);
        }

        [Right("county-summary")]
        [HttpGet("county/countries/{id}")]
        public CountrySummaryResponse GetCountriesForCounty(int id)
        {
            return _locationManager.GetCountriesForCounty(id);

        }

        [Right("edit-county")]
        [HttpPost("county/save")]
        public async Task<SaveCountyResponse> SaveCounty([FromBody] County request)
        {
            return await _locationManager.SaveCounty(request);
        }

        [Right("county-summary")]
        [HttpGet("cities/counties/{id}")]
        public async Task<CountiesResponse> GetCountiesByCity(int id)
        {
            return await _locationManager.GetCountiesByCity(id);
        }

        [HttpPost("[action]")]
        [Right("county-summary")]
        public async Task<CountySearchResponse> GetCountySearch([FromBody] CountySearchRequest request)
        {
            if (request.Index == null)
                request.Index = 0;
            if (request.pageSize == null)
                request.pageSize = 10;

            return await _locationManager.GetCountySearch(request);
        }

        [HttpGet("county/edit/{id}")]
        [Right("edit-county")]
        public async Task<EditCountyResponse> GetEditCounty(int? id)
        {
            return await _locationManager.GetEditCounty(id);

        }

        [HttpDelete("county/delete/{id}")]
        [Right("county-summary")]
        public async Task<SaveCountyResponse> DeleteCounty(int? id)
        {
            return await _locationManager.DeleteCounty(id);
        }

        [HttpGet("town/edit/{id}")]
        [Right("edit-town")]
        public async Task<EditTownResponse> GetEditTown(int? id)
        {
            return await _locationManager.GetEditTown(id);

        }

        [Right("edit-town")]
        [HttpPost("town/save")]
        public async Task<SaveTownResponse> SaveTown([FromBody] Town request)
        {
            return await _locationManager.SaveTown(request);
        }

        [HttpPost("town/search")]
        [Right("town-summary")]
        public async Task<TownSearchResponse> GetTownSearch([FromBody] TownSearchRequest request)
        {
            if (request.Index == null)
                request.Index = 0;
            if (request.pageSize == null)
                request.pageSize = 10;

            return await _locationManager.GetTownSearch(request);
        }

        [Right("town-summary")]
        [HttpGet("county/town/{id}")]
        public async Task<TownSearchResponse> GetTownByCounty(int id)
        {
            return await _locationManager.GetTownByCounty(id);
        }

        [HttpDelete("town/delete/{id}")]
        [Right("town-summary")]
        public async Task<SaveTownResponse> DeleteTown(int? id)
        {
            return await _locationManager.DeleteTown(id);
        }

        [Right("Country")]
        [HttpGet("GetCountries/{name}")]
        public async Task<DataSourceResponse> GetCountries(string name)
        {
            return await _locationManager.GetCountries(name);
        }

        [HttpPost("getcountrydatasource")]
        [Right("Country")]
        public async Task<DataSourceResponse> GetCountryDataSource(CommonCountrySourceRequest request)
        {
            return await _locationManager.GetCountryDataSource(request);
        }

        [HttpPost("getOfficeCountryDatasource")]
        [Right("Country")]
        public async Task<DataSourceResponse> GetOfficeCountryDataSource(CommonCountrySourceRequest request)
        {
            return await _locationManager.GetOfficeCountryDataSource(request);
        }

        [Right("Country")]
        [HttpPost("GetCountriesByOffice")]
        public async Task<ParentDataSourceResponse> GetCountriesByOffice(List<int> officeIdList)
        {
            return await _locationManager.GetCountriesByOffice(officeIdList);
        }
        [HttpPost("GetZoneDatasource")]
        public async Task<DataSourceResponse> GetZoneDataSource(CommonZoneSourceRequest request)
        {
            return await _locationManager.GetZoneDataSource(request);
        }
        [HttpPost("GetOfficeZoneDatasource")]
        public async Task<DataSourceResponse> GetZoneByOfficeDataSource(CommonOfficeZoneSourceRequest request)
        {
            return await _locationManager.GetZoneOfficeDataSource(request);
        }
        [HttpPost("GetCountyByCityDatasource")]
        public async Task<DataSourceResponse> GetCountyByCityDatasource(CommonCountyByCitySourceRequest request)
        {
            return await _locationManager.GetCountyByCityDatasource(request);
        }

        [HttpPost("getprovincedatasource")]
        public async Task<DataSourceResponse> GetProvinceDataSource(CommonProvinceSourceRequest request)
        {
            return await _locationManager.GetProvinceDataSource(request);
        }

        [HttpPost("getcitydatasource")]
        public async Task<DataSourceResponse> GetCityDataSource(CommonCitySourceRequest request)
        {
            return await _locationManager.GetCityDataSource(request);
        }

        [HttpPost("GetTownDataSource")]
        public async Task<DataSourceResponse> GetTownDataSource(CommonTownSourceRequest request)
        {
            return await _locationManager.GetTownDataSource(request);
        }

        [HttpPost("GetStartPortDataSource")]
        public async Task<DataSourceResponse> GetStartPortDataSource(StartPortSourceRequest request)
        {
            return await _locationManager.GetStartPortDataSource(request);
        }

        [HttpPost("GetTripTypeDataSource")]
        public async Task<DataSourceResponse> GetTripTypeDataSource(StartPortSourceRequest request)
        {
            return await _locationManager.GetStartPortDataSource(request);
        }

        [Right("Country")]
        [HttpPost("get-province-by-countryids")]
        public async Task<ProvinceListResponse> GetProvinceByCountryIds(ProvinceListSearchRequest request)
        {
            return await _locationManager.GetProvinceByCountryIds(request);
        }

        [Right("Country")]
        [HttpPost("get-city-by-provinceids")]
        public async Task<CityListResponse> GetCityByProvinceIds(CityListSearchRequest request)
        {
            return await _locationManager.GetCityByProvinceIds(request);
        }

        [HttpPost("getcitylistbysearch")]
        public async Task<CitySearchResponse> GetCityListBySearch(CommonCitySourceRequest request)
        {
            return await _locationManager.GetCityListBySearch(request);
        }
    }
}
