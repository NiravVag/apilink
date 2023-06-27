using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using DTO.Common;
namespace DTO.HumanResource
{
    public class LeaveDaysRequest
    {
        public DateObject StartDate { get; set;  }

        public DateObject EndDate { get; set;  }

        public DayType StartDayType { get; set;  }

        public DayType EndDayType { get; set;  }
    }
}
