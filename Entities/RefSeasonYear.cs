using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_Season_Year")]
    public partial class RefSeasonYear
    {
        public RefSeasonYear()
        {
            AudTransactions = new HashSet<AudTransaction>();
            InspTransactions = new HashSet<InspTransaction>();
        }

        public int Id { get; set; }
        public int Year { get; set; }
        public bool Active { get; set; }

        [InverseProperty("SeasonYear")]
        public virtual ICollection<AudTransaction> AudTransactions { get; set; }
        [InverseProperty("SeasonYear")]
        public virtual ICollection<InspTransaction> InspTransactions { get; set; }
    }
}