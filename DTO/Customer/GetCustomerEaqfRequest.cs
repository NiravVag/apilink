using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Customer
{
    public class GetCustomerEaqfRequest
    {
        public int Index { get; set; }
        public int PageSize { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
    }
}
