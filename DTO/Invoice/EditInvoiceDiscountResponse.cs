using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Invoice
{
    public class EditInvoiceDiscountResponse
    {
        public SaveInvoiceDiscount InvoiceDiscount { get; set; }
        public InvoiceDiscountResult Result { get; set; }
    }
}
