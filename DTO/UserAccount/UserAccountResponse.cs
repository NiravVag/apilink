using System;
using System.Collections.Generic;
using System.Text;
using DTO.Location;
using Entities.Enums;
namespace DTO.UserAccount
{
    public class UserAccountResponse
    {
        public IEnumerable<Country> CountryList { get; set; }
        public UserAccountResult Result { get; set; }
    }

    public enum UserAccountResult
    {
        Success = 1,
        CannotGetListCountry = 2
    }

    public class UserFBToken
    {
        public string Token { get; set; }
        public string ReportUrl { get; set; }
        public string ReportsUrl { get; set; }
        public string MissionUrl { get; set; }
        public UserAccountFBResult Result { get; set; }
    }

    public class UserTCFToken
    {
        public string Token { get; set; }
        public string Expires_In { get; set; }
        public int UserId { get; set; }
    }

    public enum UserAccountFBResult
    {
        Success = 1,
        Failure = 2
    }

    public class FBUserDetails
    {
        public string FBUserId { get; set; }
        public UserAccountFBResult Result { get; set; }
    }

    public enum FBUserDetailsResult
    {
        Success = 1,
        Failure = 2
    }
}
