using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.References
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public int? BusinessLineId { get; set; }
        public string BusinessLine { get; set; }
    }
}
