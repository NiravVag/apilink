using DTO.Customer;
using DTO.OfficeLocation;
using DTO.Supplier;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Inspection
{
    public class InspectionBookingSummaryResponse
    {
        public IEnumerable<CustomerItem> CustomerList { get; set; }

        public IEnumerable<SupplierItem> SupplierList { get; set; }

        public IEnumerable<SupplierItem> FactoryList { get; set; }

        public IEnumerable<Office> OfficeList { get; set; }

        //public IEnumerable<AuditStatus> StatusList { get; set; }

        public InspectionBookingResponseResult Result { get; set; }
    }

    public enum InspectionBookingResponseResult
    {
        success = 1,
        failed = 2
    }
}
