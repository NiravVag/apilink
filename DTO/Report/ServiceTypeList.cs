using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Report
{
    public class ServiceTypeList
    {
        public int InspectionId { get; set; }
        public int AuditId { get; set; }
        public int serviceTypeId { get; set; }
        public string serviceTypeName { get; set; }
        public bool IsAutoQCExpenseClaim { get; set; }
    }
}
