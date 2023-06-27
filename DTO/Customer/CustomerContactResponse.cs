using DTO.CommonClass;
using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerContactResponse
    {
        public CustomerContactDetails CustomerContactDetails { get; set; }        public IEnumerable<CustomerAddressData> CustomerAddressList { get; set; }
        public List<ContactType> ContactTypeList { get; set; }

		public IEnumerable<CustomerBrand> ContactBrandList { get; set; }

		public IEnumerable<CustomerDepartment> ContactDepartmentList { get; set; }

		public IEnumerable<Service> ContactServiceList { get; set; }

		public CustomerContactResult Result { get; set; }
        public IEnumerable<CommonDataSource> ReportToList { get; set; }
    }
    public enum CustomerContactResult    {        Success = 1,        CannotGetAddressList = 2,        CannotGetContactTypes = 3,    }
}
