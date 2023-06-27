using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("HR_Staff_OpCountry")]
    public partial class HrStaffOpCountry
    {
        [Column("staff_id")]
        public int StaffId { get; set; }
        [Column("country_id")]
        public int CountryId { get; set; }

        [ForeignKey("CountryId")]
        [InverseProperty("HrStaffOpCountries")]
        public virtual RefCountry Country { get; set; }
        [ForeignKey("StaffId")]
        [InverseProperty("HrStaffOpCountries")]
        public virtual HrStaff Staff { get; set; }
    }
}