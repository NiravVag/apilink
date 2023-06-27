using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Inspection
{
    public class InspectionPickingContact
    {
        public int Id { get; set; }

        public int PickingTranId { get; set; }

        public int? LabContactId { get; set; }

        public int? CusContactId { get; set; }

        public bool Active { get; set; }
    }
}
