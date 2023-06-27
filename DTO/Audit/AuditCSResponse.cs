using System;
using System.Collections.Generic;
using System.Text;
using DTO.HumanResource;
using DTO.References;

namespace DTO.Audit
{
   public class AuditCSResponse
    {
        public IEnumerable<CustomerCS> AuditCS { get; set; }
        public AuditCSResponseResult Result { get; set; }
    }
    public enum AuditCSResponseResult
    {
        success=1,
        error=2
    }
}
