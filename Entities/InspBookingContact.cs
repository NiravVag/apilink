using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_BookingContact")]
    public partial class InspBookingContact
    {
        public int Id { get; set; }
        [Column("Factory_Country_Id")]
        public int? FactoryCountryId { get; set; }
        [Column("Office_Id")]
        public int OfficeId { get; set; }
        [Column("Contact_Information")]
        [StringLength(2500)]
        public string ContactInformation { get; set; }
        public bool Active { get; set; }
        public int? EntityId { get; set; }
        public int? UserId { get; set; }
        public bool Default { get; set; }
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
        [InverseProperty("InspBookingContactCreatedByNavigations")]
        public virtual ItUserMaster CreatedByNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("InspBookingContactDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("InspBookingContacts")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("FactoryCountryId")]
        [InverseProperty("InspBookingContacts")]
        public virtual RefCountry FactoryCountry { get; set; }
        [ForeignKey("ModifiedBy")]
        [InverseProperty("InspBookingContactModifiedByNavigations")]
        public virtual ItUserMaster ModifiedByNavigation { get; set; }
        [ForeignKey("OfficeId")]
        [InverseProperty("InspBookingContacts")]
        public virtual RefLocation Office { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("InspBookingContactUsers")]
        public virtual ItUserMaster User { get; set; }
    }
}