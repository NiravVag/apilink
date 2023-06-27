using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerDepartmentResponse
    {
        public IEnumerable<CustomerItem> CustomerList { get; set; }

        public IEnumerable<CustomerDepartments> CustomerDepartmentList { get; set; }

        public bool isEdit { get; set; }

        public CustomerDepartmentResult Result { get; set; }
    }

    public class CustomerDepartments
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
    }


    public enum CustomerDepartmentResult
    {
        Success = 1,
        CannotGetCustomer = 2,
        CannotGetDepartment = 3
    }
}
