using DTO.Inspection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.InspectionPicking
{
    public class InspectionPickingSummaryResponse
    {
        public IEnumerable<InspectionPickingData> InspectionPickingList { get; set; }
        public InspectionPickingSummaryResponseResult Result { get; set; }
        public IEnumerable<PickingProduct> PickingProductList { get; set; }

    }

    public enum InspectionPickingSummaryResponseResult
    {
        pickingproductfound = 1,
        inspectionpickingproductsfound=2,
        pickingproductnotfound=3,
        datanotfound=4
    }

    public class PickingProduct
    {
        public int inspectionID { get; set; }
        public int poID { get; set; }
        public string poName { get; set; }
        public int productID { get; set; }
        public string productName { get; set; }
        public int? pickingQuantity { get; set; }
        public int? poTransactionID { get; set; }
    }
}
