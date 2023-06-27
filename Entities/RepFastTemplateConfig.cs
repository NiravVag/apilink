using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REP_FAST_Template_Config")]
    public partial class RepFastTemplateConfig
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? TemplateId { get; set; }
        public int? ServiceTypeId { get; set; }
        public int? ProductCategoryId { get; set; }
        public bool? IsStandardTemplate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ScheduleFromDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ScheduleToDate { get; set; }
        public bool? Active { get; set; }
        public int? Sort { get; set; }
        public int? BrandId { get; set; }
        public int? DepartId { get; set; }
        [Column("FileExtensionID")]
        public int? FileExtensionId { get; set; }
        [Column("ReportToolTypeID")]
        public int? ReportToolTypeId { get; set; }
        public int? Entityid { get; set; }

        [ForeignKey("BrandId")]
        [InverseProperty("RepFastTemplateConfigs")]
        public virtual CuBrand Brand { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("RepFastTemplateConfigs")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("DepartId")]
        [InverseProperty("RepFastTemplateConfigs")]
        public virtual CuDepartment Depart { get; set; }
        [ForeignKey("Entityid")]
        [InverseProperty("RepFastTemplateConfigs")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("FileExtensionId")]
        [InverseProperty("RepFastTemplateConfigs")]
        public virtual RefFileExtension FileExtension { get; set; }
        [ForeignKey("ProductCategoryId")]
        [InverseProperty("RepFastTemplateConfigs")]
        public virtual RefProductCategory ProductCategory { get; set; }
        [ForeignKey("ReportToolTypeId")]
        [InverseProperty("RepFastTemplateConfigs")]
        public virtual RepRefToolType ReportToolType { get; set; }
        [ForeignKey("ServiceTypeId")]
        [InverseProperty("RepFastTemplateConfigs")]
        public virtual RefServiceType ServiceType { get; set; }
        [ForeignKey("TemplateId")]
        [InverseProperty("RepFastTemplateConfigs")]
        public virtual RepFastTemplate Template { get; set; }
    }
}