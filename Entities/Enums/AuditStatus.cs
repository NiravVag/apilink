using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Enums
{
    public enum AuditStatus
    {

        Received = 1,
        Confirmed = 2,
        Rescheduled = 3,
        Cancel = 4,
        Scheduled = 5,
        Audited = 6,
        QuotationProgress = 7,
        QuotationDone = 8
    }
}
