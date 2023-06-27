using DTO.HumanResource;
using System;
using System.Collections.Generic;
using System.Text;
using DTO.Common;
namespace DTO.PurchaseOrder
{
    public class PurchaseOrderDetails
    {
        public int Id { get; set; }
     
        public int PoId { get; set; }
       
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductDesc { get; set; }

        public int? DestinationCountryId { get; set; }

        public string DestinationCountryName { get; set; }

        public DateObject Etd { get; set; }

        public int? BookingStatus { get; set; }

        public int? SupplierId { get; set; }

        public string SupplierName { get; set; }

        public int? FactoryId { get; set; }
  
        public string FactoryReference { get; set; }

        public int Quantity { get; set; }

        public bool? Active { get; set; }

        

        public bool IsBooked { get; set; }
    
    }
     
    public class PurchaseOrderRepo
    {
        public int Id { get; set; }

        public int PoId { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductDesc { get; set; }

        public int? DestinationCountryId { get; set; }

        public string DestinationCountryName { get; set; }

        public DateTime? Etd { get; set; }

        public int? BookingStatus { get; set; }

        public int? SupplierId { get; set; }

        public string SupplierName { get; set; }

        public int? FactoryId { get; set; }

        public string FactoryReference { get; set; }

        public int Quantity { get; set; }

        public bool? Active { get; set; }
        

        public bool IsBooked { get; set; }

    }
}
