using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_PO_Factory")]
    public partial class CuPoFactory
    {
        public int Id { get; set; }
        public int? PoId { get; set; }
        public int? FactoryId { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("CuPoFactoryCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuPoFactoryDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("FactoryId")]
        [InverseProperty("CuPoFactories")]
        public virtual SuSupplier Factory { get; set; }
        [ForeignKey("PoId")]
        [InverseProperty("CuPoFactories")]
        public virtual CuPurchaseOrder Po { get; set; }
    }
}