using System;
using System.Collections.Generic;
using System.Text;
using DTO.User;

namespace DTO.UserAccount
{
    public class EditUserAccountResponse
    {
        public IEnumerable<UserAccountItem> UserAccountList { get; set; }
        public IEnumerable<Role> RoleList { get; set; }
        public List<Contact> ContactList { get; set; }

        public string RoleName { get; set; }      
        public string Name { get; set; }

        public int TotalCount { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public EditUserAccountResult Result { get; set; }
    }

    public enum EditUserAccountResult
    {
        Success = 1,
        CanNotGetUserAccountList = 2,
        CanNotGetRoleList = 3
    }

    public class Contact
    {
        public int? ContactId { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
    }
}
