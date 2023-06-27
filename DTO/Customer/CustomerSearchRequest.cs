using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerSearchRequest
    {
        public int? Index { get; set; }        public int? pageSize { get; set; }       
        public CustomerSearchModel customerData { get; set; }
    }

    public class CustomerSearchModel {
        public int CustomerId { get; set; }
        public int? GroupId { get; set; }
        public bool? IsEAQF { get; set; }
    }
}
