using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("Ref_ServiceType_Xero")]
    public partial class RefServiceTypeXero
    {
        public int Id { get; set; }
        [StringLength(500)]
        public string TrackingOptionName { get; set; }
        [Column("XERO_Account")]
        [StringLength(500)]
        public string XeroAccount { get; set; }
        [Column("Inspection_Type_consolidate")]
        [StringLength(500)]
        public string InspectionTypeConsolidate { get; set; }
        [Column("Inspection_ServiceType_Id")]
        public int? InspectionServiceTypeId { get; set; }
        [Column("Inspection_Type")]
        [StringLength(500)]
        public string InspectionType { get; set; }
        [Column("Entity_Id")]
        public int? EntityId { get; set; }
        public bool? Active { get; set; }
        [Column("TrackingOptionName_Travel")]
        [StringLength(500)]
        public string TrackingOptionNameTravel { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("RefServiceTypeXeros")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("InspectionServiceTypeId")]
        [InverseProperty("RefServiceTypeXeros")]
        public virtual RefServiceType InspectionServiceType { get; set; }
    }
}