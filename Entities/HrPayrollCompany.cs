using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("HR_PayrollCompany")]
    public partial class HrPayrollCompany
    {
        public HrPayrollCompany()
        {
            HrStaffs = new HashSet<HrStaff>();
        }

        public int Id { get; set; }
        [StringLength(200)]
        public string CompanyName { get; set; }
        public bool Active { get; set; }
        public int? Sort { get; set; }
        [StringLength(1000)]
        public string AccountEmail { get; set; }
        public int Entity { get; set; }

        [ForeignKey("Entity")]
        [InverseProperty("HrPayrollCompanies")]
        public virtual ApEntity EntityNavigation { get; set; }
        [InverseProperty("PayrollCompanyNavigation")]
        public virtual ICollection<HrStaff> HrStaffs { get; set; }
    }
}