using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("AUD_EvaluationRound")]
    public partial class AudEvaluationRound
    {
        public AudEvaluationRound()
        {
            AudTransactions = new HashSet<AudTransaction>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public bool Active { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("AudEvaluationRounds")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("Evalution")]
        public virtual ICollection<AudTransaction> AudTransactions { get; set; }
    }
}