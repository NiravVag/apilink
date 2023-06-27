using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("JOB_Schedule_Configuration")]
    public partial class JobScheduleConfiguration
    {
        public int Id { get; set; }
        [StringLength(1000)]
        public string Name { get; set; }
        public int? Type { get; set; }
        [StringLength(1500)]
        public string To { get; set; }
        [Column("CC")]
        [StringLength(1500)]
        public string Cc { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? StartDate { get; set; }
        public int? ScheduleInterval { get; set; }
        [StringLength(1500)]
        public string FolderPath { get; set; }
        [StringLength(1500)]
        public string FileName { get; set; }
        public int? EntityId { get; set; }
        public bool? Active { get; set; }
        [StringLength(1500)]
        public string CustomerId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("JobScheduleConfigurations")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("Type")]
        [InverseProperty("JobScheduleConfigurations")]
        public virtual JobScheduleJobType TypeNavigation { get; set; }
    }
}