using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class BuyerDataSourceRequest
    {
        public string SearchText { get; set; }
        public int ServiceId { get; set; }
        public IEnumerable<int?> CustomerIds { get; set; }
        public IEnumerable<string> CustomerGLCodes { get; set; }
        public IEnumerable<int?> BuyerIds { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }

    public class BuyerDataSourceRepo
    {
        public int CustomerId { get; set; }
        public int BuyerId { get; set; }
        public string BuyerName { get; set; }
        public IEnumerable<int> ServiceIds { get; set; }
    }
}
