using DTO.OfficeLocation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Location
{
   public class EditCityResponse
    {
        public IEnumerable<Country> CountryValues { get; set; }
        public IEnumerable<State> ProvinceValues { get; set; }
        public IEnumerable<Office> OfficeValues { get; set; }
        public IEnumerable<Zone> ZoneValues { get; set; }
        public City CityDetails { get; set; }
        public EditCityResult Result { get; set; }
    }

    public enum EditCityResult {
        success = 1,
        CanNotGetCountryList = 2,
        CanNotGetProvinceDetails = 3,
        CanNotGetOfficeList=4,
        CanNotGetInspPort=5,
        CanNotGetZone=6,
        CanNotGetDistance=7,
        CanNotGetCityDetails=8
    }
}
