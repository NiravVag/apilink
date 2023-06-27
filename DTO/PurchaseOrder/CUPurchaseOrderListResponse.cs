using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.PurchaseOrder
{
    public class CUPurchaseOrderListResponse
    {

        public string pono { get; set; }
        public string customerReferencePo { get; set; }     
        public string status{ get; set; }
        public IEnumerable<PurchaseOrderDetailList> purchaseOrderDetailLists { get; set; }
    }

    public class PurchaseOrderDetailList
    {
        public string productRef { get; set; }

        public string productRefDesc { get; set; }
        public string productSubCategory { get; set; }
        public string productType { get; set; }
        public string barCode { get; set; }
        public string factoryRef { get; set; }
        public string destinationCountry { get; set; }
        public string etd { get; set; }
        public int qty { get; set; }

        public string supplierCode { get; set; }

        public string factoryCode { get; set; }

    }
}
