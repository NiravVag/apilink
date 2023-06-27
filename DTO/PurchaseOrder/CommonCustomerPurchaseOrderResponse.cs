using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DTO.PurchaseOrder
{
    public class CommonCustomerPurchaseOrderResponse
    {
        public string message { get; set; }
        public HttpStatusCode statusCode { get; set; }
        public List<string> errors { get; set; }

        public CommonCustomerPurchaseOrderResponse()
        {
            this.errors = new List<string>();
        }
    }

}
