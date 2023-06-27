using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DTO.CommonClass;
using DTO.OfficeLocation;
using Entities;

namespace Contracts.Managers
{
    public interface IOfficeLocationManager
    {
        /// <summary>
        /// Get Office Summary
        /// </summary>
        /// <returns>OfficeSummaryResponse</returns>
        OfficeSummaryResponse GetOfficeSummary();

        /// <summary>
        /// Get search office  
        /// </summary>
        /// <returns></returns>
        OfficeSearchResponse GetOfficeSearchSummary(OfficeSearchRequest request);

        /// <summary>
        /// Edit office response 
        /// </summary>
        /// <returns></returns>
       Task<EditOfficeResponse> GetEditOffice(int? id);

        Task<EditOfficeResponse> GetEditOfficeDetails(int id);
        /// <summary>
        /// Save Office
        /// </summary>
        /// <param name="Office"></param>
        /// <returns></returns>
        Task<SaveOfficeResponse> SaveOffice(Office office);

        /// <summary>
        /// Get offices
        /// </summary>
        /// <returns></returns>
        IEnumerable<Office> GetOffices();


        /// <summary>
        /// Get offices
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Office>> GetOfficesAsync();

        /// <summary>
        /// Get All offices
        /// </summary>
        /// <returns></returns>
        IEnumerable<Office> GetAllOffices();

        /// <summary>
        /// Get Location By Factory Id
        /// </summary>
        /// <param name="FactoryId"></param>
        /// <returns>GetLocationByFactoryid</returns>
        Task<Office> GetOfficeByFactoryid(int FactoryId);

        /// <summary>
        /// Get offices By UserId
        /// </summary>
        /// <returns>Office</returns>
        IEnumerable<Office> GetOfficesByUserId(int UserId);

        /// <summary>
        /// Get offices By UserId
        /// </summary>
        /// <returns>Office</returns>
        Task<IEnumerable<Office>> GetOfficesByUserIdAsync(int UserId);

        /// <summary>
        /// Get Location By Factory Id for Inspection
        /// </summary>
        /// <param name="FactoryId"></param>
        /// <returns>GetLocationByFactoryid</returns>
        Task<RefLocation> GetOfficeByFactoryidInspection(int FactoryId);

        /// <summary>
        /// Get Location for internal user
        /// </summary>
        OfficeResponse GetofficeforInternalUser();

        /// <summary>
        /// get Location List
        /// </summary>
        Task<DataSourceResponse> GetLocationList();

        Task<DataSourceResponse> GetOfficesByOfficeAccess(CommonDataSourceRequest request);

        Task<IEnumerable<int>> GetOnlyOfficeIdsByUser(int UserId);

        Task<DataSourceResponse> GetOfficeLocationList();
    }
}
