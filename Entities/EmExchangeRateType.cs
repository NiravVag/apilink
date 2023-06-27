using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("EM_ExchangeRateType")]
    public partial class EmExchangeRateType
    {
        public EmExchangeRateType()
        {
            EmExchangeRates = new HashSet<EmExchangeRate>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Label { get; set; }
        public int? TypeTransId { get; set; }
        [Required]
        public bool? Active { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("EmExchangeRateTypes")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("ExRateType")]
        public virtual ICollection<EmExchangeRate> EmExchangeRates { get; set; }
    }
}