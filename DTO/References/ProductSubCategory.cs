using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.References
{
    public class ProductSubCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProductCategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public bool Active { get; set; }
    }
}
