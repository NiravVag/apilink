using DTO.UserAccount;
using DTO.UserConfig;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IUserConfigManager
    {
        /// <summary>
        /// save the da details
        /// </summary>
        /// <param name="request"></param>
        /// <returns>success response id</returns>
        Task<UserConfigSaveResponse> Save(UserConfigSaveRequest request);

        /// <summary>
        /// edit the da details by userid
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>saved values</returns>
        Task<UserConfigEditResponse> Edit(int userId);
    }
}
