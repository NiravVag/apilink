using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("INSP_REF_PaymentOption")]
    public partial class InspRefPaymentOption
    {
        public InspRefPaymentOption()
        {
            InspTransactions = new HashSet<InspTransaction>();
        }

        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        public int? CustomerId { get; set; }
        public bool? Active { get; set; }
        public int? Sort { get; set; }

        [ForeignKey("CustomerId")]
        [InverseProperty("InspRefPaymentOptions")]
        public virtual CuCustomer Customer { get; set; }
        [InverseProperty("PaymentOptionsNavigation")]
        public virtual ICollection<InspTransaction> InspTransactions { get; set; }
    }
}