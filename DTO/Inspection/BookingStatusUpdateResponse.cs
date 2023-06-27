using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Inspection
{
    public class BookingStatusUpdateResponse
    {
        public BookingStatusUpdateResponseResult Result { get; set; }
    }
    public enum BookingStatusUpdateResponseResult
    {
        success = 1,
        failed = 2
    }
}
