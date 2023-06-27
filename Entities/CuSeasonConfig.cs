using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_Season_Config")]
    public partial class CuSeasonConfig
    {
        public CuSeasonConfig()
        {
            InspTransactions = new HashSet<InspTransaction>();
        }

        public int Id { get; set; }
        public int? SeasonId { get; set; }
        public int? CustomerId { get; set; }
        public bool? Active { get; set; }
        public bool? IsDefault { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("CuSeasonConfigs")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("CuSeasonConfigs")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("SeasonId")]
        [InverseProperty("CuSeasonConfigs")]
        public virtual RefSeason Season { get; set; }
        [InverseProperty("Season")]
        public virtual ICollection<InspTransaction> InspTransactions { get; set; }
    }
}