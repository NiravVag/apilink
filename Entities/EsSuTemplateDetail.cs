using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ES_SU_Template_Details")]
    public partial class EsSuTemplateDetail
    {
        public int Id { get; set; }
        [Column("Field_Id")]
        public int? FieldId { get; set; }
        [Column("Template_Id")]
        public int? TemplateId { get; set; }
        public int? Sort { get; set; }
        [Column("Max_Char")]
        public int? MaxChar { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public bool? IsTitle { get; set; }
        [StringLength(400)]
        public string TitleCustomName { get; set; }
        [Column("Max_Items")]
        public int? MaxItems { get; set; }
        public int? DateFormat { get; set; }
        public bool? IsDateSeperator { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("EsSuTemplateDetails")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DateFormat")]
        [InverseProperty("EsSuTemplateDetails")]
        public virtual RefDateFormat DateFormatNavigation { get; set; }
        [ForeignKey("FieldId")]
        [InverseProperty("EsSuTemplateDetails")]
        public virtual EsSuPreDefinedField Field { get; set; }
        [ForeignKey("TemplateId")]
        [InverseProperty("EsSuTemplateDetails")]
        public virtual EsSuTemplateMaster Template { get; set; }
    }
}