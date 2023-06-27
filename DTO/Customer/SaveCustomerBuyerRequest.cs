using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class SaveCustomerBuyerRequest
    {
        public int customerValue { get; set; }
        public List<CustomerBuyers> buyerList { get; set; }
    }
}
