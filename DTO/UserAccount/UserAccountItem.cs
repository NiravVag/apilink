using System.Collections.Generic;

namespace DTO.UserAccount
{
    public class UserAccountItem
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Status { get; set; }
        public List<int> Roles { get; set; }
        public int? UserTypeId { get; set; }
        public string RoleName { get; set; }
        public string Fullname { get; set; }
        public int? UserId { get; set; }
        public int Contact { get; set; }
        public int? PrimaryEntity { get; set; }
        public List<int> ApiServiceIds { get; set; }
        public List<Contact> ContactList { get; set; }
        public List<UserRoleEntity> UserRoleEntityList { get; set; }

        public int? CreatedBy { get; set; }
    }

    public class UserRoleEntity
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public List<int> RoleEntity { get; set; }
    }
}
