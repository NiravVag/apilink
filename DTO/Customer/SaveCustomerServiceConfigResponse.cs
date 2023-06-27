using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class SaveCustomerServiceConfigResponse
    {
        public int Id { get; set; }
        public SaveCustomerServiceConfigResult Result { get; set; }
    }

    public enum SaveCustomerServiceConfigResult
    {
        Success = 1,
        CustomerServiceConfigIsNotSaved = 2,
        CustomerServiceConfigIsNotFound = 3,
        CustomerServiceConfigExists = 4
    }
}
