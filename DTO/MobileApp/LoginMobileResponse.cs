using DTO.User;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.MobileApp
{
    public class LoginMobileResponse
    {
        public LoginMobileItem data { get; set; }
        public MobileResult meta { get; set; }
    }

    public class LoginMobileItem
    {
        public signInResult Result { get; set; }

        public MobileUser User { get; set; }

        public string Token { get; set; }
    }

    public enum signInResult
    {
        Success = 1,
        LoginEmpty = 2,
        PasswordEmpty = 4,
        LoginNameOrPasswordNotCorrect = 5,
        UserIsDisabled = 6,
        Other = 7
    }

    public class MobileUser
    {
        public int id { get; set; }
        public string fullName { get; set; }
        public int officeLocationId { get; set; }
        public int customerId { get; set; }
        public bool? changePassword { get; set; }
        public string emailAddress { get; set; }
        public UserTypeEnum userType { get; set; }
        public IEnumerable<Role> roles { get; set; }
        public string userImageUrl { get; set; }
    }
}

