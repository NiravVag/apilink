using DTO.Audit;
using Entities;
using System;
using System.Collections.Generic;

namespace DTO.RepoRequest.Audit
{
    public class AuditSummaryRepoResponse
    {
        //public IEnumerable<AudTransaction> Data { get; set; }

        public IEnumerable<AuditRepoItem> Data { get; set; }

        public int TotalCount { get; set; }

        public IEnumerable<AuditRepoStatus> Statuslst { get; set; }
    }

    public class AuditRepoStatus
    {
        public int Id { get; set; }

        public string StatusName { get; set; }

        public int TotalCount { get; set; }
    }

    public class AuditFactoryCountryRepoResponse
    {
        public int AuditId { get; set; }
        //public int FactoryCountryId { get; set; }
        public string FactoryCountryName { get; set; }
        public string FactoryCity { get; set; }
        public string FactoryState { get; set; }
    }

    public class AuditQuotationRepoResponse
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int AuditId { get; set; }
    }

    public class AuditServiceTypeRepoResponse
    {
        public int AuditId { get; set; }
        //public int ServiceTypeId { get; set; }
        public string ServiceTypeName { get; set; }
    }

    public class AuditAuditorRepoResponse
    {
        public int AuditId { get; set; }
        //public int AuditorId { get; set; }
        public string AuditorName { get; set; }
    }

    public class AuditSupplierCustomerRepoResponse
    {
        public int AuditId { get; set; }
        public string SupplierCustomerCode { get; set; }
    }

    public class AuditFactoryCustomerRepoResponse
    {
        public int AuditId { get; set; }
        public string FactoryCustomerCode { get; set; }
    }

    public class AuditServiceTypeData
    {
        public int AuditId { get; set; }
        public int ServiceTypeId { get; set; }
        public string ServiceTypeName { get; set; }
    }

    public class AuditCustomerContactData
    {
        public int AuditId { get; set; }
        public int ContactId { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
    }
    public class AuditCusFactDetails
    {
        public string CustomerName { get; set; }
        public string FactoryName { get; set; }
        public int AuditId { get; set; }
        public string StatusName { get; set; }
        public DateTime ServiceFromDate { get; set; }
    }
}
