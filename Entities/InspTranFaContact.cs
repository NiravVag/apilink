using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_TRAN_FA_Contact")]
    public partial class InspTranFaContact
    {
        public int Id { get; set; }
        [Column("Inspection_Id")]
        public int? InspectionId { get; set; }
        [Column("Contact_Id")]
        public int? ContactId { get; set; }
        public bool Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public int? DeletedBy { get; set; }

        [ForeignKey("ContactId")]
        [InverseProperty("InspTranFaContacts")]
        public virtual SuContact Contact { get; set; }
        [ForeignKey("CreatedBy")]
        [InverseProperty("InspTranFaContactCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InspTranFaContactDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("InspectionId")]
        [InverseProperty("InspTranFaContacts")]
        public virtual InspTransaction Inspection { get; set; }
    }
}