using BI.Cache;
using BI.Maps;
using BI.Utilities;
using Contracts.Managers;
using Contracts.Repositories;
using DTO.Common;
using DTO.CommonClass;
using DTO.DataAccess;
using DTO.User;
using Entities;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BI
{
    public class UserRightsManager : IUserRightsManager
    {
        private IUserRepository _repository = null;
        private ICacheManager _cache = null;

        private ITranslationManager _translationManager = null;
        private readonly IAPIUserContext _ApplicationContext = null;
        private readonly ISupplierRepository _supplierRepo = null;
        private readonly ICustomerRepository _customerRepository = null;
        private readonly UserMap UserMap = null;
        private readonly ITenantProvider _filterService = null;

        public UserRightsManager(IUserRepository repository, ICacheManager cache, ITranslationManager translationManager, IAPIUserContext applicationContext,
                    ISupplierRepository supplierRepo, ICustomerRepository customerRepository, ITenantProvider filterService)
        {
            _repository = repository;
            _cache = cache;
            _translationManager = translationManager;
            _ApplicationContext = applicationContext;
            _supplierRepo = supplierRepo;
            _customerRepository = customerRepository;
            UserMap = new UserMap();
            _filterService = filterService;
        }

        public async Task<bool> HasRight(string path, IEnumerable<string> roles)
        {
            // var rightList = await _cache.CacheTryGetValueSet(CacheKeys.AllRights,
            //   () => _repository.GetRights());
            var rightList = await _repository.GetRights();
            if (rightList == null || !rightList.Any())
                return true;

            var right = rightList.FirstOrDefault(x => !string.IsNullOrEmpty(x.Path) && x.Path.Contains(path));

            if (right == null || right.ItRoleRights == null || !right.ItRoleRights.Any())
                return true;

            if (roles == null || !roles.Any())
                return false;

            foreach (var role in roles)
            {
                if (await HasRight(role, right))
                    return true;
            }

            return false;
        }

        public async Task<int> GetEntityId(string name)
        {

            //var data = await _cache.CacheTryGetValueSet(CacheKeys.Instances,
            //    () => _repository.GetEntities());
            var data = await _repository.GetEntities();
            if (data == null || !data.Any())
                return 0;

            var item = data.FirstOrDefault(x => x.Name.Trim().ToUpper() == name.Trim().ToUpper());

            if (item == null)
                return 0;

            return item.Id;

        }


        private async Task<bool> HasRight(string role, ItRight right)
        {
            //var data = await _cache.CacheTryGetValueSet(CacheKeys.AllRoles,
            //    () => _repository.GetRoles());
            var data = await _repository.GetRoles();
            var roles = data.Where(x => x.ItRoleRights.Any(y => y.RightId == right.Id));

            if (roles == null && !roles.Any())
                return true;

            if (roles.Any(x => x.RoleName.ToUpper() == role.ToUpper()))
                return true;

            //if (right.Parent != null)
            //{
            //    if (await HasRight(role, right.Parent))
            //        return true; 
            //}
            return false;
        }

        public async Task<SignInResponse> SignIn(string userName, string password, bool isEncrypted)
        {
            string decryptedPassword = "";
            if (isEncrypted)
                decryptedPassword = EncryptionDecryption.DecryptStringAES(password);
            else
                decryptedPassword = password;

            var userid = await _repository.GetUserSignIn(userName, decryptedPassword);

            if (userid == null || userid <= 0)
                return new SignInResponse
                {
                    Result = SignInResult.LoginNameOrPasswordNotCorrect
                };


            var userrepoinfo = await _repository.GetUserInfo(userid.Value);
            var userinfo = UserMap.GetUserInfo(userrepoinfo);


            // get primary entity Access
            userinfo = await GetUserPrimaryEntityAccess(userinfo);


            var entityId = Int32.Parse(EncryptionDecryption.DecryptStringAES(userinfo.EntityId));
            var roles = await _repository.GetUserRole(userid.Value, entityId);
            var allrights = await _repository.GetUserRights(roles, entityId);

            //set external user email id
            switch (userinfo.UserType)
            {
                case UserTypeEnum.Customer:
                    {
                        userinfo.EmailAddress = await _customerRepository.GetCustomerContactEmailbyUserid(userinfo.Id) ?? "";
                        break;
                    }
                case UserTypeEnum.Supplier:
                    {
                        userinfo.EmailAddress = await _supplierRepo.GetSupplierContactEmailbyUserid(userinfo.Id) ?? "";
                        break;
                    }
                case UserTypeEnum.Factory:
                    {
                        userinfo.EmailAddress = await _supplierRepo.GetSupplierContactEmailbyUserid(userinfo.Id) ?? "";
                        break;
                    }
            }
            //filter the menu by customer . 
            if (userinfo.UserType == UserTypeEnum.Customer)
            {
                allrights = await FilterMenuBycustomer(allrights, userinfo.CustomerId);
            }
            userinfo.Roles = roles;
            userinfo.Rights = allrights;



            //get the service accessed for the user(currently supplier and factory
            userinfo = await GetUserServiceAccess(userinfo, EncryptionDecryption.DecryptStringAES(userinfo.EntityId));

            return new SignInResponse
            {
                User = userinfo,
                Result = SignInResult.Success
            };

        }
        private async Task<List<Right>> FilterMenuBycustomer(List<Right> rights, int customerid)
        {
            var itRightEntities = await _repository.GetItRightEntitesList();
            if (itRightEntities.Any())
            {
                var rightIds = itRightEntities.Where(x => x.CustomerId == customerid || x.CustomerId == null).Select(x => x.RightId.GetValueOrDefault()).ToList();

                if (rightIds.Any())
                {
                    rights = rights.Where(x => rightIds.Contains(x.Id)).ToList();
                }
            }
            return rights;
        }
        /// <summary>
        /// get the service accessed for the user(currently supplier and factory
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<User> GetUserServiceAccess(User user, string entityId)
        {
            switch (user.UserType)
            {
                case UserTypeEnum.InternalUser:
                    {
                        if (!string.IsNullOrEmpty(entityId))
                        {
                            user.ServiceAccess = await _repository.GetInternalUserServiceIds(user.StaffId, Int32.Parse(entityId));
                        }
                        break;
                    }
                case UserTypeEnum.OutSource:
                    {
                        if (!string.IsNullOrEmpty(entityId))
                        {
                            user.ServiceAccess = await _repository.GetInternalUserServiceIds(user.StaffId, Int32.Parse(entityId));
                        }
                        break;
                    }
                case UserTypeEnum.Supplier:
                    {
                        if (!string.IsNullOrEmpty(entityId))
                        {
                            user.ServiceAccess = await _supplierRepo.GetSupplierContactServiceIds(user.SupplierContactId.Value, Int32.Parse(entityId));
                        }
                        break;
                    }

                case UserTypeEnum.Factory:
                    {
                        if (!string.IsNullOrEmpty(entityId))
                        {
                            user.ServiceAccess = await _supplierRepo.GetSupplierContactServiceIds(user.FactoryContactId.Value, Int32.Parse(entityId));
                        }
                        break;
                    }
                case UserTypeEnum.Customer:
                    {
                        if (!string.IsNullOrEmpty(entityId))
                        {
                            user.ServiceAccess = await _customerRepository.GetCustomerContactServiceIds(user.CustomerContactId.Value, Int32.Parse(entityId));
                        }
                        break;
                    }
            }

            return user;
        }

        private async Task<User> GetUserPrimaryEntityAccess(User user)
        {
            switch (user.UserType)
            {
                case UserTypeEnum.InternalUser:
                    {
                        // set primary entity
                        var primaryEntity = await _repository.GetInternalUserPrimaryEntity(user.StaffId);
                        if (primaryEntity != null)
                        {
                            user.EntityId = EncryptionDecryption.EncryptStringAES(primaryEntity.Id.ToString());
                            user.EntityName = EncryptionDecryption.EncryptStringAES(primaryEntity.Name);
                        }
                        break;
                    }
                case UserTypeEnum.OutSource:
                    {
                        // set primary entity
                        var primaryEntity = await _repository.GetInternalUserPrimaryEntity(user.StaffId);
                        if (primaryEntity != null)
                        {
                            user.EntityId = EncryptionDecryption.EncryptStringAES(primaryEntity.Id.ToString());
                            user.EntityName = EncryptionDecryption.EncryptStringAES(primaryEntity.Name);
                        }
                        break;
                    }
                case UserTypeEnum.Supplier:
                    {
                        // set primary entity
                        var primaryEntity = await _supplierRepo.GetSupplierContactPrimaryEntity(user.SupplierContactId.Value);
                        if (primaryEntity != null)
                        {
                            user.EntityId = EncryptionDecryption.EncryptStringAES(primaryEntity.Id.ToString());
                            user.EntityName = EncryptionDecryption.EncryptStringAES(primaryEntity.Name);
                        }
                        break;
                    }

                case UserTypeEnum.Factory:
                    {
                        // set primary entity
                        var primaryEntity = await _supplierRepo.GetSupplierContactPrimaryEntity(user.FactoryContactId.Value);
                        if (primaryEntity != null)
                        {
                            user.EntityId = EncryptionDecryption.EncryptStringAES(primaryEntity.Id.ToString());
                            user.EntityName = EncryptionDecryption.EncryptStringAES(primaryEntity.Name);
                        }
                        break;
                    }
                case UserTypeEnum.Customer:
                    {
                        // set primary entity
                        var primaryEntity = await _customerRepository.GetCustomerContactPrimaryEntity(user.CustomerContactId.Value);
                        if (primaryEntity != null)
                        {
                            user.EntityId = EncryptionDecryption.EncryptStringAES(primaryEntity.Id.ToString());
                            user.EntityName = EncryptionDecryption.EncryptStringAES(primaryEntity.Name);
                        }
                        break;
                    }
            }

            return user;
        }

        public async Task<List<CommonDataSource>> GetUserEntityAccess(int userId)
        {
            List<CommonDataSource> dataSource = null;

            switch (_ApplicationContext.UserType)
            {
                case UserTypeEnum.InternalUser:
                    {
                        dataSource = await _repository.GetInternalUserEntityAccess(userId);
                        break;
                    }
                case UserTypeEnum.OutSource:
                    {
                        dataSource = await _repository.GetInternalUserEntityAccess(userId);
                        break;
                    }
                case UserTypeEnum.Supplier:
                    {
                        dataSource = await _repository.GetSupplierEntityAccess(userId);
                        break;
                    }
                case UserTypeEnum.Factory:
                    {
                        dataSource = await _repository.GetFactoryEntityAccess(userId);
                        break;
                    }
                case UserTypeEnum.Customer:
                    {
                        dataSource = await _repository.GetCustomerEntityAccess(userId);
                        break;
                    }
            }
            return dataSource;
        }

        public async Task<TaskResponse> GetTaskList()
        {
            var tasks = await _repository.GetTasks(_ApplicationContext.UserId, _ApplicationContext.RoleList, _ApplicationContext.LocationList);
            IEnumerable<InspTransaction> parent;

            if (tasks == null || !tasks.Any())
                return new TaskResponse { Result = TaskResult.NotFound };

            //this info using for split booking task which need to show the parent id.
            parent = await _repository.GetParentBookingId();

            return new TaskResponse
            {
                Data = tasks.Select(x => UserMap.GetTask(x, parent)),
                Result = TaskResult.Success
            };
        }

        public async Task<TaskResponse> TaskListData(TaskSearchRequest request)
        {
            var data = _repository.GetTaskData();

            data = data.Where(x => x.ReportTo == _ApplicationContext.UserId);
            if (request.IsUnread != null)
            {
                data = data.Where(x => x.IsDone == request.IsUnread);
            }
            if (request.TaskType != null)
            {
                data = data.Where(x => x.TaskType.Label.Contains(request.TaskType.ToString()));
            }

            var taskData = await data.OrderByDescending(x => x.CreatedOn).Skip(request.Skip).Take(10).Select(y => new TaskModel
            {
                Id = y.Id,
                LinkId = y.LinkId,
                StaffName = y.User.Staff.PersonName,
                Type = (TaskType)y.TaskTypeId,
                UserId = y.UserId,
                CreatedOn = y.CreatedOn,
                IsDone = y.IsDone,
                TypeLable = y.TaskType.Label,
                DateType = y.CreatedOn.Date == DateTime.Now.Date ? TaskDateType.Today : y.CreatedOn.Date == DateTime.Now.AddDays(-1).Date ? TaskDateType.Yesterday : TaskDateType.Older
            }).ToListAsync();

            if (taskData == null && !taskData.Any())
                return new TaskResponse { Result = TaskResult.NotFound };

            return new TaskResponse
            {
                Data = taskData,
                Result = TaskResult.Success
            };
        }

        public async Task<NotificationResponse> GetNotifications(NotificationSearchRequest request)
        {
            var data = _repository.GetNotificationData();

            data = data.Where(x => x.UserId == _ApplicationContext.UserId);
            if (request.IsUnread != null)
            {
                data = data.Where(x => x.IsRead == request.IsUnread);
            }
            if (request.NotificationType != null)
            {
                if (request.NotificationType == NotificationFilterType.Inspection)
                    data = data.Where(x => x.NotifType.Label.Contains(NotificationFilterType.Booking.ToString()) || x.NotifType.Label.Contains(NotificationFilterType.Inspection.ToString()));
                else
                    data = data.Where(x => x.NotifType.Label.Contains(request.NotificationType.ToString()));
            }

            var notificationData = await data.OrderByDescending(x => x.CreatedOn).Skip(request.Skip).Take(10).Select(y => new NotificationModel
            {
                Id = y.Id,
                LinkId = y.LinkId,
                Message = y.NotificationMessage,
                Type = (NotificationType)y.NotifTypeId,
                UserId = y.UserId,
                CreatedOn = y.CreatedOn,
                IsRead = y.IsRead,
                TypeLabel = y.NotifType.Label,
                DateType = y.CreatedOn.Date == DateTime.Now.Date ? NotificationDateType.Today : y.CreatedOn.Date == DateTime.Now.AddDays(-1).Date ? NotificationDateType.Yesterday : NotificationDateType.Older
            }).ToListAsync();

            if (notificationData == null && !notificationData.Any())
                return new NotificationResponse { Result = NotificationResult.NotFound };

            return new NotificationResponse
            {
                Data = notificationData,
                Result = NotificationResult.Success
            };
        }

        public async Task<TaskResponse> DoneTask(Guid Id)
        {
            var task = await _repository.GetSingleAsync<MidTask>(x => x.Id == Id);

            if (task == null)
                return new TaskResponse { Result = TaskResult.NotFound };

            task.IsDone = true;
            task.UpdatedOn = DateTime.Now;
            _repository.EditEntity(task);
            await _repository.Save();

            return new TaskResponse
            {
                Result = TaskResult.Success
            };
        }

        public async Task<NotificationResponse> ReadNotification(Guid Id)
        {
            var notification = await _repository.GetSingleAsync<MidNotification>(x => x.Id == Id);

            if (notification == null)
                return new NotificationResponse { Result = NotificationResult.NotFound };

            notification.IsRead = true;
            notification.UpdatedOn = DateTime.Now;
            _repository.EditEntity(notification);
            await _repository.Save();

            return new NotificationResponse
            {
                Result = NotificationResult.Success
            };
        }

        public async Task<TaskResponse> AllDoneTask()
        {
            var tasks = await _repository.GetListAsync<MidTask>(x => x.ReportTo == _ApplicationContext.UserId && !x.IsDone);

            if (tasks == null && !tasks.Any())
                return new TaskResponse { Result = TaskResult.NotFound };

            var lsttask = new List<MidTask>();
            foreach (var task in tasks)
            {
                task.IsDone = true;
                task.UpdatedOn = DateTime.Now;
                lsttask.Add(task);
            }
            if (lsttask.Count() > 0)
                _repository.EditEntities(lsttask);

            await _repository.Save();

            return new TaskResponse
            {
                Result = TaskResult.Success
            };
        }

        public async Task<NotificationResponse> ReadAllNotification()
        {
            var notifications = await _repository.GetListAsync<MidNotification>(x => x.UserId == _ApplicationContext.UserId && !x.IsRead);

            if (notifications == null && !notifications.Any())
                return new NotificationResponse { Result = NotificationResult.NotFound };

            var lstnotification = new List<MidNotification>();
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
                notification.UpdatedOn = DateTime.Now;
                lstnotification.Add(notification);
            }
            if (lstnotification.Count() > 0)
                _repository.EditEntities(lstnotification);

            await _repository.Save();

            return new NotificationResponse
            {
                Result = NotificationResult.Success
            };
        }

        public async Task<NotDoneTaskResponse> NotDoneTaskCount(TaskSearchRequest request)
        {
            var data = _repository.GetTaskData();

            data = data.Where(x => x.ReportTo == _ApplicationContext.UserId && !x.IsDone);

            if (request.TaskType != null)
            {
                data = data.Where(x => x.TaskType.Label.Contains(request.TaskType.ToString()));
            }

            return new NotDoneTaskResponse
            {
                TodayCount = await data.CountAsync(y => y.CreatedOn.Date == DateTime.Now.Date),
                YesterdayCount = await data.CountAsync(y => y.CreatedOn.Date == DateTime.Now.AddDays(-1).Date),
                OlderCount = await data.CountAsync(y => y.CreatedOn.Date != DateTime.Now.Date && y.CreatedOn.Date != DateTime.Now.AddDays(-1).Date),
            };
        }

        public async Task<UnReadNotificationResponse> UnReadNotificationCount(NotificationSearchRequest request)
        {
            var data = _repository.GetNotificationData();

            data = data.Where(x => x.UserId == _ApplicationContext.UserId && !x.IsRead);

            if (request.NotificationType != null)
            {
                if (request.NotificationType == NotificationFilterType.Inspection)
                    data = data.Where(x => x.NotifType.Label.Contains(NotificationFilterType.Booking.ToString()) || x.NotifType.Label.Contains(NotificationFilterType.Inspection.ToString()));
                else
                    data = data.Where(x => x.NotifType.Label.Contains(request.NotificationType.ToString()));
            }

            return new UnReadNotificationResponse
            {
                TodayCount = await data.CountAsync(y => y.CreatedOn.Date == DateTime.Now.Date),
                YesterdayCount = await data.CountAsync(y => y.CreatedOn.Date == DateTime.Now.AddDays(-1).Date),
                OlderCount = await data.CountAsync(y => y.CreatedOn.Date != DateTime.Now.Date && y.CreatedOn.Date != DateTime.Now.AddDays(-1).Date),
            };
        }

        public async Task<IEnumerable<User>> GetUserListByRole(int roleId, int officeId)
        {
            var data = await _repository.GetUserListByRole(officeId, roleId);

            if (data == null)
                return null;

            return data.Select(x => UserMap.GetUserModel(x, null, null));

        }

        /// <summary>
        /// get user email by role and office
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="officeId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetUserEmailbyRoleAndOffice(int roleId, int officeId)
        {
            var data = await _repository.GetUserEmailByRoleAndOffice(officeId, roleId);
            return data;
        }

        public async Task<IEnumerable<User>> GetExternalUserListByRole(int roleId, int userid, UserTypeEnum usertype)
        {
            var data = await _repository.GetExternalUserListByRole(roleId, userid, usertype);

            if (data == null)
                return null;

            return data.Select(x => UserMap.GetUserModel(x, null, null));

        }

        public async Task<MidTask> EditTask(Expression<Func<MidTask, bool>> predicate, Action<MidTask> voidUpdate)
        {
            var task = await _repository.GetTask(predicate);

            if (task != null)
            {
                voidUpdate(task);

                _repository.Save(task);
            }

            return task;
        }

        public async Task<MidNotification> EditNotification(Expression<Func<MidNotification, bool>> predicate, Action<MidNotification> voidUpdate)
        {
            var notification = await _repository.GetNotification(predicate);

            if (notification != null)
                voidUpdate(notification);

            _repository.Save(notification);

            return notification;
        }

        public async Task<ChangePasswordResponse> ChangePassword(ChangePasswordRequest request)
        {
            var response = new ChangePasswordResponse();
            var oldPasswordExists = _repository.CheckOldPasswordExists(_ApplicationContext.UserId, request.CurrentPassword);
            if (oldPasswordExists)
            {
                if (request.NewPassword != request.ConfirmPassword)
                    return new ChangePasswordResponse() { Result = ChangePasswordResult.PasswordNotMatch, Id = 0 };
                //if (ValidatePassword(request.NewPassword))
                //{
                var id = await _repository.UpdatePassword(_ApplicationContext.UserId, request.NewPassword, request.CurrentPassword);
                return new ChangePasswordResponse() { Result = ChangePasswordResult.Success, Id = id };
                //}
                //else
                //{
                //    return new ChangePasswordResponse() { Result = ChangePasswordResult.Success, Id = 0 };
                //}
            }
            else
            {
                response = new ChangePasswordResponse() { Result = ChangePasswordResult.CurrentPasswordNotMatch, Id = 0 };
            }
            return response;
        }


        private bool ValidatePassword(string password)
        {
            var input = password;

            if (string.IsNullOrWhiteSpace(input))
            {
                throw new Exception("Password should not be empty");
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasMiniMaxChars = new Regex(@".{6,25}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!$%^&*()=;',.?/~`{}|]");

            if (!hasLowerChar.IsMatch(input))
            {
                return false;
            }
            else if (!hasMiniMaxChars.IsMatch(input))
            {
                return false;
            }
            else if (!hasNumber.IsMatch(input))
            {
                return false;
            }

            else if (hasSymbols.IsMatch(input))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public void SetCacheEntities()
        {
            var data = _cache.CacheTryGetValueSet(CacheKeys.Instances, () => _repository.GetEntities());
            data.Wait();
        }

        public void SetRightsCaches()
        {
            var data = _cache.CacheTryGetValueSet(CacheKeys.AllRoles, () => _repository.GetRoles());
            data.Wait();
        }
        public async Task<IEnumerable<User>> GetUserListByRoleOfficeServiceCustomer(UserAccess userAccess)
        {
            //get user list based on role,office,service, product category
            IEnumerable<User> userListData = await _repository.GetUserListByRoleOfficeService(userAccess);
            return userListData;
        }

        public async Task<IEnumerable<User>> GetUserListByRoleOffice(UserAccess userAccess)
        {
            //get user list based on role,office,service, product category
            IEnumerable<User> userListData = await _repository.GetUserListByRoleOffice(userAccess);

            return userListData;
        }
        /// <summary>
        /// GetCustomerServiceBookingContacts
        /// </summary>
        /// <param name="userAccessFilter"></param>
        /// <returns></returns>
        public async Task<IEnumerable<UserStaffDetails>> GetCustomerServiceBookingContacts(UserAccess userAccessFilter)
        {

            List<UserStaffDetails> userList = await _repository.GetCsEnabledBookingContacts(userAccessFilter);

            userList = userList.Distinct().ToList();

            return userList;
        }

        /// <summary>
        /// GetUserListByReportCheckerProfile
        /// </summary>
        /// <param name="userAccessFilter"></param>
        /// <returns></returns>
        public async Task<IEnumerable<UserStaffDetails>> GetUserListByReportCheckerProfile(UserAccess userAccessFilter)
        {

            var userList = await _repository.GetUserListByReportCheckerProfile(userAccessFilter);

            userList = userList.Distinct().ToList();

            return userList;
        }

        /// <summary>
        /// GetCSBookingContact based on customer, office, service, role
        /// </summary>
        /// <param name="userAccessFilter"></param>
        /// <returns></returns>
        public async Task<IEnumerable<UserStaffDetails>> GetCSBookingContact(UserAccess userAccessFilter)
        {

            var userList = await _repository.GetCSBookingContacts(userAccessFilter);

            userList = userList.Distinct().ToList();

            return userList;
        }

        public async Task<IEnumerable<AEDetails>> GetAEbyCustomer(List<int> lstcustomerId)
        {
            return await _repository.GetAEByCustomer(lstcustomerId);
        }

        //get AE list based on customer list and location list
        public async Task<IEnumerable<AEDetails>> GetAEByCustomerAndLocation(IEnumerable<int> locationIds, IEnumerable<int> customerIds)
        {
            return await _repository.GetAEByCustomerAndLocation(locationIds, customerIds);
        }

        public async Task<IEnumerable<int>> GetUsersIdListByCustomer(int customerid)
        {
            return await _repository.GetUsersIdListByCustomer(customerid);
        }

        //get userlist who has role access
        public async Task<IEnumerable<UserStaffDetails>> GetRoleAccessUserList(int roleId)
        {
            return await _repository.GetRoleAccessUserList(roleId);
        }

        //Get task list based on id, tasktype id, isdone value
        public async Task<IEnumerable<MidTask>> GetTask(int linkId, IEnumerable<int> typeIdList, bool isDone)
        {
            return await _repository.GetTaskList(linkId, typeIdList, isDone);
        }

        //update task(mid_task table) 
        public async Task<IEnumerable<MidTask>> UpdateTask(int bookingId, IEnumerable<int> typeIdList, bool oldTaskDoneValue)
        {
            IEnumerable<MidTask> getTasks = await GetTask(bookingId, typeIdList, oldTaskDoneValue);
            foreach (var task in getTasks)
            {
                if (task != null)
                {
                    task.IsDone = !oldTaskDoneValue;
                    task.UpdatedBy = _ApplicationContext?.UserId;
                    task.UpdatedOn = DateTime.Now;
                }
            }
            _repository.SaveList(getTasks);
            return getTasks;
        }

        //Create Task
        public async Task AddTask(TaskType taskType, int id, IEnumerable<int> userIdList)
        {
            if (userIdList != null && userIdList.Any())
            {
                foreach (int userId in userIdList.Where(x => x > 0))
                {
                    // Add new task for user request
                    var task = new MidTask
                    {
                        Id = Guid.NewGuid(),
                        LinkId = id,
                        UserId = _ApplicationContext.UserId,
                        IsDone = false,
                        TaskTypeId = (int)taskType,
                        ReportTo = userId,
                        CreatedBy = _ApplicationContext?.UserId,
                        CreatedOn = DateTime.Now,
                        EntityId = _filterService.GetCompanyId()
                    };
                    _repository.AddEntity(task);
                }
                //Save
                await _repository.Save();
            }
        }

        public async Task AddTask(TaskType taskType, int id, IEnumerable<int> userIdList, int commomUserId)
        {
            if (userIdList != null && userIdList.Any())
            {
                foreach (int userId in userIdList.Where(x => x > 0))
                {
                    // Add new task for user request
                    var task = new MidTask
                    {
                        Id = Guid.NewGuid(),
                        LinkId = id,
                        UserId = commomUserId,
                        IsDone = false,
                        TaskTypeId = (int)taskType,
                        ReportTo = userId,
                        CreatedBy = commomUserId,
                        CreatedOn = DateTime.Now,
                        EntityId = _filterService.GetCompanyId()
                    };
                    _repository.AddEntity(task);
                }
            }
        }

        /// <summary>
        /// Save Login Log details
        /// </summary>
        /// <param name="locationDetails"></param>
        /// <returns></returns>
        public async Task<int> SaveLoginLog(DeviceLocation locationDetails)
        {
            if (locationDetails != null)
            {
                var LoginLogDetails = new ItLoginLog()
                {
                    LogInTime = locationDetails.LoginTime,
                    IpAddress = locationDetails.IpAddress,
                    BrowserType = locationDetails.BrowserName,
                    DeviceType = locationDetails.DeviceType,
                    UserItId = locationDetails.UserItId,
                    LogOutTime = locationDetails.LogoutTime
                };

                _repository.AddEntity(LoginLogDetails);
                await _repository.Save();
                return 1;
            }
            return 0;

        }

        /// <summary>
        /// get user information by user and entity
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="currentEntity"></param>
        /// <returns></returns>
        public async Task<UserEntityAccess> GetUserEntityRoleAccess(int userId, string currentEntity)
        {
            var roles = await _repository.GetUserRole(userId, _filterService.GetCompanyId());
            var allrights = await _repository.GetUserRights(roles, _filterService.GetCompanyId());
            //filter by customer id
            if (_ApplicationContext.UserType == UserTypeEnum.Customer)
            {
                allrights = await FilterMenuBycustomer(allrights, _ApplicationContext.CustomerId);
            }
            var userinfo = await _repository.GetUserInfo(userId);
            //get the service accessed for the user
            userinfo = await GetUserServiceAccess(userinfo, EncryptionDecryption.DecryptStringAES(currentEntity));

            return new UserEntityAccess
            {
                Id = userId,
                ServiceAccess = userinfo.ServiceAccess,
                Roles = roles,
                Rights = allrights,
                Result = UserEntityAccessResult.Success
            };
        }

        public async Task<IEnumerable<string>> GetUserEmailbyRoleAndOfficeList(int roleId, IEnumerable<int> officeIds)
        {
            var data = await _repository.GetUserEmailByRoleAndOfficeList(officeIds, roleId);
            return data;
        }

        /// <summary>
        /// Get the user applicant details
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<CommonDataSource>> GetUserApplicantDetails(int userId)
        {
            List<CommonDataSource> dataSource = null;

            switch (_ApplicationContext.UserType)
            {
                case UserTypeEnum.InternalUser:
                    {
                        dataSource = await _repository.GetInternalUserEntityAccess(userId);
                        break;
                    }
                case UserTypeEnum.OutSource:
                    {
                        dataSource = await _repository.GetInternalUserEntityAccess(userId);
                        break;
                    }
                case UserTypeEnum.Supplier:
                    {
                        dataSource = await _repository.GetSupplierEntityAccess(userId);
                        break;
                    }
                case UserTypeEnum.Factory:
                    {
                        dataSource = await _repository.GetFactoryEntityAccess(userId);
                        break;
                    }
                case UserTypeEnum.Customer:
                    {
                        dataSource = await _repository.GetCustomerEntityAccess(userId);
                        break;
                    }
            }
            return dataSource;
        }

        public async Task<ResetPasswordResponse> ResetPassword(ResetPasswordRequest request)
        {
            var username = EncryptionDecryption.DecryptStringAES(request.UserName);
            var userId = (await _repository.GetSingleAsync<ItUserMaster>(x => x.Active && x.LoginName == username))?.Id;
            if (userId != null)
            {
                if (request.NewPassword != request.ConfirmPassword)
                    return new ResetPasswordResponse() { Result = ResetPasswordResult.PasswordNotMatch, Id = 0 };
                var id = await _repository.ResetPassword(userId.Value, request.NewPassword);
                return new ResetPasswordResponse() { Result = ResetPasswordResult.Success, Id = id };

            }
            else
            {
                return new ResetPasswordResponse() { Result = ResetPasswordResult.PasswordNotSaved, Id = 0 };
            }
        }

        public async Task<RightTypeResponse> GetRightTypeList()
        {
            var rightTypeList = await _repository.GetRightTypeList();

            if (rightTypeList == null || !rightTypeList.Any())
            {
                return new RightTypeResponse { Result = RightTypeResult.NoDataFound };
            }

            return new RightTypeResponse
            {
                RightTypeList = rightTypeList,
                Result = RightTypeResult.Success
            };
        }
    }
}

