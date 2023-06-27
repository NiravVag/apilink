using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DTO.CommonClass;
using DTO.User;
using DTO.UserAccount;
using DTO.UserProfile;

namespace Contracts.Managers
{
    public interface IUserAccountManager
    {
        /// <summary>
        /// Get user account summary
        /// </summary>
        /// <returns>user account response</returns> 
        UserAccountResponse GetUserAccountSummary();

        /// <summary>
        /// Get user account search summary
        /// </summary>
        /// <returns>ProductResponse</returns> 
        Task<UserAccountSearchResponse> GetUserAccountSearchSummary(UserAccountSearchRequest request);

        /// <summary>
        /// Get user account detail
        /// </summary>
        /// <returns>EditUserAccountResponse</returns> 
        Task<EditUserAccountResponse> GetUserAccountDetail(UserAccountSearchRequest request);

        /// <summary>
        /// Save User Account
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<SaveUserAccountResponse> SaveUserAccount(UserAccountItem request);

        /// <summary>
        /// Delete user account by id
        /// </summary>
        /// <returns>ProductResponse</returns> 
        Task<DeleteUserAccountResponse> RemoveUserAccount(int id);
        /// <summary>
        /// Login user role exists
        /// </summary>
        /// <returns>return boolean</returns> 
        bool LoggedUserRoleExists(int roleId);

        /// <summary>
        /// get user name by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>user name</returns>
        Task<UserNameResponse> GetUserName(int id);

        Task<SaveUserResponse> AddUserDetails(UserAccountItem request);

        Task<SaveUserResponse> GetLoginUserDetail(int contactId, int usertypeId);
        Task<DataSourceResponse> GetUserDataSource(UserDataSourceRequest request);


        /// <summary>
        ///  Get all roles
        /// </summary>
        /// <returns></returns>
        Task<RolesResponse> GetRoles();

        Task<UserApplicantDetails> GetUserApplicantDetails();
        Task<UserCredentialsMailTemplateResponse> GetUserCredentialsMailDetail(int userId);

        /// <summary>
        /// Forgot Password
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<ForgotPasswordResponse> ForgotPassword(string username);
    }
}
