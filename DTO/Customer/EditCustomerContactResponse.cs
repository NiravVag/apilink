using DTO.CommonClass;
using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class EditCustomerContactResponse
    {
        public CustomerContactDetails CustomerContactDetails { get; set; }
		public IEnumerable<CustomerBrand> ContactBrandList { get; set; }

		public IEnumerable<CustomerDepartment> ContactDepartmentList { get; set; }

		public IEnumerable<Service> ContactServiceList { get; set; }
        public IEnumerable<CommonDataSource> ReportToList { get; set; }

        public EditCustomerContactResult Result { get; set; }
    }
    public enum EditCustomerContactResult    {        Success = 1,        CannotGetCustomer = 2,    }
}
