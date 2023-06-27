using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.ProductManagement
{
    public class ProductCategorySub2ListResponse
    {
        public IEnumerable<ProductCategorySub2Data> ProductCategorySub2List { get; set; }
        public ProductCategorySub2ListResult Result { get; set; }
    }

    public enum ProductCategorySub2ListResult
    {
        Success=1,
        NotFound=2
    }
}
