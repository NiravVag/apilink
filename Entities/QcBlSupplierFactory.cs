using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("QC_BL_Supplier_Factory")]
    public partial class QcBlSupplierFactory
    {
        public int Id { get; set; }
        [Column("QCBLId")]
        public int Qcblid { get; set; }
        [Column("Supplier_FactoryId")]
        public int? SupplierFactoryId { get; set; }
        public int? TypeId { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("QcBlSupplierFactories")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("Qcblid")]
        [InverseProperty("QcBlSupplierFactories")]
        public virtual QcBlockList Qcbl { get; set; }
        [ForeignKey("SupplierFactoryId")]
        [InverseProperty("QcBlSupplierFactories")]
        public virtual SuSupplier SupplierFactory { get; set; }
    }
}