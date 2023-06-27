using System;
using System.Collections.Generic;
using System.Text;
using DTO.References;

namespace DTO.ProductManagement
{
    public class EditProductCategoryResponse
    {
        public ProductCategory ProductCategory { get; set; }
        public EditProductManagementResult Result { get; set; }
    }
}
