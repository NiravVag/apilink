using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.PurchaseOrder
{
    public class PurchaseOrderAttachement
    {
        public int Id { get; set; }
      
        public int? PoId { get; set; }
        
        public string DocumentDescription { get; set; }

        public byte[] Document { get; set; }

        public bool? Active { get; set; }

        public int? CreatedBy { get; set; }
      
        public DateTime? CreatedTime { get; set; }

        public int? DeletedBy { get; set; }

        public DateTime? DeletedTime { get; set; }
    }
}
