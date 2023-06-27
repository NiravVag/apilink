using DTO.HumanResource;
using System;
using System.Collections.Generic;
using System.Text;
using DTO.Common;
using DTO.References;

namespace DTO.ExchangeRate
{
    public class RateMatrixResponse
    {
        public IEnumerable<CurrencyItem> Data { get; set;  }

        public int CurrencyTargetId { get; set;  }

        public DateObject FromDate { get; set; }

        public DateObject ToDate { get; set; }

        public int ExchangeTypeId { get; set; }

        public int TotalCount { get; set; }

        public int PageCount { get; set; }
         
        public RateMatrixResult Result { get; set;  }
    }

    public class CurrencyItem
    {
        public Currency Currency { get; set;  }

        public IEnumerable<CurrencyValue> CurrencyValueList { get; set; } 
    }

    public class CurrencyValue
    {
        public Currency Currency { get; set; }
        public string Value { get; set;  }
    }

    public enum RateMatrixResult
    {
        Success =1, 
        NotFound =2,
        CurrencyIsRequired = 3,
        DateIsrequired = 4,
        ExchangeTypeIsRequired = 5
    }
}
