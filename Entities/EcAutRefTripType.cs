using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("EC_AUT_REF_TripType")]
    public partial class EcAutRefTripType
    {
        public EcAutRefTripType()
        {
            EcAutQcTravelExpenses = new HashSet<EcAutQcTravelExpense>();
            EcExpensesClaimDetais = new HashSet<EcExpensesClaimDetai>();
        }

        public int Id { get; set; }
        [StringLength(500)]
        public string Name { get; set; }
        public bool? Active { get; set; }
        public int? Sort { get; set; }

        [InverseProperty("TripTypeNavigation")]
        public virtual ICollection<EcAutQcTravelExpense> EcAutQcTravelExpenses { get; set; }
        [InverseProperty("TripTypeNavigation")]
        public virtual ICollection<EcExpensesClaimDetai> EcExpensesClaimDetais { get; set; }
    }
}