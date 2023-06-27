using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.HumanResource
{
    public class HolidayDetaisRequest
    {
        public int? Index { get; set; }

        public int? PageSize { get; set; }

        public int Year { get; set;  }

        public int CountryId { get; set;  }

        public int? BranchId { get; set;  }
    }
}
