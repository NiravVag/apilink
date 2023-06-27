using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("QU_INSP_Product")]
    public partial class QuInspProduct
    {
        public int IdQuotation { get; set; }
        public int ProductTranId { get; set; }
        public int SampleQty { get; set; }
        [StringLength(600)]
        public string AqlLevelDesc { get; set; }

        [ForeignKey("IdQuotation")]
        [InverseProperty("QuInspProducts")]
        public virtual QuQuotation IdQuotationNavigation { get; set; }
        [ForeignKey("ProductTranId")]
        [InverseProperty("QuInspProducts")]
        public virtual InspProductTransaction ProductTran { get; set; }
    }
}