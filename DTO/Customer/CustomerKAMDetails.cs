using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerKamDetail
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class CustomerKamDetailResponse
    {
        public CustomerKamDetail CustomerKAMDetail { get; set; }
        public CustomerKAMResult Result { get; set; }
    }

    public enum CustomerKAMResult
    {
        Success=1,
        NotFound=2
    }
}
