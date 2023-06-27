using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CSConfigSearchResponse
    {
        public IEnumerable<CSConfigItem> Data { get; set; }
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public CSConfigSearchResResult Result { get; set; }
    }
    public enum CSConfigSearchResResult
    {
        Success = 1,
        NotFound = 2,
        Other = 3
    }
}
