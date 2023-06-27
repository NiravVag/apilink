using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_PO_Combine_Transaction")]
    public partial class InspPoCombineTransaction
    {
        public int Id { get; set; }
        [Column("Inspection_Id")]
        public int InspectionId { get; set; }
        [Column("Product_Id")]
        public int ProductId { get; set; }
        [Column("Po_Id")]
        public int? PoId { get; set; }
        [Column("Combine_Product_Id")]
        public int? CombineProductId { get; set; }
        [Column("AQL_Qty")]
        public int? AqlQty { get; set; }
        [Column("Combine_Qty")]
        public int? CombineQty { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public bool? Active { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("InspPoCombineTransactionCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("InspectionId")]
        [InverseProperty("InspPoCombineTransactions")]
        public virtual InspTransaction Inspection { get; set; }
        [ForeignKey("ModifiedBy")]
        [InverseProperty("InspPoCombineTransactionModifiedByNavigations")]
        public virtual ItUserMaster ModifiedByNavigation { get; set; }
        [ForeignKey("PoId")]
        [InverseProperty("InspPoCombineTransactions")]
        public virtual CuPurchaseOrder Po { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("InspPoCombineTransactions")]
        public virtual CuProduct Product { get; set; }
    }
}