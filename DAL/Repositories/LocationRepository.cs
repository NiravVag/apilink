using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.Location;
using DTO.References;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class LocationRepository : Repository, ILocationRepository
    {

        public LocationRepository(API_DBContext context) : base(context)
        {
        }

        public IEnumerable<RefCountry> GetCountryList()
        {

            return _context.RefCountries.Where(x => x.Active)
            .Include(x => x.Area)
            .OrderByDescending(x => x.Priority)
            .ThenBy(x => x.CountryName);
        }

        public IEnumerable<RefCity> GetCitiesByStateId(int stateId)
        {
            return _context.RefCities.Where(x => x.Active && x.ProvinceId == stateId)
                .OrderBy(x => x.CityName);
        }

        public IEnumerable<RefProvince> GetProvincesByCountryId(int countryId)
        {
            return _context.RefProvinces.Include(x => x.Country)
                .Where(x => x.Active != null && x.Active.Value && x.CountryId == countryId)
                .OrderBy(x => x.Country.CountryName)
                .ThenBy(x => x.ProvinceName);
        }

        public async Task<IEnumerable<CommonDataSource>> GetTownsByCountyId(int countyId)
        {
            return await _context.RefTowns
                .Where(x => x.Active && x.CountyId == countyId)
                .OrderBy(x => x.TownName).Select(x => new CommonDataSource()
                {
                    Id = x.Id,
                    Name = x.TownName
                }).AsNoTracking().ToListAsync();
        }


        public IEnumerable<RefArea> GetAreaList()
        {
            return _context.RefAreas.Where(x => x.Active);
        }

        public IEnumerable<RefProvince> GetProvinceList()
        {
            return _context.RefProvinces
                .Include(x => x.Country)
                .Where(x => x.Active != null && x.Active.Value)
                .OrderBy(x => x.ProvinceName);
        }

        public IQueryable<RefCity> GetCityList()
        {
            return _context.RefCities
                    .Include(x => x.Province)
                    .ThenInclude(x => x.Country)
                    .Include(x => x.RefCityDetails)
                    .ThenInclude(x => x.Location)
                    .Include(x => x.RefCityDetails)
                    .ThenInclude(x => x.Zone)
                    .Where(x => x.Active);
        }
        public IEnumerable<RefCity> GetCityByCountryId(int countryId)
        {
            return _context.RefCities
                    .Include(x => x.Province)
                    .ThenInclude(x => x.Country)
                    .Where(x => x.Active && x.Province.Country.Id == countryId)
                    .OrderBy(x => x.CityName);
        }

        public async Task<RefCity> GetCityByCityName(string cityName)
        {
            return await _context.RefCities
                    .FirstOrDefaultAsync(x => x.Active && x.CityName.ToLower() == cityName.ToLower());
        }

        public IEnumerable<RefZone> GetZone()
        {
            return _context.RefZones.Where(x => x.Active != null && x.Active.Value);
        }

        public RefCity GetCityDetails(int cityid)
        {
            return _context.RefCities
                   .Include(x => x.Province)
                   .ThenInclude(x => x.Country)
                   .Include(x => x.RefCityDetails)
                   .Where(x => x.Active && x.Id == cityid).FirstOrDefault();
        }
        public RefCountry GetCountryDetails(int countryid)
        {
            return _context.RefCountries.Where(x => x.Active)
                .Include(x => x.Area)
                .Where(x => x.Id == countryid).FirstOrDefault();
        }
        public int SaveNewCountry(RefCountry entity)
        {
            _context.RefCountries.Add(entity);
            _context.SaveChanges();
            return entity.Id;
        }
        public void SaveEditCountry(RefCountry entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public int SaveNewProvince(RefProvince entity)
        {
            _context.RefProvinces.Add(entity);
            _context.SaveChanges();
            return entity.Id;
        }

        public void SaveEditProvince(RefProvince entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public RefProvince GetProvinceDetails(int id)
        {
            return _context.RefProvinces
               .Include(x => x.Country)
               .Where(x => x.Active != null && x.Active.Value && x.Id == id).FirstOrDefault();
        }

        public int SaveNewCity(RefCity entity)
        {
            _context.RefCities.Add(entity);
            _context.SaveChanges();
            return entity.Id;
        }

        public void SaveEditCity(RefCity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public async Task<IEnumerable<RefCity>> GetCitiesByTerm(string term)
        {
            return await _context.RefCities.Where(x => EF.Functions.Like(x.CityName, $"{term}%")).Take(20).ToListAsync();
        }

        public async Task SaveEditCounty(RefCounty entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<RefCounty> GetCountyDetails(int id)
        {
            return await _context.RefCounties
               .Where(x => x.Active && x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveNewCounty(RefCounty entity)
        {
            _context.RefCounties.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<IEnumerable<RefCounty>> GetCountiesByCity(int cityId)
        {
            return await _context.RefCounties.Where(x => x.CityId == cityId && x.Active).ToListAsync();
        }

        public async Task<IEnumerable<RefCounty>> GetCountyList()
        {
            return await _context.RefCounties
                .Include(x => x.City)
                .ThenInclude(x => x.Province)
                .ThenInclude(x => x.Country)
                .Where(x => x.Active)
                .OrderBy(x => x.CountyName).ToListAsync();
        }

        public async Task DeleteCounty(RefCounty entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<RefTown>> GetTownList()
        {
            return await _context.RefTowns
                .Include(x => x.County)
                .ThenInclude(x => x.City)
                .ThenInclude(x => x.Province)
                .ThenInclude(x => x.Country)
                .Where(x => x.Active)
                .OrderBy(x => x.TownName).ToListAsync();
        }

        public async Task<IEnumerable<RefTown>> GetTownsByCounty(int countyId)
        {
            return await _context.RefTowns.Where(x => x.CountyId == countyId && x.Active).ToListAsync();
        }

        public async Task<int> SaveNewTown(RefTown entity)
        {
            _context.RefTowns.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<RefTown> GetTownDetails(int id)
        {
            return await _context.RefTowns
               .Where(x => x.Active && x.Id == id).FirstOrDefaultAsync();
        }

        public async Task SaveEditTown(RefTown entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTown(RefTown entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<RefCity> GetCities(string cityName)
        {
            return await _context.RefCities.Where(x => x.Active
                 && x.CityName.ToLower().Trim() == cityName.ToLower().Trim())
                .OrderBy(x => x.CityName).FirstOrDefaultAsync();
        }

        public async Task<RefCountry> GetCountries(string countryName)
        {
            return await _context.RefCountries.Where(x => x.Active
                && x.CountryName.ToLower().Trim() == countryName.ToLower().Trim())
                .OrderBy(x => x.CountryName).FirstOrDefaultAsync();
        }

        public async Task<List<CommonDataSource>> GetCountriesByName(string countryName)
        {
            return await _context.RefCountries.Where(x => x.Active
                && EF.Functions.Like(x.CountryName.ToLower().Trim(), $"%{countryName.ToLower().Trim()}%"))
                .Select(y => new CommonDataSource
                {
                    Id = y.Id,
                    Name = y.CountryName
                }).AsNoTracking()
                .OrderBy(x => x.Name).ToListAsync();
        }

        //get country name based on Ids
        public async Task<List<CommonDataSource>> GetCountryByIds(List<int> countryIds)
        {
            return await _context.RefCountries.Where(x => x.Active && countryIds.Contains(x.Id))
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.CountryName
                })
                .ToListAsync();
        }
        //Get Countries by Ids
        public async Task<List<RefCountry>> GetCountriesByIds(List<int> countryIds)
        {
            return await _context.RefCountries.Where(x => x.Active && countryIds.Contains(x.Id)).ToListAsync();
        }
        // Get Province by Ids
        public async Task<List<RefProvince>> GetProvinceByIds(List<int> provinceIds)
        {
            return await _context.RefProvinces.Where(x => x.Active != null && x.Active.Value && provinceIds.Contains(x.Id)).ToListAsync();
        }

        //get country list by active
        public IQueryable<RefCountry> GetCountryDataSource()
        {
            return _context.RefCountries.Where(x => x.Active).OrderBy(x => x.CountryName);
        }

        //get country list by Office Id
        public async Task<List<ParentDataSource>> GetCountriesByOffice(List<int> officeIdList)
        {
            return await _context.RefLocationCountries.Where(x => officeIdList.Contains(x.LocationId))
                                  .Select(x => new ParentDataSource { Id = x.Country.Id, ParentId = x.LocationId, Name = x.Country.CountryName })
                                  .Distinct().AsNoTracking().ToListAsync();
        }
        //get Zone list by acitve
        public IQueryable<RefZone> GetZoneDataSource()
        {
            return _context.RefZones.Where(x => x.Active == true);
        }
        //get Zone list by ID
        public async Task<List<RefZone>> GetZoneByIds(List<int> zoneIdList)
        {
            return await _context.RefZones.Where(x => x.Active == true && zoneIdList.Contains(x.Id)).ToListAsync();
        }
        //get Zone list by office
        public IQueryable<RefZone> GetOfficeZoneDataSource(List<int?> officeIdList)
        {
            if (officeIdList != null && officeIdList.Any())
                return _context.RefZones.Where(x => x.Active.Value && officeIdList.Contains(x.LocationId));

            return _context.RefZones.Where(x => x.Active.Value);
        }

        //get province list by active
        public IQueryable<CommonProvinceDataSource> GetProvinceDataSource()
        {
            return _context.RefProvinces.Where(x => x.Active == true)
                                  .Select(x => new CommonProvinceDataSource { Id = x.Id, Name = x.ProvinceName, CountryId = x.CountryId }).OrderBy(x => x.Name);
        }

        //get province list by active
        public IQueryable<CommonCityDataSource> GetCityDataSource()
        {
            return _context.RefCities.Where(x => x.Active)
                                  .Select(x => new CommonCityDataSource
                                  {
                                      Id = x.Id,
                                      Name = x.CityName,
                                      ProvinceId = x.ProvinceId,
                                      CountryId = x.Province.CountryId,
                                      Priority = x.Province.Country.Priority
                                  }).OrderBy(x => x.Name);
        }

        public IQueryable<CommonTownDataSource> GetTownDataSource()
        {
            return _context.RefTowns.Where(x => x.Active)
                                  .Select(x => new CommonTownDataSource
                                  {
                                      Id = x.Id,
                                      Name = x.TownName,
                                      CountyId = x.CountyId,
                                      CountryId = x.County.City.Province.CountryId
                                  }).OrderBy(x => x.Name);
        }

        public IQueryable<CommonDataSource> GetStartPortDataSource()
        {
            return _context.EcAutRefStartPorts.Where(x => x.Active.Value)
                                  .Select(x => new CommonDataSource
                                  {
                                      Id = x.Id,
                                      Name = x.StartPortName,
                                  }).OrderBy(x => x.Name);
        }


        public async Task<int?> GetOfficeIdByCityId(int cityid)
        {
            return await _context.RefCityDetails.Where(x => x.CityId == cityid).Select(x => x.LocationId).FirstOrDefaultAsync();
        }

        public IQueryable<CommonDataSource> GetOfficeCountryDataSource()
        {
            return _context.RefLocations.Where(x => x.Active)
                .Select(x => new CommonDataSource
                {
                    Id = x.City.Province.CountryId,
                    Name = x.City.Province.Country.CountryName
                })
                .Distinct().OrderBy(x => x.Name);
        }

        /// <summary>
        /// Get Province By Country Ids
        /// </summary>
        /// <param name="countryIds"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetProvinceByCountryIds(List<int> countryIds)
        {
            return await _context.RefProvinces.Where(x => x.Active.Value && countryIds.Contains(x.CountryId))
                                  .Select(x => new CommonDataSource { Id = x.Id, Name = x.ProvinceName })
                                  .Distinct().AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the city list by province ids
        /// </summary>
        /// <param name="provinceIds"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetCityByProvinceIds(List<int> provinceIds)
        {
            return await _context.RefCities.Where(x => x.Active && provinceIds.Contains(x.ProvinceId))
                                  .Select(x => new CommonDataSource { Id = x.Id, Name = x.CityName })
                                  .Distinct().AsNoTracking().ToListAsync();
        }

        public async Task<RefCountry> GetCountriesByAlpha2Code(string alpha2Code)
        {
            return await _context.RefCountries.Where(x => x.Active
                 && x.Alpha2Code.ToLower().Trim() == alpha2Code.ToLower().Trim())
                 .OrderBy(x => x.Alpha2Code).FirstOrDefaultAsync();
        }

        public async Task<List<RefCountry>> GetCountriesByAlpha2CodeList(List<string> alpha2CodeList)
        {
            alpha2CodeList = alpha2CodeList.ConvertAll(d => d.ToLower().Trim());
            return await _context.RefCountries.Where(x => x.Active
                 && alpha2CodeList.Contains(x.Alpha2Code.ToLower().Trim()))
                 .OrderBy(x => x.Alpha2Code).AsNoTracking().ToListAsync();
        }

        public IQueryable<RefCity> GetCityQueryableByCountryId(int countryId)
        {
            return _context.RefCities
                    .Where(x => x.Active && x.Province.CountryId == countryId)
                    .OrderBy(x => x.CityName);
        }

        public async Task<CityDetails> GetCityByName(string term)
        {
            return await _context.RefCities.Where(x => EF.Functions.Like(x.CityName, $"{term}%") && x.Active).Select(y => new CityDetails()
            {
                CityName = y.CityName,
                CountryId = y.Province.CountryId,
                Id = y.Id,
                ProvinceId = y.ProvinceId
            }).FirstOrDefaultAsync();
        }
    }
}
