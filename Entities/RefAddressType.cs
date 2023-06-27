using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_AddressType")]
    public partial class RefAddressType
    {
        public RefAddressType()
        {
            CuAddresses = new HashSet<CuAddress>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("RefAddressTypes")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("AddressTypeNavigation")]
        public virtual ICollection<CuAddress> CuAddresses { get; set; }
    }
}