using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("AUD_BookingContact")]
    public partial class AudBookingContact
    {
        public int Id { get; set; }
        [Column("Factory_Country_Id")]
        public int? FactoryCountryId { get; set; }
        [Column("Office_Id")]
        public int OfficeId { get; set; }
        [Column("Booking_EmailTo")]
        [StringLength(500)]
        public string BookingEmailTo { get; set; }
        [Column("BookingEmailCC")]
        [StringLength(500)]
        public string BookingEmailCc { get; set; }
        [StringLength(500)]
        public string PenaltyEmail { get; set; }
        [StringLength(2500)]
        public string ContactInformation { get; set; }
        public bool Active { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("AudBookingContacts")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("FactoryCountryId")]
        [InverseProperty("AudBookingContacts")]
        public virtual RefCountry FactoryCountry { get; set; }
        [ForeignKey("OfficeId")]
        [InverseProperty("AudBookingContacts")]
        public virtual RefLocation Office { get; set; }
    }
}