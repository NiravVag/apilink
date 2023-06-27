using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Customer
{
    public class CustomerContactSummaryResponse
    {
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }

        public IEnumerable<CustomerContact> CustomerContacts { get; set; }
        public CustomerContactSummaryResult Result { get; set; }
        public CustomerItem CustomerValues { get; set; }
    }

    public enum CustomerContactSummaryResult
    {
        Success = 1,
        NotFound = 2
    }
}
