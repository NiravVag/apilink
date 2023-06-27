using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_Entity")]
    public partial class CuEntity
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int EntityId { get; set; }
        public bool? Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("CuEntityCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("CuEntities")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuEntityDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("CuEntities")]
        public virtual ApEntity Entity { get; set; }
    }
}