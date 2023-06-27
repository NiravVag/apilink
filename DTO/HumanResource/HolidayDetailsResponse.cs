using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;
using DTO.Common;
namespace DTO.HumanResource
{
    public class HolidayDetailsResponse
    {

        public int TotalCount { get; set;  }

        public int PageCount { get; set;  }

        public IEnumerable<Holiday> Data { get; set; }

        public HolidayDetailsResult Result { get; set;  }

    }


    public class Holiday
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public RecurrenceType RecurrenceType { get; set; }

        public DateObject StartDate { get; set; }

        public DateObject EndDate { get; set; }

       // public DayOfWeek StartDay { get; set;  }

       // public DayOfWeek EndDay { get; set;  }

        public string CountryName { get; set;  } 

        public string OfficeName { get; set;  }

        public int? StartDayType { get; set;  }

        public int? EndDayType { get; set;  }

    }

    public enum HolidayDetailsResult 
    {
        Success = 1,
        NoDataFound = 2
    }

}
