using System.Data;

namespace DTO.Invoice
{
    public class XeroInvoiceData
    {
        public string ContactName { get; set; }
        public string EmailAddress { get; set; }
        public string POAddressLine1 { get; set; }
        public string POAddressLine2 { get; set; }
        public string POAddressLine3 { get; set; }
        public string POAddressLine4 { get; set; }
        public string POCity { get; set; }
        public string PORegion { get; set; }
        public string POPostalCode { get; set; }
        public string POCountry { get; set; }
        public string InvoiceNumber { get; set; }
        public string Reference { get; set; }
        public string InvoiceDate { get; set; }
        public string DueDate { get; set; }
        public string InventoryItemCode { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public double UnitAmount { get; set; }
        public double Discount { get; set; }
        public string AccountCode { get; set; }
        public string TaxType { get; set; }
        public string TrackingName1 { get; set; }
        public string TrackingOption1 { get; set; }
        public string TrackingName2 { get; set; }
        public string TrackingOption2 { get; set; }
        public string Currency { get; set; }
        public string BrandingTheme { get; set; }
        public string AccountName { get; set; }
    }

    public class XeroInvoiceResponse
    {
        public DataTable ResultData { get; set; }
        public XeroInvoiceResponseResult Result { get; set; }
    }

    public enum XeroInvoiceResponseResult
    {
        Success = 1,
        Failure = 2,
        NoInvoiceAccess = 3,
        StaffIsNotValid = 3,
        CannotGetList = 4
    }
}
