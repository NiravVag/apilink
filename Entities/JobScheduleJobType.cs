using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("JOB_Schedule_Job_Type")]
    public partial class JobScheduleJobType
    {
        public JobScheduleJobType()
        {
            JobScheduleConfigurations = new HashSet<JobScheduleConfiguration>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public bool Active { get; set; }
        public int? Sort { get; set; }

        [InverseProperty("TypeNavigation")]
        public virtual ICollection<JobScheduleConfiguration> JobScheduleConfigurations { get; set; }
    }
}