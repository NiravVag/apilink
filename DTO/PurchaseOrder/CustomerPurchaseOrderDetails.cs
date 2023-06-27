using DTO.Common;
using DTO.CustomerProducts;
using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.PurchaseOrder
{
    public class CustomerPurchaseOrderDetails
    {
      
        [Required]
        public string Pono { get; set; }
        public string  CustomerReferencePo { get; set; }
        [Required]
        public string ProductRef { get; set; }
        [Required]
        public string ProductRefDesc { get; set; }
        public string ProductSubCateory { get; set; }
        public string ProductType { get; set; }
        public string BarCode { get; set; }
        public string FactoryRef { get; set; }
        public string DestinationCountry { get; set; }
        public string Etd { get; set; }
        [Required]
        public string SupplierCode { get; set; }        
        public string FactoryCode { get; set; }

        [RequiredGreaterThanZero(ErrorMessage = "Quantity required")]
        public int Qty { get; set;}    

    }
}
