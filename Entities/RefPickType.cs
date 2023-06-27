using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_PickType")]
    public partial class RefPickType
    {
        public RefPickType()
        {
            CuServiceTypes = new HashSet<CuServiceType>();
        }

        public int Id { get; set; }
        [StringLength(50)]
        public string Value { get; set; }
        public bool Active { get; set; }

        [InverseProperty("PickTypeNavigation")]
        public virtual ICollection<CuServiceType> CuServiceTypes { get; set; }
    }
}