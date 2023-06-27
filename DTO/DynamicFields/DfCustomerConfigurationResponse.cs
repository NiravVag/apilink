using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.DynamicFields
{
    public class DfCustomerConfigurationResponse
    {
        public DfCustomerConfiguration dfCustomerConfiguration { get; set; }
        public DfCustomerConfigurationResult Result { get; set; }
    }

    public enum DfCustomerConfigurationResult
    {
        Success=1,
        NotFound=2
    }
}
