using System;
using System.Collections.Generic;

namespace DTO.Inspection
{
    public class InspectionHolidaySummaryList
    {
        public int? CountryId { get; set; }
        public IEnumerable<DateTime> HolidaysDate { get; set; }
        public HolidayResult Result { get; set; }

    }
    public enum HolidayResult
    {
        Success = 1,
        NotFound = 2
    }
}
