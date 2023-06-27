using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("EC_ReceiptFile")]
    public partial class EcReceiptFile
    {
        public int Id { get; set; }
        public Guid GuidId { get; set; }
        [Required]
        [StringLength(200)]
        public string FullFileName { get; set; }
        public byte[] File { get; set; }
        public int UserId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UploadDate { get; set; }
        public int ExpenseId { get; set; }

        [ForeignKey("ExpenseId")]
        [InverseProperty("EcReceiptFiles")]
        public virtual EcExpensesClaimDetai Expense { get; set; }
        [ForeignKey("UserId")]
        [InverseProperty("EcReceiptFiles")]
        public virtual ItUserMaster User { get; set; }
    }
}