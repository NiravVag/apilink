﻿using DTO.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.PurchaseOrder
{
    public class EditPurchaseOrderResponse
    public class PurchaseOrderResponse
    public class PurchaseOrderDetailsRequest
    {
        public int Id { get; set; }

        public int? Index { get; set; }

        public int? pageSize { get; set; }
    }
        public int TotalCount { get; set; }

        public int Index { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }
        public EditPurchaseOrderResult Result { get; set; }
    {
        public int Id { get; set; }
        public int AccessType { get; set; }
    }
    public class RemovePurchaseOrderDetailsResponse
    {
        public int Id { get; set; }
        public EditPurchaseOrderResult Result { get; set; }
    }
}
