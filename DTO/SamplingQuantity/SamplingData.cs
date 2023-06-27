using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.SamplingQuantity
{
    public class SamplingQuantityRequest
    {
        public int BookingId { get; set; }
        public int ServiceTypeId { get; set; }
        public int CustomerId { get; set; }
        public int? AqlId { get; set; }
        public int OrderQuantity { get; set; }
        public int CriticalId { get; set; }
        public int MajorId { get; set; }
        public int MinorId { get; set; }
    }
}
