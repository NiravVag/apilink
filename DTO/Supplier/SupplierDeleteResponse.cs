using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Supplier
{
    public class SupplierDeleteResponse
    {
        public int Id { get; set;  }

        public SupplierDeleteResult Result { get; set;  }
    }

    public enum SupplierDeleteResult
    {
        Success = 1, 
        NotFound = 2,
        CannotDelete = 3
    }
}
