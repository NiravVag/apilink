using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("KPI_Column")]
    public partial class KpiColumn
    {
        public KpiColumn()
        {
            KpiTemplateColumns = new HashSet<KpiTemplateColumn>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        public string FieldLabel { get; set; }
        [Required]
        [StringLength(300)]
        public string FieldName { get; set; }
        [StringLength(20)]
        public string FieldType { get; set; }
        public int? IdSubModule { get; set; }
        public int? IdModule { get; set; }
        public bool CanFilter { get; set; }
        public bool CanShowInResult { get; set; }
        public bool? FilterIsMultiple { get; set; }
        [StringLength(200)]
        public string FilterDataSourceName { get; set; }
        public int? FilterDataSourceTypeId { get; set; }
        [StringLength(100)]
        public string FilterDataSourceFieldValue { get; set; }
        [StringLength(100)]
        public string FilterDataSourceFieldName { get; set; }
        [StringLength(100)]
        public string FilterDataSourceFieldCondition { get; set; }
        [StringLength(100)]
        public string FilterDataSourceFieldConditionValue { get; set; }
        public bool FilterRequired { get; set; }
        public int? FilterSignEqualityId { get; set; }
        public bool Active { get; set; }
        public bool IsLocationId { get; set; }
        public bool IsCustomerId { get; set; }
        public bool IsKey { get; set; }

        [ForeignKey("FilterDataSourceTypeId")]
        [InverseProperty("KpiColumns")]
        public virtual RefDataSourceType FilterDataSourceType { get; set; }
        [ForeignKey("FilterSignEqualityId")]
        [InverseProperty("KpiColumns")]
        public virtual RefSignEquality FilterSignEquality { get; set; }
        [ForeignKey("IdModule")]
        [InverseProperty("KpiColumns")]
        public virtual ApModule IdModuleNavigation { get; set; }
        [ForeignKey("IdSubModule")]
        [InverseProperty("KpiColumns")]
        public virtual ApSubModule IdSubModuleNavigation { get; set; }
        [InverseProperty("IdColumnNavigation")]
        public virtual ICollection<KpiTemplateColumn> KpiTemplateColumns { get; set; }
    }
}