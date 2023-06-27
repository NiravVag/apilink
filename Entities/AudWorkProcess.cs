using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("AUD_WorkProcess")]
    public partial class AudWorkProcess
    {
        public AudWorkProcess()
        {
            AudTranWorkProcesses = new HashSet<AudTranWorkProcess>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public bool Active { get; set; }
        [Column("Entity_Id")]
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("AudWorkProcesses")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("WorkProcess")]
        public virtual ICollection<AudTranWorkProcess> AudTranWorkProcesses { get; set; }
    }
}