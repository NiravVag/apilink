using DTO.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Invoice
{
    public class SaveInvoiceDiscount
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public IEnumerable<int> CountryIds { get; set; }
        public int DiscountType { get; set; }
        [Required]
        public DateObject PeriodFrom { get; set; }
        [Required]
        public DateObject PeriodTo { get; set; }        
        public bool? ApplyToNewCountry {get;set;}
        [Required]
        public IEnumerable<InvoiceDiscountLimit> Limits { get; set; }
    }

    public class InvoiceDiscountLimit
    {
        public int Id { get; set; }
        public decimal LimitFrom { get; set; }
        public decimal LimitTo { get; set; }
        public bool? Notification { get; set; }
    }
}
