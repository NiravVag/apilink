using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("MID_TaskType")]
    public partial class MidTaskType
    {
        public MidTaskType()
        {
            MidTasks = new HashSet<MidTask>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Label { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("MidTaskTypes")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("TaskType")]
        public virtual ICollection<MidTask> MidTasks { get; set; }
    }
}