using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.UserAccount
{
    public class DeleteUserAccountResponse
    {
        public int Id { get; set; }
        public DeleteResult Result { get; set; }
    }
    public enum DeleteResult
    {
        Success = 1,
        NotFound = 2,
        CannotDelete = 3
    }
}
