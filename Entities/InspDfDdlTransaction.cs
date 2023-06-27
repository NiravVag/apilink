using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_DF_DDL_Transaction")]
    public partial class InspDfDdlTransaction
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int ControlConfigurationId { get; set; }
        [StringLength(100)]
        public string Value { get; set; }
        public bool Active { get; set; }
        public int CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("BookingId")]
        [InverseProperty("InspDfDdlTransactions")]
        public virtual InspTransaction Booking { get; set; }
        [ForeignKey("ControlConfigurationId")]
        [InverseProperty("InspDfDdlTransactions")]
        public virtual DfCuConfiguration ControlConfiguration { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("InspDfDdlTransactionCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InspDfDdlTransactionDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("InspDfDdlTransactionUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}