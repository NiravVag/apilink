using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_PR_Province")]
    public partial class CuPrProvince
    {
        public int Id { get; set; }
        [Column("CU_PR_Id")]
        public int CuPrId { get; set; }
        public int FactoryProvinceId { get; set; }
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
        [InverseProperty("CuPrProvinceCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CuPrId")]
        [InverseProperty("CuPrProvinces")]
        public virtual CuPrDetail CuPr { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("CuPrProvinceDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("FactoryProvinceId")]
        [InverseProperty("CuPrProvinces")]
        public virtual RefProvince FactoryProvince { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("CuPrProvinceUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}