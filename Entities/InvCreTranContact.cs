using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_CRE_TRAN_Contacts")]
    public partial class InvCreTranContact
    {
        public int Id { get; set; }
        public int? Credited { get; set; }
        public int? CustomerContactId { get; set; }
        public bool? Active { get; set; }
        public int? CreatedBy { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }

        [ForeignKey("CreatedBy")]
        [InverseProperty("InvCreTranContactCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("Credited")]
        [InverseProperty("InvCreTranContacts")]
        public virtual InvCreTransaction CreditedNavigation { get; set; }
        [ForeignKey("CustomerContactId")]
        [InverseProperty("InvCreTranContacts")]
        public virtual CuContact CustomerContact { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InvCreTranContactDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
    }
}