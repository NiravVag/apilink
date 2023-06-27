using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_CheckPoints_ServiceType")]
    public partial class CuCheckPointsServiceType
    {
        public int Id { get; set; }
        public int CheckpointId { get; set; }
        public int ServiceTypeId { get; set; }
        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CheckpointId")]
        [InverseProperty("CuCheckPointsServiceTypes")]
        public virtual CuCheckPoint Checkpoint { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("CuCheckPointsServiceTypeCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("CuCheckPointsServiceTypes")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("ServiceTypeId")]
        [InverseProperty("CuCheckPointsServiceTypes")]
        public virtual CuServiceType ServiceType { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("CuCheckPointsServiceTypeUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}