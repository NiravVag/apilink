using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Inspection
{
    public class InspectionPickingBase
    {
        public int Id { get; set; }
        public int PoTransactionId { get; set; }
        public int? PickingQuantity { get; set; }
    }

    public class InspectionPickingExistRequest
    {
        public List<int> PoTransactionIds { get; set; }
    }

}
