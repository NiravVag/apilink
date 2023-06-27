using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_TRAN_CS")]
    public partial class InspTranC
    {
        public int Id { get; set; }
        public int? InspectionId { get; set; }
        public int? CsId { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("InspTranCCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CsId")]
        [InverseProperty("InspTranCCs")]
        public virtual ItUserMaster Cs { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InspTranCDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("InspectionId")]
        [InverseProperty("InspTranCS")]
        public virtual InspTransaction Inspection { get; set; }
    }
}