using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Location
{
    public class TownSearchResponse
    {
        public IEnumerable<Town> DataList { get; set; }

        public Town Data { get; set; }

        public int TotalCount { get; set; }

        public int? Index { get; set; }

        public int? PageSize { get; set; }

        public int? PageCount { get; set; }

        public TownSearchResult Result { get; set; }
    }

    public enum TownSearchResult
    {
        Success = 1,
        NotFound = 2,
        Other = 3,
        NoCountryId = 4
    }
}
