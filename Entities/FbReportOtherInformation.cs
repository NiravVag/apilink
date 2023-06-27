using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_OtherInformation")]
    public partial class FbReportOtherInformation
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        [StringLength(1000)]
        public string SubCategory { get; set; }
        public string Remarks { get; set; }
        [StringLength(2000)]
        public string Result { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool? Active { get; set; }
        public int FbReportId { get; set; }
        [Column("Sub_Category2")]
        [StringLength(1000)]
        public string SubCategory2 { get; set; }

        [ForeignKey("FbReportId")]
        [InverseProperty("FbReportOtherInformations")]
        public virtual FbReportDetail FbReport { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("FbReportOtherInformations")]
        public virtual CuProduct Product { get; set; }
    }
}