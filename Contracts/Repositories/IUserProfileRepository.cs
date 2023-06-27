using DTO.UserProfile;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IUserProfileRepository : IRepository
    {
        /// <summary>
        /// Get the profile data by user Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserProfile> GetUserProfileData(int userId, int userType);

        /// <summary>
        /// Get the it user master data
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ItUserMaster> GetUserProfileEntity(int userId);

        /// <summary>
        /// Get contacts by email to check if email exists
        /// </summary>
        /// <param name="emailId"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        Task<List<UserProfile>> GetContactsByEmail(string emailId, int userType);
    }
}
