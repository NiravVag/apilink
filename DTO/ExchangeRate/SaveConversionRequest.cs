using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.ExchangeRate
{
    public class SaveConversionRequest
    {
        public int CurrencyTargetId { get; set;  }

        public int ExRateTypeId { get; set;  } 


        public IEnumerable<CurrencyRateItem> Data { get; set;  }

    }
}
