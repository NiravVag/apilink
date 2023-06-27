using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.ExchangeRate
{
    public class Conversion
    {
        public int Id { get; set;  }

        public int CurrencyId1 { get; set;  }

        public int CurrencyId2 { get; set;  }

        public DateTime BeginDate { get; set;  }

        public DateTime EndDate { get; set;  }

        public int StaffId { get; set;  }

        public decimal Rate { get; set;  }

        public DateTime CreatedDate { get; set;  }

        public DateTime LastUpdateDate { get; set;  }

        public int RateTypeId { get; set;  }

    }
}
