using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.Location;
using DTO.OfficeLocation;
using DTO.References;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class OfficeLocationRepository : Repository, IOfficeLocationRepository
    {
        public OfficeLocationRepository(API_DBContext context) : base(context)
        {
        }


        public IEnumerable<RefLocation> GetLocations()
        {
            return _context.RefLocations.Where(x => x.Active)
               .Include(x => x.LocationType).Where(x => x.LocationType.Active)
               .Include(x => x.City.Province.Country).Where(x => x.City.Province.Country.Active)
               .Include(x => x.City.Province).Where(x => x.City.Province.Active.HasValue && x.City.Province.Active.Value)
               .Include(x => x.City).Where(x => x.City.Active)
               .Include(x => x.City.SuAddresses)
               .Include(x => x.RefLocationCountries)
               .ThenInclude(x => x.Country)
               .OrderBy(x => x.LocationName);
        }

        public async Task<Office> GetLocationDetails(int officeId)
        {
            return await _context.RefLocations.Where(x => x.Active && x.Id == officeId)
                .Select(x => new Office
                {
                    Id = x.Id,
                    OfficeCode = x.OfficeCode,
                    Address = x.Address,
                    Address2 = x.Address2,
                    CityId = x.CityId,
                    ZipCode = x.ZipCode,
                    Name = x.LocationName,
                    Master_Currency_Id = x.MasterCurrencyId,
                    Default_Currency_Id = x.DefaultCurrencyId,
                    Tel = x.Tel,
                    Fax = x.Fax,
                    Email = x.Email,
                    ParentId = x.ParentId,
                    Comment = x.Comment,
                    EntityId = x.EntityId,
                    HeadOffice = x.HeadOffice,
                    LocationTypeId = x.LocationTypeId,
                    Type = new OfficeType
                    {
                        Id = x.LocationTypeId,
                        Name = x.LocationType.SgtLocationType
                    },
                    City = new City
                    {
                        Id = x.City.Id,
                        Name = x.City.CityName
                    },
                    Country = new Country
                    {
                        Id = x.City.Province.Country.Id,
                        CountryName = x.City.Province.Country.CountryName
                    }
                }).AsNoTracking().FirstOrDefaultAsync();
        }


        public async Task<List<RefLocation>> GetLocationsAsync()
        {
            return await _context.RefLocations.Where(x => x.Active)
               .Include(x => x.LocationType).Where(x => x.LocationType.Active)
               .OrderBy(x => x.LocationName).ToListAsync();
        }

        public IEnumerable<RefLocationType> GetLocationTypeList()
        {
            return _context.RefLocationTypes.Where(x => x.Active)
                .OrderBy(x => x.SgtLocationType);
        }

        public RefLocation GetOfficeDetails(int id)
        {
            return _context.RefLocations.Where(x => x.Active)
                .Include(x => x.LocationType)
                .Include(x => x.RefLocationCountries)
                .Where(x => x.Id == id).FirstOrDefault();
        }

        public Task<int> SaveEditOffice(RefLocation entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            return _context.SaveChangesAsync();
        }

        public Task<int> SaveNewOffice(RefLocation entity)
        {
            _context.RefLocations.Add(entity);
            return _context.SaveChangesAsync();
        }

        public Task<RefLocation> GetOfficeByFactId(int FactId)
        {
            return _context.RefCityDetails
                .Include(x => x.City)
                .ThenInclude(x => x.SuAddresses)
                .Include(x => x.Location)
                .Where(x => x.Active && x.City.SuAddresses.Any(y => y.SupplierId == FactId && y.AddressTypeId == (int)Entities.Enums.Supplier_Address_Type.HeadOffice))
                .Select(x => x.Location).FirstOrDefaultAsync();
        }

        public IEnumerable<RefLocation> GetLocationsByUserId(int userid)
        {
            return _context.RefLocations.Include(x => x.LocationType)
                .Include(x => x.HrOfficeControls)
                .Where(x => x.Active && x.HrOfficeControls.Any(y => y.StaffId == userid));
        }
        public async Task<List<RefLocation>> GetLocationsByUserIdAsync(int userid)
        {
            return await _context.RefLocations
                 .Include(x => x.LocationType)
                .Include(x => x.HrOfficeControls)
                .Where(x => x.Active && x.HrOfficeControls.Any(y => y.StaffId == userid)).OrderBy(x => x.LocationName).ToListAsync();
        }


        //get location list
        public async Task<IEnumerable<RefLocation>> GetLocationList()
        {
            return await _context.RefLocations.Where(x => x.Active).OrderBy(x => x.LocationName).ToListAsync();
        }

        //get office name based on Ids
        public async Task<IEnumerable<CommonDataSource>> GetLocationListByIds(List<int> locationIds)
        {
            return await _context.RefLocations.Where(x => x.Active && locationIds.Contains(x.Id)).OrderBy(x => x.LocationName)
                .Select(x => new CommonDataSource
                {
                    Id = x.Id,
                    Name = x.LocationName
                })
                .ToListAsync();
        }

        /// <summary>
        /// Location by office access
        /// </summary>
        /// <returns></returns>
        public IQueryable<CommonDataSource> GetOfficesByOfficeAccess()
        {
            return _context.RefLocations.Where(x => x.Active).OrderBy(x => x.LocationName)
               .Select(x => new CommonDataSource
               {
                   Id = x.Id,
                   Name = x.LocationName
               });
        }

        /// <summary>
        /// Get office Id by user 
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<int>> GetOfficeIdsbyUser(int UserId)
        {
            return await _context.RefLocations
                .Where(x => x.Active && x.HrOfficeControls.Any(y => y.StaffId == UserId))
                .Select(x => x.Id).ToListAsync();
        }

        /// <summary>
        /// Get the location details by staff id
        /// </summary>
        /// <param name="staffId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetLocationsByStaffId(int staffId)
        {
            return await _context.RefLocations.Where(x => x.Active && x.HrOfficeControls.Any(y => y.StaffId == staffId)).
                          Select(x => new CommonDataSource() { Id = x.Id, Name = x.LocationName }).OrderBy(x => x.Name).AsNoTracking().ToListAsync();
        }

        /// <summary>
        /// Get the office list
        /// </summary>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetOfficesList()
        {
            return await _context.RefLocations.Where(x => x.Active).OrderBy(x => x.LocationName)
               .Select(x => new CommonDataSource
               {
                   Id = x.Id,
                   Name = x.LocationName
               }).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<RefLocation>> GetHeadOfficeList(int officeId)
        {
            return await _context.RefLocations.Where(x => x.Active && x.Id != officeId).OrderBy(x => x.LocationName)
               .Select(x => new RefLocation
               {
                   Id = x.Id,
                   LocationName = x.LocationName
               }).AsNoTracking().ToListAsync();
        }

        public async Task<List<OfficeCoutry>> GetRefLocationCountries(int officeId)
        {
            return await _context.RefLocationCountries.Where(x => x.LocationId == officeId)
                .Select(x => new OfficeCoutry
                {
                    OfficeId = x.LocationId,
                    CountryId = x.CountryId,
                    CountryName = x.Country.CountryName
                }).AsNoTracking().ToListAsync();
        }
    }
}
