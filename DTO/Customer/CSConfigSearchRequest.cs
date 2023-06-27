using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CSConfigSearchRequest
    {
        public int? CustomerId { get; set; }
        public int? UserId { get; set; }
        public IEnumerable<int> OfficeLocationId { get; set; }
        public IEnumerable<int?> ServiceId { get; set; }
        public IEnumerable<int?> ProductCategoryId { get; set; }
        public int? Index { get; set; }
        public int? pageSize { get; set; }
    }
}
