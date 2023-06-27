using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Customer
{
   public class CustomerProductCategory
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        public string Code { get; set; }
        public int? CustomerId { get; set; }

        public string sector { get; set; }
        public int? ProductSubCategoryId { get; set; }
        public int? ProductCategoryId { get; set; }
        public int sort { get; set; }
    }
}
