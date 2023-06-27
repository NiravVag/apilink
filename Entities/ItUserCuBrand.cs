using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("IT_User_CU_Brand")]
    public partial class ItUserCuBrand
    {
        [Column("User_Id")]
        public int UserId { get; set; }
        [Column("Brand_Id")]
        public int BrandId { get; set; }

        [ForeignKey("BrandId")]
        [InverseProperty("ItUserCuBrands")]
        public virtual CuBrand Brand { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("ItUserCuBrands")]
        public virtual ItUserMaster User { get; set; }
    }
}