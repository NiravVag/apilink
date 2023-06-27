using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("HR_Profile")]
    public partial class HrProfile
    {
        public HrProfile()
        {
            DaUserCustomers = new HashSet<DaUserCustomer>();
            HrStaffProfiles = new HashSet<HrStaffProfile>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string ProfileName { get; set; }
        public bool Active { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("HrProfiles")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("UserTypeNavigation")]
        public virtual ICollection<DaUserCustomer> DaUserCustomers { get; set; }
        [InverseProperty("Profile")]
        public virtual ICollection<HrStaffProfile> HrStaffProfiles { get; set; }
    }
}