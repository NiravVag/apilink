using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CSAllocationSearchRequest
    {
        public int? CustomerId { get; set; }
        public int? OfficeId { get; set; }
        public int? ServiceId { get; set; }
        public IEnumerable<int> ProductCategoryIds { get; set; }
        public IEnumerable<int> BrandIds { get; set; }
        public IEnumerable<int> DepartmentIds { get; set; }
        public IEnumerable<int> UserIds { get; set; }
        public IEnumerable<int> FactoryCountryIds { get; set; }
        public int? UserType { get; set; }
        public int? Index { get; set; }
        public int? pageSize { get; set; }
    }
}
