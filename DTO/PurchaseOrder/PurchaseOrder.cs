using DTO.CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.PurchaseOrder
{
    public class PurchaseOrder
    {
        public int Id { get; set; }
    
        public string Pono { get; set; }
   
        public int? OfficeId { get; set; }
   
        public int CustomerId { get; set; }

        public List<CommonDataSource> SupplierData { get; set; }

        public List<CommonDataSource> FactoryData { get; set; }

        public int? DepartmentId { get; set; }
    
        public int? BrandId { get; set; }
     
        public string InternalRemarks { get; set; }
    
        public string CustomerRemarks { get; set; }

        public string CustomerreferencePO { get; set; }

        public bool? Active { get; set; }

        public int? CreatedBy { get; set; }   
        
        public DateTime? CreatedTime { get; set; }

       
 
    }
}
