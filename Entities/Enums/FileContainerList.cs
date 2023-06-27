using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Enums
{
    /// <summary>
    /// if you add new container, so first add the container in the FileServer ContainerList enum and Azure Blob Storage Container List Variable and sync with them.
    /// after add the container here based on the FileServer Container List enum
    /// at last add contianer in ClientApp > static-data-common.ts file have FileContainerList enum
    /// </summary>
    public enum FileContainerList
    {
        Products = 1,
        DevContainer = 2,
        InspectionBooking = 3,
        Invoice = 4,
        Hr = 5,
        TcfProducts = 6,
        EmailSend = 7,
        Audit = 8,
        Expense = 9,
        Report = 10,
        QuotationPdf = 11,
        InspectionReport = 12,
        Claim = 13,
        GapSupportingDocument = 14,
        InvoiceSend = 15,
        ScheduleJob = 16
    }
}
