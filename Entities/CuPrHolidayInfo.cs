using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_PR_HolidayInfo")]
    public partial class CuPrHolidayInfo
    {
        public CuPrHolidayInfo()
        {
            CuPrHolidayTypes = new HashSet<CuPrHolidayType>();
        }

        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public bool? Active { get; set; }

        [InverseProperty("HolidayInfo")]
        public virtual ICollection<CuPrHolidayType> CuPrHolidayTypes { get; set; }
    }
}