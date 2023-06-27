using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerBuyerResponse
    {
        public IEnumerable<CustomerItem> CustomerList { get; set; }

        public IEnumerable<CustomerBuyers> CustomerBuyerList {get;set;}

        public bool isEdit { get; set; }
        
        public CustomerBuyerResult Result { get; set; }
    }

    public class CustomerBuyers
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public List<int> apiServiceIds { get; set; }
    }

    public enum CustomerBuyerResult
    {
        Success = 1,
        CannotGetCustomer = 2,
        CannotGetBuyer = 3
    }
}
