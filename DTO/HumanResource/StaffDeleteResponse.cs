using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.HumanResource
{
    public class StaffDeleteResponse
    {
        public int Id { get; set;  }

        public StaffDeleteResult Result { get; set;  }

    }

    public enum StaffDeleteResult
    {
        Success = 1, 
        StaffNotFound  = 2,
        UserAccountDeleteError = 3,
        Other = 4
    }
}
