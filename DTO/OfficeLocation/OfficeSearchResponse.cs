using DTO.OfficeLocation;
using System.Collections.Generic;

namespace DTO.OfficeLocation
{
    public class OfficeSearchResponse
    {
        public IEnumerable<Office> Data { get; set; }

        public int TotalCount { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public OfficeSearchResult Result { get; set; }
    }
    public enum OfficeSearchResult
    {
        Success = 1,
        NotFound = 2,
        Other = 3
    }
}
