using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Location
{
   public class CitySearchRequest
    {
        public int? Countryvalues { get; set; }
        public State provinceValues { get; set; }
        public IEnumerable<State> Cityvalues { get; set; }
        public int? Index { get; set; }

        public int? pageSize { get; set; }
    }
}
