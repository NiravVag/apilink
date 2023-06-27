using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.DynamicFields
{
    public class DfCustomerSearchResponse
    {
        public int TotalCount { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public IEnumerable<DfCustomerConfigSearchData> Data { get; set; }

        public DfCustomerSearchResult Result { get; set; }
    }

    public enum DfCustomerSearchResult
    {
        Success = 1,
        NotFound = 2

    }
}
