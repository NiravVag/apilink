using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.References
{
    public class ProductCategorySubListResponse
    {
        public int ProductSubCategoryId { get; set; }
        public IEnumerable<ProductCategorySubData> ProductCategorySub2List { get; set; }
        public ProductCategorySubListResult Result { get; set; }
    }

    public enum ProductCategorySubListResult
    {
        Success = 1,
        NotFound = 2
    }
}
