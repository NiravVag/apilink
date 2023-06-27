using DTO.Customer;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Inspection
{
    public class BookingCustomerContactDetails
    {
        public IEnumerable<CustomerContact> CustomerContactList { get; set; }
        public BookingCustomerContactResult Result { get; set; }
    }

    public enum BookingCustomerContactResult
    {
        Success=1,
        NotFound=2
    }
}
