using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class EditCustomerResponse    {        public CustomerDetails CustomerDetails { get; set; }        public EditCustomerResult Result { get; set; }    }    public enum EditCustomerResult    {        Success = 1,        CannotGetCustomer = 2,    }
}
