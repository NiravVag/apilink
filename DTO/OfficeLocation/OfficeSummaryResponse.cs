using DTO.InspectionCertificate;
using System.Collections.Generic;

namespace DTO.OfficeLocation
{
    public class OfficeSummaryResponse
    {
        public IEnumerable<Office> officeList { get; set; }
        public OfficeSummaryResult Result { get; set; }
    }
    public enum OfficeSummaryResult
    {
        Success = 1,
        CannotGetOfficeList = 2
    }

    public class OfficeLocationResponse
    {
        public DropdownResult Result { get; set; }
        public IEnumerable<DropDown> OfficeLocationList { get; set; }
    }
}
