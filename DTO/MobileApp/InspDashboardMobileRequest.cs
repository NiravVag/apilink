using DTO.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.MobileApp
{
    public class InspDashboardMobileRequest
    {
        public int? customerId { get; set; }
        public DateObject serviceFromDate { get; set; }
        public DateObject serviceToDate { get; set; }
    }
}
