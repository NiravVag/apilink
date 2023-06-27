using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Audit
{
    public class AuditReportNoResponse
    {
        public string ReportNo { get; set; }
        public AuditReportNoResponseResult Result { get; set; }
    }

    public enum AuditReportNoResponseResult
    {
        success = 1,
        Fail = 2
    }
}
