using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerContactSummaryRequest
    {
        public int? index { get; set; }        public int? pageSize { get; set; }
        public int? customerID { get; set; }
    }
}
