using System;
using System.Collections.Generic;
using System.Text;
using DTO.References;

namespace DTO.ProductManagement
{
    public class EditProductCategorySub2Response
    {
        public ProductCategorySub2 ProductCategorySub2 { get; set; }
        public IEnumerable<ProductCategory> ProductCategoryList { get; set; }
        public EditProductManagementResult Result { get; set; }
    }
}
