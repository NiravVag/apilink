using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Audit
{
    public class SaveAuditResponse
    {
        public int Id { get; set; }

        public SaveAuditResult Result { get; set; }

        public string BookingNo { get; set; }
        public bool IsMissionUpdated { get; set; }

    }
    public enum SaveAuditResult
    {
        Success = 1,
        AuditNotSaved = 2,
        RequestNotCorrectFormat = 3,
        AuditNotFound = 4,
        AuditNotUpdated = 5
    }
}
