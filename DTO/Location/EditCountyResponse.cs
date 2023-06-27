using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Location
{
    public class EditCountyResponse
    {
        public IEnumerable<Country> Countryvalues { get; set; }

        public IEnumerable<State> Provincevalues { get; set; }

        public IEnumerable<City> Cityvalues { get; set; }

        public County Countydetails { get; set; }

        public CountyResult Result { get; set; }

        }

   
    public enum CountyResult
    {
        Success = 1,
        CannotGetCounty = 2
    }
    
}
