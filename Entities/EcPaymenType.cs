using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("EC_PaymenTypes")]
    public partial class EcPaymenType
    {
        public EcPaymenType()
        {
            EcExpencesClaims = new HashSet<EcExpencesClaim>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Description { get; set; }
        public int? TransId { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("EcPaymenTypes")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("PaymentType")]
        public virtual ICollection<EcExpencesClaim> EcExpencesClaims { get; set; }
    }
}