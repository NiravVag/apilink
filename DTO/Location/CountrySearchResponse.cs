using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Location
{
    public class CountrySearchResponse
    {
        public IEnumerable<Country> Data { get; set; }

        public int TotalCount { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public CountrySearchResult Result { get; set; }
    }
    public enum CountrySearchResult
    {
        Success = 1,
        NotFound = 2,
        Other = 3
    }
}
