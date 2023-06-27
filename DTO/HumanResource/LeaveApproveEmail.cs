using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.HumanResource
{
    public class LeaveApproveEmail
    {

        public Guid Id { get; set;  }

        public string ReceipentEmail { get; set; }

        public string ReceipentCCEmail { get; set; }

        public int UserId { get; set;  }

        public string StaffName { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string ApplyDate { get; set; }

        public string ApproveOn { get; set; }

        public double TotalDays { get; set; }

        public string LeaveType { get; set; }

        public string Comment { get; set; }

        public string LeaveStatus { get; set; }

        public string Url { get; set;  }

        public string UserName { get; set;  }

        public IEnumerable<HrLeaveStaff> LeaveStaffList { get; set;  }

        public bool IsCancelledAfterApproval { get; set; }
    }

    public class HrLeaveStaff
    {
        public IEnumerable<int> UserIdList { get; set;  }

        public int StaffId { get; set;  }

        public string StaffName { get; set;  }

        public string StaffEmail { get; set;  }
    }
}
