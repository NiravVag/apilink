using DTO.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.RoleRight
{
    public class RoleRightResponse
    {
        public IEnumerable<Role> RoleList { get; set; }
        public RoleRightResult Result { get; set; }
    }

    public enum RoleRightResult
    {
        Success = 1,
        CannotGetListRole = 2
    }
}
