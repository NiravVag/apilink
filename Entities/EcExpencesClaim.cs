using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("EC_ExpencesClaims")]
    public partial class EcExpencesClaim
    {
        public EcExpencesClaim()
        {
            EcExpensesClaimDetais = new HashSet<EcExpensesClaimDetai>();
        }

        public int Id { get; set; }
        public int StaffId { get; set; }
        public int CountryId { get; set; }
        public int PaymentTypeId { get; set; }
        public int LocationId { get; set; }
        public int StatusId { get; set; }
        public int? ApprovedId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ApprovedDate { get; set; }
        public int? CheckedId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CheckedDate { get; set; }
        public int? PaidId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? PaidDate { get; set; }
        [Required]
        [StringLength(100)]
        public string ClaimNo { get; set; }
        [Column("IsINsp")]
        public bool IsInsp { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ClaimDate { get; set; }
        [Required]
        [Column("active")]
        public bool? Active { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }
        [StringLength(4000)]
        public string ExpensePurpose { get; set; }
        public int EntityId { get; set; }
        [StringLength(600)]
        public string Comment { get; set; }
        public int? RejectId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? RejectDate { get; set; }
        public int? CancelId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CancelDate { get; set; }
        public int? ClaimTypeId { get; set; }
        public bool? IsAutoExpense { get; set; }

        [ForeignKey("ApprovedId")]
        [InverseProperty("EcExpencesClaimApproveds")]
        public virtual ItUserMaster Approved { get; set; }
        [ForeignKey("CancelId")]
        [InverseProperty("EcExpencesClaimCancels")]
        public virtual ItUserMaster Cancel { get; set; }
        [ForeignKey("CheckedId")]
        [InverseProperty("EcExpencesClaimCheckeds")]
        public virtual ItUserMaster Checked { get; set; }
        [ForeignKey("ClaimTypeId")]
        [InverseProperty("EcExpencesClaims")]
        public virtual EcExpenseClaimtype ClaimType { get; set; }
        [ForeignKey("CountryId")]
        [InverseProperty("EcExpencesClaims")]
        public virtual RefCountry Country { get; set; }
        [ForeignKey("EntityId")]
        [InverseProperty("EcExpencesClaims")]
        public virtual ApEntity Entity { get; set; }
        [ForeignKey("LocationId")]
        [InverseProperty("EcExpencesClaims")]
        public virtual RefLocation Location { get; set; }
        [ForeignKey("PaidId")]
        [InverseProperty("EcExpencesClaimPaids")]
        public virtual ItUserMaster Paid { get; set; }
        [ForeignKey("PaymentTypeId")]
        [InverseProperty("EcExpencesClaims")]
        public virtual EcPaymenType PaymentType { get; set; }
        [ForeignKey("RejectId")]
        [InverseProperty("EcExpencesClaimRejects")]
        public virtual ItUserMaster Reject { get; set; }
        [ForeignKey("StaffId")]
        [InverseProperty("EcExpencesClaims")]
        public virtual HrStaff Staff { get; set; }
        [ForeignKey("StatusId")]
        [InverseProperty("EcExpencesClaims")]
        public virtual EcExpClaimStatus Status { get; set; }
        [InverseProperty("Expense")]
        public virtual ICollection<EcExpensesClaimDetai> EcExpensesClaimDetais { get; set; }
    }
}