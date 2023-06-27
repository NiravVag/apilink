using DTO.CommonClass;
using DTO.Customer;
using DTO.HumanResource;
using DTO.Location;
using DTO.OfficeLocation;
using DTO.References;
using DTO.Supplier;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Audit
{
   public class AuditSummaryResponse
    {
        public IEnumerable<CommonDataSource> CustomerList { get; set; }

        public IEnumerable<SupplierItem> SupplierList { get; set; }

        public IEnumerable<SupplierItem> FactoryList { get; set; }

        public IEnumerable<Office> OfficeList { get; set; }

        public IEnumerable<AuditStatus> StatusList { get; set; }

        public AuditSummaryResponseResult Result { get; set; }
    }

    public enum AuditSummaryResponseResult
    {
        success=1,
        failed=2
    }
}
