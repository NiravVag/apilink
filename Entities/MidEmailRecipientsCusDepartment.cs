using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("MID_Email_Recipients_CusDepartment")]
    public partial class MidEmailRecipientsCusDepartment
    {
        public int Id { get; set; }
        public int EmailConfigId { get; set; }
        [Column("Cus_DepartmentId")]
        public int? CusDepartmentId { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("MidEmailRecipientsCusDepartmentCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("CusDepartmentId")]
        [InverseProperty("MidEmailRecipientsCusDepartments")]
        public virtual CuDepartment CusDepartment { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("MidEmailRecipientsCusDepartmentDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EmailConfigId")]
        [InverseProperty("MidEmailRecipientsCusDepartments")]
        public virtual MidEmailRecipientsConfiguration EmailConfig { get; set; }
        [ForeignKey("ModifiedBy")]
        [InverseProperty("MidEmailRecipientsCusDepartmentModifiedByNavigations")]
        public virtual ItUserMaster ModifiedByNavigation { get; set; }
    }
}