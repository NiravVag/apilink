using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("ENT_REF_Features")]
    public partial class EntRefFeature
    {
        public EntRefFeature()
        {
            EntFeatureDetails = new HashSet<EntFeatureDetail>();
        }

        public int Id { get; set; }
        [StringLength(500)]
        public string Name { get; set; }
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

        [ForeignKey("CreatedBy")]
        [InverseProperty("EntRefFeatureCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("EntRefFeatureDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [InverseProperty("Feature")]
        public virtual ICollection<EntFeatureDetail> EntFeatureDetails { get; set; }
    }
}