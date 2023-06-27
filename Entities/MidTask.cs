using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("MID_Task")]
    public partial class MidTask
    {
        public Guid Id { get; set; }
        public int TaskTypeId { get; set; }
        public int UserId { get; set; }
        public int ReportTo { get; set; }
        public int LinkId { get; set; }
        public bool IsDone { get; set; }
        public int? EmailState { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("MidTasks")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("TaskTypeId")]
        [InverseProperty("MidTasks")]
        public virtual MidTaskType TaskType { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("MidTasks")]
        public virtual ItUserMaster User { get; set; }
    }
}