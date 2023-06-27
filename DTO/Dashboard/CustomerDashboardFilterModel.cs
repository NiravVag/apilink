using DTO.Common;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Dashboard
{
    public class CustomerDashboardFilterRequest
    {
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public int? FactoryId { get; set; }
        public IEnumerable<int?> SelectedCountryIdList { get; set; }
        public DateObject ServiceDateFrom { get; set; }
        public DateObject ServiceDateTo { get; set; }
        public IEnumerable<int?> SelectedDeptIdList { get; set; }
        public IEnumerable<int?> SelectedBrandIdList { get; set; }
        public IEnumerable<int?> SelectedBuyerIdList { get; set; }
        public IEnumerable<int?> SelectedCollectionIdList { get; set; }
        public IEnumerable<int> StatusIdList { get; set; }
        public IEnumerable<int?> SelectedFactIdList { get; set; }
        public List<int?> ProdCategoryList { get; set; }
        public List<int?> ProductIdList { get; set; }
    }

    public class DashboardMapFilterRequest
    {
        public int? CustomerId { get; set; }
        public int? SupplierId { get; set; }
        public DateObject ServiceDateFrom { get; set; }
        public DateObject ServiceDateTo { get; set; }
        public List<int> FactoryIds { get; set; }
        public List<int> StatusIds { get; set; }
        public List<int> CountryIds { get; set; }
        public List<int> SupplierIds { get; set; }
        public List<int?> OfficeIds { get; set; }
        public List<int> ProductCategoryIds { get; set; }
        public List<int> ProductIds { get; set; }
        public List<int> DepartmentIds { get; set; }
        public List<int> BrandIds { get; set; }
        public List<int> BuyerIds { get; set; }
        public List<int?> CollectionIds { get; set; }
        public int DashboardType { get; set; }

    }

    public enum DashboardMapEnum
    {
        CustomerDashboard = 1,
        ManagementDashboard = 2
    }

    public enum ManDaySearchEnum
    {
        Daily = 1,
        Weekly = 2,
        Monthly = 3
    }
}
