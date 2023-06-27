using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_TRAN_ShipmentType")]
    public partial class InspTranShipmentType
    {
        public int Id { get; set; }
        public int? ShipmentTypeId { get; set; }
        public int? InspectionId { get; set; }
        public bool? Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("InspTranShipmentTypeCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InspTranShipmentTypeDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("InspectionId")]
        [InverseProperty("InspTranShipmentTypes")]
        public virtual InspTransaction Inspection { get; set; }
        [ForeignKey("ShipmentTypeId")]
        [InverseProperty("InspTranShipmentTypes")]
        public virtual InspRefShipmentType ShipmentType { get; set; }
    }
}