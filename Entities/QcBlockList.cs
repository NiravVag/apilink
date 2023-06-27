using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("QC_BlockList")]
    public partial class QcBlockList
    {
        public QcBlockList()
        {
            QcBlCustomers = new HashSet<QcBlCustomer>();
            QcBlProductCatgeories = new HashSet<QcBlProductCatgeory>();
            QcBlProductSubCategories = new HashSet<QcBlProductSubCategory>();
            QcBlProductSubCategory2S = new HashSet<QcBlProductSubCategory2>();
            QcBlSupplierFactories = new HashSet<QcBlSupplierFactory>();
        }

        public int Id { get; set; }
        [Column("QCId")]
        public int Qcid { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("QcBlockListCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("QcBlockListDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("QcBlockLists")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("Qcid")]
        [InverseProperty("QcBlockLists")]
        public virtual HrStaff Qc { get; set; }
        [InverseProperty("Qcbl")]
        public virtual ICollection<QcBlCustomer> QcBlCustomers { get; set; }
        [InverseProperty("Qcbl")]
        public virtual ICollection<QcBlProductCatgeory> QcBlProductCatgeories { get; set; }
        [InverseProperty("Qcbl")]
        public virtual ICollection<QcBlProductSubCategory> QcBlProductSubCategories { get; set; }
        [InverseProperty("Qcbl")]
        public virtual ICollection<QcBlProductSubCategory2> QcBlProductSubCategory2S { get; set; }
        [InverseProperty("Qcbl")]
        public virtual ICollection<QcBlSupplierFactory> QcBlSupplierFactories { get; set; }
    }
}