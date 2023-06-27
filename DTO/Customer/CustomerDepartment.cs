using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerDepartment
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string DepartmentCode { get; set; }

        public IEnumerable<CustomerItem> CustomerList { get; set; }
    }

    public class BookingCustomerDepartment
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int BookingId { get; set; }

        public string DepartmentCode { get; set; }
    }
}
