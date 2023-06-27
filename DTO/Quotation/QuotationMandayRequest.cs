using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Quotation
{
    public class QuotationMandayRequest
    {
        public IEnumerable<int> BookingId { get; set; }
        public Service service { get; set; }
    }
}
