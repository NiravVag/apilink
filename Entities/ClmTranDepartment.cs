using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CLM_TRAN_Department")]
    public partial class ClmTranDepartment
    {
        public int Id { get; set; }
        public int? Claimid { get; set; }
        public int? DepartmentId { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? UpdatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("Claimid")]
        [InverseProperty("ClmTranDepartments")]
        public virtual ClmTransaction Claim { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("ClmTranDepartmentCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("ClmTranDepartmentDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("DepartmentId")]
        [InverseProperty("ClmTranDepartments")]
        public virtual ClmRefDepartment Department { get; set; }
        [ForeignKey("UpdatedBy")]
        [InverseProperty("ClmTranDepartmentUpdatedByNavigations")]
        public virtual ItUserMaster UpdatedByNavigation { get; set; }
    }
}