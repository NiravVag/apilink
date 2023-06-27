using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.ExchangeRate
{
    public class ExchangeRateResponse 
    {
        public IEnumerable<Currency> CurrencyList { get; set;  }

        public IEnumerable<ExchangeRateType> RateTypeList { get; set;  }


        public ExchangeRateResult Result { get; set;  }
    }

    public enum ExchangeRateResult
    {
        Success = 1,
        CannotGetCurrencyList = 2,
        CannotGetRateTypeList = 3
    }
}
