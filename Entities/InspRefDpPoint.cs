using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_REF_DP_Point")]
    public partial class InspRefDpPoint
    {
        public InspRefDpPoint()
        {
            CuServiceTypes = new HashSet<CuServiceType>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        public bool Active { get; set; }
        public int? Sort { get; set; }

        [InverseProperty("DpPointNavigation")]
        public virtual ICollection<CuServiceType> CuServiceTypes { get; set; }
    }
}