using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_TRAN_Hold_reason")]
    public partial class InspTranHoldReason
    {
        public int Id { get; set; }
        public int? ReasonType { get; set; }
        public string Comment { get; set; }
        [Column("Inspection_Id")]
        public int? InspectionId { get; set; }
        public bool? Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }

        [ForeignKey("InspectionId")]
        [InverseProperty("InspTranHoldReasons")]
        public virtual InspTransaction Inspection { get; set; }
        [ForeignKey("ReasonType")]
        [InverseProperty("InspTranHoldReasons")]
        public virtual InspRefHoldReason ReasonTypeNavigation { get; set; }
    }
}