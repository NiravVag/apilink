using DTO.CommonClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerDepartmentListResponse
    {
        public IEnumerable<CommonDataSource> CustomerDepartmentList { get; set; }

        public CustomerDepartmentsResult Result { get; set; }
    }

    public enum CustomerDepartmentsResult
    {
        Success = 1,
        CannotGetDepartment = 2
    }
}
