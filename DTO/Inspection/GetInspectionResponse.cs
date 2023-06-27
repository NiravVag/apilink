using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Inspection
{
    public class GetInspectionResponse
    {
        public InspectionBookingDetails InspectionBookingDetails { get; set; }
        public InspectionResult Result { get; set; }
    }

    public enum InspectionResult    {        Success = 1,        CannotGetInspection = 2,    }
}
