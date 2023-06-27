using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Location
{
    public class CountrySummaryResponse
    {
        public IEnumerable<Country> countryList { get; set; }
        public CountrySummaryResult Result { get; set; }
    }
    public enum CountrySummaryResult
    {
        Success = 1,
        CannotGetCountryList = 2
    }
}
