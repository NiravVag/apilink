using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("QU_Quotation_PO")]
    public partial class QuQuotationPo
    {
        public int IdQuotation { get; set; }
        public int PoId { get; set; }
        public int SampleQty { get; set; }
        [StringLength(600)]
        public string AqlLevelDesc { get; set; }

        [ForeignKey("IdQuotation")]
        [InverseProperty("QuQuotationPos")]
        public virtual QuQuotation IdQuotationNavigation { get; set; }
        [ForeignKey("PoId")]
        [InverseProperty("QuQuotationPos")]
        public virtual InspPoTransaction Po { get; set; }
    }
}