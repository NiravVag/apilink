using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("QU_Quotation_Contact")]
    public partial class QuQuotationContact
    {
        public int IdQuotation { get; set; }
        public int IdContact { get; set; }
        public bool Quotation { get; set; }
        public bool Email { get; set; }

        [ForeignKey("IdContact")]
        [InverseProperty("QuQuotationContacts")]
        public virtual HrStaff IdContactNavigation { get; set; }
        [ForeignKey("IdQuotation")]
        [InverseProperty("QuQuotationContacts")]
        public virtual QuQuotation IdQuotationNavigation { get; set; }
    }
}