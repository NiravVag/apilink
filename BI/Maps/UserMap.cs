using DTO.Common;
using DTO.CommonClass;
using DTO.User;
using Entities;
using Entities.Enums;
using System.Collections.Generic;
using System.Linq;

namespace BI.Maps
{
    public class UserMap : ApiCommonData
    {

        public User GetUserModel(ItUserMaster user, IEnumerable<ItRole> roleList, IEnumerable<ItRight> rightList)
        {
            if (user == null)
                return null;

            var userInfo = new User
            {
                Id = user.Id,
                Active = user.Active,
                FullName = user.FullName,
                LoginName = user.LoginName,
                Roles = roleList?.Select(GetRoleModel),
                Rights = rightList?.Select(GetRightModel),
                StatusId = user.StatusId,
                //EntityName = user.PrimaryEntityNavigation?.Name,
                // EntityId = user.PrimaryEntity == null ? 0 : user.PrimaryEntity.Value,
                CountryId = user.Staff?.NationalityCountryId,
                CustomerId = user.CustomerId == null ? 0 : user.CustomerId.Value,
                SupplierId = user.SupplierId == null ? 0 : user.SupplierId.Value,
                FactoryId = user.FactoryId == null ? 0 : user.FactoryId.Value,
                UserType = (UserTypeEnum)user.UserTypeId,
                UserProfileList = user?.Staff?.HrStaffProfiles?.Select(x => x.ProfileId),
                StaffId = user.StaffId == null ? 0 : user.StaffId.Value,
                LocationList = user?.Staff?.HrOfficeControls?.Select(x => x.LocationId),
                LocationId = user.Staff == null || user.Staff.LocationId == null ? 0 : user.Staff.LocationId.Value,
                ChangePassword = user.ChangePassword == null ? false : user.ChangePassword,
                EmailAddress = user.Staff?.CompanyEmail,
                ProfileImageUrl = user.FileUrl,
                CustomerContactId = user.CustomerContactId,
                SupplierContactId = user.SupplierContactId
            };

            if (user.Staff != null)
                userInfo.LocationId = user.Staff.LocationId == null ? 0 : user.Staff.LocationId.Value;

            return userInfo;

        }

        public Role GetRoleModel(ItRole role)
        {
            if (role == null)
                return null;

            return new Role
            {
                Id = role.Id,
                Active = role.Active,
                RoleName = role.RoleName,
            };

        }

        public TaskModel GetTask(MidTask entity, IEnumerable<InspTransaction> parent)
        {
            if (entity == null)
                return null;

            return new TaskModel
            {
                Id = entity.Id,
                LinkId = entity.LinkId,
                StaffName = entity.User?.Staff?.PersonName,
                Type = (TaskType)entity.TaskTypeId,
                UserId = entity.UserId,
                ParentId = parent.Where(x => x.Id == entity.LinkId).Select(x => x.SplitPreviousBookingNo.Value).FirstOrDefault()
            };
        }

        public NotificationModel GetNotification(MidNotification entity)
        {
            if (entity == null)
                return null;
            return new NotificationModel
            {
                Id = entity.Id,
                LinkId = entity.LinkId,
                Message = entity.NotificationMessage,
                Type = (NotificationType)entity.NotifTypeId,
                UserId = entity.UserId
            };
        }

        //private  Right[] GetOrganizedRights(ref IEnumerable<ItRight> rightList, Right rightParent = null)
        //{
        //    int? parentId = rightParent?.Id;
        //    var headers = rightList.Where(x => x.ParentId == parentId).Select( x=> GetRightModel(x, rightParent)).ToArray();

        //    foreach (var header in headers)
        //    {
        //        var items = GetOrganizedRights(ref rightList, header);
        //        header.Children = items;
        //    }

        //    return headers.ToArray();
        //}

        public Right GetRightModel(ItRight right)
        {
            if (right == null)
                return null;

            return new Right
            {
                Active = right.Active,
                Glyphicons = right.Glyphicons,
                Id = right.Id,
                IsHeading = right.IsHeading,
                MenuName = right.MenuNameIdTran.GetTranslation(right.MenuName),
                TitleName = right.TitleNameIdTran.GetTranslation(right.TitleName),
                ParentId = right.ParentId,
                Path = right.Path,
                Ranking = right.Ranking,
                ShowMenu = right.ShowMenu != null && right.ShowMenu.Value,
                RightType = right.RightType
            };


        }
        public CommonDataSource RoleMap(ItRole entity)
        {
            return new CommonDataSource()
            {
                Id = entity.Id,
                Name = entity.RoleName
            };
        }
        public User GetUserInfo(User userinfo)
        {
            return new User()
            {
                ChangePassword = userinfo.ChangePassword ?? false,
                CountryId = userinfo.CountryId ?? 0,
                CustomerContactId = userinfo.CustomerContactId ?? 0,
                CustomerId = userinfo.CustomerId,
                EmailAddress = userinfo.EmailAddress ?? "",
                EntityId = userinfo.EntityId,
                EntityName = userinfo.EntityName ?? "",
                FactoryId = userinfo.FactoryId,
                FullName = userinfo.FullName,
                Id = userinfo.Id,
                LocationId = userinfo.LocationId,
                LocationList = userinfo.LocationList,
                LoginName = userinfo.LoginName,
                ProfileImageUrl = userinfo.ProfileImageUrl ?? "",
                StaffId = userinfo.StaffId,
                StatusId = userinfo.StatusId,
                SupplierContactId = userinfo.SupplierContactId ?? 0,
                FactoryContactId = userinfo.FactoryContactId ?? 0,
                SupplierId = userinfo.SupplierId,
                UserProfileList = userinfo.UserProfileList,
                UserType = (UserTypeEnum)userinfo.UserType,
                FbUserId = userinfo.FbUserId
            };
        }
    }
}
