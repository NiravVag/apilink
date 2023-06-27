using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.PurchaseOrder
{
    public class SavePurchaseOrderResponse
    {
        public int Id { get; set; }
        public SavePurchaseOrderResult Result { get; set; }
    }   

    public enum SavePurchaseOrderResult
    {
        Success = 1,
        PurchaseOrderIsNotSaved = 2,
        PurchaseOrderIsNotFound = 3,
        PurchaseOrderExists = 4,
        ProductDuplicate=5,
        SupplierIdCannotBeNullOrZero=6
    }

    public class PurchaseOrderDetailResponse
    {
        public PurchaseOrderDetailResult Result { get; set; }
    }

    public enum PurchaseOrderDetailResult
    {
        Success=1,
        ProductDuplicate=2
    }
}
