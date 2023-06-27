using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("SU_Entity")]
    public partial class SuEntity
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public int EntityId { get; set; }
        public bool? Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("SuEntityCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("SuEntityDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("SuEntities")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("SupplierId")]
        [InverseProperty("SuEntities")]
        public virtual SuSupplier Supplier { get; set; }
    }
}