using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_Additional_Photos")]
    public partial class FbReportAdditionalPhoto
    {
        public int Id { get; set; }
        public int FbReportDetailId { get; set; }
        [StringLength(1000)]
        public string PhotoPath { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool? Active { get; set; }
        [StringLength(1500)]
        public string Description { get; set; }
        public int? ProductId { get; set; }

        [ForeignKey("FbReportDetailId")]
        [InverseProperty("FbReportAdditionalPhotos")]
        public virtual FbReportDetail FbReportDetail { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("FbReportAdditionalPhotos")]
        public virtual CuProduct Product { get; set; }
    }
}