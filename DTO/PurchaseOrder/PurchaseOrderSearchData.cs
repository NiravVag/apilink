using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.PurchaseOrder
{
    public class PurchaseOrderSearchData
    {
        public int Id { get; set; }

        public string Pono { get; set; }

        public string CustomerName { get; set; }

        public string ETD { get; set; }

        public string DestinationCountry { get; set; }

        public bool? Active { get; set; }

        public bool? IsBooked { get; set; }

        public int PoId { get; set; }
        public bool IsDelete { get; set; }
        public int BookingNumber { get; set; }
        public int BookingCount { get; set; }
        public int ShowBookingCount { get; set; }
    }
}
