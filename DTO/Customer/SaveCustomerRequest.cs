using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class SaveCustomerDepartmentRequest
    {
        public int customerValue { get; set; }
        public List<CustomerDepartments> departmentList { get; set; }
    }
}
