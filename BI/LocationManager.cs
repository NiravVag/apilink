using BI.Cache;
using BI.Maps;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.Location;
using DTO.ExchangeRate;
using Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DTO.CommonClass;
using System;

namespace BI
{
    public class LocationManager : ILocationManager
    {
        #region Declaration 
        private ILocationRepository _locationRepository = null;
        private ICacheManager _cache = null;
        private readonly IOfficeLocationManager _office = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly LocationMap LocationMap = null;
        private readonly ITenantProvider _filterService = null;
        #endregion Declaration

        #region Constructor
        public LocationManager(ILocationRepository locationRepository, ICacheManager cache,
            IOfficeLocationManager office, IAPIUserContext applicationContext, ITenantProvider filterService)
        {
            _locationRepository = locationRepository;
            _cache = cache;
            _office = office;
            _ApplicationContext = applicationContext;
            LocationMap = new LocationMap();
            _filterService = filterService;
        }
        #endregion Constructor

        #region Country   
        public IEnumerable<Country> GetCountries()
        {
            // Countries
            var countries = _cache.CacheTryGetValueSet(CacheKeys.AllCountries,
                        () => _locationRepository.GetCountryList().ToArray());

            if (countries == null || !countries.Any())
                return null;

            return countries.Select(LocationMap.GetCountry);
        }
        public CountrySummaryResponse GetCountrySummary()
        {
            var response = new CountrySummaryResponse();

            response.countryList = GetCountries();
            if (response.countryList == null)
                return new CountrySummaryResponse { Result = CountrySummaryResult.CannotGetCountryList };
            response.Result = CountrySummaryResult.Success;
            return response;
        }

        public CountrySearchResponse GetCountrySearchSummary(CountrySearchRequest request)
        {
            var response = new CountrySearchResponse { Index = request.Index.Value, PageSize = request.pageSize.Value };

            var data = _locationRepository.GetCountryList();

            if (request.CountryValues != null && request.CountryValues.Any())
                data = data.Where(x => request.CountryValues.Any(y => x.Id == y.Id));
            response.TotalCount = data.Count();
            if (response.TotalCount == 0)
            {
                response.Result = CountrySearchResult.NotFound;
                return response;
            }
            int skip = (request.Index.Value - 1) * request.pageSize.Value;
            response.PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0);
            response.Data = data.Skip(skip).Take(request.pageSize.Value).Select(LocationMap.GetCountry).ToArray();
            response.Result = CountrySearchResult.Success;
            return response;
        }

        public EditCountryResponse GetEditCountry(int? id)
        {
            var response = new EditCountryResponse();
            if (id != null)
            {
                var data = _locationRepository.GetCountryList();
                response.CountryDetails = data.Where(x => x.Id == id).Select(LocationMap.GetCountry).FirstOrDefault();
                if (response.CountryDetails == null)
                    return new EditCountryResponse() { Result = EditCountryResult.CanNotGetCountry };
            }
            response.AreaList = _locationRepository.GetAreaList().Select(LocationMap.GetArea).ToArray();
            if (response.AreaList == null)
                return new EditCountryResponse { Result = EditCountryResult.CanNotGetAreaList };
            response.Result = EditCountryResult.Success;
            return response;
        }

        public SaveCountryResponse SaveCountry(Country request)
        {
            var response = new SaveCountryResponse();
            if (request.Id == 0)
            {
                RefCountry entity = LocationMap.MapCountryEntity(request);

                if (entity == null)
                    return new SaveCountryResponse() { Result = SaveCountryResult.CannotMapRequestToEntites };

                response.CountryId = _locationRepository.SaveNewCountry(entity);

                if (response.CountryId == 0)
                    return new SaveCountryResponse() { Result = SaveCountryResult.CannotSaveCountry };

                response.Result = SaveCountryResult.Success;
            }
            else
            {
                var entity = _locationRepository.GetCountryDetails(request.Id);
                if (entity == null)
                    return new SaveCountryResponse() { Result = SaveCountryResult.CurrentCountryNotFound };
                LocationMap.MapUpdateCountryEntity(entity, request);
                _locationRepository.SaveEditCountry(entity);
                response.CountryId = entity.Id;
                response.Result = SaveCountryResult.Success;
            }
            return response;
        }

        public async Task<DataSourceResponse> GetCountries(string name)
        {
            var response = new DataSourceResponse();
            var countryList = await _locationRepository.GetCountriesByName(name);

            if (countryList == null || !countryList.Any())
                return null;

            response.DataSourceList = countryList;
            response.Result = DataSourceResult.Success;
            return response;
        }
        #endregion Country

        #region Province
        public StatesResponse GetStates(int countryId)
        {
            var data = _locationRepository.GetProvincesByCountryId(countryId);

            if (data == null || !data.Any())
                return new StatesResponse { Result = StatesResult.CannotGetStates };

            return new StatesResponse
            {
                Data = data.Select(LocationMap.GetState).ToArray(),
                Result = StatesResult.Success
            };
        }
        public ProvinceSearchResponse GetProvinceSearchSummary(ProvinceSearchRequest request)
        {
            var response = new ProvinceSearchResponse { Index = request.Index, PageSize = request.pageSize };
            var data = _locationRepository.GetProvinceList();

            if (request.CountryValues != null)
            {
                data = data.Where(x => request.CountryValues.Id == x.CountryId);
            }
            if (request.ProvinceValues != null && request.ProvinceValues.Any())
            {
                data = data.Where(x => request.ProvinceValues.Any(y => y.Id == x.Id));
            }
            response.TotalCount = data.Count();
            if (response.TotalCount == 0)
            {
                response.Result = ProvinceSearchResult.NotFound;
                return response;
            }
            int skip = (request.Index.Value - 1) * request.pageSize.Value;
            response.PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0);
            response.Data = data.Skip(skip).Take(request.pageSize.Value).Select(LocationMap.GetState).ToArray();
            response.Result = ProvinceSearchResult.Success;
            return response;
        }

        public EditProvinceResponse GetEditProvince(int? id)
        {
            var response = new EditProvinceResponse();

            if (id != null)
            {
                var data = _locationRepository.GetProvinceList();
                response.ProvinceDetails = data.Where(x => x.Id == id).Select(LocationMap.GetState).FirstOrDefault();
                if (response.ProvinceDetails == null)
                    return new EditProvinceResponse { Result = EditProvinceResult.CanNotGetProvinceDetails };
            }
            response.Countryvalues = GetCountries();
            response.Result = EditProvinceResult.success;
            return response;
        }
        public SaveProvinceResponse SaveProvince(State request)
        {
            var response = new SaveProvinceResponse();
            if (request.Id == 0)
            {
                bool IsProvinseExists = _locationRepository.GetProvincesByCountryId(request.CountryId.Value).Any(x => x.ProvinceName.Trim().ToUpper() == request.Name.Trim().ToUpper());
                if (IsProvinseExists)
                    return new SaveProvinceResponse() { Result = SaveProvinceResult.ProvinceExists };
                RefProvince entity = LocationMap.MapProvinseEntity(request);
                if (entity == null)
                    return new SaveProvinceResponse() { Result = SaveProvinceResult.CannotMapRequestToEntites };
                response.ProvinceId = _locationRepository.SaveNewProvince(entity);
                if (response.ProvinceId == 0)
                    return new SaveProvinceResponse() { Result = SaveProvinceResult.CannotSaveProvince };
                response.Result = SaveProvinceResult.Success;
            }
            else
            {
                var entity = _locationRepository.GetProvinceDetails(request.Id);
                if (entity == null)
                    return new SaveProvinceResponse() { Result = SaveProvinceResult.CurrentProvinceNotFound };
                LocationMap.MapUpdateProvinseEntity(entity, request);
                _locationRepository.SaveEditProvince(entity);
                response.ProvinceId = entity.Id;
                response.Result = SaveProvinceResult.Success;

            }
            return response;
        }
        #endregion Province

        #region City
        public CitiesResponse GetCities(int stateId)
        {
            var data = _locationRepository.GetCitiesByStateId(stateId);

            if (data == null || !data.Any())
                return new CitiesResponse { Result = CitiesResult.CannotGetCities };

            return new CitiesResponse
            {
                Data = data.Select(LocationMap.GetCity).ToArray(),
                Result = CitiesResult.Success
            };
        }

        public async Task<CitiesResponse> GetCitiesByTerm(string term)
        {
            var data = await _locationRepository.GetCitiesByTerm(term);

            if (data == null || !data.Any())
                return new CitiesResponse { Result = CitiesResult.CannotGetCities };

            return new CitiesResponse
            {
                Data = data.Select(LocationMap.GetCity).ToArray(),
                Result = CitiesResult.Success
            };
        }



        public IEnumerable<Zone> GetZone()
        {
            var data = _locationRepository.GetZone();

            if (data == null || !data.Any())
                return null;

            return data.Select(LocationMap.GetZone);
        }

        public CitiesResponse GetCitiesByCountry(int countryId)
        {
            var data = _locationRepository.GetCityByCountryId(countryId);

            if (data == null || !data.Any())
                return new CitiesResponse { Result = CitiesResult.CannotGetCities };

            return new CitiesResponse
            {
                Data = data.Select(LocationMap.GetCity).ToArray(),
                Result = CitiesResult.Success
            };
        }


        public async Task<CitySearchResponse> GetCitySearchSummary(CitySearchRequest request)
        {
            var response = new CitySearchResponse() { Index = request.Index, PageSize = request.pageSize };
            var data = _locationRepository.GetCityList();
            if (request.Countryvalues != null && request.Countryvalues != 0)
            {
                data = data.Where(x => x.Province.Country.Id == request.Countryvalues.Value);
            }
            if (request.provinceValues != null)
            {
                data = data.Where(x => x.ProvinceId == request.provinceValues.Id);
            }
            if (request.Cityvalues != null && request.Cityvalues.Count() > 0)
            {
                data = data.Where(x => request.Cityvalues.Select(y => y.Id).Contains(x.Id));
            }
            response.TotalCount = await data.CountAsync();
            if (response.TotalCount == 0)
            {
                return new CitySearchResponse() { Result = CitySearchresult.NotFound };
            }
            var skip = (request.Index.Value - 1) * request.pageSize.Value;
            response.PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % response.PageSize.Value > 0 ? 1 : 0);
            var items = await data.Skip(skip).Take(request.pageSize.Value).ToListAsync();

            response.Data = items.Select(LocationMap.GetCityDetails).OrderBy(x => x.CountryName).ThenBy(x => x.ProvinceName).ThenBy(x => x.Name);

            response.Result = CitySearchresult.success;
            return response;
        }

        public EditCityResponse GetEditCity(int? id)
        {
            var response = new EditCityResponse();

            if (id != null)
            {
                response.CityDetails = LocationMap.GetCityDetails(_locationRepository.GetCityDetails(id.Value));
                if (response.CityDetails == null)
                    return new EditCityResponse() { Result = EditCityResult.CanNotGetCityDetails };
            }

            // get country list
            response.CountryValues = GetCountries();
            if (response.CountryValues == null || !response.CountryValues.Any())
                return new EditCityResponse() { Result = EditCityResult.CanNotGetCountryList };

            //get province
            if (response.CityDetails != null && response.CityDetails.CountryId != null)
            {
                var provinceresponse = GetStates(response.CityDetails.CountryId.Value);
                if (provinceresponse.Result == StatesResult.Success)
                {
                    response.ProvinceValues = provinceresponse.Data;
                }
                else
                    return new EditCityResponse() { Result = EditCityResult.CanNotGetProvinceDetails };

            }

            //OfficeList
            response.OfficeValues = _office.GetOffices();
            if (response.OfficeValues == null || !response.OfficeValues.Any())
                return new EditCityResponse() { Result = EditCityResult.CanNotGetOfficeList };

            //ZoneList
            response.ZoneValues = GetZone();
            if (response.ZoneValues == null || !response.ZoneValues.Any())
                return new EditCityResponse() { Result = EditCityResult.CanNotGetZone };

            response.Result = EditCityResult.success;
            return response;
        }
        public SaveCityResponse SaveCity(City request)
        {
            var response = new SaveCityResponse();
            if (request.Id == 0)
            {
                response = AddCity(request);
            }
            else if (request.Id != 0)
            {
                response = UpdateCity(request);
            }
            return response;
        }
        public SaveCityResponse AddCity(City request)
        {
            var response = new SaveCityResponse();
            bool IsCityExists = _locationRepository.GetCityByCountryId(request.CountryId.Value).Any(x => x.CityName.Trim().ToUpper() == request.Name.Trim().ToUpper());
            if (IsCityExists)
                return new SaveCityResponse() { Result = SaveCityResult.CityExists };
            var city = new RefCity()
            {
                Active = true,
                CityName = request.Name?.Trim(),
                PhCode = request.PhoneCode?.Trim(),
                ProvinceId = request.ProvinceId ?? 0
            };
            _locationRepository.AddEntity(city);
            var citydetails = new RefCityDetail()
            {
                Active = true,
                LocationId = request.OfficeId,
                TravelTime = request.TravelTimeHH,
                ZoneId = request.ZoneId,
                EntityId = _filterService.GetCompanyId()
            };
            city.RefCityDetails.Add(citydetails);
            _locationRepository.AddEntity(citydetails);

            response.CityId = _locationRepository.SaveNewCity(city);

            if (response.CityId == 0)
                return new SaveCityResponse() { Result = SaveCityResult.CannotSaveCity };
            response.Result = SaveCityResult.Success;
            return response;
        }
        public SaveCityResponse UpdateCity(City request)
        {
            var response = new SaveCityResponse();
            var entity = _locationRepository.GetCityDetails(request.Id);
            if (entity == null)
                return new SaveCityResponse() { Result = SaveCityResult.CurrentCityNotFound };

            entity.CityName = request.Name?.Trim();
            entity.Active = true;
            entity.ProvinceId = request.ProvinceId ?? 0;
            entity.PhCode = request.PhoneCode?.Trim();
            var citydetailsentity = entity.RefCityDetails?.FirstOrDefault();
            var lstcitydetails = new List<RefCityDetail>();
            if (citydetailsentity == null)
            {
                var citydetails = new RefCityDetail()
                {
                    Active = true,
                    LocationId = request.OfficeId,
                    TravelTime = request.TravelTimeHH,
                    ZoneId = request.ZoneId,
                    EntityId = _filterService.GetCompanyId()
                };
                entity.RefCityDetails.Add(citydetails);
            }
            else
            {
                citydetailsentity.TravelTime = request.TravelTimeHH;
                citydetailsentity.ZoneId = request.ZoneId;
                citydetailsentity.LocationId = request.OfficeId;
                lstcitydetails.Add(citydetailsentity);
                _locationRepository.EditEntities(lstcitydetails);
            }
            _locationRepository.SaveEditCity(entity);
            response.CityId = entity.Id;
            response.Result = SaveCityResult.Success;
            return response;
        }




        #endregion City

        #region County

        public CountrySummaryResponse GetCountriesForCounty(int id)
        {
            var response = new CountrySummaryResponse();

            response.countryList = GetCountries().Where(x => x.Id == id); ;
            if (response.countryList == null)
                return new CountrySummaryResponse { Result = CountrySummaryResult.CannotGetCountryList };
            response.Result = CountrySummaryResult.Success;
            return response;
        }

        //Returns County based on City - Summary Page
        public async Task<CountiesResponse> GetCountiesByCity(int cityId)
        {
            var countyList = await _locationRepository.GetCountiesByCity(cityId);

            if (countyList == null || !countyList.Any())
                return new CountiesResponse { Result = CountiesResult.CannotGetStates };

            return new CountiesResponse
            {
                Data = countyList.Select(LocationMap.GetCounty).FirstOrDefault(),
                DataList = countyList.Select(LocationMap.GetCounty).ToArray(),
                Result = CountiesResult.Success
            };
        }

        //Search for the specific County
        public async Task<CountySearchResponse> GetCountySearch(CountySearchRequest request)
        {
            var response = new CountySearchResponse { Index = request.Index, PageSize = request.pageSize };
            var countyList = await _locationRepository.GetCountyList();
            //var provinceName = _locationRepository.GetCityByCountryId(request.CountryValues.Id);

            if (request.CountryValues != null)
            {
                var dataCity = _locationRepository.GetCityByCountryId(request.CountryValues.Id);
                countyList = countyList.Join(dataCity, d => d.CityId, c => c.Id, ((d, c) => d));
            }
            if (request.CityValues != null)
            {
                countyList = countyList.Where(x => request.CityValues.Id == x.CityId);
            }
            if (request.CountyValues != null && !string.IsNullOrEmpty(request.CountyValues))//&& request.CountyValues.Any())
            {
                countyList = countyList.Where(x => x.CountyName.ToUpper().Contains(request.CountyValues.ToUpper())); //(y => y.Id == x.Id));
            }
            response.TotalCount = countyList.Count();
            if (response.TotalCount == 0)
            {
                response.Result = CountySearchResult.NotFound;
                return response;
            }

            int skip = (request.Index.Value - 1) * request.pageSize.Value;
            response.PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0);
            response.Data = countyList.Skip(skip).Take(request.pageSize.Value).Select(LocationMap.GetCounty).ToArray();
            response.Result = CountySearchResult.Success;
            return response;
        }

        //Returns County to the Edit-County Page
        public async Task<EditCountyResponse> GetEditCounty(int? id)
        {
            var response = new EditCountyResponse();

            if (id != null && id != 0)
            {
                var countyList = await _locationRepository.GetCountyList();
                response.Countydetails = countyList.Where(x => x.Id == id).Select(LocationMap.GetCounty).FirstOrDefault();
                if (response.Countydetails == null)
                    return new EditCountyResponse { Result = CountyResult.CannotGetCounty };
                response.Cityvalues = GetCities(response.Countydetails.ProvinceId.Value).Data;
                response.Provincevalues = GetStates(response.Countydetails.CountryId.Value).Data;
            }
            response.Countryvalues = GetCountries();
            response.Result = CountyResult.Success;
            return response;
        }

        //Add or modify County values
        public async Task<SaveCountyResponse> SaveCounty(County request)
        {
            var response = new SaveCountyResponse();
            if (request.Id == 0)
            {
                bool IsCountyExists = _locationRepository.GetCountiesByCity(request.CityId.Value).Result.Any(x => x.CountyName.ToUpper() == request.CountyName.ToUpper());
                if (IsCountyExists)
                {
                    return new SaveCountyResponse() { Result = SaveCountyResult.CountyExists };
                }
                RefCounty entity = new RefCounty();
                entity = LocationMap.MapCountyEntity(request);
                entity.CreatedBy = _ApplicationContext.UserId;
                if (entity == null)
                    return new SaveCountyResponse() { Result = SaveCountyResult.CannotMapRequestToEntites };
                response.CountyId = await _locationRepository.SaveNewCounty(entity);
                if (response.CountyId == 0)
                    return new SaveCountyResponse() { Result = SaveCountyResult.CannotSaveCounty };
                response.Result = SaveCountyResult.Success;
            }
            else
            {
                var entity = await _locationRepository.GetCountyDetails(request.Id);
                if (entity == null)
                    return new SaveCountyResponse() { Result = SaveCountyResult.CurrentCountyNotFound };
                entity.ModifiedBy = _ApplicationContext.UserId;
                LocationMap.MapUpdateCountyEntity(entity, request);
                await _locationRepository.SaveEditCounty(entity);
                response.CountyId = entity.Id;
                response.Result = SaveCountyResult.Success;

            }
            return response;
        }

        //Delete a County
        public async Task<SaveCountyResponse> DeleteCounty(int? id)
        {

            var response = new SaveCountyResponse();

            var entity = await _locationRepository.GetCountyDetails(id.Value);
            var entityTown = await _locationRepository.GetTownsByCounty(id.Value);
            foreach (var town in entityTown)
            {
                await DeleteTown(town.Id);
            }
            if (entity == null)
                return new SaveCountyResponse() { Result = SaveCountyResult.CurrentCountyNotFound };
            entity.DeletedBy = _ApplicationContext.UserId;
            LocationMap.MapDeleteCountyEntity(entity);
            await _locationRepository.DeleteCounty(entity);
            response.Result = SaveCountyResult.Success;
            return response;
        }

        #endregion

        #region Town

        public async Task<DataSourceResponse> GetTownsByCountyId(int countryId)
        {
            var data = await _locationRepository.GetTownsByCountyId(countryId);

            if (data == null && !data.Any())
                return new DataSourceResponse { Result = DataSourceResult.CannotGetList };

            return new DataSourceResponse
            {
                DataSourceList = data,
                Result = DataSourceResult.Success
            };
        }

        //Returns Town to the Edit-Town Page
        public async Task<EditTownResponse> GetEditTown(int? id)
        {
            var response = new EditTownResponse();

            if (id != null && id != 0)
            {
                var townList = await _locationRepository.GetTownList();
                response.townDetails = townList.Where(x => x.Id == id).Select(LocationMap.GetTown).FirstOrDefault();

                response.countyValues = GetCountiesByCity(response.townDetails.CityId.Value).Result.DataList;
                response.cityValues = GetCities(response.townDetails.ProvinceId.Value).Data;
                response.provinceValues = GetStates(response.townDetails.CountryId.Value).Data;
                if (response.townDetails == null)
                    return new EditTownResponse { Result = TownResult.CannotGetTown };
            }
            response.countryValues = GetCountries().Where(x => x.Id == (int)Entities.Enums.CountryEnum.China);
            response.Result = TownResult.Success;
            return response;
        }

        //Add or modify Town values
        public async Task<SaveTownResponse> SaveTown(Town request)
        {
            var response = new SaveTownResponse();
            if (request.Id == 0)
            {
                bool IsTownExists = _locationRepository.GetTownsByCounty(request.CountyId.Value).Result.Any(x => x.TownName.ToUpper() == request.TownName.ToUpper());//Any(x => x.TownName.ToUpper() == request.TownName.ToUpper() && x.Active);
                if (IsTownExists)
                {
                    return new SaveTownResponse() { Result = SaveTownResult.TownExists };
                }
                RefTown entity = new RefTown();
                entity = LocationMap.MapTownEntity(request);
                entity.CreatedBy = _ApplicationContext.UserId;
                if (entity == null)
                    return new SaveTownResponse() { Result = SaveTownResult.CannotMapRequestToEntites };
                response.TownId = await _locationRepository.SaveNewTown(entity);
                if (response.TownId == 0)
                    return new SaveTownResponse() { Result = SaveTownResult.CannotSaveTown };
                response.Result = SaveTownResult.Success;
            }
            else
            {
                var entity = await _locationRepository.GetTownDetails(request.Id);
                if (entity == null)
                    return new SaveTownResponse() { Result = SaveTownResult.CurrentTownNotFound };
                entity.ModifiedBy = _ApplicationContext.UserId;
                LocationMap.MapUpdateTownEntity(entity, request);
                await _locationRepository.SaveEditTown(entity);
                response.TownId = entity.Id;
                response.Result = SaveTownResult.Success;

            }
            return response;
        }

        //Search for the specific Town
        public async Task<TownSearchResponse> GetTownSearch(TownSearchRequest request)
        {
            var response = new TownSearchResponse { Index = request.Index, PageSize = request.pageSize };
            var townList = await _locationRepository.GetTownList();
            var countyList = await _locationRepository.GetCountyList();

            if (request.CityValues != null)
            {
                //countyList = countyList.Where(x => request.CityValues.Id == x.CityId);
                //townList = townList.Join(countyList, d => d.CountyId, c => c.Id, ((d, c) => d)); 
                townList = townList.Where(x => x.County.CityId == request.CityValues.Id);
            }

            if (request.CountyValues != null)
            {
                townList = townList.Where(x => request.CountyValues.Id == x.CountyId);
            }
            if (request.TownValues != null && !string.IsNullOrEmpty(request.TownValues))//&& request.CountyValues.Any())
            {
                townList = townList.Where(x => x.TownName.ToUpper().Contains(request.TownValues.ToUpper())); //(y => y.Id == x.Id));
            }
            response.TotalCount = townList.Count();
            if (response.TotalCount == 0)
            {
                response.Result = TownSearchResult.NotFound;
                return response;
            }
            int skip = (request.Index.Value - 1) * request.pageSize.Value;
            response.PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0);
            response.DataList = townList.Skip(skip).Take(request.pageSize.Value).Select(LocationMap.GetTown).ToArray();
            response.Result = TownSearchResult.Success;
            return response;
        }

        //Returns Town based on County - Summary Page
        public async Task<TownSearchResponse> GetTownByCounty(int countyId)
        {
            var townList = await _locationRepository.GetTownsByCounty(countyId);

            if (townList == null || !townList.Any())
                return new TownSearchResponse { Result = TownSearchResult.NotFound };

            return new TownSearchResponse
            {
                Data = townList.Select(LocationMap.GetTown).FirstOrDefault(),
                DataList = townList.Select(LocationMap.GetTown).ToArray(),
                Result = TownSearchResult.Success
            };
        }

        //Delete a Town
        public async Task<SaveTownResponse> DeleteTown(int? id)
        {

            var response = new SaveTownResponse();

            var entity = await _locationRepository.GetTownDetails(id.Value);
            if (entity == null)
                return new SaveTownResponse() { Result = SaveTownResult.CurrentTownNotFound };
            entity.DeletedBy = _ApplicationContext.UserId;
            LocationMap.MapDeleteTownEntity(entity);
            await _locationRepository.DeleteTown(entity);
            response.Result = SaveTownResult.Success;
            return response;
        }

        #endregion

        //get Customer by customer name substring
        public async Task<DataSourceResponse> GetCountryDataSource(CommonCountrySourceRequest request)
        {
            try
            {
                var response = new DataSourceResponse();

                var data = _locationRepository.GetCountryDataSource();

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    data = data.Where(x => x.CountryName != null && EF.Functions.Like(x.CountryName, $"%{request.SearchText.Trim()}%"));
                }

                // filter by country ids
                if (request.CountryIds != null && request.CountryIds.Any())
                {
                    data = data.Where(x => request.CountryIds.Contains(x.Id));
                }

                var countryList = await data.Skip(request.Skip).Take(request.Take).
                    Select(x => new CommonDataSource() { Id = x.Id, Name = x.CountryName })
                    .AsNoTracking().ToListAsync();

                if (countryList == null || !countryList.Any())
                    response.Result = DataSourceResult.CannotGetList;

                else
                {
                    response.DataSourceList = countryList;
                    response.Result = DataSourceResult.Success;
                }
                return response;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<ParentDataSourceResponse> GetCountriesByOffice(List<int> officeIdList)
        {
            var response = new ParentDataSourceResponse();

            try
            {
                var data = await _locationRepository.GetCountriesByOffice(officeIdList);

                if (data == null || !data.Any())
                {
                    response.DataSourceList = null;
                    response.Result = DataSourceResult.CannotGetList;
                    return response;
                }

                response.DataSourceList = data;
                response.Result = DataSourceResult.Success;
            }

            catch (Exception e)
            {
                response.DataSourceList = null;
                response.Result = DataSourceResult.Failed;
            }
            return response;
        }
        public async Task<DataSourceResponse> GetZoneDataSource(CommonZoneSourceRequest request)
        {
            var response = new DataSourceResponse();

            var data = _locationRepository.GetZoneDataSource();

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                data = data.Where(x => x.Name != null && EF.Functions.Like(x.Name, $"%{request.SearchText.Trim()}%"));
            }


            if (request.LocationId > 0)
            {
                data = data.Where(x => x.LocationId == request.LocationId);
            }

            if (request.ZoneId > 0)
            {
                data = data.Where(x => x.Id == request.ZoneId);
            }

            var zoneList = data.Skip(request.Skip).Take(request.Take).ToList();

            if (zoneList == null || !zoneList.Any())
                response.Result = DataSourceResult.CannotGetList;

            else
            {
                response.DataSourceList = zoneList.Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Name
                }).OrderBy(x => x.Name);
                response.Result = DataSourceResult.Success;
            }
            return response;
        }
        //get Zone by Office
        public async Task<DataSourceResponse> GetZoneOfficeDataSource(CommonOfficeZoneSourceRequest request)
        {
            var response = new DataSourceResponse();

            var data = _locationRepository.GetOfficeZoneDataSource(request.Officeids);

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                data = data.Where(x => x.Name != null && EF.Functions.Like(x.Name, $"%{request.SearchText.Trim()}%"));
            }

            if (request.ZoneId > 0)
            {
                data = data.Where(x => x.Id == request.ZoneId);
            }

            var zoneList = await data.Skip(request.Skip).Take(request.Take).ToListAsync();

            if (zoneList == null || !zoneList.Any())
                response.Result = DataSourceResult.CannotGetList;

            else
            {
                response.DataSourceList = zoneList.Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.Name
                }).OrderBy(x => x.Name);
                response.Result = DataSourceResult.Success;
            }
            return response;
        }


        public async Task<DataSourceResponse> GetCountyByCityDatasource(CommonCountyByCitySourceRequest request)
        {
            var response = new DataSourceResponse();

            var data = await _locationRepository.GetCountiesByCity(request.CityId);

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                data = data.Where(x => x.CountyName != null && EF.Functions.Like(x.CountyName, $"%{request.SearchText.Trim()}%"));
            }

            if (request.CountyId > 0)
            {
                data = data.Where(x => x.Id == request.CountyId);
            }

            var zoneList = data.Skip(request.Skip).Take(request.Take).ToList();

            if (zoneList == null || !zoneList.Any())
                response.Result = DataSourceResult.CannotGetList;

            else
            {
                response.DataSourceList = zoneList.Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.CountyName
                }).OrderBy(x => x.Name);
                response.Result = DataSourceResult.Success;
            }
            return response;
        }

        //get province by province name substring
        public async Task<DataSourceResponse> GetProvinceDataSource(CommonProvinceSourceRequest request)
        {
            try
            {
                var response = new DataSourceResponse();

                var data = _locationRepository.GetProvinceDataSource();

                if (request.ProvinceId > 0)
                {
                    data = data.Where(x => x.Id == request.ProvinceId);
                }

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    data = data.Where(x => x.Name != null && EF.Functions.Like(x.Name, $"%{request.SearchText.Trim()}%"));
                }

                // filter by country ids
                if (request.CountryIds != null && request.CountryIds.Any())
                {
                    data = data.Where(x => request.CountryIds.Contains(x.CountryId));
                }

                var provinceList = await data.Skip(request.Skip).Take(request.Take).AsNoTracking().ToListAsync();

                if (provinceList == null || !provinceList.Any())
                    response.Result = DataSourceResult.CannotGetList;

                else
                {
                    response.DataSourceList = provinceList.ConvertAll(x => new CommonDataSource { Id = x.Id, Name = x.Name });
                    response.Result = DataSourceResult.Success;
                }
                return response;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //get City by city name substring
        public async Task<DataSourceResponse> GetCityDataSource(CommonCitySourceRequest request)
        {
            try
            {
                var response = new DataSourceResponse();

                var data = _locationRepository.GetCityDataSource();

                if (request.CityIds != null && request.CityIds.Any())
                {
                    data = data.Where(x => request.CityIds.Contains(x.Id));
                }

                if (request.ProvinceId > 0)
                {
                    data = data.Where(x => x.ProvinceId == request.ProvinceId);
                }

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    data = data.Where(x => x.Name != null && EF.Functions.Like(x.Name, $"%{request.SearchText.Trim()}%"));
                }

                if (request.CountryIds != null && request.CountryIds.Any())
                {
                    data = data.Where(x => request.CountryIds.Contains(x.CountryId));
                }

                var cityList = await data.OrderByDescending(x => x.Priority).Skip(request.Skip).Take(request.Take).AsNoTracking().ToListAsync();

                if (cityList == null || !cityList.Any())
                    response.Result = DataSourceResult.CannotGetList;

                else
                {
                    response.DataSourceList = cityList.ConvertAll(x => new CommonDataSource { Id = x.Id, Name = x.Name });
                    response.Result = DataSourceResult.Success;
                }
                return response;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<DataSourceResponse> GetTownDataSource(CommonTownSourceRequest request)
        {
            try
            {
                var response = new DataSourceResponse();

                var data = _locationRepository.GetTownDataSource();

                if (request.TownIds != null && request.TownIds.Any())
                {
                    data = data.Where(x => request.TownIds.Contains(x.Id));
                }

                if (request.CountyId > 0)
                {
                    data = data.Where(x => x.CountyId == request.CountyId);
                }

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    data = data.Where(x => x.Name != null && EF.Functions.Like(x.Name, $"%{request.SearchText.Trim()}%"));
                }

                if (request.CountryIds != null && request.CountryIds.Any())
                {
                    data = data.Where(x => request.CountryIds.Contains(x.CountryId));
                }

                var townList = await data.Skip(request.Skip).Take(request.Take).AsNoTracking().ToListAsync();

                if (townList == null || !townList.Any())
                    response.Result = DataSourceResult.CannotGetList;

                else
                {
                    response.DataSourceList = townList.ConvertAll(x => new CommonDataSource { Id = x.Id, Name = x.Name });
                    response.Result = DataSourceResult.Success;
                }
                return response;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<DataSourceResponse> GetStartPortDataSource(StartPortSourceRequest request)
        {
            try
            {
                var response = new DataSourceResponse();

                var data = _locationRepository.GetStartPortDataSource();

                if (request.StartPortIds != null && request.StartPortIds.Any())
                {
                    data = data.Where(x => request.StartPortIds.Contains(x.Id));
                }

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    data = data.Where(x => x.Name != null && EF.Functions.Like(x.Name, $"%{request.SearchText.Trim()}%"));
                }

                var startPortList = await data.Skip(request.Skip).Take(request.Take).AsNoTracking().ToListAsync();

                if (startPortList == null || !startPortList.Any())
                {
                    response.Result = DataSourceResult.CannotGetList;
                }

                else
                {
                    response.DataSourceList = startPortList;
                    response.Result = DataSourceResult.Success;
                }
                return response;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<int?> GetOfficeIdByCityId(int cityid)
        {
            return await _locationRepository.GetOfficeIdByCityId(cityid);
        }

        public async Task<DataSourceResponse> GetOfficeCountryDataSource(CommonCountrySourceRequest request)
        {
            try
            {
                var response = new DataSourceResponse();

                var data = _locationRepository.GetOfficeCountryDataSource();

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    data = data.Where(x => x.Name != null && EF.Functions.Like(x.Name, $"%{request.SearchText.Trim()}%"));
                }

                // filter by country ids
                if (request.CountryIds != null && request.CountryIds.Any())
                {
                    data = data.Where(x => request.CountryIds.Contains(x.Id));
                }

                var countryList = await data.Skip(request.Skip).Take(request.Take).ToListAsync();
                //Select(x => new CommonDataSource() { Id = x.City.Province.CountryId, Name = x.City.Province.Country.CountryName })
                //.Distinct().AsNoTracking().ToListAsync();

                if (countryList == null || !countryList.Any())
                    response.Result = DataSourceResult.CannotGetList;

                else
                {
                    response.DataSourceList = countryList;
                    response.Result = DataSourceResult.Success;
                }
                return response;

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Get Province list by country ids
        /// </summary>
        /// <param name="countryIds"></param>
        /// <returns></returns>
        public async Task<ProvinceListResponse> GetProvinceByCountryIds(ProvinceListSearchRequest request)
        {
            var response = new ProvinceListResponse();

            try
            {
                //request empty
                if (request == null)
                    response.Result = ProvinceResult.RequestShouldNotBeEmpty;
                //country search filter empty
                else if (request.CountryIds == null || !request.CountryIds.Any())
                    response.Result = ProvinceResult.CountrySearchFilterEmpty;
                else
                {
                    //get the province data by country ids
                    var data = await _locationRepository.GetProvinceByCountryIds(request.CountryIds);
                    
                    //if data not found
                    if (data == null || !data.Any())
                        response.Result = ProvinceResult.DataNotFound;
                    //assign the data source list
                    else if (data != null && data.Any())
                    {
                        response.DataSourceList = data;
                        response.Result = ProvinceResult.Success;
                    }
                }
            }

            catch (Exception)
            {
                response.DataSourceList = null;
                response.Result = ProvinceResult.Failed;
            }
            return response;
        }

        /// <summary>
        /// Get the city list by province ids
        /// </summary>
        /// <param name="provinceIds"></param>
        /// <returns></returns>
        public async Task<CityListResponse> GetCityByProvinceIds(CityListSearchRequest request)
        {
            var response = new CityListResponse();

            try
            {
                //request empty
                if (request == null)
                    response.Result = CityResult.RequestShouldNotBeEmpty;
                //country search filter empty
                else if (request.ProvinceIds == null || !request.ProvinceIds.Any())
                    response.Result = CityResult.ProvinceSearchFilterEmpty;
                else 
                {
                    //get the city list data
                    var data = await _locationRepository.GetCityByProvinceIds(request.ProvinceIds);

                    //if data not available
                    if (data == null || !data.Any())
                        response.Result = CityResult.DataNotFound;
                    //assign the data
                    else if (data != null && data.Any())
                    {
                        response.DataSourceList = data;
                        response.Result = CityResult.Success;
                    }
                }
            }

            catch (Exception)
            {
                response.DataSourceList = null;
                response.Result = CityResult.Failed;
            }
            return response;
        }

        // Duplicate method but response type is different
        public async Task<CitySearchResponse> GetCityListBySearch(CommonCitySourceRequest request)
        {
            var data = _locationRepository.GetCityList();

            if (request.CityIds != null && request.CityIds.Any())
            {
                data = data.Where(x => request.CityIds.Contains(x.Id));
            }

            if (request.ProvinceId > 0)
            {
                data = data.Where(x => x.ProvinceId == request.ProvinceId);
            }

            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                data = data.Where(x => x.CityName != null && EF.Functions.Like(x.CityName, $"%{request.SearchText.Trim()}%"));
            }

            if (request.CountryIds != null && request.CountryIds.Any())
            {
                data = data.Where(x => request.CountryIds.Contains(x.Province.CountryId));
            }

            var cityList = await data.OrderBy(x => x.CityName).Skip(request.Skip).Take(request.Take).AsNoTracking().ToListAsync();

            if (cityList == null || !cityList.Any())
                return new CitySearchResponse() { Result = CitySearchresult.NotFound };

            return new CitySearchResponse()
            {
                Data = cityList.Select(LocationMap.GetCityDetails),
                Result = CitySearchresult.success
            };
        }
    }
}
