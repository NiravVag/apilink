using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.DynamicFields
{
    public class DFCustomerConfigurationDeleteResponse
    {
        public int Id { get; set; }

        public DFCustomerConfigurationDeleteResult Result { get; set; }
    }
    public enum DFCustomerConfigurationDeleteResult
    {
        Success = 1,
        NotFound = 2
    }
}
