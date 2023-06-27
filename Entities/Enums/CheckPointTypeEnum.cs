using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Enums
{
    public enum CheckPointTypeEnum
    {
        QuotationRequired = 1,
        QuotationApproveByManager = 2,
        POQuantityModification = 3,
        ICRequired = 4,
        CustomerDecisionRequired = 5,
        SkipQuotationSentToClient = 6,
        NewReportFormat = 7,
        SendBookingEmailToCustomer = 8,
        AutoCustomerDecisionForPassReportResult = 9,
        PoProductBySupplier=14,
        HideMultiSelectCustomerDecision = 15,
        HidePricecardAmountforCustomerinQuotation = 16
    }
}
