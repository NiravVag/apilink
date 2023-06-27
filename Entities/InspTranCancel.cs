using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_TRAN_Cancel")]
    public partial class InspTranCancel
    {
        public int Id { get; set; }
        public int ReasonTypeId { get; set; }
        public int? TimeTypeId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TravellingExpense { get; set; }
        public int? CurrencyId { get; set; }
        [StringLength(500)]
        public string Comments { get; set; }
        [StringLength(500)]
        public string InternalComments { get; set; }
        [Column("Inspection_Id")]
        public int InspectionId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("InspTranCancelCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CurrencyId")]
        [InverseProperty("InspTranCancels")]
        public virtual RefCurrency Currency { get; set; }
        [ForeignKey("InspectionId")]
        [InverseProperty("InspTranCancels")]
        public virtual InspTransaction Inspection { get; set; }
        [ForeignKey("ModifiedBy")]
        [InverseProperty("InspTranCancelModifiedByNavigations")]
        public virtual ItUserMaster ModifiedByNavigation { get; set; }
        [ForeignKey("ReasonTypeId")]
        [InverseProperty("InspTranCancels")]
        public virtual InspCancelReason ReasonType { get; set; }
    }
}