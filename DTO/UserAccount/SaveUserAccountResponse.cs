using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.UserAccount
{
    public class SaveUserAccountResponse
    {
        public int Id { get; set; }
        public List<int> ServiceIds { get; set; }
        public SaveResult Result { get; set; }
    }
    public enum SaveResult
    {
        Success = 1,
        CannotSaveUserAccount = 2,
        CurrentUserAccountNotFound = 3,
        CannotMapRequestToEntites = 4,
        DuplicateName = 5,
        RequestNotCorrectFormat = 6,
        Failure = 7
        
    }

    public class SaveUserResponse
    {
        public int Id { get; set; }
        public SaveResult Result { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool? ChangePassword { get; set; }
        public List<string> errors { get; set; }
        public SaveUserResponse()
        {
            this.errors = new List<string>();
        }
    }
}
