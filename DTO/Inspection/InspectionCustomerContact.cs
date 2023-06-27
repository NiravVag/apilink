using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Inspection
{
    public class InspectionCustomerContact
    {
        public int Id { get; set; }
        public int InspectionId { get; set; }
        public int ContactId { get; set; }
        public bool Active { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }
    }
}
