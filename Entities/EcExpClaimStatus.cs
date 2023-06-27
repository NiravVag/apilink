using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("EC_ExpClaimStatus")]
    public partial class EcExpClaimStatus
    {
        public EcExpClaimStatus()
        {
            EcExpencesClaims = new HashSet<EcExpencesClaim>();
            EcStatusRoles = new HashSet<EcStatusRole>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Description { get; set; }
        public int? TranId { get; set; }
        [Required]
        public bool? Active { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("EcExpClaimStatuses")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("Status")]
        public virtual ICollection<EcExpencesClaim> EcExpencesClaims { get; set; }
        [InverseProperty("IdStatusNavigation")]
        public virtual ICollection<EcStatusRole> EcStatusRoles { get; set; }
    }
}