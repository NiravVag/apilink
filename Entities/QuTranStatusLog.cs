using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("QU_TRAN_Status_Log")]
    public partial class QuTranStatusLog
    {
        public int Id { get; set; }
        public int QuotationId { get; set; }
        public int? BookingId { get; set; }
        public int? AuditId { get; set; }
        public int StatusId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime StatusChangeDate { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("QuTranStatusLogs")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("QuTranStatusLogs")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("QuotationId")]
        [InverseProperty("QuTranStatusLogs")]
        public virtual QuQuotation Quotation { get; set; }
        [ForeignKey("StatusId")]
        [InverseProperty("QuTranStatusLogs")]
        public virtual QuStatus Status { get; set; }
    }
}