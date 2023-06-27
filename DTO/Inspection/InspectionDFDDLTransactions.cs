using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Inspection
{
    public class InspectionDFDDLTransactions
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int ControlConfigurationId { get; set; }
        public int Value { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
    }
}
