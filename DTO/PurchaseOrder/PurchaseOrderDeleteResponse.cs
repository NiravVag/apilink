using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.PurchaseOrder
{

    public class PurchaseOrderDeleteResponse
    {
        public int Id { get; set; }

        public PurchaseOrderDeleteResult Result { get; set; }
    }
    public enum PurchaseOrderDeleteResult
    {
        Success = 1,
        NotFound = 2
    }
}
