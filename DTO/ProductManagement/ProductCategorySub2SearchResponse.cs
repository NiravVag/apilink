using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.ProductManagement
{
    public class ProductCategorySub2SearchResponse
    {
        public IEnumerable<ProductCategorySub2> Data { get; set; }

        public int TotalCount { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public ProductManagementResult Result { get; set; }
    }
}
