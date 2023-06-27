using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.ProductManagement
{
    public class ProductCategorySub2Response
    {
        public IEnumerable<ProductCategorySub2> ProductCategorySub2List { get; set; }
        public IEnumerable<ProductCategory> ProductCategoryList { get; set; }
        public ProductCategorySub2Result Result { get; set; }
    }

    public enum ProductCategorySub2Result
	{
        Success=1,
		CannotGetProductCategorySub2 = 2
    }
}
