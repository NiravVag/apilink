using DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Invoice
{
    public class InvoiceDiscountSearchRequest
    {
        public int? Index { get; set; }
        public int? PageSize { get; set; }
        public int? CustomerId { get; set; }
        public int? DiscountType { get; set; }
        public DateObject PeriodFrom { get; set; }
        public DateObject PeriodTo { get; set; }
        public int? CountryId { get; set; }
    }
}
