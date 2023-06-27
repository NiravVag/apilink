using System.Collections.Generic;

namespace DTO.RoleRight
{
    public class RoleRightRequest
    {
        public int RoleId { get; set; }

        public List<int> RightIdList { get; set; }
    }
}
