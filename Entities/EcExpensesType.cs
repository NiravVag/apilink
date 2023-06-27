using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("EC_ExpensesTypes")]
    public partial class EcExpensesType
    {
        public EcExpensesType()
        {
            EcExpensesClaimDetais = new HashSet<EcExpensesClaimDetai>();
        }

        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Description { get; set; }
        [Required]
        public bool? Active { get; set; }
        public int? TypeTransId { get; set; }
        public int? EntityId { get; set; }
        [Required]
        public bool? IsTravel { get; set; }
        public bool? IsOutsource { get; set; }
        public bool? IsPermanent { get; set; }
        [Column("Xero_AccountCode")]
        [StringLength(2000)]
        public string XeroAccountCode { get; set; }
        [Column("Xero_OutSource_AccountCode")]
        [StringLength(2000)]
        public string XeroOutSourceAccountCode { get; set; }

        [ForeignKey("EntityId")]
        [InverseProperty("EcExpensesTypes")]
        public virtual ApEntity Entity { get; set; }
        [InverseProperty("ExpenseType")]
        public virtual ICollection<EcExpensesClaimDetai> EcExpensesClaimDetais { get; set; }
    }
}