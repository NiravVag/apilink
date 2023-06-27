using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.User
{
    public class ChangePasswordResponse
    {
        public int Id { get; set; }
        public ChangePasswordResult Result { get; set; }
    }

    public enum ChangePasswordResult
    {
        Success = 1,
        PasswordNotSaved = 2,
        CurrentPasswordNotMatch=3,
        PasswordNotMatch = 4,
        InvalidPassword=5
    }
}
