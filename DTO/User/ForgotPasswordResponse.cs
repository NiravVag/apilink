using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.User
{
    public class ForgotPasswordResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }

        public string Link { get; set; }
        public ForgotPasswordResult Result { get; set; }
        public string EmailId { get; set; }
    }

    public enum ForgotPasswordResult
    {
        Success = 1,
        CurrentUserAccountNotFound = 2
    }
}
