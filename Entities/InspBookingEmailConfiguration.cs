using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_BookingEmailConfiguration")]
    public partial class InspBookingEmailConfiguration
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        [Required]
        [StringLength(200)]
        public string Email { get; set; }
        public int? FactoryCountryId { get; set; }
        public int BookingStatusId { get; set; }
        public bool Active { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("BookingStatusId")]
        [InverseProperty("InspBookingEmailConfigurations")]
        public virtual InspStatus BookingStatus { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("InspBookingEmailConfigurations")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("InspBookingEmailConfigurations")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("FactoryCountryId")]
        [InverseProperty("InspBookingEmailConfigurations")]
        public virtual RefCountry FactoryCountry { get; set; }
    }
}