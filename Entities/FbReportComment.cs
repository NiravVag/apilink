using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("FB_Report_Comments")]
    public partial class FbReportComment
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public string Comments { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool? Active { get; set; }
        public int FbReportId { get; set; }
        [StringLength(2000)]
        public string Category { get; set; }
        [Column("Sub_Category")]
        [StringLength(2000)]
        public string SubCategory { get; set; }
        [Column("Sub_Category2")]
        [StringLength(2000)]
        public string SubCategory2 { get; set; }
        [StringLength(2000)]
        public string CustomerReferenceCode { get; set; }

        [ForeignKey("FbReportId")]
        [InverseProperty("FbReportComments")]
        public virtual FbReportDetail FbReport { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("FbReportComments")]
        public virtual CuProduct Product { get; set; }
    }
}