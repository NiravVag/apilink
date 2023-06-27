using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Location
{
   public class CountrySearchRequest
    {
        public IEnumerable<Country> CountryValues { get; set; }

        public int? Index { get; set; }

        public int? pageSize { get; set; }
    }
}
