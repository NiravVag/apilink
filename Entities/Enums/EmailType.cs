using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Enums
{
    public enum EmailType
    {
        InspBookingRequest = 1,
        InspBookingConfirmed = 2,
        InspBookingCancelled = 3,
        InspBookingSplit = 5,
        InspBookingRescheduled = 4,
        AuditpBookingRequest = 6,
        AuditpBookingConfirmed = 7,
        AuditCancelled = 8,
        AuditBookingRescheduled = 9,
        QuotationRequest = 10,
        QuotationApproved = 11,
        QuotationSentToCustomer = 12,
        QuotationConfirmByCustomer = 13,
        QuotationRejectedByCustomer = 14,
        QuotationRejectedByManager = 15,
        QuotationCancel = 16
    }

    public enum EmailValidOption
    {
        EmailSizeExceed = 1,
        ReportLinkIsNotValid = 2,
        EmailSuccess = 3
    }


    public enum ReportInEmail
    {
        Link = 1,
        Link_Attachment = 2
    }

}
