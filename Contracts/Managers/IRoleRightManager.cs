using DTO;
using DTO.CommonClass;
using DTO.RoleRight;
using DTO.User;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IRoleRightManager
    {
        /// <summary>
        /// Get role right summary
        /// </summary>
        /// <returns></returns>
        RoleRightResponse GetRoleRightSummary();

        /// <summary>
        /// get role right by role id
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        RoleRightSearchResponse GetRoleRightSearch(int roleId);

        /// <summary>
        /// Save role right
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<SaveRoleRightResponse> SaveRoleRight(RoleRightRequest request);

        /// <summary>
        /// get role list
        /// </summary>
        /// <returns></returns>
        Task<DataSourceResponse> GetRoleList(int userId);

    }
}
