using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.PurchaseOrder
{
    public class SavePurchaseOrdersResponse
    {
        public List<SavePurchaseOrdersResult> savePurchaseOrdersResult { get; set; }
        public List<BookingPurchaseOrdersResult> bookingPurchaseOrdersResult { get; set; }
    }

    public class SavePurchaseOrdersResult
    {
       public int? Id { get; set; }
       public PurchaseOrderStatus purchaseOrderStatus { get; set; }
    }

    public class BookingPurchaseOrdersResult
    {
        public string PoId { get; set; }
        public string ProductId { get; set; }
    }
}
