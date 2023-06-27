using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Customer
{
    public class CSAllocation
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Services { get; set; }
        public string Brands { get; set; }
        public string Offices { get; set; }
        public string CS { get; set; }
        public bool? BackupCS { get; set; }
        public string ReportChecker { get; set; }
        public bool? BackupReportChecker { get; set; }
        public string Departments { get; set; }
        public string ProductCategory { get; set; }
        public string FactoryCountries { get; set; }
        public string StaffName { get; set; }
    }

    public class ExportCSAllocationData
    {
        [Description("Customer Name")]
        public string CustomerName { get; set; }
        [Description("Services")]
        public string Services { get; set; }
        [Description("Brands")]
        public string Brands { get; set; }
        [Description("Offices")]
        public string Offices { get; set; }
        public string CS { get; set; }
        [Description("Backup CS")]
        public bool? BackupCS { get; set; }
        public string ReportChecker { get; set; }
        [Description("Backup Report Checker")]
        public bool? BackupReportChecker { get; set; }
        [Description("Departments")]
        public string Departments { get; set; }
        [Description("Product Category")]
        public string ProductCategory { get; set; }

        [Description("Factory Country")]
        public string FactoryCountry { get; set; }
    }
    public class CSAllocationData
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int UserId { get; set; }        
        public bool? BackupCS { get; set; }
        public bool? BackupReportChecker { get; set; }
        public int UserType { get; set; }
        public string StaffName { get; set; }

    }
    public class CSAllocationCommonDataSource
    {
        public int Id { get; set; }
        public int DaUserCustomerId { get; set; }
        public string Name { get; set; }
    }
}
