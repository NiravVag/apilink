using DTO.Location;
using DTO.OfficeLocation;
using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.HumanResource
{
    public class StaffSearchRequest
    {
        public string EmployeeNumber { get; set;  }

        public string StaffName { get; set;  }

        public int? Index { get; set;  }

        //public bool IsLeft { get; set;  }

        public int? pageSize { get; set;  }

        public IEnumerable<Position> PositionValues { get; set;  }

        public IEnumerable<Office> OfficeValues { get; set; }

        public IEnumerable<Country> CountryValues { get; set; }

        public IEnumerable<Department> DepartmentValues { get; set; }

        public IEnumerable<EmployeeType> EmployeeTypeValues { get; set; }

    }
}
