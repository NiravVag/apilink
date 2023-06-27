using DTO.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.PurchaseOrder
{
    public class EditPurchaseOrderResponse    {        public PurchaseOrderDetailedInfo PurchaseOrderData { get; set; }        public EditPurchaseOrderResult Result { get; set; }    }
    public class PurchaseOrderResponse    {        public PurchaseOrder PurchaseOrderData { get; set; }        public EditPurchaseOrderResult Result { get; set; }    }
    public class PurchaseOrderDetailsRequest
    {
        public int Id { get; set; }

        public int? Index { get; set; }

        public int? pageSize { get; set; }
    }    public class PurchaseOrderDetailsResponse    {        public IEnumerable<PurchaseOrderDetails> Data { get; set; }
        public int TotalCount { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }        public EditPurchaseOrderResult Result { get; set; }    }    public class PurchaseOrderAttachmentsResponse    {        public IEnumerable<FileAttachment> Data { get; set; }
        public EditPurchaseOrderResult Result { get; set; }    }    public class RemovePurchaseOrderDetailsRequest
    {
        public int Id { get; set; }
        public int AccessType { get; set; }
    }
    public class RemovePurchaseOrderDetailsResponse
    {
        public int Id { get; set; }
        public EditPurchaseOrderResult Result { get; set; }
    }    public enum EditPurchaseOrderResult    {        Success = 1,        CannotGetPurchaseOrder = 2,    }
}

