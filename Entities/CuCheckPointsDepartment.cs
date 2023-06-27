using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_CheckPoints_Department")]
    public partial class CuCheckPointsDepartment
    {
        public int Id { get; set; }
        public int CheckpointId { get; set; }
        public int DeptId { get; set; }
        public bool Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CheckpointId")]
        [InverseProperty("CuCheckPointsDepartments")]
        public virtual CuCheckPoint Checkpoint { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("CuCheckPointsDepartmentCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeptId")]
        [InverseProperty("CuCheckPointsDepartments")]
        public virtual CuDepartment Dept { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("CuCheckPointsDepartments")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("CuCheckPointsDepartmentUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}