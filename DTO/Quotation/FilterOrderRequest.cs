using DTO.Common;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Quotation
{
    public class FilterOrderRequest
    {
        public int CustomerId { get; set;  }

        public int SupplierId { get; set;  }

        public int FactoryId { get; set;  }

        public Service ServiceId { get; set;  }

        public DateObject StartDate { get; set; }

        public DateObject EndDate { get; set; }

        public  int? BookingNo { get; set;  }

        public IEnumerable<int> OfficeIds { get; set; }

        public IEnumerable<int> BookingIds { get; set; }

        public int BillMethodId { get; set; }

        public int BillPaidById { get; set; }

        public int? CurrencyId { get; set; }
    }
}
