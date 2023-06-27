using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_PO_Supplier")]
    public partial class CuPoSupplier
    {
        public int Id { get; set; }
        public int? PoId { get; set; }
        public int? SupplierId { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("CuPoSupplierCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuPoSupplierDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("PoId")]
        [InverseProperty("CuPoSuppliers")]
        public virtual CuPurchaseOrder Po { get; set; }
        [ForeignKey("SupplierId")]
        [InverseProperty("CuPoSuppliers")]
        public virtual SuSupplier Supplier { get; set; }
    }
}