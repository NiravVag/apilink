using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Enums
{
    public enum BookingStatus
    {
        Received = 1,
        Confirmed = 2,
        Rescheduled = 3,
        Cancel = 4,
        Scheduled = 5,
        Inspected = 6,
        Validated = 7,
        Verified = 8,
        AllocateQC = 9,
        Hold = 10,
        ReportSent = 11
    }

    public enum AuditBookingStatus
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
