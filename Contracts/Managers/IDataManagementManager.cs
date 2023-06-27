using DTO.CommonClass;
using DTO.DataManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IDataManagementManager
    {
        /// <summary>
        /// Get Modules by id , optional
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ModuleListResponse> GetModules(int? id = null);


        /// <summary>
        /// Get Item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<DataManagementItemResponse> GetItem(int id);

        /// <summary>
        /// Save item
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DataManagmentEmailResponse> Save(SaveDataManagementRequest request);


        /// <summary>
        /// Get rights
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DataManagementRightResponse> GetRights(DataManagementRightRequest request);


        /// <summary>
        /// Save rights for modules
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<DataManagementRightResponse> SaveRights(SaveDataManagementRightRequest request);

        Task<DMModuleResponse> GetModuleList();

        Task<DataManagementListResponse> SearchDMDetail(DataManagementListRequest request);

        Task<DataManagementDeleteResponse> DeleteDataManagement(int fileId);

        Task<DMUserRightResponse> GetDMUserManagementSummary(DMUserManagementSummaryRequest request);

        Task<DataManagementRightResponse> UpdateRights(int id, SaveDataManagementRightRequest request);
        Task<DMUserManagementDataEditResponse> EditDMUserManagement(int id);

        Task<DeleteDMUserManagementResponse> DeleteDMUserManagement(int id);

        Task<bool> IsDmUploadRights(int moduleId);
        Task<ModuleListResponse> GetModulesByDmRoleId(int id);
    }
}
