using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.MasterConfig
{
    public class ConfigMasterData
    {

    }

    public enum PurchaseOrderSampleFile
    {
        ImportPOSampleFile = 1,
        ImportPODateFormat = 2
    }

    public enum EntityConfigMaster
    {
        InspBookTerms = 1,
        InspConfirmTermsEnglish = 2,
        InspConfirmTermsChinese = 3,
        BookingTeamGroupEmail = 4,
        Entity = 5,
        ImageLogo = 6,
        ImageICChop = 7,
        ImageICSign = 8,
        API_HO_Address = 9,
        API_HO_Wrap_Address = 10,
        API_INSP_Title = 11,
        API_AUD_Title = 12,
        LeadTimeMessage = 13,
        InspectionConfirmFooter = 14,
        InspectionRescheduleFooter = 15,
        PreInvoiceEmailContent1 = 18,
        PreInvoiceEmailContent2 = 19,
        ImportPurchaseOrderUpload = 20,

        DefaultOfficeForEAQF = 23,

        ImportPurchaseOrderDateFormat = 21,
        DefaultEntityBasedOffice = 22,
        ICRemarks = 24,
        ICFooter = 25,
        DefaultEAQFBank = 26,
        DefaultEAQFBillingEntity = 27,
        DefaultEAQFInvoiceOffice = 28,
        RescheduleTheBookingWithoutCancelTheQuotation = 29,
        PreInvoiceContactMandatoryInQuotation = 30,
        AuditConfirmedEnglishFooter = 31,
        AuditConfirmedChineseFooter = 32,
        AuditRescheduleEnglishFooter = 33,
        AuditRescheduleChineseFooter = 34,
    }
}