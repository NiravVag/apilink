using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("HR_REF_Social_Insurance_type")]
    public partial class HrRefSocialInsuranceType
    {
        public HrRefSocialInsuranceType()
        {
            HrStaffs = new HashSet<HrStaff>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public bool Active { get; set; }
        public int? Sort { get; set; }

        [InverseProperty("SocialInsuranceType")]
        public virtual ICollection<HrStaff> HrStaffs { get; set; }
    }
}