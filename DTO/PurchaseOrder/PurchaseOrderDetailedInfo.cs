using DTO.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.PurchaseOrder
{
    public class PurchaseOrderDetailedInfo
    {
        public int Id { get; set; }

        public string Pono { get; set; }

        public string CustomerReferencePo { get; set; }

        public int? OfficeId { get; set; }

        public int CustomerId { get; set; }


        public List<int> SupplierIds { get; set; }
        public List<int> FactoryIds { get; set; }

        public int? DepartmentId { get; set; }

        public int? BrandId { get; set; }

        public string InternalRemarks { get; set; }

        public string CustomerRemarks { get; set; }

        public int? AccessType { get; set; }

        public bool? Active { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedTime { get; set; }

        public IEnumerable<PurchaseOrderDetails> PurchaseOrderDetails { get; set; }

        public IEnumerable<FileAttachment> PurchaseOrderAttachments { get; set; }

    }
}
