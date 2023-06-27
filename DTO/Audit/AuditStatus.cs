using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Audit
{
    public class AuditStatus
    {
        public int Id { get; set; }

        public string StatusName { get; set; }

        public string StatusColor { get; set; }

        public int TotalCount { get; set; }
    }
    public class AuditStatusResponse
    {
        public IEnumerable<AuditStatus> Auditstatuslist { get; set; }
        public AuditStatusResponseResult Result { get; set; }
    }
    public enum AuditStatusResponseResult
    {
        success = 1,
        Error = 2
    }
}
