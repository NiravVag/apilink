using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.HumanResource
{
    public class SubDepartmentResponse
    {
        public IEnumerable<Department>  Data { get; set;  }

        public SubDepartmentResult Result { get; set;  }
    }

    public enum SubDepartmentResult
    {
        Success =1,
        CannotFindSubDepartments = 2
    }
}
