using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("QU_Quotation_Pdf_Version")]
    public partial class QuQuotationPdfVersion
    {
        public int Id { get; set; }
        [StringLength(1000)]
        public string UniqueId { get; set; }
        [StringLength(500)]
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public int? UserId { get; set; }
        [Column("Quotation_Id")]
        public int? QuotationId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UploadDate { get; set; }
        public bool? SendToClient { get; set; }

        [ForeignKey("QuotationId")]
        [InverseProperty("QuQuotationPdfVersions")]
        public virtual QuQuotation Quotation { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("QuQuotationPdfVersions")]
        public virtual ItUserMaster User { get; set; }
    }
}