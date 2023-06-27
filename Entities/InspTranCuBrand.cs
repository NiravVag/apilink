using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_TRAN_CU_Brand")]
    public partial class InspTranCuBrand
    {
        public int Id { get; set; }
        [Column("Inspection_Id")]
        public int InspectionId { get; set; }
        [Column("Brand_Id")]
        public int BrandId { get; set; }
        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("BrandId")]
        [InverseProperty("InspTranCuBrands")]
        public virtual CuBrand Brand { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("InspTranCuBrandCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InspTranCuBrandDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("InspectionId")]
        [InverseProperty("InspTranCuBrands")]
        public virtual InspTransaction Inspection { get; set; }
    }
}