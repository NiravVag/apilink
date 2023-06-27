using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Inspection
{
    public class BookingCustomerContactRequest
    {
        public int customerId { get; set; }
        public int bookingId { get; set; }
        public int customerServiceId { get; set; }
        public List<int> brandIdlst { get; set; }
        public List<int>  deptIdlst { get; set; }
    }
}
