using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Enums
{
    public enum TaskType
    {
        None = 0,
        LeaveToApprove = 1, 
        ExpenseToApprove = 2, 
        ExpenseToCheck = 3, 
        ExpenseToPay = 4,
		VerifyInspection = 5,
		ConfirmInspection = 6,
        QuotationToApprove  = 7,
        SplitInspectionBooking = 8,
        ScheduleInspection = 9,
        QuotationModify = 10,
        QuotationSent = 11,
        QuotationCustomerConfirmed = 12,
        QuotationCustomerReject = 13,
        QuotationPending = 14,
        UpdateCustomerToFB=15,
        UpdateSupplierToFB = 16,
        UpdateFactoryToFB=17,
        UpdateProductToFB = 18,
        UpdateCustomerContactToTCF=19,
        UpdateSupplierToTCF = 20,
        UpdateUserToTCF=21,
        UpdateBuyerToTCF = 22,
        UpdateProductToTCF = 23,
        UpdateCustomerToTCF = 24,
        TravelTariffUpdate = 25
    }
}
