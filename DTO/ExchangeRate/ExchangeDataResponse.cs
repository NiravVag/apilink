using DTO.HumanResource;
using System;
using System.Collections.Generic;
using System.Text;
using DTO.Common;
using DTO.References;

namespace DTO.ExchangeRate
{
    public class ExchangeDataResponse
    {
        public IEnumerable<Currency> CurrencyList { get; set;  }

        public IEnumerable<CurrencyRateItem> Data { get; set;  }

        public ExchangeDataResult Result { get; set;  }

        public int TotalCount { get; set;  }

        public int PageCount { get; set;  }


    }

    public class CurrencyRateItem
    {
        public DateObject BeginDate { get; set; }

        public DateObject EndDate { get; set;  }

        public List<RateItem> RateList { get; set;  }

        public bool IsNew { get; set;  }

    }

    public class RateItem
    {     
        public int ConversionId { get; set;  }

        public double Value { get; set;  }

        public int CurrencyId { get; set; }
    }

    public enum ExchangeDataResult
    {
        Success = 1, 
        NoDataFound = 2,
        TargetCurrencyRequired = 3,
        BeginDateRequired = 4,
        EndDateRequired = 5,
        ExchangeTypeRequired = 6
    }

}
