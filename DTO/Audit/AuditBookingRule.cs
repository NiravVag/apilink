using DTO.HumanResource;
using System;
using System.Collections.Generic;
using System.Text;
using DTO.Common;
namespace DTO.Audit
{
   public class AuditBookingRule
    {
        public int Id { get; set; }

        public string RuleDescription { get; set; }

        public int LeadDays { get; set; }

        public IEnumerable<DateObject> Holidays { get; set; }
    }
}
