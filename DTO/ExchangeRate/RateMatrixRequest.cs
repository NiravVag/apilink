using DTO.HumanResource;
using System;
using System.Collections.Generic;
using System.Text;
using DTO.Common;
using DTO.References;

namespace DTO.ExchangeRate
{
    public class RateMatrixRequest
    {
        public DateObject FromDate { get; set;  }

        public DateObject ToDate { get; set; }

        public ExchangeRateType ExchangeType { get; set;  }

        public Currency Currency { get; set;  } 
    }
}
