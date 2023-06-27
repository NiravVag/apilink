using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_Season")]
    public partial class RefSeason
    {
        public RefSeason()
        {
            AudTransactions = new HashSet<AudTransaction>();
            CuSeasonConfigs = new HashSet<CuSeasonConfig>();
            CuSeasons = new HashSet<CuSeason>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        [Required]
        [StringLength(200)]
        public string Code { get; set; }
        public bool? Default { get; set; }
        public bool Active { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("RefSeasons")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("Season")]
        public virtual ICollection<AudTransaction> AudTransactions { get; set; }
        [InverseProperty("Season")]
        public virtual ICollection<CuSeasonConfig> CuSeasonConfigs { get; set; }
        [InverseProperty("Season")]
        public virtual ICollection<CuSeason> CuSeasons { get; set; }
    }
}