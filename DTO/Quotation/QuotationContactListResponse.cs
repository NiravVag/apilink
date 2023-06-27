using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Quotation
{
    public class QuotationContactListResponse
    {
        public IEnumerable<QuotationEntityContact> Data { get; set;  }

        public QuotationContactListResult Result { get; set;  }
    }

    public enum QuotationContactListResult
    {
        Success = 1,
        NotFound  = 2,
        CustomerEmpty = 3,
        officeIsEmpty  = 4,
        SupplierEmpty = 5,
        FactoryEmpty =6
    }
}
