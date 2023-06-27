using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.HumanResource
{
    public class LeaveSummaryDataResponse
    {
        public IEnumerable<LeaveItem> Data { get; set; }

        public LeaveSummaryDataResult Result { get; set; }

        public bool CanCheck { get; set;  }

        public int TotalCount { get; set;  }

        public int Index { get; set;  }

        public int PageSize { get; set;  }

        public IEnumerable<LeaveStatus> LeaveStatusList { get; set;  }

    }


    public enum LeaveSummaryDataResult
    {
        Success = 1,
        NotFound = 2,
        CannotShow = 3
    }


    public class LeaveItem
    {
        public int Id { get; set; }

        public string staffName { get; set; }

        public string PositionName { get; set; }

        public string OfficeName { get; set; }

        public string ApplicationDate { get; set; }

        public LeaveType LeaveType { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public double Days { get; set; }

        public LeaveStatus Status { get; set; }

        public string Reason { get; set; }

        public bool CanApprove { get; set;  }

        public string Comment { get; set;  }

    }

    public class LeaveStatus
    {
        public int Id { get; set; }

        public string Label { get; set; }

        public int TotalCount { get; set; }
    }
}
