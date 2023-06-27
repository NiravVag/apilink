using BI.Maps;

using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.OfficeLocation;
using Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Entities.Enums;

namespace BI
{
    public class OfficeLocationManager : IOfficeLocationManager
    {
        #region Declaration 
        private IOfficeLocationRepository _officeLocationRepository = null;
        private ILocationRepository _locationRepository = null;
        private ICacheManager _cache = null;
        private ILogger _logger = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly OfficeLocationMap OfficeLocationMap = null;
        private readonly LocationMap LocationMap = null;
        private ITenantProvider _filterService = null;

        #endregion Declaration

        #region Constructor
        public OfficeLocationManager(IOfficeLocationRepository officeLocationRepository, ILocationRepository locationRepository,
            ICacheManager cache, ILogger<OfficeLocationManager> logger, IAPIUserContext applicationContext, ITenantProvider filterService)
        {
            _officeLocationRepository = officeLocationRepository;
            _locationRepository = locationRepository;
            _cache = cache;
            _logger = logger;
            _ApplicationContext = applicationContext;
            OfficeLocationMap = new OfficeLocationMap();
            LocationMap = new LocationMap();
            _filterService = filterService;
        }
        #endregion Constructor

        public async Task<EditOfficeResponse> GetEditOffice(int? id)
        {
            var response = new EditOfficeResponse();
            try
            {
                if (id != null)
                {
                    var data = _officeLocationRepository.GetLocations();
                    response.OfficeDetails = data.Where(x => x.Id == id).Select(OfficeLocationMap.GetLocation).FirstOrDefault();
                    if (response.OfficeDetails == null)
                        return new EditOfficeResponse() { Result = EditOfficeResult.CanNotGetOffice };
                }
                response.OfficeList = _officeLocationRepository.GetLocations().Select(OfficeLocationMap.GetLocation).Where(x => x.Id != id).ToArray();
                if (response.OfficeList == null)
                    return new EditOfficeResponse { Result = EditOfficeResult.CanNotGetOfficeList };                
                response.CountryList = _locationRepository.GetCountryList().Select(LocationMap.GetCountry).ToArray();
                if (response.CountryList == null)
                    return new EditOfficeResponse { Result = EditOfficeResult.CanNotGetCountryList };
                //var citylist = await _locationRepository.GetCityList().ToListAsync();
                //response.CityList = citylist.Select(LocationMap.GetCity);
                //if (response.CityList == null)
                //    return new EditOfficeResponse { Result = EditOfficeResult.CanNotGetCityList };
                response.LocationTypeList = _officeLocationRepository.GetLocationTypeList().Select(OfficeLocationMap.GetLocationType).ToArray();
                if (response.LocationTypeList == null)
                    return new EditOfficeResponse { Result = EditOfficeResult.CannotLocationTypeList };
                response.Result = EditOfficeResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "get edit office");
            }

            return response;
        }


        public async Task<EditOfficeResponse> GetEditOfficeDetails(int officeId)
        {
            var response = new EditOfficeResponse();
            try
            {

                response.OfficeDetails = await _officeLocationRepository.GetLocationDetails(officeId);
                if (response.OfficeDetails == null)
                    return new EditOfficeResponse() { Result = EditOfficeResult.CanNotGetOffice };

                var operationCountries = await _officeLocationRepository.GetRefLocationCountries(officeId);
                response.OfficeDetails.OperationCountries = operationCountries.Select(x => x.CountryId).ToList();

                var headOfficeList = await _officeLocationRepository.GetHeadOfficeList(officeId);
                response.OfficeList = headOfficeList.Select(OfficeLocationMap.GetOfficeMap).ToArray();
                if (response.OfficeList == null)
                    return new EditOfficeResponse { Result = EditOfficeResult.CanNotGetOfficeList };

                response.CountryList = _locationRepository.GetCountryList().Select(LocationMap.GetCountry).ToArray();
                if (response.CountryList == null)
                    return new EditOfficeResponse { Result = EditOfficeResult.CanNotGetCountryList };

                response.CityList = _locationRepository.GetCityByCountryId(response.OfficeDetails.Country.Id).Select(LocationMap.GetCity).ToArray();
                if (response.CityList == null)
                    return new EditOfficeResponse { Result = EditOfficeResult.CanNotGetCityList };

                response.LocationTypeList = _officeLocationRepository.GetLocationTypeList().Select(OfficeLocationMap.GetLocationType).ToArray();
                if (response.LocationTypeList == null)
                    return new EditOfficeResponse { Result = EditOfficeResult.CannotLocationTypeList };
                response.Result = EditOfficeResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "get edit office");
            }

            return response;
        }

        public OfficeSearchResponse GetOfficeSearchSummary(OfficeSearchRequest request)
        {
            var response = new OfficeSearchResponse { Index = request.Index.Value, PageSize = request.pageSize.Value };
            try
            {
                var data = _officeLocationRepository.GetLocations();

                if (request.OfficeValues != null && request.OfficeValues.Any())
                    data = data.Where(x => request.OfficeValues.Any(y => x.Id == y.Id));
                response.TotalCount = data.Count();
                if (response.TotalCount == 0)
                {
                    response.Result = OfficeSearchResult.NotFound;
                    return response;
                }
                int skip = (request.Index.Value - 1) * request.pageSize.Value;
                response.PageCount = (response.TotalCount / request.pageSize.Value) + (response.TotalCount % request.pageSize.Value > 0 ? 1 : 0);
                response.Data = data.Skip(skip).Take(request.pageSize.Value).Select(OfficeLocationMap.GetLocation).ToArray();
                response.Result = OfficeSearchResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "get office search summary");
            }

            return response;
        }

        public IEnumerable<Office> GetLocations()
        {
            // locations
            //var locations = _cache.CacheTryGetValueSet(CacheKeys.AllLocations,
            // () => _officeLocationRepository.GetLocations().ToArray());

            var locations = _officeLocationRepository.GetLocations().ToArray();
            if (locations == null || !locations.Any())
                return null;

            return locations.Select(OfficeLocationMap.GetLocation);
        }

        public OfficeSummaryResponse GetOfficeSummary()
        {
            var response = new OfficeSummaryResponse();
            try
            {
                response.officeList = GetLocations();
                if (response.officeList == null)
                    return new OfficeSummaryResponse { Result = OfficeSummaryResult.CannotGetOfficeList };
                response.Result = OfficeSummaryResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "get office summary");
            }
            return response;
        }

        public async Task<SaveOfficeResponse> SaveOffice(Office request)
        {
            var response = new SaveOfficeResponse();
            try
            {
                if (request.Id == 0)
                {
                    response = await SaveOfficeLocation(request);
                }
                else
                {
                    response = await EditOffice(request);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "save office");
            }
            return response;
        }

        private async Task<SaveOfficeResponse> SaveOfficeLocation(Office request)
        {
            try
            {
                RefLocation entity = OfficeLocationMap.MapOfficeEntity(request, _filterService.GetCompanyId());

                if (entity == null)
                    return new SaveOfficeResponse() { Result = SaveOfficeResult.CannotMapRequestToEntites };

                int id = await _officeLocationRepository.SaveNewOffice(entity);

                if (id != 0)
                    return new SaveOfficeResponse() { OfficeId = id, Result = SaveOfficeResult.Success };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "save office location");
            }
            return new SaveOfficeResponse() { Result = SaveOfficeResult.CannotSaveOffice };
        }

        private async Task<SaveOfficeResponse> EditOffice(Office request)
        {
            var entity = _officeLocationRepository.GetOfficeDetails(request.Id);
            try
            {
                if (entity == null)
                    return new SaveOfficeResponse() { Result = SaveOfficeResult.CurrentOfficeNotFound };
                //OfficeLocationMap.MapUpdateOfficeEntity(entity, request);
                #region mapping
                entity.LocationName = request.Name;
                entity.OfficeCode = request.OfficeCode;
                entity.Active = true;
                entity.Fax = request.Fax ?? null;
                entity.Tel = request.Tel ?? null;
                entity.ZipCode = request.ZipCode ?? null;
                entity.Address = request.Address;
                entity.Address2 = request.Address2;
                entity.Email = request.Email ?? null;
                entity.CityId = request.CityId;
                entity.LocationTypeId = request.LocationTypeId;
                entity.HeadOffice = request.HeadOffice;
                entity.Comment = request.Comment ?? null;

                var RefLocationCountries = entity.RefLocationCountries.ToList();
                foreach (var item in RefLocationCountries)
                    entity.RefLocationCountries.Remove(item);

                if (RefLocationCountries.Count > 0)
                    _officeLocationRepository.RemoveEntities(RefLocationCountries);

                foreach (var element in request.OperationCountries)
                {
                    RefLocationCountry newOperationCountries = new RefLocationCountry();
                    newOperationCountries.LocationId = request.Id;
                    newOperationCountries.CountryId = element;
                    entity.RefLocationCountries.Add(newOperationCountries);
                }

                #endregion
                int re = await _officeLocationRepository.SaveEditOffice(entity);
                if (re > 0)
                    return new SaveOfficeResponse { OfficeId = entity.Id, Result = SaveOfficeResult.Success };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "edit office location");
            }
            return new SaveOfficeResponse { OfficeId = entity.Id, Result = SaveOfficeResult.CannotSaveOffice };

        }

        public IEnumerable<Office> GetOffices()
        {
            //var offices = _cache.CacheTryGetValueSet(CacheKeys.AllLocations,
            //() => _officeLocationRepository.GetLocations().ToArray());
            var offices = _officeLocationRepository.GetLocations().ToArray();
            if (offices == null || !offices.Any())
                return null;

            //if (_ApplicationContext.LocationList != null && _ApplicationContext.LocationList.Any())
            //    offices = offices.Where(x => _ApplicationContext.LocationList.Contains(x.Id)).ToArray();

            return offices.Select(LocationMap.GetOffice);

        }
        public async Task<IEnumerable<Office>> GetOfficesAsync()
        {

            var offices = await _officeLocationRepository.GetLocationsAsync();
            if (offices == null || !offices.Any())
                return null;
            return offices.Select(LocationMap.GetOffice);

        }
        public IEnumerable<Office> GetAllOffices()
        {
            var offices = _officeLocationRepository.GetLocations().ToArray();
            if (offices == null || !offices.Any())
                return null;

            return offices.Select(LocationMap.GetOffice);
        }

        public IEnumerable<Office> GetOfficesByUserId(int UserId)
        {

            var offices = _officeLocationRepository.GetLocationsByUserId(UserId);

            if (offices == null || !offices.Any())
                return null;

            return offices.Select(LocationMap.GetOffice);
        }

        /// <summary>
        /// Get office id by user 
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<int>> GetOnlyOfficeIdsByUser(int UserId)
        {
            return await _officeLocationRepository.GetOfficeIdsbyUser(UserId);
        }


        public async Task<IEnumerable<Office>> GetOfficesByUserIdAsync(int UserId)
        {

            var offices = await _officeLocationRepository.GetLocationsByUserIdAsync(UserId);

            if (offices == null || !offices.Any())
                return null;

            return offices.Select(LocationMap.GetOffice);
        }
        public OfficeResponse GetofficeforInternalUser()
        {
            var _cusofficelist = GetOfficesByUserId(_ApplicationContext.StaffId);
            var officelist = _cusofficelist == null || _cusofficelist.Count() == 0 ? GetAllOffices() : _cusofficelist;
            return new OfficeResponse() { Offices = officelist, Result = OfficeResponseResult.success };
        }
        public async Task<Office> GetOfficeByFactoryid(int FactoryId)
        {
            var data = await _officeLocationRepository.GetOfficeByFactId(FactoryId);
            if (data == null)
                return null;
            return LocationMap.GetOffice(data);
        }

        public async Task<RefLocation> GetOfficeByFactoryidInspection(int FactoryId)
        {
            var data = await _officeLocationRepository.GetOfficeByFactId(FactoryId);
            if (data == null)
                return null;
            return data;
        }

        //Get location list
        public async Task<DataSourceResponse> GetLocationList()
        {
            var data = await _officeLocationRepository.GetLocationList();
            if (data == null)
                return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };

            return new DataSourceResponse()
            {
                Result = DataSourceResult.Success,
                DataSourceList =
                 data.Select(OfficeLocationMap.LocationMap).ToArray(),
            };
        }

        /// <summary>
        /// get offce list by office asses(login user)
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetOfficesByOfficeAccess(CommonDataSourceRequest request)
        {
            var data = _officeLocationRepository.GetOfficesByOfficeAccess();

            //location list filter
            if (_ApplicationContext.LocationList != null && _ApplicationContext.LocationList.Any())
            {
                data = data.Where(x => _ApplicationContext.LocationList.Contains(x.Id));
            }

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                data = data.Where(x => x.Name != null && EF.Functions.Like(x.Name, $"%{request.SearchText.Trim()}%"));
            }

            if (request.Id > 0)
            {
                data = data.Where(x => x.Id == request.Id);
            }
            if (request.IdList != null && request.IdList.Any())
            {
                data = data.Where(x => request.IdList.Contains(x.Id));
            }

            //execute the data
            var officeList = await data.Skip(request.Skip).Take(request.Take).ToListAsync();

            if (officeList == null || !officeList.Any())
                return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };

            return new DataSourceResponse()
            {
                Result = DataSourceResult.Success,
                DataSourceList = officeList
            };
        }



        /// <summary>
        /// Get the office location list
        /// </summary>
        /// <returns></returns>
        public async Task<DataSourceResponse> GetOfficeLocationList()
        {
            List<CommonDataSource> officeLocationList = null;
            //if usertype is internaluser
            if (UserTypeEnum.InternalUser == _ApplicationContext.UserType)
            {
                officeLocationList = await _officeLocationRepository.GetLocationsByStaffId(_ApplicationContext.StaffId);
                if (officeLocationList != null && officeLocationList.Any())
                    return new DataSourceResponse() { DataSourceList = officeLocationList, Result = DataSourceResult.Success };
                else
                {
                    officeLocationList = await _officeLocationRepository.GetOfficesList();
                    if (officeLocationList != null && officeLocationList.Any())
                        return new DataSourceResponse() { DataSourceList = officeLocationList, Result = DataSourceResult.Success };
                }
            }
            //if usertype is other than internal user
            else
            {
                officeLocationList = await _officeLocationRepository.GetOfficesList();
                if (officeLocationList != null && officeLocationList.Any())
                    return new DataSourceResponse() { DataSourceList = officeLocationList, Result = DataSourceResult.Success };
            }
            return new DataSourceResponse() { Result = DataSourceResult.CannotGetList };
        }


    }
}
