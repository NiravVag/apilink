using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("HR_Staff_MarketSegment")]
    public partial class HrStaffMarketSegment
    {
        [Column("staff_id")]
        public int StaffId { get; set; }
        public int MarketSegmentId { get; set; }

        [ForeignKey("MarketSegmentId")]
        [InverseProperty("HrStaffMarketSegments")]
        public virtual RefMarketSegment MarketSegment { get; set; }
        [ForeignKey("StaffId")]
        [InverseProperty("HrStaffMarketSegments")]
        public virtual HrStaff Staff { get; set; }
    }
}