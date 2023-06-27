using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("HR_StaffPhoto")]
    public partial class HrStaffPhoto
    {
        public Guid GuidId { get; set; }
        public byte[] Photo { get; set; }
        public int StaffId { get; set; }
        [Column("Photo_mType")]
        [StringLength(100)]
        public string PhotoMType { get; set; }

        [ForeignKey("StaffId")]
        [InverseProperty("HrStaffPhotos")]
        public virtual HrStaff Staff { get; set; }
    }
}