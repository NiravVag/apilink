using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.UserProfile
{
    public class UserProfileRequest
    {
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public string EmailId { get; set; }
        public string Phone { get; set; }
        public string ProfileImageName { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}
