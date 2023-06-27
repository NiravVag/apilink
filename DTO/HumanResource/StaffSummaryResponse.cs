using DTO.Location;
using DTO.OfficeLocation;
using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.HumanResource
{

    public class StaffSummaryResponse
    {
        public IEnumerable<Position> PositionList { get; set;  }

        public IEnumerable<Department> DepartmentList { get; set;  }

        public IEnumerable<Country> CountryList { get; set;  }

        public IEnumerable<Office> OfficeList { get; set;  }

        public IEnumerable<EmployeeType> EmployeeTypeList { get; set;  }

        public StaffSummaryResult Result { get; set;  }

    }

    public class Position
    {
        public int Id { get; set;  }

        public string Name { get; set;  }

    }

    public class Department
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? DepartParentId { get; set;  }

    }

    public class EmployeeType
    {
        public int Id { get; set;  }

        public string Name { get; set;  }
    }

    public enum StaffSummaryResult
    {
        Success = 1,
        CannotGetPositionList = 2,
        CannotGetDepartmentList = 3,
        CannotGetCountryList = 4,
        CannotGetOfficeList =5,
        CannotGetEmployeeTypes =6,
        Other = 7
    }

}
