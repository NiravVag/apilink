using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Audit
{
    public class AuditBookingRuleResponse
    {
        public AuditBookingRule RuleDetails{get;set;}
        public AuditBookingRuleResult Result { get; set; }
    }
    public enum AuditBookingRuleResult
    {
        Success = 1,
        NotFound = 2
    }
}
