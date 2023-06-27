using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_LevelPick1")]
    public partial class RefLevelPick1
    {
        public RefLevelPick1()
        {
            CuServiceTypes = new HashSet<CuServiceType>();
            FbReportDetails = new HashSet<FbReportDetail>();
            InspProductTransactions = new HashSet<InspProductTransaction>();
        }

        public int Id { get; set; }
        [StringLength(20)]
        public string Value { get; set; }
        public bool Active { get; set; }
        [Column("FBValue")]
        [StringLength(100)]
        public string Fbvalue { get; set; }

        [InverseProperty("LevelPick1Navigation")]
        public virtual ICollection<CuServiceType> CuServiceTypes { get; set; }
        [InverseProperty("AqlLevelNavigation")]
        public virtual ICollection<FbReportDetail> FbReportDetails { get; set; }
        [InverseProperty("AqlNavigation")]
        public virtual ICollection<InspProductTransaction> InspProductTransactions { get; set; }
    }
}