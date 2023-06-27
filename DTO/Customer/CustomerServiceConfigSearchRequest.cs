using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerServiceConfigSearchRequest
    {
        public int? index { get; set; }        public int? pageSize { get; set; }
        public int? customerValue { get; set; }
        public int? serviceValue { get; set; }
    }
}
