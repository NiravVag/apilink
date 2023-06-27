using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ENT_Feature_Details")]
    public partial class EntFeatureDetail
    {
        public int Id { get; set; }
        public int? FeatureId { get; set; }
        public int? EntityId { get; set; }
        public int? CountryId { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool? Active { get; set; }

        [ForeignKey("CountryId")]
        [InverseProperty("EntFeatureDetails")]
        public virtual RefCountry Country { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("EntFeatureDetailCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("EntFeatureDetailDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("EntFeatureDetails")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("FeatureId")]
        [InverseProperty("EntFeatureDetails")]
        public virtual EntRefFeature Feature { get; set; }
    }
}