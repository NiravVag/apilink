using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("AUD_Type")]
    public partial class AudType
    {
        public AudType()
        {
            AudTransactions = new HashSet<AudTransaction>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public bool Active { get; set; }
        [Column("Entity_Id")]
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("AudTypes")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("AuditType")]
        public virtual ICollection<AudTransaction> AudTransactions { get; set; }
    }
}