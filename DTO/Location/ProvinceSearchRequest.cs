using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Location
{
   public class ProvinceSearchRequest
    {
        public Country CountryValues { get; set; }

        public IEnumerable<State>  ProvinceValues { get; set; }

        public int? Index { get; set; }

        public int? pageSize { get; set; }
    }
}
