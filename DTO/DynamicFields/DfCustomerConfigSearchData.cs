using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.DynamicFields
{
    public class DfCustomerConfigSearchData
    {
        public int id { get; set; }
        public string ModuleName { get; set; }
        public int ModuleId { get; set; }
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        public int ControlTypeId { get; set; }
        public string ControlTypeName { get; set; }
        public string Label { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsBooking { get; set; }

    }
}
