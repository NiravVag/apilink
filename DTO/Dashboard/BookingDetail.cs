using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Dashboard
{
    public class BookingDetail
    {
        public int InspectionId { get; set; }
        public int CustomerId { get; set; }
        public int SupplierId { get; set; }
        public int? FactoryId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ServiceDateFrom { get; set; }
        public DateTime ServiceDateTo { get; set; }
        public int StatusId { get; set; }
    }
}
