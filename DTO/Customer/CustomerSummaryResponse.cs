using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerSummaryResponse
    {
        public IEnumerable<CustomerItem> CustomerList { get; set; }

        public bool IsEdit { get; set; }

        public CustomerSummaryResult Result { get; set; }
    }

    public class CustomerSourceResponse
    {
        public IEnumerable<CustomerSource> CustomerSource { get; set; }

        public CustomerGroupResult Result { get; set; }
    }

    public enum CustomerSummaryResult
    {
        Success = 1,
        CannotGetCountryList = 2,
        CannotGetTypeList = 3,
        CannotGetCustomerList = 4
    }


    public class CustomerGroupResponse
    {
        public IEnumerable<CustomerGroup> CustomerGroup { get; set; }      

        public CustomerGroupResult Result { get; set; }
    }

    public enum CustomerGroupResult
    {
        Success = 1,       
        CannotGetCustomerGroupList = 4
    }
}
