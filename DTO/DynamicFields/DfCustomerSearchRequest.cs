using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.DynamicFields
{
    public class DfCustomerSearchRequest
    {
        public int? index { get; set; }        public int? pageSize { get; set; }
        
        public int? moduleId { get; set; }
        public IEnumerable<int> customerDataList { get; set; }
        public IEnumerable<int> controlTypeDataList { get; set; }
    }
}
