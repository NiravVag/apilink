using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Location
{
    public class CountySearchRequest
    {
        public Country CountryValues { get; set; }

        public City CityValues { get; set; }

        public string CountyValues { get; set; }

        public int? Index { get; set; }

        public int? pageSize { get; set; }
    }
}
