using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("HR_StaffTraining")]
    public partial class HrStaffTraining
    {
        public int Id { get; set; }
        [Column("Staff_Id")]
        public int? StaffId { get; set; }
        [Column("Training_Topic")]
        [StringLength(200)]
        public string TrainingTopic { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateStart { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DateEnd { get; set; }
        [StringLength(200)]
        public string Trainer { get; set; }
        [StringLength(500)]
        public string Comment { get; set; }

        [ForeignKey("StaffId")]
        [InverseProperty("HrStaffTrainings")]
        public virtual HrStaff Staff { get; set; }
    }
}