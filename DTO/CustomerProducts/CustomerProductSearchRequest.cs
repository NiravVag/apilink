using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.CustomerProducts
{
    public class CustomerProductSearchRequest
    {
        public int? index { get; set; }        public int? pageSize { get; set; }
        public int? customerValue { get; set; }
        public int? productCategoryValue { get; set; }
        public int? productSubCategoryValue { get; set; }
        public IEnumerable<ProductCategorySub2> productCategorySub2s { get; set; }
        public List<int> productCategorySub3s { get; set; }
        public string productValue { get; set; }
        public string productDescription { get; set; }
        public string factoryReference { get; set; }
        public bool? isNewProduct { get; set; }
        public bool isStyle { get; set; }
        public int? categoryMapped { get; set; }
        public int? categorytypeid { get; set; }
    }


    public enum CustomerProductSearch
    {
        Category = 1,
        SubCategory = 2,
        SubCategory2 = 3,
        SubCategory3 = 4
    }

    public enum CustomerProductMapped
    {
        Mapped = 1,
        NotMapped = 2,
    }
}
