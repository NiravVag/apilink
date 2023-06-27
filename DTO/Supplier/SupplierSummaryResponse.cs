using DTO.Location;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Supplier
{
    public class SupplierSummaryResponse
    {
        public IEnumerable<Country> CountryList { get; set; }

        public IEnumerable<SupplierType>  TypeList { get; set;  }

        public bool IsEdit { get; set;  }

        public SupplierSummaryResult Result { get; set;  }
    }

    public enum SupplierSummaryResult
    {
        Success = 1, 
        CannotGetCountryList = 2 , 
        CannotGetTypeList = 3
    }
}
