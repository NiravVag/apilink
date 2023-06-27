using DTO.HumanResource;
using System;
using System.Collections.Generic;
using System.Text;
using DTO.Common;
namespace DTO.Audit
{
   public class AuditReportDetails
    {
        public int Auidtid { get; set; }

        public DateObject Servicedatefrom { get; set; }

        public DateObject Servicedateto { get; set; }

        public IEnumerable<int> Auditors { get; set; }

        public string Comment { get; set; }

        public IEnumerable<AuditReportAttachment> Attachments { get; set; }

    }

    public class AuditReportAttachment
    {
        public int Id { get; set; }

        public Guid Guid { get; set; }

        public string Uniqueld { get; set; }

        public string FileName { get; set; }

        public bool IsNew { get; set; }

        public string MimeType { get; set; }

        public string FileUrl { get; set; }
    }
    public class AuditReportSummary
    {
        public AuditReportDetails Data { get; set; }
        public AuditReportSummaryResult Result { get; set; }
    }
    public enum AuditReportSummaryResult
    {
        success=1,
        error=2
    }
}
