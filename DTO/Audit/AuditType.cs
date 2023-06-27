using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Audit
{
   public class AuditType
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class AuditTypeResponse
    {
        public IEnumerable<AuditType> AuditTypes { get; set; }
        public AuditTypeResponseResult Result { get; set; }
    }

    public enum AuditTypeResponseResult
    {
        success=1,
        error=2
    }
}
