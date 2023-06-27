using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.RoleRight
{
    public class SaveRoleRightResponse
    {
        public int Id { get; set; }
        public SaveRoleRightResult Result { get; set; }
    }
    public enum SaveRoleRightResult
    {
        Success = 1,
        CannotSaveRoleRight = 2
    }
}
