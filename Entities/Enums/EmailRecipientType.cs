namespace Entities.Enums
{
    public enum EmailRecipientType
    {
        Customer = 1,
        Supplier = 2,
        Factory = 3,
        APITeam = 4,
        Merchandiser = 5,
        CustomerContact = 6,
        QuotationCustomerContact = 7,
        QuotationSupplierContact = 8,
        QuotationFactoryContact = 9,
        QuotationInternalContact = 10,
        InvoiceContact = 11
    }

    public enum RecipientType
    {
        To = 1,
        Cc = 2,
        Bcc = 3
    }

    public enum EmailSubjectFiledType
    {
        Supplier = 1,
        ServiceDate = 2,
        Inspection = 3,
        Customer = 4,
        ServiceType = 5,
        Office = 6,
    }

    public enum Email_Report_Send_Type
    {
        OneEmailWithAllReports = 1,
        OneEmailWithSameReportResult = 2,
        OneEmailOneReport = 3,
        OneEmailWithReportSummary = 4,
        OneEmailWithReportSummaryAndAllReports = 5,
        OneInvoicePerEmail = 6,
        OneEmailWithAllInvoice = 7
    }
}
