using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_Product_MSChart")]
    public partial class CuProductMschart
    {
        public int Id { get; set; }
        [Column("Product_Id")]
        public int ProductId { get; set; }
        [Column("Product_File_Id")]
        public int? ProductFileId { get; set; }
        [StringLength(1000)]
        public string Code { get; set; }
        [StringLength(2000)]
        public string Description { get; set; }
        [Column("MPCode")]
        [StringLength(500)]
        public string Mpcode { get; set; }
        public double? Required { get; set; }
        [Column("Tolerance1_Up")]
        public double? Tolerance1Up { get; set; }
        [Column("Tolerance1_Down")]
        public double? Tolerance1Down { get; set; }
        [Column("Tolerance2_Up")]
        public double? Tolerance2Up { get; set; }
        [Column("Tolerance2_Down")]
        public double? Tolerance2Down { get; set; }
        public int? Sort { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("CuProductMschartCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("ProductId")]
        [InverseProperty("CuProductMscharts")]
        public virtual CuProduct Product { get; set; }
        [ForeignKey("ProductFileId")]
        [InverseProperty("CuProductMscharts")]
        public virtual CuProductFileAttachment ProductFile { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("CuProductMschartUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}