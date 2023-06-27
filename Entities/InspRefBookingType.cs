using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_REF_BookingType")]
    public partial class InspRefBookingType
    {
        public InspRefBookingType()
        {
            InspTransactions = new HashSet<InspTransaction>();
        }

        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public bool? Active { get; set; }
        public int? Sort { get; set; }

        [InverseProperty("BookingTypeNavigation")]
        public virtual ICollection<InspTransaction> InspTransactions { get; set; }
    }
}