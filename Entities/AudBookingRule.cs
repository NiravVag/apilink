using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("AUD_BookingRules")]
    public partial class AudBookingRule
    {
        public int Id { get; set; }
        [Column("Customer_id")]
        public int? CustomerId { get; set; }
        public int LeadDays { get; set; }
        [Column("Factory_CountryId")]
        public int? FactoryCountryId { get; set; }
        public bool IsDefault { get; set; }
        public bool Active { get; set; }
        [Column("Booking_Rule")]
        [StringLength(3000)]
        public string BookingRule { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("AudBookingRules")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("AudBookingRules")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("FactoryCountryId")]
        [InverseProperty("AudBookingRules")]
        public virtual RefCountry FactoryCountry { get; set; }
    }
}