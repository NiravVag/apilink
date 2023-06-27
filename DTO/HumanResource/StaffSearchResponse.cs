using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.HumanResource
{
    public class StaffSearchResponse
    {
        public IEnumerable<StaffItem> Data { get; set;  }

        public int TotalCount { get; set;  }

        public int Index { get; set;  }

        public int PageSize { get; set;  }

        public int PageCount { get; set;  }

        public StaffSearchResult Result { get; set; }
    }

    public class StaffItem
    {
        public int Id { get; set; }

        public string StaffName { get; set;  }

        public string CountryName { get; set;  }

        public string PositionName { get; set;  }
        
        public string DepartmentName { get; set;  }

        public string OfficeName { get; set;  }

        public string JoinDate { get; set;  }

        public string EmployeeType { get; set;  }

        public string StatusName { get; set; }

        public int? StatusId { get; set; }
    }

    public enum StaffSearchResult
    {
        Success = 1,
        NotFound = 2,
        Other = 3
    }
}
