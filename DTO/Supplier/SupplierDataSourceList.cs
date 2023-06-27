using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Supplier
{
    public class SupplierDataSourceRepo
    {
        public int SupplierId { get; set; }
        public int? SupplierType { get; set; }
        public string SupplierName { get; set; }
        public IEnumerable<int?> ServiceIds { get; set; }
        public IEnumerable<int> CustomerIds { get; set; }
    }

    public class SupplierDataSourceRequest
    {
        public string SearchText { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public IEnumerable<int?> CustomerIds { get; set; }
        public IEnumerable<string> CustomerGLCodes { get; set; }
        public int? ServiceId { get; set; }
        public int SupplierType { get; set; }
        public IEnumerable<int?> SupplierIds { get; set; }
        public int SupSearchTypeId { get; set; }
    }
}
