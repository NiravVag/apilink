using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Enums
{
    public enum Service
    {
        InspectionId = 1,
        AuditId = 2,
        Tcf = 3,
        Lab = 4,
        SGTAudit = 5
    }

    public enum CuCheckPointTypeService
    {
        QuotaionRequired = 1,
        QuotaionApprovedByManager = 1,
        CustomerDecison = 3,
        NewReportFormat = 7
    }

    public enum EmailSendingType
    {
        CustomerDecision = 1,
        ReportSend = 2,
        InvoiceStatus = 3
    }
}
