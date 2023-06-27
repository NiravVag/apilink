using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_TRAN_ServiceType")]
    public partial class InspTranServiceType
    {
        public int Id { get; set; }
        [Column("Inspection_Id")]
        public int InspectionId { get; set; }
        [Column("ServiceType_Id")]
        public int ServiceTypeId { get; set; }
        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("InspTranServiceTypeCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InspTranServiceTypeDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("InspectionId")]
        [InverseProperty("InspTranServiceTypes")]
        public virtual InspTransaction Inspection { get; set; }
        [ForeignKey("ServiceTypeId")]
        [InverseProperty("InspTranServiceTypes")]
        public virtual RefServiceType ServiceType { get; set; }
    }
}