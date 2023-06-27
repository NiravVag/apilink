using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("REF_BUSINESS_LINE")]
    public partial class RefBusinessLine
    {
        public RefBusinessLine()
        {
            InspTransactions = new HashSet<InspTransaction>();
            RefProductCategories = new HashSet<RefProductCategory>();
            RefServiceTypes = new HashSet<RefServiceType>();
        }

        [Column("ID")]
        public int Id { get; set; }
        [StringLength(200)]
        public string BusinessLine { get; set; }
        public bool? Active { get; set; }

        [InverseProperty("BusinessLineNavigation")]
        public virtual ICollection<InspTransaction> InspTransactions { get; set; }
        [InverseProperty("BusinessLine")]
        public virtual ICollection<RefProductCategory> RefProductCategories { get; set; }
        [InverseProperty("BusinessLine")]
        public virtual ICollection<RefServiceType> RefServiceTypes { get; set; }
    }
}