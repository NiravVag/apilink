using DTO.Customer;
using DTO.OfficeLocation;
using DTO.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Claim
{
    public class ClaimSummaryResponse
    {
        public IEnumerable<CustomerItem> CustomerList { get; set; }

        public IEnumerable<SupplierItem> SupplierList { get; set; }
        public IEnumerable<ClaimStatuses> StatusList { get; set; }
        public IEnumerable<Office> OfficeList { get; set; }
        public ClaimSummaryResult Result { get; set; }
    }

    public class ClaimStatuses
    {
        public int Id { get; set; }

        public string Label { get; set; }
    }

    public enum ClaimSummaryResult
    {
        Success = 1,
        CustomerListNotFound = 2,
        CannotFindOfficeList = 3,
        CannotFindStatusList = 4,
        CannotFindSupplierList = 5
    }

    public class DataSource
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public int InvoiceType { get; set; }

        public bool IsForwardToManager { get; set; }

        public int? CountyId { get; set; }
    }
}
