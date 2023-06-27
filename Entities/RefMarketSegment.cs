using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_MarketSegment")]
    public partial class RefMarketSegment
    {
        public RefMarketSegment()
        {
            CuCustomers = new HashSet<CuCustomer>();
            HrStaffMarketSegments = new HashSet<HrStaffMarketSegment>();
        }

        public int Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        public bool Active { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("RefMarketSegments")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("MargetSegmentNavigation")]
        public virtual ICollection<CuCustomer> CuCustomers { get; set; }
        [InverseProperty("MarketSegment")]
        public virtual ICollection<HrStaffMarketSegment> HrStaffMarketSegments { get; set; }
    }
}