using System;
using System.Collections.Generic;
using System.Text;
using DTO.Common;
namespace DTO.HumanResource
{
    public class LeaveSummaryRequest
    {
        public string StaffName { get; set;  }

        public IEnumerable<LeaveStatus>  StatusValues { get; set;  }

        public  IEnumerable<LeaveType> TypeValues { get; set;  }

        public DateObject StartDate { get; set;  }

        public DateObject EndDate { get; set;  }

        public int? Index { get; set; }

        public int PageSize { get; set; }

        public bool IsApproveSummary { get; set; }
    }

    public class LeaveSummaryDataRequest
    {
        public string StaffName { get; set; }

        public IEnumerable<int> StatusIds { get; set; }

        public IEnumerable<int> Typeids { get; set; }

        public IEnumerable<int> Staffids { get; set;  }

        public IEnumerable<int> LocationIds { get; set;  }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }

        public bool IsApproveSummary { get; set; }


    }
}
