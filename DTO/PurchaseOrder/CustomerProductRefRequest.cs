using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.PurchaseOrder
{
    public class CustomerProductRefRequest
    {
        [Required]
        public string ProductReference { get; set; }
        [Required]
        public string ProductRefDescription { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string ProductType { get; set; }
        public string Barcode { get; set; }
        public string FactoryReference { get; set; }
        public string Unit { get; set; }
        public string Remarks { get; set; }


    }
}
