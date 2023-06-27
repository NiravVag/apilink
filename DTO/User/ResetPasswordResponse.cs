using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.User
{
    public class ResetPasswordResponse
    {
        public int Id { get; set; }
        public ResetPasswordResult Result { get; set; }
    }
    public enum ResetPasswordResult
    {
        Success = 1,
        PasswordNotSaved = 2,
        PasswordNotMatch = 3,
        InvalidPassword = 4
    }
}
