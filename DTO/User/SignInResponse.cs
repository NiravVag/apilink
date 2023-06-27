using Entities.Enums;
using System;
using System.Collections.Generic;

namespace DTO.User
{
    public class SignInResponse
    {
        public SignInResult Result { get; set; }
        public User User { get; set; }
        public string Token { get; set; }
    }
    public enum SignInResult
    {
        Success = 1,
        LoginEmpty = 2,
        PasswordEmpty = 4,
        LoginNameOrPasswordNotCorrect = 5,
        UserIsDisabled = 6,
        Other = 7
    }
    public class User
    {
        public int Id { get; set; }

        public string LoginName { get; set; }

        public string FullName { get; set; }

        public bool Active { get; set; }

        public short StatusId { get; set; }

        public string EntityId { get; set; }

        public string EntityName { get; set; }

        public int LocationId { get; set; }

        public int CustomerId { get; set; }

        public int SupplierId { get; set; }

        public int FactoryId { get; set; }

        public int StaffId { get; set; }

        public bool? ChangePassword { get; set; }

        public string EmailAddress { get; set; }

        public UserTypeEnum UserType { get; set; }

        public IEnumerable<int> UserProfileList { get; set; }

        public IEnumerable<Role> Roles { get; set; }

        public IEnumerable<Right> Rights { get; set; }

        public int? CountryId { get; set; }

        public IEnumerable<int> LocationList { get; set; }

        public bool? isLabTestingAccess { get; set; }

        public bool? isTCFAccess { get; set; }

        public bool? isAuditAccess { get; set; }

        public bool? isInspectionAccess { get; set; }

        public int? CustomerContactId { get; set; }

        public int? SupplierContactId { get; set; }

        public int? FactoryContactId { get; set; }

        public string ProfileImageUrl { get; set; }

        public string OnsiteEmail { get; set; }

        public IEnumerable<int> ServiceAccess { get; set; }

        public int? FbUserId { get; set; }
    }

    public class UserRepo
    {
        public int Id { get; set; }

        public string LoginName { get; set; }

        public string FullName { get; set; }

        public short StatusId { get; set; }

        public int? EntityId { get; set; }

        public string EntityName { get; set; }

        public int? LocationId { get; set; }

        public int? CustomerId { get; set; }

        public int? SupplierId { get; set; }

        public int? FactoryId { get; set; }

        public int? StaffId { get; set; }

        public bool? ChangePassword { get; set; }

        public string EmailAddress { get; set; }

        public int UserType { get; set; }

        public IEnumerable<int> UserProfileList { get; set; }

        public IEnumerable<Role> Roles { get; set; }

        public IEnumerable<Right> Rights { get; set; }

        public int? CountryId { get; set; }

        public IEnumerable<int> LocationList { get; set; }

        public int? CustomerContactId { get; set; }

        public int? SupplierContactId { get; set; }

        public string ProfileImageUrl { get; set; }

    }


    public class Role
    {
        public int Id { get; set; }

        public string RoleName { get; set; }

        public bool? Active { get; set; }

    }

    public class Right
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string TitleName { get; set; }

        public string MenuName { get; set; }

        public string Path { get; set; }

        public bool? IsHeading { get; set; }

        public bool? Active { get; set; }

        public string Glyphicons { get; set; }

        public int? Ranking { get; set; }

        public bool ShowMenu { get; set; }

        public int? RightType { get; set; }

        public IEnumerable<Right> Children { get; set; }
        public int? RightTypeService { get; set; }
    }

    public class DeviceLocation
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string IpAddress { get; set; }
        public string DeviceType { get; set; }
        public string OsVersion { get; set; }
        public string BrowserName { get; set; }
        public DateTime? LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public int UserItId { get; set; }
    }

    public class UserEntityAccess
    {
        public int Id { get; set; }
        public IEnumerable<Role> Roles { get; set; }
        public IEnumerable<Right> Rights { get; set; }
        public IEnumerable<int> ServiceAccess { get; set; }
        public UserEntityAccessResult Result { get; set; }
    }

    public class UserEntityAccessRequest
    {
        public int Id { get; set; }
        public string EntityId { get; set; }
    }

    public enum UserEntityAccessResult
    {
        Success = 1,
        Failure = 2
    }
}
