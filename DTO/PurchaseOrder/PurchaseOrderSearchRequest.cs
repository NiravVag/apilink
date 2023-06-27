using DTO.HumanResource;
using System;
using System.Collections.Generic;
using System.Text;
using DTO.Common;
namespace DTO.PurchaseOrder
{
    public class PurchaseOrderSearchRequest
    {
        public int? Index { get; set; }        public int? pageSize { get; set; }
        public PurchaseOrderData PurchaseOrderData { get; set; }
    }
    public class PurchaseOrderExportRequest
    {
        public PurchaseOrderData PurchaseOrderExportData { get; set; }
    }

    public class PurchaseOrderData
    {

        public int CustomerId { get; set; }
        public DateObject FromEtd { get; set; }
        public DateObject ToEtd { get; set; }
        public string Pono { get; set; }
        public int? DestinationCountry { get; set; }
        public int? FactoryId { get; set; }
        public int? SupplierId { get; set; }

    }
}
