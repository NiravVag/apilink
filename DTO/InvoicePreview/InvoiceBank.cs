using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.InvoicePreview
{
    public class InvoiceBankPreview
    {
        public string Id { get; set; }

        public string AccountName { get; set; }

        public string AccountNumber { get; set; }

        public string ChopLink { get; set; }

        public string SignLink { get; set; }

        public List<InvoiceBankTaxPreview> InvoiceBankTaxList { get; set; }
    }
    public class InvoiceBankTaxPreview
    {
        public string TaxId { get; set; }
        public string TaxName { get; set; }
        public string TaxValue { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    public class InvoiceBankTaxRepo
    {
        public int BankId { get; set; }
        public int TaxId { get; set; }
        public string Name { get; set; }
        public decimal Value { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class InvoiceBankRepo
    {
        public int Id { get; set; }

        public string AccountName { get; set; }

        public string AccountNumber { get; set; }

        public string ChopLink { get; set; }

        public string SignLink { get; set; }

        public List<InvoiceBankTaxRepo> InvoiceBankTaxList { get; set; }
    }
}
