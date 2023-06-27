using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("MID_Email_Recipients_InternalDefault")]
    public partial class MidEmailRecipientsInternalDefault
    {
        public int Id { get; set; }
        public int? EmailTypeId { get; set; }
        [Column("Internal_ContactId")]
        public int? InternalContactId { get; set; }
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
        [InverseProperty("MidEmailRecipientsInternalDefaultCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("MidEmailRecipientsInternalDefaultDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EmailTypeId")]
        [InverseProperty("MidEmailRecipientsInternalDefaults")]
        public virtual MidEmailType EmailType { get; set; }
        [ForeignKey("InternalContactId")]
        [InverseProperty("MidEmailRecipientsInternalDefaults")]
        public virtual HrStaff InternalContact { get; set; }
        [ForeignKey("ModifiedBy")]
        [InverseProperty("MidEmailRecipientsInternalDefaultModifiedByNavigations")]
        public virtual ItUserMaster ModifiedByNavigation { get; set; }
    }
}