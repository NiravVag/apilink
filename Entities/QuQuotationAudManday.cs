using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("QU_Quotation_Aud_Manday")]
    public partial class QuQuotationAudManday
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int QuotationId { get; set; }
        public double? NoOfManday { get; set; }
        public string Remarks { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ServiceDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }

        [ForeignKey("BookingId")]
        [InverseProperty("QuQuotationAudMandays")]
        public virtual AudTransaction Booking { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("QuQuotationAudMandayCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("QuQuotationAudMandayDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("QuotationId")]
        [InverseProperty("QuQuotationAudMandays")]
        public virtual QuQuotation Quotation { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("QuQuotationAudMandayUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}