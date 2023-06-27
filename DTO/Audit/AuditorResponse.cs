using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Audit
{
   public class AuditorResponse
    {
        public IEnumerable <Auditor> Auditors { get; set; }
        public AuditorResponseResult Result { get; set; }
    }
    public enum AuditorResponseResult
    {
        success=1,
        error=2
    }
}
