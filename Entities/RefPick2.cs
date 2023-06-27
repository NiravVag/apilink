using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_Pick2")]
    public partial class RefPick2
    {
        public RefPick2()
        {
            CuServiceTypeCriticalPick2Navigations = new HashSet<CuServiceType>();
            CuServiceTypeMajorTolerancePick2Navigations = new HashSet<CuServiceType>();
            CuServiceTypeMinorTolerancePick2Navigations = new HashSet<CuServiceType>();
        }

        public int Id { get; set; }
        public double Value { get; set; }
        public bool Active { get; set; }

        [InverseProperty("CriticalPick2Navigation")]
        public virtual ICollection<CuServiceType> CuServiceTypeCriticalPick2Navigations { get; set; }
        [InverseProperty("MajorTolerancePick2Navigation")]
        public virtual ICollection<CuServiceType> CuServiceTypeMajorTolerancePick2Navigations { get; set; }
        [InverseProperty("MinorTolerancePick2Navigation")]
        public virtual ICollection<CuServiceType> CuServiceTypeMinorTolerancePick2Navigations { get; set; }
    }
}