using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Location
{
    public class CountySearchResponse
    {
        public IEnumerable<County> Data { get; set; }

        public int TotalCount { get; set; }

        public int? Index { get; set; }

        public int? PageSize { get; set; }

        public int? PageCount { get; set; }

        public CountySearchResult Result { get; set; }
    }

    public enum CountySearchResult
    {
        Success = 1,
        NotFound = 2,
        Other = 3,
        NoCountryId = 4
    }
}
