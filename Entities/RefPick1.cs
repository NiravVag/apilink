using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_Pick1")]
    public partial class RefPick1
    {
        public RefPick1()
        {
            CuServiceTypeCriticalPick1Navigations = new HashSet<CuServiceType>();
            CuServiceTypeMajorTolerancePick1Navigations = new HashSet<CuServiceType>();
            CuServiceTypeMinorTolerancePick1Navigations = new HashSet<CuServiceType>();
            InspProductTransactionCriticalNavigations = new HashSet<InspProductTransaction>();
            InspProductTransactionMajorNavigations = new HashSet<InspProductTransaction>();
            InspProductTransactionMinorNavigations = new HashSet<InspProductTransaction>();
        }

        public int Id { get; set; }
        public double? Value { get; set; }
        public bool Active { get; set; }

        [InverseProperty("CriticalPick1Navigation")]
        public virtual ICollection<CuServiceType> CuServiceTypeCriticalPick1Navigations { get; set; }
        [InverseProperty("MajorTolerancePick1Navigation")]
        public virtual ICollection<CuServiceType> CuServiceTypeMajorTolerancePick1Navigations { get; set; }
        [InverseProperty("MinorTolerancePick1Navigation")]
        public virtual ICollection<CuServiceType> CuServiceTypeMinorTolerancePick1Navigations { get; set; }
        [InverseProperty("CriticalNavigation")]
        public virtual ICollection<InspProductTransaction> InspProductTransactionCriticalNavigations { get; set; }
        [InverseProperty("MajorNavigation")]
        public virtual ICollection<InspProductTransaction> InspProductTransactionMajorNavigations { get; set; }
        [InverseProperty("MinorNavigation")]
        public virtual ICollection<InspProductTransaction> InspProductTransactionMinorNavigations { get; set; }
    }
}