using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_ServiceType")]
    public partial class CuServiceType
    {
        public CuServiceType()
        {
            CuCheckPointsServiceTypes = new HashSet<CuCheckPointsServiceType>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int ServiceId { get; set; }
        public int ServiceTypeId { get; set; }
        public bool Active { get; set; }
        public int? PickType { get; set; }
        public int? LevelPick1 { get; set; }
        public int? LevelPick2 { get; set; }
        public int? CriticalPick1 { get; set; }
        public int? CriticalPick2 { get; set; }
        public int? MajorTolerancePick1 { get; set; }
        public int? MajorTolerancePick2 { get; set; }
        public int? MinorTolerancePick1 { get; set; }
        public int? MinorTolerancePick2 { get; set; }
        [Column("AllowAQLModification")]
        public bool? AllowAqlmodification { get; set; }
        public int? DefectClassification { get; set; }
        public bool? CheckMeasurementPoints { get; set; }
        public int? ReportUnit { get; set; }
        public int? ProductCategoryId { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        [StringLength(1500)]
        public string CustomServiceTypeName { get; set; }
        public int? EntityId { get; set; }
        public double? CustomerRequirementIndex { get; set; }
        [Column("DP_Point")]
        public int? DpPoint { get; set; }
        public bool? IgnoreAcceptanceLevel { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("CuServiceTypeCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CriticalPick1")]
        [InverseProperty("CuServiceTypeCriticalPick1Navigations")]
        public virtual RefPick1 CriticalPick1Navigation { get; set; }
        [ForeignKey("CriticalPick2")]
        [InverseProperty("CuServiceTypeCriticalPick2Navigations")]
        public virtual RefPick2 CriticalPick2Navigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("CuServiceTypes")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("DefectClassification")]
        [InverseProperty("CuServiceTypes")]
        public virtual RefDefectClassification DefectClassificationNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuServiceTypeDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("DpPoint")]
        [InverseProperty("CuServiceTypes")]
        public virtual InspRefDpPoint DpPointNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("CuServiceTypes")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("LevelPick1")]
        [InverseProperty("CuServiceTypes")]
        public virtual RefLevelPick1 LevelPick1Navigation { get; set; }
        [ForeignKey("LevelPick2")]
        [InverseProperty("CuServiceTypes")]
        public virtual RefLevelPick2 LevelPick2Navigation { get; set; }
        [ForeignKey("MajorTolerancePick1")]
        [InverseProperty("CuServiceTypeMajorTolerancePick1Navigations")]
        public virtual RefPick1 MajorTolerancePick1Navigation { get; set; }
        [ForeignKey("MajorTolerancePick2")]
        [InverseProperty("CuServiceTypeMajorTolerancePick2Navigations")]
        public virtual RefPick2 MajorTolerancePick2Navigation { get; set; }
        [ForeignKey("MinorTolerancePick1")]
        [InverseProperty("CuServiceTypeMinorTolerancePick1Navigations")]
        public virtual RefPick1 MinorTolerancePick1Navigation { get; set; }
        [ForeignKey("MinorTolerancePick2")]
        [InverseProperty("CuServiceTypeMinorTolerancePick2Navigations")]
        public virtual RefPick2 MinorTolerancePick2Navigation { get; set; }
        [ForeignKey("PickType")]
        [InverseProperty("CuServiceTypes")]
        public virtual RefPickType PickTypeNavigation { get; set; }
        [ForeignKey("ProductCategoryId")]
        [InverseProperty("CuServiceTypes")]
        public virtual RefProductCategory ProductCategory { get; set; }
        [ForeignKey("ReportUnit")]
        [InverseProperty("CuServiceTypes")]
        public virtual RefReportUnit ReportUnitNavigation { get; set; }
        [ForeignKey("ServiceId")]
        [InverseProperty("CuServiceTypes")]
        public virtual RefService Service { get; set; }
        [ForeignKey("ServiceTypeId")]
        [InverseProperty("CuServiceTypes")]
        public virtual RefServiceType ServiceType { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("CuServiceTypeUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
        [InverseProperty("ServiceType")]
        public virtual ICollection<CuCheckPointsServiceType> CuCheckPointsServiceTypes { get; set; }
    }
}