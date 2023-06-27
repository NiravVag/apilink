using DTO.Location;
using DTO.OfficeLocation;
using System.Collections.Generic;

namespace DTO.OfficeLocation
{
    public class EditOfficeResponse
    {
        public IEnumerable<Office> OfficeList { get; set; }
        public IEnumerable<OfficeType> LocationTypeList { get; set; }
        public IEnumerable<Country> CountryList { get; set; }
        public IEnumerable<City> CityList { get; set; }
        public Office OfficeDetails { get; set; }
        public EditOfficeResult Result { get; set; }
    }
    public enum EditOfficeResult
    {
        Success = 1,
        CanNotGetOfficeList = 2,
        CanNotGetCityList = 3,
        CanNotGetCountryList = 4,
        CannotLocationTypeList = 5,
        CanNotGetOffice = 6
    }
}
