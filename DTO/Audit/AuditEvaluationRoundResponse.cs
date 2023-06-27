using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Audit
{
   public class AuditEvaluationRoundResponse
    {
        public IEnumerable<AuditEvaluationRound> EvaluationRoundList { get; set; }
        public AuditEvaluationRoundResponseResult Result { get; set; }
    }
    public enum AuditEvaluationRoundResponseResult
    {
        Success=1,
        Error=2
    }
}
