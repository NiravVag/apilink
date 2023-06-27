using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{

    public class CustomerContactDataSourceRequest
    {
        public string SearchText { get; set; }
        public int ServiceId { get; set; }
        public IEnumerable<int?> ContactIds { get; set; }
        public IEnumerable<int?> CustomerIds { get; set; }
        public IEnumerable<string> CustomerGLCodes { get; set; }
        public IEnumerable<int> ContactTypeIds { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class CustomerContactDataSourceRepo
    {
        public int CustomerId { get; set; }
        public int ContactId { get; set; }
        public string ContactName { get; set; }
        public IEnumerable<int> ContactTypeIds { get; set; }
        public IEnumerable<int> ServiceIds { get; set; }
    }
}
