using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerContactDeleteResponse
    {
        public int Id { get; set; }

        public CustomerContactDeleteResult Result { get; set; }
    }
    public enum CustomerContactDeleteResult
    {
        Success = 1,
        NotFound = 2
    }
}
