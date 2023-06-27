using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_REF_Request_Type")]
    public partial class InvRefRequestType
    {
        public InvRefRequestType()
        {
            CuPrDetails = new HashSet<CuPrDetail>();
        }

        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public bool? Active { get; set; }

        [InverseProperty("InvoiceRequestTypeNavigation")]
        public virtual ICollection<CuPrDetail> CuPrDetails { get; set; }
    }
}