using DTO.Common;
using DTO.CommonClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DTO.EmailSend
{
    public class EmailSubjectConfig
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public string TemplateDisplayName { get; set; }

        [Required(ErrorMessage = "EMAIL_SUBJECT_CONFIG.MSG_EMAIL_TYPE_REQ")]
        public int EmailTypeId { get; set; }

        [Required(ErrorMessage = "EMAIL_SUBJECT_CONFIG.MSG_MODULE_REQ")]
        public int ModuleId { get; set; }

        [Required(ErrorMessage = "EMAIL_SUBJECT_CONFIG.MSG_DELIMITER_REQ")]
        public int DelimiterId { get; set; }

        [Required(ErrorMessage = "EMAIL_SUBJECT_CONFIG.MSG_DELIMITER_REQ")]
        public string Delimiter { get; set; }

        [StringLength(1500)]
        public string TemplateName { get; set; }

        [RequiredList(ErrorMessage = "EMAIL_SUBJECT_CONFIG.MSG_TEMP_LIST_IS_REQUIRED")]
        public List<SubConfigColumnModel> TemplateColumnList { get; set; }
    }

    public class SubConfigSaveResponse
    {
        public int Id { get; set; }
        public Result Result { get; set; }
    }

    public enum Result
    {
        Success = 1,
        NotFound = 2,
        Failure = 3,
        RequestNotCorrectFormat = 4,
        TemplateNameExists = 5,
        TemplateFieldsExists = 6,
        NoExists = 7,
        MappedEmailRule = 8
    }

    public class SubConfigColumnModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AlaisName { get; set; }
        public int? MaxChar { get; set; }
        public bool? IsTitle { get; set; }
        public string TitleCustomName { get; set; }
        public int? MaxItems { get; set; }
        public int? DateFormat { get; set; }
        public bool? IsDateSeperator { get; set; }
        public int Sort { get; set; }
        public int FieldId { get; set; }
    }

    public class SubConfigEditResponse
    {
        public SubConfigEdit editDetails { get; set; }
        public Result Result { get; set; }
        public bool IsUseByEmailSend { get; set; }
    }

    public class SubConfigEdit
    {
        public int? CustomerId { get; set; }
        public string TemplateName {get;set;}
        public int? EmailTypeId { get; set; }
        public int? ModuleId { get; set; }
        public int? DelimiterId { get; set; }
        public string Delimiter { get; set; }
        public string TemplateDisplayName { get; set; }
        public int Id { get; set; }
        public List<SubConfigColumnEdit> TemplateColumnList { get; set; }
    }

    public class SubConfigColumnEdit
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string AlaisName { get; set; }
        public int? MaxChar { get; set; }
        public bool? IsTitle { get; set; }
        public string TitleCustomName { get; set; }
        public int? MaxItems { get; set; }
        public int? DateFormat { get; set; }
        public bool? IsDateSeperator { get; set; }
        public int Sort { get; set; }
        public int FieldId { get; set; }
        public bool? IsText { get; set; }
        public int? DataType { get; set; }
    }

    public class TemplateDetailsRepo
    {
        public int? FieldId { get; set; }
        public int? TemplateId { get; set; }
    }

    public class PreDefinedFieldColumn
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? MaxChar { get; set; }
        public int? DataType { get; set; }
        public bool? IsText { get; set; }
    }

    public class PreDefinedColSourceResponse
    {
        public IEnumerable<PreDefinedFieldColumn> DataSourceList { get; set; }
        public DataSourceResult Result { get; set; }
    }

    public class DeleteResponse
    {
        public Result Result { get; set; }
    }
}