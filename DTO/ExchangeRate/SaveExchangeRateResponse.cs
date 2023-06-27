using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.ExchangeRate
{
    public class SaveExchangeRateResponse
    {
        public SaveExchangeRateResult Result { get; set;  }

        public IEnumerable<int> AddList { get; set;  }

        public IEnumerable<int> EditList { get; set;  }

    }

    public enum  SaveExchangeRateResult
    {
        Success = 1, 
        CannotGetData = 2
    }
}
