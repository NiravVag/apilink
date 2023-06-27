using DTO.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.RoleRight
{
    public class RoleRightSearchResponse
    {
        public IEnumerable<RightTreeView> RightList { get; set; }
        public int RoleId { get; set; }
        public RoleRightSearchResult Result { get; set; }
    }

    public enum RoleRightSearchResult
    {
        Success = 1,
        CannotGetListRight = 2
    }
}
