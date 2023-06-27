using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ES_Details")]
    public partial class EsDetail
    {
        public EsDetail()
        {
            EsAdditionalRecipients = new HashSet<EsAdditionalRecipient>();
            EsApiContacts = new HashSet<EsApiContact>();
            EsCuConfigs = new HashSet<EsCuConfig>();
            EsCuContacts = new HashSet<EsCuContact>();
            EsFaCountryConfigs = new HashSet<EsFaCountryConfig>();
            EsOfficeConfigs = new HashSet<EsOfficeConfig>();
            EsProductCategoryConfigs = new HashSet<EsProductCategoryConfig>();
            EsRecipientTypes = new HashSet<EsRecipientType>();
            EsResultConfigs = new HashSet<EsResultConfig>();
            EsServiceTypeConfigs = new HashSet<EsServiceTypeConfig>();
            EsSpecialRules = new HashSet<EsSpecialRule>();
            EsSupFactConfigs = new HashSet<EsSupFactConfig>();
        }

        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int ServiceId { get; set; }
        public int TypeId { get; set; }
        public bool? Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        [Column("Recipient_Name")]
        public string RecipientName { get; set; }
        [Column("No_Of_Reports")]
        public int? NoOfReports { get; set; }
        [Column("Report_In_Email")]
        public int? ReportInEmail { get; set; }
        [Column("Email_Size")]
        public int? EmailSize { get; set; }
        [Column("Email_Subject")]
        public int? EmailSubject { get; set; }
        [Column("Report_Send_Type")]
        public int? ReportSendType { get; set; }
        public int? EntityId { get; set; }
        [Column("File_Name_Id")]
        public int? FileNameId { get; set; }
        public bool? IsPictureFileInEmail { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? InvoiceTypeId { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("EsDetails")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("EsDetails")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("EsDetailDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EmailSize")]
        [InverseProperty("EsDetails")]
        public virtual EsRefEmailSize EmailSizeNavigation { get; set; }
        [ForeignKey("EmailSubject")]
        [InverseProperty("EsDetailEmailSubjectNavigations")]
        public virtual EsSuTemplateMaster EmailSubjectNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("EsDetails")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("FileNameId")]
        [InverseProperty("EsDetailFileNames")]
        public virtual EsSuTemplateMaster FileName { get; set; }
        [ForeignKey("InvoiceTypeId")]
        [InverseProperty("EsDetails")]
        public virtual RefInvoiceType InvoiceType { get; set; }
        [ForeignKey("ReportInEmail")]
        [InverseProperty("EsDetails")]
        public virtual EsRefReportInEmail ReportInEmailNavigation { get; set; }
        [ForeignKey("ReportSendType")]
        [InverseProperty("EsDetails")]
        public virtual EsRefReportSendType ReportSendTypeNavigation { get; set; }
        [ForeignKey("ServiceId")]
        [InverseProperty("EsDetails")]
        public virtual RefService Service { get; set; }
        [ForeignKey("TypeId")]
        [InverseProperty("EsDetails")]
        public virtual EsType Type { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("EsDetailUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("EmailDetail")]
        public virtual ICollection<EsAdditionalRecipient> EsAdditionalRecipients { get; set; }
        [InverseProperty("EsDetails")]
        public virtual ICollection<EsApiContact> EsApiContacts { get; set; }
        [InverseProperty("EsDetails")]
        public virtual ICollection<EsCuConfig> EsCuConfigs { get; set; }
        [InverseProperty("EsDetails")]
        public virtual ICollection<EsCuContact> EsCuContacts { get; set; }
        [InverseProperty("EsDetails")]
        public virtual ICollection<EsFaCountryConfig> EsFaCountryConfigs { get; set; }
        [InverseProperty("EsDetails")]
        public virtual ICollection<EsOfficeConfig> EsOfficeConfigs { get; set; }
        [InverseProperty("EsDetails")]
        public virtual ICollection<EsProductCategoryConfig> EsProductCategoryConfigs { get; set; }
        [InverseProperty("EsDetails")]
        public virtual ICollection<EsRecipientType> EsRecipientTypes { get; set; }
        [InverseProperty("EsDetails")]
        public virtual ICollection<EsResultConfig> EsResultConfigs { get; set; }
        [InverseProperty("EsDetails")]
        public virtual ICollection<EsServiceTypeConfig> EsServiceTypeConfigs { get; set; }
        [InverseProperty("EsDetails")]
        public virtual ICollection<EsSpecialRule> EsSpecialRules { get; set; }
        [InverseProperty("EsDetails")]
        public virtual ICollection<EsSupFactConfig> EsSupFactConfigs { get; set; }
    }
}