using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.HumanResource
{
    public class LeaveResponse
    {
        public IEnumerable<LeaveType> LeaveTypeList { get; set;  }

        public LeaveRequest LeaveRequest { get; set;  }

        public IEnumerable<HolidayDayType> DayTypeList { get; set; }

        public LeaveResult Result { get; set;  }

        public bool CanEdit { get; set;  }

        public bool CanCancel { get; set; }

        public bool CanApprove { get; set;  }
    }

    public enum LeaveResult
    {
        Success = 1, 
        CannotFindTypes = 2, 
        CannotFindLeaveRequest = 3,
        CannotFinddayTypeList = 4,
        CannotShowLeaveRequest = 5
    }

    public class LeaveInfo
    {
        public int StaffId { get; set; }

        public string StaffName { get; set; }

        public string Remarks { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        public string LeaveTypeName { get; set; }

        public string LeaveStatus { get; set; }
    }

    public class StaffLeaveInfoResponse
    {
        public List<LeaveInfo> Data { get; set; }
        public LeaveResult Result { get; set; }
    }
}
