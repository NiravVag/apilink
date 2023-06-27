using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;
using DTO.Common;
namespace DTO.HumanResource
{
    public class EditHolidayRequest
    {
        public int Id { get; set;  }

        public DateObject  StartDate { get; set;  }

        public DateObject EndDate { get; set;  }

        public DateObject EndDateWeek { get; set; }
        

        // public int Year { get; set;  }

        public int? StartDay { get; set;  }

        public int? EndDay { get; set;  }

        public RecurrenceType RecurrenceType { get; set; }

        public bool ForAllIterations { get; set;  }

        public int CountryId { get; set;  }

        public int? OfficeId { get; set;  }

        public string HolidayName { get; set;  }

        public int StartDayType { get; set; }

        public int EndDayType { get; set; }

    }



}
