using DTO.CustomerProducts;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class EditCustomerProductResponse
    {
        public CustomerProduct CustomerProductDetails { get; set; }        public EditCustomerProductResult Result { get; set; }
    }
    public enum EditCustomerProductResult    {        Success = 1,        CannotGetCustomerProduct = 2,    }
}
