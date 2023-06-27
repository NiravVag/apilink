using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Invoice
{
    public class InvoiceDiscountData
    {
        public int Id { get; set; }
        public IEnumerable<int> CountryIds { get; set; }
        public int DiscountType { get; set; }
        public int CustomerId { get; set; }
    }
}
