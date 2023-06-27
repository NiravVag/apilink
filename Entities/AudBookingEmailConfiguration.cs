using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("AUD_BookingEmailConfiguration")]
    public partial class AudBookingEmailConfiguration
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        [Required]
        public string Email { get; set; }
        public int FactoryCountryId { get; set; }
        public int AuditStatusId { get; set; }
        public int? EntityId { get; set; }
        public bool Active { get; set; }

        [ForeignKey("AuditStatusId")]
        [InverseProperty("AudBookingEmailConfigurations")]
        public virtual AudStatus AuditStatus { get; set; }
        [ForeignKey("CustomerId")]
        [InverseProperty("AudBookingEmailConfigurations")]
        public virtual CuCustomer Customer { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("AudBookingEmailConfigurations")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("FactoryCountryId")]
        [InverseProperty("AudBookingEmailConfigurations")]
        public virtual RefCountry FactoryCountry { get; set; }
    }
}