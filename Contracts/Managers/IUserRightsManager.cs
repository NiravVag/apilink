using DTO.CommonClass;
using DTO.DataAccess;
using DTO.MobileApp;
using DTO.User;
using Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Contracts.Managers
{
    public interface IUserRightsManager
    {
        /// <summary>
        /// SignIn
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="isEncrypted"></param>
        /// <returns></returns>
        Task<SignInResponse> SignIn(string userName, string password, bool isEncrypted);

        /// <summary>
        /// Has right
        /// </summary>
        /// <param name="path"></param>
        /// <param name="roles"></param>
        /// <returns></returns>
        Task<bool> HasRight(string path, IEnumerable<string> roles);

        /// <summary>
        /// Get entity Id
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<int> GetEntityId(string name);

        /// <summary>
        /// Set cache entities
        /// </summary>
        void SetCacheEntities();

        /// <summary>
        ///  set riights entities
        /// </summary>
        /// <param name="rights"></param>
        void SetRightsCaches();

        /// <summary>
        /// Get task list
        /// </summary>
        /// <returns></returns>
        Task<TaskResponse> GetTaskList();

        /// <summary>
        /// Task List Data
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<TaskResponse> TaskListData(TaskSearchRequest request);

        /// <summary>
        /// Get notification list
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<NotificationResponse> GetNotifications(NotificationSearchRequest request);

        /// <summary>
        /// Done Task
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<TaskResponse> DoneTask(Guid Id);

        /// <summary>
        /// Read notification
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<NotificationResponse> ReadNotification(Guid Id);

        /// <summary>
        /// All Done Task
        /// </summary>
        /// <returns></returns>
        Task<TaskResponse> AllDoneTask();

        /// <summary>
        /// Read all notification
        /// </summary>
        /// <returns></returns>
        Task<NotificationResponse> ReadAllNotification();

        /// <summary>
        /// Not Done Task Count
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<NotDoneTaskResponse> NotDoneTaskCount(TaskSearchRequest request);

        /// <summary>
        /// UnRead Notification Count
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<UnReadNotificationResponse> UnReadNotificationCount(NotificationSearchRequest request);

        /// <summary>
        /// Get UserList By roles
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="officeId"></param>
        /// <returns></returns>
        Task<IEnumerable<User>> GetUserListByRole(int roleId, int officeId);

        /// <summary>
        /// Get UserList By roles
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="userid"></param>
        /// <param name="usertype"></param>
        /// <returns></returns>
        Task<IEnumerable<User>> GetExternalUserListByRole(int roleId, int userid, UserTypeEnum usertype);

        /// <summary>
        /// Edit Task
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="voidUpdate"></param>
        /// <returns></returns>
        Task<MidTask> EditTask(Expression<Func<MidTask, bool>> predicate, Action<MidTask> voidUpdate);

        /// <summary>
        /// Edit notification
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="voidUpdate"></param>
        /// <returns></returns>
        Task<MidNotification> EditNotification(Expression<Func<MidNotification, bool>> predicate, Action<MidNotification> voidUpdate);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest request);
        /// <summary>
        /// GetUserList By Role, location, customer, service and condition in Customer,email flag in data access table
        /// </summary>
        /// <param name="userAccess"></param>
        /// <returns>user list </returns>
        Task<IEnumerable<User>> GetUserListByRoleOfficeServiceCustomer(UserAccess userAccess);

        /// <summary>
        /// GetCustomerServiceBookingContacts
        /// </summary>
        /// <param name="userAccessFilter"></param>
        /// <returns></returns>
        Task<IEnumerable<UserStaffDetails>> GetCustomerServiceBookingContacts(UserAccess userAccessFilter);

        /// GetCSByCustomer
        /// </summary>
        Task<IEnumerable<AEDetails>> GetAEbyCustomer(List<int> lstcustomerId);
        /// <summary>
        /// GetAEByCustomerAndLocation
        /// </summary>
        /// <param name="locationIds"></param>
        /// <param name="customerIds"></param>
        /// <returns>AE list</returns>
        Task<IEnumerable<AEDetails>> GetAEByCustomerAndLocation(IEnumerable<int> locationIds, IEnumerable<int> customerIds);
        /// <summary>
        /// GetCustomerServiceBookingContact NoFilterProductCategory
        /// </summary>
        /// <param name="userAccessFilter"></param>
        /// <returns></returns>
        Task<IEnumerable<UserStaffDetails>> GetCSBookingContact(UserAccess userAccessFilter);
        /// <summary>
        ///GetUserListByReportCheckerProfile and useraccess filter
        /// </summary>
        /// <param name="userAccessFilter"></param>
        /// <returns></returns>
        Task<IEnumerable<UserStaffDetails>> GetUserListByReportCheckerProfile(UserAccess userAccessFilter);
        /// <summary>
        /// Get the user details who has role access
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<IEnumerable<UserStaffDetails>> GetRoleAccessUserList(int roleId);


        /// <summary>
        /// Get user ids by customer
        /// </summary>
        /// <param name="customerid"></param>
        /// <returns></returns>
        Task<IEnumerable<int>> GetUsersIdListByCustomer(int customerid);
        /// <summary>
        /// Get user ids by customer
        /// </summary>
        /// <param name="linkId"></param>
        /// <param name="typeIdList"></param>
        /// <param name="isDone"></param>
        /// <returns></returns>
        Task<IEnumerable<MidTask>> GetTask(int linkId, IEnumerable<int> typeIdList, bool isDone);
        ///</summary>
        /// <param name="id"></param>
        /// <param name="typeIdList"></param>
        /// <param name="oldTaskDoneValue"></param>
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<MidTask>> UpdateTask(int id, IEnumerable<int> typeIdList, bool oldTaskDoneValue);
        ///</summary>
        /// <param name="Id"></param>
        /// <param name="taskType"></param>
        /// </summary>
        /// <param name="userIdList"></param>
        /// <returns></returns>
        Task AddTask(TaskType taskType, int id, IEnumerable<int> userIdList);

        /// <summary>
        /// Get the task type with common user id
        /// </summary>
        /// <param name="taskType"></param>
        /// <param name="id"></param>
        /// <param name="userIdList"></param>
        /// <param name="commomUserId"></param>
        /// <returns></returns>
        Task AddTask(TaskType taskType, int id, IEnumerable<int> userIdList, int commomUserId);
        ///</summary>
        /// <param name="userAccess"></param>
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<User>> GetUserListByRoleOffice(UserAccess userAccess);
        Task<int> SaveLoginLog(DeviceLocation locationDetails);
        Task<IEnumerable<string>> GetUserEmailbyRoleAndOffice(int roleId, int officeId);
        Task<List<CommonDataSource>> GetUserEntityAccess(int userId);
        Task<UserEntityAccess> GetUserEntityRoleAccess(int userId, string currentEntity);
        Task<IEnumerable<string>> GetUserEmailbyRoleAndOfficeList(int roleId, IEnumerable<int> officeIds);
        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<ResetPasswordResponse> ResetPassword(ResetPasswordRequest request);
        Task<RightTypeResponse> GetRightTypeList();
    }
}
