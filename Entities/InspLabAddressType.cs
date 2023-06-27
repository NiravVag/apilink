using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_LAB_AddressType")]
    public partial class InspLabAddressType
    {
        public InspLabAddressType()
        {
            InspLabAddresses = new HashSet<InspLabAddress>();
        }

        public int Id { get; set; }
        [Required]
        [Column("Address_type")]
        [StringLength(50)]
        public string AddressType { get; set; }
        public int? TranslationId { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("InspLabAddressTypes")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("AddressType")]
        public virtual ICollection<InspLabAddress> InspLabAddresses { get; set; }
    }
}