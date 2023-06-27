using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerContactSearchRequest
    {
        public int? Index { get; set; }        public int? pageSize { get; set; }
        public int? customerValue { get; set; }
        public string contactName { get; set; }

        public List<int> CuBrandList { get; set; }

        public List<int> CudepartmentList { get; set; }

        public List<int> CuServiceList { get; set; }
    }
}
