using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("QU_PDFVersion")]
    public partial class QuPdfversion
    {
        public Guid GuidId { get; set; }
        [Required]
        [StringLength(200)]
        public string FileName { get; set; }
        public byte[] File { get; set; }
        public int UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime GenerateDate { get; set; }
        public int QuotationId { get; set; }

        [ForeignKey("QuotationId")]
        [InverseProperty("QuPdfversions")]
        public virtual QuQuotation Quotation { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("QuPdfversions")]
        public virtual ItUserMaster User { get; set; }
    }
}