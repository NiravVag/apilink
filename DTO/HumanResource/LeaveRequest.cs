using System;
using System.Collections.Generic;
using System.Text;
using DTO.Common;
namespace DTO.HumanResource
{
    public class LeaveRequest
    {
        public int Id { get; set;  }

        public DateObject StartDate { get; set;  }

        public DateObject EndDate { get; set;  }

        public int LeaveTypeId { get; set;  }

        public double LeaveDays { get; set;  }
        
        public string  Reason { get; set;  } 

        public int StartDayType { get; set;  }

        public int EndDayType { get; set;  }

        public int StaffId { get; set;  }

        public string StaffName { get; set;  }

        public int StatusId { get; set;  }

        public string StatusName { get; set;  }

    }
}
