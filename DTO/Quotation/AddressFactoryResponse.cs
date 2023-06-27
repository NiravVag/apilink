using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Quotation
{
    public class AddressFactoryResponse
    {
        public string Address { get; set;  }

        public AddressFactoryResult Result { get; set;  }
    }

    public enum AddressFactoryResult
    {
        Success =1,
        NotFound  = 2
    }
}
