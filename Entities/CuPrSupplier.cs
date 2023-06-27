using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_PR_Supplier")]
    public partial class CuPrSupplier
    {
        public int Id { get; set; }
        [Column("CU_PR_Id")]
        public int CuPrId { get; set; }
        public int SupplierId { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        public int? DeletedBy { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("CuPrSupplierCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CuPrId")]
        [InverseProperty("CuPrSuppliers")]
        public virtual CuPrDetail CuPr { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuPrSupplierDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("SupplierId")]
        [InverseProperty("CuPrSuppliers")]
        public virtual SuSupplier Supplier { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("CuPrSupplierUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}