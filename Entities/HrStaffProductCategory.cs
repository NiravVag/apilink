using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("HR_Staff_ProductCategory")]
    public partial class HrStaffProductCategory
    {
        [Column("staff_id")]
        public int StaffId { get; set; }
        public int ProductCategoryId { get; set; }

        [ForeignKey("ProductCategoryId")]
        [InverseProperty("HrStaffProductCategories")]
        public virtual RefProductCategory ProductCategory { get; set; }
        [ForeignKey("StaffId")]
        [InverseProperty("HrStaffProductCategories")]
        public virtual HrStaff Staff { get; set; }
    }
}