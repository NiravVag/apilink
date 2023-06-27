using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_RDNumbers")]
    public partial class FbReportRdnumber
    {
        public int Id { get; set; }
        public int? ReportdetailsId { get; set; }
        public int? ProductId { get; set; }
        [Column("RDNumber")]
        [StringLength(500)]
        public string Rdnumber { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? PoId { get; set; }
        public int? PoColorId { get; set; }

        [ForeignKey("PoId")]
        [InverseProperty("FbReportRdnumbers")]
        public virtual InspPurchaseOrderTransaction Po { get; set; }
        [ForeignKey("PoColorId")]
        [InverseProperty("FbReportRdnumbers")]
        public virtual InspPurchaseOrderColorTransaction PoColor { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("FbReportRdnumbers")]
        public virtual InspProductTransaction Product { get; set; }
        [ForeignKey("ReportdetailsId")]
        [InverseProperty("FbReportRdnumbers")]
        public virtual FbReportDetail Reportdetails { get; set; }
    }
}