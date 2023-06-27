using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_Defect_Photos")]
    public partial class FbReportDefectPhoto
    {
        public int Id { get; set; }
        public int DefectId { get; set; }
        [StringLength(1500)]
        public string Description { get; set; }
        [StringLength(1000)]
        public string Path { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool? Active { get; set; }

        [ForeignKey("DefectId")]
        [InverseProperty("FbReportDefectPhotos")]
        public virtual FbReportInspDefect Defect { get; set; }
    }
}