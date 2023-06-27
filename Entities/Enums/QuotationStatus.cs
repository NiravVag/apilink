using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Enums
{
    public enum QuotationStatus
    {
        QuotationCreated  = 1,
        ManagerApproved = 2,
        ManagerRejected = 3, 
        QuotationVerified  = 4,
        Canceled = 5, 
        CustomerRejected = 6, 
        CustomerValidated = 7,
        SentToClient = 8,
        AERejected = 9,
        Pending = 0,
        QuotationPending=10

    }
}
