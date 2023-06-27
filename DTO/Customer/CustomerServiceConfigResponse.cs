using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{

    public class CustomerServiceConfigResponse
    {
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }

        public IEnumerable<CustomerServiceConfig> Data { get; set; }
        public IEnumerable<CustomerItem> CustomerList { get; set; }
        public IEnumerable<Service> ServiceList { get; set; }
        public CustomerServiceConfigResult Result { get; set; }
        public CustomerItem CustomerValues { get; set; }
    }

    public enum CustomerServiceConfigResult
    {
        Success = 1,
        NotFound = 2
    }
}
