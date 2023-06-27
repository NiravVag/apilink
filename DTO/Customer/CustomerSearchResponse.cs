using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerSearchResponse    {        public int TotalCount { get; set; }        public int Index { get; set; }        public int PageSize { get; set; }        public int PageCount { get; set; }

        public IEnumerable<CustomerItem> Data { get; set; }        public CustomerSearchResult Result { get; set; }    }

    public enum CustomerSearchResult    {        Success = 1,        NotFound = 2    }
}
