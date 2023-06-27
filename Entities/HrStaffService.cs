using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("HR_Staff_Services")]
    public partial class HrStaffService
    {
        public int Id { get; set; }
        public int? StaffId { get; set; }
        public int? ServiceId { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool? Active { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("HrStaffServiceCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("HrStaffServiceDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("ServiceId")]
        [InverseProperty("HrStaffServices")]
        public virtual RefService Service { get; set; }
        [ForeignKey("StaffId")]
        [InverseProperty("HrStaffServices")]
        public virtual HrStaff Staff { get; set; }
    }
}