using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ES_SU_Template_Master")]
    public partial class EsSuTemplateMaster
    {
        public EsSuTemplateMaster()
        {
            EsDetailEmailSubjectNavigations = new HashSet<EsDetail>();
            EsDetailFileNames = new HashSet<EsDetail>();
            EsSuTemplateDetails = new HashSet<EsSuTemplateDetail>();
        }

        public int Id { get; set; }
        [Column("Template_Name")]
        public string TemplateName { get; set; }
        [Column("Template_Display_Name")]
        public string TemplateDisplayName { get; set; }
        [Column("Customer_Id")]
        public int? CustomerId { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public int? EntityId { get; set; }
        [Column("Email_Type_Id")]
        public int? EmailTypeId { get; set; }
        [Column("Module_Id")]
        public int? ModuleId { get; set; }
        [Column("Delimiter_Id")]
        public int? DelimiterId { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("EsSuTemplateMasterCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("EsSuTemplateMasters")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("EsSuTemplateMasterDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("DelimiterId")]
        [InverseProperty("EsSuTemplateMasters")]
        public virtual RefDelimiter Delimiter { get; set; }
        [ForeignKey("EmailTypeId")]
        [InverseProperty("EsSuTemplateMasters")]
        public virtual EsType EmailType { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("EsSuTemplateMasters")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("ModuleId")]
        [InverseProperty("EsSuTemplateMasters")]
        public virtual EsSuModule Module { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("EsSuTemplateMasterUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("EmailSubjectNavigation")]
        public virtual ICollection<EsDetail> EsDetailEmailSubjectNavigations { get; set; }
        [InverseProperty("FileName")]
        public virtual ICollection<EsDetail> EsDetailFileNames { get; set; }
        [InverseProperty("Template")]
        public virtual ICollection<EsSuTemplateDetail> EsSuTemplateDetails { get; set; }
    }
}