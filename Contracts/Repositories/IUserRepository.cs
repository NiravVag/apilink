using DTO.CommonClass;
using DTO.DataAccess;
using DTO.Inspection;
using DTO.User;
using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Repositories
{
    public interface IUserRepository : IRepository
    {
        /// <summary>
        /// Get user 
        /// </summary>
        /// <param name="idUser"></param>
        /// <returns></returns>
        Task<ItUserMaster> GetUserDetails(int idUser);

        /// <summary>
        /// get user by login and password
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<int?> GetUserSignIn(string loginName, string password);


        Task<List<Role>> GetUserRole(int UserId, int entityId);

        Task<List<Right>> GetUserRights(List<Role> RoleIds, int entityId);

        Task<User> GetUserInfo(int userId);

        /// <summary>
        /// Get Rights
        /// </summary>
        /// <param name="righIdList"></param>
        /// <returns></returns>
        Task<IEnumerable<ItRight>> GetRights();

        /// <summary>
        /// Get Roles List
        /// </summary>
        /// <returns></returns>
        IEnumerable<ItRight> GetRightList();

        /// <summary>
        /// Get Roles
        /// </summary>
        /// <param name="roleIdList"></param>
        /// <returns></returns>
        Task<IEnumerable<ItRole>> GetRoles();

        /// <summary>
        /// Get Roles List
        /// </summary>
        /// <param name="roleIdList"></param>
        /// <returns></returns>
        IEnumerable<ItRole> GetRoleList();

        /// <summary>
        /// get entities
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<ApEntity>> GetEntities();

        /// <summary>
        /// Get tasks for user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleList"></param>
        /// <param name="officeControlList"></param>
        /// <returns></returns>
        Task<IEnumerable<MidTask>> GetTasks(int userId, IEnumerable<int> roleList, IEnumerable<int> officeControlList);

        /// <summary>
        /// Ge task
        /// </summary>
        /// <returns></returns>
        IQueryable<MidTask> GetTaskData();

        /// <summary>
        /// Get Notification
        /// </summary>
        /// <returns></returns>
        IQueryable<MidNotification> GetNotificationData();


        /// <summary>
        /// Get task
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<MidTask> GetTask(Expression<Func<MidTask, bool>> predicate);

        /// <summary>
        /// Get notification
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<MidNotification> GetNotification(Expression<Func<MidNotification, bool>> predicate);

        /// <summary>
        /// Get notifications
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<MidNotification>> GetNotifications(int userId);

        /// <summary>
        /// Read Notification
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<MidNotification> ReadNotification(Guid Id);

        /// <summary>
        /// GetUserListByRole
        /// </summary>
        /// <param name="officeId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<IEnumerable<ItUserMaster>> GetUserListByRole(int officeId, int roleId);

        /// <summary>
        /// GetExternalUserListByRole
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userid"></param>
        /// <param name="usertype"></param>
        /// <returns></returns>
        Task<IEnumerable<ItUserMaster>> GetExternalUserListByRole(int roleId, int userid, UserTypeEnum usertype);

        /// <summary>
        /// CheckOldPasswordExists
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool CheckOldPasswordExists(int userId, string password);

        /// <summary>
        /// UpdatePassword
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<int> UpdatePassword(int userId, string password, string oldpassword);

        /// <summary>
        /// save role detail
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<int> SaveRoleDetail(ItRole entity);

        /// <summary>
        /// Get Parent Booking Id
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<IEnumerable<InspTransaction>> GetParentBookingId();

        /// <summary>
        /// get user list based on role, office ,service id, productcategory.
        /// </summary>
        /// <param name="userAccess"></param>
        /// <returns>return user list</returns>
        Task<IEnumerable<User>> GetUserListByRoleOfficeService(UserAccess userAccess);
        /// <summary>
        /// GetCsEnabledBookingContacts
        /// </summary>
        /// <param name="userAccess"></param>
        /// <returns></returns>
        Task<List<UserStaffDetails>> GetCsEnabledBookingContacts(UserAccess userAccess);
        /// <summary>
        /// GetCSByCustomer
        /// </summary>
        Task<List<AEDetails>> GetAEByCustomer(List<int> lstcustomerId);
        /// <summary>
        /// GetAEByCustomerAndLocation
        /// </summary>
        /// <param name="locationIds"></param>
        /// <param name="customerIds"></param>
        /// <returns>AE list</returns>
        Task<IEnumerable<AEDetails>> GetAEByCustomerAndLocation(IEnumerable<int> locationIds, IEnumerable<int> customerIds);
        /// <summary>
        /// GetCSList
        /// </summary>
        Task<List<AEDetails>> GetAEList();

        /// <summary>
        /// GetRolesList
        /// </summary>
        /// <returns>role list</returns>
        Task<IEnumerable<ItRole>> GetRolesList(int userId);
        /// <summary>
        /// Get the user details mapped to the user config and cs(AE) access 
        /// </summary>
        /// <param name="userAccess"></param>
        /// <returns></returns>
        Task<List<UserStaffDetails>> GetCSBookingContacts(UserAccess userAccess);
        /// <summary>
        /// Get the user details mapped to the user config and report checker access 
        /// </summary>
        /// <param name="userAccess"></param>
        /// <returns></returns>
        Task<List<UserStaffDetails>> GetUserListByReportCheckerProfile(UserAccess userAccess);

        /// <summary>
        /// Get user idList by customer
        /// </summary>
        /// <param name="customerid"></param>
        /// <returns></returns>
        Task<IEnumerable<int>> GetUsersIdListByCustomer(int customerid);
        /// <summary>
        /// Get the user details who has role access
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<IEnumerable<UserStaffDetails>> GetRoleAccessUserList(int roleId);
        /// <summary>
        /// Get the user details who has role access
        /// </summary>
        /// <param name="linkId"></param>
        /// <param name="typeIdList"></param>
        /// <param name="isDone"></param>
        /// <returns></returns>
        Task<IEnumerable<MidTask>> GetTaskList(int linkId, IEnumerable<int> typeIdList, bool isDone);
        /// <summary>
        /// get user list based on role, office.
        /// </summary>
        /// <param name="userAccess"></param>
        /// <returns>return user list</returns>
        Task<IEnumerable<User>> GetUserListByRoleOffice(UserAccess userAccess);
        /// <summary>
        /// get user list by customer, dept, and brand id
        /// </summary>
        /// <param name="userAccess"></param>
        /// <returns></returns>
        Task<IEnumerable<CSConfigDetail>> GetUserListByCusDeptBrandData(UserAccess userAccess);
        /// <summary>
        /// get ae config list
        /// </summary>
        /// <param name="UserIds"></param>
        /// <returns></returns>
        Task<List<CSConfigDetail>> GetAECustomerConfigList(IEnumerable<int> UserIds);

        Task<IEnumerable<string>> GetUserEmailByRoleAndOffice(int officeId, int roleId);

        Task<List<CommonDataSource>> GetAEUserList();

        Task<List<int>> GetInternalUserServiceIds(int? staffId, int primaryEntity);

        Task<List<CommonDataSource>> GetInternalUserEntityAccess(int userId);

        Task<List<CommonDataSource>> GetSupplierEntityAccess(int userId);

        Task<List<CommonDataSource>> GetFactoryEntityAccess(int userId);

        Task<List<CommonDataSource>> GetCustomerEntityAccess(int userId);

        Task<CommonDataSource> GetInternalUserPrimaryEntity(int? staffId);
        Task<IEnumerable<ItUserRole>> GetUserRolesByUserIds(IEnumerable<int> userIds);
        Task<IEnumerable<string>> GetUserEmailByRoleAndOfficeList(IEnumerable<int> officeIds, int roleId);
        Task<IEnumerable<ItUserRole>> GetUserRolesByUserIdsIgnoreQueryFilter(IEnumerable<int> userIds);

        Task<IEnumerable<ItRightEntity>> GetItRightEntitesList();
        Task<IEnumerable<ItUserMaster>> GetUsersByRoleId(int roleId);
        Task<int> ResetPassword(int userId, string password);
        Task<int> GetCustomerContactIdByUserId(int userId);
        Task<List<RightType>> GetRightTypeList();

        Task<CuContact> GetCustomerContactByUserId(int userId);
    }
}
