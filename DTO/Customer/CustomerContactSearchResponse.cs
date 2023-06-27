using DTO.References;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Customer
{
    public class CustomerContactSearchResponse
    {
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }

        public IEnumerable<CustomerContact> Data { get; set; }
        public CustomerContactSearchResult Result { get; set; }

		public List<int> ContactBrandList { get; set; }

		public List<int> ContactDepartmentList { get; set; }

		public List<int> ContactServiceList { get; set; }

		public IEnumerable<CustomerBrand> BrandList { get; set; }

		public IEnumerable<CustomerDepartment> DepartmentList { get; set; }

		public IEnumerable<Service> ServiceList { get; set; }

	}

    public enum CustomerContactSearchResult
    {
        Success = 1,
        NotFound = 2
    }
}
