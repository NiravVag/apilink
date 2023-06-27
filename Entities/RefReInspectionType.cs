using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_ReInspectionType")]
    public partial class RefReInspectionType
    {
        public RefReInspectionType()
        {
            InspTransactions = new HashSet<InspTransaction>();
        }

        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public bool? Active { get; set; }

        [InverseProperty("ReInspectionTypeNavigation")]
        public virtual ICollection<InspTransaction> InspTransactions { get; set; }
    }
}