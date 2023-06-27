using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.CustomReport
{
    public class InspectionCustomReportSummaryResponse
    {
        public InspectionCustomReportItem Data { get; set; }
        public InspectionCustomReportSummaryResponseResult Result { get; set; }
    }
    public enum InspectionCustomReportSummaryResponseResult
    {
        Success = 1,
        NotFound = 2,
        Other = 3
    }
}
