using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("CU_PR_RefComplexType")]
    public partial class CuPrRefComplexType
    {
        public CuPrRefComplexType()
        {
            CuPrDetails = new HashSet<CuPrDetail>();
        }

        public int Id { get; set; }
        [StringLength(300)]
        public string Name { get; set; }
        public bool? Active { get; set; }

        [InverseProperty("PriceComplexTypeNavigation")]
        public virtual ICollection<CuPrDetail> CuPrDetails { get; set; }
    }
}