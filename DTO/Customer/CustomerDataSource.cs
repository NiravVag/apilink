using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerDataSourceRequest
    {
        public string SearchText { get; set; }
        public int ServiceId { get; set; }
        public IEnumerable<string> CustomerGLCodes { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class CustomerDataSourceRepo
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public IEnumerable<int?> ServiceIds { get; set; }
        public IEnumerable<int> SupplierIds { get; set; }
    }
}
