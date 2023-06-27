using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerDepartmentDeleteResponse
    {
        public int Id { get; set; }

        public CustomerDepartmentDeleteResult Result { get; set; }
    }
    public enum CustomerDepartmentDeleteResult
    {
        Success = 1,
        NotFound = 2
    }
}
