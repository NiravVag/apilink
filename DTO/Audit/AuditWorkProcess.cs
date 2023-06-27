using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Audit
{
   public class AuditWorkProcess
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
    public class AuditWorkprocessResponse
    {
        public IEnumerable<AuditWorkProcess> AuditWorkProcessList { get; set; }
        public AuditWorkprocessResponseResult Result { get; set; }
    }
    public enum AuditWorkprocessResponseResult
    {
        success=1,
        error=2
    }
}
