using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.ExchangeRate
{
    public class MatrixDataExport
    {
        public  string CurrencyTarget { get; set; }

        public string FromDate { get; set;  }

        public string ToDate { get; set; }

        public string Type { get; set;  }
        
        public IEnumerable<CurrencyValue> Data { get; set; }
    }
}
