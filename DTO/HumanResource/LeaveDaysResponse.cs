using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.HumanResource
{
    public class LeaveDaysResponse
    {
        public LeaveDaysResult Result { get; set;  }

        public double Days { get; set;  }

    }

    public enum LeaveDaysResult
    {
        Success = 1, 
        Error = 2
    }
}
