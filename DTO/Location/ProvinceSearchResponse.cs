using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Location
{
    public class ProvinceSearchResponse
    {
        public IEnumerable<State> Data { get; set; }

        public int TotalCount { get; set; }

        public int? Index { get; set; }

        public int? PageSize { get; set; }

        public int? PageCount { get; set; }

        public ProvinceSearchResult Result { get; set; }
    }
    public enum ProvinceSearchResult
    {
        Success = 1,
        NotFound = 2,
        Other = 3,
        NoCountryId=4
    }
}
