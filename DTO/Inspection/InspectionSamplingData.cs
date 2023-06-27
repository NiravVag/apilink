using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Inspection
{
    public class SamplingQuantityBookingRequest
    {
        public int CustomerId { get; set; }
        public int ServiceTypeId { get; set; }
    }

    public class SamplingQuantityDataRequest
    {
        public int? AqlId { get; set; }
        public int? CriticalId { get; set; }
        public int? MajorId { get; set; }
        public int? MinorId { get; set; }
        public int TotalBookingQuantity { get; set; }
    }
}
