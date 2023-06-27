using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Inspection
{
    public class BookingCustomerContactSearchRequest
    {
        public int CustomerId { get; set; }
        public IEnumerable<int> BrandIdlst { get; set; }
        public IEnumerable<int> DeptIdlst { get; set; }
    }
}
