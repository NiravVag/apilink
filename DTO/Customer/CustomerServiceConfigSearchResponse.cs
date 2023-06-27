using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerServiceConfigSearchResponse
    {
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }

        public IEnumerable<CustomerServiceConfig> Data { get; set; }
        public CustomerServiceConfigSearchResult Result { get; set; }
    }

    public class CustomerServiceConfig
    {
        public int Id { get; set; }

        public string Service { get; set; }

        public string ProductCategory { get; set; }

        public string ServiceType { get; set; }

        public string SamplingMethod { get; set; }

        public string CustomerName { get; set; }

    }

    public enum CustomerServiceConfigSearchResult
    {
        Success = 1,
        NotFound = 2
    }
}
