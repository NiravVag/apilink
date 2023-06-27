using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_TM_Type")]
    public partial class InvTmType
    {
        public InvTmType()
        {
            CuPrDetails = new HashSet<CuPrDetail>();
            InvAutTranDetails = new HashSet<InvAutTranDetail>();
            InvTmDetails = new HashSet<InvTmDetail>();
        }

        public int Id { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
        public bool? Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }

        [InverseProperty("TravelMatrixType")]
        public virtual ICollection<CuPrDetail> CuPrDetails { get; set; }
        [InverseProperty("TravelMatrixTypeNavigation")]
        public virtual ICollection<InvAutTranDetail> InvAutTranDetails { get; set; }
        [InverseProperty("TravelMatrixType")]
        public virtual ICollection<InvTmDetail> InvTmDetails { get; set; }
    }
}