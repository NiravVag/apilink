using DTO.Location;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Supplier
{
    public class SupplierListRequest
    {
        public IEnumerable<Country> CountryValues { get; set;  }
        
        public IEnumerable<SupplierType> TypeValues { get; set;  }
    }
}
