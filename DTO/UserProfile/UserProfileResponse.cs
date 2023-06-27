using DTO.UserConfig;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.UserProfile
{
    public class UserProfileResponse
    {
        public UserProfile Data { get; set; }
        public UserProfileResult Result { get; set; }
    }

    public class UserProfile
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string EmailId { get; set; }
        public string Phone { get; set; }
        public string ProfileImageUrl { get; set; }
        public string ProfileImageName { get; set; }
    }

    public enum UserProfileResult
    {
        Success = 1,
        Fail = 2,
        NoData = 3,
        InvalidUserId = 4
    }

    public enum UserProfileResponseResult
    {
        Success = 1,
        Faliure = 2,
        RequestNotCorrectFormat = 3,
        Error = 4,
        NotFound = 5,
        AlreadyExists = 6
    }

    public class UserProfileSaveResponse
    {
        public int Id { get; set; }
        public UserProfileResponseResult Result { get; set; }
    }
}
