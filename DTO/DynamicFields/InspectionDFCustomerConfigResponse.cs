using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.DynamicFields
{
    public class InspectionDFCustomerConfigResponse
    {
        public List<InspectionDFCustomerConfig> InspectionDFCustomerConfig { get; set; }
        public InspectionDFCustomerConfigResult Result { get; set; }
    }

    public enum InspectionDFCustomerConfigResult
    {
        Success=1,
        NotFound=2
    }
}
