using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Audit
{
    public class SaveCancelRescheduleAuditResponse
    {
        public DateTime PrevServiceDateFrom { get; set; }
        public DateTime PrevServiceDateTo { get; set; }
        public int FactoryId { get; set; }
        public int CustomerId { get; set; }
        public SaveCancelAuditResult Result { get; set; }
    }
    public enum SaveCancelAuditResult
    {
        Success = 1,
        RequestNotCorrectFormat = 3,
        AuditNotFound = 4,
        AuditNotUpdated = 2
    }
}
