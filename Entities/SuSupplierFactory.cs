using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("SU_Supplier_Factory")]
    public partial class SuSupplierFactory
    {
        [Column("Parent_Id")]
        public int ParentId { get; set; }
        [Column("Supplier_Id")]
        public int SupplierId { get; set; }

        [ForeignKey("ParentId")]
        [InverseProperty("SuSupplierFactoryParents")]
        public virtual SuSupplier Parent { get; set; }
        [ForeignKey("SupplierId")]
        [InverseProperty("SuSupplierFactorySuppliers")]
        public virtual SuSupplier Supplier { get; set; }
    }
}