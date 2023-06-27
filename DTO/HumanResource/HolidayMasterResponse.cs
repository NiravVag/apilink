using DTO.Location;
using DTO.OfficeLocation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.HumanResource
{
    public class HolidayMasterResponse
    {
        public IEnumerable<Country> CountryList { get; set; }

        public IEnumerable<Office> OfficeList { get; set; }

        public IEnumerable<int> YearList { get; set; }

        public IEnumerable<HolidayDayType> HolidayDayTypeList { get; set;  }

        public HolidayMasterResult Result { get; set; }
    }

    public enum HolidayMasterResult
    {
        Success = 1, 
        CannotGetOfficeList = 2, 
        CannotGetCountryList = 3,
        CannotGetHolidayDayTypes = 4
    }
}
