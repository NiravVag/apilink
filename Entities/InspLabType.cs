using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_LAB_Type")]
    public partial class InspLabType
    {
        public InspLabType()
        {
            InspLabDetails = new HashSet<InspLabDetail>();
        }

        public int Id { get; set; }
        [StringLength(200)]
        public string Type { get; set; }
        public int? TypeTransId { get; set; }
        public int? EntityId { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("InspLabTypes")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("Type")]
        public virtual ICollection<InspLabDetail> InspLabDetails { get; set; }
    }
}