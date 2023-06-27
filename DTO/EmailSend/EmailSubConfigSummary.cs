using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.EmailSend
{
    public class EmailSubConfigSummary
    {
        public List<int?> CustomerIds { get; set; }
        public int? EmailTypeId { get; set; }
        public int? ModuleId { get; set; }
        public int? Index { get; set; }
        public int? PageSize { get; set; }
        public string TemplateName { get; set; }
    }

    public class EmailSubConfigSummaryItem
    {
        public string CustomerName { get; set; }
        public string TemplateName { get; set; }
        public string TemplateDisplayName { get; set; }
        public int SubConfigId { get; set; }
        public bool IsDelete { get; set; }
        public string EmailType { get; set; }
        public string ModuleName { get; set; }
    }

    public enum EmailSubConfigSummaryResult
    {
        Success = 1,
        NotFound = 2,
        Fail = 3,
        RequestNotCorrectFormat = 4
    }

    public class EmailSubConfigSummaryResponse
    {
        public List<EmailSubConfigSummaryItem> Data { get; set; }
        public int TotalCount { get; set; }
        public int Index { get; set; }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public EmailSubConfigSummaryResult Result { get; set; }
    }

    public class EmailSubConfigRepo
    {
        public string CustomerName { get; set; }
        public string TemplateDisplayName { get; set; }
        public int SubConfigId { get; set; }
        public int? CustomerId { get; set; }
        public string TemplateName { get; set; }
        public int? EmailTypeId { get; set; }
        public int? ModuleId { get; set; }
        public string EmailType { get; set; }
        public string ModuleName { get; set; }
    }

    public class TemplateDetailRepo
    {
        public string FieldName { get; set; }
        public int? TemplateId { get; set; }
    }

    public class EmailTemplate
    {
        public int SubjectId { get; set; }
        public int FileNameId { get; set; }
    }
}
