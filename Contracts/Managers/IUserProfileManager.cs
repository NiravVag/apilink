using DTO.UserProfile;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IUserProfileManager
    {
        /// <summary>
        /// Fetch the profile data
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserProfileResponse> GetUserProfileSummary(int userId);

        /// <summary>
        /// Update the Profile data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<UserProfileSaveResponse> Save(UserProfileRequest request);
    }
}
