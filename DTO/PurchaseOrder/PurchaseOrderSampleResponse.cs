using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.PurchaseOrder
{
    public class PurchaseOrderSampleResponse
    {
        public PurchaseOrderSample PurchaseOrderSampleData { get; set; }

        public PurchaseOrderSampleResult Result { get; set; }
    }

    public class PurchaseOrderSample
    {
        public string POSampleFile { get; set; }
        public string PODateFormatFile { get; set; }
    }

    public enum PurchaseOrderSampleResult
    {
        Success = 1,
        NotFound = 2
    }
}
