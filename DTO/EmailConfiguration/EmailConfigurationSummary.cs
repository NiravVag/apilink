using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTO.EmailConfiguration
{
    public class EmailConfigurationSummary
    {
        public int? CustomerId { get; set; }
        public int? ServiceId { get; set; }
        public List<int> ServiceTypeIdList { get; set; }
        public List<int> FactoryCountryIdList { get; set; }
        public List<int> DepartmentIdList { get; set; }
        public List<int> OfficeIdList { get; set; }
        public List<int> BrandIdList { get; set; }
        public List<int> ProductCategoryIdList { get; set; }
        public List<int> ResultIdList { get; set; }
        public int? Index { get; set; }
        public int? PageSize { get; set; }
        public int? TypeId { get; set; }
    }

    public class EmailConfigSummaryItem
    {
        public string CustomerName { get; set; }
        public string ServiceTypeName { get; set; }
        public string OfficeName { get; set; }
        public string FactoryCountryName { get; set; }
        public string DepartmentName { get; set; }
        public string BrandName { get; set; }
        public string ProductcategoryName { get; set; }
        public string ResultName { get; set; }
        public int EmailConfigId { get; set; }
        public string Service { get; set; }
        public string ReportSendType { get; set; }
        public string ReportInEmail { get; set; }
        public string EmailTypeName { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedOn { get; set; }
    }

    public class EmailConfigurationSummaryResponse
    {
        public List<EmailConfigSummaryItem> Data { get; set; }
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public EmailConfigResult Result { get; set; }
    }
    public class EmailConfigurationDeleteResponse
    {
        public EmailConfigResult Result { get; set; }
    }

    public class EmailConfigSearchRepo
    {
        public string CustomerName { get; set; }
        public string ServiceName { get; set; }
        public int EmailConfigId { get; set; }
        public int? CustomerId { get; set; }
        public int? ServiceId { get; set; }
        public string ReportSendType { get; set; }
        public string ReportInEmail { get; set; }
        public string EmailTypeName { get; set; }
        public int EmailTypeId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        //public IEnumerable<string> ServiceTypeNameList { get; set; }
        //public IEnumerable<int> ServiceTypeIdList { get; set; }
    }

    public class EmailConfigFactoryCountryRepo
    {
        public string FactoryCountryName { get; set; }
        public int FactoryCountryId { get; set; }
        public int EmailConfigId { get; set; }
    }

    public class EmailConfigServiceTypeRepo
    {
        public string ServiceTypeName { get; set; }
        public int ServiceTypeId { get; set; }
        public int EmailConfigId { get; set; }
    }

    public class EmailConfigOfficeRepo
    {
        public string OfficeName { get; set; }
        public int? OfficeId { get; set; }
        public int EmailConfigId { get; set; }
    }

    public class EmailConfigDeptRepo
    {
        public string DepartmentName { get; set; }
        public int DepartmentId { get; set; }
        public int EmailConfigId { get; set; }
    }

    public class EmailConfigBrandRepo
    {
        public string BrandName { get; set; }
        public int BrandId { get; set; }
        public int EmailConfigId { get; set; }
    }

    public class EmailConfigProductCategoryRepo
    {
        public string ProductCategoryName { get; set; }
        public int ProductCategoryId { get; set; }
        public int EmailConfigId { get; set; }
    }
    public class EmailConfigResultRepo
    {
        public string ResultName { get; set; }
        public int ResultId { get; set; }
        public int EmailConfigId { get; set; }
    }

    public class EmailSubjectRepo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CustomerId { get; set; }
        public int? EmailTypeId { get; set; }
    }

    public class EmailSubRequest
    {
        public int? CustomerId { get; set; }
        public int? EmailTypeId { get; set; }
    }

    public class EmailConfigParameterMap
    {
        public List<EmailConfigResultRepo> EmailConfigResultRepo { get; set; }
        public List<EmailConfigProductCategoryRepo> EmailConfigProductCategoryRepo { get; set; }
        public List<EmailConfigDeptRepo> EmailConfigDeptRepo { get; set; }
        public List<EmailConfigBrandRepo> EmailConfigBrandRepo { get; set; }
        public List<EmailConfigOfficeRepo> EmailConfigOfficeRepo { get; set; }
        public List<EmailConfigServiceTypeRepo> EmailConfigServiceTypeRepo { get; set; }
        public List<EmailConfigFactoryCountryRepo> EmailConfigFactoryCountryRepo { get; set; }
        public IQueryable<EmailConfigSearchRepo> EmailConfigSearchRepo { get; set; }
    }
}
