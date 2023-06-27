using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("QU_Quotation_FactoryContact")]
    public partial class QuQuotationFactoryContact
    {
        public int IdQuotation { get; set; }
        public int IdContact { get; set; }
        public bool Quotation { get; set; }
        public bool Email { get; set; }
        public bool InvoiceEmail { get; set; }

        [ForeignKey("IdContact")]
        [InverseProperty("QuQuotationFactoryContacts")]
        public virtual SuContact IdContactNavigation { get; set; }
        [ForeignKey("IdQuotation")]
        [InverseProperty("QuQuotationFactoryContacts")]
        public virtual QuQuotation IdQuotationNavigation { get; set; }
    }
}