using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("SU_AddressType")]
    public partial class SuAddressType
    {
        public SuAddressType()
        {
            SuAddresses = new HashSet<SuAddress>();
        }

        public int Id { get; set; }
        [Required]
        [Column("Address_Type")]
        [StringLength(50)]
        public string AddressType { get; set; }
        [Required]
        [Column("Address_Type_Flag")]
        [StringLength(1)]
        public string AddressTypeFlag { get; set; }
        public int? TranslationId { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("SuAddressTypes")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("AddressType")]
        public virtual ICollection<SuAddress> SuAddresses { get; set; }
    }
}