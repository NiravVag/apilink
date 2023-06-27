using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INV_REF_PaymentTerms")]
    public partial class InvRefPaymentTerm
    {
        public int Id { get; set; }
        [StringLength(1000)]
        public string Name { get; set; }
        public int? Duration { get; set; }
        public bool? Active { get; set; }
    }
}