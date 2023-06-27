using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.CustomerProducts
{
    public class CustomerProductResponse
    {
        public IEnumerable<CustomerProduct> CustomerProductList { get; set; }

        public CustomerProductResult Result { get; set; }
    }

    public enum CustomerProductResult
    {
        Success = 1,
        CannotGetCustomerProduct = 2,
    }
}
