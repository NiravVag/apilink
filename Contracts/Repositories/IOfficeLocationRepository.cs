using DTO.CommonClass;
using DTO.OfficeLocation;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IOfficeLocationRepository : IRepository
    {
        /// <summary>
        /// Get all locations
        /// </summary>
        /// <returns></returns>
        IEnumerable<RefLocation> GetLocations();

        Task<Office> GetLocationDetails(int id);

        /// <summary>
        /// Get all locations
        /// </summary>
        /// <returns></returns>
        Task<List<RefLocation>> GetLocationsAsync();

        /// <summary>
        /// Get all Location Type
        /// </summary>
        /// <returns></returns>
        IEnumerable<RefLocationType> GetLocationTypeList();

        /// <summary>
        /// Get OfficeDetails By Id
        /// </summary>
        /// <param name="office id"></param>
        /// <returns>Ref Location</returns>
        RefLocation GetOfficeDetails(int id);

        /// <summary>
        /// Save New Office
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>id</returns>
        Task<int> SaveNewOffice(RefLocation entity);

        /// <summary>
        /// Save Edit Office
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> SaveEditOffice(RefLocation entity);

        /// <summary>
        /// Get Office By Factory Id
        /// </summary>
        /// <returns></returns>
        Task<RefLocation> GetOfficeByFactId(int FactId);

        /// <summary>
        /// Get all locations By UserId
        /// </summary>
        /// <returns></returns>
        IEnumerable<RefLocation> GetLocationsByUserId(int userid);

        /// <summary>
        /// Get all locations By UserId async
        /// </summary>
        /// <returns></returns>
        Task<List<RefLocation>> GetLocationsByUserIdAsync(int userid);

        /// <summary>
        /// get location list
        /// </summary>
        /// <returns>location list</returns>
        Task<IEnumerable<RefLocation>> GetLocationList();

        /// <summary>
        /// get location list by Ids
        /// </summary>
        /// <returns>location list</returns>
        Task<IEnumerable<CommonDataSource>> GetLocationListByIds(List<int> locationIds);

        IQueryable<CommonDataSource> GetOfficesByOfficeAccess();

        Task<IEnumerable<int>> GetOfficeIdsbyUser(int UserId);

        Task<List<CommonDataSource>> GetLocationsByStaffId(int staffId);

        Task<List<CommonDataSource>> GetOfficesList();
        Task<IEnumerable<RefLocation>> GetHeadOfficeList(int id);
        Task<List<OfficeCoutry>> GetRefLocationCountries(int id);
    }
}
