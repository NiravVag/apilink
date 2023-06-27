using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Invoice
{
    public class InvoiceDiscountSearchResponse
    {
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }

        public IEnumerable<InvoiceDiscountSummaryItem> Data { get; set; }
        public InvoiceDiscountResult Result { get; set; }
    }
    public enum InvoiceDiscountResult
    {
        Success = 1,
        NotFound = 2,
        PeriodNotAvailable = 3,
        InvoiceDiscountPeriodNotFound = 4
    }
    public class InvoiceDiscountSummaryItem
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public string DiscountType { get; set; }
        public string PeriodFrom { get; set; }
        public string PeriodTo { get; set; }
        public string Country { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
    }

    public class DeleteInvoiceDiscountResponse
    {
        public InvoiceDiscountResult Result { get; set; }
    }

    public class SaveInvoiceDiscountResponse
    {
        public InvoiceDiscountResult Result { get; set; }
    }
}
