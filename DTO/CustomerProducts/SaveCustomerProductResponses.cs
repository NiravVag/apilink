using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DTO.CustomerProducts
{
    public class SaveCustomerProductResponses
    {
        public string message { get; set; }
        public HttpStatusCode statusCode { get; set; }
        public CUProductListResponse data { get; set; }
        public List<string> errors { get; set; }
    }
}
