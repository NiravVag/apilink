using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_ServiceType")]
    public partial class RefServiceType
    {
        public RefServiceType()
        {
            AudCuProductCategories = new HashSet<AudCuProductCategory>();
            AudTranServiceTypes = new HashSet<AudTranServiceType>();
            CuPrServiceTypes = new HashSet<CuPrServiceType>();
            CuServiceTypes = new HashSet<CuServiceType>();
            EsServiceTypeConfigs = new HashSet<EsServiceTypeConfig>();
            InspRepCusDecisionTemplates = new HashSet<InspRepCusDecisionTemplate>();
            InspTranServiceTypes = new HashSet<InspTranServiceType>();
            InvManTransactions = new HashSet<InvManTransaction>();
            RefServiceTypeXeros = new HashSet<RefServiceTypeXero>();
            RepFastTemplateConfigs = new HashSet<RepFastTemplateConfig>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public bool Active { get; set; }
        public int? EntityId { get; set; }
        public bool? IsReInspectedService { get; set; }
        [Column("Fb_ServiceType_Id")]
        public int? FbServiceTypeId { get; set; }
        [StringLength(50)]
        public string Abbreviation { get; set; }
        public int? ServiceId { get; set; }
        public int? BusinessLineId { get; set; }
        public bool? ShowServiceDateTo { get; set; }
        [Column("IsAutoQCExpenseClaim")]
        public bool? IsAutoQcexpenseClaim { get; set; }
        public int? Sort { get; set; }
        public bool? Is100Inspection { get; set; }

        [ForeignKey("BusinessLineId")]
        [InverseProperty("RefServiceTypes")]
        public virtual RefBusinessLine BusinessLine { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("RefServiceTypes")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("ServiceId")]
        [InverseProperty("RefServiceTypes")]
        public virtual RefService Service { get; set; }
        [InverseProperty("ServiceTypeNavigation")]
        public virtual ICollection<AudCuProductCategory> AudCuProductCategories { get; set; }
        [InverseProperty("ServiceType")]
        public virtual ICollection<AudTranServiceType> AudTranServiceTypes { get; set; }
        [InverseProperty("ServiceType")]
        public virtual ICollection<CuPrServiceType> CuPrServiceTypes { get; set; }
        [InverseProperty("ServiceType")]
        public virtual ICollection<CuServiceType> CuServiceTypes { get; set; }
        [InverseProperty("ServiceType")]
        public virtual ICollection<EsServiceTypeConfig> EsServiceTypeConfigs { get; set; }
        [InverseProperty("ServiceType")]
        public virtual ICollection<InspRepCusDecisionTemplate> InspRepCusDecisionTemplates { get; set; }
        [InverseProperty("ServiceType")]
        public virtual ICollection<InspTranServiceType> InspTranServiceTypes { get; set; }
        [InverseProperty("ServiceTypeNavigation")]
        public virtual ICollection<InvManTransaction> InvManTransactions { get; set; }
        [InverseProperty("InspectionServiceType")]
        public virtual ICollection<RefServiceTypeXero> RefServiceTypeXeros { get; set; }
        [InverseProperty("ServiceType")]
        public virtual ICollection<RepFastTemplateConfig> RepFastTemplateConfigs { get; set; }
    }
}