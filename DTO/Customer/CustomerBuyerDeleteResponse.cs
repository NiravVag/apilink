using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerBuyerDeleteResponse
    {
        public int Id { get; set; }

        public CustomerBuyerDeleteResult Result { get; set; }
    }
    public enum CustomerBuyerDeleteResult
    {
        Success = 1,
        NotFound = 2
    }
}
