using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class ZohoCustomerDetailResponse    {        public List<ZohoCustomer> CustomerDetails { get; set; }        public CustomerDetailResult Result { get; set; }    }    public enum CustomerDetailResult    {        Success = 1,        CannotGetCustomer = 2,    }
}
