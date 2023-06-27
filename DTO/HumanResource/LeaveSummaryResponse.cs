using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.HumanResource
{
    public class LeaveSummaryResponse
    {
        public IEnumerable<LeaveType> LeaveTypeList { get; set;  }

        public IEnumerable<LeaveStatus> LeaveStatusList { get; set;  } 

        public IEnumerable<HolidayDayType> DayTypeList { get; set;  }

        public LeaveSummaryResult Result { get; set;  }

    }

    public enum LeaveSummaryResult
    {
        Success = 1, 
        CannotFindStatuses = 2, 
        CannotFindTypes = 3,
        CannotFinddayTypeList = 4
    }


}
