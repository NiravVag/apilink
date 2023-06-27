using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.User
{
    public class UserCredentialsMailTemplateResponse
    {
        public UserCredentialsMailTemplate UserCredentialsMailTemplate { get; set; }
        public UserCredentialsMailTemplateResult Result { get; set; }
    }
    public class UserCredentialsMailTemplate
    {
        public string ContactName { get; set; }
        public string Name { get; set; }
        public string BaseUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
    }
    public enum UserCredentialsMailTemplateResult
    {
        Success = 1,
        DataNotFound = 2
    }
}
