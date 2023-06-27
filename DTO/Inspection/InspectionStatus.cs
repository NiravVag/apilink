using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Inspection
{
    public class InspectionStatus
    {
        public int Id { get; set; }

        public int StatusId { get; set; }
        public string StatusName { get; set; }

        public string StatusColor { get; set; }

        public int TotalCount { get; set; }

        public int Priority { get; set; }

        public bool StatusActive { get; set; }
    }
}
