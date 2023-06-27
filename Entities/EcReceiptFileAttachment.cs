using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("EC_ReceiptFileAttachment")]
    public partial class EcReceiptFileAttachment
    {
        public int Id { get; set; }
        public int ExpenseId { get; set; }
        [StringLength(1000)]
        public string UniqueId { get; set; }
        [StringLength(500)]
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public int? Createdby { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedOn { get; set; }
        public int? DeletedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? DeletedOn { get; set; }
        public bool Active { get; set; }

        [ForeignKey("Createdby")]
        [InverseProperty("EcReceiptFileAttachmentCreatedbyNavigations")]
        public virtual ItUserMaster CreatedbyNavigation { get; set; }
        [ForeignKey("DeletedBy")]
        [InverseProperty("EcReceiptFileAttachmentDeletedByNavigations")]
        public virtual ItUserMaster DeletedByNavigation { get; set; }
        [ForeignKey("ExpenseId")]
        [InverseProperty("EcReceiptFileAttachments")]
        public virtual EcExpensesClaimDetai Expense { get; set; }
    }
}