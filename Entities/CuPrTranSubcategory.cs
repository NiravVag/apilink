using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_PR_TRAN_Subcategory")]
    public partial class CuPrTranSubcategory
    {
        public int Id { get; set; }
        [Column("Cu_Price_Id")]
        public int? CuPriceId { get; set; }
        public int? MandayProductivity { get; set; }
        public int? MandayReports { get; set; }
        public double? MandayBuffer { get; set; }
        public double? UnitPrice { get; set; }
        [Column("AQL_QTY_8")]
        public double? AqlQty8 { get; set; }
        [Column("AQL_QTY_13")]
        public double? AqlQty13 { get; set; }
        [Column("AQL_QTY_20")]
        public double? AqlQty20 { get; set; }
        [Column("AQL_QTY_32")]
        public double? AqlQty32 { get; set; }
        [Column("AQL_QTY_50")]
        public double? AqlQty50 { get; set; }
        [Column("AQL_QTY_80")]
        public double? AqlQty80 { get; set; }
        [Column("AQL_QTY_125")]
        public double? AqlQty125 { get; set; }
        [Column("AQL_QTY_200")]
        public double? AqlQty200 { get; set; }
        [Column("AQL_QTY_315")]
        public double? AqlQty315 { get; set; }
        [Column("AQL_QTY_500")]
        public double? AqlQty500 { get; set; }
        [Column("AQL_QTY_800")]
        public double? AqlQty800 { get; set; }
        [Column("AQL_QTY_1250")]
        public double? AqlQty1250 { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        [Column("Sub_Category2Id")]
        public int? SubCategory2Id { get; set; }
        [Column("Sub_CategoryId")]
        public int? SubCategoryId { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("CuPrTranSubcategoryCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CuPriceId")]
        [InverseProperty("CuPrTranSubcategories")]
        public virtual CuPrDetail CuPrice { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuPrTranSubcategoryDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("SubCategoryId")]
        [InverseProperty("CuPrTranSubcategories")]
        public virtual RefProductCategorySub SubCategory { get; set; }
        [ForeignKey("SubCategory2Id")]
        [InverseProperty("CuPrTranSubcategories")]
        public virtual RefProductCategorySub2 SubCategory2 { get; set; }
    }
}