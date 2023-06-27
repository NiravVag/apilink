using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Enums
{
    public enum NotificationType
    {
        LeaveApproved = 1, 
        LeaveRejected  = 2,
        ExpenseApproved = 3, 
        ExpenseChecked = 4, 
        ExpensePaid = 5, 
        ExpenseRejected = 6,
        LeaveApprovedHrLeave = 7,
        LeaveCancelled = 8,
        ExpenseCancelled = 9,
		InspectionRequested = 10,
		InspectionConfirmed = 11,
		InspectionCancelled = 12,
		InspectionModified = 13,
        InspectionVerified = 14,
        InspectionRescheduled = 15,
        InspectionSplit = 16,
        QuotationAdd = 17,
        QuotationToApprove = 18,
        QuotationSent = 19,
        QuotationCustomerConfirmed = 20,
        QuotationCustomerReject = 21,
        QuotationCanceled = 22,
        QuotationRejected = 23,
        QuotationModified=24,
        BookingQuantityChange = 25,
        BookingReportValidated=26,
        InspectionHold = 27,
        CustomerCreationFailed=28,
        CustomerUpdationFailed = 29,
        SupplierCreationFailed = 30,
        SupplierUpdationFailed = 31,
        FactoryCreationFailed = 30,
        FactoryUpdationFailed = 31,
        FastReportGenerationFailed=32,
    }

    public enum NotificationMessages
    {
        FastReportGenerationFailed=1,
    }
}


