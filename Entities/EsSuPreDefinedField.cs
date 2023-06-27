using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ES_SU_PreDefined_Fields")]
    public partial class EsSuPreDefinedField
    {
        public EsSuPreDefinedField()
        {
            EsSuTemplateDetails = new HashSet<EsSuTemplateDetail>();
        }

        public int Id { get; set; }
        [Column("Field_Name")]
        public string FieldName { get; set; }
        [Column("Field_Alias_Name")]
        public string FieldAliasName { get; set; }
        [Column("Max_Char")]
        public int? MaxChar { get; set; }
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
        public bool? IsText { get; set; }
        public int? DataType { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("EsSuPreDefinedFieldCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DataType")]
        [InverseProperty("EsSuPreDefinedFields")]
        public virtual EsSuDataType DataTypeNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("EsSuPreDefinedFieldDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("EsSuPreDefinedFields")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("EsSuPreDefinedFieldUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("Field")]
        public virtual ICollection<EsSuTemplateDetail> EsSuTemplateDetails { get; set; }
    }
}