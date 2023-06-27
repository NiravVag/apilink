using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("MID_EmailTypes")]
    public partial class MidEmailType
    {
        public MidEmailType()
        {
            MidEmailRecipientsConfigurations = new HashSet<MidEmailRecipientsConfiguration>();
            MidEmailRecipientsCusContactDefaults = new HashSet<MidEmailRecipientsCusContactDefault>();
            MidEmailRecipientsInternalDefaults = new HashSet<MidEmailRecipientsInternalDefault>();
        }

        public int Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        public int? ModuleId { get; set; }
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
        [InverseProperty("MidEmailTypeCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("MidEmailTypeDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("ModifiedBy")]
        [InverseProperty("MidEmailTypeModifiedByNavigations")]
        public virtual ItUserMaster ModifiedByNavigation { get; set; }
        [ForeignKey("ModuleId")]
        [InverseProperty("MidEmailTypes")]
        public virtual MidEmailModule Module { get; set; }
        [InverseProperty("EmailType")]
        public virtual ICollection<MidEmailRecipientsConfiguration> MidEmailRecipientsConfigurations { get; set; }
        [InverseProperty("EmailType")]
        public virtual ICollection<MidEmailRecipientsCusContactDefault> MidEmailRecipientsCusContactDefaults { get; set; }
        [InverseProperty("EmailType")]
        public virtual ICollection<MidEmailRecipientsInternalDefault> MidEmailRecipientsInternalDefaults { get; set; }
    }
}