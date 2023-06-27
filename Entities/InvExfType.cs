using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_EXF_Type")]
    public partial class InvExfType
    {
        public InvExfType()
        {
            InvExfTranDetails = new HashSet<InvExfTranDetail>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? Active { get; set; }
        public bool? Sort { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedOn { get; set; }

        [InverseProperty("ExtraFeeTypeNavigation")]
        public virtual ICollection<InvExfTranDetail> InvExfTranDetails { get; set; }
    }
}