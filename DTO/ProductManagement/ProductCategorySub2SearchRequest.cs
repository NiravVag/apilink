using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.ProductManagement
{
    public class ProductCategorySub2SearchRequest
    {
        public IEnumerable<ProductCategorySub2> ProductCategorySub2Values { get; set; }

        public int? ProductCategoryId { get; set; }

        public int? ProductSubCategoryId { get; set; }

        public int? Index { get; set; }

        public int? pageSize { get; set; }

        public string ProductCategorySub2Name { get; set; }

    }
}
