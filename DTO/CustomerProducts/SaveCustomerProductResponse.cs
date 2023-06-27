using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.CustomerProducts
{
    public class SaveCustomerProductResponse
    {
        public int Id { get; set; }
        public List<CustomerProductDetail> ProductList { get; set; }
        public SaveCustomerProductResult Result { get; set; }
    }

    public enum SaveCustomerProductResult
    {
        Success = 1,
        CustomerProductIsNotSaved = 2,
        CustomerProductIsNotFound = 3,
        CustomerProductExists = 4
    }
}
