using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class EditCustomerServiceConfigResponse
    {
        public EditCustomerServiceConfigData CustomerServiceConfigData { get; set; }        public EditCustomerServiceConfigResult Result { get; set; }
    }

    public enum EditCustomerServiceConfigResult    {        Success = 1,        CannotGetServiceType = 2    }
}
