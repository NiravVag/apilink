using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.ProductManagement
{
    public class ProductCategoryResponse
    {
        public IEnumerable<ProductCategory> ProductCategoryList { get; set; }
        public ProductManagementResult Result { get; set; }
    }
}
