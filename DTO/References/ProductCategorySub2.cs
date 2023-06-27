using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.References
{
    public class ProductCategorySub2
	{
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductSubCategoryId { get; set; }
        public ProductSubCategory ProductSubCategory { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public bool Active { get; set; }
    }
}
