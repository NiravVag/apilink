using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.ProductManagement
{
    public class ProductSubCategoryResponse
    {
        public IEnumerable<ProductSubCategory> ProductSubCategoryList { get; set; }
        public IEnumerable<ProductCategory> ProductCategoryList { get; set; }
        public ProductManagementResult Result { get; set; }
    }
}
