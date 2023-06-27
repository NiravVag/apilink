using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Audit
{
   public class SaveAuditReportResponse
    {
        public SaveAuditReportResponseResult Result { get; set; }
    }
    public enum SaveAuditReportResponseResult
    {
        Success = 1,
        RequestNotCorrectFormat = 3,
        AuditNotFound = 4,
        AuditNotUpdated = 2,
        NoAuditorsFound=5
    }
}
