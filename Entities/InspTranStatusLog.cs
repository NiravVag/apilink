using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_TRAN_Status_Log")]
    public partial class InspTranStatusLog
    {
        public int Id { get; set; }
        [Column("Booking_Id")]
        public int BookingId { get; set; }
        [Column("Status_Id")]
        public int StatusId { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? StatusChangeDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ServiceDateFrom { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ServiceDateTo { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("BookingId")]
        [InverseProperty("InspTranStatusLogs")]
        public virtual InspTransaction Booking { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("InspTranStatusLogs")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("InspTranStatusLogs")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("StatusId")]
        [InverseProperty("InspTranStatusLogs")]
        public virtual InspStatus Status { get; set; }
    }
}