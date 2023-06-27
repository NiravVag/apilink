using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Location
{
    public class CountryResponseEnum
    {
        public Array countryList { get; set; }
        public CountryResponseEnumResult Result { get; set;}
    }

    public enum CountryResponseEnumResult
    {
        Success = 1,
        CannotGetCountryList = 2
    }
}
 