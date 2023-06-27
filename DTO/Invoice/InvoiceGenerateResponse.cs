using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Invoice
{
    public class InvoiceGenerateResponse
    {
        public List<string> InvoiceData { get; set; }
        public InvoiceGenerateResult Result { get; set; }
    }

    public enum InvoiceGenerateResult
    {
        Success = 1,
        Failure = 2,
        RequestIsNotValid = 3,
        NoPricecardRuleFound = 4,
        NoInspectionFound = 5,
        NoRuleMapped = 6,
        FutureDateNotAllowed = 7,
        FromDateAfterToDate = 8,
        NoSupplierSelected = 9,
        SupplierIsRequired = 10,
        TravelOrInspectionRequired = 11,
        NoInspectionSelected = 12,
        NoInvoiceConfigured = 13,
        BankIsRequired = 14,
        NoInvoiceDataAccess = 15
    }
}
