using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DTO.PurchaseOrder
{
    public class GetCustomerPurchaseOrderResponse
    {
        public string message { get; set; }
        public HttpStatusCode statusCode { get; set; }
        public CUPurchaseOrderListResponse data { get; set;}
        public List<string> errors { get; set; }

    }
}
