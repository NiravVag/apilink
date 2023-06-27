using DTO.Common;
using System.Collections.Generic;

namespace DTO.EmailConfiguration
{
    public class EmailConfiguration
    {
        public int Id { get; set; }
        public IEnumerable<int> CustomerContactIdList { get; set; }
        public IEnumerable<int> APIContactIdList { get; set; }
        public string RecipientName { get; set; }
        
        public int? CustomerId { get; set; }

        [RequiredGreaterThanZero(ErrorMessage = "COMMON.MSG_SERVICE_REQUIRED")]
        public int ServiceId { get; set; }

        public int? NumberOfReports { get; set; }
        public int? ReportInEmailId { get; set; }
        public int? EmailSizeId { get; set; }
        public int? ReportSendTypeId { get; set; }
        public int? EmailSubjectId { get; set; }

        [RequiredGreaterThanZero(ErrorMessage = "EMAIL_CONFIGURATION.MSG_EMAIL_SEND_REQUIRED")]
        public int TypeId { get; set; }
        public int? FileNameId { get; set; }
        public IEnumerable<int?> OfficeIdList { get; set; }
        public IEnumerable<int> CusDecisionIdList { get; set;}
        public IEnumerable<int> ServiceTypeIdList { get; set; }
        public IEnumerable<int> ProductCategoryIdList { get; set; }
        public IEnumerable<int> ApiResultIdList { get; set; }
        public IEnumerable<int> SupplierIdList { get; set; }
        public IEnumerable<int> FactoryIdList { get; set; }
        public IEnumerable<int> FactoryCountryIdList { get; set; }
        public IEnumerable<int> DepartmentIdList { get; set; }
        public IEnumerable<int> BrandIdList { get; set; }
        public IEnumerable<int> BuyerIdList { get; set; }
        public IEnumerable<int> CollectionIdList { get; set; }
        public IEnumerable<int> SpecialRuleIdList { get; set; }
        public IEnumerable<int> ToIdList { get; set; }
        public IEnumerable<int> CCIdList { get; set; }        
        public bool? IsPictureFileInEmail { get; set; }
        public List<AdditionalEmailRecipient> AdditionalEmailRecipients { get; set; }
        public int? InvoiceTypeId { get; set; }
    }

    public class EditEmailConfiguration
    {
        public int Id { get; set; }
        public IEnumerable<int> CustomerContactIdList { get; set; }
        public IEnumerable<int> APIContactIdList { get; set; }        
        public string RecipientName { get; set; }

        public int? CustomerId { get; set; }
        public int ServiceId { get; set; }
        public int? NumberOfReports { get; set; }
        public int? ReportInEmailId { get; set; }
        public int? EmailSizeId { get; set; }
        public int? ReportSendTypeId { get; set; }
        public int? ReportSubjectId { get; set; }

        public IEnumerable<int?> OfficeIdList { get; set; }
        public IEnumerable<int> ServiceTypeIdList { get; set; }
        public IEnumerable<int> ProductCategoryIdList { get; set; }
        public IEnumerable<int?> ApiResultIdList { get; set; }
        public IEnumerable<int> SupplierIdList { get; set; }
        public IEnumerable<int> FactoryIdList { get; set; }
        public IEnumerable<int> FactoryCountryIdList { get; set; }
        public IEnumerable<int?> DepartmentIdList { get; set; }
        public IEnumerable<int?> BrandIdList { get; set; }
        public IEnumerable<int?> BuyerIdList { get; set; }
        public IEnumerable<int?> CollectionIdList { get; set; }
        public IEnumerable<int?> SpecialRuleIdList { get; set; }
        public int TypeId { get; set; }
        public int? EmailSubjectId { get; set; }
        public IEnumerable<int?> CusDecisionIdList { get; set; }
        public int? FileNameId { get; set; }
        public IEnumerable<int?> ToIdList { get; set; }
        public IEnumerable<int?> CCIdList { get; set; }
        public IEnumerable<AdditionalEmailRecipient> AdditionalEmailRecipients { get; set; }
        public bool? isPictureFileInEmail { get; set; }
        public int? InvoiceTypeId { get; set; }
    }

    public class EmailConfigSaveReponse
    {
        public int Id { get; set; }
        public EmailConfigResult Result { get; set; }
    }

    public enum EmailConfigResult
    {
        Success = 1,
        Error = 2,
        RequestNotCorrectFormat = 3,
        NotFound = 4,
        Exists = 5
    }

    public class EmailEditResponse
    {
        public EditEmailConfiguration EmailSendDetails { get; set; }
        public EmailConfigResult Result { get; set; }
    }
    public class AdditionalEmailRecipient
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public int? RecipientId { get; set; }
        public string RecipientType { get; set; }
    }
}
