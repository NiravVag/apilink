using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerDeleteResponse
    {
        public int Id { get; set; }

        public CustomerDeleteResult Result { get; set; }
    }
    public enum CustomerDeleteResult
    {
        Success = 1,
        NotFound = 2
    }
}
