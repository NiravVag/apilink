using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Inspection
{
    public class ReInspectionTypeResponse
    {
        public IEnumerable<ReInspectionTypeData> reInspectionTypeList { get; set; }
        public ReInspectionTypeResult result { get; set; }
    }

    public class ReInspectionTypeData
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public enum ReInspectionTypeResult
    {
       success=1,
       notFound=2
    }
}
