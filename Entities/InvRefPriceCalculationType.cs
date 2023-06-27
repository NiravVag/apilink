using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_REF_PriceCalculationType")]
    public partial class InvRefPriceCalculationType
    {
        public InvRefPriceCalculationType()
        {
            InvAutTranDetails = new HashSet<InvAutTranDetail>();
        }

        public int Id { get; set; }
        [StringLength(250)]
        public string Name { get; set; }
        public bool? Active { get; set; }

        [InverseProperty("PriceCalculationTypeNavigation")]
        public virtual ICollection<InvAutTranDetail> InvAutTranDetails { get; set; }
    }
}