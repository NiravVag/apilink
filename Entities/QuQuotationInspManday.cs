using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("QU_Quotation_Insp_Manday")]
    public partial class QuQuotationInspManday
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
        public bool? Active { get; set; }

        [ForeignKey("BookingId")]
        [InverseProperty("QuQuotationInspMandays")]
        public virtual InspTransaction Booking { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("QuQuotationInspMandayCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("QuQuotationInspMandayDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("QuotationId")]
        [InverseProperty("QuQuotationInspMandays")]
        public virtual QuQuotation Quotation { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("QuQuotationInspMandayUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}