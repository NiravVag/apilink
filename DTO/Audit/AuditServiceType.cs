using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Audit
{

    public class AuditServiceTypeResponse
    {
        public IEnumerable<DTO.References.ServiceType> auditServiceTypes { get; set; }
        public AuditServiceTypeResponseResult result;
    }
    public enum AuditServiceTypeResponseResult
    {
        success=1,
        error=2
    }

}
