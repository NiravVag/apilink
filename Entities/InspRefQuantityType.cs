using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_REF_QuantityType")]
    public partial class InspRefQuantityType
    {
        public InspRefQuantityType()
        {
            CuPrDetails = new HashSet<CuPrDetail>();
            QuQuotationInsps = new HashSet<QuQuotationInsp>();
        }

        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public bool? Active { get; set; }
        public int? Sort { get; set; }

        [InverseProperty("BilledQuantityTypeNavigation")]
        public virtual ICollection<CuPrDetail> CuPrDetails { get; set; }
        [InverseProperty("BilledQtyTypeNavigation")]
        public virtual ICollection<QuQuotationInsp> QuQuotationInsps { get; set; }
    }
}