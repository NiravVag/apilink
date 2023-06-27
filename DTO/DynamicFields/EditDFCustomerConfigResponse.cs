using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.DynamicFields
{
    public class EditDFCustomerConfigResponse
    {
        public EditDfCustomerConfiguration DFCustomerConfiguration { get; set; }
        public EditDFCustomerConfigResult Result { get; set; }
    }

    public enum EditDFCustomerConfigResult
    {
        Success=1,
        NotFound=2
    }
}
