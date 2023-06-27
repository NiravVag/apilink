using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("MID_Email_Recipients_ServiceType")]
    public partial class MidEmailRecipientsServiceType
    {
        public int Id { get; set; }
        public int EmailConfigId { get; set; }
        public int? ServiceTypeId { get; set; }
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
        [InverseProperty("MidEmailRecipientsServiceTypeCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("MidEmailRecipientsServiceTypeDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EmailConfigId")]
        [InverseProperty("MidEmailRecipientsServiceTypes")]
        public virtual MidEmailRecipientsConfiguration EmailConfig { get; set; }
        [ForeignKey("ModifiedBy")]
        [InverseProperty("MidEmailRecipientsServiceTypeModifiedByNavigations")]
        public virtual ItUserMaster ModifiedByNavigation { get; set; }
        [ForeignKey("ServiceTypeId")]
        [InverseProperty("MidEmailRecipientsServiceTypes")]
        public virtual RefServiceType ServiceType { get; set; }
    }
}