using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerServConfigSummaryResponse
    {
        public IEnumerable<CustomerItem> CustomerList { get; set; }

        public IEnumerable<Service> ServiceList { get; set; }

        public bool IsEdit { get; set; }

        public CustomerServConfigSummaryResult Result { get; set; }
    }

    public enum CustomerServConfigSummaryResult
    {
        Success = 1,
        CannotGetCountryList = 2,
        CannotGetTypeList = 3,
        CannotGetCustomerList = 4
    }

    public class CustomerServiceTypeResponse
    {
        public IEnumerable<ServiceType> CustomerServiceList { get; set; }
        public CustomerServiceTypeResult Result { get; set; }
    }

    public enum CustomerServiceTypeResult
    {
        Success = 1,
        CannotGetServiceTypeList = 2
    }
}
