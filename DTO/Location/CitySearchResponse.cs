using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Location
{
    public class CitySearchResponse
    {
        public IEnumerable<City> Data { get; set; }
        public int TotalCount { get; set; }

        public int? Index { get; set; }

        public int? PageSize { get; set; }

        public int? PageCount { get; set; }

        public CitySearchresult Result { get; set; }
    }

    public enum CitySearchresult
    {
        success = 1,
        NotFound = 2
    }
}
