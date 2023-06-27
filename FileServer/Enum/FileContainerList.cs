using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileServer.Enum
{
    /// <summary>
    /// when you add the container first add here, and after sync the container to below files
    /// 1. in the AzureBlobStorage containerList variable add new container
    /// 2. LINK_UI:- Entities Project FileContainerList enum in the Enum folder
    /// 3. LINK UI ClientApp:- in static-data-common.ts file have FileContainerList enum    
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
        DataManagement = 12,
        Claim = 13,
        GapSupportingDocument = 14,
        InvoiceSend=15,
        ScheduleJob=16
    }

    public enum EntityEnum
    {
        API = 1,
        SGT = 2,
        AQF = 3
    }
}
