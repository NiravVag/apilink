using DTO.HumanResource;
using System;
using System.Collections.Generic;
using System.Text;
using DTO.Common;
using DTO.References;

namespace DTO.ExchangeRate
{
    public class ExchangeDataRequest
    {
        public Currency Currency { get; set;  }

        public DateObject  FromDate { get; set;  }

        public DateObject ToDate { get; set;  }

        public ExchangeRateType ExchangeType { get; set;  }

        public int? Index { get; set; }

        public int? pageSize { get; set; }
    }
}
