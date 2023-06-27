using DTO.User;
using DTO.UserAccount;
using Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IUserAccountRepository : IRepository
    {
        /// <summary>
        /// Get user by type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IQueryable<ItUserMaster> GetUserByType(int? type);

        /// <summary>
        /// Get user by name
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool GetUserByName(string name, int? id);

        /// <summary>
        /// Save edit user account
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> SaveEditUserAccount(ItUserMaster entity);

        /// <summary>
        /// Save new user account
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> SaveNewUserAccount(ItUserMaster entity);

        /// <summary>
        /// Delete user account
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> RemoveUserAccount(ItUserMaster entity, int updatedby);
        /// <summary>
        /// Get all user type
        /// </summary>
        /// <returns></returns>
        IEnumerable<ItUserType> GetUserTypes();

        /// <summary>
        ///Get ITUser by Contact Id
        /// </summary>
        /// <returns>User Id</returns> 
        Task<ItUserMaster> GetUserByContactId(int contactId);

        /// <summary>
        ///Get ITUser by Contact Id
        /// </summary>
        /// <returns>User Id</returns> 
        Task<IEnumerable<ItUserMaster>> GetUserListByContactId(List<int> contactId);
        /// <summary>
        /// get user name by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>user name</returns>
        Task<string> GetUserName(int id);

        IQueryable<ItUserMaster> GetUserDetails();

        Task<string> GetLoginName(string prefix);

        Task<bool> CheckLoginNameExist(string LoginName);

        Task<int> GetCustomerContactIdByUser(int userId);

        Task<int> GetSupplierContactIdByUser(int userId);

        Task<int> GetFactoryContactIdByUser(int userId);

        Task<UserApplicantDetails> GetCustomerContactUserApplicationDetails(int contactId);

        Task<UserApplicantDetails> GetSupplierContactUserApplicationDetails(int contactId);

        Task<IEnumerable<ItUserMaster>> GetUserByCustomerContactIds(IEnumerable<int> customerContactIds);
    }
}
