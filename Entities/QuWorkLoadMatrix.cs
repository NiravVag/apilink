using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("QU_WorkLoadMatrix")]
    public partial class QuWorkLoadMatrix
    {
        public int Id { get; set; }
        public int? ProductSubCategory3Id { get; set; }
        public double? PreparationTime { get; set; }
        [Column("SampleSize_8h")]
        public int? SampleSize8h { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("QuWorkLoadMatrixCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("QuWorkLoadMatrixDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("QuWorkLoadMatrices")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("ProductSubCategory3Id")]
        [InverseProperty("QuWorkLoadMatrices")]
        public virtual RefProductCategorySub3 ProductSubCategory3 { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("QuWorkLoadMatrixUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}