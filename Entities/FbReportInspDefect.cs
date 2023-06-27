using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_InspDefects")]
    public partial class FbReportInspDefect
    {
        public FbReportInspDefect()
        {
            FbReportDefectPhotos = new HashSet<FbReportDefectPhoto>();
        }

        public int Id { get; set; }
        public int FbReportDetailId { get; set; }
        public int InspPoTransactionId { get; set; }
        [StringLength(1500)]
        public string Description { get; set; }
        [StringLength(100)]
        public string Position { get; set; }
        public int? Critical { get; set; }
        public int? Major { get; set; }
        public int? Minor { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool? Active { get; set; }
        public int? DefectId { get; set; }
        public int? CategoryId { get; set; }
        [StringLength(3000)]
        public string CategoryName { get; set; }
        [Column("Qty_Reworked")]
        public int? QtyReworked { get; set; }
        [Column("Qty_Replaced")]
        public int? QtyReplaced { get; set; }
        [Column("Qty_Rejected")]
        public int? QtyRejected { get; set; }
        public int? InspColorTransactionId { get; set; }
        [StringLength(100)]
        public string Code { get; set; }
        [StringLength(100)]
        public string Size { get; set; }
        [StringLength(100)]
        public string Reparability { get; set; }
        [Column("Garment_Grade")]
        [StringLength(100)]
        public string GarmentGrade { get; set; }
        [StringLength(100)]
        public string Zone { get; set; }
        [StringLength(2000)]
        public string DefectInfo { get; set; }
        public int? DefectCheckPoint { get; set; }

        [ForeignKey("FbReportDetailId")]
        [InverseProperty("FbReportInspDefects")]
        public virtual FbReportDetail FbReportDetail { get; set; }
        [ForeignKey("InspColorTransactionId")]
        [InverseProperty("FbReportInspDefects")]
        public virtual InspPurchaseOrderColorTransaction InspColorTransaction { get; set; }
        [ForeignKey("InspPoTransactionId")]
        [InverseProperty("FbReportInspDefects")]
        public virtual InspPurchaseOrderTransaction InspPoTransaction { get; set; }
        [InverseProperty("Defect")]
        public virtual ICollection<FbReportDefectPhoto> FbReportDefectPhotos { get; set; }
    }
}