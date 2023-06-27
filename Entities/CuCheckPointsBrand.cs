using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_CheckPoints_Brand")]
    public partial class CuCheckPointsBrand
    {
        public int Id { get; set; }
        public int CheckpointId { get; set; }
        public int BrandId { get; set; }
        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("BrandId")]
        [InverseProperty("CuCheckPointsBrands")]
        public virtual CuBrand Brand { get; set; }
        [ForeignKey("CheckpointId")]
        [InverseProperty("CuCheckPointsBrands")]
        public virtual CuCheckPoint Checkpoint { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("CuCheckPointsBrandCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("CuCheckPointsBrands")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("CuCheckPointsBrandUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}