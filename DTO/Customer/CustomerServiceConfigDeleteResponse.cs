using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerServiceConfigDeleteResponse
    {
        public int Id { get; set; }

        public CustomerServiceConfigDeleteResult Result { get; set; }
    }
    public enum CustomerServiceConfigDeleteResult
    {
        Success = 1,
        NotFound = 2
    }
}
