using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.HumanResource
{
    public class LeaveEmailModel
    {
        public int LeaveId { get; set; }

        public int RecepiendUserId { get; set;  }

        public string RecepientName { get; set; }

        public string RecepientEmail { get; set; }

        public string RecepientCCEmail { get; set; }

        public string SenderName { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string ApplyDate { get; set; }

        public string LeaveType { get; set; }

        public double Days { get; set; }

        public string Reason { get; set; }

        public string WebSite { get; set; }

    }
}
