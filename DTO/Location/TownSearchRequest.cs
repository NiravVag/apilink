using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Location
{
    public class TownSearchRequest
    {
        public Country CountryValues { get; set; }

        public State ProvinceValues { get; set; }

        public City CityValues { get; set; }

        public County CountyValues { get; set; }

        public string TownValues { get; set; }

        public int? Index { get; set; }

        public int? pageSize { get; set; }
    }
}
