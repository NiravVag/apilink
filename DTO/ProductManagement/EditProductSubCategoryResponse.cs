using System;
using System.Collections.Generic;
using System.Text;
using DTO.References;

namespace DTO.ProductManagement
{
    public class EditProductSubCategoryResponse
    {
        public ProductSubCategory ProductSubCategory { get; set; }
        public EditProductManagementResult Result { get; set; }
    }
}
