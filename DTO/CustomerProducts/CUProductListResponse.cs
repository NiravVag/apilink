using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.CustomerProducts
{
    public class CUProductListResponse
    {
        public string ProductReference { get; set; }
        public string ProductRefDescription { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string ProductType { get; set; }
        public string Barcode { get; set; }
        public string FactoryReference { get; set; }
        public string Unit { get; set; }
        public string Remarks { get; set; }
        public IEnumerable<FileTypes> files { get; set; }
    }

    public class FileTypes
    {
        public int Type { get; set; }
        public string TypeName { get; set; }
        public string UniqueId { get; set; }
        public string Link { get; set; }
    }
}
